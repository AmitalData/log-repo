using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators.GLSHKSimulators
{
    public partial class GLSHKSimulator
    {
        public int Tenant { get; set; }
        private SimulatorArgs args;
        public GLSHK.Message Message { get; set; }
        public GLSHKSimulator(SimulatorArgs args, string myTenantParameter, string myAirlineParameter)
        {
            this.args = args;
            this.Tenant = args.Tenant;

            string myFullRefID = "HMF" + args.Master + "X" + args.AirlinePrefix;

            string myShipmentNumber = args.ShipmentNumber;
            if (myShipmentNumber != null)
            {
                if (myShipmentNumber.Length > 14)
                {
                    myShipmentNumber = myShipmentNumber.Substring(0, 14);
                }
            }

            this.Message = new GLSHK.Message()
            {
                version = GLSHK.MessageVersion.Item20,

                Envelope = new GLSHK.Envelope()
                {
                    SenderID = myAirlineParameter,
                    RecipientID = myTenantParameter,
                    MsgFormat = "XML",
                    MsgType = "CIM" + args.MessageIdentifier,
                    Version = 17,
                    MsgDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant),
                    RefID = myFullRefID,
                    MessageRefNum = myShipmentNumber,
                    InterchangeControlRef = myShipmentNumber,
                    Item = Tenant.ToString(),
                    ItemElementName = GLSHK.ItemChoiceType4.CompanyID,
                },
            };

            if (args.IsViaColoader && !string.IsNullOrEmpty(args.ColoaderReference))
            {
                string myColoaderKey = FormatHelper.FormatString(args.ColoaderReference, FormatHelper.PatternType.Text, 50);
                this.Message.Envelope.Item = myColoaderKey;
                this.Message.Envelope.ItemElementName = GLSHK.ItemChoiceType4.ColoaderKey;
            }

            if (args.IsGLSHKISAC)
            {
                if (args.MessageIdentifier == "FNA" || args.MessageIdentifier == "FMA")
                {
                    this.Message.Envelope.SenderID = args.ISAC_Sender;
                }
            }
        }

        public void Run()
        {
            switch (args.MessageIdentifier)
            {
                case "FMA":
                    {
                        this.SimulateFMA();
                        break;
                    }

                case "FNA":
                    {
                        this.SimulateFNA();
                        break;
                    }

                case "FSA":
                case "FSU":
                    {
                        this.SimulateFSA();
                        break;
                    }
            }
        }

        private string GetPortCode(string myPortId)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(myPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(Tenant, myPortId, true);
                if (myPort != null)
                {
                    if (myPort.Code != null)
                    {
                        myResult = myPort.Code.ToUpper();
                    }
                }
            }

            return myResult;
        }

        private string GetCardCode(string myCardId)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(myCardId))
            {
                Card myCard = CardRepository.GetSingleCard(myCardId, Tenant, true);
                if (myCard != null)
                {
                    if (myCard.Code != null)
                    {
                        myResult = myCard.Code.ToUpper();
                    }
                }
            }

            return myResult;
        }
    }
}
