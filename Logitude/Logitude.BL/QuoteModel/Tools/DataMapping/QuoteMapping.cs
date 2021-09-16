using System;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public partial class QuoteMapping
    {
        public static void MapEntity(QuotePM entityPM, Quote entityPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPoco.QuoteNumber = entityPM.QuoteNumber;
                entityPoco.CreatedByUserId = entityPM.CreatedByUserId;
                entityPoco.OpenDate = entityPM.OpenDate;
                entityPoco.Tenant = entityPM.Tenant;
                entityPoco.DirectionId = entityPM.DirectionId;
                entityPoco.ProductCode = entityPM.ProductCode;
            }

            entityPoco.TransportModeId = entityPM.TransportModeId;
            entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;
            entityPoco.TotalPerContainer = entityPM.TotalPerContainer;  
            entityPoco.UpdateDate = entityPM.UpdateDate;
            entityPoco.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPoco.LastVersionNumber = entityPM.LastVersionNumber;
            entityPoco.QuoteTemplateId = entityPM.QuoteTemplateId;
            entityPoco.QuoteCustomerTypeCode = entityPM.QuoteCustomerTypeCode;
            entityPoco.CustomerId = entityPM.CustomerId;
            entityPoco.CustomerContactId = entityPM.CustomerContactId;
            entityPoco.CustomerReference1 = entityPM.CustomerReference1;
            entityPoco.CustomerReference2 = entityPM.CustomerReference2;
            entityPoco.ShipperId = entityPM.ShipperId;
            entityPoco.ShipperName = entityPM.ShipperName;
            entityPoco.ShipperContactId = entityPM.ShipperContactId;
            entityPoco.ShipperReference1 = entityPM.ShipperReference1;
            entityPoco.ShipperReference2 = entityPM.ShipperReference2;
            entityPoco.ConsigneeId = entityPM.ConsigneeId;
            entityPoco.ConsigneeName = entityPM.ConsigneeName;
            entityPoco.ConsigneeContactId = entityPM.ConsigneeContactId;
            entityPoco.ConsigneeReference1 = entityPM.ConsigneeReference1;
            entityPoco.ConsigneeReference2 = entityPM.ConsigneeReference2;
            entityPoco.IncotermId = entityPM.IncotermId;
            entityPoco.SalesmanUserId = entityPM.SalesmanUserId;
            entityPoco.IsClosed = entityPM.IsClosed;
            entityPoco.IsCancelled = entityPM.IsCancelled;
            entityPoco.FromPortId = entityPM.FromPortId;
            entityPoco.ToPortId = entityPM.ToPortId;
            entityPoco.IsFixedPrice = entityPM.IsFixedPrice;
            entityPoco.CustomerName = entityPM.CustomerName;
            entityPoco.SaleCurrencyId = entityPM.SaleCurrencyId;
            entityPoco.ExchangeRate = entityPM.ExchangeRate;
            entityPoco.VolumeUnitCode = entityPM.VolumeUnitCode;
            entityPoco.DimensionsUnitCode = entityPM.DimensionsUnitCode;
            entityPoco.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            entityPoco.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
            entityPoco.PickupDeliveryCWeightUnitCode = entityPM.PickupDeliveryCWeightUnitCode;
            entityPoco.Volume = entityPM.Volume;
            entityPoco.VolumetricWeight = entityPM.VolumetricWeight;
            entityPoco.VolumeInCBM = GetVolumeInCBM(entityPM.VolumeUnitCode, entityPM.Volume);
            entityPoco.GrossWeight = entityPM.GrossWeight;
            entityPoco.GrossWeightInKG = entityPM.GrossWeightInKG = GetWeightInKG(entityPM.GrossWeightUnitCode, entityPM.GrossWeight);
            entityPoco.GrossWeightPerTon = entityPM.GrossWeightPerTon = GetWeightInTon(entityPM.GrossWeightInKG);
            entityPoco.ChargeableWeight = entityPM.ChargeableWeight;
            entityPoco.ChargeableWeightInKG = entityPM.ChargeableWeightInKG = GetChargeableWeightInKG(entityPM.ChargeableWeightUnitCode, entityPM.ChargeableWeight);
            entityPoco.PickupDeliveryChargeableWeight = entityPM.PickupDeliveryChargeableWeight;
            entityPoco.Ratio = entityPM.Ratio;
            entityPoco.PickupDeliveryRatio = entityPM.PickupDeliveryRatio;
            entityPoco.DimFactor = entityPM.DimFactor;
            entityPoco.NumberOfPackages = entityPM.NumberOfPackages;
            entityPoco.NumberOfContainers = entityPM.NumberOfContainers;
            entityPoco.DescriptionOfGoods = entityPM.DescriptionOfGoods;
            entityPoco.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;
            entityPoco.Field2 = entityPM.Field2 != null ? entityPM.Field2.Value : null;
            entityPoco.Field3 = entityPM.Field3 != null ? entityPM.Field3.Value : null;
            entityPoco.Field4 = entityPM.Field4 != null ? entityPM.Field4.Value : null;
            entityPoco.Field5 = entityPM.Field5 != null ? entityPM.Field5.Value : null;
            entityPoco.Field6 = entityPM.Field6 != null ? entityPM.Field6.Value : null;
            entityPoco.Field7 = entityPM.Field7 != null ? entityPM.Field7.Value : null;
            entityPoco.Field8 = entityPM.Field8 != null ? entityPM.Field8.Value : null;
            entityPoco.Field9 = entityPM.Field9 != null ? entityPM.Field9.Value : null;
            entityPoco.Field10 = entityPM.Field10 != null ? entityPM.Field10.Value : null;
            entityPoco.LastModified = entityPM.LastModified;
            entityPoco.Notes = entityPM.Notes;
            entityPoco.IsDangerous = entityPM.IsDangerous;
            entityPoco.IsFreightBySteps = entityPM.IsFreightBySteps;
            entityPoco.ExpirationDate = entityPM.ExpirationDate;
            entityPoco.StartDate = entityPM.StartDate;
            entityPoco.ExpirationDays = entityPM.ExpirationDays;
            entityPoco.BranchId = entityPM.BranchId;
            entityPoco.DepartmentId = entityPM.DepartmentId;
            entityPoco.PackageType1Id = entityPM.PackageType1Id;
            entityPoco.PackageType2Id = entityPM.PackageType2Id;
            entityPoco.PackageType3Id = entityPM.PackageType3Id;
            entityPoco.PackageType4Id = entityPM.PackageType4Id;
            entityPoco.PackageType5Id = entityPM.PackageType5Id;
            entityPoco.PackageType1Quantity = entityPM.PackageType1Quantity;
            entityPoco.PackageType2Quantity = entityPM.PackageType2Quantity;
            entityPoco.PackageType3Quantity = entityPM.PackageType3Quantity;
            entityPoco.PackageType4Quantity = entityPM.PackageType4Quantity;
            entityPoco.PackageType5Quantity = entityPM.PackageType5Quantity;
            entityPoco.QuoteTypeCode = entityPM.QuoteTypeCode;
            entityPoco.IsByContainer = entityPM.IsByContainer;
            entityPoco.IsByKG = entityPM.IsByKG;
            entityPoco.EstimateProfit = entityPM.EstimateProfit;
            entityPoco.EstimateProfitEdited = entityPM.EstimateProfitEdited;
            entityPoco.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
            entityPoco.FreelancerId = entityPM.FreelancerId;
            entityPoco.FreelancerAddressId = entityPM.FreelancerAddressId;
            entityPoco.FreelancerContactId = entityPM.FreelancerContactId;
            entityPoco.ConcurrencyGUID = Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPoco.ConcurrencyGUID;
            entityPoco.FromPartnerId = entityPM.FromPartnerId;
            entityPoco.ToPartnerId = entityPM.ToPartnerId;
            entityPoco.FromPartnerAddressId = entityPM.FromPartnerAddressId;
            entityPoco.ToPartnerAddressId = entityPM.ToPartnerAddressId;
            entityPoco.IncludePickUp = entityPM.IncludePickUp;
            entityPoco.IncludeDelivery = entityPM.IncludeDelivery;
            entityPoco.FromAddressId = entityPM.PickUpAddressId;
            entityPoco.ToAddressId = entityPM.DeliveryAddressId;
            entityPoco.FromAddressCity = entityPM.FromAddressCity;
            entityPoco.FromAddressZipCode = entityPM.FromAddressZipCode;
            entityPoco.FromAddressCountryId = entityPM.FromAddressCountryId;
            entityPoco.ToAddressCity = entityPM.ToAddressCity;
            entityPoco.ToAddressZipCode = entityPM.ToAddressZipCode;
            entityPoco.ToAddressCountryId = entityPM.ToAddressCountryId;
            entityPoco.UsageCount = entityPM.UsageCount;
            entityPoco.LastUsageDate = entityPM.LastUsageDate;
            entityPoco.QuoteClosingReasonCode = entityPM.QuoteClosingReasonCode;
            entityPoco.QuoteClosingReasonId = entityPM.QuoteClosingReasonId;
            entityPoco.SentDate = entityPM.SentDate;
            entityPoco.AcceptedDate = entityPM.AcceptedDate;
            entityPoco.DeclinedDate = entityPM.DeclinedDate;
            entityPoco.BusinessUnitId = entityPM.BusinessUnitId;
            entityPoco.Subject = entityPM.Subject;
            entityPoco.IsSubjectEdited = entityPM.IsSubjectEdited;
            entityPoco.StageId = entityPM.StageId;
            entityPoco.StageDueDate = entityPM.StageDueDate;
            entityPoco.RatingCode = entityPM.RatingCode;
            entityPoco.LastActivityTypeCode = entityPM.LastActivityTypeCode;
            entityPoco.LastActivitySubject = entityPM.LastActivitySubject;
            entityPoco.LastActivityDate = entityPM.LastActivityDate;
            entityPoco.NextActivityTypeCode = entityPM.NextActivityTypeCode;
            entityPoco.NextActivitySubject = entityPM.NextActivitySubject;
            entityPoco.NextActivityDate = entityPM.NextActivityDate;
            entityPoco.OpportunityId = entityPM.OpportunityId;
            entityPoco.IsAutomaticallyClosed = entityPM.IsAutomaticallyClosed;
            entityPoco.AutomaticallyCloseDate = entityPM.AutomaticallyCloseDate;
            entityPoco.AutomaticallyCloseDays = entityPM.AutomaticallyCloseDays;
            entityPoco.TransitTime = entityPM.TransitTime;
            entityPoco.DepartureFrequency = entityPM.DepartureFrequency;
            entityPoco.ETD = entityPM.ETD;
            entityPoco.ETA = entityPM.ETA;
            entityPoco.AgentId = entityPM.AgentId;
            entityPoco.AgentAddressId = entityPM.AgentAddressId;
            entityPoco.AgentContactId = entityPM.AgentContactId;
            entityPoco.AgentReference1 = entityPM.AgentReference1;
            entityPoco.AgentReference2 = entityPM.AgentReference2;
            entityPoco.MoveTypeId = entityPM.MoveTypeId;
            entityPoco.TEU = entityPM.TEU;
            entityPoco.IsSaleCurrencySameAsCost = entityPM.IsSaleCurrencySameAsCost;
            entityPoco.ValueOfGoods = entityPM.ValueOfGoods;
            entityPoco.ValueOfGoodsCurrencyId = entityPM.ValueOfGoodsCurrencyId;
            entityPoco.IsChargesByVAT = entityPM.IsChargesByVAT;
            entityPoco.IsQuoteDataExternal = entityPM.IsQuoteDataExternal;
            entityPoco.LastStageDate = entityPM.LastStageDate;
            entityPoco.QuotationSections = entityPM.QuotationSections;
            entityPoco.NotifyId = entityPM.NotifyId;
            entityPoco.NotifyReference1 = entityPM.NotifyReference1;
            entityPoco.NotifyReference2 = entityPM.NotifyReference2;
            entityPoco.NotifyAddressId = entityPM.NotifyAddressId;
            entityPoco.NotifyContactId = entityPM.NotifyContactId;
            entityPoco.NumberOfFollowUps = entityPM.NumberOfFollowUps;
            entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;
            entityPoco.GrossWeightEdited = entityPM.GrossWeightEdited;
            entityPoco.ChargeableWeightEdited = entityPM.ChargeableWeightEdited;
            entityPoco.QuoteHTMLDocumentId = entityPM.QuoteHTMLDocumentId;
            entityPoco.Field11 = entityPM.Field11 != null ? entityPM.Field11.Value : null;
            entityPoco.Field12 = entityPM.Field12 != null ? entityPM.Field12.Value : null;
            entityPoco.Field13 = entityPM.Field13 != null ? entityPM.Field13.Value : null;
            entityPoco.Field14 = entityPM.Field14 != null ? entityPM.Field14.Value : null;
            entityPoco.Field15 = entityPM.Field15 != null ? entityPM.Field15.Value : null;
            entityPoco.Field16 = entityPM.Field16 != null ? entityPM.Field16.Value : null;
            entityPoco.Field17 = entityPM.Field17 != null ? entityPM.Field17.Value : null;
            entityPoco.Field18 = entityPM.Field18 != null ? entityPM.Field18.Value : null;
            entityPoco.Field19 = entityPM.Field19 != null ? entityPM.Field19.Value : null;
            entityPoco.Field20 = entityPM.Field20 != null ? entityPM.Field20.Value : null;
            entityPoco.CountryForStatisticsId = entityPM.CountryForStatisticsId;
            entityPoco.RequestDate = entityPM.RequestDate;
            entityPoco.EstimatedProfitInLocal = entityPM.EstimatedProfitInLocal;
            entityPoco.EstimatedProfitInProfit = entityPM.EstimatedProfitInProfit;
            entityPoco.ProfitCurrencyId = entityPM.ProfitCurrencyId;
            entityPoco.ProfitExchangeRate = entityPM.ProfitExchangeRate;
            entityPoco.ShipmentSubTypeId = entityPM.ShipmentSubTypeId;
            entityPoco.ShipmentSubTypeId = entityPM.ShipmentSubTypeId;
            entityPoco.PickupDeliveryVolumetricWeight = entityPM.PickupDeliveryVolumetricWeight;
            entityPoco.RegionalTaxId = entityPM.RegionalTaxId;
            entityPoco.RegionalTaxPercentage = entityPM.RegionalTaxPercentage;
            entityPoco.DescriptionRightToLeft = entityPM.DescriptionRightToLeft;
            entityPoco.IsMultiCurrency = entityPM.IsMultiCurrency;
            entityPoco.InlandDomesticFromZipCode = entityPM.InlandDomesticFromZipCode;
            entityPoco.InlandDomesticToZipCode = entityPM.InlandDomesticToZipCode;
            entityPoco.InlandDomesticFromCity = entityPM.InlandDomesticFromCity;
            entityPoco.InlandDomesticToCity = entityPM.InlandDomesticToCity;
            entityPoco.InlandDomesticFromCountryId = entityPM.InlandDomesticFromCountryId;
            entityPoco.InlandDomesticToCountryId = entityPM.InlandDomesticToCountryId;
            entityPoco.InlandDomesticFromTypeCode = entityPM.InlandDomesticFromTypeCode;
            entityPoco.InlandDomesticToTypeCode = entityPM.InlandDomesticToTypeCode;
            entityPoco.MainCarriageFromPortAddress = entityPM.MainCarriageFromPortAddress;
            entityPoco.MainCarriageToPortAddress = entityPM.MainCarriageToPortAddress;

            if (MethodHelper.IsLCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId))
            {
                entityPM.PackagesQuantity = entityPM.NumberOfPackages;
            }

            else
            {
                entityPM.PackagesQuantity = entityPM.NumberOfContainers;
            }

            entityPoco.PackagesQuantity = entityPM.PackagesQuantity;

            BuildSearchField(entityPM, entityPoco);

            entityPM.ConvertToLCL = false;
            entityPM.ConvertToFCL = false;
            entityPM.ConvertTransportMode = false;
        }

        private static void BuildSearchField(QuotePM entityPM, Quote entityPoco)
        {
            string mySearchFields = "";

            int tenant = entityPM.Tenant;

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.QuoteNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Subject);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Notes);

            #region Ports
            QueryHelper.AddPortToSearchFields(ref mySearchFields, tenant, entityPM.FromPortId);
            QueryHelper.AddPortToSearchFields(ref mySearchFields, tenant, entityPM.ToPortId);
            #endregion

            #region Partners

            if (!string.IsNullOrEmpty(entityPM.ShipperId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ShipperId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ConsigneeId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomerId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference2);
                }

                if (!string.IsNullOrEmpty(entityPM.CustomerContactId))
                {
                    Contact myContact = ContactRepository.GetSingleContact(entityPM.CustomerContactId, tenant, true);
                    if (myContact != null)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, myContact.Email);                        
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.NotifyId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.NotifyId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
            #endregion

            #region Carrier
            if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.MainCarriageCarrierId, entityPM.Tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
            #endregion

            if (mySearchFields.Length > 1500)
            {
                mySearchFields = mySearchFields.Substring(0, 1500);
            }

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }

        public static double? GetWeightInKG(string weightCode, double? weight)
        {
            double? myResult = null;

            if (weight != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                myResult = weight * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetWeightInTon(double? weightInKG)
        {
            double? myResult = null;

            if (weightInKG != null)
            {
                myResult = weightInKG / 1000;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }

        public static double? GetChargeableWeightInKG(string weightCode, double? weight)
        {
            double? myResult = null;

            if (weight != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                myResult = weight * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetVolumeInCBM(string volumeCode, double? volume)
        {
            double? myResult = null;

            if (volume != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(volumeCode))
                {
                    switch (volumeCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                    }
                }

                myResult = volume / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
    }
}