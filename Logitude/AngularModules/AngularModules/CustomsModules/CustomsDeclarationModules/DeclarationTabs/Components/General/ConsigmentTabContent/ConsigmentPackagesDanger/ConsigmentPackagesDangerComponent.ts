import { OnDestroy, Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../../../../Infrastructure/Tools';
import { ConsignmentPackDangerPM } from '../../../../../../../Customs/EntityPMs/ConsignmentPackDangerPM';
import { DecDangersContactPM } from '../../../../../../../Customs/EntityPMs/DecDangersContactPM';
import { SessionLocator } from '../../../../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../../../../Infrastructure/Validators/Validator';
import { DeclarationValidator } from '../../../../../../../Customs/Validators/DeclarationValidator';


@Component({
    selector: 'ConsigmentPackagesDangerComponent',
    moduleId: module.id,
    templateUrl: './ConsigmentPackagesDangerComponent.html',
})

export class ConsigmentPackagesDangerComponent
    extends BaseComponent
    implements OnDestroy, OnInit {
    
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.ConsignmentPackDanger";
    public ObjectTableNameContact: string = "Customs.DecDangersContact";
    public finishedLoad: boolean;
    public OriginalConsignmentPackDangerPM: ConsignmentPackDangerPM;
    public ClonedConsignmentPackDangerPM: ConsignmentPackDangerPM; 

    public OriginalDecDangersContactPM: DecDangersContactPM;
    public ClonedDecDangersContactPM: DecDangersContactPM; 

    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: any[] = [];
    public declarationValidator: DeclarationValidator = new DeclarationValidator();



    public get UNCode() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.UNCode : null; }
    public set UNCode(newValue: string) { this.OriginalConsignmentPackDangerPM.UNCode = newValue; }

    public get DangerousGoodsPackingReqCode() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.DangerousGoodsPackingReqCode : null; }
    public set DangerousGoodsPackingReqCode(newValue: string) { this.OriginalConsignmentPackDangerPM.DangerousGoodsPackingReqCode = newValue; }

    public get FlashpointTemperature() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.FlashpointTemperature : null; }
    public set FlashpointTemperature(newValue: string) { this.OriginalConsignmentPackDangerPM.FlashpointTemperature = newValue; }

    public get StorageTemperature() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.StorageTemperature : null; }
    public set StorageTemperature(newValue: string) { this.OriginalConsignmentPackDangerPM.StorageTemperature = newValue; }

    public get CompanyName() { return this.OriginalDecDangersContactPM ? this.OriginalDecDangersContactPM.CompanyName : null; }
    public set CompanyName(newValue: string) { this.OriginalDecDangersContactPM.CompanyName = newValue; }


    public get CompanyCommNumber() { return this.OriginalDecDangersContactPM ? this.OriginalDecDangersContactPM.CompanyCommNumber : null; }
    public set CompanyCommNumber(newValue: string) { this.OriginalDecDangersContactPM.CompanyCommNumber = newValue; }

    public get CompanyCommTypeCode() { return this.OriginalDecDangersContactPM ? this.OriginalDecDangersContactPM.CompanyCommTypeCode : null; }
    public set CompanyCommTypeCode(newValue: string) { this.OriginalDecDangersContactPM.CompanyCommTypeCode = newValue; }

    public get ContactName() { return this.OriginalDecDangersContactPM ? this.OriginalDecDangersContactPM.ContactName : null; }
    public set ContactName(newValue: string) { this.OriginalDecDangersContactPM.ContactName = newValue; }

    public get ContactCommNumber() { return this.OriginalDecDangersContactPM ? this.OriginalDecDangersContactPM.ContactCommNumber : null; }
    public set ContactCommNumber(newValue: string) { this.OriginalDecDangersContactPM.ContactCommNumber = newValue; }

    public get ContactCommTypeCode() { return this.OriginalDecDangersContactPM ? this.OriginalDecDangersContactPM.ContactCommTypeCode : null; }
    public set ContactCommTypeCode(newValue: string) { this.OriginalDecDangersContactPM.ContactCommTypeCode = newValue; }

    //public get DeclarationId() { return this.OriginalDecDangersContactPM ? this.OriginalDecDangersContactPM.DeclarationId : null; }
    //public set DeclarationId(newValue: string) {
    //    debugger;
    //    this.OriginalDecDangersContactPM.DeclarationId = this.args2.Parent.DeclarationId;
    //}

    constructor() {
        super();

    }

    ngOnInit(): void {
    }
    public args2: any;
    SetWindowArgs(args: any) {

        SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName("Customs.DecDangersContact", 0).subscribe(response => {

            SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName("Customs.ConsignmentPackDanger", 0).subscribe(response => {
                if (!AppTool.IsNullOrEmpty(args)) {
                    this.args2 = args;
                    if (args.ConsignmentPackagesDangerPM.consignmentPackDangers != undefined) {
                        this.OriginalConsignmentPackDangerPM = args.ConsignmentPackagesDangerPM.consignmentPackDangers[0];
                        this.ClonedConsignmentPackDangerPM = this.CloneConsignmentPackDangerPM(args.ConsignmentPackagesDangerPM.consignmentPackDangers[0]);
                    }
                   else

                    {
                         this.OriginalConsignmentPackDangerPM = new ConsignmentPackDangerPM(null);
                        //this.OriginalConsignmentPackDangerPM.DeclarationId = args.Parent.declarationId;
                        //this.OriginalConsignmentPackDangerPM.ConsignmentNumber = args.ConsignmentPackagesDangerPM.consignmentNumber;
                        //this.OriginalConsignmentPackDangerPM.LineNumber = args.ConsignmentPackagesDangerPM.lineNumber;
                   }
                    if (args.Declaration.DecDangersContacts != undefined) {

                        this.OriginalDecDangersContactPM = args.Declaration.DecDangersContacts[0];
                        this.ClonedDecDangersContactPM = this.CloneDecDangersContactPM(args.Declaration.DecDangersContacts[0]);
                    }
                   this. finishedLoad = true;

                }


            });

        });




     }

    CloneDecDangersContactPM(entityToClone: DecDangersContactPM) {

        var clonedEntity: DecDangersContactPM;
        clonedEntity = new DecDangersContactPM(entityToClone.EntityParentPM); // check it !!

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }


    CloneConsignmentPackDangerPM(entityToClone: ConsignmentPackDangerPM) {

        var clonedEntity: ConsignmentPackDangerPM;
        clonedEntity = new ConsignmentPackDangerPM(entityToClone.EntityParentPM); // check it !!

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }


    CancelButtonClicked() {
        if (this.ClonedConsignmentPackDangerPM != undefined) {
     this.RejectChanges();}
        this.CurrentSession.CloseCurrentWindow();
    }

    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedConsignmentPackDangerPM, this.OriginalConsignmentPackDangerPM, true);
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

        debugger;
        errors = this.declarationValidator.validatePackagesDanger(this.OriginalDecDangersContactPM, this.OriginalConsignmentPackDangerPM);

      //  errors = this.ValidateCustomsItemField();

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        } else {
            this.CurrentSession.CloseCurrentWindow();
        }

        //if (!isValid)
        //    return;

 

      //  this.ValidationErrorsList = errors;
      ////  errors = this.declarationValidator.ValidateSupplierInvoiceItem(this.OriginalConsignmentPackDangerPM);

      //  if (errors.length > 0) {
      //      this.ValidationErrorsList = errors;
      //  } else {
      //      this.CurrentSession.CloseCurrentWindow();
      //  }
    }
    ValidateCustomsItemField() {
        var errors = [];

        Validator.TryValidateObject(this.OriginalDecDangersContactPM, this.ObjectTableNameContact, errors);
        Validator.TryValidateObject(this.OriginalConsignmentPackDangerPM, this.ObjectTableName, errors);

        return errors;
        // throw new Error("Method not implemented.");
    }

    ngOnDestroy(): void {
       // throw new Error("Method not implemented.");
    }

}
