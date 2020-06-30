import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { DeclarationValidator } from "../../../../../Customs/Validators/DeclarationValidator";
import { SupplierInvoiceCurrency } from "../../../DeclarationTabs/Components/Taxes/DeclarationTaxesTabComponent";
import { SupplierInvoicePM } from "../../../../../Customs/EntityPMs/SupplierInvoicePM";
import { SessionLocator } from "../../../../../Infrastructure/Utilities/SessionLocator";
import { AppTool } from "../../../../../Infrastructure/Tools";
import { SupplierInvoicePaymentPM } from "../../../../../Customs/EntityPMs/SupplierInvoicePaymentPM";
import { SupplierInvoiceUCRPM } from "../../../../../Customs/EntityPMs/SupplierInvoiceUCRPM";
import { Validator } from "../../../../../Infrastructure/Validators/Validator";

@Component({
    
    templateUrl: './ExporterInvoiceComponent.html',
})


export class ExporterInvoiceComponent extends BaseComponent
    implements OnInit {
 
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.SupplierInvoice";
    public ObjectTableNameSupplierInvoicePayment: string = "Customs.SupplierInvoicePayment";
    public ObjectTableNameSupplierInvoiceUCR: string = "Customs.SupplierInvoiceUCR";

    public ValidationErrorsList: any[] = [];
    public declarationValidator: DeclarationValidator = new DeclarationValidator();
    public finishedLoad: boolean;
    originalSupplierInvoice: SupplierInvoicePM;
    clonedSupplierInvoice: SupplierInvoicePM;


    originalSupplierInvoicePayment: SupplierInvoicePaymentPM;
    clonedSupplierInvoicePayment: SupplierInvoicePaymentPM;

    originalSupplierInvoiceUCR: SupplierInvoiceUCRPM;
    clonedSupplierInvoiceUCR: SupplierInvoiceUCRPM;

    private CurrentSession = SessionLocator.SelectedSession;


    public get BuyerName() { return this.originalSupplierInvoice ? this.originalSupplierInvoice.BuyerName : null; }
    public set BuyerName(newValue: string) {
        //if (AppTool.IsNullOrEmpty(newValue))
        //    this.UIProperties.SetRequired("BuyerName", this.ObjectTableName, true);
        //else
        //    this.UIProperties.SetRequired("BuyerName", this.ObjectTableName, false);

        this.originalSupplierInvoice.BuyerName = newValue;
    }

    public get BuyerAddress() { return this.originalSupplierInvoice ? this.originalSupplierInvoice.BuyerAddress : null; }
    public set BuyerAddress(newValue: string) {
        //if (AppTool.IsNullOrEmpty(newValue))
        //    this.UIProperties.SetRequired("BuyerAddress", this.ObjectTableName, true);
        //else
        //    this.UIProperties.SetRequired("BuyerAddress", this.ObjectTableName, false);

        this.originalSupplierInvoice.BuyerAddress = newValue;
    }

    public get BuyerCountryCode() { return this.originalSupplierInvoice ? this.originalSupplierInvoice.BuyerCountryCode : null; }
    public set BuyerCountryCode(newValue: string) {
        //if (AppTool.IsNullOrEmpty(newValue))
        //    this.UIProperties.SetRequired("BuyerCountryCode", this.ObjectTableName, true);
        //else
        //    this.UIProperties.SetRequired("BuyerCountryCode", this.ObjectTableName, false);

        this.originalSupplierInvoice.BuyerCountryCode = newValue;
    }

    public get BuyerRoleCode() { return this.originalSupplierInvoice ? this.originalSupplierInvoice.BuyerRoleCode : null; }
    public set BuyerRoleCode(newValue: string) {
         //if (AppTool.IsNullOrEmpty(newValue))
         //    this.UIProperties.SetRequired("BuyerRoleCode", this.ObjectTableName, true);
         //else
         //   this.UIProperties.SetRequired("BuyerRoleCode", this.ObjectTableName, false);

        this.originalSupplierInvoice.BuyerRoleCode = newValue;
    }

    public get PartyRelationshipCode() { return this.originalSupplierInvoice ? this.originalSupplierInvoice.PartyRelationshipCode : null; }
    public set PartyRelationshipCode(newValue: string) {
        if (AppTool.IsNullOrEmpty(newValue))
            this.UIProperties.SetRequired("PartyRelationshipCode", this.ObjectTableName, true);
        else
            this.UIProperties.SetRequired("PartyRelationshipCode", this.ObjectTableName, false);

        this.originalSupplierInvoice.PartyRelationshipCode = newValue;
    }


    public get PaymentTypeCode() { return this.originalSupplierInvoicePayment ? this.originalSupplierInvoicePayment.PaymentTypeCode : null; }
    public set PaymentTypeCode(newValue: string) {
        this.originalSupplierInvoicePayment.PaymentTypeCode = newValue; this.originalSupplierInvoicePayment.IsDirty = true;}


    public get PaymentAmount() { return this.originalSupplierInvoicePayment ? this.originalSupplierInvoicePayment.PaymentAmount : null; }
    public set PaymentAmount(newValue: any) {
        this.originalSupplierInvoicePayment.PaymentAmount = newValue; this.originalSupplierInvoicePayment.IsDirty = true;}


    public get SupplierChargeID() { return this.originalSupplierInvoiceUCR ? this.originalSupplierInvoiceUCR.SupplierChargeID : null; }
    public set SupplierChargeID(newValue: any) {
        this.originalSupplierInvoiceUCR.SupplierChargeID = newValue; this.originalSupplierInvoiceUCR.IsDirty = true;}


    public get AgentChargeID() { return this.originalSupplierInvoiceUCR ? this.originalSupplierInvoiceUCR.AgentChargeID : null; }
    public set AgentChargeID(newValue: any) {
        this.originalSupplierInvoiceUCR.AgentChargeID = newValue; this.originalSupplierInvoiceUCR.IsDirty = true; }



    ngOnInit(): void {
    }
    SetWindowArgs(args: any) {

        SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice", 0).subscribe(response => {

            SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoicePayment", 0).subscribe(response => {

                SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceUCR", 0).subscribe(response => {

                    if (!AppTool.IsNullOrEmpty(args)) {

                     //this.IsDisplayOnly = args.IsDisplayOnly;
                    if (args.SupplierInvoice != null) {
                         this.originalSupplierInvoice = args.SupplierInvoice;
                        this.clonedSupplierInvoice = this.CloneSupplierInvoice(args.SupplierInvoice);

                        this.BuyerRoleCode = this.originalSupplierInvoice.BuyerRoleCode;
                        this.BuyerName = this.originalSupplierInvoice.BuyerName;
                        this.BuyerCountryCode = this.originalSupplierInvoice.BuyerCountryCode;
                        this.BuyerAddress = this.originalSupplierInvoice.BuyerAddress;
                        this.PartyRelationshipCode = this.originalSupplierInvoice.PartyRelationshipCode;

                        if (args.SupplierInvoice.SupplierInvoicePayments != null && args.SupplierInvoice.SupplierInvoicePayments.length > 0) {
                            this.originalSupplierInvoicePayment = args.SupplierInvoice.SupplierInvoicePayments[0];
                            this.clonedSupplierInvoicePayment = this.CloneSupplierInvoicePayment(args.SupplierInvoice.SupplierInvoicePayments[0]);
                        }
                        else {
                            this.originalSupplierInvoicePayment = new SupplierInvoicePaymentPM(this.originalSupplierInvoice);

                        }
                         if (args.SupplierInvoice.SupplierInvoiceUCRs != null && args.SupplierInvoice.SupplierInvoiceUCRs.length > 0) {
                            this.originalSupplierInvoiceUCR = args.SupplierInvoice.SupplierInvoiceUCRs[0];
                            this.clonedSupplierInvoiceUCR = this.CloneSupplierInvoiceUCR(args.SupplierInvoice.SupplierInvoiceUCRs[0]);
                        }
                        else {
                            this.originalSupplierInvoiceUCR = new SupplierInvoiceUCRPM(this.originalSupplierInvoice);

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
   




    }

   

    CloneSupplierInvoice(entityToClone: SupplierInvoicePM) {

        var clonedEntity: SupplierInvoicePM;
        clonedEntity = new SupplierInvoicePM();

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }

    CloneSupplierInvoicePayment(entityToClone: SupplierInvoicePaymentPM) {

        var clonedEntity: SupplierInvoicePaymentPM;
        clonedEntity = new SupplierInvoicePaymentPM(null);

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }

    CloneSupplierInvoiceUCR(entityToClone: SupplierInvoiceUCRPM) {

        var clonedEntity: SupplierInvoiceUCRPM;
        clonedEntity = new SupplierInvoiceUCRPM(null);

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }


    CancelButtonClicked() {
        if (this.clonedSupplierInvoice != null) {
            this.RejectChanges();
        }
        this.CurrentSession.CloseCurrentWindow();
    }

    RejectChanges() {
        this.MapEntitytoEntity(this.clonedSupplierInvoice, this.originalSupplierInvoice, true);
        this.MapEntitytoEntity(this.clonedSupplierInvoicePayment, this.originalSupplierInvoicePayment, true);
        this.MapEntitytoEntity(this.clonedSupplierInvoiceUCR, this.originalSupplierInvoiceUCR, true);

    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        if (srcEntity == null) return;

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
        if (this.originalSupplierInvoicePayment.DeclarationId == undefined && this.originalSupplierInvoice != undefined) {
            this.originalSupplierInvoicePayment.DeclarationId = this.originalSupplierInvoice.DeclarationId;
            this.originalSupplierInvoicePayment.InvoiceCounterKey = this.originalSupplierInvoice.InvoiceCounterKey;
            this.originalSupplierInvoicePayment.SequenceNumeric = 1;
            this.originalSupplierInvoicePayment.Tenant = SessionLocator.Tenant;
            this.originalSupplierInvoice.AddSupplierInvoicePayment(this.originalSupplierInvoicePayment);
        }
      

        if (this.originalSupplierInvoiceUCR.DeclarationId == undefined && this.originalSupplierInvoice != undefined) {
            this.originalSupplierInvoiceUCR.DeclarationId = this.originalSupplierInvoice.DeclarationId;
            this.originalSupplierInvoiceUCR.InvoiceCounterKey = this.originalSupplierInvoice.InvoiceCounterKey;
            this.originalSupplierInvoiceUCR.SequenceNumeric = 1;
            this.originalSupplierInvoiceUCR.Tenant = SessionLocator.Tenant;
 
            this.originalSupplierInvoice.AddSupplierInvoiceUCR(this.originalSupplierInvoiceUCR);
        }
      

        if (this.originalSupplierInvoiceUCR.IsDirty || this.originalSupplierInvoicePayment.IsDirty) {
            this.originalSupplierInvoice.IsDirty = true;

        }

        //errors = this.ValidateCustomsItemField();

        var errors = [];
        Validator.TryValidateObject(this.originalSupplierInvoice, "Customs.SupplierInvoice", errors);
        Validator.TryValidateObject(this.originalSupplierInvoicePayment, "Customs.SupplierInvoicePayment", errors);
        Validator.TryValidateObject(this.originalSupplierInvoiceUCR, "Customs.SupplierInvoiceUCR", errors);
      //  if (AppTool.IsNullOrEmpty(this.BuyerAddress)) errors.push( "כתובת הקונה שדה חובה");
       // if (AppTool.IsNullOrEmpty(this.BuyerCountryCode)) errors.push("מדינת הקונה שדה חובה");
      //  if (AppTool.IsNullOrEmpty(this.BuyerName)) errors.push("שם הקונה שדה חובה");
        if (AppTool.IsNullOrEmpty(this.PartyRelationshipCode)) errors.push("קוד קשר בעלות שדה חובה");
        //if (AppTool.IsNullOrEmpty(this.BuyerRoleCode)) errors.push("תפקיד הקונה שדה חובה");

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        } else {


            this.CurrentSession.CloseCurrentWindow();
        }


    }



}
