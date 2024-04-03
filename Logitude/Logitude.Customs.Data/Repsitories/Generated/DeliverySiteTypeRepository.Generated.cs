 
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
   public partial class DeliverySiteTypeRepository:IRepository<DeliverySiteType>
   {
   
        private ICustomContext currentContext;
        public DeliverySiteTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeliverySiteTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeliverySiteType GetSingle(string code)
        {
            return (from a in context.DeliverySiteTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DeliverySiteType> GetAll()
        {
            return from a in context.DeliverySiteTypes  
                   select a;
        }
				 
        public DeliverySiteType GetSingle(EntityKeyFields entityKeys)
        {
            DeliverySiteTypeKeys keys = entityKeys as DeliverySiteTypeKeys;
            return (from a in context.DeliverySiteTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeliverySiteType entity)
        {
            onAdd();
            context.DeliverySiteTypes.Add(entity);
        }

        public void Remove(DeliverySiteType entity)
        {
            context.DeliverySiteTypes.Attach(entity);
            context.DeliverySiteTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeliverySiteType entity)
        {
            onUpdate();
            context.DeliverySiteTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeliverySiteType> All()
        {
            return context.DeliverySiteTypes.ToList();
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
	 