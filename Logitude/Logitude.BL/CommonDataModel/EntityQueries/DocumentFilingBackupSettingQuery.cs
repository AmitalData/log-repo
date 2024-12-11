using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentFilingBackupSettingQuery
    {
        DocumentFilingBackupSettingRepository repository;
        public DocumentFilingBackupSettingQuery()
        {
            repository = new DocumentFilingBackupSettingRepository();
        }


        public DocumentFilingBackupSettingQuery(DocumentFilingBackupSettingRepository DocumentFilingBackupSettingRepository)
        {
            repository = DocumentFilingBackupSettingRepository;
        }

        public DocumentFilingBackupSettingQuery(int tenant)
        {
            repository = new DocumentFilingBackupSettingRepository(tenant);
        }

        public DocumentFilingBackupSettingPM GetSinglePM(int tenantId, int tenant)
        {


            string entityName = "DocumentFilingBackupSettingPM" + tenantId;
            DocumentFilingBackupSettingPM entity;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {

                    entity = (from a in repository.context.DocumentFilingBackupSettings
                                       where a.Tenant == tenantId
                                       select
                        new DocumentFilingBackupSettingPM()
                        {
                            Tenant = a.Tenant,
                            IsActive = a.IsActive,
                            FTPDetailId = a.FTPDetailId,
                            ActivationDate = a.ActivationDate,
                            DeactivationDate = a.DeactivationDate,
                        }).FirstOrDefault();

                    if (entity != null)
                    {
                        string cname = "DocumentFilingBackupSettingPM" + entity.Tenant;

                        if (CacheManager.CacheWrapper.Get(cname) == null)
                        {
                            CacheManager.CacheWrapper.Insert(cname, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                        entity = (DocumentFilingBackupSettingPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    
                 

                }
                else
                {
                    entity = (DocumentFilingBackupSettingPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                entity = (from a in repository.context.DocumentFilingBackupSettings
                          where a.Tenant == tenantId
                          select
                        new DocumentFilingBackupSettingPM()
                        {
                            Tenant = a.Tenant,
                            IsActive = a.IsActive,
                            FTPDetailId = a.FTPDetailId,
                            ActivationDate = a.ActivationDate,
                            DeactivationDate = a.DeactivationDate,
                        }).FirstOrDefault();
            }


            return entity;


        }


        public DocumentFilingBackupSettingPM GetDocumentFilingBackupSettingPMByTenant(int tenant)
        {
            return (from a in repository.context.DocumentFilingBackupSettings
                    where a.Tenant == tenant
                    select new DocumentFilingBackupSettingPM()
                    {
                        Tenant = a.Tenant,
                        IsActive = a.IsActive,
                        FTPDetailId = a.FTPDetailId,
                        ActivationDate = a.ActivationDate,
                        DeactivationDate = a.DeactivationDate,

                    }).FirstOrDefault();
        }

        public IQueryable<DocumentFilingBackupSettingPM> GetDocumentFilingBackupSettingPMs()
        {
            return from a in repository.context.DocumentFilingBackupSettings 
                   select new DocumentFilingBackupSettingPM()
                   {
                       Tenant = a.Tenant,
                       IsActive = a.IsActive,
                       FTPDetailId = a.FTPDetailId,
                       ActivationDate = a.ActivationDate,
                       DeactivationDate = a.DeactivationDate,

                   };
        }

        public IQueryable<DocumentFilingBackupSettingList> GetIQueryableEntityList(IQueryable<DocumentFilingBackupSetting> iQueryable)
        {
            IQueryable<DocumentFilingBackupSettingList> result = from a in iQueryable
                                                         select new DocumentFilingBackupSettingList()
                                                         {
                                                             Tenant = a.Tenant,
                                                             IsActive = a.IsActive,
                                                             FTPDetailId = a.FTPDetailId,
                                                             ActivationDate = a.ActivationDate,
                                                             DeactivationDate = a.DeactivationDate,

                                                         };
            return result;
        }


    }
}
