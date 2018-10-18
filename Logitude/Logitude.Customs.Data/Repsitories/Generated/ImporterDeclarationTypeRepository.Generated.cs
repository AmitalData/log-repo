 
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
   public partial class ImporterDeclarationTypeRepository:IRepository<ImporterDeclarationType>
   {
   
        private ICustomContext currentContext;
        public ImporterDeclarationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ImporterDeclarationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ImporterDeclarationType GetSingle(string code)
        {
            return (from a in context.ImporterDeclarationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ImporterDeclarationType> GetAll()
        {
            return from a in context.ImporterDeclarationTypes  
                   select a;
        }
				 
        public ImporterDeclarationType GetSingle(EntityKeyFields entityKeys)
        {
            ImporterDeclarationTypeKeys keys = entityKeys as ImporterDeclarationTypeKeys;
            return (from a in context.ImporterDeclarationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ImporterDeclarationType entity)
        {
            onAdd();
            context.ImporterDeclarationTypes.Add(entity);
        }

        public void Remove(ImporterDeclarationType entity)
        {
            context.ImporterDeclarationTypes.Attach(entity);
            context.ImporterDeclarationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ImporterDeclarationType entity)
        {
            onUpdate();
            context.ImporterDeclarationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ImporterDeclarationType> All()
        {
            return context.ImporterDeclarationTypes.ToList();
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
	 