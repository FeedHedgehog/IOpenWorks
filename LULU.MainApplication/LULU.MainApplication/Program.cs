using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using UIShell.OSGi;
using System.Reflection;
using UIShell.PageFlowService;
using System.Threading;
using System.Runtime.Serialization;
using LULU.MainApplication.Properties;
using UIShell.OSGi.Utility;
using System.Configuration;
using UIShell.OSGi.Logging;
using UIShell.iOpenWorks.Bootstrapper;

namespace LULU.MainApplication
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            UpdateCore();
            RunApplication();
        }

        private static void UpdateCore()
        {
            if (AutoUpdateCoreFiles)
            {
                new CoreFileUpdater().UpdateCoreFiles(CoreFileUpdateCheckType.Daily);
            }
        }

        private static void RunApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            const string PAGE_NAME_LOGIN = "LoginPage";

            FileLogUtility.SetFileLogEnabled(true);
            FileLogUtility.SetLogFileName("log.txt");
            FileLogUtility.SetLogLevel(LogLevel);
            FileLogUtility.SetMaxFileSizeByMB(MaxLogFileSize);
            FileLogUtility.SetCreateNewFileOnMaxSize(CreateNewLogFileOnMaxSize);

            using (BundleRuntime bundleRuntime = new BundleRuntime())
            {
                bundleRuntime.Start();

                // Get the page flow service.
                IPageFlowService pageFlowService = bundleRuntime.GetFirstOrDefaultService<IPageFlowService>();

                if (pageFlowService == null)
                {
                    throw new Exception(Resources.IPageFlowServiceServiceNotFound);
                }

                // Assert the first page node.
                if (string.IsNullOrEmpty(pageFlowService.FirstPageNodeValue) ||
                    string.IsNullOrEmpty(pageFlowService.FirstPageNodeName) ||
                    pageFlowService.FirstPageNodeOwner == null)
                {
                    throw new Exception(Resources.CanNotFindAnAvailablePageNode);
                }

                // Load the form type of first page node.
                Type formType = pageFlowService.FirstPageNodeOwner.LoadClass(pageFlowService.FirstPageNodeValue);
                if (formType == null)
                {
                    throw new Exception(string.Format(Resources.CanNotLoadTypeFromBundle, pageFlowService.FirstPageNodeValue, pageFlowService.FirstPageNodeOwner.SymbolicName));
                }

                // Create the form instance of first page node and show it.
                Form formInstance = System.Activator.CreateInstance(formType) as Form;

                if (formInstance == null)
                {
                    throw new Exception(string.Format(Resources.TypeIsNotWindowsFormType, pageFlowService.FirstPageNodeValue, pageFlowService.FirstPageNodeOwner.SymbolicName));
                }

                // If first page node name is 'Login' then, show the dialog and then run application.
                if (pageFlowService.FirstPageNodeName.Equals(PAGE_NAME_LOGIN))
                {
                    DialogResult result = formInstance.ShowDialog();
                    if (result == DialogResult.OK || result == DialogResult.Yes)
                    {
                        PageNode nextNode = pageFlowService.GetNextPageNode(pageFlowService.FirstPageNodeName);
                        if (nextNode != null)
                        {
                            Type mainPageFormType = nextNode.Bundle.LoadClass(nextNode.Value);
                            if (mainPageFormType == null)
                            {
                                throw new Exception(string.Format(Resources.CanNotLoadTypeFromBundle, nextNode.Value, nextNode.Bundle.SymbolicName));
                            }
                            Form mainPageForm = System.Activator.CreateInstance(mainPageFormType) as Form;
                            Application.Run(mainPageForm);
                        }
                    }
                }
                else // Run the application directly.
                {
                    Application.Run(formInstance);
                }
            }
        }

        /// <summary>
        /// 日志级别。
        /// </summary>
        private static LogLevel LogLevel
        {
            get
            {
                string level = ConfigurationSettings.AppSettings["LogLevel"];
                if (!string.IsNullOrEmpty(level))
                {
                    try
                    {
                        object result = Enum.Parse(typeof(LogLevel), level);
                        if (result != null)
                        {
                            return (LogLevel)result;
                        }
                    }
                    catch { }
                }
                return LogLevel.Debug;
            }
        }

        /// <summary>
        /// 日志文件限制的大小。
        /// </summary>
        private static int MaxLogFileSize
        {
            get
            {
                string size = ConfigurationSettings.AppSettings["MaxLogFileSize"];
                if (!string.IsNullOrEmpty(size))
                {
                    try
                    {
                        return int.Parse(size);
                    }
                    catch { }
                }

                return 10;
            }
        }

        /// <summary>
        /// 当日志大小超过限制时，是否新建一个。
        /// </summary>
        private static bool CreateNewLogFileOnMaxSize
        {
            get
            {
                string createNew = ConfigurationSettings.AppSettings["CreateNewLogFileOnMaxSize"];
                if (!string.IsNullOrEmpty(createNew))
                {
                    try
                    {
                        return bool.Parse(createNew);
                    }
                    catch { }
                }

                return false;
            }
        }

        /// <summary>
        /// 是否启用内核自动更新。
        /// </summary>
        private static bool AutoUpdateCoreFiles
        {
            get
            {
                string autoUpdateCoreFiles = ConfigurationSettings.AppSettings["AutoUpdateCoreFiles"];
                if (!string.IsNullOrEmpty(autoUpdateCoreFiles))
                {
                    try
                    {
                        return bool.Parse(autoUpdateCoreFiles);
                    }
                    catch { }
                }

                return false;
            }
        }
    }
}
