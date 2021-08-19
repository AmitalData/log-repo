using Amital.QuoteOPM.BL.Tools.Initializers;
using Amital.QuoteOPM.Def.EntityPMs;
using Simplog.Server.Infrastructure.Interfaces;

namespace Amital.QuoteOPM.BL.Tools.Behaviours.QuoteBehaviours
{
    public class QuoteOPFieldsBehaviour : IServiceBehaviour
    {
        private QuoteOPPM entityPM;

        private QuoteOPServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (QuoteOPServiceInitializer)initializer;
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
