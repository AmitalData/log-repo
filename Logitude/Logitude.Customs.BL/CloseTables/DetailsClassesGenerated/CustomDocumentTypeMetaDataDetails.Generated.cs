
   
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class CustomDocumentTypeMetaDataDetails : CustomDocumentTypeMetaData, ICloseTable<CustomDocumentTypeMetaData, CustomDocumentTypeMetaDataDetails>
   {
       public List<CustomDocumentTypeMetaDataDetails> GetAll()
       {
		    var all = new List<CustomDocumentTypeMetaDataDetails>(); 
            return all;
       }

	    public void MapPoco(CustomDocumentTypeMetaData newPoco)
        {    
        }

		public string GetSearchFields(CustomDocumentTypeMetaData rec)
        {   
           return string.Empty;
        }
		public string Code
        {
            get
            {
                return MetaDataTypeCode;//throw new NotImplementedException();
            }
            set
            {
                MetaDataTypeCode = value;//throw new NotImplementedException();
            }
        }
   }
}

