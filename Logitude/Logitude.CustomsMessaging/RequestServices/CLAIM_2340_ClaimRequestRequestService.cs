using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using UnifreightIIG.Common.ClaimAnswerServiceReference;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data.EntityPOCOs;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CLAIM_2340_ClaimRequestRequestService : RequestServiceBase<CLAIM_MSG1_ClaimRequest, CLAIM_2340_ClaimRequestRequestParams>
    {
        private ClaimPM _ClaimPM;
        private ICustomContext _DbContext;

        public override CLAIM_MSG1_ClaimRequest GetRequest(CLAIM_2340_ClaimRequestRequestParams requestParams)
        {
            //Build request 2340- Send Claim Request
            var myClaimRequest = new CLAIM_MSG1_ClaimRequest();
            myClaimRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            this._DbContext = CustomContext.GetContext(requestParams.Tenant);
            var myClaimQueryService = new ClaimQueryService(this._DbContext);
            _ClaimPM = myClaimQueryService.GetSingle(requestParams.AppicationId, true, false);

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
            this.MyRequestSheetParam.EntityId1 = _ClaimPM.Id;
            this.MyRequestSheetParam.RequestDescription = "הגשת בקשה לתביעה " + _ClaimPM.ClaimSubmiterNumber;

            if (_ClaimPM.ClaimsRelatedEntities != null && _ClaimPM.ClaimsRelatedEntities.Count == 1 &&
                _ClaimPM.ClaimsRelatedEntities.FirstOrDefault().ClaimEntityTypeCode == "1055"  && !string.IsNullOrWhiteSpace(_ClaimPM.ClaimsRelatedEntities.FirstOrDefault().ExternalClaimNumber))
            {
                this.MyRequestSheetParam.CustomFileNo = _ClaimPM.ClaimsRelatedEntities.FirstOrDefault().ExternalClaimNumber;
            }

            myClaimRequest.ClaimRequest = GetClaimRequest();
            myClaimRequest.ImporterContactDetails = GetImporterContactDetails();
            myClaimRequest.ClaimSubmiter = GetClaimSubmiter();
            myClaimRequest.ClaimDetail = GetClaimDetail(requestParams.ClaimsRelatedEntitiesList);
            myClaimRequest.ClaimRefundMethod = GetClaimRefundMethod();
            myClaimRequest.ImporterDeclaration = GetImporterDeclaration();
            //myClaimRequest.Attachment = GetClaimAttachment();

            return myClaimRequest;
        }

        private CLAIM_MSG1_ClaimRequestImporterDeclaration GetImporterDeclaration()
        {
            if (string.IsNullOrEmpty(_ClaimPM.ImporterAffidavit)
                && (_ClaimPM.ClaimImporterDeclarsPage3 == null || (_ClaimPM.ClaimImporterDeclarsPage3 != null && _ClaimPM.ClaimImporterDeclarsPage3.Count == 0))
                && (_ClaimPM.ClaimImporterDeclarsPage3A == null || (_ClaimPM.ClaimImporterDeclarsPage3A != null && _ClaimPM.ClaimImporterDeclarsPage3A.Count == 0))
                && string.IsNullOrEmpty(_ClaimPM.RawMaterialsDescription))
            {
                return null;
            }
            CLAIM_MSG1_ClaimRequestImporterDeclaration claimRequestImporterDeclaration = new CLAIM_MSG1_ClaimRequestImporterDeclaration();
            claimRequestImporterDeclaration.importerAffidavit = _ClaimPM.ImporterAffidavit;
            claimRequestImporterDeclaration.DeclarationPag3 = GetImporterDeclarationPag3();
            claimRequestImporterDeclaration.DeclarationPag3ACommercialSaleCode = GetImporterDeclarationPag3A();
            claimRequestImporterDeclaration.DeclarationPag3B = GetGetImporterDeclarationPag3B();
            if (!string.IsNullOrEmpty(_ClaimPM.RawMaterialsDescription))
            {
                claimRequestImporterDeclaration.DeclarationPag3C = new CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3C();
                claimRequestImporterDeclaration.DeclarationPag3C.rawMaterialsDescription = _ClaimPM.RawMaterialsDescription;
            }
            return claimRequestImporterDeclaration;
        }

        private CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3B[] GetGetImporterDeclarationPag3B()
        {
            if (_ClaimPM.ClaimImporterDeclarsPage3B == null)
            {
                return null;
            }

            List<CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3B> claimRequestImporterDeclarationDeclarationPag3BList = new List<CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3B>();

            foreach (var declarationPag3BItem in _ClaimPM.ClaimImporterDeclarsPage3B)
            {
                CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3B claimRequestImporterDeclarationDeclarationPag3B = new CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3B();
                claimRequestImporterDeclarationDeclarationPag3B.saleAmountAfter = declarationPag3BItem.SaleAmountAfter > 0 ? (decimal)declarationPag3BItem.SaleAmountAfter : 0;
                claimRequestImporterDeclarationDeclarationPag3B.saleAmountATClaimTime = declarationPag3BItem.SaleAmountClaim > 0 ? (decimal)declarationPag3BItem.SaleAmountClaim : 0;
                claimRequestImporterDeclarationDeclarationPag3B.saleAmountBefore = declarationPag3BItem.SaleAmountBefore > 0 ? (decimal)declarationPag3BItem.SaleAmountBefore : 0;
                claimRequestImporterDeclarationDeclarationPag3B.descriptionOfGoods = declarationPag3BItem.DescriptionOfGoods;
                claimRequestImporterDeclarationDeclarationPag3B.inventoryAmount = declarationPag3BItem.InventoryAmount > 0 ? (decimal)declarationPag3BItem.InventoryAmount : 0;
                claimRequestImporterDeclarationDeclarationPag3B.soldGoodsAmount = declarationPag3BItem.SoldGoodsAmount > 0 ? (decimal)declarationPag3BItem.SoldGoodsAmount : 0;
                claimRequestImporterDeclarationDeclarationPag3BList.Add(claimRequestImporterDeclarationDeclarationPag3B);
            }

            return claimRequestImporterDeclarationDeclarationPag3BList.ToArray();
        }

        private int[] GetImporterDeclarationPag3A()
        {
            if (_ClaimPM.ClaimImporterDeclarsPage3A == null)
            {
                return null;
            }

            List<int> claimImporterDeclarationsPage3AList = new List<int>();

            foreach (var commercialSaleItem in _ClaimPM.ClaimImporterDeclarsPage3A)
            {
                if (!string.IsNullOrWhiteSpace(commercialSaleItem.CommercialSaleTypeCode))
                {
                    int commercialSaleType;
                    int.TryParse(commercialSaleItem.CommercialSaleTypeCode, out commercialSaleType);
                    claimImporterDeclarationsPage3AList.Add(commercialSaleType);
                }
            }

            return claimImporterDeclarationsPage3AList.ToArray();
        }

        private CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3[] GetImporterDeclarationPag3()
        {
            List<CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3> claimRequestImporterDeclarationDeclarationPag3List = new List<CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3>();

            foreach (var claimImporterDeclarationsPage3Item in _ClaimPM.ClaimImporterDeclarsPage3)
            {
                if (!string.IsNullOrWhiteSpace(claimImporterDeclarationsPage3Item.ImporterLoiDeclarationTypeCode))
                {
                    int importerLoIDeclarationTypeCode;
                    int.TryParse(claimImporterDeclarationsPage3Item.ImporterLoiDeclarationTypeCode, out importerLoIDeclarationTypeCode);
                    CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3 claimRequestImporterDeclarationDeclarationPag3 = new CLAIM_MSG1_ClaimRequestImporterDeclarationDeclarationPag3();
                    claimRequestImporterDeclarationDeclarationPag3.importerLoIDeclarationTypeCode = importerLoIDeclarationTypeCode;

                    if (claimImporterDeclarationsPage3Item.ClaimImporterDeclarsP3Loi != null)
                    {
                        List<string> loiInDeclarationPag3loiIDList = new List<string>();
                        foreach (var declarationPag3loiItem in claimImporterDeclarationsPage3Item.ClaimImporterDeclarsP3Loi)
                        {
                            loiInDeclarationPag3loiIDList.Add(declarationPag3loiItem.DeclarationNumber);
                        }
                        claimRequestImporterDeclarationDeclarationPag3.LoiInDeclarationPag3loiID = loiInDeclarationPag3loiIDList.ToArray();
                    }

                    claimRequestImporterDeclarationDeclarationPag3List.Add(claimRequestImporterDeclarationDeclarationPag3);
                }
            }

            return claimRequestImporterDeclarationDeclarationPag3List.ToArray();
        }

        private CLAIM_MSG1_ClaimRequestClaimRefundMethod GetClaimRefundMethod()
        {
            if (string.IsNullOrWhiteSpace(_ClaimPM.BeneficiaryExternalID) && string.IsNullOrWhiteSpace(_ClaimPM.BeneficiaryActivityTypeCode) && string.IsNullOrWhiteSpace(_ClaimPM.AccountCountryCode)
                && string.IsNullOrWhiteSpace(_ClaimPM.AccountNumber) && string.IsNullOrWhiteSpace(_ClaimPM.ForeignAccountNumber))
            {
                return null;
            }
            CLAIM_MSG1_ClaimRequestClaimRefundMethod claimRequestClaimRefundMethod = new CLAIM_MSG1_ClaimRequestClaimRefundMethod();

            if (!string.IsNullOrWhiteSpace(_ClaimPM.BeneficiaryExternalID))
            {
                int beneficiaryExternalID = 0;
                int.TryParse(_ClaimPM.BeneficiaryExternalID, out beneficiaryExternalID);
                claimRequestClaimRefundMethod.BeneficiaryExternalID = beneficiaryExternalID;
            }

            if (!string.IsNullOrWhiteSpace(_ClaimPM.BeneficiaryActivityTypeCode))
            {
                int beneficiaryActivityType = 0;
                int.TryParse(_ClaimPM.BeneficiaryActivityTypeCode, out beneficiaryActivityType);
                claimRequestClaimRefundMethod.BeneficiaryActivityType = beneficiaryActivityType;
            }

            //AccountDetails
            claimRequestClaimRefundMethod.accountCountry = _ClaimPM.AccountCountryCode;
            if (!string.IsNullOrWhiteSpace(_ClaimPM.BankTypeCode) || !string.IsNullOrWhiteSpace(_ClaimPM.AccountBranchCode) || !string.IsNullOrWhiteSpace(_ClaimPM.AccountNumber))
            {
                claimRequestClaimRefundMethod.AccountDetails = new AccountDetails();
                if (!string.IsNullOrWhiteSpace(_ClaimPM.BankTypeCode))
                {
                    int codeBank = 0;
                    int.TryParse(_ClaimPM.BankTypeCode, out codeBank);
                    claimRequestClaimRefundMethod.AccountDetails.bank = codeBank;
                    claimRequestClaimRefundMethod.AccountDetails.bankSpecified = true;
                }
                if (!string.IsNullOrWhiteSpace(_ClaimPM.AccountBranchCode))
                {
                    int branch = 0;
                    if (_ClaimPM.AccountBranchCode.Contains(","))
                    {
                        string[] branchArray = _ClaimPM.AccountBranchCode.Split(',');
                        _ClaimPM.AccountBranchCode = branchArray[0];
                    }
                    int.TryParse(_ClaimPM.AccountBranchCode, out branch);
                    claimRequestClaimRefundMethod.AccountDetails.branch = branch;
                    claimRequestClaimRefundMethod.AccountDetails.branchSpecified = true;
                }
                claimRequestClaimRefundMethod.AccountDetails.accountNumber = _ClaimPM.AccountNumber;
            }

            //ForeignAccountDetails
            if (!string.IsNullOrWhiteSpace(_ClaimPM.AccountCurrencyTypeCode) || !string.IsNullOrWhiteSpace(_ClaimPM.ForeignAccountNumber)
                || !string.IsNullOrWhiteSpace(_ClaimPM.ForeignBank) || !string.IsNullOrWhiteSpace(_ClaimPM.ForeignBranch))
            {
                claimRequestClaimRefundMethod.ForeignAccountDetails = new CLAIM_MSG1_ClaimRequestClaimRefundMethodForeignAccountDetails();
                claimRequestClaimRefundMethod.ForeignAccountDetails.accountCurrency = _ClaimPM.AccountCurrencyTypeCode;
                claimRequestClaimRefundMethod.ForeignAccountDetails.accountNumber = _ClaimPM.ForeignAccountNumber;
                if (!string.IsNullOrWhiteSpace(_ClaimPM.ForeignBank))
                {
                    int foreignBank = 0;
                    int.TryParse(_ClaimPM.ForeignBank, out foreignBank);
                    claimRequestClaimRefundMethod.ForeignAccountDetails.foreignBank = foreignBank;
                }
                if (!string.IsNullOrWhiteSpace(_ClaimPM.ForeignBranch))
                {
                    int foreignBranch = 0;
                    int.TryParse(_ClaimPM.ForeignBranch, out foreignBranch);
                    claimRequestClaimRefundMethod.ForeignAccountDetails.foreignBranch = foreignBranch;
                }
            }

            return claimRequestClaimRefundMethod;
        }

        private CLAIM_MSG1_ClaimRequestClaimDetail[] GetClaimDetail(List<string> claimsRelatedEntitiesList)
        {
            List<CLAIM_MSG1_ClaimRequestClaimDetail> claimRequestClaimDetaillist = new List<CLAIM_MSG1_ClaimRequestClaimDetail>();

            foreach (var claimsRelatedEntity in _ClaimPM.ClaimsRelatedEntities)
            {
                if (claimsRelatedEntitiesList.Contains(claimsRelatedEntity.EntityCounterKey.ToString()))
                {
                    CLAIM_MSG1_ClaimRequestClaimDetail claimRequestClaimDetail = new CLAIM_MSG1_ClaimRequestClaimDetail();
                    if (!string.IsNullOrWhiteSpace(claimsRelatedEntity.ClaimEntityTypeCode))
                    {
                        int claimEntity;
                        int.TryParse(claimsRelatedEntity.ClaimEntityTypeCode, out claimEntity);
                        claimRequestClaimDetail.claimEntity = claimEntity;
                    }
                    claimRequestClaimDetail.claimEntityID = claimsRelatedEntity.ClaimEntityNumber;
                    claimRequestClaimDetail.externalClaimNumber = claimsRelatedEntity.ExternalClaimNumber;
                    if (!string.IsNullOrWhiteSpace(claimsRelatedEntity.CourtCode))
                    {
                        int courtCode;
                        int.TryParse(claimsRelatedEntity.CourtCode, out courtCode);
                        claimRequestClaimDetail.courts = courtCode;
                        claimRequestClaimDetail.courtsSpecified = true;
                    }
                    claimRequestClaimDetail.proceedingNumber = claimsRelatedEntity.ProceedingNumber;
                    claimRequestClaimDetail.isFinancialRefundDemand = claimsRelatedEntity.IsFinancialRefundDemand;
                    if (!string.IsNullOrWhiteSpace(claimsRelatedEntity.SeconderyClaimEntityCode))
                    {
                        int seconderyClaimEntity;
                        int.TryParse(claimsRelatedEntity.SeconderyClaimEntityCode, out seconderyClaimEntity);
                        claimRequestClaimDetail.seconderyClaimEntity = seconderyClaimEntity;
                        claimRequestClaimDetail.seconderyClaimEntitySpecified = true;
                    }
                    claimRequestClaimDetail.seconderyClaimEntityID = claimsRelatedEntity.SeconderyClaimEntityID;
                    claimRequestClaimDetail.Version = claimsRelatedEntity.DeclarationVersion;
                    claimRequestClaimDetail.VersionSpecified = claimRequestClaimDetail.Version != null && claimRequestClaimDetail.Version > 0 ? true : false;
                    claimRequestClaimDetail.committeeDecisionNumber = claimsRelatedEntity.CommitteeDecisionNumber;
                    claimRequestClaimDetail.abandonmentANDdestructionReference = claimsRelatedEntity.AbandonmentDestructionReferenc;
                    claimRequestClaimDetail.warehouseNumber = claimsRelatedEntity.WarehouseTypeCode;
                    claimRequestClaimDetail.ClaimDetailClaimAmount = GetClaimDetailClaimAmount(claimsRelatedEntity);
                    claimRequestClaimDetail.ClaimReason = GetClaimReason(claimsRelatedEntity);
                    claimRequestClaimDetail.exportListOfItemsID = GetClaimExportDeclaration(claimsRelatedEntity);
                    claimRequestClaimDetail.ClaimAttachmentID = GetClaimAttachmentID(claimsRelatedEntity);
                    claimRequestClaimDetaillist.Add(claimRequestClaimDetail);
                }
            }

            return claimRequestClaimDetaillist.ToArray();
        }

        private string[] GetClaimExportDeclaration(ClaimsRelatedEntityPM claimsRelatedEntity)
        {
            List<string> claimExportDeclarationList = new List<string>();

            if (claimsRelatedEntity.ClaimsRelatedEntsExpDeclars != null)
            {
                foreach (var claimsRelatedEntsExpDeclarItem in claimsRelatedEntity.ClaimsRelatedEntsExpDeclars)
                {
                    if (!string.IsNullOrWhiteSpace(claimsRelatedEntsExpDeclarItem.ExportDeclarationNumber))
                    {
                        claimExportDeclarationList.Add(claimsRelatedEntsExpDeclarItem.ExportDeclarationNumber);
                    }
                }
            }

            return claimExportDeclarationList.ToArray();
        }

        private CLAIM_MSG1_ClaimRequestClaimDetailClaimDetailClaimAmount GetClaimDetailClaimAmount(ClaimsRelatedEntityPM claimsRelatedEntities)
        {
            CLAIM_MSG1_ClaimRequestClaimDetailClaimDetailClaimAmount claimRequestClaimDetailClaimDetailClaimAmount = new CLAIM_MSG1_ClaimRequestClaimDetailClaimDetailClaimAmount();
            //claimRequestClaimDetailClaimDetailClaimAmount.claimAmount = claimsRelatedEntities.ClaimAmount;
            //claimRequestClaimDetailClaimDetailClaimAmount.claimAmountSpecified = claimsRelatedEntities.ClaimAmount > 0 ? true : false;
            claimRequestClaimDetailClaimDetailClaimAmount.claimAmountSpecified = false;

            if (claimsRelatedEntities.ClaimsRelatedEntitiesAmounts != null)
            {
                List<CLAIM_MSG1_ClaimRequestClaimDetailClaimDetailClaimAmountPaymentComponentAmount> paymentComponentAmountList = new List<CLAIM_MSG1_ClaimRequestClaimDetailClaimDetailClaimAmountPaymentComponentAmount>();
                foreach (var claimsRelatedEntitiesAmountItem in claimsRelatedEntities.ClaimsRelatedEntitiesAmounts)
                {
                    if (!string.IsNullOrWhiteSpace(claimsRelatedEntitiesAmountItem.PaymentTypeCode))
                    {
                        CLAIM_MSG1_ClaimRequestClaimDetailClaimDetailClaimAmountPaymentComponentAmount claimRequestClaimDetailClaimDetailClaimAmountPaymentComponentAmount = new CLAIM_MSG1_ClaimRequestClaimDetailClaimDetailClaimAmountPaymentComponentAmount();
                        int paymentTypeCode;
                        int.TryParse(claimsRelatedEntitiesAmountItem.PaymentTypeCode, out paymentTypeCode);
                        claimRequestClaimDetailClaimDetailClaimAmountPaymentComponentAmount.paymentCompanentCode = paymentTypeCode;
                        claimRequestClaimDetailClaimDetailClaimAmountPaymentComponentAmount.amount = claimsRelatedEntitiesAmountItem.Amount > 0 ? (decimal)claimsRelatedEntitiesAmountItem.Amount : 0;
                        paymentComponentAmountList.Add(claimRequestClaimDetailClaimDetailClaimAmountPaymentComponentAmount);
                    }
                }
                claimRequestClaimDetailClaimDetailClaimAmount.PaymentComponentAmount = paymentComponentAmountList.ToArray();
            }
            return claimRequestClaimDetailClaimDetailClaimAmount;
        }

        private CLAIM_MSG1_ClaimRequestClaimDetailClaimReason GetClaimReason(ClaimsRelatedEntityPM claimsRelatedEntities)
        {
            CLAIM_MSG1_ClaimRequestClaimDetailClaimReason claimRequestClaimDetailClaimReason = new CLAIM_MSG1_ClaimRequestClaimDetailClaimReason();
            claimRequestClaimDetailClaimReason.claimExplanation = claimsRelatedEntities.ClaimExplanation;

            if (claimsRelatedEntities.ClaimsRelatedEntitiesReasons != null)
            {
                List<CLAIM_MSG1_ClaimRequestClaimDetailClaimReasonClaimReasonList> claimReasonList = new List<CLAIM_MSG1_ClaimRequestClaimDetailClaimReasonClaimReasonList>();
                foreach (var claimsRelatedEntitiesReasonsItem in claimsRelatedEntities.ClaimsRelatedEntitiesReasons)
                {
                    if (!string.IsNullOrWhiteSpace(claimsRelatedEntitiesReasonsItem.ReasonListTypeCode))
                    {
                        int reasonListCode;
                        int.TryParse(claimsRelatedEntitiesReasonsItem.ReasonListTypeCode, out reasonListCode);
                        CLAIM_MSG1_ClaimRequestClaimDetailClaimReasonClaimReasonList claimRequestClaimDetailClaimReasonClaimReasonList = new CLAIM_MSG1_ClaimRequestClaimDetailClaimReasonClaimReasonList();
                        claimRequestClaimDetailClaimReasonClaimReasonList.reasonListCod = reasonListCode;
                        if (claimsRelatedEntitiesReasonsItem.ClaimsRelatedEntsReasonsExps != null)
                        {
                            List<CLAIM_MSG1_ClaimRequestClaimDetailClaimReasonClaimReasonListExplanationList> claimRequestClaimDetailClaimReasonClaimReasonListExplanationList = new List<CLAIM_MSG1_ClaimRequestClaimDetailClaimReasonClaimReasonListExplanationList>();
                            foreach (var claimsRelatedEntsReasonsExpItem in claimsRelatedEntitiesReasonsItem.ClaimsRelatedEntsReasonsExps)
                            {
                                if (!string.IsNullOrWhiteSpace(claimsRelatedEntsReasonsExpItem.ClaimExplanationTypeCode))
                                {
                                    int claimExplanationCode;
                                    int.TryParse(claimsRelatedEntsReasonsExpItem.ClaimExplanationTypeCode, out claimExplanationCode);
                                    CLAIM_MSG1_ClaimRequestClaimDetailClaimReasonClaimReasonListExplanationList claimRequestClaimDetailClaimReasonClaimReasonListExplanation = new CLAIM_MSG1_ClaimRequestClaimDetailClaimReasonClaimReasonListExplanationList();
                                    claimRequestClaimDetailClaimReasonClaimReasonListExplanation.claimExplanationCode = claimExplanationCode;
                                    claimRequestClaimDetailClaimReasonClaimReasonListExplanation.explanationNote = claimsRelatedEntsReasonsExpItem.ExplanationNote;
                                    claimRequestClaimDetailClaimReasonClaimReasonListExplanationList.Add(claimRequestClaimDetailClaimReasonClaimReasonListExplanation);
                                }
                            }
                            claimRequestClaimDetailClaimReasonClaimReasonList.ExplanationList = claimRequestClaimDetailClaimReasonClaimReasonListExplanationList.ToArray();
                        }
                        claimReasonList.Add(claimRequestClaimDetailClaimReasonClaimReasonList);
                    }
                }
                claimRequestClaimDetailClaimReason.ClaimReasonList = claimReasonList.ToArray();
            }
            return claimRequestClaimDetailClaimReason;
        }

        private CLAIM_MSG1_ClaimRequestClaimSubmiter GetClaimSubmiter()
        {
            CLAIM_MSG1_ClaimRequestClaimSubmiter claimRequestClaimSubmiter = new CLAIM_MSG1_ClaimRequestClaimSubmiter();

            if (!string.IsNullOrWhiteSpace(_ClaimPM.ClaimSubmiterNumber))
            {
                int claimSubmiterID = 0;
                int.TryParse(_ClaimPM.ClaimSubmiterNumber, out claimSubmiterID);
                claimRequestClaimSubmiter.claimSubmiterID = claimSubmiterID;
            }

            if (!string.IsNullOrWhiteSpace(_ClaimPM.ClaimSubmiterTypeCode))
            {
                int claimSubmiterTypeCode = 0;
                int.TryParse(_ClaimPM.ClaimSubmiterTypeCode, out claimSubmiterTypeCode);
                claimRequestClaimSubmiter.claimSubmiterTypeCode = claimSubmiterTypeCode;
            }

            claimRequestClaimSubmiter.hebrewCorporationName = _ClaimPM.HebrewCorporationName;

            if (!string.IsNullOrWhiteSpace(_ClaimPM.AddressCode))
            {
                int addressId = 0;
                int.TryParse(_ClaimPM.AddressCode, out addressId);
                claimRequestClaimSubmiter.addressId = addressId;
            }

            return claimRequestClaimSubmiter;
        }

        private CLAIM_MSG1_ClaimRequestImporterContactDetails GetImporterContactDetails()
        {
            CLAIM_MSG1_ClaimRequestImporterContactDetails claimRequestImporterContactDetails = new CLAIM_MSG1_ClaimRequestImporterContactDetails();

            if (!string.IsNullOrWhiteSpace(_ClaimPM.CustomsAddressCode))
            {
                int localAddressID = 0;
                int.TryParse(_ClaimPM.CustomsAddressCode, out localAddressID);
                claimRequestImporterContactDetails.localAddressID = localAddressID;
            }

            if (!string.IsNullOrWhiteSpace(_ClaimPM.ContactPhoneAddressCode))
            {
                int contactPhoneAddressId = 0;
                int.TryParse(_ClaimPM.ContactPhoneAddressCode, out contactPhoneAddressId);
                claimRequestImporterContactDetails.contactPhoneAddressId = contactPhoneAddressId;
            }

            return claimRequestImporterContactDetails;
        }

        private CLAIM_MSG1_ClaimRequestClaimRequest GetClaimRequest()
        {
            CLAIM_MSG1_ClaimRequestClaimRequest claimRequest = new CLAIM_MSG1_ClaimRequestClaimRequest();
            claimRequest.messageSourceCode = 6;
            if (!string.IsNullOrWhiteSpace(_ClaimPM.ImporterClaimTypeCode))
            {
                int importerClaimTypeCode = 0;
                int.TryParse(_ClaimPM.ImporterClaimTypeCode, out importerClaimTypeCode);
                claimRequest.importerClaimType = importerClaimTypeCode;
            }

            if (!string.IsNullOrWhiteSpace(_ClaimPM.SoldierPersonalNumber))
            {
                int soldiered = 0;
                int.TryParse(_ClaimPM.SoldierPersonalNumber, out soldiered);
                claimRequest.soldiered = soldiered;
                claimRequest.soldieredSpecified = true;
            }
            if (_ClaimPM.SubmitDate != null)
            {
                claimRequest.submitDate = (DateTime)_ClaimPM.SubmitDate;
            }

            claimRequest.CustomerIdentification = new CustomerIdentification();
            if (_ClaimPM.ClientId != null)
            {
                string clientCode = GetClientCode(_ClaimPM.ClientId);
                int code;
                if (!string.IsNullOrWhiteSpace(clientCode))
                {
                    int.TryParse(clientCode, out code);
                    claimRequest.CustomerIdentification.externalID = code;
                    claimRequest.CustomerIdentification.externalIDSpecified = true;
                }
            }
            else
            {
                claimRequest.CustomerIdentification.passportCountry = _ClaimPM.PassportCountryTypeCode;
                claimRequest.CustomerIdentification.passportNumber = _ClaimPM.PassportNumber;
                if (!string.IsNullOrWhiteSpace(_ClaimPM.PassportTypeCode))
                {
                    int passportType;
                    int.TryParse(_ClaimPM.PassportTypeCode, out passportType);
                    claimRequest.CustomerIdentification.passportType = passportType;
                    claimRequest.CustomerIdentification.passportTypeSpecified = true;
                }
            }

            return claimRequest;
        }

        private string GetClientCode(string clientId)
        {
            var queryService = new ClientQueryService(_DbContext);
            var importerPM = queryService.GetSingle(clientId, true, false);
            if (importerPM == null) return "";
            return importerPM.Code;

        }

        private Attachment[] GetClaimAttachment()
        {
            var claimAttachmentList = new List<Attachment>();
            var customsDocumentQueryService = new CustomsDocumentQueryService(_DbContext);

            //Get Claims Attachments
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = this._ClaimPM.Id, ParentEntityCode = "Claim" }, this._ClaimPM.Tenant);
            foreach (CustomsDocumentPM customsDocumentPM in customsDocumentPMList)
            {
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                {
                    var claimAttachment = new Attachment();
                    claimAttachment.documentType = customsDocumentPM.DocumentTypeCode;
                    claimAttachment.fileName = customsDocumentPM.Name;
                    claimAttachment.externalAttachmentID = customsDocumentPM.ExternalAttachmentId;
                    claimAttachment.IsAttachment = false.ToString();
                    //claimAttachment.Remark = customsDocumentPM.DocumentRemarks;
                    //claimAttachment.content = customsDocumentPM.

                    claimAttachmentList.Add(claimAttachment);
                }
            }

            //Get ClaimsRelatedEntity Attachments
            foreach (ClaimsRelatedEntityPM claimsRelatedEntityPM in _ClaimPM.ClaimsRelatedEntities)
            {
                var relatedEntityCustomsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = claimsRelatedEntityPM.ClaimId, ParentEntityCode = "Claim", Child1EntityCode = "ClaimRelatedEntity", Child1EntityId = claimsRelatedEntityPM.EntityCounterKey.ToString() }, this._ClaimPM.Tenant);
                foreach (CustomsDocumentPM customsDocumentPM in relatedEntityCustomsDocumentPMList)
                {
                    if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                    {
                        var claimAttachment = new Attachment();
                        claimAttachment.documentType = customsDocumentPM.DocumentTypeCode;
                        claimAttachment.fileName = customsDocumentPM.Name;
                        claimAttachment.externalAttachmentID = customsDocumentPM.ExternalAttachmentId;
                        claimAttachment.IsAttachment = false.ToString();
                        claimAttachmentList.Add(claimAttachment);
                    }
                }
            }

            return claimAttachmentList.ToArray();
        }

        private string GetClaimAttachmentID(ClaimsRelatedEntityPM claimsRelatedEntityPM)
        {
            var claimAttachmentList = new List<Attachment>();
            var customsDocumentQueryService = new CustomsDocumentQueryService(_DbContext);

            //Get Claims Attachments


            //Get ClaimsRelatedEntity Attachments

            CustomsDocumentPM customsDocumentPM = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = claimsRelatedEntityPM.ClaimId, ParentEntityCode = "Claim", Child1EntityCode = "ClaimsRelatedEntity",Child1EntityCode2 = "ClaimRelatedEntityCancelOrObjection", Child1EntityId = claimsRelatedEntityPM.EntityCounterKey.ToString() }, this._ClaimPM.Tenant).FirstOrDefault();
                
            if (!string.IsNullOrWhiteSpace(customsDocumentPM?.CustomsDocId))
            {
                        
                return customsDocumentPM.ExternalAttachmentId;
                   
            }
            

            return null;
        }

        public static byte[] stringToBase64ByteArray(String input)
        {
            byte[] ret = System.Text.Encoding.Unicode.GetBytes(input);
            string s = Convert.ToBase64String(ret);
            ret = System.Text.Encoding.Unicode.GetBytes(s);
            return ret;
        }

        private Attachment[] GetClaimAttachmentTest()
        {
            byte[] myContent = stringToBase64ByteArray("Mirit test !!!!GetApproveChangeTimeListXML()");
            var requestMessage = new Attachment();
            var Attachments = new Attachment[] {
                new Attachment()
            {

                documentType = "380",
                //"לא התקבלו כל שדות המטה-דטא חובה הבאים: : 3,39,55,87 עבור סוג מסמך : 380"
                //"צרופה לא תקינה סוג המסמך : <NULL> שם :  נתוני שדה נוסף : 3 שגויים - הערך : IL אינו מסוג : Int"
                AdditionalData =new AttachmentAdditionalData[] 
                { 
                    new  AttachmentAdditionalData (){fieldID = 3,fieldData="US"  } ,//ארץ חשבון 
                    new  AttachmentAdditionalData (){fieldID = 39,fieldData="5520"} ,///מספר חשבון
                    new  AttachmentAdditionalData (){fieldID = 55,fieldData=DataTypeConvertorUtil .Convert(DateTime.Now)  },//תאריך החשבון
                    new  AttachmentAdditionalData (){fieldID = 87,fieldData=false.ToString()  } ,//האם מסמך מקורי
                },
                fileName = "testdd99000.txt",

               // attachmentID = "USIGN-1",
                externalAttachmentID = "EMTYC-99000",
                Remark = "Claim Remark ",
                content = myContent,
                IsAttachment = "true",
            } };
            return Attachments;
        }

        public override void PostGetRequest(CLAIM_MSG1_ClaimRequest customRequest, CLAIM_2340_ClaimRequestRequestParams requestParams)
        {
            //Create Event SND - “Sent To Customs” 
            var myUpdateEventContextTagModel = new EventContextTagModel()
            {
                CallProccessID = EventContextTagModel.ProccessEnum.SentClaimToCustoms,
                EventCode = "SNDC",
                EventRemarks = "Send Claim To Customs",
                FUStatusRemarks = "Send Claim To Customs",
            };
            _ClaimPM.CurrentContextTag = myUpdateEventContextTagModel;

            Logitude.Customs.BL.EntityUpdateServices.ClaimUpdateService.RaiseClaimEventAndStatus("SNDC", "SNDC", _ClaimPM, null, true);
        }
    }
}
