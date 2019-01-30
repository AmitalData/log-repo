using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ComputingPartnerService
    {
        bool isNewEntity;
        private int tenant;
        private string loggedContactId;
        public ComputingPartner Poco { get; set; }
        private ComputingPartnerPM entityPM;
        private ICommonDataContext objectContext;
        private ContactRepository contactRepository;

        private ComputingPartnerRepository entityRepository;
        private ComputingPartnerTableRepository computingPartnerTableRepository;
        public ComputingPartnerService(ICommonDataContext objectContext, ComputingPartnerPM entityPM)
        {
            this.tenant = entityPM.LoggedTenantId;
            this.entityPM = entityPM;
            this.objectContext = objectContext;
            this.entityRepository = new ComputingPartnerRepository(objectContext);
            this.computingPartnerTableRepository = new ComputingPartnerTableRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(tenant);

            string email = HttpContext.Current.User.Identity.Name;
            Contact loggedContact= contactRepository.GetSingleContactByEmail(email, tenant);

            if (loggedContact == null)
            {
                loggedContact =  contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant);
                this.loggedContactId = loggedContact.Id;
            }

            else
            {
                if (loggedContact.Tenant != tenant)
                {
                    loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant);
                    this.loggedContactId = loggedContact.Id;
                }
                else
                {
                    this.loggedContactId = loggedContact.Id;
                }
            }
            


        }

        public void Create()
        {
            this.isNewEntity = true;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "Computing Partner");
                throw new ApplicationException(msg);
            }

            else
            {
                bool isDoublicatedTablesNames = this.IsDoublicatedTablesNames();

                if (isDoublicatedTablesNames)
                {
                    throw new ApplicationException("More than one table has the same name");
                }

                else
                {
                    bool isDoublicatedTablesObjectTables = this.IsDoublicatedTablesObjectTables();

                    if (isDoublicatedTablesObjectTables)
                    {
                        throw new ApplicationException("More than one table has the same Object Table");
                    }

                    else
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

                        this.entityPM.Id = IdCounter.GetNumber("ComputingPartner", tenant).ToString();
                        this.entityPM.CreateDate = entityPM.UpdateDate = todayDateTime;
                        this.entityPM.CreatedByUserId = entityPM.UpdatedByUserId = this.loggedContactId;

                        this.Poco = new ComputingPartner()
                        {
                            Id = entityPM.Id,
                           Tenant=entityPM.Tenant,
                           InActive=false,
                        };

                        this.UpdatePartnerTables();
                        this.ComputingPartnerNameValidation(entityPM);
                        ComputingPartnerMapping.MapEntity(entityPM, Poco, isNewEntity);

                        entityRepository.Add(Poco);
                        entityRepository.SubmitChanges();
                    }
                }
            }
        }

        private List<ComputingPartnerTablePM> myTablesChangeSet;
        public void Update(List<ComputingPartnerTablePM> myTablesChangeSet)
        {
            this.isNewEntity = false;
            this.myTablesChangeSet = myTablesChangeSet;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "Computing Partner");
                throw new ApplicationException(msg);
            }

            else
            {
                bool isDoublicatedTablesNames = this.IsDoublicatedTablesNames();

                if (isDoublicatedTablesNames)
                {
                    throw new ApplicationException("More than one table has the same name");
                }

                else
                {
                    bool isDoublicatedTablesObjectTables = this.IsDoublicatedTablesObjectTables();

                    if (isDoublicatedTablesObjectTables)
                    {
                        throw new ApplicationException("More than one table has the same Object Table");
                    }

                    else
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

                        this.entityPM.UpdateDate = todayDateTime;
                        this.entityPM.UpdatedByUserId = this.loggedContactId;

                        this.Poco = entityRepository.GetSingleComputingPartner(entityPM.Id,tenant);

                        this.UpdatePartnerTables();
                        this.ComputingPartnerNameValidation(entityPM);
                        ComputingPartnerMapping.MapEntity(entityPM, Poco, isNewEntity);

                        entityRepository.Update(Poco);
                        entityRepository.SubmitChanges();
                    }
                }
            }
        }

        private bool IsEntityExists()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                myResult = (from a in entityRepository.GetComputingPartners(tenant)
                            where  a.Code==entityPM.Code && a.Tenant==entityPM.Tenant
                            select a).Any();
            }
            else
            {
                myResult = (from a in entityRepository.GetComputingPartners(tenant)
                            where  a.Code == entityPM.Code  && a.Id!=entityPM.Id && a.Tenant==entityPM.Tenant
                            select a).Any();
            }
            
            return myResult;
        }
        private bool IsDoublicatedTablesNames()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                var myGroup = (from f in entityPM.PartnerTables
                               group f by f.Name into g
                               select new
                               {
                                   Name = g.Key,
                                   Count = g.Count()
                               });

                if (myGroup.Where(d => d.Count > 1).Any())
                {
                    myResult = true;
                }
            }

            else
            {
                var myGroup = (from f in myTablesChangeSet
                               group f by f.Name into g
                               select new
                               {
                                   Name = g.Key,
                                   Count = g.Count()
                               });

                if (myGroup.Where(d => d.Count > 1).Any())
                {
                    myResult = true;
                }
            }

            return myResult;
        }
        private bool IsDoublicatedTablesObjectTables()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                var myGroup = (from f in entityPM.PartnerTables
                               group f by f.ObjectTableId into g
                               select new
                               {
                                   ObjectTableId = g.Key,
                                   Count = g.Count()
                               });

                if (myGroup.Where(d => d.Count > 1).Any())
                {
                    myResult = true;
                }
            }

            else
            {
                var myGroup = (from f in myTablesChangeSet
                               group f by f.ObjectTableId into g
                               select new
                               {
                                   ObjectTableId = g.Key,
                                   Count = g.Count()
                               });

                if (myGroup.Where(d => d.Count > 1).Any())
                {
                    myResult = true;
                }
            }

            return myResult;
        }

        private void ComputingPartnerNameValidation(ComputingPartnerPM entityPM) {
            if (entityPM.Tenant == 0)
            {
                if (!entityPM.Code.StartsWith("G-"))
                {
                    throw new ApplicationException("The code must starts with G-");
                }
            }

            else
            {
                if (entityPM.Code.StartsWith("G-"))
                {
                    throw new ApplicationException("The code mustn't starts with G-");
                }

            }

        }

        private void UpdatePartnerTables()
        {
            if (isNewEntity)
            {
                foreach (ComputingPartnerTablePM itemPM in entityPM.PartnerTables)
                {
                    this.CreatePartnerTable(itemPM);
                }
            }

            else
            {
                foreach (ComputingPartnerTablePM itemPM in myTablesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreatePartnerTable(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdatePartnerTable(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeletePartnerTable(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void CreatePartnerTable(ComputingPartnerTablePM itemPM)
        {
            itemPM.Tenant = tenant;
            itemPM.ComputingPartnerId = entityPM.Id;
            itemPM.CreatedByUserId = this.loggedContactId;
            itemPM.UpdatedByUserId = this.loggedContactId;
            ComputingPartnerTable itemPoco = new ComputingPartnerTable()
            {
                Tenant = itemPM.Tenant,
                ObjectTableId = itemPM.ObjectTableId,
                ComputingPartnerId = itemPM.ComputingPartnerId,
            };

            ComputingPartnerTableMapping.MapEntity(itemPM, itemPoco, true);
            computingPartnerTableRepository.Add(itemPoco);
        }
        private void UpdatePartnerTable(ComputingPartnerTablePM itemPM)
        {
            ComputingPartnerTable itemPoco = computingPartnerTableRepository.GetSingleComputingPartnerTable(itemPM.Tenant, itemPM.ObjectTableId, itemPM.ComputingPartnerId);
            itemPM.UpdatedByUserId = this.loggedContactId;
            ComputingPartnerTableMapping.MapEntity(itemPM, itemPoco, false);
            computingPartnerTableRepository.Update(itemPoco);
        }
        private void DeletePartnerTable(ComputingPartnerTablePM itemPM)
        {
            ComputingPartnerTable itemPoco = computingPartnerTableRepository.GetSingleComputingPartnerTable(itemPM.Tenant, itemPM.ObjectTableId, itemPM.ComputingPartnerId);
            computingPartnerTableRepository.Remove(itemPoco);
        }
    }
}
