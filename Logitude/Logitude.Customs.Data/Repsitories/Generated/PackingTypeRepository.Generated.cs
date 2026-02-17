 
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
   public partial class PackingTypeRepository:IRepository<PackingType>
   {
   
        private ICustomContext currentContext;
        public PackingTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PackingTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PackingType GetSingle(string code)
        {
            return (from a in context.PackingTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PackingType> GetAll()
        {
            return from a in context.PackingTypes  
                   select a;
        }
				 
        public PackingType GetSingle(EntityKeyFields entityKeys)
        {
            PackingTypeKeys keys = entityKeys as PackingTypeKeys;
            return (from a in context.PackingTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PackingType entity)
        {
            onAdd();
            context.PackingTypes.Add(entity);
        }

        public void Remove(PackingType entity)
        {
            context.PackingTypes.Attach(entity);
            context.PackingTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PackingType entity)
        {
            onUpdate();
            context.PackingTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PackingType> All()
        {
            return context.PackingTypes.ToList();
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
	 