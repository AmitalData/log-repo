using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;

using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
   public partial class ARInvoiceLineQueryService
    {

        public List<ARInvoiceLinePM> ARInvoiceLineCustomDataMappingAndValidatin(ARInvoice MyEntity,List<ARInvoiceLine> lines, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                

                var MyList = new List<ARInvoiceLinePM>();
                foreach (var item in lines)
                {
                    var temp = new ARInvoiceLinePM();
                    if (!string.IsNullOrEmpty(item.Id))
                    {
                        temp = query.GetSinglePM(item.Id, Tenant);
                    }

                    if (temp == null)
                    {
                        throw new ApplicationException("ARInvoiceLine with Id " + item.Id + " doesn't exist");
                    }
                    if (string.IsNullOrEmpty(temp.Id))
                    {
                        temp.Id = item.Id;
                    }
                    temp.LineNumber = item.LineNumber;
                    ChargesTypePM myChargesTypePM = new ChargesTypePM();
                    ChargesTypeQueryService ChargesTypeChargesTypeService = new ChargesTypeQueryService(Tenant);
                    if (item.ChargesType != null)
                    {
                         myChargesTypePM = ChargesTypeChargesTypeService.ChargesTypeCustomDataMappingAndValidatin(item.ChargesType, Tenant);
                        if (myChargesTypePM != null)
                        {
                            temp.ChargesTypeId = myChargesTypePM.Id;
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Charge type code in invoice line is required");
                    }


                    if (item.GLAccountId != null)
                    {


                        IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                        GLAccountPM accountPM = glAccountQuery.GetGLAccountByInternalNumber(item.GLAccountId, MyEntity.Tenant);
                        if (accountPM != null)
                        {
                            temp.GLAccountId = accountPM.Id;
                        }
                        else
                        {
                            throw new ApplicationException("Opposite account is not found");
                        }
                    }
                  
                    CurrencyQueryService ForiegnCurrencyCurrencyService = new CurrencyQueryService(Tenant);
                    if (item.ForeignCurrency != null)
                    {
                        var myForiegnCurrencyPM = ForiegnCurrencyCurrencyService.CurrencyCustomDataMappingAndValidatin(item.ForeignCurrency, Tenant);
                        if (myForiegnCurrencyPM != null)
                        {
                            temp.ForiegnCurrencyId = myForiegnCurrencyPM.Id;
                        }
                    }
                    temp.ForiegnExchangeRate = item.ForeignExchangeRate;
                    temp.LocalCurrencyAmount = item.LocalCurrencyAmount;
                    temp.ForiegnCurrencyAmount = item.ForeignCurrencyAmount;
                    temp.LocalDescription = item.LocalDescription;
                    temp.Notes = item.Notes;
                    temp.ValueDate = item.ValueDate;
                    temp.DateForInterest = item.DateForInterest;
                    VatTypeQueryService VatTypeVatTypeService = new VatTypeQueryService(Tenant);
                    if (item.VatType != null)
                    {
                        var myVatTypePM = VatTypeVatTypeService.VatTypeCustomDataMappingAndValidatin(item.VatType, Tenant);
                        if (myVatTypePM != null)
                        {
                            temp.VatTypeId = myVatTypePM.Id;
                        }
                    }
                    temp.Description = item.Description;
                    temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                    temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                    temp.VatPercentage = item.VatPercentage;
                    temp.UnitPrice = item.UnitPriceInForeignCurrency;
                    temp.ExchangeRateDate = item.ExchangeRateDate;
                    temp.Quantity = item.Quantity;
                    temp.Tenant = Tenant;
                    //temp.LineActionCode = item.LineActionCode;
                    ARInvoiceLineActionQueryService ARInvoiceLineActionQuery = new ARInvoiceLineActionQueryService(Tenant);
                    if (item.ARInvoiceLineAction != null)
                    {
                        var ARInvoiceLineActionPM = ARInvoiceLineActionQuery.ARInvoiceLineActionDataMappingAndValidatin(item.ARInvoiceLineAction, Tenant);
                        if (ARInvoiceLineActionPM != null)
                        {
                            temp.LineActionCode = ARInvoiceLineActionPM.Code;
                        }
                    }

                    if (myChargesTypePM != null)
                    {
                        if (string.IsNullOrEmpty(temp.Description))
                        {
                            temp.Description = myChargesTypePM.EnglishName;
                        }
                        if (string.IsNullOrEmpty(temp.LocalDescription))
                        {
                            temp.LocalDescription = myChargesTypePM.LocalName;
                        }
                    }
                 //   temp.EntityId = "1";
                    MyList.Add(temp);
                }

                return MyList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ARInvoiceLine> ARInvoiceLineCustomDataMapping(ARInvoicePM MyEntity,List<ARInvoiceLinePM> lines, int Tenant)
        {
            try
            {

                var MyList = new List<ARInvoiceLine>();
                foreach (var item in lines)
                {

                    var temp = new ARInvoiceLine();
                    temp.Id = item.Id;
                    temp.LineNumber = item.LineNumber;

                    if (item.ChargesTypeId != null)
                    {
                        ChargesTypeQueryService ChargesTypeService0 = new ChargesTypeQueryService(Tenant);
                        temp.ChargesType = ChargesTypeService0.ChargesTypeCustomDataMapping(item.ChargesTypeId, Tenant);

                    }


                    if (item.GLAccountId != null)
                    {
                        IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                        GLAccountPM accountPM = glAccountQuery.GetSingleGLAccountPM(item.GLAccountId, MyEntity.Tenant);
                        if (accountPM != null)
                        {
                            temp.GLAccountId = accountPM.InternalNumber;
                        }

                    }
                    if (item.ForiegnCurrencyId != null)
                    {
                        CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
                        temp.ForeignCurrency = CurrencyService1.CurrencyCustomDataMapping(item.ForiegnCurrencyId, Tenant);

                    }
                    temp.ForeignExchangeRate = item.ForiegnExchangeRate;
                    temp.LocalCurrencyAmount = item.LocalCurrencyAmount;
                    temp.ForeignCurrencyAmount = item.ForiegnCurrencyAmount;
                    temp.LocalDescription = item.LocalDescription;
                    temp.Notes = item.Notes;
                    temp.ValueDate = item.ValueDate;
                    temp.DateForInterest = item.DateForInterest;
                    temp.Quantity = item.Quantity;
                    temp.UnitPriceInForeignCurrency = item.UnitPrice;
                    if (item.VatTypeId != null)
                    {
                        VatTypeQueryService VatTypeService2 = new VatTypeQueryService(Tenant);
                        temp.VatType = VatTypeService2.VatTypeCustomDataMapping(item.VatTypeId, Tenant);

                    }
                    temp.Description = item.Description;
                    temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                    temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                    temp.VatPercentage = item.VatPercentage;
                    //temp.LineActionCode = item.LineActionCode;

                    if (item.LineActionCode != null)
                    {
                        ARInvoiceLineActionQueryService ARInvoiceLineActionQueryService = new ARInvoiceLineActionQueryService(Tenant);
                        temp.ARInvoiceLineAction = ARInvoiceLineActionQueryService.ARInvoiceLineActionDataMapping(item.LineActionCode, Tenant);

                    }




                    MyList.Add(temp);
                }

                return MyList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetARInvoiceLineGLAccount(ARInvoiceLine MyEntity) {

           
          
             

        }


    }
}
