 
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
   public partial class CustomsBranchRepository:IRepository<CustomsBranch>
   {
   
        private ICustomContext currentContext;
        public CustomsBranchRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsBranchRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsBranch GetSingle(string id)
        {
            return (from a in context.CustomsBranches
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsBranch> GetAll()
        {
            return from a in context.CustomsBranches  
                   select a;
        }
				 
        public CustomsBranch GetSingle(EntityKeyFields entityKeys)
        {
            CustomsBranchKeys keys = entityKeys as CustomsBranchKeys;
            return (from a in context.CustomsBranches
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsBranch entity)
        {
            onAdd();
            context.CustomsBranches.Add(entity);
        }

        public void Remove(CustomsBranch entity)
        {
            context.CustomsBranches.Attach(entity);
            context.CustomsBranches.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsBranch entity)
        {
            onUpdate();
            context.CustomsBranches.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsBranch> All()
        {
            return context.CustomsBranches.ToList();
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
	 