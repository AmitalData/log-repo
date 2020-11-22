using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.Initializers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.Behaviours.QuoteBehaviours
{
    public class QuoteFieldsBehaviour : IServiceBehaviour
    {
        private QuotePM entityPM;

        private QuoteServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (QuoteServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                GetProductCode();
            }
        }

        private void GetProductCode()
        {
            if (entityPM.DirectionId == "C")
            {
                entityPM.ProductCode = "CI";
            }

            else
            {
                entityPM.ProductCode = entityPM.TransportModeId + entityPM.DirectionId;
            }
        }
    }
}
