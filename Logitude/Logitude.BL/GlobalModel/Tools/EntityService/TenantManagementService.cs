using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Logitude.BL.GlobalModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class TenantManagementService
    {
        bool isNewEntity;
        int tenant;
        private TenantManagementPM entityPM;
        public TenantManagement entityPoco { get; set; }
        private IGlobalContext objectContext;
        private TenantManagementRepository entityRepository;
        private TenantManagementLicenseRepository tenantManagementLicenseRepository;
        private TenantAddOnRepository tenantAddOnRepository;
        public TenantManagementService(IGlobalContext objectContext,int tenant = 0)
        {
            this.objectContext = objectContext;
            this.entityRepository = new TenantManagementRepository(objectContext);
            this.tenantManagementLicenseRepository = new TenantManagementLicenseRepository(objectContext);
            this.tenantAddOnRepository = new TenantAddOnRepository(objectContext);
        }

        private List<TenantManagementLicensePM> licensesChangeSet;
        private List<TenantAddOnPM> addOnsChangeSet;
        public void SetChangeSet(List<TenantManagementLicensePM> licensesChangeSet, List<TenantAddOnPM> addOnsChangeSet)
        {
            this.licensesChangeSet = licensesChangeSet;
            this.addOnsChangeSet = addOnsChangeSet;
        }

        public void Create(TenantManagementPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
        }

        public void Update(TenantManagementPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.tenant = entityPM.Id;
            this.entityPM = entityPM;
            this.entityPoco = entityRepository.GetSingleTenantManagement(entityPM.Id);

            if (mapComposition)
            {
                this.licensesChangeSet = entityPM.TenantManagementLicenses;
                this.addOnsChangeSet = entityPM.AddOns;
            }

            TenantManagementValidating.Validate(entityPM, entityPoco, isNewEntity, this.entityRepository);

            string entityName = "TenantManagement" + entityPM.Id;
            string entityPmName = "TenantManagementPM" + entityPM.Id;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            this.UpdateParticipants();
            this.UpdateDocumentsArchive();
            this.UpdateLicenses();
            this.UpdateAddOns();
            this.UpdateGlobalTenants();
            this.ClearAllUsersCache();
            this.BrandingEvent();
            this.CheckParentTenants();

            TenantManagementTracing.Trace(entityPM, entityPoco, isNewEntity);
            TenantManagementMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();

        }

        private void BrandingEvent()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (entityPoco.EnableBranding != entityPM.EnableBranding)
                {
                    string updateByUserId = "";
                    int tenant = entityPM.Id;

                    if (!string.IsNullOrEmpty(entityPM.UpdateByUserId))
                    {
                        if (!string.IsNullOrEmpty(entityPM.UpdateByUserId.Split('^')[0])) updateByUserId = entityPM.UpdateByUserId.Split('^')[0];
                        if (!string.IsNullOrEmpty(entityPM.UpdateByUserId.Split('^')[1])) tenant = Int32.Parse(entityPM.UpdateByUserId.Split('^')[1]);
                
                    }
                  
                    string eventCode = entityPM.EnableBranding ? "BREN" : "BRDI";
                    string note = entityPM.EnableBranding ? "Branding enabled" : "Branding disabled";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = eventCode,
                        EntityId = entityPM.Id.ToString(),
                        ObjectTableName = "TenantManagement",
                        Notes = note,
                        UserId = updateByUserId,
                        EventDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Id),
                    });


                }
                scope.Complete();
            }
        }

        private void UpdateParticipants()
        {
            if (entityPM.TTY != entityPoco.TTY)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    ParticipantRepository participantRepository = new ParticipantRepository(entityPM.Id);
                    IQueryable<Participant> Participants = participantRepository.GetParticipantsByForwarderTenant(entityPM.Id);

                    foreach (Participant p in Participants)
                    {
                        p.TTY = entityPM.TTY;
                        participantRepository.Update(p);
                    }

                    participantRepository.SubmitChanges();

                    scope.Complete();
                }
            }
        }
        private void UpdateDocumentsArchive()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                var tenantRepository = new TenantRepository(0);
                Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Id);

                if (currentTenant != null && entityPM.PackageCode == "IMPO" && !currentTenant.IsDocumentsArchive)
                {
                    currentTenant.IsDocumentsArchive = true;
                    tenantRepository.Update(currentTenant);
                    tenantRepository.SubmitChanges();
                }

                else if (currentTenant != null && entityPM.PackageCode != "IMPO" && currentTenant.IsDocumentsArchive)
                {
                    currentTenant.IsDocumentsArchive = false;
                    tenantRepository.Update(currentTenant);
                    tenantRepository.SubmitChanges();
                }

                scope.Complete();
            }
        }
        private void UpdateLicenses()
        {
            if (isNewEntity)
            {

            }

            else
            {
                if (licensesChangeSet != null)
                {
                    foreach (TenantManagementLicensePM itemPM in licensesChangeSet)
                    {
                        switch (itemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateTenantManagementLicense(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateTenantManagementLicense(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteTenantManagementLicense(itemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }
            }
        }
        private void UpdateAddOns()
        {
            if (isNewEntity)
            {

            }

            else
            {
                if (addOnsChangeSet != null)
                {
                    foreach (TenantAddOnPM itemPM in addOnsChangeSet)
                    {
                        switch (itemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateTenantAddOn(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateTenantAddOn(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteTenantAddOn(itemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }
            }
        }

        private void CreateTenantManagementLicense(TenantManagementLicensePM itemPM)
        {
            //bool isExists = (from d in this.objectContext.TenantManagementLicenses
            //                 where d.PackageCode == itemPM.PackageCode
            //                 select d).Any();

            itemPM.Id = IdCounter.GetNumber("TenantManagementLicense", tenant).ToString();            
            itemPM.Tenant = tenant;

            TenantManagementLicense itemPoco = new TenantManagementLicense()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                PackageCode = itemPM.PackageCode,
                NumberOfUsers = itemPM.NumberOfUsers,
            };

            tenantManagementLicenseRepository.Add(itemPoco);
        }
        private void UpdateGlobalTenants()
        {
           GlobalTenantRepository Rep = new GlobalTenantRepository();
           GlobalTenant Gtenant = Rep.GetGlobalTenantsByTenant(this.entityPM.Id);
           if (Gtenant != null)
           {
               Gtenant.PrivateLabelId = this.entityPM.PrivateLabelId;
               Rep.Update(Gtenant);
               Rep.SubmitChanges();
           }
        }
        private void UpdateTenantManagementLicense(TenantManagementLicensePM itemPM)
        {
            TenantManagementLicense itemPoco = tenantManagementLicenseRepository.GetSingleTenantManagementLicense(itemPM.Id);

            if (itemPoco != null)
            {
                itemPoco.PackageCode = itemPM.PackageCode;
                itemPoco.NumberOfUsers = itemPM.NumberOfUsers;
                tenantManagementLicenseRepository.Update(itemPoco);
            }
        }
        private void DeleteTenantManagementLicense(TenantManagementLicensePM itemPM)
        {
            TenantManagementLicense itemPoco = tenantManagementLicenseRepository.GetSingleTenantManagementLicense(itemPM.Id);

            if (itemPoco != null)
            {
                tenantManagementLicenseRepository.Remove(itemPoco);
            }
        }

        private void CreateTenantAddOn(TenantAddOnPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("TenantAddOn", tenant).ToString();

            TenantAddOn itemPoco = new TenantAddOn()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                PackageCode = itemPM.PackageCode,
            };

            tenantAddOnRepository.Add(itemPoco);
        }
        private void UpdateTenantAddOn(TenantAddOnPM itemPM)
        {
            TenantAddOn itemPoco = tenantAddOnRepository.GetSingleTenantAddOn(itemPM.Id);

            if (itemPoco != null)
            {
                itemPoco.PackageCode = itemPM.PackageCode;
                tenantAddOnRepository.Update(itemPoco);
            }
        }
        private void DeleteTenantAddOn(TenantAddOnPM itemPM)
        {
            TenantAddOn itemPoco = tenantAddOnRepository.GetSingleTenantAddOn(itemPM.Id);

            if (itemPoco != null)
            {
                tenantAddOnRepository.Remove(itemPoco);
            }
        }

        private void ClearAllUsersCache()
        {
            bool isClearing = false;

            if (entityPM.IsMultiPackage != entityPoco.IsMultiPackage)
            {
                isClearing = true;
            }

            else if (entityPM.PackageCode != entityPoco.PackageCode)
            {
                isClearing = true;
            }

            else if (addOnsChangeSet != null && addOnsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).Any())
            {
                isClearing = true;
            }

            else if (licensesChangeSet != null && licensesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).Any())
            {
                isClearing = true;
            }

            if (isClearing)
            {
                bool isUsingNewCode = true;

                if (isUsingNewCode)
                {
                    string iKeys = null;

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        ICommonDataContext iContext = CommonDataContext.GetContext(tenant);
                        var iData = (from d in iContext.Users.Include("Contact")
                                     where d.Tenant == tenant && d.Contact.UserType == "R"
                                     select new
                                     {
                                         Id = d.Id,
                                         Email = d.Contact.Email,
                                     }).ToList();

                        foreach (var item in iData)
                        {
                            string key = item.Email + "_" + item.Id + "_info";

                            if (iKeys == null)
                            {
                                iKeys = key;
                            }

                            else
                            {
                                iKeys += "$" + key;
                            }
                        }

                        scope.Complete();
                    }

                    if (iKeys != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(iKeys);
                    }
                }

                else
                {
                    //List<User> allUsers = new List<User>();
                    //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //    UserRepository userRepository = new UserRepository(entityPM.Id);
                    //    allUsers = userRepository.GetUsers(entityPM.Id).ToList();

                    //    scope.Complete();
                    //}

                    //foreach (User item in allUsers)
                    //{
                    //    if (item.Contact != null)
                    //    {
                    //        string email = item.Contact.Email;
                    //        if (!string.IsNullOrEmpty(email))
                    //        {
                    //            string key = email + "_" + entityPM.Id + "_info";
                    //            CacheManager.CacheWrapper.Invalidate(key);
                    //        }
                    //    }
                    //}
                }
            }
        }

        private void CheckParentTenants()
        {
            if (!entityPM.IsParentTenant && entityPoco.IsParentTenant)
            {
                List<TenantManagement> childTenants = entityRepository.GetChildTenantManagements(this.entityPM.Id).ToList();

                if(childTenants.Count > 0)
                {
                    throw new Exception("Sorry You can't unckek Parent Tenant since there are connected child tenants!");
                }
            }
        }
    }
}
