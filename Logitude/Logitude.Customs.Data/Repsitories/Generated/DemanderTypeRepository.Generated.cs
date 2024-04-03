 
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
   public partial class DemanderTypeRepository:IRepository<DemanderType>
   {
   
        private ICustomContext currentContext;
        public DemanderTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DemanderTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DemanderType GetSingle(string code)
        {
            return (from a in context.DemanderTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DemanderType> GetAll()
        {
            return from a in context.DemanderTypes  
                   select a;
        }
				 
        public DemanderType GetSingle(EntityKeyFields entityKeys)
        {
            DemanderTypeKeys keys = entityKeys as DemanderTypeKeys;
            return (from a in context.DemanderTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DemanderType entity)
        {
            onAdd();
            context.DemanderTypes.Add(entity);
        }

        public void Remove(DemanderType entity)
        {
            context.DemanderTypes.Attach(entity);
            context.DemanderTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DemanderType entity)
        {
            onUpdate();
            context.DemanderTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DemanderType> All()
        {
            return context.DemanderTypes.ToList();
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
	 