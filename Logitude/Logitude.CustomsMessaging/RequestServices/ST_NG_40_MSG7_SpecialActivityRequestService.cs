                                                        //Yuval Chalup 25.06.2015 TASK-8907
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CreditQueryServiceReference;
using UnifreightIIG.Common.SpecialActivityRequestMessageServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class ST_NG_40_MSG7_SpecialActivityRequestService
        : RequestServiceBase<ST_NG_40_MSG7_SpecialActivityRequestMessage, SpecialActivityRequestParams>
    {
        public override ST_NG_40_MSG7_SpecialActivityRequestMessage GetRequest(SpecialActivityRequestParams requestParams)
        {
            var myST_NG_40_MSG7_SpecialActivityRequestMessage = new ST_NG_40_MSG7_SpecialActivityRequestMessage();

            // General 
            if (requestParams.GeneralDetailsData != null)
            {
                myST_NG_40_MSG7_SpecialActivityRequestMessage.General = GetGeneralDetails(requestParams);
            }

            // GoodsDetail
            if (requestParams.GoodsDetailsData != null)
            {
                myST_NG_40_MSG7_SpecialActivityRequestMessage.GoodsDetailRequest = GetGeneralGoodsDetails(requestParams);
            }

            // SampleRequest              
            List<ST_NG_40_MSG7_SpecialActivityRequestMessageSampleRequestDetail> mySampleRequestDetailList = new List<ST_NG_40_MSG7_SpecialActivityRequestMessageSampleRequestDetail>();
            if (requestParams.SampleRequestDetailsDataList != null)
            {
                myST_NG_40_MSG7_SpecialActivityRequestMessage.SampleRequestDetail = GetSampleRequestDetails(requestParams);
            }

            // StorageRePackingApproval              
            if (requestParams.RePackingApprovalDetailsData != null)
            {
                myST_NG_40_MSG7_SpecialActivityRequestMessage.StorageRePackingApproval = GetStorageRePackingApproval(requestParams);
            }


            // PresentPacking         
            if (requestParams.CurrentPackingDetailsDataList != null)
            {
                myST_NG_40_MSG7_SpecialActivityRequestMessage.presentPackingDetails = GetPresentPackingList(requestParams);              
            }

            // DesiredPacking        
            if (requestParams.DesiredPackingDetailsDataList != null)
            {
                myST_NG_40_MSG7_SpecialActivityRequestMessage.desiredPackingDetails = GetdesiredPackingDetailsList(requestParams);  
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "בקשה לפעולות מיוחדות";
            this.MyRequestSheetParam.CustomFileNo = requestParams.GeneralDetailsData.CustomFileNo;

            return myST_NG_40_MSG7_SpecialActivityRequestMessage;

        }

        //////////////////////////////          General         //////////////////////////////
        private ST_NG_40_MSG7_SpecialActivityRequestMessageGeneral GetGeneralDetails(SpecialActivityRequestParams requestParams)
        {
            int cargoRowNumber = 0;
            int.TryParse(requestParams.GeneralDetailsData.CargoRowNumber,out cargoRowNumber);
            ST_NG_40_MSG7_SpecialActivityRequestMessageGeneral myGeneral = new ST_NG_40_MSG7_SpecialActivityRequestMessageGeneral()
            {
                applicantAgentNumber = requestParams.GeneralDetailsData.ApplicantAgentNumber,
                cargoRowNumber = cargoRowNumber,
                CheckSite = requestParams.GeneralDetailsData.CheckSite,
                ClientFullName = requestParams.GeneralDetailsData.ClientFullName,
                ContainerNumber = requestParams.GeneralDetailsData.ContainerNumber,
                ImporterName = requestParams.GeneralDetailsData.ImporterName,
                //ImporterNumber = requestParams.GeneralDetailsData.ImporterNumber,
                IsContainer = requestParams.GeneralDetailsData.IsContainer,
                SealNumber = requestParams.GeneralDetailsData.SealNumber,
                siteNumber = requestParams.GeneralDetailsData.SiteNumber,
                specialActivityRequestNumber = requestParams.GeneralDetailsData.SpecialActivityRequestNumber,
                specialActivityType = requestParams.GeneralDetailsData.SpecialActivityType,
            };
            DateTime dateTime;
            int intValue;
            if (DateTime.TryParse(requestParams.GeneralDetailsData.ActivityRequestEndDate.ToString(), out dateTime))
            {
                myGeneral.activityRequestEndDate = dateTime;
            }
            if (DateTime.TryParse(requestParams.GeneralDetailsData.ActivityRequestStartDate.ToString(), out dateTime))
            {
                myGeneral.activityRequestStartDate = dateTime;
            }
            if (int.TryParse(requestParams.GeneralDetailsData.WarehouseBlockNumber.ToString(), out intValue))
            {
                myGeneral.warehouseBlockNumber = intValue;
            }
            if (int.TryParse(requestParams.GeneralDetailsData.ImporterNumber, out intValue))
            {
                myGeneral.ImporterNumber = intValue;
                myGeneral.ImporterNumberSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(requestParams.GeneralDetailsData.AuthorityCode) && int.TryParse(requestParams.GeneralDetailsData.AuthorityCode, out intValue))
            {
                myGeneral.authorityCode = intValue;
                myGeneral.authorityCodeSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(requestParams.GeneralDetailsData.SpecialActivityTypeEssence) && int.TryParse(requestParams.GeneralDetailsData.SpecialActivityTypeEssence, out intValue))
            {
                myGeneral.specialActivityTypeEssence = intValue;
                myGeneral.specialActivityTypeEssenceSpecified = true;
            }
            //myGeneral.authorityCodeSpecified = requestParams.GeneralDetailsData.AuthorityCode == null ? false : true;
            myGeneral.cargoRowNumberSpecified = cargoRowNumber == 0 ? false : true;
            //myGeneral.ImporterNumberSpecified = requestParams.GeneralDetailsData.ImporterNumber == null ? false : true;
            myGeneral.IsContainerSpecified = requestParams.GeneralDetailsData.IsContainer == null ? false : true;

            if (requestParams.GeneralDetailsData.CargoIdentifier != null)
            {
                myGeneral.cargoIdentifier = new cargoIdentifier()
                {
                    cargoIdentifierKey1 = requestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1,
                    cargoIdentifierKey2 = requestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2,
                    cargoIdentifierKey3 = requestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3,
                    cargoIdentifierType = requestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierType,
                };
            }

            // OtherActivity        
            if (requestParams.OtherActivityDetailsData != null)
            {
                myGeneral.OtherComment = requestParams.OtherActivityDetailsData.OtherActivityComment;
            }

            return myGeneral;
        }

        ///////////////////////                 GoodsDetail         //////////////////////////////
        private ST_NG_40_MSG7_SpecialActivityRequestMessageGoodsDetailRequest GetGeneralGoodsDetails(SpecialActivityRequestParams requestParams)
        {
            if (string.IsNullOrWhiteSpace(requestParams.GoodsDetailsData.GoodsDescription) && string.IsNullOrWhiteSpace(requestParams.GoodsDetailsData.OtherDescription) && requestParams.GoodsDetailsData.IdemanderTypeSpecified == false && requestParams.GoodsDetailsData.SpecialActionsCodeSpecified == false)
            {
                return new ST_NG_40_MSG7_SpecialActivityRequestMessageGoodsDetailRequest();
            }
            ST_NG_40_MSG7_SpecialActivityRequestMessageGoodsDetailRequest myGoodsDetailRequest = new ST_NG_40_MSG7_SpecialActivityRequestMessageGoodsDetailRequest()
            {
                goodsDescription = requestParams.GoodsDetailsData.GoodsDescription,
                idemanderType = requestParams.GoodsDetailsData.IdemanderType,
                otherDescription = requestParams.GoodsDetailsData.OtherDescription,
                specialActionsCode = requestParams.GoodsDetailsData.SpecialActionsCode,
            };
            myGoodsDetailRequest.idemanderTypeSpecified = requestParams.GoodsDetailsData.IdemanderType == null ? false : true;
            myGoodsDetailRequest.specialActionsCodeSpecified = requestParams.GoodsDetailsData.SpecialActionsCode == null ? false : true;

            if (requestParams.GoodsDetailsData.RepresentativeList != null)
            {
                int representativeIDInt;
                List<ST_NG_40_MSG7_SpecialActivityRequestMessageGoodsDetailRequestGoodsDetailRequestRepresentative> myGoodsDetailRequestRepresentativeList = new List<ST_NG_40_MSG7_SpecialActivityRequestMessageGoodsDetailRequestGoodsDetailRequestRepresentative>();
                foreach (var representative in requestParams.GoodsDetailsData.RepresentativeList)
                {
                    ST_NG_40_MSG7_SpecialActivityRequestMessageGoodsDetailRequestGoodsDetailRequestRepresentative myGoodsDetailRequestRepresentative = new ST_NG_40_MSG7_SpecialActivityRequestMessageGoodsDetailRequestGoodsDetailRequestRepresentative()
                    {
                        representativeName = representative.RepresentativeName,
                        representativeNumber = representative.RepresentativeNumber,
                        representativeNumberSpecified = true
                    };
                    if (int.TryParse(representative.RepresentativeID, out representativeIDInt))
                    {
                        myGoodsDetailRequestRepresentative.representativeID = representativeIDInt;
                    }
                    myGoodsDetailRequestRepresentative.representativeIDSpecified = representative.RepresentativeID == null ? false : true;
                    myGoodsDetailRequestRepresentative.representativeNumberSpecified = representative.RepresentativeNumber == null ? false : true;

                    myGoodsDetailRequestRepresentativeList.Add(myGoodsDetailRequestRepresentative);
                }
                myGoodsDetailRequest.GoodsDetailRequestRepresentative = myGoodsDetailRequestRepresentativeList.ToArray();
            }
            return myGoodsDetailRequest;
        }

        ///////////////////////             SampleRequest              ///////////////////////
        private ST_NG_40_MSG7_SpecialActivityRequestMessageSampleRequestDetail[] GetSampleRequestDetails(SpecialActivityRequestParams requestParams)
        {
            List<ST_NG_40_MSG7_SpecialActivityRequestMessageSampleRequestDetail> mySampleRequestDetailList = new List<ST_NG_40_MSG7_SpecialActivityRequestMessageSampleRequestDetail>();

            int intValue;
            foreach (var sampleRequestData in requestParams.SampleRequestDetailsDataList)
            {
                ST_NG_40_MSG7_SpecialActivityRequestMessageSampleRequestDetail mySampleRequestDetail = new ST_NG_40_MSG7_SpecialActivityRequestMessageSampleRequestDetail()
                {
                    currencyTypeCode = sampleRequestData.CurrencyTypeCode,
                    customsItem = sampleRequestData.CustomsItem,
                    customsItemQuantity = sampleRequestData.CustomsItemQuantity,
                    sampleDescription = sampleRequestData.SampleDescription,
                    sampleRowNumber = sampleRequestData.SampleRowNumber,
                };
                if (mySampleRequestDetail.customsItem != null && mySampleRequestDetail.customsItem.Length == 11)
                {
                    mySampleRequestDetail.customsItem = mySampleRequestDetail.customsItem.Insert(10, "/");
                }
                DateTime dateTime;
                if (DateTime.TryParse(sampleRequestData.SampleReturnDate.ToString(), out dateTime))
                {
                    mySampleRequestDetail.sampleReturnDate = dateTime;
                }
                if (int.TryParse(sampleRequestData.SampleValue.ToString(), out intValue))
                {
                    mySampleRequestDetail.sampleValue = intValue;
                }
                mySampleRequestDetail.customsItemQuantitySpecified = sampleRequestData.CustomsItemQuantity == null ? false : true;
                mySampleRequestDetail.sampleRowNumberSpecified = sampleRequestData.SampleRowNumber == null ? false : true;

                if (sampleRequestData.SamplePackingDetails != null)
                {
                    mySampleRequestDetail.PackingDetails = new ST_NG_40_MSG7_SpecialActivityRequestMessageSampleRequestDetailPackingDetails()
                    {
                        packageType = sampleRequestData.SamplePackingDetails.PackageType,
                        quantity = sampleRequestData.SamplePackingDetails.Quantity,
                        //sampleValue = sampleRequestData.SamplePackingDetails.SampleValue,
                        weight = sampleRequestData.SamplePackingDetails.Weight,
                    };
                    if (!string.IsNullOrWhiteSpace(sampleRequestData.SamplePackingDetails.PackageId))
                    {
                        mySampleRequestDetail.PackingDetails.packageId = sampleRequestData.SamplePackingDetails.PackageId.Substring(0, Math.Min(sampleRequestData.SamplePackingDetails.PackageId.Length, 32));
                    }

                    mySampleRequestDetail.PackingDetails.weightSpecified = sampleRequestData.SamplePackingDetails.Weight == null ? false : true;
                }
                mySampleRequestDetailList.Add(mySampleRequestDetail);
            }

            return mySampleRequestDetailList.ToArray();
        }

        ///////////////////////             StorageRePackingApproval              ///////////////////////
        private ST_NG_40_MSG7_SpecialActivityRequestMessageStorageRePackingApproval GetStorageRePackingApproval(SpecialActivityRequestParams requestParams)
        {
            ST_NG_40_MSG7_SpecialActivityRequestMessageStorageRePackingApproval myStorageRePackingApproval = new ST_NG_40_MSG7_SpecialActivityRequestMessageStorageRePackingApproval()
            {
                approvalDate = requestParams.RePackingApprovalDetailsData.ApprovalDate,
                approvalName = requestParams.RePackingApprovalDetailsData.ApprovalName,
                siteNumber = requestParams.RePackingApprovalDetailsData.SiteNumber,
            };
            myStorageRePackingApproval.approvalDateSpecified = requestParams.RePackingApprovalDetailsData.ApprovalDate == null ? false : true;

            return myStorageRePackingApproval;
        }

        private ST_NG_40_MSG7_SpecialActivityRequestMessagePresentPackingDetails[] GetPresentPackingList(SpecialActivityRequestParams requestParams)
        {
            List<ST_NG_40_MSG7_SpecialActivityRequestMessagePresentPackingDetails> myPresentPackingList = new List<ST_NG_40_MSG7_SpecialActivityRequestMessagePresentPackingDetails>();
            foreach (var currentPackingDetailsData in requestParams.CurrentPackingDetailsDataList)
            {
                ST_NG_40_MSG7_SpecialActivityRequestMessagePresentPackingDetails myPresentPacking = new ST_NG_40_MSG7_SpecialActivityRequestMessagePresentPackingDetails()
                {
                    presentPackingStateContent = currentPackingDetailsData.PresentPackingStateContent,
                    rePackingOldLineNumber = currentPackingDetailsData.RePackingOldLineNumber,
                };
                myPresentPacking.rePackingOldLineNumberSpecified = currentPackingDetailsData.RePackingOldLineNumber == null ? false : true;

                if (currentPackingDetailsData.PackingDetails != null)
                {
                    myPresentPacking.packingDetails = new ST_NG_40_MSG7_SpecialActivityRequestMessagePresentPackingDetailsPackingDetails()
                    {
                        packageType = currentPackingDetailsData.PackingDetails.PackageType,
                        quantity = currentPackingDetailsData.PackingDetails.Quantity,
                        weight = currentPackingDetailsData.PackingDetails.Weight,
                    };
                    if (!string.IsNullOrWhiteSpace(currentPackingDetailsData.PackingDetails.PackageId))
                    {
                        myPresentPacking.packingDetails.packageId = currentPackingDetailsData.PackingDetails.PackageId.Substring(0, Math.Min(currentPackingDetailsData.PackingDetails.PackageId.Length, 32));
                    }
                    myPresentPacking.packingDetails.weightSpecified = currentPackingDetailsData.PackingDetails.Weight == null ? false : true;
                }
                myPresentPackingList.Add(myPresentPacking);
            }

            return myPresentPackingList.ToArray();
        }

        private ST_NG_40_MSG7_SpecialActivityRequestMessageDesiredPackingDetails[] GetdesiredPackingDetailsList(SpecialActivityRequestParams requestParams)
        {
            List<ST_NG_40_MSG7_SpecialActivityRequestMessageDesiredPackingDetails> mydesiredPackingDetailsList = new List<ST_NG_40_MSG7_SpecialActivityRequestMessageDesiredPackingDetails>();
            foreach (var desiredPackingDetailsData in requestParams.DesiredPackingDetailsDataList)
            {
                ST_NG_40_MSG7_SpecialActivityRequestMessageDesiredPackingDetails mydesiredPackingDetails = new ST_NG_40_MSG7_SpecialActivityRequestMessageDesiredPackingDetails()
                {
                    rePackingNewLineNumber = desiredPackingDetailsData.RePackingNewLineNumber,
                    rePackingNewLineNumberSpecified = true,
                    rePackingOldLineNumber = desiredPackingDetailsData.RePackingOldLineNumber,
                    rePackingOldLineNumberSpecified = true,
                };
                mydesiredPackingDetails.rePackingNewLineNumberSpecified = desiredPackingDetailsData.RePackingNewLineNumber == null ? false : true;
                mydesiredPackingDetails.rePackingOldLineNumberSpecified = desiredPackingDetailsData.RePackingOldLineNumber == null ? false : true;

                if (desiredPackingDetailsData.PackingDetails != null)
                {
                    mydesiredPackingDetails.PackingDetails = new PackingDetails()
                    {
                        //packageId = desiredPackingDetailsData.PackingDetails.PackageId,
                        packageType = desiredPackingDetailsData.PackingDetails.PackageType,
                        quantity = desiredPackingDetailsData.PackingDetails.Quantity,
                        weight = desiredPackingDetailsData.PackingDetails.Weight,
                    };
                    if (!string.IsNullOrWhiteSpace(desiredPackingDetailsData.PackingDetails.PackageId))
                    {
                        mydesiredPackingDetails.PackingDetails.packageId = desiredPackingDetailsData.PackingDetails.PackageId.Substring(0, Math.Min(desiredPackingDetailsData.PackingDetails.PackageId.Length, 32));
                    }
                    mydesiredPackingDetails.PackingDetails.weightSpecified = desiredPackingDetailsData.PackingDetails.Weight == null ? false : true;
                }
                mydesiredPackingDetailsList.Add(mydesiredPackingDetails);
            }
            return mydesiredPackingDetailsList.ToArray();
        }
    }
}