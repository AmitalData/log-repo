
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using WebFreight.Web.CustomWebServices.BL.XLSExport;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.CustomWebServices.Testers
{
    public partial class ItzikTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();

                //    var myCCUFILEMRepository = new CCUFILEMRepository(1);
                //var ccufilem= myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(51340152);
                //var myCCUQUELOCKRepository = new CCUQUELOCKRepository(1);
                //var res =
                //    //myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CFIFILEM", "51340152");
                //    myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());

                //var s = new
                //    //DCAInUCB2750_MsgMessagingService();
                //    DCAInUCB2755_MsgMessagingService();
                //s.CreateCRS(1, "1-7", "1-69", "bbb" , "1-3");
                //ExportExcel();
                ExportExcel8330();

            }
            catch (Exception eee)
            {
                Response.Clear();
                Response.Write(eee.ToString());

            }
            
        }
        private static void ExportExcel8330()
        {
            var myXLSExportService = new XLSExportService();
            var result =
            myXLSExportService
            //.Start("8347","1-1370596", 1);
            .Start("8330", null, 1, new BlockListInWarehouseDetailProvider());

            string ShowType = "attachment";
            string documentName = Guid.NewGuid().ToString() + ".xls";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
            HttpContext.Current.Response.BinaryWrite(result);
        }

        private static void ExportExcel()
        {
            var myXLSExportService = new XLSExportService();
            var result =
            myXLSExportService
            //.Start("8347","1-1370596", 1);
            .Start("8368", null, 1, new MasavPaymentsToAgentProvider());

            string ShowType = "attachment";
            string documentName = Guid.NewGuid().ToString() + ".xls";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
            HttpContext.Current.Response.BinaryWrite(result);
        }

        private static void Test1()
        {
            var ex = new ExternalTasksQueueWcfService();
            var ttt = ex.GetTaskFromQueue(1, 1);
            var s = new Unifreight_L2US01_US2L01_SivugMessagingService();
            var res = s.Send(new Logitude.CustomsMessaging.Common.RequestParams.Unifreight_L2US01RequestParam()
            {
                CCUFILEmFileNo = "50012651",

                CFIFILEMFileNo = "616200433",


                Tenant = 1,



                ///<-- itzik (yaron ask )
                RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive,
                LoggingEntityId = "1-2651", //declarationId 
                ///LoggingEntityReference = entityPM.DeclarationNumber,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingUserId = "1-10",
                RequestName = "L2US01 Request",
                ResponseName = "US2L01 Request",
            });

            return;

            Logitude.BL.CommonDataModel.EntityQueries.TenantQuery.GetSingleTenantPM(1, false);
            var closedTableId = "1118";
            closedTableId = "1091";
            //closedTableId = "1354";
            var tenant = 1;
            //var DualQueryService = new DualQueryService(AmitalContext.GetContext(208));
            //OracleTime = DualQueryService.GetServerDateTime().GetValueOrDefault(); 
            var messageService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
            var req = new Logitude.CustomsMessaging.Common.RequestParams.SystemTableRequestParams()
            {
                TableId = closedTableId,
                Tenant = tenant,//_CustomsSetting.Tenant ,
                RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch
            };
            //if (closedTableId == "1892")
            {
                ///req.AsTableData = true;
                var debugIt = true;
                if (debugIt)
                {
                    req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive;
                }
            }
            // req.RequestVIA = Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive;    
            messageService.Send(req);
        }






        public DateTime? OracleTime { get; set; }
    }
}