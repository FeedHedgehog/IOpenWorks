using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UIShell.PageFlowService;
using UIShell.OSGi;

namespace DemoMVC4
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get PageFlowService.
            IPageFlowService pageFlowService = BundleRuntime.Instance.GetFirstOrDefaultService<IPageFlowService>();
            if (pageFlowService == null)
            {
                throw new ServiceNotAvailableException(typeof(IPageFlowService).FullName, Properties.Resources.IOpenWorksWebShellName);
            }

            if (string.IsNullOrEmpty(pageFlowService.FirstPageNodeValue))
            {
                throw new Exception(Properties.Resources.CanNotFindAnAvailablePageNode);
            }
            // Redirect to first node.
            Response.Redirect(pageFlowService.FirstPageNodeValue);
        }
    }
}