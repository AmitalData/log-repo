
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.Repsitories;
using System.Linq.Expressions;
using CHAMP17;
using NPOI.SS.Formula.Functions;
using System.Data.Entity.Infrastructure;
using System.Runtime.Remoting.Contexts;
using Logitude.Customs.Data;
using System.Data.Entity;
using Simplog.Global.Data.GlobalModel;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Customs
{
    public class CertificateOfOriginCountLoader
    {
        private int tenant;
        private CertificateOfOriginCountDataProvider dataProvider;
        private QueryOperations reportQueryOperations;

        public CertificateOfOriginCountLoader(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);
        }

        public byte[] GetData()
        {          
            BuildDataProvider();
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CertificateOfOriginCountDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, dataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private void BuildDataProvider()
        {
            dataProvider = new CertificateOfOriginCountDataProvider();

            SetCertificateOfOriginCount(tenant, reportQueryOperations);
        }

        public void SetCertificateOfOriginCount(int tenant, QueryOperations queryOperations)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            IGlobalContext globalContext = GlobalContext.GetContext();

            var certificateOfOriginsCounts = (from a in context.CertificateOfOrigins

                                              join de in context.Declarations
                                              .Select(x => new { x.Id, x.CreateDateTime, x.TransportModeId })
                                              on a.DeclarationId equals de.Id into deJoin
                                              from der in deJoin.DefaultIfEmpty().Take(1)

                                              select new
                                              {
                                                  a.Id,
                                                  a.Tenant,
                                                  a.COONumber,
                                                  der.CreateDateTime,
                                                  der.TransportModeId,
                                              });

            #region  ApplyCustomFilters

            QueryFilterItem CreateDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDate").FirstOrDefault();
            if (CreateDateFilter != null)
            {
                DateTime startDate = ((DateTime)CreateDateFilter.FieldValue).Date;
                DateTime endDate = ((DateTime)CreateDateFilter.FieldValue2).Date.AddDays(1);
                certificateOfOriginsCounts = certificateOfOriginsCounts.Where(x => x.CreateDateTime >= startDate && x.CreateDateTime < endDate);

            }

            QueryFilterItem TransportModeIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportModeId").FirstOrDefault();
            if (TransportModeIdFilter != null)
            {
                certificateOfOriginsCounts = certificateOfOriginsCounts.Where(x => x.TransportModeId == TransportModeIdFilter.FieldValue.ToString());
            }

            QueryFilterItem TenantFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Tenant").FirstOrDefault();
            if (TenantFilter != null)
            {
                certificateOfOriginsCounts = certificateOfOriginsCounts.Where(x => x.Tenant.ToString() == TenantFilter.FieldValue.ToString());
            }

            #endregion

            #region map to data provider

            var certificateOfOriginCount = certificateOfOriginsCounts
                .Where(x => !string.IsNullOrEmpty(x.COONumber))
                .GroupBy(d => new { d.Tenant })
                .Select(g => new CertificateOfOriginCount()
                {
                    Tenant = g.Key.Tenant,
                    // TenantName = tenants.ToString(), //.ContainsKey(g.Key.Tenant) ? tenants[g.Key.Tenant]: null,
                    Count = g.Count(),
                }).ToList();

            // add the tenant name
            var tenants = (from t in globalContext.GlobalTenants
                           select new
                           {
                               t.Id,
                               t.CompanyName,
                           })
                          .ToList().ToDictionary(x => x.Id, x => x.CompanyName);
            certificateOfOriginCount.ForEach(x => x.TenantName = tenants.GetValueOrNull(x.Tenant));

            dataProvider.CertificateOfOriginCount = certificateOfOriginCount;
            #endregion
        }

        private QueryOperations DeserializeQueryOperationFromXml(byte[] xmlFilters)
        {
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            return queryOperations;
        }
    }
}
