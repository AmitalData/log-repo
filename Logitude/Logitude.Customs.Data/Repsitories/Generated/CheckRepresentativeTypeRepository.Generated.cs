 
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
   public partial class CheckRepresentativeTypeRepository:IRepository<CheckRepresentativeType>
   {
   
        private ICustomContext currentContext;
        public CheckRepresentativeTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CheckRepresentativeTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CheckRepresentativeType GetSingle(string code)
        {
            return (from a in context.CheckRepresentativeTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CheckRepresentativeType> GetAll()
        {
            return from a in context.CheckRepresentativeTypes  
                   select a;
        }
				 
        public CheckRepresentativeType GetSingle(EntityKeyFields entityKeys)
        {
            CheckRepresentativeTypeKeys keys = entityKeys as CheckRepresentativeTypeKeys;
            return (from a in context.CheckRepresentativeTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CheckRepresentativeType entity)
        {
            onAdd();
            context.CheckRepresentativeTypes.Add(entity);
        }

        public void Remove(CheckRepresentativeType entity)
        {
            context.CheckRepresentativeTypes.Attach(entity);
            context.CheckRepresentativeTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CheckRepresentativeType entity)
        {
            onUpdate();
            context.CheckRepresentativeTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CheckRepresentativeType> All()
        {
            return context.CheckRepresentativeTypes.ToList();
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
	 