import {AppTool} from '../Tools';
import {SessionLocator} from '../Utilities/SessionLocator';
import {ChargesTypeList} from '../../Common/EntityLists/ChargesTypeList';
import {VatTypeList} from '../../Common/EntityLists/VatTypeList';
import {VatTypeListService} from '../../Common/Services/StandardLists/VatTypeListService';
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPayablePM} from '../../Shipment/EntityPMs/ShipmentPayablePM';
import {ShipmentReceivablePM} from '../../Shipment/EntityPMs/ShipmentReceivablePM';

export class VatTypesValidator {
    public static GetVatTypeDependency1() {
        var myResult: boolean = false;

        if (SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
            myResult = null;
        }

        return myResult;
    }
    public static ValidateMultiPercentages(vatTypesIds: string[] = []) {
        var myResult: boolean = true;

        if (!SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
            if (vatTypesIds.length > 0) {

                var myService: VatTypeListService = new VatTypeListService();
                myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var allVatTypes: VatTypeList[] = myResponse.Result;

                        vatTypesIds.forEach(id => {
                            var item = allVatTypes.filter(f => f.Id == id)[0];
                            if (item) {
                                if (item.IsMultiPercentage) {
                                    myResult = false;
                                }
                            }
                        });
                    }
                });
            }
        }

        return myResult;
    }
    public static FilterChargesByVATs(allChargesTypes: ChargesTypeList[]) {
        var myResult: ChargesTypeList[] = [];

        if (SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
            myResult = allChargesTypes;
        }

        else {

            var myService: VatTypeListService = new VatTypeListService();
            myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var allVatTypes: VatTypeList[] = myResponse.Result;

                    allChargesTypes.forEach(item => {
                        if (item.VatTypeId == null) {
                            myResult.push(item);
                        }

                        else {
                            var vatType = allVatTypes.filter(f => f.Id == item.VatTypeId)[0];
                            if (vatType) {
                                if (vatType.IsMultiPercentage == false) {
                                    myResult.push(item);
                                }
                            }
                        }
                    });
                }
            });
        }

        return myResult;
    }
    public static GetAllVatTypes() {

        var myResult: VatTypeList[] = [];

        var myService: VatTypeListService = new VatTypeListService();
        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                myResult = myResponse.Result;
            }
        });

        return myResult;
    }
    public static IsVatAllowed(vatTypeId: string, allVatTypes: VatTypeList[]) {
        var myResult = true;

        if (!AppTool.IsNullOrEmpty(vatTypeId)) {
            if (!SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                var vatType = allVatTypes.filter(f => f.Id == vatTypeId)[0];
                if (vatType) {
                    if (vatType.IsMultiPercentage) {
                        myResult = false;
                    }
                }
            }
        }

        return myResult;
    }
    public static GetError() {
        return "Your accounting settings doesn't enable Multi-percentage VATs";
    }
}