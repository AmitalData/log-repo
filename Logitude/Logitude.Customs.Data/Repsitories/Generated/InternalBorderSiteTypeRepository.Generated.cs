 
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
   public partial class InternalBorderSiteTypeRepository:IRepository<InternalBorderSiteType>
   {
   
        private ICustomContext currentContext;
        public InternalBorderSiteTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public InternalBorderSiteTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  InternalBorderSiteType GetSingle(string code)
        {
            return (from a in context.InternalBorderSiteTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InternalBorderSiteType> GetAll()
        {
            return from a in context.InternalBorderSiteTypes  
                   select a;
        }
				 
        public InternalBorderSiteType GetSingle(EntityKeyFields entityKeys)
        {
            InternalBorderSiteTypeKeys keys = entityKeys as InternalBorderSiteTypeKeys;
            return (from a in context.InternalBorderSiteTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InternalBorderSiteType entity)
        {
            onAdd();
            context.InternalBorderSiteTypes.Add(entity);
        }

        public void Remove(InternalBorderSiteType entity)
        {
            context.InternalBorderSiteTypes.Attach(entity);
            context.InternalBorderSiteTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InternalBorderSiteType entity)
        {
            onUpdate();
            context.InternalBorderSiteTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InternalBorderSiteType> All()
        {
            return context.InternalBorderSiteTypes.ToList();
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
	 