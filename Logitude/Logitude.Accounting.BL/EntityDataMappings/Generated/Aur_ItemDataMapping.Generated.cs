
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class Aur_ItemDataMapping: IMapping<Aur_ItemPM, Aur_Item>,IMappingEncodeBase64NVARCHARFields<Aur_ItemPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         PaymentId, 
	         Line, 
	         SalesOrderid, 
	         RelatedContract, 
	         ProductNumber, 
	         ProductName, 
	         PricePerUnit, 
	         Quantity, 
	         Discount, 
	         BaseAmount, 
	         Tax, 
	         ExtendedAmount, 
	         Tenant,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         PaymentId, 
	         Line, 
	         SalesOrderid, 
	         RelatedContract, 
	         ProductNumber, 
	         ProductName, 
	         PricePerUnit, 
	         Quantity, 
	         Discount, 
	         BaseAmount, 
	         Tax, 
	         ExtendedAmount, 
	         Tenant,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(Aur_ItemPM entityPM, Aur_Item entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SalesOrderid))
            {
				entityPOCO.SalesOrderid = entityPM.SalesOrderid;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RelatedContract))
            {
				entityPOCO.RelatedContract = entityPM.RelatedContract;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProductNumber))
            {
				entityPOCO.ProductNumber = entityPM.ProductNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProductName))
            {
				entityPOCO.ProductName = entityPM.ProductName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PricePerUnit))
            {
				entityPOCO.PricePerUnit = entityPM.PricePerUnit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
				entityPOCO.Quantity = entityPM.Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Discount))
            {
				entityPOCO.Discount = entityPM.Discount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BaseAmount))
            {
				entityPOCO.BaseAmount = entityPM.BaseAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tax))
            {
				entityPOCO.Tax = entityPM.Tax;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExtendedAmount))
            {
				entityPOCO.ExtendedAmount = entityPM.ExtendedAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(Aur_ItemPM entityPM, Aur_Item entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentId))
            {
					entityPM.PaymentId = entityPOCO.PaymentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SalesOrderid))
            {
					entityPM.SalesOrderid = entityPOCO.SalesOrderid;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RelatedContract))
            {
					entityPM.RelatedContract = entityPOCO.RelatedContract;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProductNumber))
            {
					entityPM.ProductNumber = entityPOCO.ProductNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProductName))
            {
					entityPM.ProductName = entityPOCO.ProductName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PricePerUnit))
            {
					entityPM.PricePerUnit = entityPOCO.PricePerUnit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Quantity))
            {
					entityPM.Quantity = entityPOCO.Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Discount))
            {
					entityPM.Discount = entityPOCO.Discount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BaseAmount))
            {
					entityPM.BaseAmount = entityPOCO.BaseAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tax))
            {
					entityPM.Tax = entityPOCO.Tax;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExtendedAmount))
            {
					entityPM.ExtendedAmount = entityPOCO.ExtendedAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

		}

		public void PMToOldPM(Aur_ItemPM entityPM, Aur_ItemPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SalesOrderid))
            {
                oldEntityPM.SalesOrderid = entityPM.SalesOrderid;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RelatedContract))
            {
                oldEntityPM.RelatedContract = entityPM.RelatedContract;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProductNumber))
            {
                oldEntityPM.ProductNumber = entityPM.ProductNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProductName))
            {
                oldEntityPM.ProductName = entityPM.ProductName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PricePerUnit))
            {
                oldEntityPM.PricePerUnit = entityPM.PricePerUnit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
                oldEntityPM.Quantity = entityPM.Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Discount))
            {
                oldEntityPM.Discount = entityPM.Discount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BaseAmount))
            {
                oldEntityPM.BaseAmount = entityPM.BaseAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tax))
            {
                oldEntityPM.Tax = entityPM.Tax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExtendedAmount))
            {
                oldEntityPM.ExtendedAmount = entityPM.ExtendedAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(Aur_ItemPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SalesOrderid)) //T4 find type == nText 
            {
                entityPM.SalesOrderid = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SalesOrderid));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RelatedContract)) //T4 find type == nText 
            {
                entityPM.RelatedContract = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RelatedContract));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ProductNumber)) //T4 find type == nText 
            {
                entityPM.ProductNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ProductNumber));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ProductName)) //T4 find type == nText 
            {
                entityPM.ProductName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ProductName));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
			  
   }
}
	 