using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ComputingPartnerQuery
    {
        ComputingPartnerRepository repository;

        public ComputingPartnerQuery()
        {
            repository = new ComputingPartnerRepository(); 
        }

        public ComputingPartnerQuery(int tenant)
        {
            repository = new ComputingPartnerRepository(tenant);
        }

        public ComputingPartnerQuery(ComputingPartnerRepository myRepository)
        {
            repository = myRepository;
        }

        public ComputingPartnerPM GetSinglePM(string id)
        {
            ComputingPartnerPM entityPM =
                (from a in repository.Context.ComputingPartners.Include("CreatedByUser").Include("UpdatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                 where a.Id == id
                 select new ComputingPartnerPM()
                 {
                     Id = a.Id,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     Name = a.Name,
                     Remarks = a.Remarks,
                     SearchFields = a.SearchFields,
                     CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                     Code=a.Code,
                     Tenant=a.Tenant
                 }).FirstOrDefault();


           // ComputingPartnerTableQuery myPartnerTableQuery = new ComputingPartnerTableQuery(tenant);

           // entityPM.PartnerTables = myPartnerTableQuery.GetTablesByPartnerId(id, tenant).ToList();

            return entityPM;
        }




        public ComputingPartnerPM GetSinglePMByName(string name, int tenant)
        {
            ComputingPartnerPM entityPM =
                (from a in repository.Context.ComputingPartners
                 where a.Name == name && a.Tenant == tenant
                 select new ComputingPartnerPM()
                 {
                     Id = a.Id,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     Name = a.Name,
                     Remarks = a.Remarks,
                     SearchFields = a.SearchFields,
                     Code = a.Code,
                     InActive = a.InActive,
                     Description = a.Description,
                     Tenant = a.Tenant,

                 }).FirstOrDefault();


            
            return entityPM;
        }

        public ComputingPartnerPM GetSinglePMByCode(string code, int tenant)
        {
            if (string.IsNullOrWhiteSpace(code)) return new ComputingPartnerPM();
            string key = $"GetSinglePMByCode({code},{tenant})";
            return CacheManager.GetOrInsertNewObject<ComputingPartnerPM>(key, () =>
            {
                return GetSinglePMByCodeSlow(code, tenant);
            });
        }

        ComputingPartnerPM GetSinglePMByCodeSlow(string code, int tenant)
        {
            ComputingPartnerPM entityPM =
                (from a in repository.Context.ComputingPartners
                 where a.Code == code && a.Tenant == tenant
                 select new ComputingPartnerPM()
                 {
                     Id = a.Id,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     Name = a.Name,
                     Remarks = a.Remarks,
                     SearchFields = a.SearchFields,
                     Code=a.Code,
                     InActive=a.InActive,
                     Description=a.Description,
                     Tenant=a.Tenant,                     
                 }).FirstOrDefault();



            return entityPM;
        }

        public ComputingPartnerPM GetSinglePMByCodeAndCheckTenantZero(string code, int tenant)
        {
            ComputingPartnerPM entityPM =
                (from a in repository.Context.ComputingPartners
                 where a.Code == code && a.Tenant == tenant
                 select new ComputingPartnerPM()
                 {
                     Id = a.Id,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     Name = a.Name,
                     Remarks = a.Remarks,
                     SearchFields = a.SearchFields,
                     Code = a.Code,
                     InActive = a.InActive,
                     Description = a.Description,
                     Tenant = a.Tenant,
                 }).FirstOrDefault();

            if (entityPM == null)
            {
                entityPM =
                   (from a in repository.Context.ComputingPartners
                    where a.Code == code && a.Tenant == 0
                    select new ComputingPartnerPM()
                    {
                        Id = a.Id,
                        CreateDate = a.CreateDate,
                        UpdateDate = a.UpdateDate,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdatedByUserId = a.UpdatedByUserId,
                        Name = a.Name,
                        Remarks = a.Remarks,
                        SearchFields = a.SearchFields,
                        Code = a.Code,
                        InActive = a.InActive,
                        Description = a.Description,
                        Tenant = a.Tenant,
                    }).FirstOrDefault();
            }


            return entityPM;
        }

        public ComputingPartnerPM GetSinglePM(string id, int tenant)
        {
            ComputingPartnerPM entityPM =
                (from a in repository.Context.ComputingPartners.Include("CreatedByUser").Include("UpdatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                 where a.Id == id 
                 select new ComputingPartnerPM()
                 {
                     Id = a.Id,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     Name = a.Name,
                     Remarks = a.Remarks,
                     SearchFields = a.SearchFields,
                     CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                     LoggedTenantId = tenant,
                     Code=a.Code,
                     Tenant=a.Tenant,
                     Description = a.Description,
                     InActive = a.InActive
                 }).FirstOrDefault();


            ComputingPartnerTableQuery myPartnerTableQuery = new ComputingPartnerTableQuery(entityPM.Tenant);

            entityPM.PartnerTables = myPartnerTableQuery.GetTablesByPartnerId(id, tenant).ToList();

            return entityPM;
        }

        public IQueryable<ComputingPartnerList> GetIQueryableEntityList(IQueryable<ComputingPartner> iQueryable, int tenant)
        {
            IQueryable<ComputingPartnerList> myResult = from a in iQueryable.Include("CreatedByUser").Include("UpdatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                                                        select new ComputingPartnerList()
                                                 {
                                                     Id = a.Id,
                                                     CreateDate = a.CreateDate,
                                                     UpdateDate = a.UpdateDate,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     UpdatedByUserId = a.UpdatedByUserId,
                                                     Name = a.Name,
                                                     Remarks = a.Remarks,
                                                     SearchFields = a.SearchFields,
                                                     CreatedByUserName = a.CreatedByUser == null ?null : (a.CreatedByUser.Contact == null ? null :a.CreatedByUser.Tenant==0&&tenant!=0?"System":a.CreatedByUser.Contact.EnglishName),
                                                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Tenant == 0 && tenant != 0 ? "System" : a.UpdatedByUser.Contact.EnglishName),
                                                     LoggedTenantId = tenant,
                                                            Tenant = a.Tenant,
                                                            Code = a.Code,
                                                            Description = a.Description,
                                                            InActive = a.InActive
                                                        };
            return myResult;
        }

        public IQueryable<ComputingPartnerList> GetIQueryableEntityList(IQueryable<ComputingPartner> iQueryable)
        {
            IQueryable<ComputingPartnerList> myResult = from a in iQueryable.Include("CreatedByUser").Include("UpdatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                                                        select new ComputingPartnerList()
                                                        {
                                                            Id = a.Id,
                                                            CreateDate = a.CreateDate,
                                                            UpdateDate = a.UpdateDate,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            Name = a.Name,
                                                            Remarks = a.Remarks,
                                                            SearchFields = a.SearchFields,
                                                            CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                                            UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                                                          Code=a.Code,
                                                          Tenant=a.Tenant,
                                                          Description=a.Description,
                                                          InActive=a.InActive
                                                        };
            return myResult;
        }


        public string GetComputingPartnerNameById(string id)
        {
            return (from a in repository.Context.ComputingPartners.Where(d => d.Id == id) select a.Name).FirstOrDefault();
        }

        public void CopyFromTenant0(int tenant, int tenatToCopy)
        {



            ComputingPartner pocos = repository.Context.ComputingPartners.Where(r => r.Tenant == tenant && r.Name == "customs").FirstOrDefault();
            if (pocos != null)
            {
                List<ComputingPartnerTranslation> pocosTranslation = repository.Context.ComputingPartnerTranslations.Where(r => r.Tenant == tenant && pocos.Id == r.ComputingPartnerId).ToList();

                ComputingPartnerPM ComputingPartner = new ComputingPartnerPM()
                {
                    Name = pocos.Name,
                    CreateDate = DateTime.UtcNow,
                    Remarks = pocos.Remarks,
                    Code = pocos.Code,
                    Tenant = tenatToCopy,
                    Description = pocos.Description,
                    InActive = pocos.InActive

                };
                ComputingPartnerService service = new ComputingPartnerService(this.repository.Context, ComputingPartner);

                service.Create();


                string ComputingPartnerId = repository.Context.ComputingPartners.Where(r => r.Tenant == tenatToCopy && r.Name == "customs").FirstOrDefault().Id;
                foreach (var item in pocosTranslation)
                {

                    ComputingPartnerTranslationPM computingPartnerTranslationPM = new ComputingPartnerTranslationPM()
                    {
                        Tenant = item.Tenant,
                        ObjectTableId = item.ObjectTableId,
                        ComputingPartnerId = ComputingPartnerId,
                        OurCode = item.OurCode,
                        PartnerCode = item.PartnerCode,


                    };


                    ComputingPartnerTranslationService computingPartnerTranslationService = new ComputingPartnerTranslationService(this.repository.Context, tenatToCopy);

                    computingPartnerTranslationService.Create(computingPartnerTranslationPM);

                }



                this.repository.Context.SaveChanges();


            }

        }
    }


}
