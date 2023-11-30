
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
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs; 
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{
   
   public partial class WidgetDataMapping: IMapping<WidgetPM, Widget>,IMappingEncodeBase64NVARCHARFields<WidgetPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Title, 
	         GroupById, 
	         DashboardId, 
	         StartPotistion, 
	         EndPosition, 
	         TypeCode, 
	         EntityId, 
	         Filters, 
	         DateGroupCode, 
	         MaximumGrouping, 
	         SortBy, 
	         SortDirection, 
	         TimeOverTime, 
	         ComparisonPeriod, 
	         Increase, 
	         ComparisonOperator, 
	         ComparisonDateGroup, 
	         FromDate, 
	         ToDate, 
	         SecondaryGroupById, 
	         SecondaryDateGroupCode, 
	         Alignment, 
	         ThousandSeparator, 
	         UseNumberAbbreviation, 
	         DecimalPlaces, 
	         UseAbbreviationAfter, 
	         LabelsPosition,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Title, 
	         GroupById, 
	         DashboardId, 
	         StartPotistion, 
	         EndPosition, 
	         TypeCode, 
	         EntityId, 
	         Filters, 
	         DateGroupCode, 
	         MaximumGrouping, 
	         SortBy, 
	         SortDirection, 
	         Key, 
	         TimeOverTime, 
	         ComparisonPeriod, 
	         Increase, 
	         ComparisonOperator, 
	         ComparisonDateGroup, 
	         FromDate, 
	         ToDate, 
	         GlobalFilters, 
	         SecondaryGroupById, 
	         SecondaryDateGroupCode, 
	         Alignment, 
	         ThousandSeparator, 
	         UseNumberAbbreviation, 
	         DecimalPlaces, 
	         UseAbbreviationAfter, 
	         LabelsPosition,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WidgetPM entityPM, Widget entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
				entityPOCO.Title = entityPM.Title;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupById))
            {
				entityPOCO.GroupById = entityPM.GroupById;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DashboardId))
            {
				entityPOCO.DashboardId = entityPM.DashboardId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartPotistion))
            {
				entityPOCO.StartPotistion = entityPM.StartPotistion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndPosition))
            {
				entityPOCO.EndPosition = entityPM.EndPosition;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
				entityPOCO.TypeCode = entityPM.TypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Filters))
            {
				entityPOCO.Filters = entityPM.Filters;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DateGroupCode))
            {
				entityPOCO.DateGroupCode = entityPM.DateGroupCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MaximumGrouping))
            {
				entityPOCO.MaximumGrouping = entityPM.MaximumGrouping;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SortBy))
            {
				entityPOCO.SortBy = entityPM.SortBy;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SortDirection))
            {
				entityPOCO.SortDirection = entityPM.SortDirection;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TimeOverTime))
            {
				entityPOCO.TimeOverTime = entityPM.TimeOverTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComparisonPeriod))
            {
				entityPOCO.ComparisonPeriod = entityPM.ComparisonPeriod;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Increase))
            {
				entityPOCO.Increase = entityPM.Increase;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComparisonOperator))
            {
				entityPOCO.ComparisonOperator = entityPM.ComparisonOperator;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComparisonDateGroup))
            {
				entityPOCO.ComparisonDateGroup = entityPM.ComparisonDateGroup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromDate))
            {
				entityPOCO.FromDate = entityPM.FromDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDate))
            {
				entityPOCO.ToDate = entityPM.ToDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondaryGroupById))
            {
				entityPOCO.SecondaryGroupById = entityPM.SecondaryGroupById;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondaryDateGroupCode))
            {
				entityPOCO.SecondaryDateGroupCode = entityPM.SecondaryDateGroupCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Alignment))
            {
				entityPOCO.Alignment = entityPM.Alignment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThousandSeparator))
            {
				entityPOCO.ThousandSeparator = entityPM.ThousandSeparator;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UseNumberAbbreviation))
            {
				entityPOCO.UseNumberAbbreviation = entityPM.UseNumberAbbreviation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DecimalPlaces))
            {
				entityPOCO.DecimalPlaces = entityPM.DecimalPlaces;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UseAbbreviationAfter))
            {
				entityPOCO.UseAbbreviationAfter = entityPM.UseAbbreviationAfter;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LabelsPosition))
            {
				entityPOCO.LabelsPosition = entityPM.LabelsPosition;
			}
			}

		public void POCOToPM(WidgetPM entityPM, Widget entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Title))
            {
					entityPM.Title = entityPOCO.Title;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GroupById))
            {
					entityPM.GroupById = entityPOCO.GroupById;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DashboardId))
            {
					entityPM.DashboardId = entityPOCO.DashboardId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartPotistion))
            {
					entityPM.StartPotistion = entityPOCO.StartPotistion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndPosition))
            {
					entityPM.EndPosition = entityPOCO.EndPosition;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TypeCode))
            {
					entityPM.TypeCode = entityPOCO.TypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Filters))
            {
					entityPM.Filters = entityPOCO.Filters;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DateGroupCode))
            {
					entityPM.DateGroupCode = entityPOCO.DateGroupCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MaximumGrouping))
            {
					entityPM.MaximumGrouping = entityPOCO.MaximumGrouping;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SortBy))
            {
					entityPM.SortBy = entityPOCO.SortBy;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SortDirection))
            {
					entityPM.SortDirection = entityPOCO.SortDirection;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TimeOverTime))
            {
					entityPM.TimeOverTime = entityPOCO.TimeOverTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ComparisonPeriod))
            {
					entityPM.ComparisonPeriod = entityPOCO.ComparisonPeriod;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Increase))
            {
					entityPM.Increase = entityPOCO.Increase;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ComparisonOperator))
            {
					entityPM.ComparisonOperator = entityPOCO.ComparisonOperator;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ComparisonDateGroup))
            {
					entityPM.ComparisonDateGroup = entityPOCO.ComparisonDateGroup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromDate))
            {
					entityPM.FromDate = entityPOCO.FromDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToDate))
            {
					entityPM.ToDate = entityPOCO.ToDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecondaryGroupById))
            {
					entityPM.SecondaryGroupById = entityPOCO.SecondaryGroupById;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecondaryDateGroupCode))
            {
					entityPM.SecondaryDateGroupCode = entityPOCO.SecondaryDateGroupCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Alignment))
            {
					entityPM.Alignment = entityPOCO.Alignment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ThousandSeparator))
            {
					entityPM.ThousandSeparator = entityPOCO.ThousandSeparator;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UseNumberAbbreviation))
            {
					entityPM.UseNumberAbbreviation = entityPOCO.UseNumberAbbreviation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DecimalPlaces))
            {
					entityPM.DecimalPlaces = entityPOCO.DecimalPlaces;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UseAbbreviationAfter))
            {
					entityPM.UseAbbreviationAfter = entityPOCO.UseAbbreviationAfter;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LabelsPosition))
            {
					entityPM.LabelsPosition = entityPOCO.LabelsPosition;
            }

		}

		public void PMToOldPM(WidgetPM entityPM, WidgetPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
                oldEntityPM.Title = entityPM.Title;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupById))
            {
                oldEntityPM.GroupById = entityPM.GroupById;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DashboardId))
            {
                oldEntityPM.DashboardId = entityPM.DashboardId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartPotistion))
            {
                oldEntityPM.StartPotistion = entityPM.StartPotistion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndPosition))
            {
                oldEntityPM.EndPosition = entityPM.EndPosition;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
                oldEntityPM.TypeCode = entityPM.TypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Filters))
            {
                oldEntityPM.Filters = entityPM.Filters;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DateGroupCode))
            {
                oldEntityPM.DateGroupCode = entityPM.DateGroupCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MaximumGrouping))
            {
                oldEntityPM.MaximumGrouping = entityPM.MaximumGrouping;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SortBy))
            {
                oldEntityPM.SortBy = entityPM.SortBy;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SortDirection))
            {
                oldEntityPM.SortDirection = entityPM.SortDirection;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TimeOverTime))
            {
                oldEntityPM.TimeOverTime = entityPM.TimeOverTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComparisonPeriod))
            {
                oldEntityPM.ComparisonPeriod = entityPM.ComparisonPeriod;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Increase))
            {
                oldEntityPM.Increase = entityPM.Increase;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComparisonOperator))
            {
                oldEntityPM.ComparisonOperator = entityPM.ComparisonOperator;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComparisonDateGroup))
            {
                oldEntityPM.ComparisonDateGroup = entityPM.ComparisonDateGroup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromDate))
            {
                oldEntityPM.FromDate = entityPM.FromDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDate))
            {
                oldEntityPM.ToDate = entityPM.ToDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondaryGroupById))
            {
                oldEntityPM.SecondaryGroupById = entityPM.SecondaryGroupById;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondaryDateGroupCode))
            {
                oldEntityPM.SecondaryDateGroupCode = entityPM.SecondaryDateGroupCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Alignment))
            {
                oldEntityPM.Alignment = entityPM.Alignment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThousandSeparator))
            {
                oldEntityPM.ThousandSeparator = entityPM.ThousandSeparator;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UseNumberAbbreviation))
            {
                oldEntityPM.UseNumberAbbreviation = entityPM.UseNumberAbbreviation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DecimalPlaces))
            {
                oldEntityPM.DecimalPlaces = entityPM.DecimalPlaces;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UseAbbreviationAfter))
            {
                oldEntityPM.UseAbbreviationAfter = entityPM.UseAbbreviationAfter;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LabelsPosition))
            {
                oldEntityPM.LabelsPosition = entityPM.LabelsPosition;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WidgetPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Title)) //T4 find type == nText 
            {
                entityPM.Title = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Title));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.StartPotistion)) //T4 find type == nText 
            {
                entityPM.StartPotistion = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.StartPotistion));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Filters)) //T4 find type == nText 
            {
                entityPM.Filters = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Filters));
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
	 