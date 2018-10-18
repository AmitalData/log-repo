
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
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL.EntityPMs; 
using Logitude.Social.Data;

namespace Logitude.Social.BL.EntityDataMappings
{
   
   public partial class PostDataMapping: IMapping<PostPM, Post>,IMappingEncodeBase64NVARCHARFields<PostPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreatedById, 
	         GroupId, 
	         BodyText, 
	         CreateDate, 
	         ParentPostId, 
	         NumberOfLikes, 
	         IsPrivate, 
	         IsCancelled, 
	         ObjectTableId, 
	         EntityId, 
	         IsAutomatic, 
	         UpdateDate, 
	         EntityDescription, 
	         NumberOfComments,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreatedById, 
	         GroupId, 
	         BodyText, 
	         CreateDate, 
	         ParentPostId, 
	         NumberOfLikes, 
	         IsPrivate, 
	         IsCancelled, 
	         ObjectTableId, 
	         EntityId, 
	         CreatedByUserName, 
	         IsAutomatic, 
	         UpdateDate, 
	         EntityDescription, 
	         NumberOfComments, 
	         UpdatedByUserId, 
	         UserImageDetailId, 
	         IndexColor, 
	         DefaultColor,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PostPM entityPM, Post entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedById))
            {
				entityPOCO.CreatedById = entityPM.CreatedById;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupId))
            {
				entityPOCO.GroupId = entityPM.GroupId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BodyText))
            {
				entityPOCO.BodyText = entityPM.BodyText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentPostId))
            {
				entityPOCO.ParentPostId = entityPM.ParentPostId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfLikes))
            {
				entityPOCO.NumberOfLikes = entityPM.NumberOfLikes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPrivate))
            {
				entityPOCO.IsPrivate = entityPM.IsPrivate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAutomatic))
            {
				entityPOCO.IsAutomatic = entityPM.IsAutomatic;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityDescription))
            {
				entityPOCO.EntityDescription = entityPM.EntityDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfComments))
            {
				entityPOCO.NumberOfComments = entityPM.NumberOfComments;
			}
			}

		public void POCOToPM(PostPM entityPM, Post entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedById))
            {
					entityPM.CreatedById = entityPOCO.CreatedById;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GroupId))
            {
					entityPM.GroupId = entityPOCO.GroupId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BodyText))
            {
					entityPM.BodyText = entityPOCO.BodyText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParentPostId))
            {
					entityPM.ParentPostId = entityPOCO.ParentPostId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfLikes))
            {
					entityPM.NumberOfLikes = entityPOCO.NumberOfLikes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPrivate))
            {
					entityPM.IsPrivate = entityPOCO.IsPrivate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAutomatic))
            {
					entityPM.IsAutomatic = entityPOCO.IsAutomatic;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityDescription))
            {
					entityPM.EntityDescription = entityPOCO.EntityDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfComments))
            {
					entityPM.NumberOfComments = entityPOCO.NumberOfComments;
            }

		}

		public void PMToOldPM(PostPM entityPM, PostPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedById))
            {
                oldEntityPM.CreatedById = entityPM.CreatedById;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupId))
            {
                oldEntityPM.GroupId = entityPM.GroupId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BodyText))
            {
                oldEntityPM.BodyText = entityPM.BodyText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentPostId))
            {
                oldEntityPM.ParentPostId = entityPM.ParentPostId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfLikes))
            {
                oldEntityPM.NumberOfLikes = entityPM.NumberOfLikes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPrivate))
            {
                oldEntityPM.IsPrivate = entityPM.IsPrivate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAutomatic))
            {
                oldEntityPM.IsAutomatic = entityPM.IsAutomatic;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityDescription))
            {
                oldEntityPM.EntityDescription = entityPM.EntityDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfComments))
            {
                oldEntityPM.NumberOfComments = entityPM.NumberOfComments;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(PostPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.BodyText)) //T4 find type == nText 
            {
                entityPM.BodyText = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.BodyText));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.EntityDescription)) //T4 find type == nText 
            {
                entityPM.EntityDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.EntityDescription));
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
	 