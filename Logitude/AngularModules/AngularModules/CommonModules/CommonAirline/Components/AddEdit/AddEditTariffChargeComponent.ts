import {Component} from '@angular/core';
import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {LogLabelComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogLabelComponent';
import {LogTextBoxComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogTextBoxComponent';
import {LogLovComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogLovComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TarrifChargePM} from '../../../../Common/EntityPMs/TarrifChargePM';
import {TarrifHeaderPM} from '../../../../Common/EntityPMs/TarrifHeaderPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {AddEditTarrifHeaderComponent, TariffChargeItem} from '../AddEdit/AddEditTarrifHeaderComponent';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {QueryFilterItem} from '../../../../Report/Components/Filters/QueryFilterItem';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditTariffChargeComponent.html',

})

export class AddEditTariffChargeComponent extends BaseComponent {

    public ObjectTableName: string = "TarrifCharge";
    public EntityPM: TarrifChargePM;
    public TarrifHeaderPM: TarrifHeaderPM;
    public DataContext: TariffChargeItem;
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public UnitPrice: any;
    public MinPrice: any;
    public MaxPrice: any;
    private chargesTypeListService: ChargesTypeListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.chargesTypeListService = new ChargesTypeListService();
    }

    public get ChargesTypeId() { return this.EntityPM.ChargesTypeId; }
    public set ChargesTypeId(value: string) {
        if (this.EntityPM.ChargesTypeId != value) {
            this.EntityPM.ChargesTypeId = value;
            this.GetChargesTypeData();
        }
    }

    private GetChargesTypeData() {
        if (this.EntityPM.ChargesTypeId == null) {
            this.ChargesGroupCode = null;
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.MeasurementId = null;
            this.CurrencyId = null;

        }

        else {
            this.chargesTypeListService.getAllFromCache().subscribe(p => {
                var list: ChargesTypeList = p.Result.filter(p => p.id == this.EntityPM.ChargesTypeId)[0];
                if (list != null) {
                    this.ChargesGroupCode = list.ChargesGroupCode;
                    this.ChargesTypeCode = list.Code;
                    this.ChargesTypeName = list.EnglishName;
                    this.MeasurementId = list.MeasurementId;

                    if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                        this.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                    }

                    else {
                        this.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                    }
                }
            });


    }
    }



    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('MeasurementId');
        this.myCloner.AddField('CurrencyId');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddField('MinPrice');
        this.myCloner.AddField('MaxPrice');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }



    private chargesGroupCode:string;
    public get ChargesGroupCode() { return this.chargesGroupCode; }
    public set ChargesGroupCode(value: string) { this.chargesGroupCode = value; }
    public get ChargesTypeCode() { return this.EntityPM.ChargesTypeCode; }
    public set ChargesTypeCode(value: string) { this.EntityPM.ChargesTypeCode = value; }
    public get ChargesTypeName() { return this.EntityPM.ChargesTypeName; }
    public set ChargesTypeName(value: string) { this.EntityPM.ChargesTypeName = value; }
    public get MeasurementId() { return this.EntityPM.MeasurementId; }
    public set MeasurementId(value: string) { this.EntityPM.MeasurementId = value; }
    public get CurrencyId() { return this.EntityPM.CurrencyId; }
    public set CurrencyId(value: string) { this.EntityPM.CurrencyId = value; }

    public get ChargeType() {
        var str: string = null;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ChargesTypeId)) {
            str = "(" + this.ChargesTypeCode + ") " + this.ChargesTypeName;
        }
        return str;
    }

    SetDataContext(dataContext: TariffChargeItem) {
        this.TarrifHeaderPM = dataContext.TarrifHeaderPM;
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        if (this.TarrifHeaderPM.TarrifTypeCode == "S") {
            this.ChargeTypesQueryFilters = new ApiQueryFilters();
            this.ChargeTypesQueryFilters.addAdditionalFilter("ChargesGroupCode", "SCH", null, null, "Equals", false, false, false, "string");
        }
        this.Clone();

      //  this.EntityPM.CloneMe();
       
        //this.SetLabels();
    }
   
    //Commands 
    CancelButtonClicked() {
        // this.EntityPM.RejectChanges();
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);

        if (this.TarrifHeaderPM.TarrifTypeCode == "S") {
            if (this.DataContext.ChargesGroupCode == "FRT"){
                errors.push(TextCodeTranslator.Translate("TarrifCharge.M.CantAddFreightToSurcharge"));
            }
        }

        if (this.DataContext.ChargesTypeId != null) {
            if (this.DataContext.fatherComponent.TarrifChargesObsList.filter(p => p.EntityPM != this.EntityPM && p.ChargesTypeId == this.ChargesTypeId)[0]) {
                errors.push(TextCodeTranslator.Translate("TarrifCharge.M.TarrifChargeTypeAlreadyAdded"));

            }


        }

        if (this.DataContext.MinPrice != null && this.DataContext.MaxPrice != null && this.DataContext.MinPrice > this.DataContext.MaxPrice) {
            errors.push(TextCodeTranslator.Translate("TarrifCharge.M.MinLessThanMax"));
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {

                this.DataContext.IsNewEntity = false;

                if (this.DataContext.TarrifHeaderPM.TarrifCharges.indexOf(this.EntityPM) == -1) {
                    this.TarrifHeaderPM.TarrifCharges.push(this.EntityPM);
                    this.DataContext.fatherComponent.BuildData();
                }
            }

            this.CurrentSession.CloseCurrentWindow();
            //this.SubmitChanges();
        }
    }

}

export class ChargeWindowArgs {
    public EntityPM: TarrifHeaderPM;
    public Trigger: AddEditTarrifHeaderComponent;
}
