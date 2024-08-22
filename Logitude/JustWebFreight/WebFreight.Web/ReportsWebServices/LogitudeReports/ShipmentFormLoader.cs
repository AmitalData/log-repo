
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
using System.Linq.Dynamic.Core;
using Logitude.Customs.Def.EntityPMs;
using WebFreight.Web.Services;

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
            return new ReportMemoryStreamService().Convert(dataProvider, typeof(ShipmentFormDataProvider), tenant);
        }
        private void BuildDataProvider()
        {
            dataProvider = new ShipmentFormDataProvider();

            SetShipmentForm(tenant, reportQueryOperations);
        }

        public void SetShipmentForm(int tenant, QueryOperations queryOperations)
        {
            // get shipment id from filter
            QueryFilterItem ShipmentIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "Id").FirstOrDefault();
            if (ShipmentIdFilter == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"missing shipment id from filter, can not create form");
                throw new Exception("missing shipment id");
            }
            string shipmentId = ShipmentIdFilter.FieldValue.ToString();

            // get shipment by id from db
            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var shipments = (from a in context.Shipments
                                            .Include("UserId").Include("UserId.Contact") // for ReferentUserId
                                            .Include("CustomerCard")
                                            .Include("Department")
                                where a.Tenant == tenant
                                select new
                                {
                                    a.Id,
                                    a.ShipmentNumber,
                                    a.House,
                                    a.NumberOfPackages,
                                    a.GrossWeight,
                                    a.FreightForwarderId,
                                    a.IskaNumber,
                                    CustomerName = a.CustomerCard != null ? a.CustomerCard.LocalName: null,
                                    ReferantUserName = a.UserId != null && a.UserId.Contact != null? a.UserId.Contact.LocalName: null,
                                    DepartmentName = a.Department != null? a.Department.LocalName : null,
                                });

            var shipmentData = shipments.Where(x => x.Id == shipmentId).FirstOrDefault();
            if (shipmentData == null)
            {
                throw new Exception($"shipment not found with id: {shipmentId}");
            }

            #region map to data provider

            var shipmentFormProvider = new ShipmentForm()
            {
                ShipmentNumber = shipmentData.ShipmentNumber,
                ShipmentNumberTenant = shipmentData.ShipmentNumber + " " + tenant,
                ReferentUserName = shipmentData.ReferantUserName,
                DepartmentName = shipmentData.DepartmentName,
                IskaNumber = shipmentData.IskaNumber,
                CustomerName = shipmentData.CustomerName,
                House = shipmentData.House,
                NumberOfPackages = shipmentData.NumberOfPackages,
                GrossWeight = shipmentData.GrossWeight,
                FreightForwarderId = shipmentData.FreightForwarderId,
            };

            // get declaration and referant data by shipment number
            if (!string.IsNullOrEmpty(shipmentData.ShipmentNumber))
            {
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(tenant);
                DeclarationPM declarationPM = declarationQueryService.GetSingleByCustomFileNo(shipmentData.ShipmentNumber, tenant);

                shipmentFormProvider.DescriptionOfGoods = declarationPM?.CargoDescription;
                shipmentFormProvider.DeclarationOfficeName = declarationPM?.DeclarationOfficeName;

                if (declarationPM != null)
                {
                    DeclarationReferantDataQueryService declarationReferantDataQueryService = new DeclarationReferantDataQueryService(tenant);
                    DeclarationReferantDataPM declarationReferantDataPM = declarationReferantDataQueryService.GetSingle(declarationPM.Id, false, true);

                    shipmentFormProvider.CarrierCode = declarationReferantDataPM?.CarrierCode;
                    shipmentFormProvider.Mawb = declarationReferantDataPM?.Mawb;
                    shipmentFormProvider.EstimatedArrivalDate = declarationReferantDataPM?.EstimatedArrivalDate;
                    shipmentFormProvider.Vessel = declarationReferantDataPM?.Vessel;
                }
                else
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteWarning($"failed to retrieved declaration by shipment number: {shipmentData.ShipmentNumber}");
                }
            }
            else
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteWarning($"missing ShipmentNumber from shipment: {shipmentId}");
            }

            dataProvider.ShipmentForm = shipmentFormProvider;

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
