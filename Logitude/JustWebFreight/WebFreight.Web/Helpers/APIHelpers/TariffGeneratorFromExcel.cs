using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Controllers.WebDomainControllers;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class TariffGeneratorFromExcel : BatchTaskExecutionsService
    {
        public TariffGeneratorFromExcel(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(TariffsExcelGeneratorArgs));
            TariffsExcelGeneratorArgs parameterArgs = serializer.Deserialize(stringReader) as TariffsExcelGeneratorArgs;
            List<ExcelTariffLines> TariffLines = parameterArgs.TariffLines; 
            ITariffModuleContext iContext = TariffModuleContext.GetContext(parameterArgs.Tenant);
            TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == parameterArgs.Tenant select d).FirstOrDefault();

            if (iTariffSetting != null)
            {

            }
        }
    }

    public class TariffsExcelGeneratorArgs
    {
        public int Tenant { get; set; }
        public string LoggedUserEmail { get; set; }
        public List<ExcelTariffLines> TariffLines { get; set; }
    }
}