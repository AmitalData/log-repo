
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Logitude.Server.Tools.Counters;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestBasesPeriodDataMapping: IMapping<InterestBasesPeriodPM, InterestBasesPeriod>
   {

        public void CustomPMToPOCO(InterestBasesPeriodPM entityPM, InterestBasesPeriod entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            entityPOCO.LineNumber = entityPM.LineNumber;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InterestBaseTypeId);
            entityPOCO.InterestBaseTypeId = entityPM.InterestBaseTypeId;

            InterestBasesPeriodRepository  PeriodRepository = new InterestBasesPeriodRepository(entityPM.Tenant);
            InterestBasesPeriod Period = PeriodRepository.GetSingleByInterestBaseStartDate(entityPM.InterestBaseStartDate);
            if (Period!=null)
            {
                 throw new Exception("Interest Base StartDate Already Exist");
            }

        }

        public void CustomPOCOToPM(InterestBasesPeriodPM entityPM, InterestBasesPeriod entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   