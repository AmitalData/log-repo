using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
	public partial class InvoiceDomainService
	{
		public APInvoiceStatusPM GetSingleAPInvoiceStatus(string code, int tenant)
		{
			SecurityUtility.AuthenticationOnTenant(tenant);

            apInvoiceStatusQuery = new APInvoiceStatusQuery(tenant);
			return apInvoiceStatusQuery.GetSingleAPInvoiceStatusPM(code);
		}

		public void UpdateAPInvoiceStatusList(APInvoiceStatusList list)
		{

		}

		public APInvoiceStatusList GetSingleAPInvoiceStatusList(string code, int tenant)
		{
			SecurityUtility.AuthenticationOnTenant(tenant);

			APInvoiceStatusList entityList = null;
			aPInvoiceStatusRepository = new APInvoiceStatusRepository(tenant);
            apInvoiceStatusQuery = new APInvoiceStatusQuery(aPInvoiceStatusRepository);

			APInvoiceStatus entity = aPInvoiceStatusRepository.GetSingleAPInvoiceStatus(code);

			if (entity != null)
			{
				List<APInvoiceStatus> SingleEntityList = new List<APInvoiceStatus>();
				SingleEntityList.Add(entity);

				IQueryable<APInvoiceStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<APInvoiceStatusList> iQueryableEntityList = apInvoiceStatusQuery.GetIQueryableEntityList(iQueryable);
				entityList = iQueryableEntityList.FirstOrDefault();
			}

			return entityList;
		}

		public IQueryable<APInvoiceStatusList> GetAPInvoiceStatusLists(int tenant)
		{
			SecurityUtility.AuthenticationOnTenant(tenant);

			aPInvoiceStatusRepository = new APInvoiceStatusRepository(tenant);
            apInvoiceStatusQuery = new APInvoiceStatusQuery(aPInvoiceStatusRepository);
			IQueryable<APInvoiceStatus> iQueryable = aPInvoiceStatusRepository.GetAPInvoiceStatus();
            IQueryable<APInvoiceStatusList> query2 = apInvoiceStatusQuery.GetIQueryableEntityList(iQueryable);
			return query2;

		}

		[Query(HasSideEffects = true)]
		public IQueryable<APInvoiceStatusList> GetAPInvoiceStatusFilters(byte[] XmlFilters, int tenant)
		{
			SecurityUtility.AuthenticationOnTenant(tenant);

			aPInvoiceStatusRepository = new APInvoiceStatusRepository(tenant);
            apInvoiceStatusQuery = new APInvoiceStatusQuery(aPInvoiceStatusRepository);
			MemoryStream memorystream = new MemoryStream(XmlFilters);
			XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
			QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
			GenericFilter filter = new GenericFilter();
			GenericSort sortClass = new GenericSort();

			IQueryable<APInvoiceStatus> iQueryable = aPInvoiceStatusRepository.GetAPInvoiceStatus();

			QueryOperations nonListQueryOperation = new QueryOperations();
			nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
			QueryOperations listQueryOperation = new QueryOperations();
			listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

			iQueryable = filter.GetFilteredQuery<APInvoiceStatus>(nonListQueryOperation, iQueryable);

			int skippedPorts = queryOperations.PageIndex;

			IQueryable<APInvoiceStatusList> query2 = apInvoiceStatusQuery.GetIQueryableEntityList(iQueryable);

			query2 = filter.GetFilteredQuery<APInvoiceStatusList>(listQueryOperation, query2);

			if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
			{
				PropertyInfo propInfo = typeof(APInvoiceStatusList).GetProperty(queryOperations.SortByColumnName);
				switch (propInfo.PropertyType.Name.ToLower())
				{
					case "string":
						{
							query2 = sortClass.GetSorterQuery<APInvoiceStatusList, string>(queryOperations, query2);
							break;
						}
					case "double":
						{
							query2 = sortClass.GetSorterQuery<APInvoiceStatusList, double>(queryOperations, query2);
							break;
						}
					case "datetime":
						{
							query2 = sortClass.GetSorterQuery<APInvoiceStatusList, DateTime>(queryOperations, query2);
							break;
						}
					case "int":
						{
							query2 = sortClass.GetSorterQuery<APInvoiceStatusList, int>(queryOperations, query2);
							break;
						}
					case "boolean":
						{
							query2 = sortClass.GetSorterQuery<APInvoiceStatusList, bool>(queryOperations, query2);
							break;
						}
					default:
						{
							query2 = query2.OrderByDescending(d => d.Code);
							break;
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

		public int GetAPInvoiceStatusCount(byte[] XmlFilters, int tenant)
		{
			SecurityUtility.AuthenticationOnTenant(tenant);

			aPInvoiceStatusRepository = new APInvoiceStatusRepository(tenant);
            apInvoiceStatusQuery = new APInvoiceStatusQuery(aPInvoiceStatusRepository);
			MemoryStream memorystream = new MemoryStream(XmlFilters);
			XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
			QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
			GenericFilter filter = new GenericFilter();
			GenericSort sortClass = new GenericSort();

			IQueryable<APInvoiceStatus> iQueryable = aPInvoiceStatusRepository.GetAPInvoiceStatus();

			QueryOperations nonListQueryOperation = new QueryOperations();
			nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
			QueryOperations listQueryOperation = new QueryOperations();
			listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

			iQueryable = filter.GetFilteredQuery<APInvoiceStatus>(nonListQueryOperation, iQueryable);

			IQueryable<APInvoiceStatusList> query2 = apInvoiceStatusQuery.GetIQueryableEntityList(iQueryable);

			query2 = filter.GetFilteredQuery<APInvoiceStatusList>(listQueryOperation, query2);

			int count = query2.Count();
			return count;
		}
	}
}