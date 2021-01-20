

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
   public class DocumentTypeCategoryDetails : DocumentTypeCategory, ICloseTable<DocumentTypeCategory, DocumentTypeCategoryDetails>
   {
       public List<DocumentTypeCategoryDetails> GetAll()
       {
		    var all = new List<DocumentTypeCategoryDetails>();  
            all.Add(new DocumentTypeCategoryDetails()
            {    
                Code = "A", 
                SearchFields = "A,Accounting Documents", 
                Name = "Accounting Documents", 
			});
			 
            all.Add(new DocumentTypeCategoryDetails()
            {    
                Code = "O", 
                SearchFields = "O,Others", 
                Name = "Others", 
			});
			 
            all.Add(new DocumentTypeCategoryDetails()
            {    
                Code = "P", 
                SearchFields = "P,Operational Documents", 
                Name = "Operational Documents", 
			});
			 
            all.Add(new DocumentTypeCategoryDetails()
            {    
                Code = "E", 
                Name = "Export Customs", 
                SearchFields = "E,Export Customs", 
			});
			
            return all;
       }

	    public void MapPoco(DocumentTypeCategory newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(DocumentTypeCategory rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

