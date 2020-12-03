import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool, DateTool } from '../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { CustomerIdentifyTypePM } from '../../../../../Customs/EntityPMs/CustomerIdentifyTypePM';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { DeclarationExportRecipientPM } from '../../../../../Customs/EntityPMs/DeclarationExportRecipientPM';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({

    
    templateUrl: './ExportDeclarationComponent.html',
    selector: 'ExportDeclarationComponent',

})
export class ExportDeclarationComponent extends BaseComponent {
    public EntityPM: DeclarationPM;
    public DataContext: any = this;
    type: string;
    public ObjectTableName: string = "Customs.Declaration";
    public DeclarationExportRecipientTableName: string = "Customs.DeclarationExportRecipient";
    
    declarationPMService: DeclarationPMService = new DeclarationPMService();
    public OriginalEntityPM: DeclarationPM;
    public ClonedEntityPM: DeclarationPM;
    public IsDisplayOnly: boolean = false;
    public PreceduralFilterItems: ApiQueryFilters;

    _DeclarationExportRecipientPM: DeclarationExportRecipientPM;
    ClonedDeclarationExportRecipientPM: DeclarationExportRecipientPM;
    public ValidationErrorsList: string[] = [];
    constructor() {
        super();

        this.EntityPM = new DeclarationPM();
        this._DeclarationExportRecipientPM = new DeclarationExportRecipientPM(this.EntityPM);
    }


    //#region properties

    _LoadingDateTime: any = null;
    public get LoadingDateTime() {
        return this._LoadingDateTime;
        
        
    }
    public set LoadingDateTime(newValue: any) {
        this.EntityPM.LoadingDateTime = newValue;
        this._LoadingDateTime = newValue;
        if (DateTool.IsNullOrMinDateTime(newValue)) {
            this._LoadingDateTime= null;
        }

    }



    public get ShipCode() { return this.EntityPM.ShipCode; }
    public set ShipCode(newValue: string) {
        this.EntityPM.ShipCode = newValue;
    }

    _CustomsShip: any;
    public get CustomsShip() { return this._CustomsShip; }
    public set CustomsShip(newValue: string) {
        this._CustomsShip;
    }
    public get ProcedureCurrentCode() { return this.EntityPM.ProcedureCurrentCode; }
    public set ProcedureCurrentCode(newValue: string) {
        this.EntityPM.ProcedureCurrentCode = newValue;
    }

    public get ExportAutonomyRegionTypeCode() { return this.EntityPM.ExportAutonomyRegionTypeCode; }
    public set ExportAutonomyRegionTypeCode(newValue: string) {
        this.EntityPM.ExportAutonomyRegionTypeCode = newValue;
    }

    DestinationCountry: any;
    public get DestinationCountryCode() { return this.EntityPM.DestinationCountryCode; }
    public set DestinationCountryCode(newValue: string) {
        this.EntityPM.DestinationCountryCode = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("DestinationCountryCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("DestinationCountryCode", this.ObjectTableName, true);
        }
    }



    public get IsExporterConfirmation() { return this.EntityPM.IsExporterConfirmation; }
    public set IsExporterConfirmation(newValue: boolean) {
        this.EntityPM.IsExporterConfirmation = newValue;
        this.UIProperties.SetRequired("IsExporterConfirmation", this.ObjectTableName, false);
    }

    ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

    public get RecipientName() { return this._DeclarationExportRecipientPM.RecipientName; }
    public set RecipientName(newValue: string) {
        this._DeclarationExportRecipientPM.RecipientName = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("RecipientName", this.DeclarationExportRecipientTableName, false);
        }
        else {
            this.UIProperties.SetRequired("RecipientName", this.DeclarationExportRecipientTableName, true);
        }
    }
    public get RecipientAddress() { return this._DeclarationExportRecipientPM.RecipientAddress; }
    public set RecipientAddress(newValue: string) {
        this._DeclarationExportRecipientPM.RecipientAddress = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("RecipientAddress", this.DeclarationExportRecipientTableName, false);
        }
        else {
            this.UIProperties.SetRequired("RecipientAddress", this.DeclarationExportRecipientTableName, true);
        }
    }
    public _RecipientIssueCountry: any;
    public get RecipientIssueCountryCode() { return this._DeclarationExportRecipientPM.RecipientIssueCountryCode; }
    public set RecipientIssueCountryCode(newValue: string) {
        this._DeclarationExportRecipientPM.RecipientIssueCountryCode = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("RecipientIssueCountryCode", this.DeclarationExportRecipientTableName, false);
        }
        else {
            this.UIProperties.SetRequired("RecipientIssueCountryCode", this.DeclarationExportRecipientTableName, true);
        }  
    }
    ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;


    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.OriginalEntityPM = args.EntityPM;
            this.ClonedEntityPM = this.CloneEntity(args.EntityPM);
            this.type = args.Type;
            this.IsDisplayOnly = args.IsDisplayOnly;
            if (this.EntityPM.DeclarationExportRecipients.length < 1) {
                this._DeclarationExportRecipientPM = new DeclarationExportRecipientPM(this.EntityPM);
                this.EntityPM.AddDeclarationExportRecipient(this._DeclarationExportRecipientPM);
            } else {
                this._DeclarationExportRecipientPM = this.EntityPM.DeclarationExportRecipients[0];
                this.ClonedDeclarationExportRecipientPM = this.CloneEntityDeclarationExportRecipientPM(this._DeclarationExportRecipientPM);
            }

            this.PreceduralFilterItems = new ApiQueryFilters();
            this.PreceduralFilterItems.addAdditionalFilter("IsExport", true, true, true, "Equal", false, false, false, "boolean");

            //Disable fields
            if (this.IsDisplayOnly) {
                this.SetScreenFieldsEditability();
            } else {
                this.LoadingDateTime = this.EntityPM.LoadingDateTime;//Make Nullabe
                this.ForceMust();
            }
            


        }
    }
    ForceMust(): any {
        
        this.DestinationCountryCode = this.EntityPM.DestinationCountryCode;
        this.IsExporterConfirmation = this.EntityPM.IsExporterConfirmation;
        this.RecipientName = this._DeclarationExportRecipientPM.RecipientName;
        this.RecipientAddress = this._DeclarationExportRecipientPM.RecipientAddress;
        this.RecipientIssueCountryCode = this._DeclarationExportRecipientPM.RecipientIssueCountryCode;
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("LoadingDateTime", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ShipCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DestinationCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsExporterConfirmation", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientAddress", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientIssueCountryCode", this.ObjectTableName, !this.IsDisplayOnly);

        

    }





   

    CloneEntity(entityToClone: DeclarationPM) {

        var clonedEntity: DeclarationPM;
        clonedEntity = new DeclarationPM();

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        return clonedEntity;
    }
    CloneEntityDeclarationExportRecipientPM(entityToClone: DeclarationExportRecipientPM) {

        var clonedEntity: DeclarationExportRecipientPM;
        clonedEntity = new DeclarationExportRecipientPM(this.EntityPM);

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        return clonedEntity;
    }
    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedEntityPM, this.OriginalEntityPM, true);
        if (this.ClonedDeclarationExportRecipientPM!=null) {
            this.MapEntitytoEntity(this.ClonedDeclarationExportRecipientPM, this._DeclarationExportRecipientPM, true);

        }
        
    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("cancel");
    }


    doDisable: boolean;

    FillErrors() {
        var errors: string[] = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.DestinationCountryCode)) {

            this.ValidationErrorsList.push("ארץ יעד הינו שדה  חובה");
        }

        //if (AppTool.IsNullOrEmpty(this.IsExporterConfirmation)) {
        //    this.ValidationErrorsList.push("נמל פריקה הינו שדה  חובה");
        //}
        if (AppTool.IsNullOrEmpty(this.RecipientName)) {
            this.ValidationErrorsList.push("שם המקבל הינו שדה  חובה");
        }
        if (AppTool.IsNullOrEmpty(this.RecipientAddress)) {
            this.ValidationErrorsList.push("כתובת המקבל הינו שדה  חובה");
        }
        if (AppTool.IsNullOrEmpty(this.RecipientIssueCountryCode)) {
            this.ValidationErrorsList.push("מדינת המקבל הינו שדה  חובה");
        }

    }
    OkButtonClicked() {
      
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
    


        if (this.doDisable) {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
        }
        else {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit("!ok");
        }




    }

}
