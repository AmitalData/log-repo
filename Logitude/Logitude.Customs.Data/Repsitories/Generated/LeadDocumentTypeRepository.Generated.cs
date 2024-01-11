 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class LeadDocumentTypeRepository:IRepository<LeadDocumentType>
   {
   
        private ICustomContext currentContext;
        public LeadDocumentTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LeadDocumentTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LeadDocumentType GetSingle(string code)
        {
            return (from a in context.LeadDocumentTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LeadDocumentType> GetAll()
        {
            return from a in context.LeadDocumentTypes  
                   select a;
        }
				 
        public LeadDocumentType GetSingle(EntityKeyFields entityKeys)
        {
            LeadDocumentTypeKeys keys = entityKeys as LeadDocumentTypeKeys;
            return (from a in context.LeadDocumentTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LeadDocumentType entity)
        {
            onAdd();
            context.LeadDocumentTypes.Add(entity);
        }

        public void Remove(LeadDocumentType entity)
        {
            context.LeadDocumentTypes.Attach(entity);
            context.LeadDocumentTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LeadDocumentType entity)
        {
            onUpdate();
            context.LeadDocumentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LeadDocumentType> All()
        {
            return context.LeadDocumentTypes.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 