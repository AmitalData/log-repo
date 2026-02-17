

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
   public class CommunicationLogTypeDetails : CommunicationLogType, ICloseTable<CommunicationLogType, CommunicationLogTypeDetails>
   {
       public List<CommunicationLogTypeDetails> GetAll()
       {
		    var all = new List<CommunicationLogTypeDetails>();  
            all.Add(new CommunicationLogTypeDetails()
            {    
                Code = "A", 
                SearchFields = "A,API", 
                Name = "API", 
			});
			 
            all.Add(new CommunicationLogTypeDetails()
            {    
                Code = "DCBK", 
                SearchFields = "DCBK,Document Backup", 
                Name = "Document Backup", 
			});
			 
            all.Add(new CommunicationLogTypeDetails()
            {    
                Code = "E", 
                SearchFields = "E,Email,", 
                Name = "Email", 
			});
			 
            all.Add(new CommunicationLogTypeDetails()
            {    
                Code = "SMS", 
                SearchFields = "SMS,Mobile SMS", 
                Name = "Mobile SMS", 
			});
			 
            all.Add(new CommunicationLogTypeDetails()
            {    
                Code = "Q", 
                SearchFields = "Q,Queue Service", 
                Name = "Queue Service", 
			});
			 
            all.Add(new CommunicationLogTypeDetails()
            {    
                Code = "T", 
                SearchFields = "T,Transmission,", 
                Name = "Transmission", 
			});
			
            return all;
       }

	    public void MapPoco(CommunicationLogType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(CommunicationLogType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

