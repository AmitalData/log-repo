 
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
   public partial class UnloadingSiteTypeRepository:IRepository<UnloadingSiteType>
   {
   
        private ICustomContext currentContext;
        public UnloadingSiteTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public UnloadingSiteTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  UnloadingSiteType GetSingle(string code)
        {
            return (from a in context.UnloadingSiteType
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<UnloadingSiteType> GetAll()
        {
            return from a in context.UnloadingSiteType  
                   select a;
        }
				 
        public UnloadingSiteType GetSingle(EntityKeyFields entityKeys)
        {
            UnloadingSiteTypeKeys keys = entityKeys as UnloadingSiteTypeKeys;
            return (from a in context.UnloadingSiteType
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(UnloadingSiteType entity)
        {
            onAdd();
            context.UnloadingSiteType.Add(entity);
        }

        public void Remove(UnloadingSiteType entity)
        {
            context.UnloadingSiteType.Attach(entity);
            context.UnloadingSiteType.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(UnloadingSiteType entity)
        {
            onUpdate();
            context.UnloadingSiteType.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UnloadingSiteType> All()
        {
            return context.UnloadingSiteType.ToList();
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
	 