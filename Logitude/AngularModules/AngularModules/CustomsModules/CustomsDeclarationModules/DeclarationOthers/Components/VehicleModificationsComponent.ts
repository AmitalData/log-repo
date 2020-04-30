                                                                   import {Component, Output, EventEmitter}  from '@angular/core';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationVehicleModificationListService, DeclarationVehicleModificationList } from '../../../../Customs/Services/ExtendedLists/DeclarationVehicleModificationListService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { VehicleReductionTypeListService } from '../../../../Customs/Services/StandardLists/VehicleReductionTypeListService';
import { VehicleReductionTypeList } from '../../../../Customs/EntityLists/VehicleReductionTypeList';

@Component({
    selector: 'VehicleModificationsComponent',
    
    templateUrl: './VehicleModificationsComponent.html',
})


export class VehicleModificationsComponent extends BaseComponent {
  public ObjectTableName: any;

    EntityPM: DeclarationPM;
    entityListService: EntityListService = new EntityListService();
    DataContext: any = this;
    DummyList: ObservableCollection = new ObservableCollection([]);
    _DeclarationVehicleModificationListService: DeclarationVehicleModificationListService = new DeclarationVehicleModificationListService();

    ChassisNumber: string ="";
    AdjustmentTypeCode: string = "";
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    Loaded: boolean = false;
    _VehicleReductionTypeList: VehicleReductionTypeList[] = [];
    constructor() {
        super();
        let myVehicleReductionTypeListService = new VehicleReductionTypeListService();
        myVehicleReductionTypeListService.getAllFromCache().
            subscribe((res:any) => {
                this._VehicleReductionTypeList = res.Result;
                this._entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder", 0).subscribe((response:any) => {
                    this.Loaded = true;
                });
            });

        
            


        
        

    }
    //public get HaveAdjustmentTypeCode(): boolean {
    //    if (AppTool.IsNullOrEmpty(this.AdjustmentTypeCode)) {
    //        return true;
    //    }
    //    return false;
    //}
    //_IsFucos: boolean = false;
    //public get ShowWaterMark(): boolean {
    //    if (!AppTool.IsNullOrEmpty(this.AdjustmentTypeCode)) {
    //        return false;
    //    }
    //    if (this._IsFucos) {
    //        return false;
    //    }
    //    return true;
    //}
    //IsFucos(val: boolean) {
    //    console.log("IsFucos", val);
    //    this._IsFucos = val;
    //}
    ChassisNumberTextChanged($event) {
        this.ChassisNumber = $event;
        this.LoadDeclarationVehicleModifications();
    }
    OnLovItemChanged() {
        this.LoadDeclarationVehicleModifications();
    }
    SetWindowArgs(windowArgs) {
        this.EntityPM = windowArgs.EntityPM;
        this.LoadDeclarationVehicleModifications();
    }
    VehicleModiGroupList: VehicleModiGroup[];
    LoadDeclarationVehicleModifications() {
        let test = false;
        if (test) {

            this.VehicleModiGroupList = [];
            let myGroupChassisNumber: VehicleModiGroup = new VehicleModiGroup();
            myGroupChassisNumber.ChassisNumber = "ChassisNumber   wqwa";
            myGroupChassisNumber.Total = 12323;
            myGroupChassisNumber.MyList = [];
            let myDeclarationVehicleModificationList = new DeclarationVehicleModificationList();
            myDeclarationVehicleModificationList.AdjustmentType = "AdjustmentType";
            myDeclarationVehicleModificationList.DeductAmount = 1313131;
            
            myGroupChassisNumber.MyList.push(myDeclarationVehicleModificationList);
            myGroupChassisNumber.MyList.push(myDeclarationVehicleModificationList);
            myGroupChassisNumber.MyList.push(myDeclarationVehicleModificationList);
            this.VehicleModiGroupList.push(myGroupChassisNumber);
            this.VehicleModiGroupList.push(myGroupChassisNumber);



            return;
        }
        let adjustmentTypeCode = this.AdjustmentTypeCode || "";
        let chassisNumber = this.ChassisNumber || "";
        this._DeclarationVehicleModificationListService
            .GetDeclarationVehicleModification(this.EntityPM.Id,
            chassisNumber, adjustmentTypeCode
            , this.EntityPM.Tenant).
            subscribe((res:any) => {
                let aryDeclarationVehicleModificationList: DeclarationVehicleModificationList[];
                aryDeclarationVehicleModificationList = res.Result;
                this.VehicleModiGroupList = [];
                
                aryDeclarationVehicleModificationList.forEach(
                    (itemDb: DeclarationVehicleModificationList) => {
                        let vehicleReductionType = this._VehicleReductionTypeList.filter(typeRec => typeRec.Code == itemDb.AdjustmentType)[0];
                        if (!AppTool.IsNullOrEmpty(vehicleReductionType)) {
                            itemDb.AdjustmentTypeName = vehicleReductionType.LocalName;
                        }
                        
                        let listChassisNumber = this.VehicleModiGroupList.filter(group => itemDb.ChassisNumber == group.ChassisNumber);
                        let myGroupChassisNumber: VehicleModiGroup = null;
                        if (!AppTool.IsNullOrEmpty(listChassisNumber) && listChassisNumber.length > 0) {
                            myGroupChassisNumber = listChassisNumber[0];
                        }
                        if (AppTool.IsNullOrEmpty(myGroupChassisNumber)) {
                            myGroupChassisNumber = new VehicleModiGroup();
                            myGroupChassisNumber.ChassisNumber = itemDb.ChassisNumber;
                            this.VehicleModiGroupList.push(myGroupChassisNumber);    
                        }
                        myGroupChassisNumber.Total = myGroupChassisNumber.Total + Number(itemDb.DeductAmount);
                        myGroupChassisNumber.MyList.push(itemDb); 
                    });

                //Order list by CreateDate
                this.VehicleModiGroupList.sort(
                    (a, b) => {
                        return (a.ChassisNumber === b.ChassisNumber) ? 0 :
                            (a.ChassisNumber < b.ChassisNumber) ? -1 : 1
                    });

            });
    } 
   
 
   
   


    
}
class VehicleModiGroup {
    public ChassisNumber: string;
    public Total: number=0;
    public MyList: DeclarationVehicleModificationList[] = [];


}
