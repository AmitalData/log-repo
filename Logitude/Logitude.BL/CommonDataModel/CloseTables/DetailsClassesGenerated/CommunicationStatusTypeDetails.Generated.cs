

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.CloseTablesClasses;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs; 
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel
{
   public class CommunicationStatusTypeDetails : CommunicationStatusType, ICloseTable<CommunicationStatusType, CommunicationStatusTypeDetails>
   {
       public List<CommunicationStatusTypeDetails> GetAll()
       {
		    var all = new List<CommunicationStatusTypeDetails>();  
            all.Add(new CommunicationStatusTypeDetails()
            {    
                Code = "D", 
                SearchFields = "D,Done,", 
                Name = "Done", 
			});
			 
            all.Add(new CommunicationStatusTypeDetails()
            {    
                Code = "F", 
                SearchFields = "F,Fail,", 
                Name = "Fail", 
			});
			 
            all.Add(new CommunicationStatusTypeDetails()
            {    
                Code = "P", 
                SearchFields = "P,InProgress", 
                Name = "InProgress", 
			});
			 
            all.Add(new CommunicationStatusTypeDetails()
            {    
                Code = "W", 
                SearchFields = "W,Waiting,", 
                Name = "Waiting", 
			});
			 
            all.Add(new CommunicationStatusTypeDetails()
            {    
                Code = "T", 
                Name = "Time out", 
                SearchFields = "T,Time out", 
			});
			
            return all;
       }

	    public void MapPoco(CommunicationStatusType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(CommunicationStatusType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

