
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
    public class CustomsCollateralLoader
    {
        private int tenant;
        private CustomsCollateralDataProvider dataProvider;
        private QueryOperations reportQueryOperations;

        public CustomsCollateralLoader(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);


        }


       


        public byte[] GetData()
        {           
            BuildDataProvider();
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomsCollateralDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream,dataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private void BuildDataProvider()
        {
            dataProvider = new CustomsCollateralDataProvider();


            SetCustomsCollateral(tenant, reportQueryOperations);

        }
        


        public void SetCustomsCollateral(int tenant, QueryOperations queryOperations)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var CustomsCollaterals = (from a in context.CustomsCollaterals
                                .Include(a => a.CollateralRequestStatus)
                                 join d in context.Declarations
                                 on a.DeclarationId equals d.Id into dJoin
                                 from declaration in dJoin.DefaultIfEmpty()
                                  join cc in context.CustomsCollateralsConditions
                                  on new { a.Id, Tenant = tenant } equals new {Id = cc.CustomsCollateralId, cc.Tenant } into ccJoin
                                 from cond in ccJoin.DefaultIfEmpty()
                                 join ca in context.CustomsCollateralsAnswers
                                .Include(a => a.CollateralAnswerType)
                                .Include(a => a.AnswerEntityType)
                                on a.Id equals ca.CustomsCollateralId into cJoin
                                from answer in cJoin.DefaultIfEmpty()
                                where a.Tenant == tenant
                                select new
                                {
                                    a.Id,
                                    a.CreateDateTime,
                                    a.CollateralRequestNumber,
                                    a.FileNo,
                                    a.EntityIdKey1,
                                    a.RequestValidityDate,
                                    a.CollateralValidityDate,
                                    CollateralRequestStatusName = a.CollateralRequestStatus != null ? a.CollateralRequestStatus.LocalName : null,
                                    a.CustomerId,
                                    //Declaration
                                    DeclarationId = declaration != null ? declaration.Id : null,
                                    Direction = declaration != null ? declaration.Direction : null,
                                    TransportModeId = declaration != null ? declaration.TransportModeId : null,
                                    //CustomsCollateralsAnswers
                                    CustomsCollateralId = answer != null ? answer.CustomsCollateralId : null,
                                    LineNumber = answer != null ? answer.LineNumber : 0,
                                    RequestFileTypeName = answer != null && answer.CollateralAnswerType != null ? answer.CollateralAnswerType.LocalName : null,
                                    RequestFileAmount = answer != null ? answer.RequestFileAmount : null,
                                    AllocatedAmount = answer != null ? answer.AllocatedAmount : null,
                                    AnswerEntityTypeName = answer != null && answer.AnswerEntityType != null ? answer.AnswerEntityType.LocalName : null,
                                    CustomsTapgFile = answer != null  ? answer.CustomsTapgFile : null,
                                    CustomsNumeral = answer != null  ? answer.CustomsNumeral : null,
                                    RequestedAmount = cond != null ? cond.RequestedAmount : null,

                                });

            #region  ApplyCustomFilters

            QueryFilterItem CreateDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDate").FirstOrDefault();
            if (CreateDateFilter != null)
            {
                DateTime startDate = ((DateTime)CreateDateFilter.FieldValue).Date;
                DateTime endDate = ((DateTime)CreateDateFilter.FieldValue2).Date.AddDays(1);
                CustomsCollaterals = CustomsCollaterals.Where(x => x.CreateDateTime >= startDate && x.CreateDateTime < endDate);

            }

            QueryFilterItem TransportModeIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportModeId").FirstOrDefault();
            if (TransportModeIdFilter != null)
            {
                CustomsCollaterals = CustomsCollaterals.Where(x => x.TransportModeId == TransportModeIdFilter.FieldValue.ToString());
            }

            
            QueryFilterItem CustomerFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Customer").FirstOrDefault();
            if (CustomerFilter != null)
            {
                CustomsCollaterals = CustomsCollaterals.Where(x => x.CustomerId == CustomerFilter.FieldValue.ToString());
            }

            QueryFilterItem DirectionFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ImportExport").FirstOrDefault();
            if (DirectionFilter != null)
            {
                CustomsCollaterals = CustomsCollaterals.Where(x => x.Direction == DirectionFilter.FieldValue.ToString());

            }

            #endregion

            #region map to data provider


            dataProvider.CustomsCollateral = CustomsCollaterals.GroupBy(c => new
            {
                c.Id,
                c.CollateralRequestNumber,
                c.FileNo,
                c.EntityIdKey1,
                c.RequestValidityDate,
                c.CollateralValidityDate,
                c.CollateralRequestStatusName,
               c.RequestedAmount
            })
            .Select(g => new CustomsCollateral()
            {
                CollateralRequestNumber = g.Key.CollateralRequestNumber,
                FileNo = g.Key.FileNo,
                EntityIdKey1 = g.Key.EntityIdKey1,
                RequestValidityDate = g.Key.RequestValidityDate,
                CollateralValidityDate = g.Key.CollateralValidityDate,
                CollateralRequestStatusName = g.Key.CollateralRequestStatusName,
                RequestedAmount = g.Key.RequestedAmount ,
                CustomsCollateralsAnswers = g.Select(d => new CustomsCollateralsAnswers()
                {
                    RequestFileTypeName = d.RequestFileTypeName,
                    RequestFileAmount = d.RequestFileAmount,
                    AllocatedAmount = d.AllocatedAmount,
                    AnswerEntityTypeName = d.AnswerEntityTypeName,
                    CustomsTapgFile = d.CustomsTapgFile,
                    CustomsNumeral = d.CustomsNumeral,
                    LineNumber = d.LineNumber,
                }).Where(x => x.LineNumber != 0)
                .ToList()
            }).ToList();
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

