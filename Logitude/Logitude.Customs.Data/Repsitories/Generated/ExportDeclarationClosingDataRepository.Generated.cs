 
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
   public partial class ExportDeclarationClosingDataRepository:IRepository<ExportDeclarationClosingData>
   {
   
        private ICustomContext currentContext;
        public ExportDeclarationClosingDataRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportDeclarationClosingDataRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExportDeclarationClosingData GetSingle(string declarationid, int tenant)
        {
            return (from a in context.ExportDeclarationClosingDatas
                    where a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExportDeclarationClosingData> GetAll(int tenant)
        {
            return from a in context.ExportDeclarationClosingDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExportDeclarationClosingData GetSingle(EntityKeyFields entityKeys)
        {
            ExportDeclarationClosingDataKeys keys = entityKeys as ExportDeclarationClosingDataKeys;
            return (from a in context.ExportDeclarationClosingDatas
                    where a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExportDeclarationClosingData entity)
        {
            onAdd();
            context.ExportDeclarationClosingDatas.Add(entity);
        }

        public void Remove(ExportDeclarationClosingData entity)
        {
            context.ExportDeclarationClosingDatas.Attach(entity);
            context.ExportDeclarationClosingDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExportDeclarationClosingData entity)
        {
            onUpdate();
            context.ExportDeclarationClosingDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExportDeclarationClosingData> All()
        {
            return context.ExportDeclarationClosingDatas.ToList();
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
	 