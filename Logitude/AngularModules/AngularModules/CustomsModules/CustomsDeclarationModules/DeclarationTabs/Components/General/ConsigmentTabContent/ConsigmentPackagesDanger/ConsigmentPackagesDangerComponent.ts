import { OnDestroy, Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../../../../Infrastructure/Tools';
import { ConsignmentPackDangerPM } from '../../../../../../../Customs/EntityPMs/ConsignmentPackDangerPM';
import { DecDangersContactPM } from '../../../../../../../Customs/EntityPMs/DecDangersContactPM';
import { SessionLocator } from '../../../../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../../../../Infrastructure/Validators/Validator';
import { DeclarationValidator } from '../../../../../../../Customs/Validators/DeclarationValidator';
import { ConsignmentPackagePM } from '../../../../../../../Customs/EntityPMs/ConsignmentPackagePM';
import { DeclarationPM } from '../../../../../../../Customs/EntityPMs/DeclarationPM';
import { TextCodeTranslator } from '../../../../../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    selector: 'ConsigmentPackagesDangerComponent',    
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
    public IsDisplayOnly: boolean;
    public IsDisplayOnlyContact: boolean;

    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: any[] = [];
    public declarationValidator: DeclarationValidator = new DeclarationValidator();
    public Package: ConsignmentPackagePM;
    public Declaration: DeclarationPM;

    public get UNCode() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.UNCode : null; }
    public set UNCode(newValue: string) { this.OriginalConsignmentPackDangerPM.UNCode = newValue; }

    public get DangerousGoodsPackingReqCode() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.DangerousGoodsPackingReqCode : null; }
    public set DangerousGoodsPackingReqCode(newValue: string) { this.OriginalConsignmentPackDangerPM.DangerousGoodsPackingReqCode = newValue; }

    public get FlashpointTemperature() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.FlashpointTemperature : null; }
    public set FlashpointTemperature(newValue: string) { this.OriginalConsignmentPackDangerPM.FlashpointTemperature = newValue; }

    public get StorageTemperature() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.StorageTemperature : null; }
    public set StorageTemperature(newValue: string) { this.OriginalConsignmentPackDangerPM.StorageTemperature = newValue; }

    public get ClassificationFourDigit() { return this.OriginalConsignmentPackDangerPM ? this.OriginalConsignmentPackDangerPM.ClassificationFourDigit : null; }
    public set ClassificationFourDigit(newValue: string) { this.OriginalConsignmentPackDangerPM.ClassificationFourDigit = newValue; }


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



    constructor() {
        super();

    }

    ngOnInit(): void {
    }
    SetWindowArgs(args: any) {

        SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName("Customs.DecDangersContact", 0).subscribe(response => {

            SessionLocator.SelectedSession.entityResourceService.getEntityResourceByTableName("Customs.ConsignmentPackDanger", 0).subscribe(response => {

                if (!AppTool.IsNullOrEmpty(args)) {

                    this.IsDisplayOnly = args.IsDisplayOnly;
                    this.IsDisplayOnlyContact = args.IsDisplayOnlyContact;
                    if (args.ConsignmentPackagesDangerPM.consignmentPackDangers != undefined && args.ConsignmentPackagesDangerPM.consignmentPackDangers.length > 0) {
                        this.OriginalConsignmentPackDangerPM = args.ConsignmentPackagesDangerPM.consignmentPackDangers[0];
                        this.ClonedConsignmentPackDangerPM = this.CloneConsignmentPackDangerPM(args.ConsignmentPackagesDangerPM.consignmentPackDangers[0]);
                    }
                    else {
                        this.OriginalConsignmentPackDangerPM = new ConsignmentPackDangerPM(args.Parent.EntityPM);
                        this.Package = args.Parent.EntityPM;
                    }
                    if (args.Declaration.DecDangersContacts != undefined && args.Declaration.DecDangersContacts.length > 0) {

                        this.OriginalDecDangersContactPM = args.Declaration.DecDangersContacts[0];
                        this.ClonedDecDangersContactPM = this.CloneDecDangersContactPM(args.Declaration.DecDangersContacts[0]);
                    }

                    else {
                        this.OriginalDecDangersContactPM = new DecDangersContactPM(null);
                        this.Declaration = args.Declaration;

                    }
                    this.finishedLoad = true;
                    this.SetScreenFieldsEditability();
                    this.SetContactFieldsEditability();

                }

            });

        });




    }

    CloneDecDangersContactPM(entityToClone: DecDangersContactPM) {

        var clonedEntity: DecDangersContactPM;
        clonedEntity = new DecDangersContactPM(entityToClone.EntityParentPM);

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }


    CloneConsignmentPackDangerPM(entityToClone: ConsignmentPackDangerPM) {

        var clonedEntity: ConsignmentPackDangerPM;
        clonedEntity = new ConsignmentPackDangerPM(entityToClone.EntityParentPM);

        this.MapEntitytoEntity(entityToClone, clonedEntity);
        return clonedEntity;
    }


    CancelButtonClicked() {
        if (this.ClonedConsignmentPackDangerPM != undefined) {
            this.RejectChanges();
        }
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

        if (this.OriginalConsignmentPackDangerPM.DeclarationId == undefined && this.Package != undefined) {
            this.OriginalConsignmentPackDangerPM.DeclarationId = "-1";
            this.OriginalConsignmentPackDangerPM.LineNumber = -1;
            this.OriginalConsignmentPackDangerPM.ConsignmentNumber = -1;
            this.OriginalConsignmentPackDangerPM.DangerousLineNo = 1;
            this.OriginalConsignmentPackDangerPM.Tenant = SessionLocator.Tenant;
            this.Package.AddConsignmentPackDanger(this.OriginalConsignmentPackDangerPM);

        }
        if (this.OriginalDecDangersContactPM.DeclarationId == undefined && this.Declaration != undefined) {
            this.OriginalDecDangersContactPM.DeclarationId = this.Declaration.Id;
            this.OriginalDecDangersContactPM.Tenant = SessionLocator.Tenant;
            this.Declaration.AddDecDangersContact(this.OriginalDecDangersContactPM);
        }

        errors = this.ValidateCustomsItemField();


        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        } else {


            this.CurrentSession.CloseCurrentWindow();
        }


    }
    ValidateCustomsItemField() {
        var errors = [];
        this.declarationValidator.validatePackagesDanger(this.OriginalDecDangersContactPM, this.OriginalConsignmentPackDangerPM);
        if (!this.validationTemperature(this.OriginalConsignmentPackDangerPM.FlashpointTemperature))
            errors.push(TextCodeTranslator.Translate("Customs.ConsignmentPackDanger.O.FlashpointTemperature") + "-" + TextCodeTranslator.Translate("Customs.General.O.PackageDangerTempValid"));
        if (!this.validationTemperature(this.OriginalConsignmentPackDangerPM.StorageTemperature))
            errors.push(TextCodeTranslator.Translate("Customs.ConsignmentPackDanger.O.StorageTemperature") + "-" + TextCodeTranslator.Translate("Customs.General.O.PackageDangerTempValid"));

        return errors;
    }

    ngOnDestroy(): void {
    }


    validationTemperature(temperature) {
        var re = /^[\d\-+]+$/m;

        if (re.exec(temperature) !== null || temperature == null)
            return true;

        return false;
    }


    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("UNCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ClassificationFourDigit", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DangerousGoodsPackingReqCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("FlashpointTemperature", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StorageTemperature", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CompanyName", this.ObjectTableNameContact, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CompanyCommNumber", this.ObjectTableNameContact, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CompanyCommTypeCode", this.ObjectTableNameContact, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ContactCommNumber", this.ObjectTableNameContact, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ContactName", this.ObjectTableNameContact, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ContactCommTypeCode", this.ObjectTableNameContact, !this.IsDisplayOnly);

    }

    SetContactFieldsEditability() {
        if (this.IsDisplayOnly) this.IsDisplayOnlyContact = true;
        this.UIProperties.SetEnabled("CompanyName", this.ObjectTableNameContact, !this.IsDisplayOnlyContact);
        this.UIProperties.SetEnabled("CompanyCommNumber", this.ObjectTableNameContact, !this.IsDisplayOnlyContact);
        this.UIProperties.SetEnabled("CompanyCommTypeCode", this.ObjectTableNameContact, !this.IsDisplayOnlyContact);
        this.UIProperties.SetEnabled("ContactCommNumber", this.ObjectTableNameContact, !this.IsDisplayOnlyContact);
        this.UIProperties.SetEnabled("ContactName", this.ObjectTableNameContact, !this.IsDisplayOnlyContact);
        this.UIProperties.SetEnabled("ContactCommTypeCode", this.ObjectTableNameContact, !this.IsDisplayOnlyContact);
    }


}
