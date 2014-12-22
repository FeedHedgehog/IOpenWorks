using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UIShell.OSGi;
using UIShell.OSGi.Core.Service;
using System.Threading;

namespace UIShell.SimpleWinFormShellPlugin.ExtensionModel
{
    /// <summary>
    /// 将扩展点解析成WinShellApplication对象。
    /// </summary>
    public class WinShellApplicationContainer
    {
        private static WinShellApplicationContainer _instance = null;

        /// <summary>
        /// 单例。
        /// </summary>
        public static WinShellApplicationContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    Interlocked.CompareExchange(ref _instance,
                        new WinShellApplicationContainer(), null);
                }
                return _instance;
            }
        }

        /// <summary>
        /// 所有WinShellApplication对象。
        /// </summary>
        public List<WinShellApplication> Applications { get; private set; }

        private WinShellApplicationContainer()
        {
            Applications = new List<WinShellApplication>();
        }

        /// <summary>
        /// 将一个扩展转换成WinShellApplication对象。
        /// </summary>
        /// <param name="extension">指定的扩展对象。</param>
        /// <returns>返回的WinShellApplication对象。</returns>
        public WinShellApplication AddApplicationForExtension(Extension extension)
        {
            WinShellApplication app = null;
            List<XmlNode> data = extension.Data;

            if (data != null && data.Count > 0)
            {
                // 1 将扩展的Application XML节点转换成WinShellApplication对象。
                XmlNode topNode = data[0];
                if (topNode.NodeType == XmlNodeType.Element && topNode.Attributes != null && topNode.Attributes["Title"] != null)
                {
                    string appTitle = topNode.Attributes["Title"].Value;
                    string appToolTip = topNode.Attributes["ToolTip"].Value;
                    string appIcon = topNode.Attributes["Icon"].Value;

                    if (!string.IsNullOrEmpty(appTitle))
                    {
                        string parentGuid = Guid.NewGuid().ToString();
                        int topNodeLevel = 0;
                        app = new WinShellApplication(appTitle, appToolTip, appIcon, extension.Owner);

                        // 2 递归的将扩展的Menu XML节点及其子节点转换成WinShellMenu对象。
                        CreateWinShellMenusFromXmlNode(app.Menus, app, null, topNode, topNodeLevel);

                        // 3 将Application对象保存起来。
                        _instance.AddApplication(app);
                    }
                }
            }

            return app;
        }        

        /// <summary>
        /// 将扩展的Menu对应的XML节点解析成WinShellMenu集合。
        /// </summary>
        /// <param name="menus">顶层WinShellMenu集合。</param>
        /// <param name="application">所属的WinShellApplication。</param>
        /// <param name="parent">父菜单。</param>
        /// <param name="topNode">顶层XML节点。</param>
        /// <param name="nodeLevel">XML节点层次。</param>
        private void CreateWinShellMenusFromXmlNode(List<WinShellMenu> menus, WinShellApplication application, WinShellMenu parent, XmlNode topNode, int nodeLevel)
        {
            if (menus == null)
            {
                menus = new List<WinShellMenu>();
            }

            foreach (XmlNode childNode in topNode.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    string type = null;
                    if (childNode.Attributes["Class"] != null)
                        type = childNode.Attributes["Class"].Value;

                    string text = childNode.Attributes["Text"] == null ? string.Empty : childNode.Attributes["Text"].Value;
                    string toolTip = childNode.Attributes["ToolTip"] == null ? string.Empty : childNode.Attributes["ToolTip"].Value;
                    string icon = childNode.Attributes["Icon"] == null ? string.Empty : childNode.Attributes["Icon"].Value;
                    string guid = Guid.NewGuid().ToString();

                    // 创建WinShellMenu对象。
                    WinShellMenu winShellMenu = new WinShellMenu(application, parent, text, toolTip, icon, type, nodeLevel, guid);
                    menus.Add(winShellMenu);

                    // 递归创建子节点对象。
                    int childNodeLevel = nodeLevel + 1;
                    CreateWinShellMenusFromXmlNode(winShellMenu.Children, application, winShellMenu, childNode, childNodeLevel);
                }
            }
        }

        /// <summary>
        /// 删除Extension对应的WinShellApplication。
        /// </summary>
        /// <param name="extension">指定的Extension。</param>
        /// <returns>删除的WinShellApplication对象。</returns>
        public WinShellApplication RemoveApplicationForExtension(Extension extension)
        {
            var app = GetWinShellApplication(extension.Owner);
            if (app != null)
            {
                RemoveApplication(app);
            }
            return app;
        }

        /// <summary>
        /// 获取指定插件的WinShellApplication对象。
        /// </summary>
        /// <param name="bundle">指定插件。</param>
        /// <returns>WinShellApplication对象。</returns>
        public WinShellApplication GetWinShellApplication(IBundle bundle)
        {
            foreach (WinShellApplication app in Applications)
            {
                if (app.Bundle.Equals(bundle))
                    return app;
            }

            return null;
        }

        /// <summary>
        /// 获取指定插件的顶层菜单对象。
        /// </summary>
        /// <param name="bundle">指定插件。</param>
        /// <returns>顶层菜单对象集合。</returns>
        public List<WinShellMenu> GetTopWinShellMenus(IBundle bundle)
        {
            WinShellApplication app = GetWinShellApplication(bundle);
            if (app != null)
                return app.Menus;

            return null;
        }

        private void AddApplication(WinShellApplication app)
        {
            if (app != null)
                Applications.Add(app);
        }

        private void RemoveApplication(WinShellApplication app)
        {
            if (app != null && Applications.Contains(app))
            {
                Applications.Remove(app);
            }
        }
    }
}