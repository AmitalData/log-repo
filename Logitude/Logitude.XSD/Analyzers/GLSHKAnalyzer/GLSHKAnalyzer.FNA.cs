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
        private GLSHK.FNA myFNA;
        private void AnalyzeBaseData_FNA()
        {
            this.myFNA = (GLSHK.FNA)myMessage.Item;           
            string myReceivedMessageDetail = myFNA.ReceivedMessageDetail;

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

                case "FSR":
                    {
                        string[] messageDetails = myReceivedMessageDetail.Split(new string[] { }, StringSplitOptions.RemoveEmptyEntries);
                        if (messageDetails.Count() > 0)
                        {
                            string master = messageDetails[1];

                            string[] PrefixAndAWB = master.Split('-');
                            myPrefix = PrefixAndAWB[0];
                            myMaster = PrefixAndAWB[1];
                        }

                        else
                        {
                            throw new Exception("Message details not recognized for analyzing fna for fsa ");
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

        private void AnalyzeMessageQueue_FNA()
        {
            string[] myReasonforRejectionError = myFNA.ReasonforRejectionError;

            shipmentPM.IsUpdatedByGLSHKAnalyzer = true;

            shipmentPM.FNAReason = null;
            shipmentPM.CarrierLastStatusCode = "FNA";
            shipmentPM.CarrierLastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
            string systemEmail = "system@tenant" + myTenant + ".com";

            foreach (string reason in myReasonforRejectionError)
            {
                if (shipmentPM.FNAReason == null)
                {
                    shipmentPM.FNAReason = reason;
                }

                else
                {
                    shipmentPM.FNAReason = shipmentPM.FNAReason + ',' + reason;
                }
            }

            if (shipmentPM.FNAReason.Length > 249)
            {
                shipmentPM.FNAReason = shipmentPM.FNAReason.Substring(0, 249);
            }

            switch (myMessageDetailCode)
            {
                case "FWB":
                case "AWB":
                    {
                        shipmentPM.FWBStatusCode = "EROR";
                        shipmentPM.FWBStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

                        ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
                        service.Update();

                        break;
                    }

                case "FHL":
                    {
                        shipmentPM.FHLStatusCode = "EROR";
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

                case "FSR":
                    {
                        break;
                    }
            }
        }
        private void AnalyzeMessageQueue_FNA_ISAC()
        {
            shipmentPM.IsUpdatedByGLSHKAnalyzer = true;

            shipmentPM.ManifestReason = null;
            shipmentPM.ManifestStatusCode = "DECL";

            this.myFNA = (GLSHK.FNA)myMessage.Item;
            
            string[] myReasonforRejectionError = myFNA.ReasonforRejectionError;

            string systemEmail = "system@tenant" + myTenant + ".com";

            foreach (string reason in myReasonforRejectionError)
            {
                if (shipmentPM.ManifestReason == null)
                {
                    shipmentPM.ManifestReason = reason;
                }

                else
                {
                    shipmentPM.ManifestReason = shipmentPM.ManifestReason + ',' + reason;
                }
            }

            if (shipmentPM.ManifestReason.Length > 249)
            {
                shipmentPM.ManifestReason = shipmentPM.ManifestReason.Substring(0, 249);
            }

            ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
            service.Update();
        }
    }
}
