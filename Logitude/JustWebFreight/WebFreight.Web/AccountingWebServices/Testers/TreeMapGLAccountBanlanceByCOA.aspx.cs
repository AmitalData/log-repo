using Logitude.Accounting.BL.CoreBL.Dashboard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.AccountingWebServices.Testers
{
    public partial class TreeMapGLAccountBanlanceByCOA : System.Web.UI.Page
    {
        protected string _MyJson;
        protected string _Tenant;
        protected string _MyCollector;
        protected string _CallBackCOATypeCode;
        protected string _CallBackParentCOAId;

        protected void Page_Load(object sender, EventArgs e)
        {
            string stenant = Request.QueryString["Tenant"];
            _Tenant = stenant;
            int tenant = -999;
            if (!int.TryParse(stenant, out tenant))
            {
                throw new Exception("tenant is must");
            }
            string MyCollector= Request.QueryString["MyCollector"];
            _MyCollector = MyCollector;
            var ByBalance = Request.QueryString["ByBalance"];
            if (string.IsNullOrEmpty(ByBalance ))
            {
                throw new Exception("ByBalance  is must");
            }
            _ByBalance = ByBalance;
            var CallBackCOATypeCode = Request.QueryString["CallBackCOATypeCode"];
            _CallBackCOATypeCode = CallBackCOATypeCode;
            var CallBackParentCOAId = Request.QueryString["CallBackParentCOAId"];
            _CallBackParentCOAId = CallBackParentCOAId;
            
            var myAllCardServiceDS = new GLAccountDashboard();
            bool twoLevel = false;
            List<ChartOfAccountBalanceM> dic = myAllCardServiceDS.TreeMapGLAccountBanlanceByCOA(tenant, ByBalance, MyCollector, CallBackCOATypeCode, CallBackParentCOAId/*, twoLevel*/);
            dic.ForEach(item => item.ChildName = item.ChildName.Replace("'"[0], ' '));
            dic.ForEach(item => item.ParentName = item.ParentName.Replace("'"[0], ' '));
            _MyJson =JsonConvert.SerializeObject(dic);
        }



        public string _ByBalance { get; set; }
    }
}