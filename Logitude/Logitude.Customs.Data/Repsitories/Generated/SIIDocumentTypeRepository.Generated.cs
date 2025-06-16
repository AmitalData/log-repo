 
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
   public partial class SIIDocumentTypeRepository:IRepository<SIIDocumentType>
   {
   
        private ICustomContext currentContext;
        public SIIDocumentTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SIIDocumentTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SIIDocumentType GetSingle(string code)
        {
            return (from a in context.SIIDocumentTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SIIDocumentType> GetAll()
        {
            return from a in context.SIIDocumentTypes  
                   select a;
        }
				 
        public SIIDocumentType GetSingle(EntityKeyFields entityKeys)
        {
            SIIDocumentTypeKeys keys = entityKeys as SIIDocumentTypeKeys;
            return (from a in context.SIIDocumentTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SIIDocumentType entity)
        {
            onAdd();
            context.SIIDocumentTypes.Add(entity);
        }

        public void Remove(SIIDocumentType entity)
        {
            context.SIIDocumentTypes.Attach(entity);
            context.SIIDocumentTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SIIDocumentType entity)
        {
            onUpdate();
            context.SIIDocumentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SIIDocumentType> All()
        {
            return context.SIIDocumentTypes.ToList();
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
	 