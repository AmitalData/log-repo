 
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
   public partial class ConverterTypeRepository:IRepository<ConverterType>
   {
   
        private ICustomContext currentContext;
        public ConverterTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConverterTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConverterType GetSingle(string code)
        {
            return (from a in context.ConverterTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ConverterType> GetAll()
        {
            return from a in context.ConverterTypes  
                   select a;
        }
				 
        public ConverterType GetSingle(EntityKeyFields entityKeys)
        {
            ConverterTypeKeys keys = entityKeys as ConverterTypeKeys;
            return (from a in context.ConverterTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConverterType entity)
        {
            onAdd();
            context.ConverterTypes.Add(entity);
        }

        public void Remove(ConverterType entity)
        {
            context.ConverterTypes.Attach(entity);
            context.ConverterTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConverterType entity)
        {
            onUpdate();
            context.ConverterTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConverterType> All()
        {
            return context.ConverterTypes.ToList();
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
	 