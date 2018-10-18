
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
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class BookingPackageDataMapping: IMapping<BookingPackagePM, BookingPackage>,IMappingEncodeBase64NVARCHARFields<BookingPackagePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BookingId, 
	         Description, 
	         PackageTypeId, 
	         ContainerNumber, 
	         Seal, 
	         Quantity, 
	         Weight, 
	         Volume, 
	         Tare, 
	         Height, 
	         Width, 
	         Length, 
	         UnNumber, 
	         ClassNumber, 
	         Temperature, 
	         Ventilation, 
	         Seal2, 
	         SOC, 
	         MarksAndNumbers, 
	         PackagingGroup, 
	         IMDGCode, 
	         FlashPoint, 
	         Harmonize, 
	         MaterialDescription, 
	         IsDangerous, 
	         OriginalBookingPackageId, 
	         VolumetricWeight, 
	         CommodityId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BookingId, 
	         Description, 
	         PackageTypeId, 
	         ContainerNumber, 
	         Seal, 
	         Quantity, 
	         Weight, 
	         Volume, 
	         Tare, 
	         Height, 
	         Width, 
	         Length, 
	         UnNumber, 
	         ClassNumber, 
	         Temperature, 
	         Ventilation, 
	         Seal2, 
	         SOC, 
	         MarksAndNumbers, 
	         PackagingGroup, 
	         IMDGCode, 
	         FlashPoint, 
	         Harmonize, 
	         MaterialDescription, 
	         IsDangerous, 
	         OriginalBookingPackageId, 
	         VolumetricWeight, 
	         CommodityId, 
	         PackageTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(BookingPackagePM entityPM, BookingPackage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingId))
            {
				entityPOCO.BookingId = entityPM.BookingId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeId))
            {
				entityPOCO.PackageTypeId = entityPM.PackageTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerNumber))
            {
				entityPOCO.ContainerNumber = entityPM.ContainerNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Seal))
            {
				entityPOCO.Seal = entityPM.Seal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
				entityPOCO.Quantity = entityPM.Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
				entityPOCO.Weight = entityPM.Weight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
				entityPOCO.Volume = entityPM.Volume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tare))
            {
				entityPOCO.Tare = entityPM.Tare;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Height))
            {
				entityPOCO.Height = entityPM.Height;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Width))
            {
				entityPOCO.Width = entityPM.Width;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Length))
            {
				entityPOCO.Length = entityPM.Length;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnNumber))
            {
				entityPOCO.UnNumber = entityPM.UnNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassNumber))
            {
				entityPOCO.ClassNumber = entityPM.ClassNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Temperature))
            {
				entityPOCO.Temperature = entityPM.Temperature;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ventilation))
            {
				entityPOCO.Ventilation = entityPM.Ventilation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Seal2))
            {
				entityPOCO.Seal2 = entityPM.Seal2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SOC))
            {
				entityPOCO.SOC = entityPM.SOC;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarksAndNumbers))
            {
				entityPOCO.MarksAndNumbers = entityPM.MarksAndNumbers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackagingGroup))
            {
				entityPOCO.PackagingGroup = entityPM.PackagingGroup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IMDGCode))
            {
				entityPOCO.IMDGCode = entityPM.IMDGCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlashPoint))
            {
				entityPOCO.FlashPoint = entityPM.FlashPoint;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Harmonize))
            {
				entityPOCO.Harmonize = entityPM.Harmonize;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MaterialDescription))
            {
				entityPOCO.MaterialDescription = entityPM.MaterialDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDangerous))
            {
				entityPOCO.IsDangerous = entityPM.IsDangerous;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalBookingPackageId))
            {
				entityPOCO.OriginalBookingPackageId = entityPM.OriginalBookingPackageId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
				entityPOCO.VolumetricWeight = entityPM.VolumetricWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommodityId))
            {
				entityPOCO.CommodityId = entityPM.CommodityId;
			}
			}

		public void POCOToPM(BookingPackagePM entityPM, BookingPackage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingId))
            {
					entityPM.BookingId = entityPOCO.BookingId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageTypeId))
            {
					entityPM.PackageTypeId = entityPOCO.PackageTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerNumber))
            {
					entityPM.ContainerNumber = entityPOCO.ContainerNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Seal))
            {
					entityPM.Seal = entityPOCO.Seal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Quantity))
            {
					entityPM.Quantity = entityPOCO.Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Weight))
            {
					entityPM.Weight = entityPOCO.Weight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Volume))
            {
					entityPM.Volume = entityPOCO.Volume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tare))
            {
					entityPM.Tare = entityPOCO.Tare;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Height))
            {
					entityPM.Height = entityPOCO.Height;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Width))
            {
					entityPM.Width = entityPOCO.Width;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Length))
            {
					entityPM.Length = entityPOCO.Length;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UnNumber))
            {
					entityPM.UnNumber = entityPOCO.UnNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClassNumber))
            {
					entityPM.ClassNumber = entityPOCO.ClassNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Temperature))
            {
					entityPM.Temperature = entityPOCO.Temperature;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Ventilation))
            {
					entityPM.Ventilation = entityPOCO.Ventilation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Seal2))
            {
					entityPM.Seal2 = entityPOCO.Seal2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SOC))
            {
					entityPM.SOC = entityPOCO.SOC;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MarksAndNumbers))
            {
					entityPM.MarksAndNumbers = entityPOCO.MarksAndNumbers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackagingGroup))
            {
					entityPM.PackagingGroup = entityPOCO.PackagingGroup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IMDGCode))
            {
					entityPM.IMDGCode = entityPOCO.IMDGCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FlashPoint))
            {
					entityPM.FlashPoint = entityPOCO.FlashPoint;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Harmonize))
            {
					entityPM.Harmonize = entityPOCO.Harmonize;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MaterialDescription))
            {
					entityPM.MaterialDescription = entityPOCO.MaterialDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDangerous))
            {
					entityPM.IsDangerous = entityPOCO.IsDangerous;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginalBookingPackageId))
            {
					entityPM.OriginalBookingPackageId = entityPOCO.OriginalBookingPackageId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumetricWeight))
            {
					entityPM.VolumetricWeight = entityPOCO.VolumetricWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommodityId))
            {
					entityPM.CommodityId = entityPOCO.CommodityId;
            }

		}

		public void PMToOldPM(BookingPackagePM entityPM, BookingPackagePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingId))
            {
                oldEntityPM.BookingId = entityPM.BookingId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeId))
            {
                oldEntityPM.PackageTypeId = entityPM.PackageTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerNumber))
            {
                oldEntityPM.ContainerNumber = entityPM.ContainerNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Seal))
            {
                oldEntityPM.Seal = entityPM.Seal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
                oldEntityPM.Quantity = entityPM.Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
                oldEntityPM.Weight = entityPM.Weight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
                oldEntityPM.Volume = entityPM.Volume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tare))
            {
                oldEntityPM.Tare = entityPM.Tare;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Height))
            {
                oldEntityPM.Height = entityPM.Height;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Width))
            {
                oldEntityPM.Width = entityPM.Width;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Length))
            {
                oldEntityPM.Length = entityPM.Length;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnNumber))
            {
                oldEntityPM.UnNumber = entityPM.UnNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassNumber))
            {
                oldEntityPM.ClassNumber = entityPM.ClassNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Temperature))
            {
                oldEntityPM.Temperature = entityPM.Temperature;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ventilation))
            {
                oldEntityPM.Ventilation = entityPM.Ventilation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Seal2))
            {
                oldEntityPM.Seal2 = entityPM.Seal2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SOC))
            {
                oldEntityPM.SOC = entityPM.SOC;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarksAndNumbers))
            {
                oldEntityPM.MarksAndNumbers = entityPM.MarksAndNumbers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackagingGroup))
            {
                oldEntityPM.PackagingGroup = entityPM.PackagingGroup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IMDGCode))
            {
                oldEntityPM.IMDGCode = entityPM.IMDGCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlashPoint))
            {
                oldEntityPM.FlashPoint = entityPM.FlashPoint;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Harmonize))
            {
                oldEntityPM.Harmonize = entityPM.Harmonize;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MaterialDescription))
            {
                oldEntityPM.MaterialDescription = entityPM.MaterialDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDangerous))
            {
                oldEntityPM.IsDangerous = entityPM.IsDangerous;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalBookingPackageId))
            {
                oldEntityPM.OriginalBookingPackageId = entityPM.OriginalBookingPackageId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
                oldEntityPM.VolumetricWeight = entityPM.VolumetricWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommodityId))
            {
                oldEntityPM.CommodityId = entityPM.CommodityId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(BookingPackagePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
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
	 