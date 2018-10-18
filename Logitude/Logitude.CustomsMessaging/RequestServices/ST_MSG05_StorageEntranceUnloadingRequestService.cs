using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.StorageEntranceUnloadingServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class ST_MSG05_StorageEntranceUnloadingRequestService : RequestServiceBase<ST_NG_20_MSG5_StorageEntranceUnloading, StorageEntranceUnloadingRequestParams>
    {
        private ICustomContext _Context;
        private DeclarationPM _DeclarationPM;

        public override ST_NG_20_MSG5_StorageEntranceUnloading GetRequest(StorageEntranceUnloadingRequestParams requestParams)
        {
            var myST_NG_20_MSG5_StorageEntranceUnloading = new ST_NG_20_MSG5_StorageEntranceUnloading();
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading = new ST_NG_20_MSG5_StorageEntranceUnloadingStorageEntranceUnloading();

            SetDeclarationPM(requestParams);

            //StorageEntranceUnloading
            int consignmentNumber = 0;
            int.TryParse(requestParams.ConsignmentNumber, out consignmentNumber);
            ConsignmentPM consignmentPM = FindConsignmentInList(consignmentNumber);
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading = new ST_NG_20_MSG5_StorageEntranceUnloadingStorageEntranceUnloading();
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.functionCode = 1;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.siteNumber = consignmentPM.StorageSiteCode;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.cargoIdentifier = new cargoIdentifier();
            int cargoTypeCode = 0;
            int.TryParse(consignmentPM.CargoTypeCode, out cargoTypeCode);
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.cargoIdentifier.cargoIdentifierType = cargoTypeCode;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.cargoIdentifier.cargoIdentifierKey1 = consignmentPM.ManifestNumber;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.cargoIdentifier.cargoIdentifierKey2 = consignmentPM.SecondCargoID;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.cargoIdentifier.cargoIdentifierKey3 = consignmentPM.ThirdCargoID;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.ManifestNumber = consignmentPM.ManifestNumber;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.cargoRowNumber = 1;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.cargoRowNumberSpecified = true;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.loadUnloadCloseDate = requestParams.EntryDate;
            myST_NG_20_MSG5_StorageEntranceUnloading.StorageEntranceUnloading.acceptanceAppearanceNumber = 1; // LineNumber!!!


            //ConsignmentItem
            List<ST_NG_20_MSG5_StorageEntranceUnloadingConsignmentItem> consignmentItemList = new List<ST_NG_20_MSG5_StorageEntranceUnloadingConsignmentItem>();
            ST_NG_20_MSG5_StorageEntranceUnloadingConsignmentItem consignmentItem = new ST_NG_20_MSG5_StorageEntranceUnloadingConsignmentItem();
            consignmentItem.PackingDetails = new PackingDetails();
            consignmentItem.PackingDetails.packageType = requestParams.PackageTypeCode;
            consignmentItem.PackingDetails.quantity = requestParams.Quantity;
            if (!string.IsNullOrEmpty(requestParams.GrossWeight))
            {
                decimal weight = 0;
                decimal.TryParse(requestParams.GrossWeight, out weight);
                consignmentItem.PackingDetails.weight = weight;
                consignmentItem.PackingDetails.weightSpecified = true;
            }
            consignmentItemList.Add(consignmentItem);
            myST_NG_20_MSG5_StorageEntranceUnloading.ConsignmentItem = consignmentItemList.ToArray();

            //AccumulatedPackingFromStorageSite
            List<PackingDetails> packingDetailsList = new List<PackingDetails>();
            PackingDetails packingDetailsOtem = new PackingDetails();
            decimal grossWeight = 0;
            int quantity = 0;
            packingDetailsOtem.packageType = requestParams.PackageTypeCode;
            if (_DeclarationPM.DeclarationConsAcceptances != null && _DeclarationPM.DeclarationConsAcceptances.Count > 0)
            {
                foreach (DeclarationConsAcceptancePM consAcceptancesItem in _DeclarationPM.DeclarationConsAcceptances)
                {
                    if(consAcceptancesItem.PackageTypeCode == requestParams.PackageTypeCode
                        && consAcceptancesItem.ConsignmentNumber == consignmentNumber)
                    {
                        if (consAcceptancesItem.GrossWeight != null)
                        {
                            grossWeight += (decimal)consAcceptancesItem.GrossWeight;
                        }
                        if (consAcceptancesItem.Quantity != null)
                        {
                            quantity += (int)consAcceptancesItem.Quantity;
                        }
                    }
                }
            }
            packingDetailsOtem.weight = grossWeight;
            packingDetailsOtem.weightSpecified = grossWeight > 0 ? true : false;
            packingDetailsOtem.quantity = quantity.ToString();

            packingDetailsList.Add(packingDetailsOtem);
            myST_NG_20_MSG5_StorageEntranceUnloading.AccumulatedPackingFromStorageSite = packingDetailsList.ToArray();


            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.CustomFileNo = _DeclarationPM.CustomFileNo;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = _DeclarationPM.Id;
            this.MyRequestSheetParam.RequestDescription = "זמינות כניסה למחסן " + _DeclarationPM.DeclarationNumber;

            return myST_NG_20_MSG5_StorageEntranceUnloading;
        }

        private void SetDeclarationPM(StorageEntranceUnloadingRequestParams requestParams)
        {
            if (this._Context == null) this._Context = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(_Context);
            declarationQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
            _DeclarationPM = declarationQueryService.GetSingle(requestParams.DeclarationId, true, false);
        }

        private ConsignmentPM FindConsignmentInList(int ConsignmentNumber)
        {

            if (_DeclarationPM.Consignments.Count == 0)
            {
                return null;
            }

            ConsignmentPM consignmentPM  = (from a in _DeclarationPM.Consignments
                                        where (a.ConsignmentNumber == ConsignmentNumber)
                                        select a).FirstOrDefault();

            if (consignmentPM != null)
            {
                return consignmentPM;
            }
            else
            {
                return null;
            }
        }
    }
}

