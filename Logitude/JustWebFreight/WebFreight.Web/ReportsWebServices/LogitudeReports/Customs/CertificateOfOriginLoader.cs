
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

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Customs
{
    public class CertificateOfOriginLoader
    {
        private int tenant;
        private CertificateOfOriginDataProvider dataProvider;
        private QueryOperations reportQueryOperations;

        public CertificateOfOriginLoader(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);
        }

        public byte[] GetData()
        {
            CertificateOfOriginDataProvider myDataProvider = new CertificateOfOriginDataProvider();
           
            BuildDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CertificateOfOriginDataProvider));
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
            dataProvider = new CertificateOfOriginDataProvider();


            SetCertificateOfOrigin(tenant, reportQueryOperations);
        }

        public void SetCertificateOfOrigin(int tenant, QueryOperations queryOperations)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var certificateOfOrigins = (from a in context.CertificateOfOrigins
                                                   .Include(a => a.CertificateOfOriginTypeCodeEnum)
                                                   .Include(a => a.CertificateOfOriginStatusCodeEnum)

                                join de in context.Declarations
                                .Select(x => new { x.Id, x.CustomFileNo, x.CreateDateTime, x.DeclarationNumber, x.CustomerId, x.TransportModeId })
                                on a.DeclarationId equals de.Id into deJoin
                                from der in deJoin.DefaultIfEmpty().Take(1)

                                where a.Tenant == tenant
                                select new
                                {
                                    a.COONumber,
                                    a.CooTypeCode,
                                    a.CooStatusCode,
                                    der.CreateDateTime,
                                    der.CustomFileNo,
                                    der.DeclarationNumber,
                                    der.CustomerId,
                                    der.TransportModeId,
                                    CooTypeCodeName = a.CertificateOfOriginTypeCodeEnum != null ? a.CertificateOfOriginTypeCodeEnum.LocalName : null,
                                    CooStatusCodeName = a.CertificateOfOriginStatusCodeEnum != null ? a.CertificateOfOriginStatusCodeEnum.LocalName : null,
                                });

            #region  ApplyCustomFilters

            QueryFilterItem CreateDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDate").FirstOrDefault();
            if (CreateDateFilter != null)
            {
                DateTime startDate = ((DateTime)CreateDateFilter.FieldValue).Date;
                DateTime endDate = ((DateTime)CreateDateFilter.FieldValue2).Date.AddDays(1);
                certificateOfOrigins = certificateOfOrigins.Where(x => x.CreateDateTime >= startDate && x.CreateDateTime < endDate);

            }

            QueryFilterItem CooTypeCodeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CooTypeCode").FirstOrDefault();
            if (CooTypeCodeFilter != null)
            {
                certificateOfOrigins = certificateOfOrigins.Where(x => x.CooTypeCode == CooTypeCodeFilter.FieldValue.ToString());
            }

            QueryFilterItem COONumberFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "COONumber").FirstOrDefault();
            if (COONumberFilter != null)
            {
                certificateOfOrigins = certificateOfOrigins.Where(x => x.COONumber == COONumberFilter.FieldValue.ToString());
            }

            QueryFilterItem DeclarationNumberFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DeclarationNumber").FirstOrDefault();
            if (DeclarationNumberFilter != null)
            {
                certificateOfOrigins = certificateOfOrigins.Where(x => x.DeclarationNumber == DeclarationNumberFilter.FieldValue.ToString());
            }

            QueryFilterItem CustomFileNoFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomFileNo").FirstOrDefault();
            if (CustomFileNoFilter != null)
            {
                certificateOfOrigins = certificateOfOrigins.Where(x => x.CustomFileNo == CustomFileNoFilter.FieldValue.ToString());
            }

            QueryFilterItem CustomerFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Customer").FirstOrDefault();
            if (CustomerFilter != null)
            {
                certificateOfOrigins = certificateOfOrigins.Where(x => x.CustomerId == CustomerFilter.FieldValue.ToString());
            }

            QueryFilterItem TransportModeIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportModeId").FirstOrDefault();
            if (TransportModeIdFilter != null)
            {
                certificateOfOrigins = certificateOfOrigins.Where(x => x.TransportModeId == TransportModeIdFilter.FieldValue.ToString());
            }


            #endregion

            #region map to data provider

            var certificateOfOrigin = certificateOfOrigins.Select(g => new CertificateOfOrigin()
            {
                CustomFileNo = g.CustomFileNo,
                COONumber = g.COONumber,
                CooStatusCodeName = g.CooStatusCodeName,
                CooTypeCodeName = g.CooTypeCodeName,
            }).ToList();

            dataProvider.CertificateOfOrigin = certificateOfOrigin;
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
