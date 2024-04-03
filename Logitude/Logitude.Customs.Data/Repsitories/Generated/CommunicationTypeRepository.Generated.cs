 
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
   public partial class CommunicationTypeRepository:IRepository<CommunicationType>
   {
   
        private ICustomContext currentContext;
        public CommunicationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CommunicationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CommunicationType GetSingle(string code)
        {
            return (from a in context.CommunicationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CommunicationType> GetAll()
        {
            return from a in context.CommunicationTypes  
                   select a;
        }
				 
        public CommunicationType GetSingle(EntityKeyFields entityKeys)
        {
            CommunicationTypeKeys keys = entityKeys as CommunicationTypeKeys;
            return (from a in context.CommunicationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CommunicationType entity)
        {
            onAdd();
            context.CommunicationTypes.Add(entity);
        }

        public void Remove(CommunicationType entity)
        {
            context.CommunicationTypes.Attach(entity);
            context.CommunicationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CommunicationType entity)
        {
            onUpdate();
            context.CommunicationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CommunicationType> All()
        {
            return context.CommunicationTypes.ToList();
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
	 