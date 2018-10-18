using Logitude.Server.Tools.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.CustomWebServices.Maintenance
{
    public partial class SignServiceTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Task.Factory.StartNew(() =>
            {
             
                Logitude.Customs.BL.Messaging.Customs.SignQueue.Instance.RefreshDb();
            
                Logitude.Server.Tools.ExternalServices.SignatureHubClient.Instance.WakeUp();
            });
        }



        protected void ButtonGetSignQueueList_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(TextBoxTenant.Text);
            var myServerStateWebService = new ServerStateWebService();
            TextBoxResult.Text = myServerStateWebService.GetSignQueueXml(tenant);
        }

        protected void ButtonSubscribeSignServer_Click(object sender, EventArgs e)
        {
            var tenant = int.Parse(TextBoxTenant.Text);
            var myServerStateWebService = new ServerStateWebService();
            TextBoxResult.Text = myServerStateWebService.GetSubscribeSignServerListXml(tenant);
        }

        protected void ButtonSignQPersonal_Click1(object sender, EventArgs e)
        {
            Logitude.Server.Tools.ExternalServices.SignatureHubClient.Instance.Send(SignQueueByType.SignQueueByPersonId, this.TextBoxId.Text);
        }

        protected void ButtonSignQCompany_Click1(object sender, EventArgs e)
        {
            Logitude.Server.Tools.ExternalServices.SignatureHubClient.Instance.Send(SignQueueByType.SignQueueByCustomsAgentId, this.TextBoxId.Text);
        }

    }
}