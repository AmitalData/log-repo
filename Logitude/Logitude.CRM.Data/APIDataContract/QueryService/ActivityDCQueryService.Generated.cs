using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.CRM.Data.EntityPOCOs;

 namespace Logitude.CRM.Data.APIDataContract
{ 
   public class ActivityDCQueryService
   {
		ICRMContext  context;
        public ActivityDCQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant); 
        }

		
		public ActivityDC GetActivityDCById(string Id)
        { 
            var temp = (from a in context.Activities where a.Id == Id select a).FirstOrDefault();
			return ActivityDCDataMapping(temp);
        }
		
		public ActivityDC ActivityDCDataMapping(Activity MyPoco)
        {
		       return new ActivityDC(){
			   
				Id = MyPoco.Id,
						  
				Subject = MyPoco.Subject,
						  
				Date = MyPoco.DueDate,
						  
			   };   
        }
		 
   }
}
