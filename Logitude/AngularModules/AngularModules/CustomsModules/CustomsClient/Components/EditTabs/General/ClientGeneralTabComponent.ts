import {Component}  from '@angular/core';
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../../../../Infrastructure/Tools';



@Component({
    
    templateUrl: './ClientGeneralTabComponent.html',
})

export class ClientGeneralTabComponent extends BaseComponent{
    public ObjectTableName: string = "Customs.Client";
    public DataContext = this;
    public isCorporation: boolean; 
    public isCitizen: boolean;
    public isPassport: boolean;
    public entityPM: ClientPM;
    isNew: boolean;
    controlEnabled: boolean;
    constructor() {
        super();
    }

    InitTab(EntityPM :ClientPM, IsNew:boolean) {

        this.entityPM = EntityPM;

        this.isNew = IsNew;
        this.controlEnabled = IsNew;
        this.SetPropertiesEnabled();

        this.isPassport = (AppTool.IsNullOrEmpty(this.entityPM.Code)) && (!AppTool.IsNullOrEmpty(this.entityPM.PassportNumber));

        
        if (!AppTool.IsNullOrEmpty(this.entityPM.Code)) {
            this.isCorporation = this.entityPM.Code.startsWith("5");
            this.isCitizen = (!this.entityPM.Code.startsWith("5")) && (this.entityPM.Code != "");
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
    
        public get Corporation_Visibility()   {
        return (this.isCorporation == true) ? true : false; }



        public get Citizen_Visibility() {
            return (this.isCitizen == true) ? true : false;
        }
    

        public get Passport_Visibility() {
            return (this.isPassport == true) ? true : false;
        }


        public get Code() { return this.entityPM.Code; }
        public set Code(newValue: string) { this.entityPM.Code = newValue; }

        public get LocalCorporationName() { return this.entityPM.LocalCorporationName; }
        public set LocalCorporationName(newValue: string) { this.entityPM.LocalCorporationName = newValue; }
       
        public get EnglishCorporationName() { return this.entityPM.EnglishCorporationName; }
        public set EnglishCorporationName(newValue: string) { this.entityPM.EnglishCorporationName = newValue; }

        public get DunsNumber() { return this.entityPM.DunsNumber; }
        public set DunsNumber(newValue: string) { this.entityPM.DunsNumber = newValue; }

        public get ClientTypeSpecificCode() { return this.entityPM.ClientTypeSpecificCode; }
        public set ClientTypeSpecificCode(newValue: string) { this.entityPM.ClientTypeSpecificCode = newValue; }

        public get IsActive() { return this.entityPM.IsActive; }
        public set IsActive(newValue: boolean) { this.entityPM.IsActive = newValue; }

        public get IsExporter() { return this.entityPM.IsExporter; }
        public set IsExporter(newValue: boolean) { this.entityPM.IsExporter = newValue; }
    
        public get IsImporter() { return this.entityPM.IsImporter; }
        public set IsImporter(newValue: boolean) { this.entityPM.IsImporter = newValue; }

        public get LocalFirstName() { return this.entityPM.LocalFirstName; }
        public set LocalFirstName(newValue: string) { this.entityPM.LocalFirstName = newValue; }

        public get LocalLastName() { return this.entityPM.LocalLastName; }
        public set LocalLastName(newValue: string) { this.entityPM.LocalLastName = newValue; }

        public get EnglishFirstName() { return this.entityPM.EnglishFirstName; }
        public set EnglishFirstName(newValue: string) { this.entityPM.EnglishFirstName = newValue; }

        public get EnglishLastName() { return this.entityPM.EnglishLastName; }
        public set EnglishLastName(newValue: string) { this.entityPM.EnglishLastName = newValue; }

        public get BirthDate() { return this.entityPM.BirthDate; }
        public set BirthDate(newValue: Date) { this.entityPM.BirthDate = newValue; }

        public get PassportTypeCode() { return this.entityPM.PassportTypeCode; }
        public set PassportTypeCode(newValue: string) { this.entityPM.PassportTypeCode = newValue; }

        public get PassportNumber() { return this.entityPM.PassportNumber; }
        public set PassportNumber(newValue: string) { this.entityPM.PassportNumber = newValue; }

        public get PassportCountryCode() { return this.entityPM.PassportCountryCode; }
        public set PassportCountryCode(newValue: string) { this.entityPM.PassportCountryCode = newValue; }

        public get PassportFirstName() { return this.entityPM.PassportFirstName; }
        public set PassportFirstName(newValue: string) { this.entityPM.PassportFirstName = newValue; }

        public get PassportLastName() { return this.entityPM.PassportLastName; }
        public set PassportLastName(newValue: string) { this.entityPM.PassportLastName = newValue; }

        public get EnglishFatherName() { return this.entityPM.EnglishFatherName; }
        public set EnglishFatherName(newValue: string) { this.entityPM.EnglishFatherName = newValue; }

        public get EnglishBirthPlace() { return this.entityPM.EnglishBirthPlace; }
        public set EnglishBirthPlace(newValue: string) { this.entityPM.EnglishBirthPlace = newValue; }

        public get PassportIssueDate() { return this.entityPM.PassportIssueDate; }
        public set PassportIssueDate(newValue: Date) { this.entityPM.PassportIssueDate = newValue; }


        public get PassportExpirationDate() { return this.entityPM.PassportExpirationDate; }
        public set PassportExpirationDate(newValue: Date) { this.entityPM.PassportExpirationDate = newValue; }
 
        public get NationalIdentificationNumber() { return this.entityPM.NationalIdentificationNumber; }
        public set NationalIdentificationNumber(newValue: string) { this.entityPM.NationalIdentificationNumber = newValue; }

    //#endregio

}
