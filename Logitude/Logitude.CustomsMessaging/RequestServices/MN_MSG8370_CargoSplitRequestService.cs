using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CargoSplitSaveServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class MN_MSG8370_CargoSplitRequestService
        : RequestServiceBase<MN_MSG8370_CargoSplitRequest_Message, CargoSplitRequestParams>
    {
        private DeclarationCargoSplitPM _DeclarationCargoSplitPM;
        private ICustomContext _DbContext;

        public override MN_MSG8370_CargoSplitRequest_Message GetRequest(CargoSplitRequestParams requestParams)
        {
            this._DbContext = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationCargoSplitQueryService = new DeclarationCargoSplitQueryService(this._DbContext);
            string cargoSplitID = requestParams.LoggingEntityId;
            if (!string.IsNullOrEmpty(requestParams.DeclarationCargoSplit))
            {
                cargoSplitID = requestParams.DeclarationCargoSplit;
            }
            _DeclarationCargoSplitPM = myDeclarationCargoSplitQueryService.GetSingle(cargoSplitID, true, false);
            if (_DeclarationCargoSplitPM == null) return null;
            var myMN_MSG8370_CargoSplitRequest_Message = new MN_MSG8370_CargoSplitRequest_Message();
            int result;
            if (!int.TryParse(_DeclarationCargoSplitPM.ActionTypeCode, out result))
            {
                return null;
            }
            if (!int.TryParse(_DeclarationCargoSplitPM.RequestReason, out result))
            {
                return null;
            }
            
            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.DeclarationCargoSplit");
            this.MyRequestSheetParam.EntityId1 = cargoSplitID;
            this.MyRequestSheetParam.RequestDescription = "בקשת פיצול מטען";
            if (!string.IsNullOrEmpty(_DeclarationCargoSplitPM.DeclarationId))
            {
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId2 = _DeclarationCargoSplitPM.DeclarationId;

                DeclarationPM declaration = new DeclarationPM();
                var declarationQueryService = new DeclarationQueryService(_DeclarationCargoSplitPM.Tenant);
                declaration = declarationQueryService.GetSingle(_DeclarationCargoSplitPM.DeclarationId, false, true);
                if (declaration != null && !string.IsNullOrWhiteSpace(declaration.CustomFileNo))
                {
                    this.MyRequestSheetParam.RequestDescription = "בקשת פיצול מטען תיק " + declaration.CustomFileNo;
                }
                if (_DeclarationCargoSplitPM.ActionTypeCode == "1")// || _DeclarationCargoSplitPM.ActionTypeCode == "2")//Task 43520
                {
                    ICustomContext dbContext = CustomContext.GetContext(_DeclarationCargoSplitPM.Tenant);
                    var DeclarationCargoSplitUpdateService = new DeclarationCargoSplitUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _DeclarationCargoSplitPM.Tenant);
                    DeclarationCargoSplitUpdateService.RaiseDeclarationCargoSplitEventAndStatus("CSR", "CSR", _DeclarationCargoSplitPM, declaration, false, "", "");
                }
            }
            

            if(_DeclarationCargoSplitPM != null)
            {
                myMN_MSG8370_CargoSplitRequest_Message.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
                myMN_MSG8370_CargoSplitRequest_Message.CargoSplitRequestGeneral = GetCargoSplitRequestGeneral();
                myMN_MSG8370_CargoSplitRequest_Message.Consignment = GetConsignment();
                myMN_MSG8370_CargoSplitRequest_Message.SourceCargoIdentifier = GetSourceCargoIdentifier();
            }

            return myMN_MSG8370_CargoSplitRequest_Message;

        }

        private cargoIdentifier GetSourceCargoIdentifier()
        {
            var myCargoIdentifier = new cargoIdentifier();
            if (!string.IsNullOrWhiteSpace(_DeclarationCargoSplitPM.CargoTypeCode))
            {
                int cargoIdentifierType = 0;
                int.TryParse(_DeclarationCargoSplitPM.CargoTypeCode, out cargoIdentifierType);
                myCargoIdentifier.cargoIdentifierType = cargoIdentifierType;
            }
            myCargoIdentifier.cargoIdentifierKey1 = _DeclarationCargoSplitPM.ManifestNumber;
            myCargoIdentifier.cargoIdentifierKey2 = _DeclarationCargoSplitPM.SecondCargoID;
            myCargoIdentifier.cargoIdentifierKey3 = _DeclarationCargoSplitPM.ThirdCargoID;
            return myCargoIdentifier;
        }

        private MN_MSG8370_CargoSplitRequest_MessageConsignment[] GetConsignment()
        {
            var CargoSplitRequest_MessageConsignmentList = new List<MN_MSG8370_CargoSplitRequest_MessageConsignment>();
            foreach (var Consignment in _DeclarationCargoSplitPM.DecCargoSplitCons)
            {
                var CargoSplitRequest_MessageConsignment = new MN_MSG8370_CargoSplitRequest_MessageConsignment();
                CargoSplitRequest_MessageConsignment.ConditionCode = Consignment.ConditionCode;
                CargoSplitRequest_MessageConsignment.GovernmentProcedureCurrentCode = Consignment.ProcedureCurrentCode;
                CargoSplitRequest_MessageConsignment.ImporterNumber = Consignment.ImporterCode;
                CargoSplitRequest_MessageConsignment.ConsignmentItem = GetConsignmentItem(Consignment);
                CargoSplitRequest_MessageConsignmentList.Add(CargoSplitRequest_MessageConsignment);
            }
            return CargoSplitRequest_MessageConsignmentList.ToArray();
        }

        

        private MN_MSG8370_CargoSplitRequest_MessageConsignmentConsignmentItem[] GetConsignmentItem(DecCargoSplitConPM consignment)
        {
            var CargoSplitRequest_MessageConsignmentConsignmentItemList = new List<MN_MSG8370_CargoSplitRequest_MessageConsignmentConsignmentItem>();
            foreach (var ConsignmentItem in consignment.DecCargoSplitConsItems)
            {
                var CargoSplitRequest_MessageConsignmentItem = new MN_MSG8370_CargoSplitRequest_MessageConsignmentConsignmentItem();
                CargoSplitRequest_MessageConsignmentItem.CargoDescription = ConsignmentItem.CargoDescription;
                if (ConsignmentItem.GrossMassMeasure.HasValue)
                {
                    CargoSplitRequest_MessageConsignmentItem.GrossMassMeasureWeight = ConsignmentItem.GrossMassMeasure.Value;
                    CargoSplitRequest_MessageConsignmentItem.GrossMassMeasureWeightSpecified = true;
                }
                if (!string.IsNullOrWhiteSpace(ConsignmentItem.ParentCargoConsinmentItem))
                {
                    int ParentCargoConsinmentItem = 0;
                    int.TryParse(ConsignmentItem.ParentCargoConsinmentItem, out ParentCargoConsinmentItem);
                    CargoSplitRequest_MessageConsignmentItem.ParentCargoConsinmentItem = ParentCargoConsinmentItem;
                }
                if (!string.IsNullOrWhiteSpace(ConsignmentItem.RequestReasonCode))
                {
                    int RequestReason = 0;
                    int.TryParse(ConsignmentItem.RequestReasonCode, out RequestReason);
                    CargoSplitRequest_MessageConsignmentItem.RequestReason = RequestReason;
                }
                CargoSplitRequest_MessageConsignmentItem.PackagingDetails = GetPackagingDetails(ConsignmentItem);
                CargoSplitRequest_MessageConsignmentConsignmentItemList.Add(CargoSplitRequest_MessageConsignmentItem);
            }
            return CargoSplitRequest_MessageConsignmentConsignmentItemList.ToArray();
        }

        private MN_MSG8370_CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDetails[] GetPackagingDetails(DecCargoSplitConsItemPM consignmentItem)
        {
            var CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDetList = new List<MN_MSG8370_CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDetails>();
            foreach (var ConsignmentItemPackagingDet in consignmentItem.DecCargoSplitConsPackDets)
            {
                var CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDet = new MN_MSG8370_CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDetails();
                CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDet.ManifestNumber = ConsignmentItemPackagingDet.ManifestNumber;
                CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDet.MarksNumbers = ConsignmentItemPackagingDet.MarksNumbers;
                CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDet.packageTypeCode = ConsignmentItemPackagingDet.PackageTypeCode;
                if(ConsignmentItemPackagingDet.PackageQuantity.HasValue)CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDet.quantity = ConsignmentItemPackagingDet.PackageQuantity.Value;
                if(ConsignmentItemPackagingDet.GrossMassMeasure.HasValue)
                {
                    long Weight = 0;
                    long.TryParse(ConsignmentItemPackagingDet.GrossMassMeasure.Value.ToString(), out Weight);
                    CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDet.Weight = Weight;
                    CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDet.WeightSpecified = true;
                }

                CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDetList.Add(CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDet);
            }
            return CargoSplitRequest_MessageConsignmentConsignmentItemPackagingDetList.ToArray();
        }

        private MN_MSG8370_CargoSplitRequest_MessageCargoSplitRequestGeneral GetCargoSplitRequestGeneral()
        {
            MN_MSG8370_CargoSplitRequest_MessageCargoSplitRequestGeneral cargoSplitRequestGeneral = new MN_MSG8370_CargoSplitRequest_MessageCargoSplitRequestGeneral();
            if (!string.IsNullOrWhiteSpace(_DeclarationCargoSplitPM.ActionTypeCode))
            {
                int actionTypeCode = 0;
                int.TryParse(_DeclarationCargoSplitPM.ActionTypeCode, out actionTypeCode);
                cargoSplitRequestGeneral.actionTypeCode = actionTypeCode;
            }
            if (_DeclarationCargoSplitPM.RequestDate != null)
            {
                cargoSplitRequestGeneral.RequestDate = _DeclarationCargoSplitPM.RequestDate;
                cargoSplitRequestGeneral.RequestDateSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(_DeclarationCargoSplitPM.RequestNumber))
            {
                int requestNumber = 0;
                int.TryParse(_DeclarationCargoSplitPM.RequestNumber, out requestNumber);
                cargoSplitRequestGeneral.RequestNumber = requestNumber;
                cargoSplitRequestGeneral.RequestNumberSpecified = true;
            }
            if (!string.IsNullOrWhiteSpace(_DeclarationCargoSplitPM.RequestReason))
            {
                int requestReason = 0;
                int.TryParse(_DeclarationCargoSplitPM.RequestReason, out requestReason);
                cargoSplitRequestGeneral.RequestReason = requestReason;
            }
            cargoSplitRequestGeneral.RequestRemarks = _DeclarationCargoSplitPM.RequestRemarks;
            return cargoSplitRequestGeneral;
        }
    }
}