import { Component } from '@angular/core';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { CertificateOfOriginInvoicePM } from 'Customs/EntityPMs/CertificateOfOriginInvoicePM';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { StatusCertificateOfOrigin } from '../../DigitalCertificateOfOriginTabComponent';
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';



@Component({

    templateUrl: './CertificateOfOriginMoreDetailsTabComponent.html',
})

export class CertificateOfOriginMoreDetailsTabComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.CertificateOfOrigin";
    public DataContext = this;
    public entityPM: CertificateOfOriginPM;
    public currentDeclaration: DeclarationPM;
    IsNewOrEdit: StatusCertificateOfOrigin;
    IsDisplayOnly: boolean = false;
    public ErrorsList: string[];

    controlEnabled: boolean;
    constructor() {
        super();
    }

    InitTab(EntityPM: CertificateOfOriginPM, currentDeclaration: DeclarationPM, IsNewOrEdit: StatusCertificateOfOrigin, IsDisplayOnly: boolean) {
        this.entityPM = EntityPM;        
        this.currentDeclaration = currentDeclaration;
        this.IsDisplayOnly = IsDisplayOnly;
        this.IsNewOrEdit = IsNewOrEdit;
        this.controlEnabled = StatusCertificateOfOrigin.IsNew ? true : false;
        this.SetPropertiesEnabled();
        this.SetWarningByCooTypeCode(this.entityPM.CooTypeCode);
    }

    SetPropertiesEnabled() {

        var enabled = !this.IsDisplayOnly;
        // Fields in the First table
        this.UIProperties.SetEnabled("IsCumulation", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("CumulationCountry", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("CumulationGroupOfCountries", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("CityOfDeclaration", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsDeclaredByExporter", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsDeclaredByManufacture", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsExportDecForPrint", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("InsufficentWorkingInd", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsConsigneeForPrint", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsAttachedList", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("InsufficentWorkingText", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("COONumberToCancel", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("Observations", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ConsigneeRemarks", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ReplacementReason", this.ObjectTableName, enabled);

        // Fields in the second table
        this.UIProperties.SetEnabled("NonExportPort", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonExportCountry", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonExportDate", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonPortOfEntrance", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonImportBillOfLadingNum", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonImportDate", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonExitPort", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonExportBillOfLadingNum", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonExpectedExitDate", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonDeclaringCompany", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonDeclaringPerson", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonDeclaringPosition", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonGoodsDescription", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("NonManifestNum", this.ObjectTableName, enabled);


    }

    updateEntity(EntityPM: CertificateOfOriginPM) {
        this.entityPM = EntityPM;
    }
    
    CheckMandatoryFields() {
        if (!this.entityPM.CooTypeCode && !this.entityPM.RequestReasonCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.MandatoryFields')];
        }
        else if (!this.entityPM.CooTypeCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.TypeCodeMandatory')];
        }
        else if (!this.entityPM.RequestReasonCode) {
            this.ErrorsList = [TextCodeTranslator.Translate('Customs.CertificateOfOrigin.O.RequestReasonMandatory')];
        }
        else {
            this.ErrorsList = [];
        }
    }
    mandatoryFielsList = [];
    certificateOfOriginMandatoryFieldsList = [];
    tempCertificateOfOriginMandatoryFieldsList = [];
    certificateOfOriginWebService: CertificateOfOriginWebService = new CertificateOfOriginWebService();
    SetWarningByCooTypeCode(CooTypeCode) {
        if(!CooTypeCode) {
            this.tempCertificateOfOriginMandatoryFieldsList.forEach(i=>{
                this.UIProperties.SetWarning(i.MappedCertificateFieldsName, this.ObjectTableName, false);
            });
            this.tempCertificateOfOriginMandatoryFieldsList = [];
            return;
        }
        this.certificateOfOriginWebService.GetMandatoryFieldsByCooTypeCode(CooTypeCode, this.entityPM.Tenant).subscribe((myResponse: any) => {
            if (!myResponse.HasError) {
                if(this.tempCertificateOfOriginMandatoryFieldsList.length > 0) {
                    this.tempCertificateOfOriginMandatoryFieldsList.forEach(i=>{
                        this.UIProperties.SetWarning(i.MappedCertificateFieldsName, this.ObjectTableName, false);
                    });
                }


                this.certificateOfOriginMandatoryFieldsList = myResponse?.Result;
                
                if (this.certificateOfOriginMandatoryFieldsList.length > 0) {
                    this.certificateOfOriginMandatoryFieldsList.forEach(item => {
                        if (item.IsMandatory) {
                            this.UIProperties.SetWarning(item.MappedCertificateFieldsName, this.ObjectTableName, true);
                        }
                    });

                    this.tempCertificateOfOriginMandatoryFieldsList = this.certificateOfOriginMandatoryFieldsList;
                }
                else{        
                                
                    this.tempCertificateOfOriginMandatoryFieldsList.forEach(i=>{
                        this.UIProperties.SetWarning(i.MappedCertificateFieldsName, this.ObjectTableName, false);
                    });
                    this.tempCertificateOfOriginMandatoryFieldsList = [];
                }

            }
        });
    }

    
    CheckMandatoryCustomsFields(ValidationErrors = []) {
        this.tempCertificateOfOriginMandatoryFieldsList.forEach(item => {
            if(item){
                let field = this.entityPM[item.MappedCertificateFieldsName];
                if (!field){
                    var fieldName = TextCodeTranslator.Translate('Customs.CertificateOfOrigin.F.' + item.MappedCertificateFieldsName);  
                    ValidationErrors.push(fieldName);
                }
            }
        });
    }
    //#region properties

    public get IsCumulation(): boolean {
        return this.entityPM.IsCumulation;
    }
    public set IsCumulation(newValue: boolean) {
        this.entityPM.IsCumulation = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CumulationCountry(): string {
        return this.entityPM.CumulationCountry;
    }
    public set CumulationCountry(newValue: string) {
        this.entityPM.CumulationCountry = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CumulationGroupOfCountries(): string {
        return this.entityPM.CumulationGroupOfCountries;
    }
    public set CumulationGroupOfCountries(newValue: string) {
        this.entityPM.CumulationGroupOfCountries = newValue;
        this.entityPM.IsDirty = true;
    }

    public get CityOfDeclaration(): string {
        return this.entityPM.CityOfDeclaration;
    }
    public set CityOfDeclaration(newValue: string) {
        this.entityPM.CityOfDeclaration = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsDeclaredByExporter(): boolean {
        if (AppTool.IsNullOrEmpty(this.entityPM.IsDeclaredByExporter)) {
            this.entityPM.IsDeclaredByExporter = true;
        }
        return this.entityPM.IsDeclaredByExporter;
    }
    public set IsDeclaredByExporter(newValue: boolean) {
        this.entityPM.IsDeclaredByExporter = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsDeclaredByManufacture(): boolean {
        if (AppTool.IsNullOrEmpty(this.entityPM.IsDeclaredByManufacture)) {
            this.entityPM.IsDeclaredByManufacture = false;
        }
        return this.entityPM.IsDeclaredByManufacture;
    }
    public set IsDeclaredByManufacture(newValue: boolean) {
        this.entityPM.IsDeclaredByManufacture = newValue;
        this.SetWarningByCooTypeCode(this.entityPM.CooTypeCode);
        this.entityPM.IsDirty = true;
    }

    public get IsExportDecForPrint(): boolean {
        if (!this.entityPM.IsExportDecForPrint) {
            this.entityPM.IsExportDecForPrint = false;
        }
        return this.entityPM.IsExportDecForPrint;
    }
    public set IsExportDecForPrint(newValue: boolean) {
        this.entityPM.IsExportDecForPrint = newValue;
        this.entityPM.IsDirty = true;
    }

    public get InsufficentWorkingInd(): boolean {
        return this.entityPM.InsufficentWorkingInd;
    }
    public set InsufficentWorkingInd(newValue: boolean) {
        this.entityPM.InsufficentWorkingInd = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsConsigneeForPrint(): boolean {
        if (AppTool.IsNullOrEmpty(this.entityPM.IsConsigneeForPrint)) {
            this.entityPM.IsConsigneeForPrint = true;
        }
        return this.entityPM.IsConsigneeForPrint;
    }
    public set IsConsigneeForPrint(newValue: boolean) {
        this.entityPM.IsConsigneeForPrint = newValue;
        this.entityPM.IsDirty = true;
    }

    public get IsAttachedList(): boolean {
        return this.entityPM.IsAttachedList;
    }
    public set IsAttachedList(newValue: boolean) {
        this.entityPM.IsAttachedList = newValue;
        this.entityPM.IsDirty = true;
    }

    public get InsufficentWorkingText(): string {
        return this.entityPM.InsufficentWorkingText;
    }
    public set InsufficentWorkingText(newValue: string) {
        this.entityPM.InsufficentWorkingText = newValue;
        this.entityPM.IsDirty = true;
    }

    public get COONumberToCancel(): string {
        return this.entityPM.COONumberToCancel;
    }
    public set COONumberToCancel(newValue: string) {
        this.entityPM.COONumberToCancel = newValue;
        this.entityPM.IsDirty = true;
    }

    public get Observations(): string {
        return this.entityPM.Observations;
    }
    public set Observations(newValue: string) {
        this.entityPM.Observations = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ConsigneeRemarks(): string {
        return this.entityPM.ConsigneeRemarks;
    }
    public set ConsigneeRemarks(newValue: string) {
        this.entityPM.ConsigneeRemarks = newValue;
        this.entityPM.IsDirty = true;
    }

    public get ReplacementReason(): string {
        return this.entityPM.ReplacementReason;
    }
    public set ReplacementReason(newValue: string) {
        this.entityPM.ReplacementReason = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExportPort(): string {
        return this.entityPM.NonExportPort;
    }
    public set NonExportPort(newValue: string) {
        this.entityPM.NonExportPort = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExportCountry(): string {
        return this.entityPM.NonExportCountry;
    }
    public set NonExportCountry(newValue: string) {
        this.entityPM.NonExportCountry = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExportDate(): Date {
        return this.entityPM.NonExportDate;
    }
    public set NonExportDate(newValue: Date) {
        this.entityPM.NonExportDate = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonPortOfEntrance(): string {
        return this.entityPM.NonPortOfEntrance;
    }
    public set NonPortOfEntrance(newValue: string) {
        this.entityPM.NonPortOfEntrance = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonImportBillOfLadingNum(): string {
        return this.entityPM.NonImportBillOfLadingNum;
    }
    public set NonImportBillOfLadingNum(newValue: string) {
        this.entityPM.NonImportBillOfLadingNum = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonImportDate(): Date {
        return this.entityPM.NonImportDate;
    }
    public set NonImportDate(newValue: Date) {
        this.entityPM.NonImportDate = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExitPort(): string {
        return this.entityPM.NonExitPort;
    }
    public set NonExitPort(newValue: string) {
        this.entityPM.NonExitPort = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExportBillOfLadingNum(): string {
        return this.entityPM.NonExportBillOfLadingNum;
    }
    public set NonExportBillOfLadingNum(newValue: string) {
        this.entityPM.NonExportBillOfLadingNum = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonExpectedExitDate(): Date {
        return this.entityPM.NonExpectedExitDate;
    }
    public set NonExpectedExitDate(newValue: Date) {
        this.entityPM.NonExpectedExitDate = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonDeclaringCompany(): string {
        return this.entityPM.NonDeclaringCompany;
    }
    public set NonDeclaringCompany(newValue: string) {
        this.entityPM.NonDeclaringCompany = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonDeclaringPerson(): string {
        return this.entityPM.NonDeclaringPerson;
    }
    public set NonDeclaringPerson(newValue: string) {
        this.entityPM.NonDeclaringPerson = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonDeclaringPosition(): string {
        return this.entityPM.NonDeclaringPosition;
    }
    public set NonDeclaringPosition(newValue: string) {
        this.entityPM.NonDeclaringPosition = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonGoodsDescription(): string {
        return this.entityPM.NonGoodsDescription;
    }
    public set NonGoodsDescription(newValue: string) {
        this.entityPM.NonGoodsDescription = newValue;
        this.entityPM.IsDirty = true;
    }

    public get NonManifestNum(): string {
        return this.entityPM.NonManifestNum;
    }
    public set NonManifestNum(newValue: string) {
        this.entityPM.NonManifestNum = newValue;
        this.entityPM.IsDirty = true;
    }

    //#endregion properties

}
