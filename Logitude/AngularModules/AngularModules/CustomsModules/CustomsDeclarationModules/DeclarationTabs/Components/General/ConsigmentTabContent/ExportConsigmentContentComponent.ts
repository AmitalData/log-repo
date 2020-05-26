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

    
    templateUrl: './ExportConsigmentContentComponent.html',
    selector: 'ExportConsigmentContentComponent',

})
export class ExportConsigmentContentComponent extends BaseComponent {
    public EntityPM: ConsignmentPM;
    public OriginalDeclarationPM: DeclarationPM;

    public ObjectTableName: string = "Customs.Consignment";
    public DataContext: any = this;
    

    public IsDisplayOnly: boolean = false;
    public ParentIsDisplayOnly: boolean = false;


    declarationPMService: DeclarationPMService = new DeclarationPMService();
    public OriginalEntityPM: ConsignmentPM;
    public ClonedEntityPM: ConsignmentPM;
    
    public ValidationErrorsList: string[] = [];
    ClonedDeclarationPM: DeclarationPM;

    constructor() {
        super();

        //this.EntityPM = new ConsignmentPM();
    }


    //#region properties


    public get LoadingPortCode() { return this.EntityPM.LoadingPortCode; }
    public set LoadingPortCode(newValue: string) {
        this.EntityPM.LoadingPortCode = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("LoadingPortCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("LoadingPortCode", this.ObjectTableName, true);
        }
    }



    public get UnloadPortCode() { return this.EntityPM.UnloadPortCode; }
    public set UnloadPortCode(newValue: string) {
        this.EntityPM.UnloadPortCode = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("UnloadPortCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("UnloadPortCode", this.ObjectTableName, true);
        }
    }

   

    DestinationCountry: any;
    public get FinalDestinationPortCode() { return this.EntityPM.FinalDestinationPortCode; }
    public set FinalDestinationPortCode(newValue: string) {
        this.EntityPM.FinalDestinationPortCode = newValue;
      
    }

    ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
    public get StorageSiteCode() { return this.EntityPM.StorageSiteCode; }
    public set StorageSiteCode(newValue: string) {
        this.EntityPM.StorageSiteCode = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("StorageSiteCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("StorageSiteCode", this.ObjectTableName, true);
        }
    }

    
    public get RecieverWareHouseCode() { return this.EntityPM.RecieverWareHouseCode; }
    public set RecieverWareHouseCode(newValue: string) {
        this.EntityPM.RecieverWareHouseCode = newValue;
        if (newValue) {
            this.UIProperties.SetRequired("RecieverWareHouseCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("RecieverWareHouseCode", this.ObjectTableName, true);
        }
    }
    

    
    public get IsDangerousGoods() { return this.EntityPM.IsDangerousGoods; }
    public set IsDangerousGoods(newValue: boolean) {
        this.EntityPM.IsDangerousGoods = newValue;
        //if (newValue) {
        //    this.UIProperties.SetRequired("IsDangerousGoods", this.ObjectTableName, false);
        //}
        //else {
        //    this.UIProperties.SetRequired("IsDangerousGoods", this.ObjectTableName, true);
        //}
    }
    ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;


    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.OriginalEntityPM = args.EntityPM;
            this.ClonedEntityPM = this.CloneEntityConsignmentPM(args.EntityPM);

            this.OriginalDeclarationPM = args.declarationPM;
            this.ClonedDeclarationPM = this.CloneEntityDeclarationPM(args.declarationPM);


            this.IsDisplayOnly = args.IsDisplayOnly;
           


            //Disable fields
            if (this.IsDisplayOnly) {
                this.SetScreenFieldsEditability();
            } else {
                
                this.ForceMust();
               
            }



        }
    }

    private ForceMust() {
        this.LoadingPortCode = this.EntityPM.LoadingPortCode;
        this.UnloadPortCode = this.EntityPM.UnloadPortCode;
        //this.FinalDestinationPortCode = this.EntityPM.FinalDestinationPortCode;
        this.StorageSiteCode = this.EntityPM.StorageSiteCode;
        this.RecieverWareHouseCode = this.EntityPM.RecieverWareHouseCode;
        this.IsDangerousGoods = this.EntityPM.IsDangerousGoods;
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("LoadingPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("FinalDestinationPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StorageSiteCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecieverWareHouseCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsDangerousGoods", this.ObjectTableName, !this.IsDisplayOnly);



    }







    CloneEntityDeclarationPM(entityToClone: DeclarationPM) {

        var clonedEntity: DeclarationPM;
        clonedEntity = new DeclarationPM();

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        return clonedEntity;
    }
    CloneEntityConsignmentPM(entityToClone: ConsignmentPM) {

        var clonedEntity: ConsignmentPM;
        clonedEntity = new ConsignmentPM(this.EntityPM);

        this.MapEntitytoEntity(entityToClone, clonedEntity);


        return clonedEntity;
    }
    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedEntityPM, this.OriginalEntityPM, true);
        this.MapEntitytoEntity(this.ClonedDeclarationPM, this.OriginalDeclarationPM);

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

        if (AppTool.IsNullOrEmpty(this.LoadingPortCode)) {
           
            this.ValidationErrorsList.push("נמל טעינה שדה חובה");
        }

        if (AppTool.IsNullOrEmpty(this.UnloadPortCode)) {
            this.ValidationErrorsList.push("נמל פריקה שדה חובה");
        }
        if (AppTool.IsNullOrEmpty(this.StorageSiteCode)) {
            this.ValidationErrorsList.push("אתר  מסירה שדה חובה");
        }
        if (AppTool.IsNullOrEmpty(this.RecieverWareHouseCode)) {
            this.ValidationErrorsList.push("אתר  המכלה שדה חובה");
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
