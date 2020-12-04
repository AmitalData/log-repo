using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
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
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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
        const string CargoTrackingImageFolder = "CargoTrackingImages";
        const string CargoTrackingImageExtensionType = "jpg";
        private TenantManagementPM entityPM;
        public TenantManagement entityPoco { get; set; }
        private IGlobalContext objectContext;
        private ICommonDataContext CommonContext;
        private TenantManagementRepository entityRepository;
        private TenantManagementLicenseRepository tenantManagementLicenseRepository;
        private TenantAddOnRepository tenantAddOnRepository;
        public TenantManagementService(IGlobalContext objectContext, int tenant = 0, ICommonDataContext CommonContext=null)
        {
            this.objectContext = objectContext;
            this.CommonContext = CommonContext;
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

            this.CheckSubscriptionSwitch();

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
            this.UpdateGlobalTenants();
            
            this.UpdateLicenses();
            this.UpdateAddOns();
            this.ClearAllUsersCache();
            this.BrandingEvent();
            this.CheckParentTenants();         
            this.UpdateCargoTrackingColors();
            this.DeleteOldImages();
            if (entityPM.Id == 341)
            {
                this.UpdateCustomer();
            }

            TenantManagementTracing.Trace(entityPM, entityPoco, isNewEntity);
            TenantManagementMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }


        private void DeleteOldImages()
        {
             
            if (this.entityPM.BackgroundId != this.entityPoco.BackgroundId)
            {
              DeleteImageFromCargoTrackingImages(entityPoco.BackgroundId);
            }
        }
        public void DeleteImageFromCargoTrackingImages(string imgId)
        {
            string imagePath = GetFilePath(GetFileNameWithExtension(imgId));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

        }
        private string GetFilePath(string fileName)
        {
            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/" + CargoTrackingImageFolder + "/");
            string filePath = folderPath + fileName;
            return filePath;
        }

        private string GetFileNameWithExtension(string imgName)
        {
            return imgName + "." + CargoTrackingImageExtensionType;
        }
        private void  UpdateCargoTrackingColors()
        {
            int index =  (entityPM.MainColor!= null && entityPM.MainColor.Length > 7) ? 3 : 1;
            this.entityPM.MainColor= (this.entityPM.MainColor!= null && entityPM.MainColorOpacity != null) ? "#" +entityPM.MainColorOpacity + entityPM.MainColor.ToString().Substring(index, 6): entityPM.MainColor;
            index =( entityPM.SecondaryColor!= null && entityPM.SecondaryColor.Length > 7) ? 3 : 1;
            this.entityPM.SecondaryColor = (entityPM.SecondaryColor!= null && entityPM.SecondaryColorOpacity != null ) ? "#" + entityPM.SecondaryColorOpacity + entityPM.SecondaryColor.ToString().Substring(index, 6) : entityPM.SecondaryColor;


        }
        private void UpdateCustomer()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                CustomerQuery customerQuery = new CustomerQuery(entityPM.Id);
                CustomerPM customer = customerQuery.GetSinglePMByExternalId(entityPM.Id.ToString(), entityPM.Id);
                if (customer != null)
                {
                    string numberOfUsers = null;
                    if (entityPM.TotalNumberOfUsers != null)
                    {
                        numberOfUsers = entityPM.TotalNumberOfUsers.ToString();
                    }

                    customer.Field1 = new CustomFieldClass("Field1", "Customer", numberOfUsers);
                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Id);
                    CustomerService service = new CustomerService(MyContext, customer);
                    service.Update();
                }

                scope.Complete();
            }
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
                var LBtenantRepository = new LogBoxTenantSettingRepository(0);
                LogBoxTenantSetting LBcurrentTenant = LBtenantRepository.GetSingleLBTenant(entityPM.Id);

                if (LBcurrentTenant!= null && currentTenant != null && entityPM.PackageCode == "IMPO" && !LBcurrentTenant.IsDocumentsArchive)
                {
                    LBcurrentTenant.IsDocumentsArchive = true;
                    tenantRepository.Update(currentTenant);
                    tenantRepository.SubmitChanges();
                }

                else if (LBcurrentTenant != null && currentTenant != null && entityPM.PackageCode != "IMPO" && LBcurrentTenant.IsDocumentsArchive)
                {
                    LBcurrentTenant.IsDocumentsArchive = false;
                    tenantRepository.Update(currentTenant);
                    tenantRepository.SubmitChanges();
                }

                scope.Complete();
            }
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
        private void CheckSubscriptionSwitch()
        {
            if (entityPM.MainAdditionalPackageApplied != entityPoco.MainAdditionalPackageApplied)
            {
                if (entityPM.MainAdditionalPackageApplied)
                {
                    if (entityPM.IsMultiPackage)
                    {
                        if (entityPM.TenantManagementLicenses.Count > 0)
                        {
                            if (!entityPM.TenantManagementLicenses.Where(d => d.PackageCode == entityPM.PackageCode).Any())
                            {
                                throw new ApplicationException("Main package should be one of the additional packages");
                            }

                            if (entityPM.TenantManagementLicenses.Where(d => d.PackageCode == entityPM.PackageCode && d.ChangeSetOp != ChangeSetOperation.Delete).Any())
                            {
                                throw new ApplicationException("Main package should be deleted from the additional packages");
                            }
                        }

                        this.SwitchToMainAdditionalPackageMulti();                        
                    }

                    else
                    {
                        this.SwitchToMainAdditionalPackageSingle();
                    }
                }

                else
                {
                    throw new ApplicationException("Switching to Single/Multi Package is not allowed");                   
                }
            }
        }

        private void SwitchToMainAdditionalPackageMulti()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ICommonDataContext iContext = CommonDataContext.GetContext(tenant);

                List<User> allUsers = (from myUser in iContext.Users.Include("Contact")
                                       join db_UserLicenses in iContext.UserLicenses on myUser.Id equals db_UserLicenses.Id into UserLicenses
                                       from iUserLicense in UserLicenses.DefaultIfEmpty()
                                       where
                                       myUser.Tenant == tenant
                                       && myUser.AdditionalPackagesOnly == false
                                       && myUser.Contact.UserType == "R"
                                       && myUser.Contact.InActive == false
                                       && !iContext.UserLicenses.Any(f => f.UserId == myUser.Id && f.PackageCode == entityPM.PackageCode)
                                       select myUser).ToList();

                if (allUsers.Count > 0)
                {
                    UserRepository userRepository = new UserRepository(iContext);

                    foreach (User item in allUsers)
                    {
                        item.AdditionalPackagesOnly = true;
                        userRepository.Update(item);
                    }

                    userRepository.SubmitChanges();
                }

                this.DeleteLicennsesForMainPackage(iContext);

                scope.Complete();
            }
        }
        private void SwitchToMainAdditionalPackageSingle()
        {
            /*
                select * from Users
                join Contacts on Users.Id = Contacts.Id
                where Users.Tenant = 1
                and Contacts.InActive = 0
                and Contacts.UserType = 'R'
             * */

            /*                          
                select * from UserLicenses
                where UserId in
                (
                select Users.Id from Users
                join Contacts on Users.Id = Contacts.Id
                where Users.Tenant = 1
                and Contacts.InActive = 0
                and Contacts.UserType = 'R'
                )             
             * */

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    ICommonDataContext iContext = CommonDataContext.GetContext(tenant);

            //    List<string> allUsersIds = (from iUser in iContext.Users.Include("Contact")
            //                                join db_UserLicenses in iContext.UserLicenses on iUser.Id equals db_UserLicenses.Id into UserLicenses
            //                                from iUserLicense in UserLicenses.DefaultIfEmpty()
            //                                where iUser.Tenant == tenant
            //                                && iUser.Contact.UserType == "R"
            //                                && iUser.Contact.InActive == false
            //                                && !iContext.UserLicenses.Any(f => f.UserId == iUser.Id && f.PackageCode == entityPM.PackageCode)
            //                                select iUser.Id).ToList();

            //    if (allUsersIds.Count > 0)
            //    {
            //        UserLicenseRepository userLicenseRepository = new UserLicenseRepository(iContext);

            //        foreach (string id in allUsersIds)
            //        {
            //            UserLicense userLicense = new UserLicense()
            //            {
            //                Id = IdCounter.GetNumber("UserLicense", tenant).ToString(),
            //                Tenant = entityPM.Id,
            //                PackageCode = entityPM.PackageCode,
            //                UserId = id,
            //            };

            //            userLicenseRepository.Add(userLicense);
            //        }

            //        userLicenseRepository.SubmitChanges();
            //    }

            //    scope.Complete();
            //}
        }
        private void DeleteLicennsesForMainPackage(ICommonDataContext iContext)
        {
            UserLicenseRepository userLicenseRepository = new UserLicenseRepository(iContext);
            List<UserLicense> userLicenses = userLicenseRepository.GetUserLicensesByPackageCode(entityPM.PackageCode, tenant);

            if (userLicenses.Count > 0)
            {
                foreach (UserLicense item in userLicenses)
                {
                    userLicenseRepository.Remove(item);
                }

                userLicenseRepository.SubmitChanges();
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
                FreeUsers = itemPM.FreeUsers,
                Price = itemPM.FreeUsers,
                TotalPrice = itemPM.TotalPrice,
            };

            tenantManagementLicenseRepository.Add(itemPoco);
        }
        private void UpdateTenantManagementLicense(TenantManagementLicensePM itemPM)
        {
            TenantManagementLicense itemPoco = tenantManagementLicenseRepository.GetSingleTenantManagementLicense(itemPM.Id);

            if (itemPoco != null)
            {
                itemPoco.PackageCode = itemPM.PackageCode;
                itemPoco.NumberOfUsers = itemPM.NumberOfUsers;
                itemPoco.FreeUsers = itemPM.FreeUsers;
                itemPoco.Price = itemPM.Price;
                itemPoco.TotalPrice = itemPM.TotalPrice;
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

                if (childTenants.Count > 0)
                {
                    throw new Exception("Sorry You can't unckek Parent Tenant since there are connected child tenants!");
                }
            }
        }
    }
}
