	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Data.Entity.Infrastructure;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class ProceduralFaultListQueryService
    {


        public IQueryable<ProceduralFaultDeclarationView> GetProceduralFaultViews(int tenant)
        {
            
            ICustomContext customContext = CustomContext.GetContext(tenant);
            CustomContext activeContext = customContext.GetActiveDbContext() as CustomContext;
            object[] parameters = new object[] { };
             string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
             DbRawSqlQuery<ProceduralFaultDeclarationView> iQueryable = null;
             if (dbms == "oracle")
             {
                //iQueryable = activeContext.Database.SqlQuery<ProceduralFaultDeclarationView>("SELECT  ProceduralFaults.Id, ProceduralFaults.Tenant, ProceduralFaults.ProceduralFaultNumber, ProceduralFaults.ProceduralFaultStatusCode, ProceduralFaults.CreateDate, ProceduralFaults.InputTypeCode, ProceduralFaults.InspectionTypeCode, ProceduralFaults.ProceduralFaultCode,  ProceduralFaults.ProceduralFaultInputProcesCode, ProceduralFaults.RansomViolationTypeCode,  ProceduralFaults.RansomViolationSum,  ProceduralFaults.Remarks, ProceduralFaults.IsCustomerResponsibility,  ProceduralFaults.IsAgentProceduralFaultCountabl, ProceduralFaults.IsCustProceduralFaultCountabl,  ProceduralFaults.IsAgentResponsibility,  ProceduralFaults.UpdateDate, ProceduralFaults.LeadingDocumentVersion,ProceduralFaults.Notes,  ProceduralFaults.IsCancelled, ProceduralFaults.CancellationDate,ProceduralFaults.SearchFields,  ProceduralFaults.DeclarationId,  Declarations.CustomFileNo,Declarations.CustomerId,FaultInspectionTypes.LocalName AS FaultInspectionName, ProceduralFaultInSourceTypes.LocalName AS ProceduralFaultInSourceName,  ProceduralFaultTypes.LocalName AS ProceduralFaultTypeName, ProceduralFaultInProcessTypes.LocalName AS ProceduralFaultInProcesTypName, ProceduralFaultStatuses.LocalName AS ProceduralFaultStatuseName, dbo.Cards.LocalName AS CustomerName,  Declarations.DeclarationNumber, RansomViolationTypes.LocalName AS RansomViolationTypeName FROM               ProceduralFaults  LEFT OUTER JOIN FaultInspectionTypes ON  ProceduralFaults.InspectionTypeCode =  FaultInspectionTypes.Code AND  ProceduralFaults.InspectionTypeCode =  FaultInspectionTypes.Code LEFT OUTER JOIN ProceduralFaultInSourceTypes ON  ProceduralFaults.InputTypeCode =  ProceduralFaultInSourceTypes.Code AND ProceduralFaults.InputTypeCode =  ProceduralFaultInSourceTypes.Code LEFT OUTER JOIN ProceduralFaultInProcessTypes ON  ProceduralFaults.ProceduralFaultInputProcesCode =  ProceduralFaultInProcessTypes.Code AND ProceduralFaults.ProceduralFaultInputProcesCode =  ProceduralFaultInProcessTypes.Code LEFT OUTER JOIN ProceduralFaultTypes ON  ProceduralFaults.ProceduralFaultCode =  ProceduralFaultTypes.Code AND  ProceduralFaults.ProceduralFaultCode =  ProceduralFaultTypes.Code LEFT OUTER JOIN ProceduralFaultStatuses ON  ProceduralFaults.ProceduralFaultStatusCode =  ProceduralFaultStatuses.Code AND  ProceduralFaults.ProceduralFaultStatusCode =  ProceduralFaultStatuses.Code LEFT OUTER JOIN RansomViolationTypes ON  ProceduralFaults.RansomViolationTypeCode =  RansomViolationTypes.Code LEFT OUTER JOIN Declarations ON  ProceduralFaults.DeclarationId =  Declarations.Id LEFT OUTER JOIN Cards ON  Declarations.CustomerId = Cards.Id where ProceduralFaults.Tenant=" + tenant.ToString(), parameters);
                iQueryable = activeContext.Database.SqlQuery<ProceduralFaultDeclarationView>("SELECT Contacts.LocalName AS SignedByUserName, ProceduralFaults.SignedByUserId,  ProceduralFaults.Id, ProceduralFaults.Tenant, ProceduralFaults.ProceduralFaultNumber, ProceduralFaults.ProceduralFaultStatusCode, ProceduralFaults.CreateDate, ProceduralFaults.InputTypeCode, ProceduralFaults.InspectionTypeCode, ProceduralFaults.ProceduralFaultCode,  ProceduralFaults.ProceduralFaultInputProcesCode, ProceduralFaults.RansomViolationTypeCode,  ProceduralFaults.RansomViolationSum,  ProceduralFaults.Remarks, ProceduralFaults.IsCustomerResponsibility,  ProceduralFaults.IsAgentProceduralFaultCountabl, ProceduralFaults.IsCustProceduralFaultCountabl,  ProceduralFaults.IsAgentResponsibility,  ProceduralFaults.UpdateDate, ProceduralFaults.LeadingDocumentVersion,ProceduralFaults.Notes,  ProceduralFaults.IsCancelled, ProceduralFaults.CancellationDate,ProceduralFaults.SearchFields,  ProceduralFaults.DeclarationId,  Declarations.CustomFileNo,Declarations.CustomerId,FaultInspectionTypes.LocalName AS FaultInspectionName, ProceduralFaultInSourceTypes.LocalName AS ProceduralFaultInSourceName,  ProceduralFaultTypes.LocalName AS ProceduralFaultTypeName, ProceduralFaultInProcessTypes.LocalName AS ProceduralFaultInProcesTypName, ProceduralFaultStatuses.LocalName AS ProceduralFaultStatuseName,  Cards.LocalName AS CustomerName,  Declarations.DeclarationNumber, RansomViolationTypes.LocalName AS RansomViolationTypeName FROM               ProceduralFaults  LEFT OUTER JOIN FaultInspectionTypes ON  ProceduralFaults.InspectionTypeCode =  FaultInspectionTypes.Code AND  ProceduralFaults.InspectionTypeCode =  FaultInspectionTypes.Code LEFT OUTER JOIN ProceduralFaultInSourceTypes ON  ProceduralFaults.InputTypeCode =  ProceduralFaultInSourceTypes.Code AND ProceduralFaults.InputTypeCode =  ProceduralFaultInSourceTypes.Code LEFT OUTER JOIN ProceduralFaultInProcessTypes ON  ProceduralFaults.ProceduralFaultInputProcesCode =  ProceduralFaultInProcessTypes.Code AND ProceduralFaults.ProceduralFaultInputProcesCode =  ProceduralFaultInProcessTypes.Code LEFT OUTER JOIN ProceduralFaultTypes ON  ProceduralFaults.ProceduralFaultCode =  ProceduralFaultTypes.Code AND  ProceduralFaults.ProceduralFaultCode =  ProceduralFaultTypes.Code LEFT OUTER JOIN ProceduralFaultStatuses ON  ProceduralFaults.ProceduralFaultStatusCode =  ProceduralFaultStatuses.Code AND  ProceduralFaults.ProceduralFaultStatusCode =  ProceduralFaultStatuses.Code LEFT OUTER JOIN RansomViolationTypes ON  ProceduralFaults.RansomViolationTypeCode =  RansomViolationTypes.Code LEFT OUTER JOIN Declarations ON  ProceduralFaults.DeclarationId =  Declarations.Id LEFT OUTER JOIN Cards ON  Declarations.CustomerId = Cards.Id LEFT JOIN Users ON ProceduralFaults.SignedByUserId=Users.Id LEFT JOIN Contacts ON Users.Id=Contacts.Id where ProceduralFaults.Tenant=" + tenant.ToString(), parameters);

            }
            else
             {
                 iQueryable = activeContext.Database.SqlQuery<ProceduralFaultDeclarationView>("SELECT Customs.ProceduralFaults.Id, Customs.ProceduralFaults.Tenant, Customs.ProceduralFaults.ProceduralFaultNumber,  Customs.ProceduralFaults.ProceduralFaultStatusCode, Customs.ProceduralFaults.CreateDate, Customs.ProceduralFaults.InputTypeCode,    Customs.ProceduralFaults.InspectionTypeCode, Customs.ProceduralFaults.ProceduralFaultCode, Customs.ProceduralFaults.ProceduralFaultInputProcesCode,    Customs.ProceduralFaults.RansomViolationTypeCode, Customs.ProceduralFaults.RansomViolationSum, Customs.ProceduralFaults.Remarks,     Customs.ProceduralFaults.IsCustomerResponsibility, Customs.ProceduralFaults.IsAgentProceduralFaultCountabl,    Customs.ProceduralFaults.IsCustProceduralFaultCountabl, Customs.ProceduralFaults.IsAgentResponsibility, Customs.ProceduralFaults.UpdateDate,       Customs.ProceduralFaults.LeadingDocumentVersion, Customs.ProceduralFaults.Notes, Customs.ProceduralFaults.IsCancelled,   Customs.ProceduralFaults.CancellationDate, Customs.ProceduralFaults.SearchFields, Customs.ProceduralFaults.DeclarationId, Customs.Declarations.CustomFileNo,   Customs.Declarations.CustomerId, Customs.FaultInspectionTypes.LocalName AS FaultInspectionName,    Customs.ProceduralFaultInSourceTypes.LocalName AS ProceduralFaultInputSourceName, Customs.ProceduralFaultTypes.LocalName AS ProceduralFaultTypeName,     Customs.ProceduralFaultInProcessTypes.LocalName AS ProceduralFaultInProcesTypName,   Customs.ProceduralFaultStatuses.LocalName AS ProceduralFaultStatuseName, dbo.Cards.LocalName AS CustomerName, Customs.Declarations.DeclarationNumber,       Customs.RansomViolationTypes.LocalName AS RansomViolationTypeName FROM            Customs.ProceduralFaults LEFT OUTER JOIN     Customs.FaultInspectionTypes ON Customs.ProceduralFaults.InspectionTypeCode = Customs.FaultInspectionTypes.Code AND      Customs.ProceduralFaults.InspectionTypeCode = Customs.FaultInspectionTypes.Code LEFT OUTER JOIN      Customs.ProceduralFaultInSourceTypes ON Customs.ProceduralFaults.InputTypeCode = Customs.ProceduralFaultInSourceTypes.Code AND     Customs.ProceduralFaults.InputTypeCode = Customs.ProceduralFaultInSourceTypes.Code LEFT OUTER JOIN     Customs.ProceduralFaultInProcessTypes ON Customs.ProceduralFaults.ProceduralFaultInputProcesCode = Customs.ProceduralFaultInProcessTypes.Code AND      Customs.ProceduralFaults.ProceduralFaultInputProcesCode = Customs.ProceduralFaultInProcessTypes.Code LEFT OUTER JOIN     Customs.ProceduralFaultTypes ON Customs.ProceduralFaults.ProceduralFaultCode = Customs.ProceduralFaultTypes.Code AND     Customs.ProceduralFaults.ProceduralFaultCode = Customs.ProceduralFaultTypes.Code LEFT OUTER JOIN      Customs.ProceduralFaultStatuses ON Customs.ProceduralFaults.ProceduralFaultStatusCode = Customs.ProceduralFaultStatuses.Code AND       Customs.ProceduralFaults.ProceduralFaultStatusCode = Customs.ProceduralFaultStatuses.Code LEFT OUTER JOIN     Customs.RansomViolationTypes ON Customs.ProceduralFaults.RansomViolationTypeCode = Customs.RansomViolationTypes.Code LEFT OUTER JOIN     Customs.Declarations ON Customs.ProceduralFaults.DeclarationId = Customs.Declarations.Id LEFT OUTER JOIN    dbo.Cards ON Customs.Declarations.CustomerId = dbo.Cards.Id where Customs.ProceduralFaults.Tenant=" + tenant.ToString(), parameters);
             }

             IQueryable<ProceduralFaultDeclarationView> castedIqueryable = from a in iQueryable.AsQueryable()
                                                                           select new ProceduralFaultDeclarationView()
                                                                           {
                                                                               Id = a.Id,
                                                                               CancellationDate = a.CancellationDate,
                                                                               CreateDate = a.CreateDate,
                                                                               DeclarationId = a.DeclarationId,
                                                                               InputTypeCode = a.InputTypeCode,
                                                                               InspectionTypeCode = a.InspectionTypeCode,
                                                                               IsAgentProceduralFaultCountabl = a.IsAgentProceduralFaultCountabl,
                                                                               IsAgentResponsibility = a.IsAgentResponsibility,
                                                                               IsCancelled = a.IsCancelled,
                                                                               IsCustProceduralFaultCountabl = a.IsCustProceduralFaultCountabl,
                                                                               IsCustomerResponsibility = a.IsCustomerResponsibility,
                                                                               LeadingDocumentVersion = a.LeadingDocumentVersion,
                                                                               Notes = a.Notes,
                                                                               ProceduralFaultCode = a.ProceduralFaultCode,
                                                                               ProceduralFaultInputProcesCode = a.ProceduralFaultInputProcesCode,
                                                                               ProceduralFaultNumber = a.ProceduralFaultNumber,
                                                                               ProceduralFaultStatusCode = a.ProceduralFaultStatusCode,
                                                                               RansomViolationSum = a.RansomViolationSum,
                                                                               RansomViolationTypeCode = a.RansomViolationTypeCode,
                                                                               Remarks = a.Remarks,
                                                                               Tenant = a.Tenant,
                                                                               SearchFields = a.SearchFields,
                                                                               UpdateDate = a.UpdateDate,
                                                                               DeclarationNumber = a.DeclarationNumber,
                                                                               CustomFileNo = a.CustomFileNo,
                                                                               CustomerName = a.CustomerName,
                                                                               ProceduralFaultInProcesTypName = a.ProceduralFaultInProcesTypName,
                                                                               ProceduralFaultInputSourceName = a.ProceduralFaultInputSourceName,
                                                                               FaultInspectionName = a.FaultInspectionName,
                                                                               ProceduralFaultTypeName = a.ProceduralFaultTypeName,
                                                                               ProceduralFaultStatuseName = a.ProceduralFaultStatuseName,
                                                                               RansomViolationTypeName = a.RansomViolationTypeName,
                                                                               SignedByUserId = a.SignedByUserId,
                                                                               SignedByUserName = a.SignedByUserName,

                                                                           };



            return castedIqueryable;
            
        }

        public List<ProceduralFaultList> GetListFromView(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IProceduralFaultsDeclarationsViewContext viewContext = ProceduralFaultsDeclarationsViewContext.GetContext(tenant);

            IQueryable<ProceduralFaultDeclarationView> iQueryable = GetProceduralFaultViews(tenant); /*(from a in viewContext.ProceduralFaultDeclarationViews

                                                      where a.Tenant == tenant
                                                      select a);*/

            iQueryable = ApplyCustomFiltersForView(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ProceduralFaultDeclarationView>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ProceduralFaultList> query2 = GetIqueryableListForView(iQueryable);

            query2 = filter.GetFilteredQuery<ProceduralFaultList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ProceduralFaultList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ProceduralFaultObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ProceduralFault", tenant).ToList();

                ObjectField objectField = (from a in ProceduralFaultObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ProceduralFaultList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<ProceduralFaultList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<ProceduralFaultList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<ProceduralFaultList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<ProceduralFaultList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<ProceduralFaultList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<ProceduralFaultList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.Id);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }
            return query2.ToList();


        }

        public int GetListCountFromView(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IProceduralFaultsDeclarationsViewContext viewContext = ProceduralFaultsDeclarationsViewContext.GetContext(tenant);
            IQueryable<ProceduralFaultDeclarationView> iQueryable = GetProceduralFaultViews(tenant); /*(from a in viewContext.ProceduralFaultDeclarationViews
                                                      where a.Tenant == tenant
                                                      select a);*/
            
            iQueryable = ApplyCustomFiltersForView(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ProceduralFaultDeclarationView>(nonListQueryOperation, iQueryable);

            IQueryable<ProceduralFaultList> query2 = GetIqueryableListForView(iQueryable);

            query2 = filter.GetFilteredQuery<ProceduralFaultList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        private IQueryable<ProceduralFaultList> GetIqueryableList(IQueryable<ProceduralFault> iQueryable)
        {
            IQueryable<ProceduralFaultList> query = (from a in iQueryable.Include("ProceduralFaultStatus").Include("ProceduralFaultInProcessType").Include("ProceduralFaultType").Include("RansomViolationType")
                                                     join d in context.Declarations.Include("CustomerCard")
                                                     on a.DeclarationId equals d.Id
                                                     select new ProceduralFaultList()
                                                            {
                                                        Id = a.Id,
                                                        CancellationDate = a.CancellationDate,
                                                        CreateDate = a.CreateDate,
                                                        DeclarationId = a.DeclarationId,
                                                        InputTypeCode = a.InputTypeCode,
                                                        InspectionTypeCode = a.InspectionTypeCode,
                                                        IsAgentProceduralFaultCountabl = a.IsAgentProceduralFaultCountabl,
                                                        IsAgentResponsibility = a.IsAgentResponsibility,
                                                        IsCancelled = a.IsCancelled,
                                                        IsCustProceduralFaultCountabl = a.IsCustProceduralFaultCountabl,
                                                        IsCustomerResponsibility = a.IsCustomerResponsibility,
                                                        LeadingDocumentVersion = a.LeadingDocumentVersion,
                                                        Notes = a.Notes,
                                                        ProceduralFaultCode = a.ProceduralFaultCode,
                                                        ProceduralFaultName = a.ProceduralFaultType != null ? a.ProceduralFaultType.LocalName : null,
                                                        ProceduralFaultInputProcesCode = a.ProceduralFaultInputProcesCode,
                                                        ProceduralFaultInputProcesName = a.ProceduralFaultInProcessType != null ? a.ProceduralFaultStatus.LocalName : null,
                                                        ProceduralFaultNumber = a.ProceduralFaultNumber,
                                                        ProceduralFaultStatusCode = a.ProceduralFaultStatusCode,
                                                        ProceduralFaultStatusName = a.ProceduralFaultStatus != null ? a.ProceduralFaultStatus.LocalName : null,
                                                        RansomViolationSum = a.RansomViolationSum,
                                                        RansomViolationTypeCode = a.RansomViolationTypeCode,
                                                        RansomViolationTypeName = a.RansomViolationType != null ? a.RansomViolationType.LocalName : null,
                                                        Remarks = a.Remarks,
                                                        Tenant = a.Tenant,
                                                        SearchFields = a.SearchFields,
                                                        UpdateDate = a.UpdateDate,
                                                        CustomFileNo = d.CustomFileNo,
                                                        DeclarationNumber = d.DeclarationNumber,
                                                        CustomerName = d.CustomerCard.LocalName != null ? d.CustomerCard.LocalName : d.CustomerCard.EnglishName,
                                                        SignedByUserId = a.SignedByUserId,
                                                         SignedByUserName = a.User != null && a.User.Contact != null ? a.User.Contact.LocalName : null,
                                                     });
            return query;
		}

        private IQueryable<ProceduralFaultList> GetIqueryableListForView(IQueryable<ProceduralFaultDeclarationView> iQueryable)
        {
            IQueryable<ProceduralFaultList> query = (from a in iQueryable
                                                     where a.DeclarationId != null
                                                     select new ProceduralFaultList()
                                                     {
                                                         Id = a.Id,
                                                         CancellationDate = a.CancellationDate,
                                                         CreateDate = a.CreateDate,
                                                         DeclarationId = a.DeclarationId,
                                                         InputTypeCode = a.InputTypeCode,
                                                         InspectionTypeCode = a.InspectionTypeCode,
                                                         IsAgentProceduralFaultCountabl = a.IsAgentProceduralFaultCountabl,
                                                         IsAgentResponsibility = a.IsAgentResponsibility,
                                                         IsCancelled = a.IsCancelled,
                                                         IsCustProceduralFaultCountabl = a.IsCustProceduralFaultCountabl,
                                                         IsCustomerResponsibility = a.IsCustomerResponsibility,
                                                         LeadingDocumentVersion = a.LeadingDocumentVersion,
                                                         Notes = a.Notes,
                                                         ProceduralFaultCode = a.ProceduralFaultCode,
                                                         ProceduralFaultInputProcesCode = a.ProceduralFaultInputProcesCode,
                                                         ProceduralFaultNumber = a.ProceduralFaultNumber,
                                                         ProceduralFaultStatusCode = a.ProceduralFaultStatusCode,
                                                         RansomViolationSum = a.RansomViolationSum,
                                                         RansomViolationTypeCode = a.RansomViolationTypeCode,
                                                         Remarks = a.Remarks,
                                                         Tenant = a.Tenant,
                                                         SearchFields = a.SearchFields,
                                                         UpdateDate = a.UpdateDate,
                                                         DeclarationNumber = a.DeclarationNumber,
                                                         CustomFileNo = a.CustomFileNo,
                                                         CustomerName = a.CustomerName,
                                                         ProceduralFaultInputProcesName = a.ProceduralFaultInProcesTypName,
                                                         InputTypeName = a.ProceduralFaultInputSourceName,
                                                         InspectionTypeName = a.FaultInspectionName,
                                                         ProceduralFaultName = a.ProceduralFaultTypeName,
                                                         ProceduralFaultStatusName = a.ProceduralFaultStatuseName,
                                                         RansomViolationTypeName = a.RansomViolationTypeName,
                                                         SignedByUserId = a.SignedByUserId,
                                                         SignedByUserName = a.SignedByUserName,
                                                     });
            return query;
        }

        private IQueryable<ProceduralFault> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ProceduralFault> iQueryable, int tenant)
        {
            return iQueryable;
		}

        private IQueryable<ProceduralFaultDeclarationView> ApplyCustomFiltersForView(QueryOperations queryOperations, IQueryable<ProceduralFaultDeclarationView> iQueryable)
        {
            return iQueryable;
        }
	}


}
	