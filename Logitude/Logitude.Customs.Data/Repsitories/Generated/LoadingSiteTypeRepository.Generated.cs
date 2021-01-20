 
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
   public partial class LoadingSiteTypeRepository:IRepository<LoadingSiteType>
   {
   
        private ICustomContext currentContext;
        public LoadingSiteTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LoadingSiteTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LoadingSiteType GetSingle(string code)
        {
            return (from a in context.LoadingSiteTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LoadingSiteType> GetAll()
        {
            return from a in context.LoadingSiteTypes  
                   select a;
        }
				 
        public LoadingSiteType GetSingle(EntityKeyFields entityKeys)
        {
            LoadingSiteTypeKeys keys = entityKeys as LoadingSiteTypeKeys;
            return (from a in context.LoadingSiteTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LoadingSiteType entity)
        {
            onAdd();
            context.LoadingSiteTypes.Add(entity);
        }

        public void Remove(LoadingSiteType entity)
        {
            context.LoadingSiteTypes.Attach(entity);
            context.LoadingSiteTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LoadingSiteType entity)
        {
            onUpdate();
            context.LoadingSiteTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LoadingSiteType> All()
        {
            return context.LoadingSiteTypes.ToList();
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
	 