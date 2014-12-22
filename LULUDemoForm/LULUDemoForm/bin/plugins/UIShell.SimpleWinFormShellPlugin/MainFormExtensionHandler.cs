using System;
using System.Collections.Generic;
using System.Text;
using UIShell.OSGi;
using UIShell.SimpleWinFormShellPlugin.ExtensionModel;
using System.Windows.Forms;
using System.Drawing;
using UIShell.OSGi.Utility;
using System.IO;

namespace UIShell.SimpleWinFormShellPlugin
{
    partial class MainForm
    {
        // 扩展点
        const string SimpleWinFormShellExtensionPoint = "UIShell.Applications";
        // Application容器、当前插件上下文、当前插件
        WinShellApplicationContainer ApplicationContainer { set; get; }
        IBundleContext BundleContext { set; get; }
        IBundle Bundle { set; get; }
        // WinShellApplication和顶层菜单节点对应表
        Dictionary<WinShellApplication, ToolStripMenuItem> ApplicationMenuStripMap { set; get; }

        /// <summary>
        /// 初始化变量。
        /// </summary>
        private void Initialize()
        {
            ApplicationContainer = WinShellApplicationContainer.Instance;
            BundleContext = Activator.Context;
            Bundle = BundleContext.Bundle;
            ApplicationMenuStripMap = new Dictionary<WinShellApplication, ToolStripMenuItem>();
        }

        /// <summary>
        /// 处理扩展信息。
        /// </summary>
        private void HanldeExtension()
        {
            Initialize();

            // 1 获取所有扩展信息
            var extensions = BundleContext.GetExtensions(SimpleWinFormShellExtensionPoint);
            WinShellApplication application;

            foreach (var extension in extensions)
            {
                // 2 将扩展Extension对象转换成WinShellApplication对象
                application = ApplicationContainer.AddApplicationForExtension(extension);
                // 3 为WinShellApplication对象创建顶层菜单和子菜单
                CreateMenuStripForApplication(application);
            }
            // 4 监听扩展变更事件
            BundleContext.ExtensionChanged += BundleContextExtensionChanged;
        }

        private void BundleContextExtensionChanged(object sender, ExtensionEventArgs e)
        {
            if (e.ExtensionPoint.Equals(SimpleWinFormShellExtensionPoint))
            {
                if (e.Action == CollectionChangedAction.Add)
                {
                    // 新增扩展信息，说明有插件被启动，为其创建界面菜单
                    var app = ApplicationContainer.AddApplicationForExtension(e.Extension);
                    CreateMenuStripForApplication(app);
                }
                else if (e.Action == CollectionChangedAction.Remove)
                {
                    // 扩展信息被删除，有插件被停止，删除对应的界面菜单
                    var app = ApplicationContainer.RemoveApplicationForExtension(e.Extension);
                    RemoveMenuStripForApplication(app);
                }
            }
        }

        /// <summary>
        /// 扩展变更事件是一个异步事件，即是一个新的线程来处理事件。
        /// 因此，需要将扩展变更事件对界面的变更通过代理发送到界面
        /// 线程，由其来更新界面。
        /// </summary>
        /// <param name="application">应用对象。</param>
        private delegate void CreateMenuStripForApplicationDelegate(
            WinShellApplication application);

        private void CreateMenuStripForApplication(WinShellApplication application)
        {
            CreateMenuStripForApplicationDelegate del = (WinShellApplication app) =>
            {
                // 为WinShellApplication创建根菜单节点
                var topToolStripItem = TopMenuStrip.Items.Add(app.Title,
                    LoadImage(app.Bundle, app.Icon)) as ToolStripMenuItem;

                topToolStripItem.ToolTipText = app.ToolTip;

                // 创建子菜单节点
                CreateChildMenu(app.Menus, topToolStripItem.DropDownItems);
                // 添加到WinShellApplication和顶层菜单节点对应表
                ApplicationMenuStripMap.Add(app, topToolStripItem);
            };

            // 如果当前线程不是界面线程，则通过Invoke来处理界面变更
            if (InvokeRequired)
            {
                Invoke(del, application);
            }
            else // 否则，直接操作界面
            {
                del(application);
            }
        }

        /// <summary>
        /// 递归创建菜单。
        /// </summary>
        /// <param name="menus">WinShellMenu对象列表。</param>
        /// <param name="dropDownItems">菜单列表。</param>
        private void CreateChildMenu(List<WinShellMenu> menus, 
            ToolStripItemCollection dropDownItems)
        {
            ToolStripMenuItem item;

            foreach (var menu in menus)
            {
                item = (ToolStripMenuItem)dropDownItems.Add(menu.Text,
                    LoadImage(menu.Application.Bundle, menu.Icon), MenuClicked);

                // 将WinShellMenu保存到ToolStripMenuItem.Tag中
                item.Tag = menu;
                item.ToolTipText = menu.Tooltip;

                CreateChildMenu(menu.Children, item.DropDownItems);
            }
        }

        // 处理菜单点击事件
        private void MenuClicked(object sender, EventArgs e)
        {
            // 通过Tag属性获取WinShellMenu对象
            var menu = (sender as ToolStripMenuItem).Tag as WinShellMenu;

            // 如果已经有一个TabPage显示，则直接选择该TabPage
            foreach (TabPage tab in WorkspaceTabControl.TabPages)
            {
                if (tab.Tag == menu)
                {
                    WorkspaceTabControl.SelectedTab = tab;
                    return;
                }
            }

            // 加载菜单对应的类型
            Type menuClass = null;
            if (!string.IsNullOrEmpty(menu.ClassName))
            {
                try
                {
                    // 必须通过对应的插件来动态加载类型
                    menuClass = menu.Application.Bundle.LoadClass(menu.ClassName);
                }
                catch
                {
                    FileLogUtility.Error(string.Format("Failed to load class '{0}' from bundle '{1}'.", menu.ClassName, menu.Application.Bundle.SymbolicName));
                }
            }
            if (menuClass != null)
            {
                // 创建菜单对应的控件实例
                Control control = (Control)System.Activator.CreateInstance(menuClass);

                Form form = control as Form;
                if (form != null)
                {
                    form.TopLevel = false;
                    form.WindowState = FormWindowState.Maximized;
                    form.ControlBox = false;
                    form.FormBorderStyle = FormBorderStyle.None;
                }

                // 创建一个标签页，并显示菜单对应的控件（窗体或用户控件）
                TabPage tabPage = new TabPage(menu.Text);
                // 将WinShellMenu与TabPage关联
                tabPage.Tag = menu;
                tabPage.Controls.Add(control);
                WorkspaceTabControl.TabPages.Add(tabPage);
                tabPage.AutoScroll = true;
                WorkspaceTabControl.SelectedTab = tabPage;
                control.Dock = DockStyle.Fill;
                control.Visible = true;
            }
        }

        /// <summary>
        /// 从插件加载图像。
        /// </summary>
        /// <param name="bundle">所在插件。</param>
        /// <param name="imageFullName">嵌入资源图像全名。</param>
        /// <returns>图像对象。</returns>
        private Image LoadImage(IBundle bundle, string imageFullName)
        {
            if (!string.IsNullOrEmpty(imageFullName))
            {
                try
                {
                    var resource = bundle.LoadResource(imageFullName, ResourceLoadMode.Local);
                    if (resource != null)
                    {
                        return Image.FromStream((Stream)resource);
                    }
                }
                catch
                {
                    FileLogUtility.Error(string.Format("Failed to load image '{0}' from bundle '{1}'.", imageFullName, bundle.SymbolicName));
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 扩展变更事件是一个异步事件，即是一个新的线程来处理事件。
        /// 因此，需要将扩展变更事件对界面的变更通过代理发送到界面
        /// 线程，由其来更新界面。
        /// </summary>
        /// <param name="application">应用对象。</param>
        private delegate void RemoveMenuStripForApplicationDelegate(WinShellApplication application);

        private void RemoveMenuStripForApplication(WinShellApplication application)
        {
            RemoveMenuStripForApplicationDelegate del = (WinShellApplication app) =>
            {
                if (ApplicationMenuStripMap.ContainsKey(app))
                {
                    // 删除菜单、Map、显示的TabPage
                    TopMenuStrip.Items.Remove(ApplicationMenuStripMap[app]);
                    ApplicationMenuStripMap.Remove(app);
                    foreach (TabPage tab in WorkspaceTabControl.TabPages)
                    {
                        var menu = tab.Tag as WinShellMenu;
                        if (menu.Application.Equals(app)) // 清除Selection
                        {
                            var index = WorkspaceTabControl.SelectedIndex;
                            if (index > 0 && tab.Equals(WorkspaceTabControl.SelectedTab))
                            {
                                WorkspaceTabControl.SelectedIndex = index - 1;
                            }
                            WorkspaceTabControl.TabPages.Remove(tab);
                        }
                    }
                }
            };
            // 如果当前线程不是界面线程，则通过Invoke来处理界面变更
            if (InvokeRequired)
            {
                Invoke(del, application);
            }
            else // 否则，直接操作界面
            {
                del(application);
            }
        }
    }
}