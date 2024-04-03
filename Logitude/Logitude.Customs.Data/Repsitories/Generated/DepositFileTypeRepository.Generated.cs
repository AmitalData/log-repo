 
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
   public partial class DepositFileTypeRepository:IRepository<DepositFileType>
   {
   
        private ICustomContext currentContext;
        public DepositFileTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DepositFileTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DepositFileType GetSingle(string code)
        {
            return (from a in context.DepositFileTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DepositFileType> GetAll()
        {
            return from a in context.DepositFileTypes  
                   select a;
        }
				 
        public DepositFileType GetSingle(EntityKeyFields entityKeys)
        {
            DepositFileTypeKeys keys = entityKeys as DepositFileTypeKeys;
            return (from a in context.DepositFileTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DepositFileType entity)
        {
            onAdd();
            context.DepositFileTypes.Add(entity);
        }

        public void Remove(DepositFileType entity)
        {
            context.DepositFileTypes.Attach(entity);
            context.DepositFileTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DepositFileType entity)
        {
            onUpdate();
            context.DepositFileTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DepositFileType> All()
        {
            return context.DepositFileTypes.ToList();
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
	 