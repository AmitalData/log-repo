using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Logitude.BL.QuoteModel.Tools.Behaviours
{
	public class UpdateQuoteComputedFieldBehaviour : IServiceBehaviour
	{
		private QuoteServiceInitializer initializer;
		private QuoteComputedField quoteComputedField;
		private QuotePM quoteEntityPM;
		private List<QuoteChargePM> quoteCharges;
		public void Handle(IServiceInitializer initializer)
		{
			this.initializer = (QuoteServiceInitializer)initializer;
			if (this.initializer.QuoteComputedFieldPOCO != null)
			{
				this.quoteComputedField = this.initializer.QuoteComputedFieldPOCO;
			}
			if (this.initializer.EntityPM != null)
			{
				this.quoteEntityPM = this.initializer.EntityPM;
			}
			this.HandleBehaviour();
		}
		private void HandleBehaviour()
		{
			MapConnectedToShipmentField();
			MapConnectedToTicketField();
			MapFromLocationField();
			MapToLocationField();
			MapPickupFromField();
			MapDeliveryToField();
			FilterDeletedQuoteCharges();
			MapEstimatedPayablesInLocalCurrencyField();
			MapEstimatedPayablesInSalesCurrencyField();
			MapEstimatedReceivablesInLocalCurrencyField();
			MapEstimatedReceivablesInSalesCurrencyField();
			MapMarkupAmountField();
			MapCostChargeGroupValField();

		}

		private void MapConnectedToShipmentField()
		{
			quoteComputedField.ConnectedToShipment = (from shipment in initializer.ShipmentContext.Shipments where shipment.QuoteId == quoteEntityPM.Id && shipment.Tenant == quoteEntityPM.Tenant select shipment).Any();
		}

		private void MapConnectedToTicketField()
		{
			quoteComputedField.ConnectedToTicket = (from tickect in initializer.CRMcontext.Tickets where tickect.QuoteId == quoteEntityPM.Id && tickect.Tenant == quoteEntityPM.Tenant select tickect).Any();
		}

		private void MapFromLocationField()
		{
			if (IsInlandDomestic())
			{
				quoteComputedField.FromLocation = GetLocationAddress(quoteEntityPM.FromPartnerAddressId);
			}
		}

		private void MapToLocationField()
		{
			if (IsInlandDomestic())
			{
				quoteComputedField.ToLocation = GetLocationAddress(quoteEntityPM.ToPartnerAddressId);
			}
		}

		private string GetLocationAddress(string addressId)
		{
			if (!string.IsNullOrEmpty(addressId))
			{
				Address address = initializer.AddressRepository.GetSingleAddress(addressId, initializer.Tenant);
				if (address != null)
				{
					string locationAddress = (!string.IsNullOrEmpty(address.City)) ? address.City : "";

					if (address.Country != null)
					{
						locationAddress = string.IsNullOrEmpty(locationAddress) ? address.Country.Code : locationAddress + " " + address.Country.Code;
					}
					return locationAddress;
				}
			}
			return "";
		}

		private void MapPickupFromField()
		{
			if (quoteEntityPM.IncludePickUp)
			{
				if (!string.IsNullOrEmpty(quoteEntityPM.PickUpAddressId))
				{
					quoteComputedField.PickupFrom = CalculatePickupAndDeliveryToByAddressId(quoteEntityPM.PickUpAddressId);
				}
				else
				{
					quoteComputedField.PickupFrom = CalculatePickupFromUsingFromAddressCountryAndCity();
				}
			}
			else
			{
				quoteComputedField.PickupFrom = null;
			}
		}

		private void MapDeliveryToField()
		{
			if (quoteEntityPM.IncludeDelivery)
			{
				if (!string.IsNullOrEmpty(quoteEntityPM.DeliveryAddressId))
				{
					quoteComputedField.DeliveryTo = CalculatePickupAndDeliveryToByAddressId(quoteEntityPM.DeliveryAddressId);
				}
				else
				{
					quoteComputedField.DeliveryTo = CalculateDeliveryToUsingToAddressCountryAndCity();
				}
			}
			else
			{
				quoteComputedField.DeliveryTo = null;
			}
		}

		private bool IsInlandDomestic()
		{
			return (quoteEntityPM.DirectionId == "D" && quoteEntityPM.TransportModeId == "I");
		}

		private string CalculatePickupAndDeliveryToByAddressId(string id)
		{
			Address address = initializer.AddressRepository.GetSingleAddress(id, initializer.Tenant);
			if (address != null)
			{
				string location = (!string.IsNullOrEmpty(address.City)) ? address.City : "";

				if (address.Country != null)
				{
					location = (string.IsNullOrEmpty(location)) ? address.Country.Code : location + " " + address.Country.Code;
				}
				if (!string.IsNullOrEmpty(address.ZipCode))
				{
					location = (string.IsNullOrEmpty(location)) ? address.ZipCode : location + " - " + address.ZipCode;
				}
				return location;
			}
			return "";
		}

		private string CalculatePickupFromUsingFromAddressCountryAndCity()
		{

			string location = (!string.IsNullOrEmpty(quoteEntityPM.FromAddressCity)) ? quoteEntityPM.FromAddressCity : "";

			if (!string.IsNullOrEmpty(quoteEntityPM.FromAddressCountryId))
			{
				Country country = initializer.CountryRepository.GetSingleCountry(quoteEntityPM.FromAddressCountryId, initializer.Tenant);
				if (country != null)
				{
					location = (string.IsNullOrEmpty(location)) ? country.Code : location + " " + country.Code;
				}
			}
			if (!string.IsNullOrEmpty(quoteEntityPM.FromAddressZipCode))
			{
				location = (string.IsNullOrEmpty(location)) ? quoteEntityPM.FromAddressZipCode : location + " - " + quoteEntityPM.FromAddressZipCode;
			}
			return location;
		}

		private string CalculateDeliveryToUsingToAddressCountryAndCity()
		{
			string location = (!string.IsNullOrEmpty(quoteEntityPM.ToAddressCity)) ? quoteEntityPM.ToAddressCity : "";

			if (!string.IsNullOrEmpty(quoteEntityPM.ToAddressCountryId))
			{
				Country country = initializer.CountryRepository.GetSingleCountry(quoteEntityPM.ToAddressCountryId, initializer.Tenant);
				if (country != null)
				{
					location = (string.IsNullOrEmpty(location)) ? country.Code : location + " " + country.Code; ;
				}
			}
			if (!string.IsNullOrEmpty(quoteEntityPM.ToAddressZipCode))
			{
				location = (string.IsNullOrEmpty(location)) ? quoteEntityPM.ToAddressZipCode : location += " - " + quoteEntityPM.ToAddressZipCode;
			}
			return location;
		}

		private void FilterDeletedQuoteCharges()
		{
			quoteCharges = new List<QuoteChargePM>();
			quoteCharges = quoteEntityPM.QuoteCharges.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
		}

		private void MapEstimatedPayablesInLocalCurrencyField()
		{
			if (quoteEntityPM.QuoteCharges != null)
			{
				quoteComputedField.EstimatedPayablesInLocal = quoteCharges.Sum(d => d.CostTotalAmountLocal);
			}
		}

		private void MapEstimatedPayablesInSalesCurrencyField()
		{
			if (quoteEntityPM.QuoteCharges != null)
			{
				quoteComputedField.EstimatedPayablesInSales = quoteCharges.Sum(d => d.CostAmountInSaleCurrency);
			}
		}

		private void MapEstimatedReceivablesInLocalCurrencyField()
		{
			if (quoteEntityPM.QuoteCharges != null)
			{
				quoteComputedField.EstimatedReceivablesInLocal = quoteCharges.Where(d => d.IsAllIN == false).Sum(d => d.SaleTotalAmountLocal);
			}
		}

		private void MapEstimatedReceivablesInSalesCurrencyField()
		{
			if (quoteEntityPM.QuoteCharges != null)
			{
				quoteComputedField.EstimatedReceivablesInSales = quoteCharges.Where(d => d.IsAllIN == false).Sum(d => d.SaleAmountInSaleCurrency);
			}
		}

		private void MapMarkupAmountField()
		{
			double? summaryCostAmount = 0;
			double? summarySaleAmount = 0;

			if (quoteEntityPM != null)
			{
				var myCostAmountLocal = MethodHelper.Round(quoteEntityPM.QuoteCharges.Sum(a => a.CostTotalAmountLocal), 2);
				var mySaleAmountLocal = MethodHelper.Round(quoteEntityPM.QuoteCharges.Where(f => f.IsAllIN == false).Sum(a => a.SaleTotalAmountLocal), 2);
				summaryCostAmount = MethodHelper.Round(myCostAmountLocal, 2);
				summarySaleAmount = MethodHelper.Round(mySaleAmountLocal, 2);

				quoteComputedField.MarkupPercentage = summaryCostAmount == 0 ? summaryCostAmount : MethodHelper.Round((summarySaleAmount - summaryCostAmount) * 100 / summaryCostAmount, 2);
			}
		}
		private void MapCostChargeGroupValField()
		{
			if (quoteEntityPM?.QuoteCharges == null || !quoteEntityPM.QuoteCharges.Any())
				return;

			ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(initializer.Tenant);
			CurrencyRepository currencyRepository = new CurrencyRepository(initializer.Tenant);
			TenantQuery tenantQuery = new TenantQuery(initializer.Tenant);
			TenantPM tPM = tenantQuery.GetSinglePM(initializer.Tenant);
			string accountingCurrencyId = tPM.CurrencyId;

			var chargesTypes = chargesTypeRepository.GetChargesTypesOfVAL(initializer.Tenant).ToHashSet();
			var quoteChargesVal = quoteCharges
				.Where(d => d != null && chargesTypes.Contains(d.ChargesTypeId)).ToList();

			if (!quoteChargesVal.Any())
				return;

			var distinctCurrencies = quoteChargesVal.Select(d => d.CostCurrencyId).Distinct().ToList();
			var costCurrencyId = distinctCurrencies.Count == 1 ? distinctCurrencies[0] : quoteEntityPM.SaleCurrencyId;


			var costTotalAmount = quoteChargesVal.Sum(d =>
			{
				return d.CostCurrencyId == costCurrencyId
			          ? d.CostTotalAmount
			          : ConvertCurrency(d.CostTotalAmount, d.CostCurrencyId, costCurrencyId, accountingCurrencyId);
			});


			var costCurrencyName = currencyRepository.GetSingleCurrencyById(costCurrencyId, initializer.Tenant, true)?.Code;
			quoteComputedField.CostChargeGroupVal = string.Format(CultureInfo.InvariantCulture, "{0} {1:0.##}", costCurrencyName, costTotalAmount);

		}

		private double? ConvertCurrency(double? amount, string fromCurrencyId, string toCurrencyId,string accountingCurrencyId)
		{
			if (amount == null)
				return 0;

			var ratesTablesRepository = new RatesTableRepository(initializer.Tenant);
			var ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
	

			RatesTablePM rateFrom = ratesTableQuery.GetLastRateByValueDate(initializer.Tenant, fromCurrencyId, accountingCurrencyId, quoteEntityPM.OpenDate);
			RatesTablePM rateTo = ratesTableQuery.GetLastRateByValueDate(initializer.Tenant, toCurrencyId, accountingCurrencyId, quoteEntityPM.OpenDate);

			double fromRate = rateFrom?.Rate ?? 1.0;
			double toRate = rateTo?.Rate ?? 1.0;


			if (toRate == 0)
				throw new InvalidOperationException($"Invalid currency rate for {toCurrencyId}: rate cannot be zero.");

			return (amount.Value * fromRate) / toRate;
		}
	}
}
