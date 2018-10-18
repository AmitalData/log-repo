using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators.CHAMPSimulators
{
    public partial class CHAMPSimulator
    {
        private void SimulateFNA()
        {
            string myReceivedMessageDetail = null;

            if (args.EntityName == "Shipment")
            {
                if (args.ShipmentLevelCode == "H")
                {
                    myReceivedMessageDetail = "FHL" + Environment.NewLine + "MBI/" + args.AirlinePrefix + "-" + args.Master + Environment.NewLine + "HBS/" + args.House;
                }

                else
                {
                    myReceivedMessageDetail = "FWB" + args.AirlinePrefix + "-" + args.Master;
                }
            }

            else if (args.EntityName == "Booking")
            {
                myReceivedMessageDetail = "FFR" + args.AirlinePrefix + "-" + args.Master;
            }

            if (args.ReasonForRejection != null)
            {
                args.ReasonForRejection = args.ReasonForRejection.ToUpper();
            }

            this.Envelope.Item = new CHAMP17.ErrorMessage()
            {
                StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
                {
                    StandardMessageIdentifier = "FNA",
                    MessageTypeVersionNumber = 1,
                },

                ReasonForRejection = new string[] { args.ReasonForRejection },
                ReceivedMessageDetail = myReceivedMessageDetail,
            };
        }
    }
}
