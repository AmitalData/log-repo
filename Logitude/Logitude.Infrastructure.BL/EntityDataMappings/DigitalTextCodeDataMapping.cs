using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{

    public partial class DigitalTextCodeDataMapping: IMapping<DigitalTextCodePM, DigitalTextCode>
   {
        public void CustomPMToPOCO(DigitalTextCodePM entityPM, DigitalTextCode entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert) 
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant; 
            }
        }

        public void CustomPOCOToPM(DigitalTextCodePM entityPM, DigitalTextCode entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   