using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UIShell.OSGi;
using UIShell.OSGi.Core.Service;

namespace UIShell.SimpleWinFormShellPlugin.ExtensionModel
{
    /// <summary>
    /// ��Ӧ��չ��Menu�ڵ㡣
    /// </summary>
    public class WinShellMenu
    {
        public WinShellMenu(WinShellApplication application, WinShellMenu parent, 
            string text, string tooltip, string icon, string className, int level, string guid)
        {
            Application = application;
            Parent = parent;
            Text = text;
            Tooltip = tooltip;
            Icon = icon;
            ClassName = className;
            Level = level;
            Guid = guid;
            Children = new List<WinShellMenu>();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is WinShellMenu)
            {
                WinShellMenu other = obj as WinShellMenu;
                return Equals(Guid, other.Guid);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        /// <summary>
        /// ������Application��
        /// </summary>
        public WinShellApplication Application { get; private set; }

        public string Text { get; private set; }

        public string Tooltip { get; private set; }

        public string Icon { get; private set; }

        /// <summary>
        /// ����˵���ʾ�Ĵ�������û��ؼ������͡�
        /// </summary>
        public string ClassName { get; private set; }

        public int Level { get; private set; }

        public string Guid { get; private set; }

        /// <summary>
        /// ���˵���
        /// </summary>
        public WinShellMenu Parent { get; private set; }

        /// <summary>
        /// �Ӳ˵���
        /// </summary>
        public List<WinShellMenu> Children { get; private set; }
    }
}