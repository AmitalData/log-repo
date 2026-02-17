using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateContactDoneMethodList(ContactDoneMethodList currentEntity)
        {
        }

        public IQueryable<ContactDoneMethod> GetContactDoneMethods(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            return ContactDoneMethodRepository.GetContactDoneMethods();
        }

        public IQueryable<ContactDoneMethod> GetContactDoneMethodsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            return ContactDoneMethodRepository.GetContactDoneMethods();
        }

        public ContactDoneMethodPM GetSingleContactDoneMethod(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodQuery = new ContactDoneMethodQuery(tenant);
            return ContactDoneMethodQuery.GetSingleContactDoneMethodPM(code);
        }

        public ContactDoneMethodList GetSingleContactDoneMethodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodList entityList = null;
            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            ContactDoneMethodQuery = new ContactDoneMethodQuery(ContactDoneMethodRepository);
            ContactDoneMethod entity = ContactDoneMethodRepository.GetSingleContactDoneMethod(code);

            if (entity != null)
            {
                List<ContactDoneMethod> singleEntityList = new List<ContactDoneMethod>();
                singleEntityList.Add(entity);

                IQueryable<ContactDoneMethod> iQueryable = singleEntityList.AsQueryable();
                IQueryable<ContactDoneMethodList> iQueryableEntityList = ContactDoneMethodQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ContactDoneMethodList> GetContactDoneMethodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            ContactDoneMethodQuery = new ContactDoneMethodQuery(ContactDoneMethodRepository);

            IQueryable<ContactDoneMethod> iQueryable = ContactDoneMethodRepository.GetContactDoneMethods();
            IQueryable<ContactDoneMethodList> query2 = ContactDoneMethodQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ContactDoneMethodList> GetContactDoneMethodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            ContactDoneMethodQuery = new ContactDoneMethodQuery(ContactDoneMethodRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ContactDoneMethod> iQueryable = ContactDoneMethodRepository.GetContactDoneMethods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ContactDoneMethod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ContactDoneMethodList> query2 = ContactDoneMethodQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ContactDoneMethodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ContactDoneMethodList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ContactDoneMethod", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ContactDoneMethodList, string>(queryOperations, query2);
                                break;
                            }

                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<ContactDoneMethodList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ContactDoneMethodList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ContactDoneMethodList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ContactDoneMethodList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ContactDoneMethodList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetContactDoneMethodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            ContactDoneMethodQuery = new ContactDoneMethodQuery(ContactDoneMethodRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ContactDoneMethod> iQueryable = ContactDoneMethodRepository.GetContactDoneMethods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ContactDoneMethod>(nonListQueryOperation, iQueryable);

            IQueryable<ContactDoneMethodList> query2 = ContactDoneMethodQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ContactDoneMethodList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<ContactDoneMethod> GetFirstContactDoneMethods(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            input = input.ToUpper();
            return ContactDoneMethodRepository.GetContactDoneMethods().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public IQueryable<ContactDoneMethod> GetContactDoneMethodsByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return ContactDoneMethodRepository.GetContactDoneMethods().Where(d => d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return ContactDoneMethodRepository.GetContactDoneMethods().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return ContactDoneMethodRepository.GetContactDoneMethods();
            }
        }

        public IQueryable<ContactDoneMethod> GetSingleContactDoneMethodByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactDoneMethodRepository = new ContactDoneMethodRepository(tenant);
            if (byCode)
            {
                return ContactDoneMethodRepository.GetContactDoneMethods().Where(d => d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return ContactDoneMethodRepository.GetContactDoneMethods().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertContactDoneMethod(ContactDoneMethod entity)
        {
            ContactDoneMethodRepository.Add(entity);
        }

        public void UpdateContactDoneMethod(ContactDoneMethod currentEntity)
        {
            ContactDoneMethodRepository.Update(currentEntity);
        }

        public void DeleteContactDoneMethod(ContactDoneMethod entity)
        {
            ContactDoneMethodRepository.Remove(entity);
        }
    }
}