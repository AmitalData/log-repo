using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentFilingBackupSettingRepository : IRepository<DocumentFilingBackupSetting>
    {
        ICommonDataContext commonDataContext;

        public DocumentFilingBackupSettingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentFilingBackupSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentFilingBackupSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
 

        public DocumentFilingBackupSetting GetSingleDocumentFilingBackupSetting(int id, int otherTenant = 0)
        {
            return (from a in this.context.DocumentFilingBackupSettings
                    where a.Tenant == id
                    select a).FirstOrDefault();
        }

        public void Add(DocumentFilingBackupSetting entity)
        {
            this.context.DocumentFilingBackupSettings.Add(entity);
        }

        public void Remove(DocumentFilingBackupSetting entity)
        {
            this.context.DocumentFilingBackupSettings.Attach(entity);
            this.context.DocumentFilingBackupSettings.Remove(entity);
        }

        public void Update(DocumentFilingBackupSetting entity)
        {
            this.context.DocumentFilingBackupSettings.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<DocumentFilingBackupSetting> All()
        {
            return this.context.DocumentFilingBackupSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<DocumentFilingBackupSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentFilingBackupSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<DocumentFilingBackupSetting> GetDocumentFilingBackupSettings(int tenant)
        {
            return (from a in context.DocumentFilingBackupSettings select a);
        }


        public bool IsDocumentFilingBackupSettingActive(int tenant)
        {
            return (from a in this.context.DocumentFilingBackupSettings
                    where a.Tenant == tenant && a.IsActive == true
                    select a).Any();
        }

    }
}