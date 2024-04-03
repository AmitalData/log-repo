 
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
   public partial class PointerLevelRepository:IRepository<PointerLevel>
   {
   
        private ICustomContext currentContext;
        public PointerLevelRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PointerLevelRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PointerLevel GetSingle(string code)
        {
            return (from a in context.PointerLevels
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PointerLevel> GetAll()
        {
            return from a in context.PointerLevels  
                   select a;
        }
				 
        public PointerLevel GetSingle(EntityKeyFields entityKeys)
        {
            PointerLevelKeys keys = entityKeys as PointerLevelKeys;
            return (from a in context.PointerLevels
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PointerLevel entity)
        {
            onAdd();
            context.PointerLevels.Add(entity);
        }

        public void Remove(PointerLevel entity)
        {
            context.PointerLevels.Attach(entity);
            context.PointerLevels.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PointerLevel entity)
        {
            onUpdate();
            context.PointerLevels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PointerLevel> All()
        {
            return context.PointerLevels.ToList();
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
	 