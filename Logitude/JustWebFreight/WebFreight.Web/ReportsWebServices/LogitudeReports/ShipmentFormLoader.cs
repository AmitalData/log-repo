
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
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using System.Linq.Dynamic.Core;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Customs
{
    public class ShipmentFormLoader
    {
        private int tenant;
        private ShipmentFormDataProvider dataProvider;
        private QueryOperations reportQueryOperations;

        public ShipmentFormLoader(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);
        }

        public byte[] GetData()
        {
            BuildDataProvider();
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShipmentFormDataProvider));
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
            dataProvider = new ShipmentFormDataProvider();

            SetShipmentForm(tenant, reportQueryOperations);
        }

        public void SetShipmentForm(int tenant, QueryOperations queryOperations)
        {
            // ICustomContext context = CustomContext.GetContext(tenant);
            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            ICustomContext customContext = CustomContext.GetContext(tenant);

            var declarations = (from a in context.Shipments
                                            // .Include("Declarations")
                                            .Include("UserId").Include("UserId.Contact") // for ReferentUserId
                                            .Include("CustomerCard")
                                            .Include("Department")


                                join d in customContext.Declarations.Include(a => a.DeclarationOffice)
                                .Select(x => new {
                                    x.CustomFileNo,
                                    x.Id,
                                    x.CargoDescription,
                                    DeclarationOfficeName = x.DeclarationOffice.LocalName 
                                })
                                on a.ShipmentNumber equals d.CustomFileNo into dJoin
                                from declaration in dJoin.DefaultIfEmpty()

                                join drd in customContext.DeclarationReferantDatas
                                .Select(x => new {x.DeclarationId, x.CarrierCode, x.EstimatedArrivalDate, x.Vessel, x.Mawb })
                                on declaration.Id equals drd.DeclarationId into drdJoin
                                from declarationReferentData in drdJoin.DefaultIfEmpty()

                                where a.Tenant == tenant
                                select new
                                {
                                    a.ShipmentNumber,
                                    a.House,
                                    a.NumberOfPackages,
                                    a.GrossWeight,
                                    a.FreightForwarderId,
                                    a.IskaNumber,
                                    CustomerName = a.CustomerCard != null ? a.CustomerCard.LocalName: null,
                                    ReferantUserName = a.UserId != null && a.UserId.Contact != null? a.UserId.Contact.LocalName: null,
                                    DepartmentName = a.Department != null? a.Department.LocalName : null,

                                    DeclarationOfficeName = declaration != null? declaration.DeclarationOfficeName: null,
                                    DescriptionOfGoods = declaration != null ? declaration.CargoDescription: null,

                                    CarrierCode = declarationReferentData != null ? declarationReferentData.CarrierCode : null,
                                    EstimatedArrivalDate = declarationReferentData != null ? declarationReferentData.EstimatedArrivalDate : null,
                                    Vessel = declarationReferentData != null ? declarationReferentData.Vessel : null,
                                    Mawb = declarationReferentData != null ? declarationReferentData.Mawb : null,
                                });

            #region  ApplyCustomFilters

            /*
            QueryFilterItem CreateDateFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDate").FirstOrDefault();
            if (CreateDateFilter != null)
            {
                DateTime startDate = ((DateTime)CreateDateFilter.FieldValue).Date;
                DateTime endDate = ((DateTime)CreateDateFilter.FieldValue2).Date.AddDays(1);
                declarations = declarations.Where(x => x.CreateDateTime >= startDate && x.CreateDateTime < endDate);

            }

            QueryFilterItem TransportModeIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportModeId").FirstOrDefault();
            if (TransportModeIdFilter != null)
            {
                declarations = declarations.Where(x => x.TransportModeId == TransportModeIdFilter.FieldValue.ToString());
            }

            QueryFilterItem DeclarationStatusFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DeclarationStatusTypeCode").FirstOrDefault();
            if (DeclarationStatusFilter != null)
            {
                declarations = declarations.Where(x => x.DeclarationStatusTypeCode == DeclarationStatusFilter.FieldValue.ToString());
            }

            QueryFilterItem DeclarationTypeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DeclarationTypeCode").FirstOrDefault();
            if (DeclarationTypeFilter != null)
            {
                declarations = declarations.Where(x => x.DeclarationTypeCode == DeclarationTypeFilter.FieldValue.ToString());
            }

            QueryFilterItem ReferentUserIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ReferentUserId").FirstOrDefault();
            if (ReferentUserIdFilter != null)
            {
                declarations = declarations.Where(x => x.ReferentUserId == ReferentUserIdFilter.FieldValue.ToString());
            }

            QueryFilterItem DestinationCountryCodeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DestinationCountryCode").FirstOrDefault();
            if (DestinationCountryCodeFilter != null)
            {
                declarations = declarations.Where(x => x.DestinationCountryCode == DestinationCountryCodeFilter.FieldValue.ToString());
            }

            QueryFilterItem CustomerFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Customer").FirstOrDefault();
            if (CustomerFilter != null)
            {
                declarations = declarations.Where(x => x.CustomerId == CustomerFilter.FieldValue.ToString());
            }

            */
            #endregion

            #region map to data provider

            dataProvider.ShipmentForm = declarations.Select(g => new ShipmentForm()
            {
                ShipmentNumber = g.ShipmentNumber,
                ShipmentNumberTenant = g.ShipmentNumber + " " + tenant,
                ReferentUserName = g.ReferantUserName,
                DepartmentName = g.DepartmentName,
                DeclarationOfficeName = g.DeclarationOfficeName,
                IskaNumber = g.IskaNumber,
                CustomerName = g.CustomerName,
                CarrierCode = g.CarrierCode,
                Mawb = g.Mawb,
                House = g.House,
                NumberOfPackages = g.NumberOfPackages,
                GrossWeight = g.GrossWeight,
                EstimatedArrivalDate = g.EstimatedArrivalDate,
                Vessel = g.Vessel,
                FreightForwarderId = g.FreightForwarderId,
                DescriptionOfGoods = g.DescriptionOfGoods,

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
