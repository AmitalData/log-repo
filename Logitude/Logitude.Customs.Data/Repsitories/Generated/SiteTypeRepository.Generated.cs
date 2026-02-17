 
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
   public partial class SiteTypeRepository:IRepository<SiteType>
   {
   
        private ICustomContext currentContext;
        public SiteTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SiteTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SiteType GetSingle(string code)
        {
            return (from a in context.SiteTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SiteType> GetAll()
        {
            return from a in context.SiteTypes  
                   select a;
        }
				 
        public SiteType GetSingle(EntityKeyFields entityKeys)
        {
            SiteTypeKeys keys = entityKeys as SiteTypeKeys;
            return (from a in context.SiteTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SiteType entity)
        {
            onAdd();
            context.SiteTypes.Add(entity);
        }

        public void Remove(SiteType entity)
        {
            context.SiteTypes.Attach(entity);
            context.SiteTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SiteType entity)
        {
            onUpdate();
            context.SiteTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SiteType> All()
        {
            return context.SiteTypes.ToList();
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
	 