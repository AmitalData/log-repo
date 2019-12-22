using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<OccasionContactList> GetOccasionContactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            List<ObjectField> myObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("OccasionContact", tenant);
            string occasionId = "";
            string filterBy = "";

            if (queryOperations.QueryFilterItems != null)
            {
                foreach (QueryFilterItem filter in queryOperations.QueryFilterItems)
                {
                    if (filter.FieldName == "OccasionId")
                    {
                        occasionId = filter.FieldValue.ToString();
                    }

                    else if (filter.FieldName == "FilterBy")
                    {
                        filterBy = filter.FieldValue.ToString();
                    }

                    else
                    {
                        ObjectField field = myObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                        }

                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }
            }

            ICRMContext MyContext = CRMContext.GetContext(tenant);
            OccasionInviteeRepository entityRepository = new OccasionInviteeRepository(MyContext);
            IQueryable<OccasionInvitee> entityPocos = entityRepository.GetOccasionInviteesByOccasion(occasionId, tenant);

            switch (filterBy)
            {
                case "ALL":
                    {
                        break;
                    }

                case "INVT":
                    {
                        entityPocos = entityPocos.Where(d => d.Invited);
                        break;
                    }

                case "PART":
                    {
                        entityPocos = entityPocos.Where(d => d.Participated);
                        break;
                    }
            }

            GenericFilter genericFilter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            int skippedEntities = queryOperations.PageIndex;

            IQueryable<OccasionContactList> entityLists = (from f in entityPocos.Include("Contact")
                                                           where f.Tenant == tenant
                                                           select new OccasionContactList()
                                                           {
                                                               Id = f.ContactId,
                                                               Tenant = f.Tenant,
                                                               Name = f.Contact == null ? null : f.Contact.EnglishName,
                                                               Email = f.Contact == null ? null : f.Contact.Email,
                                                               BusinessPhone = f.Contact == null ? null : f.Contact.BusinessPhone,
                                                               Mobile = f.Contact == null ? null : f.Contact.Mobile,
                                                               Position = f.Contact == null ? null : f.Contact.Position,
                                                               Notes = f.Contact == null ? null : f.Contact.Notes,
                                                               Invited = f.Invited,
                                                               Participated = f.Participated,
                                                           });

            List<OccasionContactList> tempList = entityLists.ToList();
            foreach (OccasionContactList item in tempList)
            {
                item.Customers = this.BuildCustomers(item);
            }

            entityLists = tempList.AsQueryable();

            entityLists = genericFilter.GetFilteredQuery<OccasionContactList>(nonListQueryOperation, entityLists);
            entityLists = genericFilter.GetFilteredQuery<OccasionContactList>(listQueryOperation, entityLists);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(OccasionContactList).GetProperty(queryOperations.SortByColumnName);


                ObjectField objectField = (from a in myObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        entityLists = sortClass.GetSorterQuery<OccasionContactList, string>(queryOperations, entityLists);
                    }

                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                            case "lookup":
                                {
                                    entityLists = sortClass.GetSorterQuery<OccasionContactList, string>(queryOperations, entityLists);
                                    break;
                                }

                            case "sigdouble":
                            case "double":
                                {
                                    entityLists = sortClass.GetSorterQuery<OccasionContactList, double>(queryOperations, entityLists);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    entityLists = sortClass.GetSorterQuery<OccasionContactList, DateTime>(queryOperations, entityLists);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    entityLists = sortClass.GetSorterQuery<OccasionContactList, int>(queryOperations, entityLists);
                                    break;
                                }
                            case "boolean":
                                {
                                    entityLists = sortClass.GetSorterQuery<OccasionContactList, bool>(queryOperations, entityLists);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    entityLists = sortClass.GetSorterQuery<OccasionContactList, decimal>(queryOperations, entityLists);
                                    break;
                                }

                            default:
                                {
                                    entityLists = entityLists.OrderBy(d => d.Id);
                                    break;
                                }
                        }
                    }
                }
            }

            else
            {
                entityLists = entityLists.OrderBy(d => d.Id);
            }

            entityLists = entityLists.Skip(skippedEntities);
            entityLists = entityLists.Take(queryOperations.PageSize);

            List<OccasionContactList> listResult = entityLists.ToList();
            return listResult;
        }

        public int GetOccasionContactFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            List<ObjectField> myObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("OccasionContact", tenant);
            string occasionId = "";
            string filterBy = "";

            if (queryOperations.QueryFilterItems != null)
            {
                foreach (QueryFilterItem filter in queryOperations.QueryFilterItems)
                {
                    if (filter.FieldName == "OccasionId")
                    {
                        occasionId = filter.FieldValue.ToString();
                    }

                    else if (filter.FieldName == "FilterBy")
                    {
                        filterBy = filter.FieldValue.ToString();
                    }

                    else
                    {
                        ObjectField field = myObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                        }

                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }
            }

            ICRMContext MyContext = CRMContext.GetContext(tenant);
            OccasionInviteeRepository entityRepository = new OccasionInviteeRepository(MyContext);
            IQueryable<OccasionInvitee> entityPocos = entityRepository.GetOccasionInviteesByOccasion(occasionId, tenant);

            switch (filterBy)
            {
                case "ALL":
                    {
                        break;
                    }

                case "INVT":
                    {
                        entityPocos = entityPocos.Where(d => d.Invited);
                        break;
                    }

                case "PART":
                    {
                        entityPocos = entityPocos.Where(d => d.Participated);
                        break;
                    }
            }

            GenericFilter genericFilter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            int skippedEntities = queryOperations.PageIndex;

            IQueryable<OccasionContactList> entityLists = (from f in entityPocos.Include("Contact")
                                                           where f.Tenant == tenant
                                                           select new OccasionContactList()
                                                           {
                                                               Id = f.ContactId,
                                                               Tenant = f.Tenant,
                                                               Name = f.Contact == null ? null : f.Contact.EnglishName,
                                                               Email = f.Contact == null ? null : f.Contact.Email,
                                                               BusinessPhone = f.Contact == null ? null : f.Contact.BusinessPhone,
                                                               Mobile = f.Contact == null ? null : f.Contact.Mobile,
                                                               Position = f.Contact == null ? null : f.Contact.Position,
                                                               Notes = f.Contact == null ? null : f.Contact.Notes,
                                                               Invited = f.Invited,
                                                               Participated = f.Participated,
                                                           });

            List<OccasionContactList> tempList = entityLists.ToList();
            foreach (OccasionContactList item in tempList)
            {
                item.Customers = this.BuildCustomers(item);
            }

            entityLists = tempList.AsQueryable();

            entityLists = genericFilter.GetFilteredQuery<OccasionContactList>(nonListQueryOperation, entityLists);
            entityLists = genericFilter.GetFilteredQuery<OccasionContactList>(listQueryOperation, entityLists);
            
            entityLists = entityLists.Skip(skippedEntities);
            entityLists = entityLists.Take(queryOperations.PageSize);

            int count = entityLists.Count();
            return count;
        }
        
        private string BuildCustomers(OccasionContactList entity)
        {
            string result = "";

            CardContactRepository cardContactRepository = new CardContactRepository(entity.Tenant);

            List<CardContact> cardContacts = cardContactRepository.GetCardContactForContact(entity.Id, entity.Tenant);

            if (cardContacts != null && cardContacts.Count > 0)
            {
                foreach (CardContact item in cardContacts)
                {
                    if (item.Card != null)
                    {
                        if (string.IsNullOrEmpty(result))
                        {
                            result = item.Card.EnglishName;
                        }

                        else
                        {
                            result = result + ", " + item.Card.EnglishName;
                        }
                    }
                }
            }

            if (result.Length > 500)
            {
                result = result.Substring(0, 500);
            }

            return result;
        }
    }
}