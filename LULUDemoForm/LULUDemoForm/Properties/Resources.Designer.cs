﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5448
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LULUDemoForm.Properties
{
    using System;


    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources()
        {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("UIShell.iOpenWorks.WinForm.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Can not find an available page node..
        /// </summary>
        internal static string CanNotFindAnAvailablePageNode
        {
            get
            {
                return ResourceManager.GetString("CanNotFindAnAvailablePageNode", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Can not load type &apos;{0}&apos; from bundle &apos;{1}&apos;.
        /// </summary>
        internal static string CanNotLoadTypeFromBundle
        {
            get
            {
                return ResourceManager.GetString("CanNotLoadTypeFromBundle", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Can not find an IPageFlowService service. Make sure that the bundle UIShell.PageFlowService can be started with framework..
        /// </summary>
        internal static string IPageFlowServiceServiceNotFound
        {
            get
            {
                return ResourceManager.GetString("IPageFlowServiceServiceNotFound", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; from bundle &apos;{1}&apos; is not a Windows Form type..
        /// </summary>
        internal static string TypeIsNotWindowsFormType
        {
            get
            {
                return ResourceManager.GetString("TypeIsNotWindowsFormType", resourceCulture);
            }
        }
    }
}
