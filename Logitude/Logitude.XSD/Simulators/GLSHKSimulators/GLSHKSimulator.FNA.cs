using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators.GLSHKSimulators
{
    public partial class GLSHKSimulator
    {
        private void SimulateFNA()
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

            if (args.ReasonForRejection != null)
            {
                args.ReasonForRejection = args.ReasonForRejection.ToUpper();
            }

            this.Message.Item = new GLSHK.FNA()
            {
                ReceivedMessageDetail = myReceivedMessageDetail,
                ReasonforRejectionError = new string[] { args.ReasonForRejection },
            };
        }
    }
}
