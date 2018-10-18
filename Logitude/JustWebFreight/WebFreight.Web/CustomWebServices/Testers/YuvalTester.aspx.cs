using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.CustomWebServices.Testers
{
    public partial class YuvalTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO.DBTester.test();
        }
    }
}