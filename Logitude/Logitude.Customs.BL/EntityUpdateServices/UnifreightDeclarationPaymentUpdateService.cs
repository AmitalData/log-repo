using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Logitude.Customs.Def.Messaging.Customs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public class UnifreightDeclarationPaymentUpdateService
    {
        private DeclarationPaymentPM _DirtyDeclarationPaymentPM;
        private AmitalContext _AmitalContext;
        private CCUPAYHANDPM _CCUPAYHAND;
        private DeclarationPaymentPM entityPM;
        private DeclarationPM _DeclarationPM;



        public UnifreightDeclarationPaymentUpdateService(DeclarationPaymentPM entityPM, DeclarationPM declarationPM)
        {
            // TODO: Complete member initialization
            this._DirtyDeclarationPaymentPM = entityPM;
            this._DeclarationPM = declarationPM;
        }

        public void Update()
        {
            if (this._DeclarationPM.PaymentStatusCode =="0" && this._DeclarationPM.Direction=="E")
            {
                return;

            }
            var sw = Stopwatch.StartNew();
            try
            {
                //if ((!Environment.MachineName.Equals("itzik-7-new", StringComparison.OrdinalIgnoreCase)) && (!Environment.MachineName.Equals("yuval-7-new", StringComparison.OrdinalIgnoreCase))) return;
                ///if (String.IsNullOrWhiteSpace(_DirtyDeclarationPaymentPM.DeclarationId))
                ///all ref 2 _DirtyDeclarationPaymentPM check if null !!!!!!!!
                if (_DirtyDeclarationPaymentPM== null || String.IsNullOrWhiteSpace(_DirtyDeclarationPaymentPM.DeclarationId))
                {
                    return;
                }

                //var setting = CustomsSettingQueryService.GetSettingByTenant(_DirtyDeclarationPaymentPM.Tenant);
                //if (!setting.IsConnectedToUniFreight)
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(_DirtyDeclarationPaymentPM.Tenant);
                DeclarationPM declarationPM = declarationQueryService.GetSingle(_DirtyDeclarationPaymentPM.DeclarationId, false, false);
                if (declarationPM == null || (declarationPM != null && !declarationPM.IsConnectedToUnifreight))
                {
                    return;
                }

                long lCUSTOMFILENO;
                if (!long.TryParse(_DeclarationPM.CustomFileNo, out lCUSTOMFILENO))
                {
                    throw new BusinessErrorException("_DirtyDeclarationPaymentPM.DeclarationId could not convert to long ");
                }


                TransactionScope scope = null;//TransactionFactory.GetNewTransaction())//new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
                {
                    scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
                }
                try
                {
                    using (_AmitalContext = AmitalContext.GetContext(_DeclarationPM.Tenant))
                    {
                        //AmitalContext.SetOracleMonitor();
                        var myCCUFILEMQueryService = new CCUFILEMQueryService(_AmitalContext);
                        var myCCUPAYHANDQueryService = new CCUPAYHANDQueryService(_AmitalContext);
                        var myCCUPAYHANDUpdateService = new CCUPAYHANDUpdateService(_AmitalContext);
                        myCCUPAYHANDUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                        int? FILENO = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
                        if (!FILENO.HasValue)
                        {
                            throw new BusinessErrorException("GetFILENOByCUSTOMFILENO return null");
                        }

						int? FILENO1 = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO_forUpdateNOWAIT(lCUSTOMFILENO, _DeclarationPM.Tenant);

						_CCUPAYHAND = myCCUPAYHANDQueryService.GetSingle(FILENO.Value, true, false);

						Boolean noUpdate = false;
                        var currentRequestSheetContext = RequestSheetContext.Current.GetContextOrDefault();
                        if (!(currentRequestSheetContext != null && string.IsNullOrWhiteSpace(currentRequestSheetContext.CustomsRequestsSheetId)))
                        {
                            if (_CCUPAYHAND != null)
                            {
                                if (_CCUPAYHAND.CCUPAYLINEFPMs != null)
                                {
                                    var lineWithPAYORDNO = _CCUPAYHAND.CCUPAYLINEFPMs.Where(r => r.PAYORDNO != null).FirstOrDefault();
                                    if(lineWithPAYORDNO != null)
                                    {
                                        noUpdate = true;
                                    }
                                }
                            }
                        }
                        if (!noUpdate)
                        {
                            if (_CCUPAYHAND != null)
                            {
                                if (_CCUPAYHAND.CCUPAYLINEFPMs != null)
                                {
                                    foreach (var paymentline in _CCUPAYHAND.CCUPAYLINEFPMs)
                                    {
                                        paymentline.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                                    }


                                    _CCUPAYHAND.DeletedCCUPAYLINEFs = _CCUPAYHAND.CCUPAYLINEFPMs;
                                    _CCUPAYHAND.CCUPAYLINEFPMs = null;

                                    _CCUPAYHAND.ChangeSetOp = ChangeSetOperation.Update;
                                    // Update for the Delete
                                    myCCUPAYHANDUpdateService.Update(_CCUPAYHAND, true);
                                    //Clean up the Supplier Invoices
                                    _CCUPAYHAND.CCUPAYLINEFPMs = null;
                                    _CCUPAYHAND.DeletedCCUPAYLINEFs = null;
                                    _CCUPAYHAND.PAYTAX = null; // moran 15.12.15 - bug found by anat - error message in unifreight while entering VAT in billing - calc of total pay lines amount not equal taxes amount - keep adding each pay hand
                                    _CCUPAYHAND.TOTALPAYTAX = null; // moran 15.12.15 - bug found by anat - error message in unifreight while entering VAT in billing - calc of total tax to pay not equal taxes amount - keep adding each pay hand
                                }
                            }

                            DeclarationPayment(FILENO.Value);
                            _CCUPAYHAND.CurrentContextTag = _DeclarationPM.Id;
                            myCCUPAYHANDUpdateService.Update(_CCUPAYHAND, true);
                        }
                        if (scope != null)
                        {
                            scope.Complete();
                        }
                    }

                }
                finally
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                }
            }
            catch (Exception e3)
            {
                //e3.Message="UnifreightDeclarationPaymentUpdateService" +e3.Message ;

                throw new Exception("UnifreightDeclarationPaymentUpdateService unhandle error :" + e3.ToString(), e3);
            }
            finally
            {
                //if(file!=null) file.Close();
                LogMessagingUtil.Instance.AppendLine("UnifreightDeclarationPaymentUpdateService:took:" + sw.Elapsed.ToString());
            }
        }

        private void DeclarationPayment(int FileNo)
        {
            var userRepository = new UserRepository(_DirtyDeclarationPaymentPM.Tenant);

            if (_CCUPAYHAND == null)
            {
                _CCUPAYHAND = new CCUPAYHANDPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    DeclarationId = _DeclarationPM.Id,
                    FILENO = FileNo, // Mirit 02/02/16 Call 260132
                };
            }
            else
            {
                _CCUPAYHAND.ChangeSetOp = ChangeSetOperation.Update;
            }
            
            
            var setting = CustomsSettingQueryService.GetSettingByTenant(_DirtyDeclarationPaymentPM.Tenant);
            if (!setting.IsConnectedToUniFreight)
            {
                _CCUPAYHAND.Tenant =  _DirtyDeclarationPaymentPM.Tenant;
            }

            _CCUPAYHAND.DeclarationId = _DeclarationPM.Id;

            //_CCUPAYHAND.PAYTAX = _DirtyDeclarationPaymentPM.;
            ///_CCUPAYHAND.REJECTTAX = _DirtyDeclarationPaymentPM.;
            if (_DirtyDeclarationPaymentPM.IsProcessA == true)
            {
                _CCUPAYHAND.PROCESSWANT = "א";
            }



            //cCUTAXPM.POSTPONEDTAX = decSupplierInvoiceItemTaxes.DeferedTaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.DeferedTaxAmount");
            //cCUTAXPM.TAXTOPAY = cCUTAXPM.TAXAMOUNT - cCUTAXPM.POSTPONEDTAX;
            // cCUTAXPM.TAXTOPAY = cCUTAXPM.TAXAMOUNT - decDeclarationTaxes.DeferredTaxAmount.ToNullableDouble("decDeclarationTaxes.DeferredTaxAmount");
            //_DeclarationPM.DeclarationTaxes[0].DeferredTaxAmount

            //_CCUPAYHAND.TOTALPAYTAX = _DirtyDeclarationPaymentPM.;
            //_CCUPAYHAND.TOTALPAYDEPOSIT = _DirtyDeclarationPaymentPM.;
            _CCUPAYHAND.HANDDATE = _DirtyDeclarationPaymentPM.PaymentDate;
            _CCUPAYHAND.HANDTYPE = true;
            if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPaymentPM.SignatoryIdentification))
            {
                _CCUPAYHAND.RESHIMONSIGN = _DirtyDeclarationPaymentPM.SignatoryIdentification.Substring(0, Math.Min(9, _DirtyDeclarationPaymentPM.SignatoryIdentification.Length)); // moran 27.7.16 - Task 21585 - take first 9
            }
            _CCUPAYHAND.OBJECTIONEXPLAIN = null; // moran 5.1.17 - AMI-58876
            foreach (var declarationPaymentProtests in _DirtyDeclarationPaymentPM.DeclarationPaymentProtests)
            {
                if (!String.IsNullOrWhiteSpace(_CCUPAYHAND.OBJECTIONEXPLAIN) && !String.IsNullOrWhiteSpace(declarationPaymentProtests.CustomsAgentExplanation))
                {
                    _CCUPAYHAND.OBJECTIONEXPLAIN = _CCUPAYHAND.OBJECTIONEXPLAIN + " ";
                }
                if (!String.IsNullOrWhiteSpace(declarationPaymentProtests.CustomsAgentExplanation))
                {
                    _CCUPAYHAND.OBJECTIONEXPLAIN = _CCUPAYHAND.OBJECTIONEXPLAIN + declarationPaymentProtests.CustomsAgentExplanation;
                }
            }
            if (!string.IsNullOrEmpty(_CCUPAYHAND.OBJECTIONEXPLAIN) && _CCUPAYHAND.OBJECTIONEXPLAIN.Length > 75) _CCUPAYHAND.OBJECTIONEXPLAIN = _CCUPAYHAND.OBJECTIONEXPLAIN.Substring(0, 75);
            User myUser = userRepository.GetSingleUser(_DirtyDeclarationPaymentPM.CreatedByUserId, _DirtyDeclarationPaymentPM.Tenant, true);
            if (myUser != null)
            {
                _CCUPAYHAND.SIGNERID = myUser.Code;
            }

            if (_CCUPAYHAND.TOTALPAYTAX == null)
            {
                _CCUPAYHAND.TOTALPAYTAX = 0;
            }
            foreach (var decDeclarationTaxes in _DeclarationPM.DeclarationTaxes)
            {
                switch (decDeclarationTaxes.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        _CCUPAYHAND.TOTALPAYTAX = _CCUPAYHAND.TOTALPAYTAX.GetValueOrDefault() +
                           (decimal?)decDeclarationTaxes.TotalAmount.ToNullableDouble("decDeclarationTaxes.TotalAmount") -
                        (decimal?)decDeclarationTaxes.DeferredTaxAmount.ToNullableDouble("decDeclarationTaxes.DeferredTaxAmount");
                        break;
                }
            }


            if (!string.IsNullOrWhiteSpace(_DeclarationPM.ImporterId))
            {
                ClientQueryService myClientQueryService = new ClientQueryService(_DeclarationPM.Tenant);
                ClientPM myClientPM = myClientQueryService.GetSingle(_DeclarationPM.ImporterId, false, false);
                if (myClientPM != null)
                {
                    _CCUPAYHAND.IMPORTERNAME = myClientPM.FullName;
                }
                else
                {
                    _CCUPAYHAND.IMPORTERNAME = _DeclarationPM.ImporterName;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_DeclarationPM.ImporterName))
                {
                    _CCUPAYHAND.IMPORTERNAME = _DeclarationPM.ImporterName;
                }
            }


            DoDeclarationPaymentMethods();
        }

        private void DoDeclarationPaymentMethods()
        {
            if (_DirtyDeclarationPaymentPM.DeclarationPaymentMethods.Count < 1) return;

            if (_CCUPAYHAND.PAYTAX == null)
            {
                _CCUPAYHAND.PAYTAX = 0;
            }
            foreach (var decDeclarationPaymentMethods in _DirtyDeclarationPaymentPM.DeclarationPaymentMethods)
            {
                switch (decDeclarationPaymentMethods.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        _CCUPAYHAND.CCUPAYLINEFPMs.Add(SetDeclarationPaymentMethods(decDeclarationPaymentMethods));
                        break;
                }
            }
        }

        private CCUPAYLINEFPM SetDeclarationPaymentMethods(DeclarationPaymentMethodPM decDeclarationPaymentMethods)
        {
            int resultInt;
            var customBankRepository = new CustomBankRepository(_DirtyDeclarationPaymentPM.Tenant);

            Unifreight.BL.EntityPMs.CCUPAYLINEFPM curCCUPAYLINEF = new Unifreight.BL.EntityPMs.CCUPAYLINEFPM();
            curCCUPAYLINEF.ChangeSetOp = ChangeSetOperation.Insert;

            curCCUPAYLINEF.TYPE = 1;
            curCCUPAYLINEF.PAYMETHOD = decDeclarationPaymentMethods.MethodTypeCode;
            if (curCCUPAYLINEF.PAYMETHOD != null && curCCUPAYLINEF.PAYMETHOD.Length == 1)
            {
                curCCUPAYLINEF.PAYMETHOD = "0" + curCCUPAYLINEF.PAYMETHOD;
            }
            curCCUPAYLINEF.PAYAMOUNT = (decimal?)(decDeclarationPaymentMethods.Amount.ToNullableDouble("decDeclarationPaymentMethods.Amount"));
            _CCUPAYHAND.PAYTAX = (_CCUPAYHAND.PAYTAX.GetValueOrDefault() + (decimal?)decDeclarationPaymentMethods.Amount.ToNullableDouble("decDeclarationPaymentMethods.Amount"));

            //curCCUPAYLINEF.TYPE = decDeclarationPaymentMethods.;
            if (decDeclarationPaymentMethods.PayerActivityTypeCode == "0")
            {
                curCCUPAYLINEF.PAYEETYPE = 2;
            }
            if (decDeclarationPaymentMethods.PayerActivityTypeCode == "3")
            {
                curCCUPAYLINEF.PAYEETYPE = 1;
            }

            CustomBank myCustomBank = customBankRepository.GetSingle(decDeclarationPaymentMethods.InternalBankId, _DirtyDeclarationPaymentPM.Tenant);
            if (myCustomBank != null)
            {
                curCCUPAYLINEF.ACCOUNTNAME = myCustomBank.LocalName;
            }
            //string translatedCode = GetTranslationP2L("IIGC", "GTBBANK", decDeclarationPaymentMethods.BankCode);
            //curCCUPAYLINEF.BANKID = translatedCode.ToNullableInt("translatedCode");
            curCCUPAYLINEF.BANKID = decDeclarationPaymentMethods.BankCode.ToNullableInt("decDeclarationPaymentMethods.BankCode");
            curCCUPAYLINEF.BANKBRANCH = decDeclarationPaymentMethods.BranchCode;
            curCCUPAYLINEF.BANKACCOUNT = decDeclarationPaymentMethods.AccountNumber;
            curCCUPAYLINEF.PAYORDNO = _DeclarationPM.PaymentOrderNumber.ToNullableInt("_DeclarationPM.PaymentOrderNumber"); //Yuval Chalup 20.09.2015 TASK-16498      

            var setting = CustomsSettingQueryService.GetSettingByTenant(_DirtyDeclarationPaymentPM.Tenant);
            if (!setting.IsConnectedToUniFreight)
            {
                curCCUPAYLINEF.Tenant = _DirtyDeclarationPaymentPM.Tenant;
            }

            return curCCUPAYLINEF;
        }

        private int GetCounter(DeclarationPaymentPM _DeclarationPaymentPM)
        {
            int i = Convert.ToInt32(_DeclarationPaymentPM.DeclarationId.Replace("-", ""));
            return 50000000 + i;
        }

        private string GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {
            var myGTRTRANQueryService = new GTRTRANQueryService(_AmitalContext);

            if (partnerID == null || tableID == null || partnerCode == null)
            {
                return ("");
            }

            GTRTRAN myGTRTRANPM = myGTRTRANQueryService.GetTranslationP2L(partnerID, tableID, partnerCode);

            if (myGTRTRANPM == null)
            {
                return ("");
            }

            return (myGTRTRANPM.LOCALCODE);
        }
    }
}
