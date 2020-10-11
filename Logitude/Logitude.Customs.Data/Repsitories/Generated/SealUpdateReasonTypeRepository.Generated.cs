 
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
   public partial class SealUpdateReasonTypeRepository:IRepository<SealUpdateReasonType>
   {
   
        private ICustomContext currentContext;
        public SealUpdateReasonTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SealUpdateReasonTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SealUpdateReasonType GetSingle(string code)
        {
            return (from a in context.SealUpdateReasonTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SealUpdateReasonType> GetAll()
        {
            return from a in context.SealUpdateReasonTypes  
                   select a;
        }
				 
        public SealUpdateReasonType GetSingle(EntityKeyFields entityKeys)
        {
            SealUpdateReasonTypeKeys keys = entityKeys as SealUpdateReasonTypeKeys;
            return (from a in context.SealUpdateReasonTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SealUpdateReasonType entity)
        {
            onAdd();
            context.SealUpdateReasonTypes.Add(entity);
        }

        public void Remove(SealUpdateReasonType entity)
        {
            context.SealUpdateReasonTypes.Attach(entity);
            context.SealUpdateReasonTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SealUpdateReasonType entity)
        {
            onUpdate();
            context.SealUpdateReasonTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SealUpdateReasonType> All()
        {
            return context.SealUpdateReasonTypes.ToList();
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
	 