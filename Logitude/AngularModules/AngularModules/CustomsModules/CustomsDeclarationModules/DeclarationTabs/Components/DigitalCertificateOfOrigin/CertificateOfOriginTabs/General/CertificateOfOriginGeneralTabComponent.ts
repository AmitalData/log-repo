import { Component } from '@angular/core';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';



@Component({

    templateUrl: './CertificateOfOriginGeneralTabComponent.html',
})

export class CertificateOfOriginGeneralTabComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.CertificateOfOrigin";
    public DataContext = this;
    public isCorporation: boolean;
    public isCitizen: boolean;
    public isPassport: boolean;
    public entityPM: CertificateOfOriginPM;
    isNew: boolean;
    controlEnabled: boolean;
    constructor() {
        super();
    }

    InitTab(EntityPM: CertificateOfOriginPM, currentDeclaration: DeclarationPM ,IsNew: boolean) {
        
        // this.entityPM = EntityPM;

        this.isNew = IsNew;
        this.controlEnabled = IsNew;
        this.SetPropertiesEnabled();

        // this.isPassport = (AppTool.IsNullOrEmpty(this.entityPM.Id)) && (!AppTool.IsNullOrEmpty(this.entityPM.PassportNumber));


        if (!AppTool.IsNullOrEmpty(this.entityPM?.Id)) {
            // this.isCorporation = this.entityPM.Code.startsWith("5");
            // this.isCitizen = (!this.entityPM.Code.startsWith("5")) && (this.entityPM.Code != "");
        }

    }

    SetPropertiesEnabled() {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("LocalCorporationName", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("EnglishCorporationName", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("DunsNumber", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("ClientTypeSpecificCode", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("IsActive", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("IsExporter", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("IsImporter", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("LocalFirstName", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("LocalLastName", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("EnglishFirstName", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("EnglishLastName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("BirthDate", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("PassportTypeCode", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("PassportNumber", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("PassportCountryCode", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportFirstName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportLastName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("NationalIdentificationNumber", this.ObjectTableName, this.controlEnabled);

        this.UIProperties.SetEnabled("EnglishFatherName", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("EnglishBirthPlace", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportIssueDate", this.ObjectTableName, this.controlEnabled);
        this.UIProperties.SetEnabled("PassportExpirationDate", this.ObjectTableName, this.controlEnabled);




    }


    //#region properties

    public get Id(): string {
        return this.entityPM.Id;
    }
    public set Id(newValue: string) {
        this.entityPM.Id = newValue;
    }

    public get Tenant(): number {
        return this.entityPM.Tenant;
    }
    public set Tenant(newValue: number) {
        this.entityPM.Tenant = newValue;
    }

    public get SearchFields(): string {
        return this.entityPM.SearchFields;
    }
    public set SearchFields(newValue: string) {
        this.entityPM.SearchFields = newValue;
    }

    public get Counter(): string {
        return this.entityPM.Counter;
    }
    public set Counter(newValue: string) {
        this.entityPM.Counter = newValue;
    }
    
    
    
    _cooTypeCode:string;
    public get CooTypeCode(): string {
        return this._cooTypeCode;
        // return this.entityPM.CooTypeCode;
    }
    public set CooTypeCode(newValue: string) {
        this._cooTypeCode = newValue;
        // this.entityPM.CooTypeCode = newValue;
    }
    
    _RequestReasonCode:string;
    public get RequestReasonCode(): string {
        return this._RequestReasonCode;
        // return this.entityPM.RequestReasonCode;
    }
    public set RequestReasonCode(newValue: string) {
        this._RequestReasonCode = newValue;
        // this.entityPM.RequestReasonCode = newValue;
    }

    public get COONumber(): string {
        return this.entityPM.COONumber;
    }
    public set COONumber(newValue: string) {
        this.entityPM.COONumber = newValue;
    }

    public get COONumberToCancel(): string {
        return this.entityPM.COONumberToCancel;
    }
    public set COONumberToCancel(newValue: string) {
        this.entityPM.COONumberToCancel = newValue;
    }

    public get ReplacementReason(): string {
        return this.entityPM.ReplacementReason;
    }
    public set ReplacementReason(newValue: string) {
        this.entityPM.ReplacementReason = newValue;
    }

    public get DeclarationNumber(): string {
        return this.entityPM.DeclarationNumber;
    }
    public set DeclarationNumber(newValue: string) {
        this.entityPM.DeclarationNumber = newValue;
    }

    public get ExporterVat(): string {
        return this.entityPM.ExporterVat;
    }
    public set ExporterVat(newValue: string) {
        this.entityPM.ExporterVat = newValue;
    }

    _ExporterName: string;
    public get ExporterName(): string {
        return this._ExporterName;
        // return this.entityPM.ExporterName;
    }
    public set ExporterName(newValue: string) {
        // this.entityPM.ExporterName = newValue;
        this._ExporterName = newValue;
    }

    _ExporterAddress: string;
    public get ExporterAddress(): string {
        return this._ExporterAddress; 
        // return this.entityPM.ExporterAddress ? this.entityPM.ExporterAddress: ""; 
    }
    public set ExporterAddress(newValue: string) {
        this._ExporterAddress = newValue;
        // this.entityPM.ExporterAddress = newValue;
    }

    _ExporterCountry:string;
    public get ExporterCountry(): string {
        return  this._ExporterCountry;
        // return this.entityPM.ExporterCountry;
    }
    public set ExporterCountry(newValue: string) {
        // this.entityPM.ExporterCountry = newValue;
        this._ExporterCountry = newValue;
    }

    public get TradeAgreementCountry1(): string {
        return this.entityPM.TradeAgreementCountry1;
    }
    public set TradeAgreementCountry1(newValue: string) {
        this.entityPM.TradeAgreementCountry1 = newValue;
    }

    public get TradeAgreementCountry2(): string {
        return this.entityPM.TradeAgreementCountry2;
    }
    public set TradeAgreementCountry2(newValue: string) {
        this.entityPM.TradeAgreementCountry2 = newValue;
    }

    public get TradeAgreementGroupOfCountries(): string {
        return this.entityPM.TradeAgreementGroupOfCountries;
    }
    public set TradeAgreementGroupOfCountries(newValue: string) {
        this.entityPM.TradeAgreementGroupOfCountries = newValue;
    }

    _ConsigneeName:string;
    public get ConsigneeName(): string {
        // return this.entityPM.ConsigneeName;
        return this._ConsigneeName;
    }
    public set ConsigneeName(newValue: string) {
        // this.entityPM.ConsigneeName = newValue;
        this._ConsigneeName = newValue;
    }
    
    _ConsigneeAddress:string;
    public get ConsigneeAddress(): string {
        // return this.entityPM.ConsigneeAddress;
        return this._ConsigneeAddress;
    }
    public set ConsigneeAddress(newValue: string) {
        // this.entityPM.ConsigneeAddress = newValue;
        this._ConsigneeAddress = newValue;
    }

    _ConsigneeCountry:string;
    public get ConsigneeCountry(): string {
        return this._ConsigneeCountry;
        // return this.entityPM.ConsigneeCountry;
    }
    public set ConsigneeCountry(newValue: string) {
        // this.entityPM.ConsigneeCountry = newValue;
        this._ConsigneeCountry = newValue;
    }

    public get ConsigneeRemarks(): string {
        return this.entityPM.ConsigneeRemarks;
    }
    public set ConsigneeRemarks(newValue: string) {
        this.entityPM.ConsigneeRemarks = newValue;
    }

    public get IsConsigneeForPrint(): boolean {
        return this.entityPM.IsConsigneeForPrint;
    }
    public set IsConsigneeForPrint(newValue: boolean) {
        this.entityPM.IsConsigneeForPrint = newValue;
    }

    _OriginCountry:string;
    public get OriginCountry(): string {
        return this._OriginCountry;
        // return this.entityPM.OriginCountry;
    }
    public set OriginCountry(newValue: string) {
        // this.entityPM.OriginCountry = newValue;
        this._OriginCountry = newValue;
    }

    _OriginGroupOfCountry:string;
    public get OriginGroupOfCountry(): string {
        // return this.entityPM.OriginGroupOfCountry;
        return this._OriginGroupOfCountry;
    }
    public set OriginGroupOfCountry(newValue: string) {
        // this.entityPM.OriginGroupOfCountry = newValue;
        this._OriginGroupOfCountry = newValue;

    }


    _DestinationCountry:string;
    public get DestinationCountry(): string {
        // return this.entityPM.DestinationCountry;
        return this._DestinationCountry;
    }
    public set DestinationCountry(newValue: string) {
        // this.entityPM.DestinationCountry = newValue;
        this._DestinationCountry = newValue;
    }

    public get DestinationGroupOfCountries(): string {
        return this.entityPM.DestinationGroupOfCountries;
    }
    public set DestinationGroupOfCountries(newValue: string) {
        this.entityPM.DestinationGroupOfCountries = newValue;
    }

    public get Transport(): string {
        return this.entityPM.Transport;
    }
    public set Transport(newValue: string) {
        this.entityPM.Transport = newValue;
    }

    public get PortOfShipment(): string {
        return this.entityPM.PortOfShipment;
    }
    public set PortOfShipment(newValue: string) {
        this.entityPM.PortOfShipment = newValue;
    }

    public get IsCumulation(): boolean {
        return this.entityPM.IsCumulation;
    }
    public set IsCumulation(newValue: boolean) {
        this.entityPM.IsCumulation = newValue;
    }

    public get CumulationCountry(): string {
        return this.entityPM.CumulationCountry;
    }
    public set CumulationCountry(newValue: string) {
        this.entityPM.CumulationCountry = newValue;
    }

    public get CumulationGroupOfCountries(): string {
        return this.entityPM.CumulationGroupOfCountries;
    }
    public set CumulationGroupOfCountries(newValue: string) {
        this.entityPM.CumulationGroupOfCountries = newValue;
    }

    public get PlaceOfManufacture(): string {
        return this.entityPM.PlaceOfManufacture;
    }
    public set PlaceOfManufacture(newValue: string) {
        this.entityPM.PlaceOfManufacture = newValue;
    }

    public get ZipCodeOfManufacture(): string {
        return this.entityPM.ZipCodeOfManufacture;
    }
    public set ZipCodeOfManufacture(newValue: string) {
        this.entityPM.ZipCodeOfManufacture = newValue;
    }

    public get Observations(): string {
        return this.entityPM.Observations;
    }
    public set Observations(newValue: string) {
        this.entityPM.Observations = newValue;
    }

    public get IsExportDecForPrint(): boolean {
        return this.entityPM.IsExportDecForPrint;
    }
    public set IsExportDecForPrint(newValue: boolean) {
        this.entityPM.IsExportDecForPrint = newValue;
    }

    public get IsUnitedInvoices(): boolean {
        return this.entityPM.IsUnitedInvoices;
    }
    public set IsUnitedInvoices(newValue: boolean) {
        this.entityPM.IsUnitedInvoices = newValue;
    }

    public get CustomsHouse(): string {
        return this.entityPM.CustomsHouse;
    }
    public set CustomsHouse(newValue: string) {
        this.entityPM.CustomsHouse = newValue;
    }

    public get IssuingCountry(): string {
        return this.entityPM.IssuingCountry;
    }
    public set IssuingCountry(newValue: string) {
        this.entityPM.IssuingCountry = newValue;
    }

    public get CityOfDeclaration(): string {
        return this.entityPM.CityOfDeclaration;
    }
    public set CityOfDeclaration(newValue: string) {
        this.entityPM.CityOfDeclaration = newValue;
    }

    public get CountryOfDeclaration(): string {
        return this.entityPM.CountryOfDeclaration;
    }
    public set CountryOfDeclaration(newValue: string) {
        this.entityPM.CountryOfDeclaration = newValue;
    }

    _DateOfDeclaration:Date;
    public get DateOfDeclaration(): Date {
        
        if(!this._DateOfDeclaration){
            var todayDate = DateTool.GetCurrentDateAsUtc();
            this._DateOfDeclaration = todayDate;
            return this._DateOfDeclaration;
        }
        return this._DateOfDeclaration;
        // return this.entityPM.DateOfDeclaration;
    }
    public set DateOfDeclaration(newValue: Date) {
        // this.entityPM.DateOfDeclaration = newValue;
        
        
        
        // if(this.entityPM.DateOfDeclaration){
        //     this.entityPM.DateOfDeclaration = newValue;
        // }
        this._DateOfDeclaration = newValue;

        
    }

    public get IsDeclaredByManufacture(): boolean {
        return this.entityPM.IsDeclaredByManufacture;
    }
    public set IsDeclaredByManufacture(newValue: boolean) {
        this.entityPM.IsDeclaredByManufacture = newValue;
    }

    public get IsDeclaredByExporter(): boolean {
        return this.entityPM.IsDeclaredByExporter;
    }
    public set IsDeclaredByExporter(newValue: boolean) {
        this.entityPM.IsDeclaredByExporter = newValue;
    }

    public get IsAttachedList(): boolean {
        return this.entityPM.IsAttachedList;
    }
    public set IsAttachedList(newValue: boolean) {
        this.entityPM.IsAttachedList = newValue;
    }

    public get InsufficentWorkingInd(): boolean {
        return this.entityPM.InsufficentWorkingInd;
    }
    public set InsufficentWorkingInd(newValue: boolean) {
        this.entityPM.InsufficentWorkingInd = newValue;
    }

    public get InsufficentWorkingText(): string {
        return this.entityPM.InsufficentWorkingText;
    }
    public set InsufficentWorkingText(newValue: string) {
        this.entityPM.InsufficentWorkingText = newValue;
    }

    public get NonExportDate(): Date {
        return this.entityPM.NonExportDate;
    }
    public set NonExportDate(newValue: Date) {
        this.entityPM.NonExportDate = newValue;
    }

    public get NonExportCountry(): string {
        return this.entityPM.NonExportCountry;
    }
    public set NonExportCountry(newValue: string) {
        this.entityPM.NonExportCountry = newValue;
    }

    public get NonImportBillOfLadingNum(): string {
        return this.entityPM.NonImportBillOfLadingNum;
    }
    public set NonImportBillOfLadingNum(newValue: string) {
        this.entityPM.NonImportBillOfLadingNum = newValue;
    }

    public get NonExportPort(): string {
        return this.entityPM.NonExportPort;
    }
    public set NonExportPort(newValue: string) {
        this.entityPM.NonExportPort = newValue;
    }

    public get NonImportDate(): Date {
        return this.entityPM.NonImportDate;
    }
    public set NonImportDate(newValue: Date) {
        this.entityPM.NonImportDate = newValue;
    }

    public get NonExportBillOfLadingNum(): string {
        return this.entityPM.NonExportBillOfLadingNum;
    }
    public set NonExportBillOfLadingNum(newValue: string) {
        this.entityPM.NonExportBillOfLadingNum = newValue;
    }

    public get NonTransirCountry(): string {
        return this.entityPM.NonTransirCountry;
    }
    public set NonTransirCountry(newValue: string) {
        this.entityPM.NonTransirCountry = newValue;
    }

    public get NonPortOfEntrance(): string {
        return this.entityPM.NonPortOfEntrance;
    }
    public set NonPortOfEntrance(newValue: string) {
        this.entityPM.NonPortOfEntrance = newValue;
    }

    public get NonExpectedExitDate(): Date {
        return this.entityPM.NonExpectedExitDate;
    }
    public set NonExpectedExitDate(newValue: Date) {
        this.entityPM.NonExpectedExitDate = newValue;
    }

    public get NonExitPort(): string {
        return this.entityPM.NonExitPort;
    }
    public set NonExitPort(newValue: string) {
        this.entityPM.NonExitPort = newValue;
    }

    public get NonGoodsDescription(): string {
        return this.entityPM.NonGoodsDescription;
    }
    public set NonGoodsDescription(newValue: string) {
        this.entityPM.NonGoodsDescription = newValue;
    }

    public get NonDeclaringCompany(): string {
        return this.entityPM.NonDeclaringCompany;
    }
    public set NonDeclaringCompany(newValue: string) {
        this.entityPM.NonDeclaringCompany = newValue;
    }

    public get NonDeclaringPerson(): string {
        return this.entityPM.NonDeclaringPerson;
    }
    public set NonDeclaringPerson(newValue: string) {
        this.entityPM.NonDeclaringPerson = newValue;
    }

    public get NonDeclaringPosition(): string {
        return this.entityPM.NonDeclaringPosition;
    }
    public set NonDeclaringPosition(newValue: string) {
        this.entityPM.NonDeclaringPosition = newValue;
    }

    public get NonManifestNum(): string {
        return this.entityPM.NonManifestNum;
    }
    public set NonManifestNum(newValue: string) {
        this.entityPM.NonManifestNum = newValue;
    }

    public get ErrXml(): string {
        return this.entityPM.ErrXml;
    }
    public set ErrXml(newValue: string) {
        this.entityPM.ErrXml = newValue;
    }

    public get CooStatusCode(): string {
        return this.entityPM.CooStatusCode;
    }
    public set CooStatusCode(newValue: string) {
        this.entityPM.CooStatusCode = newValue;
    }
    public get FeedbackRemark(): string {
        return this.entityPM.FeedbackRemark;
    }
    public set FeedbackRemark(newValue: string) {
        this.entityPM.FeedbackRemark = newValue;
    }

    public get RejectCancelReason(): string {
        return this.entityPM.RejectCancelReason;
    }
    public set RejectCancelReason(newValue: string) {
        this.entityPM.RejectCancelReason = newValue;
    }

    public get IssueDateIfReleased(): Date {
        return this.entityPM.IssueDateIfReleased;
    }
    public set IssueDateIfReleased(newValue: Date) {
        this.entityPM.IssueDateIfReleased = newValue;
    }

    public get QueryUrl(): string {
        return this.entityPM.QueryUrl;
    }
    public set QueryUrl(newValue: string) {
        this.entityPM.QueryUrl = newValue;
    }

    public get CooPdf(): string {
        return this.entityPM.CooPdf;
    }
    public set CooPdf(newValue: string) {
        this.entityPM.CooPdf = newValue;
    }

    public get CoodPdf1(): string {
        return this.entityPM.CoodPdf1;
    }
    public set CoodPdf1(newValue: string) {
        this.entityPM.CoodPdf1 = newValue;
    }

    public get OpenByUser(): string {
        return this.entityPM.OpenByUser;
    }
    public set OpenByUser(newValue: string) {
        this.entityPM.OpenByUser = newValue;
    }

    public get IsSubmitted(): boolean {
        return this.entityPM.IsSubmitted;
    }
    public set IsSubmitted(newValue: boolean) {
        this.entityPM.IsSubmitted = newValue;
    }
    //#endregio

}
