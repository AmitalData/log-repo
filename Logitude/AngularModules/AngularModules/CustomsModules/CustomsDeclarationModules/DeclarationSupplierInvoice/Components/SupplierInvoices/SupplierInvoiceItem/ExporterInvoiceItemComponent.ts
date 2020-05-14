import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { DeclarationValidator } from "../../../../../../Customs/Validators/DeclarationValidator";
import { SupplierInvoicePM } from "../../../../../../Customs/EntityPMs/SupplierInvoicePM";
import { SupplierInvoicePaymentPM } from "../../../../../../Customs/EntityPMs/SupplierInvoicePaymentPM";
import { SupplierInvoiceItemPM } from "../../../../../../Customs/EntityPMs/SupplierInvoiceItemPM";
import { SuppInvoiceItemsAbachStatementPM } from "../../../../../../Customs/EntityPMs/SuppInvoiceItemsAbachStatementPM";
import { SupplierInvoiceItemVehiclePM } from "../../../../../../Customs/EntityPMs/SupplierInvoiceItemVehiclePM";
import { SupplierInvoiceItemsPricePM } from "../../../../../../Customs/EntityPMs/SupplierInvoiceItemsPricePM";
import { SessionLocator } from "../../../../../../Infrastructure/Utilities/SessionLocator";
import { AppTool } from "../../../../../../Infrastructure/Tools";
import { SupplierInvoiceItemProcesTypePM } from "../../../../../../Customs/EntityPMs/SupplierInvoiceItemProcesTypePM";
import { Validator } from "../../../../../../Infrastructure/Validators/Validator";
  
@Component({
    moduleId: module.id,
    templateUrl: './ExporterInvoiceItemComponent.html',
})


export class ExporterInvoiceItemComponent extends BaseComponent
    implements OnInit {
 
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.SupplierInvoiceItem";
    public ObjectTableNameSupplierInvoiceItemVehicle: string = "Customs.SupplierInvoiceItemVehicle";
    public ObjectTableNameSuppInvoiceItemsAbachStatement: string = "Customs.SuppInvoiceItemsAbachStatement";
    public ObjectTableNameSupplierInvoiceItemsPrice: string = "Customs.SupplierInvoiceItemsPrice";

    public ValidationErrorsList: any[] = [];
    public declarationValidator: DeclarationValidator = new DeclarationValidator();
    public finishedLoad: boolean;
    originalSupplierInvoiceItem: SupplierInvoiceItemPM;
    clonedSupplierInvoiceItem: SupplierInvoiceItemPM;


    originalSupplierInvoiceItemVehicle: SupplierInvoiceItemVehiclePM;
    clonedSupplierInvoiceItemVehicle: SupplierInvoiceItemVehiclePM;

    originalSuppInvoiceItemsAbachStatement: SuppInvoiceItemsAbachStatementPM;
    clonedSuppInvoiceItemsAbachStatement: SuppInvoiceItemsAbachStatementPM;

    originalSupplierInvoiceItemsPrice: SupplierInvoiceItemsPricePM;
    clonedSupplierInvoiceItemsPrice: SupplierInvoiceItemsPricePM;

    private CurrentSession = SessionLocator.SelectedSession;


    public get ClassificationTypeCode() { return this.originalSupplierInvoiceItem ? this.originalSupplierInvoiceItem.ClassificationTypeCode : null; }
    public set ClassificationTypeCode(newValue: string) {
        if (AppTool.IsNullOrEmpty(newValue))
            this.UIProperties.SetRequired("ClassificationTypeCode", this.ObjectTableName, true);
        else
            this.UIProperties.SetRequired("ClassificationTypeCode", this.ObjectTableName, false);

        this.originalSupplierInvoiceItem.ClassificationTypeCode = newValue;
    }

    public get TransactionNatureCode() { return this.originalSupplierInvoiceItem ? this.originalSupplierInvoiceItem.TransactionNatureCode : null; }
    public set TransactionNatureCode(newValue: string) {
        if (AppTool.IsNullOrEmpty(newValue))
            this.UIProperties.SetRequired("TransactionNatureCode", this.ObjectTableName, true);
        else
            this.UIProperties.SetRequired("TransactionNatureCode", this.ObjectTableName, false);

        this.originalSupplierInvoiceItem.TransactionNatureCode = newValue;
    }

    public get ClaimReasonCode() { return this.originalSupplierInvoiceItem ? this.originalSupplierInvoiceItem.ClaimReasonCode : null; }
    public set ClaimReasonCode(newValue: string) {
        if (AppTool.IsNullOrEmpty(newValue))
            this.UIProperties.SetRequired("ClaimReasonCode", this.ObjectTableName, true);
        else
            this.UIProperties.SetRequired("ClaimReasonCode", this.ObjectTableName, false);

        this.originalSupplierInvoiceItem.ClaimReasonCode = newValue;
    }

    public get StatementType() { return this.originalSuppInvoiceItemsAbachStatement ? this.originalSuppInvoiceItemsAbachStatement.StatementType : null; }
    public set StatementType(newValue: string) {
        this.originalSuppInvoiceItemsAbachStatement.StatementType = newValue;
        this.originalSuppInvoiceItemsAbachStatement.IsDirty = true;

    }

    public get StatementInd() { return this.originalSuppInvoiceItemsAbachStatement ? this.originalSuppInvoiceItemsAbachStatement.StatementInd : null; }
    public set StatementInd(newValue: string) {
        this.originalSuppInvoiceItemsAbachStatement.StatementInd = newValue;
        this.originalSuppInvoiceItemsAbachStatement.IsDirty = true;
    }


    public get IdentifierID() { return this.originalSupplierInvoiceItemVehicle ? this.originalSupplierInvoiceItemVehicle.IdentifierID : null; }
    public set IdentifierID(newValue: string) {
        this.originalSupplierInvoiceItemVehicle.IdentifierID = newValue;
        this.originalSupplierInvoiceItemVehicle.IsDirty = true;
    }


    public get VehicleIDTypeCode() { return this.originalSupplierInvoiceItemVehicle ? this.originalSupplierInvoiceItemVehicle.VehicleIDTypeCode : null; }
    public set VehicleIDTypeCode(newValue: any) {
        this.originalSupplierInvoiceItemVehicle.VehicleIDTypeCode = newValue;
        this.originalSupplierInvoiceItemVehicle.IsDirty = true;
    }


    public get AdditionalPriceTypeCode() { return this.originalSupplierInvoiceItemsPrice ? this.originalSupplierInvoiceItemsPrice.AdditionalPriceTypeCode : null; }
    public set AdditionalPriceTypeCode(newValue: any) {
        this.originalSupplierInvoiceItemsPrice.AdditionalPriceTypeCode = newValue;
        this.originalSupplierInvoiceItemsPrice.IsDirty = true;
    }


    public get AdditionalPrice() { return this.originalSupplierInvoiceItemsPrice ? this.originalSupplierInvoiceItemsPrice.AdditionalPrice : null; }
    public set AdditionalPrice(newValue: any) {
        this.originalSupplierInvoiceItemsPrice.AdditionalPrice = newValue; this.originalSupplierInvoiceItemsPrice.IsDirty = true; }



    ngOnInit(): void {
    }
    SetWindowArgs(args: any) {

        SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {

            SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName(this.ObjectTableNameSuppInvoiceItemsAbachStatement, 0).subscribe(response => {

                SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName(this.ObjectTableNameSupplierInvoiceItemsPrice, 0).subscribe(response => {

                    SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName(this.ObjectTableNameSupplierInvoiceItemVehicle, 0).subscribe(response => {

                    if (!AppTool.IsNullOrEmpty(args)) {
                      //this.IsDisplayOnly = args.IsDisplayOnly;
                    if (args.SupplierInvoiceItem != null) {
                         this.originalSupplierInvoiceItem = args.SupplierInvoiceItem;
                        this.clonedSupplierInvoiceItem = this.CloneSupplierInvoiceItem(args.SupplierInvoiceItem);

                        this.ClaimReasonCode = this.originalSupplierInvoiceItem.ClaimReasonCode;
                        this.TransactionNatureCode = this.originalSupplierInvoiceItem.TransactionNatureCode;
                        this.ClassificationTypeCode = this.originalSupplierInvoiceItem.ClassificationTypeCode;
 
                        if (args.SupplierInvoiceItem.SupplierInvoiceItemsPrices != null && args.SupplierInvoiceItem.SupplierInvoiceItemsPrices.length > 0) {
                            this.originalSupplierInvoiceItemsPrice = args.SupplierInvoiceItem.SupplierInvoiceItemsPrices[0];
                            this.clonedSupplierInvoiceItemsPrice = this.CloneSupplierInvoiceItemsPrice(args.SupplierInvoiceItem.SupplierInvoiceItemsPrices[0]);
                        }
                        else {
                            this.originalSupplierInvoiceItemsPrice = new SupplierInvoiceItemsPricePM(this.originalSupplierInvoiceItem);

                        }
                        if (args.SupplierInvoiceItem.SupplierInvoiceItemVehicles != null && args.SupplierInvoiceItem.SupplierInvoiceItemVehicles.length > 0) {
                            this.originalSupplierInvoiceItemVehicle = args.SupplierInvoiceItem.SupplierInvoiceItemVehicles[0];
                            this.clonedSupplierInvoiceItemVehicle = this.CloneSupplierInvoiceItemVehicle(args.SupplierInvoiceItem.SupplierInvoiceItemVehicles[0]);
                        }
                        else {
                            this.originalSupplierInvoiceItemVehicle = new SupplierInvoiceItemVehiclePM(this.originalSupplierInvoiceItem);

                        }

                        if (args.SupplierInvoiceItem.SuppInvoiceItemsAbacshStatement != null && args.SupplierInvoiceItem.SuppInvoiceItemsAbacshStatement.length > 0) {
                            this.originalSuppInvoiceItemsAbachStatement = args.SupplierInvoiceItem.SuppInvoiceItemsAbacshStatement[0];
                            this.clonedSuppInvoiceItemsAbachStatement = this.CloneSuppInvoiceItemsAbachStatement(args.SupplierInvoiceItem.SuppInvoiceItemsAbacshStatement[0]);
                        }
                        else {
                            this.originalSuppInvoiceItemsAbachStatement = new SuppInvoiceItemsAbachStatementPM(this.originalSupplierInvoiceItem);

                        }


 }
                    //this.UIProperties.SetRequired("BuyerName", this.ObjectTableName, true);
                    //this.UIProperties.SetRequired("PartyRelationshipCode", this.ObjectTableName, true);
 
                    this.finishedLoad = true;
                    //this.SetScreenFieldsEditability();
                    //this.SetContactFieldsEditability();

                }

            });
        });
    });
        });





    }

   

    CloneSupplierInvoiceItem(entityToClone: SupplierInvoiceItemPM) {

        var clonedEntity: SupplierInvoiceItemPM;
        clonedEntity = new SupplierInvoiceItemPM(null);

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }

    CloneSupplierInvoiceItemsPrice(entityToClone: SupplierInvoiceItemsPricePM) {

        var clonedEntity: SupplierInvoiceItemsPricePM;
        clonedEntity = new SupplierInvoiceItemsPricePM(null);

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }

    CloneSupplierInvoiceItemVehicle(entityToClone: SupplierInvoiceItemVehiclePM) {

        var clonedEntity: SupplierInvoiceItemVehiclePM;
        clonedEntity = new SupplierInvoiceItemVehiclePM(null);

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }

    CloneSuppInvoiceItemsAbachStatement(entityToClone: SuppInvoiceItemsAbachStatementPM) {

        var clonedEntity: SuppInvoiceItemsAbachStatementPM;
        clonedEntity = new SuppInvoiceItemsAbachStatementPM(null);

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }


    CancelButtonClicked() {
        if (this.clonedSupplierInvoiceItem != null) {
            this.RejectChanges();
        }
        this.CurrentSession.CloseCurrentWindow();
    }

    RejectChanges() {
        this.MapEntitytoEntity(this.clonedSupplierInvoiceItem, this.originalSupplierInvoiceItem, true);
       // this.MapEntitytoEntity(this.clonedSupplierInvoiceItemsPrice, this.originalSupplierInvoiceItemsPrice, true);
        //this.MapEntitytoEntity(this.clonedSupplierInvoiceItemVehicle, this.originalSupplierInvoiceItemVehicle, true);
        //this.MapEntitytoEntity(this.clonedSuppInvoiceItemsAbachStatement, this.clonedSuppInvoiceItemsAbachStatement, true);

    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }

    OkButtonClicked() {
         var errors = [];

        //if (this.OriginalConsignmentPackDangerPM.DeclarationId == undefined && this.Package != undefined) {
        //    this.OriginalConsignmentPackDangerPM.DeclarationId = "-1";
        //    this.OriginalConsignmentPackDangerPM.LineNumber = -1;
        //    this.OriginalConsignmentPackDangerPM.ConsignmentNumber = -1;
        //    this.OriginalConsignmentPackDangerPM.DangerousLineNo = 1;
        //    this.OriginalConsignmentPackDangerPM.Tenant = SessionLocator.Tenant;
        //    this.Package.AddConsignmentPackDanger(this.OriginalConsignmentPackDangerPM);

        //}
        if (this.originalSupplierInvoiceItemsPrice.DeclarationId == undefined && this.originalSupplierInvoiceItem != undefined) {
            this.originalSupplierInvoiceItemsPrice.DeclarationId = this.originalSupplierInvoiceItem.DeclarationId;
            this.originalSupplierInvoiceItemsPrice.InvoiceCounterKey = this.originalSupplierInvoiceItem.CounterKey;
            this.originalSupplierInvoiceItemsPrice.LineNumber = 1;
            this.originalSupplierInvoiceItemsPrice.InvoiceItemLineNumber = this.originalSupplierInvoiceItem.LineNumber;

            this.originalSupplierInvoiceItemsPrice.Tenant = SessionLocator.Tenant;
            this.originalSupplierInvoiceItem.AddSupplierInvoiceItemsPrice(this.originalSupplierInvoiceItemsPrice);
        }
        else {
            this.originalSupplierInvoiceItemsPrice.UniqueKey = "1";
  
        }

        if (this.originalSupplierInvoiceItemVehicle.DeclarationId == undefined && this.originalSupplierInvoiceItem != undefined) {
            this.originalSupplierInvoiceItemVehicle.DeclarationId = this.originalSupplierInvoiceItem.DeclarationId;
            this.originalSupplierInvoiceItemVehicle.InvoiceCounterKey = this.originalSupplierInvoiceItem.CounterKey;
            this.originalSupplierInvoiceItemVehicle.LineNumber = 1;
            this.originalSupplierInvoiceItemVehicle.InvoiceItemLineNumber = this.originalSupplierInvoiceItem.LineNumber;

            this.originalSupplierInvoiceItemVehicle.Tenant = SessionLocator.Tenant;
            this.originalSupplierInvoiceItem.AddSupplierInvoiceItemVehicle(this.originalSupplierInvoiceItemVehicle);
        }
        else {
            this.originalSupplierInvoiceItemVehicle.UniqueKey = "1";

        }


        if (this.originalSuppInvoiceItemsAbachStatement.DeclarationId == undefined && this.originalSupplierInvoiceItem != undefined) {
            this.originalSuppInvoiceItemsAbachStatement.DeclarationId = this.originalSupplierInvoiceItem.DeclarationId;
            this.originalSuppInvoiceItemsAbachStatement.InvoiceCounterKey = this.originalSupplierInvoiceItem.CounterKey;
            this.originalSuppInvoiceItemsAbachStatement.SequenceNumeric = 1;
            this.originalSuppInvoiceItemsAbachStatement.InvoiceItemLineNumber = this.originalSupplierInvoiceItem.LineNumber;

            this.originalSuppInvoiceItemsAbachStatement.Tenant = SessionLocator.Tenant;
            this.originalSupplierInvoiceItem.AddSuppInvoiceItemsAbachStatement(this.originalSuppInvoiceItemsAbachStatement);
        }
        else {
            this.originalSuppInvoiceItemsAbachStatement.UniqueKey = "1";

        }

        if (this.originalSuppInvoiceItemsAbachStatement.IsDirty || this.originalSupplierInvoiceItemsPrice.IsDirty || this.originalSupplierInvoiceItemVehicle.IsDirty) {
            this.originalSupplierInvoiceItem.IsDirty = true;

        }

        //errors = this.ValidateCustomsItemField();

        var errors = [];
        Validator.TryValidateObject(this.originalSupplierInvoiceItem, this.ObjectTableName, errors);
        Validator.TryValidateObject(this.originalSupplierInvoiceItemsPrice, this.ObjectTableNameSupplierInvoiceItemsPrice, errors);
        Validator.TryValidateObject(this.originalSupplierInvoiceItemVehicle, this.ObjectTableNameSupplierInvoiceItemVehicle, errors);
        Validator.TryValidateObject(this.originalSuppInvoiceItemsAbachStatement, this.ObjectTableNameSuppInvoiceItemsAbachStatement, errors);

        if (AppTool.IsNullOrEmpty(this.ClaimReasonCode)) errors.push("סיבת תביעה שדה חובה");
        if (AppTool.IsNullOrEmpty(this.TransactionNatureCode)) errors.push("אופי עסקה שדה חובה");
        if (AppTool.IsNullOrEmpty(this.ClassificationTypeCode)) errors.push("סוג ערך סיווג שדה חובה");
        
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        } else {


            this.CurrentSession.CloseCurrentWindow();
        }


    }



}
