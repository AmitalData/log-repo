using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logitude.Accounting.BL.CoreBL.Reports;
using System.Text;

namespace WebFreight.Web.AccountingWebServices.Testers
{
    public partial class TrailReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                var optTrailReportLevel = string.Join(",", Enum.GetNames(typeof(ReportLevel)));
                var param = new
                   {
                       Tenant = 989,
                       FromDate = new DateTime(2015, 1, 20),
                       ToDate = DateTime.Now.Date, //new DateTime(2016, 11, 20),
                       TrailReportLevelOption = "ChartofaccountType=1,Chartofaccount=2,GLAccount=3",
                       MyTrailReportLevel = ReportLevel.GLAccount,
                       CurrenciesDetailed = true,
                       //filter the GLAccount ?!?!?
                       Category1 = "",
                       Category2="",
                       Category3 = "",
                       Category4 = "",
                       Category5 = "",

                       //filter the GLAccount ?!?!?
                       DetailedControlClients = false,
                       DetailedControlVendors = false,
                       DetailedControlJob = false,
                       DetailedControlFile = false,
                       Suppress_DoNotShowCardWithoutActivity= true,
                    DoNotShowCardWithLocalCloseBalanceEqualZero=true,
                    ChartOfAccountsTypeCodeList = new List<string>()
                    {
                        "1","2"
                    },
                    ChartOfAccountsIdList = new List<string>(),

                };
                _HiddenFieldTrail.Value= _TextBoxParam.Text = JsonConvert.SerializeObject(param);


                var param1 = new //RevenueExpenseReportParam
                {
                    Tenant = 989,
                    FromDate = DateTime.Now.Date.AddMonths(-3),
                    ToDate = DateTime.Now.Date, //new DateTime(2016, 11, 20),
                    FromDatePeriod2 = DateTime.Now.Date.AddMonths(-1),
                    ToDatePeriod2 = DateTime.Now.Date, //new DateTime(2016, 11, 20),

                    MyRevenueExpenseReportLevelOptions = "ChartofaccountType=1,Chartofaccount=2,GLAccount=3",
                    MyRevenueExpenseReportLevel = ReportLevel.ChartofaccountType,
                    MyCardFilterOptions="DoNotShowCardWithZeroBalance=0,ShowCardsWithActivity_EvenBalanceItsZero=1,ShowAllCard=2",
                    MyCardFilter =  Logitude.Accounting.BL.CoreBL.Reports.RevenueExpenseReportParam.CardFilterEnum.DoNotShowCardWithZeroBalance,
                };
                _HiddenFieldExpense.Value =  JsonConvert.SerializeObject(param1);
            }
        }

        protected void _TrailReport_Click(object sender, EventArgs e)
        {

            var param = JsonConvert.DeserializeObject<TrailReportParam>(_TextBoxParam.Text);
            //using (
            var trailReportService = TrailReportFactory.CreateNew(param);//)
            try
            {

                var res = trailReportService.Execute();

                _Log.Text = trailReportService.DbLog;
                var bytes = LogitudeXmlSerializer.SerializeObject<List<TrailReportM>>(res);
                _ResultXML.Text = Encoding.UTF8.GetString(bytes);
                var ds = new DataSet();

                ds.ReadXml(new MemoryStream(bytes));
                ReloadGrid(ds);
            }
            finally
            {
               // trailReportService.Dispose();
            }

        }


        private void ReloadGrid(DataSet ds)
        {
            
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds.Tables[0].DefaultView;


            }
            else
            {
                GridView1.DataSource = null;
            }
            //_LabelLog.Text = LogMessagingUtil.Instance.ToString();
            GridView1.DataBind();
        }

        protected void _RevenueExpenseReport_Click(object sender, EventArgs e)
        {
            var param = JsonConvert.DeserializeObject<RevenueExpenseReportParam>(_TextBoxParam.Text);
            var trailReportService = new RevenueExpenseReportService(param, 10);
            try
            {
                trailReportService.InteractiveCheck();
                var res = trailReportService.Execute();

                _Log.Text = trailReportService.DbLog;
                var bytes = LogitudeXmlSerializer.SerializeObject<List<RevenueExpenseReportM>>(res);
                var ds = new DataSet();

                ds.ReadXml(new MemoryStream(bytes));
                ReloadGrid(ds);
            }
            finally
            {
                //trailReportService.Dispose();
            }
            
            
        }
    }
}