using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers.GLSHKAnalyzer
{
    public partial class GLSHKAnalyzer
    {
        private GLSHK.FMA myFMA;
        private void AnalyzeBaseData_FMA()
        {
            this.myFMA = (GLSHK.FMA)myMessage.Item;
            string myReceivedMessageDetail = myFMA.ReceivedMessageDetail;

            this.myMessageDetailCode = myReceivedMessageDetail.Substring(0, 3);
            switch (myMessageDetailCode)
            {
                case "FWB":
                case "AWB":
                case "FFR":
                    {
                        int indexOfSeparator = myReceivedMessageDetail.IndexOf("-");
                        this.myPrefix = myReceivedMessageDetail.Substring(indexOfSeparator - 3, 3);
                        this.myMaster = myReceivedMessageDetail.Substring(indexOfSeparator + 1, 8);

                        break;
                    }

                case "FHL":
                    {
                        this.isFHLType = true;

                        int indexOfSeparator = myReceivedMessageDetail.IndexOf("-");
                        this.myPrefix = myReceivedMessageDetail.Substring(indexOfSeparator - 3, 3);
                        this.myMaster = myReceivedMessageDetail.Substring(indexOfSeparator + 1, 8);

                        if (myReceivedMessageDetail.Contains("HBS"))
                        {
                            string myFixedDetails = myReceivedMessageDetail.Substring(myReceivedMessageDetail.IndexOf("HBS"));
                            string[] dataArray = myFixedDetails.Split('/');
                            this.myHouse = dataArray[1];
                        }

                        break;
                    }

                default:
                    {
                        string myRefId = this.myEnvelope.RefID;
                        if (!string.IsNullOrEmpty(myRefId))
                        {
                            myRefId = myRefId.Replace("HMF", "");
                            string[] myRefIdArray = myRefId.Split('X');
                            
                            this.myPrefix = myRefIdArray[1];
                            this.myMaster = myRefIdArray[0];

                            if (myMaster.Length < 8)
                            {
                                myMaster += GetCheckDigit(myMaster);
                            }
                        }

                        break;
                    }
            }
        }

        private void AnalyzeMessageQueue_FMA()
        {
            shipmentPM.IsUpdatedByGLSHKAnalyzer = true;

            shipmentPM.FNAReason = null;
            shipmentPM.CarrierLastStatusCode = "FMA";
            shipmentPM.CarrierLastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
            string systemEmail = "system@tenant" + myTenant + ".com";

            switch (myMessageDetailCode)
            {
                case "FWB":
                case "AWB":
                    {
                        shipmentPM.FWBStatusCode = "ACPT";
                        shipmentPM.FWBStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

                        ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
                        service.Update();

                        break;
                    }

                case "FHL":
                    {
                        shipmentPM.FHLStatusCode = "ACPT";
                        shipmentPM.FHLStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

                        ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
                        service.Update();

                        if (shipmentPM.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipmentPM.MasterShipmentDataId))
                        {
                            ShipmentRepository shipmentRepository = new ShipmentRepository(myShipmentContext);
                            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                            ShipmentPM masterShipmentPM = shipmentQuery.GetSinglePM(shipmentPM.MasterShipmentDataId, myTenant);

                            masterShipmentPM.FHLStatusCode = shipmentRepository.GetMasterFHLStatus(masterShipmentPM.Id);
                            masterShipmentPM.IsUpdatedByGLSHKAnalyzer = true;

                            ShipmentService masterService = new ShipmentService(myShipmentContext, masterShipmentPM, systemEmail);
                            masterService.Update();
                        }

                        break;
                    }
            }
        }
        private void AnalyzeMessageQueue_FMA_ISAC()
        {
            shipmentPM.IsUpdatedByGLSHKAnalyzer = true;

            shipmentPM.ManifestReason = null;
            shipmentPM.ManifestStatusCode = "ACCP";

            string systemEmail = "system@tenant" + myTenant + ".com";

            ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
            service.Update();
        }
    }
}
