using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.StimulReportCustomizationDataProvider
{
    public class StiBusinessObjectDataService
    {

        public List<StiBusinessObjectData> Get(DocumentTypeTemplatePM documentTypeTemplatePM)
        {
            List<StiBusinessObjectData> businessObjects = new List<StiBusinessObjectData>();
            DocumentDataProviderArgs documentDataProviderArgs = null;
            switch (documentTypeTemplatePM.DocumentTypeCode)
            {
                case "EXCU":
                case "SELE":
                case "TML":
                case "740PP":
                case "740":
                case "AVISC":
                    {
                        businessObjects.Add(new StiBusinessObjectData("ShipmentPM", "ShipmentPMDataProvider", "ShipmentPMDataProvider", typeof(ShipmentPM)));
                        documentDataProviderArgs = new DocumentDataProviderArgs() { Type = typeof(AWBDataProvider), Category = "AWB" };
                        break;
                    }
                default:
                    {
                        break;

                    }

            }

            if (documentDataProviderArgs == null) return businessObjects;

            documentDataProviderArgs.DocumentTypeTemplatePM = documentTypeTemplatePM;
            var documentDataProvider = new DocumentDataProviderGreator(documentDataProviderArgs).Create();
            businessObjects.Add(new StiBusinessObjectData(documentDataProviderArgs.Category, documentDataProvider.Name, documentDataProvider.Name, documentDataProvider.Type));
            return businessObjects;


        }


    }
}