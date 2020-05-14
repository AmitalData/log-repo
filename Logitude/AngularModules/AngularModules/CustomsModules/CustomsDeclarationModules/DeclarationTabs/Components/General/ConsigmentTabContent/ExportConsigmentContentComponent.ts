import { Component } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../../Customs/EntityPMs/DeclarationPM';
import { ClientList } from '../../../../../../Customs/EntityLists/ClientList';
import { CustomerIdentifyTypePM } from '../../../../../../Customs/EntityPMs/CustomerIdentifyTypePM';
import { MessageWindow } from '../../../../../../Controls/Windows/MessageWindow';
import { DeclarationPMService } from '../../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { DeclarationExportRecipientPM } from '../../../../../../Customs/EntityPMs/DeclarationExportRecipientPM';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { ConsignmentPM } from '../../../../../../Customs/EntityPMs/ConsignmentPM';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({

    moduleId: module.id,
    templateUrl: './ExportConsigmentContentComponent.html',
    selector: 'ExportConsigmentContentComponent',

})
export class ExportConsigmentContentComponent extends BaseComponent {
    public EntityPM: ConsignmentPM;
    public declarationPM: DeclarationPM;

    public ObjectTableName: string = "Customs.Consignment";
    public DataContext: any = this;
    

    public IsDisplayOnly: boolean = false;
    public ParentIsDisplayOnly: boolean = false;


    declarationPMService: DeclarationPMService = new DeclarationPMService();
    public OriginalEntityPM: ConsignmentPM;
    public ClonedEntityPM: ConsignmentPM;
    
   

    constructor() {
        super();

        //this.EntityPM = new ConsignmentPM();
    }


    //#region properties


    public get LoadingDateTime() { return this.EntityPM.LoadingDateTime; }
    public set LoadingDateTime(newValue: Date) { this.EntityPM.LoadingDateTime = newValue; }



    public get ShipCode() { return this.EntityPM.ShipCode; }
    public set ShipCode(newValue: string) {
        this.EntityPM.ShipCode = newValue;
    }

    _CustomsShip: any;
    public get CustomsShip() { return this._CustomsShip; }
    public set CustomsShip(newValue: string) {
        this._CustomsShip;
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
    }
    public get RecipientAddress() { return this._DeclarationExportRecipientPM.RecipientAddress; }
    public set RecipientAddress(newValue: string) {
        this._DeclarationExportRecipientPM.RecipientAddress = newValue;
    }
    public _RecipientIssueCountry: any;
    public get RecipientIssueCountryCode() { return this._DeclarationExportRecipientPM.RecipientIssueCountryCode; }
    public set RecipientIssueCountryCode(newValue: string) {
        this._DeclarationExportRecipientPM.RecipientIssueCountryCode = newValue;

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


            //Disable fields
            if (this.IsDisplayOnly) {
                this.SetScreenFieldsEditability();
            } else {

            }



        }
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
        if (this.ClonedDeclarationExportRecipientPM != null) {
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


    OkButtonClicked() {


        SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();



        if (this.doDisable) {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
        }
        else {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit("!ok");
        }




    }

}
