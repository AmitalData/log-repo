 
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
   public partial class NbcDeclarationTypeRepository:IRepository<NbcDeclarationType>
   {
   
        private ICustomContext currentContext;
        public NbcDeclarationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public NbcDeclarationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  NbcDeclarationType GetSingle(string code)
        {
            return (from a in context.NbcDeclarationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<NbcDeclarationType> GetAll()
        {
            return from a in context.NbcDeclarationTypes  
                   select a;
        }
				 
        public NbcDeclarationType GetSingle(EntityKeyFields entityKeys)
        {
            NbcDeclarationTypeKeys keys = entityKeys as NbcDeclarationTypeKeys;
            return (from a in context.NbcDeclarationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(NbcDeclarationType entity)
        {
            onAdd();
            context.NbcDeclarationTypes.Add(entity);
        }

        public void Remove(NbcDeclarationType entity)
        {
            context.NbcDeclarationTypes.Attach(entity);
            context.NbcDeclarationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(NbcDeclarationType entity)
        {
            onUpdate();
            context.NbcDeclarationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NbcDeclarationType> All()
        {
            return context.NbcDeclarationTypes.ToList();
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
	 