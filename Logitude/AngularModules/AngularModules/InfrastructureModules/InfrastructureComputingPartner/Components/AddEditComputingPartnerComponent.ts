import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../Infrastructure/Tools';
import {ComputingPartnerTablePM} from '../../../Common/EntityPMs/ComputingPartnerTablePM';
import {ComputingPartnerPM} from '../../../Common/EntityPMs/ComputingPartnerPM'; 
import {ObjectTablePM} from '../../../Infrastructure/EntityPMs/ObjectTablePM';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';

declare var window: any;
@Component({
    selector: 'AddEditComputingPartnerComponent',
    moduleId: module.id,
    templateUrl: './AddEditComputingPartnerComponent.html',
})

export class AddEditComputingPartnerComponent extends BaseComponent {

    public ValidationErrorsList: string[] = [];
    public DataContext: AddEditComputingPartnerComponent = this;
    public TargetEntityName: string = "ComputingPartnerTable";
    public EntityPM:ComputingPartnerTablePM ;
    public myComputingPartnerPM: ComputingPartnerPM;
    public IsNewEntity: boolean = true;
    public HiddenFields: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

    }


    SetWindowArgs(args:any) {
        if (args) {
            this.EntityPM = args.entity;
            this.EntityPM.IsDirty = false;
            this.Clone(this.EntityPM);
            this.myComputingPartnerPM = args.FatherEntity;
            this.IsNewEntity = args.IsNewEntity;
            if (!this.IsNewEntity) {
                this.UIProperties.SetEnabled("ObjectTableId", this.TargetEntityName, false);
            }
            this.HiddenFields  = SessionLocator.Tenant == 0 ? true : false;            
        }
    }

    OkButtonClicked() {
        if (this.EntityPM.IsDirty) {
            var errors: string[] = [];
            Validator.TryValidateObject(this.EntityPM, this.TargetEntityName, errors);
            if (!AppTool.IsNullOrEmpty(this.Name)) {
                if (this.myComputingPartnerPM.PartnerTables.filter(d => d.Name == this.Name && d.ObjectTableId != this.EntityPM.ObjectTableId)[0]) {
                    errors.push("Partner table with same Name already exists");
                }
            }

            if (!AppTool.IsNullOrEmpty(this.ObjectTableId) && this.IsNewEntity) {
                if (this.myComputingPartnerPM.PartnerTables.filter(d => d.ObjectTableId == this.ObjectTableId && d != this.EntityPM)[0]) {
                    errors.push("Partner table with same Object Table already exists");
                }
            }

            this.ValidationErrorsList = errors;



            if (this.ValidationErrorsList.length == 0) {

                this.SubmitChanges();
            }
        }
        else {
            this.CancelButtonClicked();

        }

    }

    private myCloner: Cloner;
    private Clone(EntityPM: any) {
        this.myCloner = new Cloner(EntityPM);       
        this.myCloner.AddField('Name');
        this.myCloner.AddField('ObjectTableId');
        this.myCloner.AddField('MustUsePartnerList');
        this.myCloner.AddField('TransalationRequired');
        this.myCloner.AddField('TenantLevelTranslationBlocked');
        this.myCloner.AddField('ObjectTableName');
        this.myCloner.AddField('HasPartnerList');
        this.myCloner.AddField('UpdateDate');
        this.myCloner.AddField('UpdatedByUserId');
        this.myCloner.AddField('UpdatedByUserName');
        this.myCloner.AddField('ComputingPartnerId');    
        this.myCloner.AddField('MarkAsDirty');        
    
        this.myCloner.AddEntity(EntityPM);
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.myComputingPartnerPM);

    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }



    SubmitChanges() {
        this.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        if (this.IsNewEntity) {
            this.IsNewEntity = false;

            if (!this.myComputingPartnerPM.PartnerTables.includes(this.EntityPM)) {
                this.myComputingPartnerPM.AddComputingPartnerTablePM(this.EntityPM);
            }

        }

        this.CurrentSession.CloseCurrentWindow();
    }

    public get IsEditTableAllowed() {

        var myResult: boolean = true;

        if (SessionLocator.Tenant != 0) {
            if (this.EntityPM.Tenant == 0) {
                myResult = false;
            }

            else if (!FeatureLocator.HasFeaturePermession("ComputingPartner", "ComputingPartner.A.AllowAddEditTables")) {
                myResult = false;
            }
        }

        return myResult;

    }

    public get IsTranslationAllowed() {
        return FeatureLocator.HasFeaturePermession("ComputingPartner", "ComputingPartner.A.AllowTranslation") ? true : false; 
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) { if (this.EntityPM.Name != value) this.EntityPM.Name = value; }

    public get ObjectTableId() { return this.EntityPM.ObjectTableId; }
    public set ObjectTableId(value: string) {
        if (this.EntityPM.ObjectTableId != value) {
            this.EntityPM.ObjectTableId = value;

            if (value != null) {            
                var objectTablePM: ObjectTablePM = window.ObjectTables.filter(d => d.Id == value)[0];
                this.ObjectTableName = objectTablePM.Name;
            }
        }
    }

    public get MustUsePartnerList() { return this.EntityPM.MustUsePartnerList; }
    public set MustUsePartnerList(value: boolean) { if (this.EntityPM.MustUsePartnerList != value) this.EntityPM.MustUsePartnerList = value; }


    public get TransalationRequired() { return this.EntityPM.TransalationRequired; }
    public set TransalationRequired(value: boolean) { if (this.EntityPM.TransalationRequired != value) this.EntityPM.TransalationRequired = value; }


    public get HasPartnerList() { return this.EntityPM.HasPartnerList; }
    public set HasPartnerList(value: boolean) { if (this.EntityPM.HasPartnerList != value) this.EntityPM.HasPartnerList = value; }


    public get TenantLevelTranslationBlocked() { return this.EntityPM.TenantLevelTranslationBlocked; }
    public set TenantLevelTranslationBlocked(value: boolean) { if (this.EntityPM.TenantLevelTranslationBlocked != value) this.EntityPM.TenantLevelTranslationBlocked = value; }

    public get UpdateDate() { return this.EntityPM.UpdateDate; }
    public set UpdateDate(value: Date) { if (this.EntityPM.UpdateDate != value) this.EntityPM.UpdateDate = value; }

    public get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    public set UpdatedByUserId(value: string) { if (this.EntityPM.UpdatedByUserId != value) this.EntityPM.UpdatedByUserId = value; }


    public get UpdatedByUserName() { return this.EntityPM.UpdatedByUserName; }
    public set UpdatedByUserName(value: string) { if (this.EntityPM.UpdatedByUserName != value) this.EntityPM.UpdatedByUserName = value; }   

    public get ComputingPartnerId() { return this.EntityPM.ComputingPartnerId; }
    public set ComputingPartnerId(value: string) { if (this.EntityPM.ComputingPartnerId != value) this.EntityPM.ComputingPartnerId = value; }

    public get ObjectTableName() { return this.EntityPM.ObjectTableName; }
    public set ObjectTableName(value: string) { if (this.EntityPM.ObjectTableName != value) this.EntityPM.ObjectTableName = value; }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

}
