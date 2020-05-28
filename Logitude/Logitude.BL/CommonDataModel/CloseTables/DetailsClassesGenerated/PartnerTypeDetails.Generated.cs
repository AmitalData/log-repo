

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
   public class PartnerTypeDetails : PartnerType, ICloseTable<PartnerType, PartnerTypeDetails>
   {
       public List<PartnerTypeDetails> GetAll()
       {
		    var all = new List<PartnerTypeDetails>();  
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "ag,agent", 
                Id = "AG", 
                Name = "Agent", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "al,airline", 
                Id = "AL", 
                Name = "AirLine", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "cc,custom clearance", 
                Id = "CC", 
                Name = "Custom Clearance", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "cg,custom agent", 
                Id = "CG", 
                Name = "Custom Agent", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "co,coloader", 
                Id = "CO", 
                Name = "Coloader", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "cs,customer", 
                Id = "CS", 
                Name = "Customer", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "fl,freelancer", 
                Id = "FL", 
                Name = "Freelancer", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "ot,others", 
                Id = "OT", 
                Name = "Others", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "po,potential customer", 
                Id = "PO", 
                Name = "Potential Customer", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "pt,participant", 
                Id = "PT", 
                Name = "Participant", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "sg,shipping agent", 
                Id = "SG", 
                Name = "Shipping Agent", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "sl,shipping line", 
                Id = "SL", 
                Name = "Shipping Line", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "tr,trucker", 
                Id = "TR", 
                Name = "Trucker", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "vd,vendor", 
                Id = "VD", 
                Name = "Vendor", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "wh,warehouse", 
                Id = "WH", 
                Name = "Warehouse", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                SearchFields = "ch,customs shipper", 
                Id = "CH", 
                Name = "Customs Shipper", 
			});
			 
            all.Add(new PartnerTypeDetails()
            {    
                Id = "AC", 
                Name = "Accounting Partner", 
                SearchFields = "AC,Accounting Partner", 
			});
			
            return all;
       }

	    public void MapPoco(PartnerType newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Id = this.Id;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(PartnerType rec)
        {   
           return String.Concat(rec.Id,",",rec.Name,",");
        }
		public string Code
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }
   }
}

