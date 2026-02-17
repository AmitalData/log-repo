using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.WebServices;
using Simplog.Global.Data.GlobalModel;
using System.Text;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
       

        public void UpdateContactList(ContactList currentEntity)
        {

        }

        public bool DoesContactEmailExist(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactRepository = new ContactRepository(tenant);
            return (ContactRepository.GetActiveContacts(tenant).Where(d => d.Email == email && d.Tenant == tenant)).Any();
        }

        [Invoke]
        public bool IsEmailExists(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            ContactRepository = new ContactRepository(objectContext);


            bool result = false;
            bool isContactExists = ContactRepository.IsContactByEmailExists(email, tenant);

            result = (isContactExists);
            return result;
        }

        public ContactPM GetContactByEmailOnly(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            //ContactRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetContactByEmailOnly(email, tenant);
        }

        public ContactPM GetSingleContact(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            //ContactRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetSinglePM(id, tenant);
        }

        public ContactList GetSingleContactList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            ContactRepository = new ContactRepository(tenant);
            ContactList contactList = null;
            Contact contact = ContactRepository.GetSingleContact(id, tenant, false);

            if (contact != null)
            {
                List<Contact> singleEntityList = new List<Contact>();
                singleEntityList.Add(contact);

                IQueryable<Contact> iQueryable = singleEntityList.AsQueryable();
                ContactQuery contactQuery = new ContactQuery(ContactRepository);

                IQueryable<ContactList> iQueryableEntityList = contactQuery.GetIQueryableEntityList(iQueryable);
                contactList = iQueryableEntityList.FirstOrDefault();
            }
            return contactList;
        }

        public IQueryable<ContactList> GetContactLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            ContactRepository = new ContactRepository(tenant);
            IQueryable<Contact> contacts = ContactRepository.GetActiveContacts(tenant);
            ContactQuery contactQuery = new ContactQuery(ContactRepository);
            IQueryable<ContactList> query2 = contactQuery.GetIQueryableEntityList(contacts);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ContactList> GetContactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            ContactRepository = new ContactRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Contact> contacts = ContactRepository.GetContacts(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            ContactCustomFilter customfilters = new ContactCustomFilter(tenant);
            contacts = customfilters.GetFilteredQuery(queryOperations, contacts);

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            contacts = filter.GetFilteredQuery<Contact>(nonListQueryOperation, contacts);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            ContactQuery contactQuery = new ContactQuery(ContactRepository);
            IQueryable<ContactList> query2 = contactQuery.GetIQueryableEntityList(contacts);
            query2 = filter.GetFilteredQuery<ContactList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ContactList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Contact", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.EnglishName);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetContactFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            ContactRepository = new ContactRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Contact> contacts = ContactRepository.GetContacts(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            contacts = filter.GetFilteredQuery<Contact>(nonListQueryOperation, contacts);

            ContactCustomFilter customfilters = new ContactCustomFilter(tenant);
            contacts = customfilters.GetFilteredQuery(queryOperations, contacts);

            ContactQuery contactQuery = new ContactQuery(ContactRepository);
            IQueryable<ContactList> query2 = contactQuery.GetIQueryableEntityList(contacts);
            query2 = filter.GetFilteredQuery<ContactList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public List<ContactPM> GetContactsByEmail(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            ContactRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(ContactRepository);
            return contactQuery.GetContactsByEmail(email, tenant);
        }

        public void InsertContact(ContactPM entityPM)
        {
            if (!entityPM.IsCreatedWithPartner)
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Contact", "NEW", entityPM.Tenant);

                if (objectContext == null)
                {
                    objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                }

                ContactService service = new ContactService(objectContext, entityPM.Tenant);
                service.Create(entityPM);
            }
        }

        public void UpdateContact(ContactPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Contact", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            if (entityPM.IsChangeSignatur)
            {
                HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                entityPM.SignatureHtml = htmlEditorHelper.ConvertXmlByteToHtmlByte(entityPM.Signature);
            }

            ContactService service = new ContactService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }

        public void DeleteContact(ContactPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            if (entityPM.Id != null)
            {
                if (entityPM.Id.Contains(","))
                {
                    string[] ids = entityPM.Id.Split(',');
                    entityPM.Id = ids[0];
                }
            }

            ContactRepository = new ContactRepository(objectContext);
            Contact entity = ContactRepository.GetSingleContact(entityPM.Id, entityPM.Tenant);
            if (entity != null)
            {
                ContactRepository.Remove(entity);
            }
        }

        public IQueryable<ContactPM> GetContactsbyCardID(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            ContactQuery contactQuery = new ContactQuery(tenant);
            IQueryable<ContactPM> contacts = contactQuery.GetContactsbyCardId(id, tenant);

            return contacts;
        }

        [Invoke]
        public void ContactInternetAccessInvitation(SharedLogisticContactPM sharedLogisticsContact)
        {
            SecurityUtility.AuthenticationOnTenant(sharedLogisticsContact.Tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", sharedLogisticsContact.Tenant);
            SharedLogisticContactHelper sharedLogisticContactHelper = new SharedLogisticContactHelper();

            sharedLogisticContactHelper.InternetAccessInvitation(sharedLogisticsContact, objectContext);


        }       
    }
}
