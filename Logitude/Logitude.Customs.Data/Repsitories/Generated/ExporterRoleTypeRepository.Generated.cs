 
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
   public partial class ExporterRoleTypeRepository:IRepository<ExporterRoleType>
   {
   
        private ICustomContext currentContext;
        public ExporterRoleTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExporterRoleTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExporterRoleType GetSingle(string code)
        {
            return (from a in context.ExporterRoleTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ExporterRoleType> GetAll()
        {
            return from a in context.ExporterRoleTypes  
                   select a;
        }
				 
        public ExporterRoleType GetSingle(EntityKeyFields entityKeys)
        {
            ExporterRoleTypeKeys keys = entityKeys as ExporterRoleTypeKeys;
            return (from a in context.ExporterRoleTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExporterRoleType entity)
        {
            onAdd();
            context.ExporterRoleTypes.Add(entity);
        }

        public void Remove(ExporterRoleType entity)
        {
            context.ExporterRoleTypes.Attach(entity);
            context.ExporterRoleTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExporterRoleType entity)
        {
            onUpdate();
            context.ExporterRoleTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExporterRoleType> All()
        {
            return context.ExporterRoleTypes.ToList();
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
	 