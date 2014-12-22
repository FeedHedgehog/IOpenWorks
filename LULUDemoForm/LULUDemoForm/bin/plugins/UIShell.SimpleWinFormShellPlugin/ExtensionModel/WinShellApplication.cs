using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UIShell.OSGi;
using UIShell.OSGi.Core.Service;

namespace UIShell.SimpleWinFormShellPlugin.ExtensionModel
{
    /// <summary>
    /// ��ӦUIShell.Applications��չ��һ��Application���ý����Ľ����
    /// 
    /// ApplicationΪ���ڵ㣬�������Menu�ֽڵ㣬���Menuʱ��������������ʾ
    /// ��Ӧ�Ĵ�������û��ؼ�������ʾһ������ṩ�Ĳ˵���
    /// 
    /// Application��Title��ToolTip��Icon�������ԣ��ֱ�����Ӧ�ñ��⡢��ʾ�ı�
    /// ��ͼ�ꡣ
    /// 
    /// Menu��Text��ToolTip��Icon��Class�ĸ����ԣ�ǰ��������ͬApplication��Class
    /// ��ʾ���ʱ������������ʾ�Ĵ�������û��ؼ���
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
        /// �������Application�Ĳ����
        /// </summary>
        public IBundle Bundle { get; private set; }

        public long BundleID
        {
            get { return Bundle.BundleID; }
        }

        /// <summary>
        /// �˵����塣
        /// </summary>
        public List<WinShellMenu> Menus { get; private set; }

        public void AddMenu(WinShellMenu menu)
        {
            Menus.Add(menu);
        }
    }
}