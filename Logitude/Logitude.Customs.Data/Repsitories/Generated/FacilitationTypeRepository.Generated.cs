 
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
   public partial class FacilitationTypeRepository:IRepository<FacilitationType>
   {
   
        private ICustomContext currentContext;
        public FacilitationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public FacilitationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  FacilitationType GetSingle(string code)
        {
            return (from a in context.FacilitationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<FacilitationType> GetAll()
        {
            return from a in context.FacilitationTypes  
                   select a;
        }
				 
        public FacilitationType GetSingle(EntityKeyFields entityKeys)
        {
            FacilitationTypeKeys keys = entityKeys as FacilitationTypeKeys;
            return (from a in context.FacilitationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FacilitationType entity)
        {
            onAdd();
            context.FacilitationTypes.Add(entity);
        }

        public void Remove(FacilitationType entity)
        {
            context.FacilitationTypes.Attach(entity);
            context.FacilitationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FacilitationType entity)
        {
            onUpdate();
            context.FacilitationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FacilitationType> All()
        {
            return context.FacilitationTypes.ToList();
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
	 