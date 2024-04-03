 
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
   public partial class TPGFileTypeRepository:IRepository<TPGFileType>
   {
   
        private ICustomContext currentContext;
        public TPGFileTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TPGFileTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TPGFileType GetSingle(string code)
        {
            return (from a in context.TPGFileTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TPGFileType> GetAll()
        {
            return from a in context.TPGFileTypes  
                   select a;
        }
				 
        public TPGFileType GetSingle(EntityKeyFields entityKeys)
        {
            TPGFileTypeKeys keys = entityKeys as TPGFileTypeKeys;
            return (from a in context.TPGFileTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TPGFileType entity)
        {
            onAdd();
            context.TPGFileTypes.Add(entity);
        }

        public void Remove(TPGFileType entity)
        {
            context.TPGFileTypes.Attach(entity);
            context.TPGFileTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TPGFileType entity)
        {
            onUpdate();
            context.TPGFileTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TPGFileType> All()
        {
            return context.TPGFileTypes.ToList();
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
	 