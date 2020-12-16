using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.CompositionBehaviours
{
    public class OrderPackagesBehaviour: IServiceBehaviour
    {
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            
        }
    }
}
