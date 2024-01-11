import { Component } from '@angular/core';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { CertificateOfOriginInvoicePM } from 'Customs/EntityPMs/CertificateOfOriginInvoicePM';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { StatusCertificateOfOrigin } from '../../DigitalCertificateOfOriginTabComponent';



@Component({

    templateUrl: './CertificateOfOriginMoreDetailsTabComponent.html',
})

export class CertificateOfOriginMoreDetailsTabComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.CertificateOfOrigin";
    public DataContext = this;
    public entityPM: CertificateOfOriginPM;
    public currentDeclaration: DeclarationPM;
    IsNewOrEdit: StatusCertificateOfOrigin;
    controlEnabled: boolean;
    isDispalyOnlyStatusList: number[] = [4, 8];
    constructor() {
        super();
    }

    InitTab(EntityPM: CertificateOfOriginPM, currentDeclaration: DeclarationPM, IsNewOrEdit: StatusCertificateOfOrigin) {
        this.entityPM = EntityPM;
        this.currentDeclaration = currentDeclaration;
        
        this.IsNewOrEdit = IsNewOrEdit;
        this.controlEnabled = StatusCertificateOfOrigin.IsNew ? true:false;
        this.SetPropertiesEnabled();
    }

    SetPropertiesEnabled() {

        // ADD to do Name FILDES 101507
        var enabled = !this.IsDispalyOnly;
        this.UIProperties.SetEnabled("CooTypeCode", this.ObjectTableName, enabled);



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


    public get IsDispalyOnly() {
        return this.isDispalyOnlyStatusList.includes(Number(this.entityPM.CooStatusCode))
    }


    //#region properties

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

    public get CityOfDeclaration(): string {
        return this.entityPM.CityOfDeclaration;
    }
    public set CityOfDeclaration(newValue: string) {
        this.entityPM.CityOfDeclaration = newValue;
    }

    public get IsDeclaredByExporter(): boolean {
        if(!this.entityPM.IsDeclaredByExporter){
            this.entityPM.IsDeclaredByExporter = true;
        }
        return this.entityPM.IsDeclaredByExporter;
    }
    public set IsDeclaredByExporter(newValue: boolean) {
        this.entityPM.IsDeclaredByExporter = newValue;
    }

    public get IsDeclaredByManufacture(): boolean {
        if(!this.entityPM.IsDeclaredByManufacture){
            this.entityPM.IsDeclaredByManufacture = false;
        }
        return this.entityPM.IsDeclaredByManufacture;
    }
    public set IsDeclaredByManufacture(newValue: boolean) {
        this.entityPM.IsDeclaredByManufacture = newValue;
    }

    public get IsExportDecForPrint(): boolean {
        if(!this.entityPM.IsExportDecForPrint){
            this.entityPM.IsExportDecForPrint = false;
        }
        return this.entityPM.IsExportDecForPrint;
    }
    public set IsExportDecForPrint(newValue: boolean) {
        this.entityPM.IsExportDecForPrint = newValue;
    }

    public get InsufficentWorkingInd(): boolean {
        return this.entityPM.InsufficentWorkingInd;
    }
    public set InsufficentWorkingInd(newValue: boolean) {
        this.entityPM.InsufficentWorkingInd = newValue;
    }

    public get IsConsigneeForPrint(): boolean {
        if(!this.entityPM.IsConsigneeForPrint){
            this.entityPM.IsConsigneeForPrint = true;
        }
        return this.entityPM.IsConsigneeForPrint;
    }
    public set IsConsigneeForPrint(newValue: boolean) {
        this.entityPM.IsConsigneeForPrint = newValue;
    }

    public get IsAttachedList(): boolean {
        return this.entityPM.IsAttachedList;
    }
    public set IsAttachedList(newValue: boolean) {
        this.entityPM.IsAttachedList = newValue;
    }

    public get InsufficentWorkingText(): string {
        return this.entityPM.InsufficentWorkingText;
    }
    public set InsufficentWorkingText(newValue: string) {
        this.entityPM.InsufficentWorkingText = newValue;
    }

    public get COONumberToCancel(): string {
        return this.entityPM.COONumberToCancel;
    }
    public set COONumberToCancel(newValue: string) {
        this.entityPM.COONumberToCancel = newValue;
    }

    public get Observations(): string {
        return this.entityPM.Observations;
    }
    public set Observations(newValue: string) {
        this.entityPM.Observations = newValue;
    }

    public get ConsigneeRemarks(): string {
        return this.entityPM.ConsigneeRemarks;
    }
    public set ConsigneeRemarks(newValue: string) {
        this.entityPM.ConsigneeRemarks = newValue;
    }

    public get ReplacementReason(): string {
        return this.entityPM.ReplacementReason;
    }
    public set ReplacementReason(newValue: string) {
        this.entityPM.ReplacementReason = newValue;
    }

    public get NonExportPort(): string {
        return this.entityPM.NonExportPort;
    }
    public set NonExportPort(newValue: string) {
        this.entityPM.NonExportPort = newValue;
    }

    public get NonExportCountry(): string {
        return this.entityPM.NonExportCountry;
    }
    public set NonExportCountry(newValue: string) {
        this.entityPM.NonExportCountry = newValue;
    }

    public get NonExportDate(): Date {
        return this.entityPM.NonExportDate;
    }
    public set NonExportDate(newValue: Date) {
        this.entityPM.NonExportDate = newValue;
    }

    public get NonPortOfEntrance(): string {
        return this.entityPM.NonPortOfEntrance;
    }
    public set NonPortOfEntrance(newValue: string) {
        this.entityPM.NonPortOfEntrance = newValue;
    }

    public get NonImportBillOfLadingNum(): string {
        return this.entityPM.NonImportBillOfLadingNum;
    }
    public set NonImportBillOfLadingNum(newValue: string) {
        this.entityPM.NonImportBillOfLadingNum = newValue;
    }

    public get NonImportDate(): Date {
        return this.entityPM.NonImportDate;
    }
    public set NonImportDate(newValue: Date) {
        this.entityPM.NonImportDate = newValue;
    }

    public get NonExitPort(): string {
        return this.entityPM.NonExitPort;
    }
    public set NonExitPort(newValue: string) {
        this.entityPM.NonExitPort = newValue;
    }

    public get NonExportBillOfLadingNum(): string {
        return this.entityPM.NonExportBillOfLadingNum;
    }
    public set NonExportBillOfLadingNum(newValue: string) {
        this.entityPM.NonExportBillOfLadingNum = newValue;
    }

    public get NonExpectedExitDate(): Date {
        return this.entityPM.NonExpectedExitDate;
    }
    public set NonExpectedExitDate(newValue: Date) {
        this.entityPM.NonExpectedExitDate = newValue;
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

    public get NonGoodsDescription(): string {
        return this.entityPM.NonGoodsDescription;
    }
    public set NonGoodsDescription(newValue: string) {
        this.entityPM.NonGoodsDescription = newValue;
    }

    public get NonManifestNum(): string {
        return this.entityPM.NonManifestNum;
    }
    public set NonManifestNum(newValue: string) {
        this.entityPM.NonManifestNum = newValue;
    }

    //#endregion properties

}
