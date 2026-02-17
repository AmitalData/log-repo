using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Controllers.CommonDataModel.Extended;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.Linq;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class XLSExportService
    {
        public XLSExportService()
        {

        }
        static XLSExportService()
        {
          
            ContainerAccessor.Container.RegisterType<IExcelExport, ExchangeRateExport>
                ((new ExchangeRateExport()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IExcelExport, GuaranteeExport>
                ((new GuaranteeExport()).MainInterfaceCode);


            ContainerAccessor.Container.RegisterType<IExcelExport, MasavPaymentsToAgentExport>
                ((new MasavPaymentsToAgentExport()).MainInterfaceCode);

        }

        public static IExcelExport GetExcelFormator(string mainInterfaceCode, string correlationId = "")
        {
            

            if (!ContainerAccessor.Container.IsRegistered<IExcelExport>(mainInterfaceCode))
            {
                var inst = new InterfaceManagementDetails();
                var row = inst.GetAll().FirstOrDefault(r => r.ResponseInterfaceCode == mainInterfaceCode);
                if (row == null)
                {
                    throw new System.Exception("ResolveAndExecute(" + mainInterfaceCode + " , " + correlationId + ") But if (!ContainerAccessor.Container.IsRegistered<IExcelFormator>(mainInterfaceCode)), No ResponseInterfaceCode");
                }

                mainInterfaceCode = row.Code;
                if (!ContainerAccessor.Container.IsRegistered<IExcelExport>(mainInterfaceCode))
                {
                    throw new System.Exception("ResolveAndExecute(" + mainInterfaceCode + " , " + correlationId + ") is response of  But if (!ContainerAccessor.Container.IsRegistered<IExcelFormator>(mainInterfaceCode))");
                }
            }
            var anaO = ContainerAccessor.Container.Resolve<IExcelExport>(mainInterfaceCode);
            return anaO;

        }
    
        public byte[] Start( string mainInterfaceCode/*=8347*/,String logId/*=1-1370569&*/,int tenant, IRequestProvider requestProvider =null)
        {

            var excelFormator =GetExcelFormator(mainInterfaceCode);
            //excelFormator.Start(mainInterfaceCode/*=8347*/, logId/*=1-1370569&*/, tenant);


            excelFormator.RequestProvider = requestProvider;

            var ary =excelFormator.ExportCustomRequestToExcel(mainInterfaceCode,logId/*=1-1370569&*/, tenant);
            return ary;

        }
    }
    public interface IExcelExport {
        string MainInterfaceCode { get; }
        IRequestProvider RequestProvider { get; set; }

        byte[] ExportCustomRequestToExcel(string mainInterfaceCode,string logId, int tenant);
        
        
    };
}