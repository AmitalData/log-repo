import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { CustomerIdentifyTypePM } from '../../../../../Customs/EntityPMs/CustomerIdentifyTypePM';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';

@Component({

    moduleId: module.id,
    templateUrl: './ExportDeclarationComponent.html',
    selector: 'ExportDeclarationComponent',

})
export class ExportDeclarationComponent extends BaseComponent {
    public EntityPM: DeclarationPM;
    public DataContext: any = this;
    type: string;
    public ObjectTableName: string = "Customs.Declaration";
    declarationPMService: DeclarationPMService = new DeclarationPMService();
    public OriginalEntityPM: DeclarationPM;
    public ClonedEntityPM: DeclarationPM;
    public IsDisplayOnly: boolean = false;

    constructor() {
        super();


    }


    //#region properties

   
    public get LoadingDateTime() { return this.EntityPM.LoadingDateTime; }
    public set LoadingDateTime(newValue: Date) { this.EntityPM.LoadingDateTime = newValue; }



    public get ShipCode() { return this.EntityPM.ShipCode; }
    public set ShipCode(newValue: string) {
        this.EntityPM.ShipCode = newValue;
    }

    _Ship: any;
    public get Ship() { return this._Ship; }
    public set Ship(newValue: string) {
        this._Ship;
    }
    

    public get DestinationCountryCode() { return this.EntityPM.DestinationCountryCode; }
    public set DestinationCountryCode(newValue: string) {
        this.EntityPM.DestinationCountryCode = newValue;
    }

    

    public get IsExporterConfirmation() { return this.EntityPM.IsExporterConfirmation; }
    public set IsExporterConfirmation(newValue: boolean) {
        this.EntityPM.IsExporterConfirmation = newValue;
    }


    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.OriginalEntityPM = args.EntityPM;
            this.ClonedEntityPM = this.CloneEntity(args.EntityPM);
            this.type = args.Type;
            this.IsDisplayOnly = args.IsDisplayOnly;

            

            //Disable fields
            if (this.IsDisplayOnly) {
                this.SetScreenFieldsEditability();
            }



        }
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("ShipCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterAddress", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferShipCode", this.ObjectTableName, !this.IsDisplayOnly);
        
        if (this.IsDisplayOnly) {
        
            this.IsImporterEnabled = !this.IsDisplayOnly;
        }

    }

  



    ImporterClicked(type, client: ClientList) {

        if (client) {
            switch (type) {
                case 'Importer': {
                    this.ImporterPassportNumber = client != null ? client.PassportNumber : null;

                    break;
                }
                case 'Transfer': {
                    this.TransferPassportNumber = client != null ? client.PassportNumber : null;

                    break;
                }
                case 'Entitle': {
                    this.EntitlePassportNumber = client != null ? client.PassportNumber : null;
                    break;
                }
            }
        }



    }

    CloneEntity(entityToClone: DeclarationPM) {

        var clonedEntity: DeclarationPM;
        clonedEntity = new DeclarationPM();

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        return clonedEntity;
    }

    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedEntityPM, this.OriginalEntityPM, true);
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
        if (this.type == "Importer" && this.isCourierDeclaration) {
            //if (!FormatTool.IsEmail(this.CasualImporterEmail)) {
            //errors.push("Invalid email format!");
            //}

            //save Declaration
            //merge CasualImporterAddress1, CasualImporterAddress2, CasualImporterCity  to ImporterAddress field
            this.ImporterAddress = null;
            this.Append2ImporterAddress(this.CasualImporterAddress1);
            this.Append2ImporterAddress(this.CasualImporterAddress2);
            this.Append2ImporterAddress(this.CasualImporterCity);


        }

        SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
        var passportNumber: string;
        switch (this.type) {
            case "Importer": {
                if (this.ImporterTypeCode == "1" || AppTool.IsNullOrEmpty(this.ImporterTypeName)) {
                    this.doDisable = false;
                    if (AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {

                        this.EntityPM.CalculatedShipCode = this.ShipCode;

                    }


                }
                else {
                    this.doDisable = true;
                    if (this.ImporterPassportNumber) {
                        passportNumber = this.GetPassportNumber(this.ImporterPassportNumber);
                        this.EntityPM.ImporterCode = this.ImporterTypeName + "-" + passportNumber;
                    }
                    else {
                        this.EntityPM.ImporterCode = this.ImporterTypeName
                    }
                    this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, false);
                }

                break;
            }

            case "Transfer": {
                if (this.TransferImporterTypeCode == "1" || AppTool.IsNullOrEmpty(this.TransferImporterTypeName)) {
                    this.doDisable = false;
                    if (AppTool.IsNullOrEmpty(this.EntityPM.TransferImporterCode)) {

                        this.EntityPM.CalculatedTransferShipCode = this.TransferShipCode;

                    }
                }
                else {
                    this.doDisable = true;
                    if (this.TransferPassportNumber) {
                        passportNumber = this.GetPassportNumber(this.TransferPassportNumber);
                        this.EntityPM.TransferImporterCode = this.TransferImporterTypeName + "-" + passportNumber;
                    }
                    else {
                        this.EntityPM.TransferImporterCode = this.TransferImporterTypeName;
                    }
                    this.UIProperties.SetEnabled("TransferImporterCode", this.ObjectTableName, false);
                }

                break;
            }

            case "Entitle": {
                if (this.EntitleImporterTypeCode == "1" || AppTool.IsNullOrEmpty(this.TransferImporterTypeName)) {
                    this.doDisable = false;
                    if (AppTool.IsNullOrEmpty(this.EntityPM.EntitleImporterCode)) {

                        this.EntityPM.CalculatedLoadingDateTime = this.LoadingDateTime;

                    }
                }

                else {
                    this.doDisable = true;
                    if (this.EntitlePassportNumber) {
                        passportNumber = this.GetPassportNumber(this.EntitlePassportNumber);
                        this.EntityPM.EntitleImporterCode = this.EntitleImporterTypeName + "-" + passportNumber;
                    }
                    else {
                        this.EntityPM.EntitleImporterCode = this.EntitleImporterTypeName;
                    }
                    this.UIProperties.SetEnabled("EntitleImporterCode", this.ObjectTableName, false);
                }
                break;
            }
        }


        if (this.doDisable) {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
        }
        else {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit("!ok");
        }







    }

}
