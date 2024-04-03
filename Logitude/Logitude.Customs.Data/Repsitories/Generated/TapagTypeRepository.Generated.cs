 
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
   public partial class TapagTypeRepository:IRepository<TapagType>
   {
   
        private ICustomContext currentContext;
        public TapagTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TapagTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TapagType GetSingle(string code)
        {
            return (from a in context.TapagTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TapagType> GetAll()
        {
            return from a in context.TapagTypes  
                   select a;
        }
				 
        public TapagType GetSingle(EntityKeyFields entityKeys)
        {
            TapagTypeKeys keys = entityKeys as TapagTypeKeys;
            return (from a in context.TapagTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TapagType entity)
        {
            onAdd();
            context.TapagTypes.Add(entity);
        }

        public void Remove(TapagType entity)
        {
            context.TapagTypes.Attach(entity);
            context.TapagTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TapagType entity)
        {
            onUpdate();
            context.TapagTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TapagType> All()
        {
            return context.TapagTypes.ToList();
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
	 