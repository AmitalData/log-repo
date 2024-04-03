 
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
   public partial class StuffingSiteTypeRepository:IRepository<StuffingSiteType>
   {
   
        private ICustomContext currentContext;
        public StuffingSiteTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public StuffingSiteTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  StuffingSiteType GetSingle(string code)
        {
            return (from a in context.StuffingSiteTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<StuffingSiteType> GetAll()
        {
            return from a in context.StuffingSiteTypes  
                   select a;
        }
				 
        public StuffingSiteType GetSingle(EntityKeyFields entityKeys)
        {
            StuffingSiteTypeKeys keys = entityKeys as StuffingSiteTypeKeys;
            return (from a in context.StuffingSiteTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(StuffingSiteType entity)
        {
            onAdd();
            context.StuffingSiteTypes.Add(entity);
        }

        public void Remove(StuffingSiteType entity)
        {
            context.StuffingSiteTypes.Attach(entity);
            context.StuffingSiteTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(StuffingSiteType entity)
        {
            onUpdate();
            context.StuffingSiteTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<StuffingSiteType> All()
        {
            return context.StuffingSiteTypes.ToList();
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
	 