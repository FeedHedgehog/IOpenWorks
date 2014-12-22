using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UIShell.OSGi;
using UIShell.OSGi.Core.Service;

namespace UIShell.SimpleWinFormShellPlugin.ExtensionModel
{
    /// <summary>
    /// 对应UIShell.Applications扩展点一个Application配置解析的结果。
    /// 
    /// Application为根节点，下面包含Menu字节点，点击Menu时，在内容区域显示
    /// 对应的窗体或者用户控件。它表示一个插件提供的菜单。
    /// 
    /// Application有Title、ToolTip和Icon三个属性，分别描述应用标题、提示文本
    /// 和图标。
    /// 
    /// Menu有Text、ToolTip、Icon和Class四个属性，前三个属性同Application，Class
    /// 表示点击时在内容区域显示的窗体或者用户控件。
    /// 
    /// <![CDATA[
    /// <Extension Point="UIShell.Applications">
    /// 	  <Application Title="" ToolTip="" Icon="">
    /// 		  <Menu Text="" ToolTip="" Icon="" Class="">
    /// 			  <Menu Text="" ToolTip="" Icon="" Class=""/>
    /// 		  </Menu>
    /// 		  <Menu Text="" ToolTip="" Icon="" Class=""/>
    /// 	  </Application>
    ///   </Extension>
    /// ]]>
    /// </summary>
    public class WinShellApplication
    {
        public WinShellApplication(string title, string tooltip, string icon, IBundle bundle)
        {
            Title = title;
            ToolTip = tooltip;
            Icon = icon;
            Bundle = bundle;
            Menus = new List<WinShellMenu>();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is WinShellApplication)
            {
                WinShellApplication other = obj as WinShellApplication;
                return Equals(BundleID, other.BundleID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Bundle.GetHashCode();
        }

        public string Title { get; private set; }

        public string ToolTip { get; private set; }

        public string Icon { get; private set; }

        /// <summary>
        /// 定义这个Application的插件。
        /// </summary>
        public IBundle Bundle { get; private set; }

        public long BundleID
        {
            get { return Bundle.BundleID; }
        }

        /// <summary>
        /// 菜单定义。
        /// </summary>
        public List<WinShellMenu> Menus { get; private set; }

        public void AddMenu(WinShellMenu menu)
        {
            Menus.Add(menu);
        }
    }
}