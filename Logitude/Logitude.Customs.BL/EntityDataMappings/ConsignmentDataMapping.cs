
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ConsignmentDataMapping: IMapping<ConsignmentPM, Consignment>
   {

        public void CustomPMToPOCO(ConsignmentPM entityPM, Consignment entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConsignmentNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
              
                entityPOCO.DeclarationId = entityPM.DeclarationId;                
                entityPOCO.ConsignmentNumber = entityPM.ConsignmentNumber;          
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(ConsignmentPM entityPM, Consignment entityPOCO)
        {
            if (entityPOCO.CargoTypeCode == "17")
            {
                if (!string.IsNullOrEmpty(entityPOCO.ThirdCargoID))
                {
                    try
                    {
                        int day = int.Parse(entityPOCO.ThirdCargoID.Substring(0, 2));
                        int month = int.Parse(entityPOCO.ThirdCargoID.Substring(2, 2));
                        int year = int.Parse(entityPOCO.ThirdCargoID.Substring(4, 2));

                        int currentYear = DateTime.Now.Year;
                        string currentYearMillinium = currentYear.ToString().Substring(0, 1);
                        currentYearMillinium = currentYearMillinium + "000";
                        int currentMillinium;
                        int.TryParse(currentYearMillinium, out currentMillinium);
                        if (year == 0)
                        {
                            year = currentYear;
                        }
                        if (year < 1000)
                        {
                            year = year + currentMillinium;
                        }

                        entityPM.CargoDate = new DateTime(year, month, day);
                    }
                    catch //(Exception)
                    {

                        //throw;
                    }

                }
            }
            if(entityPOCO.StorageSiteCode != null)
            {
                entityPM.DeliverySiteCode = entityPOCO.StorageSiteCode;
            }
        }
    }


}
   