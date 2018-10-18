using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators.GLSHKSimulators
{
    public partial class GLSHKSimulator
    {
        private void SimulateFMA()
        {
            string myReceivedMessageDetail = null;

            if (args.ShipmentLevelCode == "H")
            {
                myReceivedMessageDetail = "FHL" + Environment.NewLine + "MBI/" + args.AirlinePrefix + "-" + args.Master + Environment.NewLine + "HBS/" + args.House;
            }

            else
            {
                myReceivedMessageDetail = "FWB" + args.AirlinePrefix + "-" + args.Master;
            }

            if (args.ReasonForAcknowledgement != null)
            {
                args.ReasonForAcknowledgement = args.ReasonForAcknowledgement.ToUpper();
            }

            this.Message.Item = new GLSHK.FMA()
            {
                ReceivedMessageDetail = myReceivedMessageDetail,
                ReasonforAcknowledgement = new string[] { args.ReasonForAcknowledgement },
            };
        }
    }
}
