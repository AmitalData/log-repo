 
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
   public partial class DeclarationStatusTypeRepository:IRepository<DeclarationStatusType>
   {
   
        private ICustomContext currentContext;
        public DeclarationStatusTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationStatusTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationStatusType GetSingle(string code)
        {
            return (from a in context.DeclarationStatusTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationStatusType> GetAll()
        {
            return from a in context.DeclarationStatusTypes  
                   select a;
        }
				 
        public DeclarationStatusType GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationStatusTypeKeys keys = entityKeys as DeclarationStatusTypeKeys;
            return (from a in context.DeclarationStatusTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationStatusType entity)
        {
            onAdd();
            context.DeclarationStatusTypes.Add(entity);
        }

        public void Remove(DeclarationStatusType entity)
        {
            context.DeclarationStatusTypes.Attach(entity);
            context.DeclarationStatusTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationStatusType entity)
        {
            onUpdate();
            context.DeclarationStatusTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationStatusType> All()
        {
            return context.DeclarationStatusTypes.ToList();
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
	 