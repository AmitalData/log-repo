using Logitude.XSD;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;

namespace WebFreight.Web.WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ShipmentToCustomsWebService : System.Web.Services.WebService
    {
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public Shipment Shipment { get; set; }       
        public List<Shipment> AllHouses { get; set; }
        public ShipmentMasterData MasterData { get; set; }

        [WebMethod]
        public void Send(string myShipmentId, int myTenant, string myRecipient)
        {
            this.Tenant = myTenant;
            this.EntityId = myShipmentId;

            XmlSender xmlSender = new XmlSender(myTenant, "CUSEXP");

            if (xmlSender.CCSTypeCode == "GLSHK")
            {
                this.GetShipmentObjects();

                GLSHKCustomsContext dataContext = new GLSHKCustomsContext(Shipment, MasterData, AllHouses, xmlSender.PIMA, myRecipient);
                GLSHKCustomsBuilder dataBuilder = new GLSHKCustomsBuilder(dataContext);

                GLSHK_ISAC.Message myMessage = dataBuilder.GetMessage();

                xmlSender.SetSettings(EntityId, Shipment.ShipmentNumber, "Shipment", false);

                xmlSender.Send(myMessage, "glshkmessageoutqueue");

                MasterData.ManifestReason = null;
                MasterData.ManifestStatusCode = "SENT";
                shipmentRepository.Update(Shipment);
                shipmentRepository.SubmitChanges();
                shipmentMasterDataRepository.Update(MasterData);
                shipmentMasterDataRepository.SubmitChanges();
            }
        }

        ShipmentRepository shipmentRepository;
        ShipmentMasterDataRepository shipmentMasterDataRepository;
        private void GetShipmentObjects()
        {
            IShipmentsContext shipmentContext = ShipmentsContext.GetContext(Tenant);
            shipmentRepository = new ShipmentRepository(shipmentContext);
            shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentContext);

            this.AllHouses = new List<Shipment>();

            Shipment myShipment = shipmentRepository.GetSingleShipment(EntityId, Tenant);
            string myShipmentLevelCode = myShipment.ShipmentLevelCode;
            string myMasterShipmentDataId = myShipment.MasterShipmentDataId;

            if (!string.IsNullOrEmpty(myMasterShipmentDataId))
            {
                this.MasterData = shipmentMasterDataRepository.GetSingleMasterData(myMasterShipmentDataId);

                switch (myShipmentLevelCode)
                {
                    case "D":
                        {
                            this.Shipment = myShipment;
                            break;
                        }

                    case "C":
                        {
                            this.Shipment = myShipment;
                            this.AllHouses = shipmentRepository.GetHouseShipmentsForMaster(EntityId, Tenant);
                            break;
                        }

                    case "H":
                        {
                            this.Shipment = shipmentRepository.GetSingleShipment(myMasterShipmentDataId, Tenant);
                            this.AllHouses.Add(myShipment);                            
                            break;
                        }
                }
            }

        }

    }
}
