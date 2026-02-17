 
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
   public partial class DeficitConnFileParagraphTypeRepository:IRepository<DeficitConnFileParagraphType>
   {
   
        private ICustomContext currentContext;
        public DeficitConnFileParagraphTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeficitConnFileParagraphTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeficitConnFileParagraphType GetSingle(string deficitid, string declarationid, string paragraphtypecode, int tenant)
        {
            return (from a in context.DeficitConnFileParagraphTypes
                    where a.DeficitId == deficitid && a.DeclarationId == declarationid && a.ParagraphTypeCode == paragraphtypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeficitConnFileParagraphType> GetAll(int tenant)
        {
            return from a in context.DeficitConnFileParagraphTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeficitConnFileParagraphType GetSingle(EntityKeyFields entityKeys)
        {
            DeficitConnFileParagraphTypeKeys keys = entityKeys as DeficitConnFileParagraphTypeKeys;
            return (from a in context.DeficitConnFileParagraphTypes
                    where a.DeficitId == keys.DeficitId && a.DeclarationId == keys.DeclarationId && a.ParagraphTypeCode == keys.ParagraphTypeCode
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeficitConnFileParagraphType entity)
        {
            onAdd();
            context.DeficitConnFileParagraphTypes.Add(entity);
        }

        public void Remove(DeficitConnFileParagraphType entity)
        {
            context.DeficitConnFileParagraphTypes.Attach(entity);
            context.DeficitConnFileParagraphTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeficitConnFileParagraphType entity)
        {
            onUpdate();
            context.DeficitConnFileParagraphTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeficitConnFileParagraphType> All()
        {
            return context.DeficitConnFileParagraphTypes.ToList();
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
	 