using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators.CHAMPSimulators
{
    public partial class CHAMPSimulator
    {
        public int Tenant { get; set; }
        private SimulatorArgs args;
        public CHAMP17.Envelope Envelope { get; set; }
        public CHAMPSimulator(SimulatorArgs args, string myTenantParameter, string myAirlineParameter)
        {
            this.args = args;
            this.Tenant = args.Tenant;

            this.Envelope = new CHAMP17.Envelope()
            {
                Sender = myAirlineParameter,
                Recipient = myTenantParameter,
            };
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

                case "FFA":
                    {
                        this.SimulateFFA();
                        break;
                    }

                case "FVA":
                    {
                        this.SimulateFVA();
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
