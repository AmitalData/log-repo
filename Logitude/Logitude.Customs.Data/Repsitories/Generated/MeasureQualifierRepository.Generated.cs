 
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
   public partial class MeasureQualifierRepository:IRepository<MeasureQualifier>
   {
   
        private ICustomContext currentContext;
        public MeasureQualifierRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public MeasureQualifierRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  MeasureQualifier GetSingle(string code)
        {
            return (from a in context.MeasureQualifier
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MeasureQualifier> GetAll()
        {
            return from a in context.MeasureQualifier  
                   select a;
        }
				 
        public MeasureQualifier GetSingle(EntityKeyFields entityKeys)
        {
            MeasureQualifierKeys keys = entityKeys as MeasureQualifierKeys;
            return (from a in context.MeasureQualifier
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MeasureQualifier entity)
        {
            onAdd();
            context.MeasureQualifier.Add(entity);
        }

        public void Remove(MeasureQualifier entity)
        {
            context.MeasureQualifier.Attach(entity);
            context.MeasureQualifier.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MeasureQualifier entity)
        {
            onUpdate();
            context.MeasureQualifier.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MeasureQualifier> All()
        {
            return context.MeasureQualifier.ToList();
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
	 