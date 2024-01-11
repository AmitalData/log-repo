 
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
   public partial class MAWBTypeRepository:IRepository<MAWBType>
   {
   
        private ICustomContext currentContext;
        public MAWBTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public MAWBTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  MAWBType GetSingle(string code)
        {
            return (from a in context.MAWBTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MAWBType> GetAll()
        {
            return from a in context.MAWBTypes  
                   select a;
        }
				 
        public MAWBType GetSingle(EntityKeyFields entityKeys)
        {
            MAWBTypeKeys keys = entityKeys as MAWBTypeKeys;
            return (from a in context.MAWBTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MAWBType entity)
        {
            onAdd();
            context.MAWBTypes.Add(entity);
        }

        public void Remove(MAWBType entity)
        {
            context.MAWBTypes.Attach(entity);
            context.MAWBTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MAWBType entity)
        {
            onUpdate();
            context.MAWBTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MAWBType> All()
        {
            return context.MAWBTypes.ToList();
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
	 