import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool} from '../../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {DeclarationPM} from '../../../../../../Customs/EntityPMs/DeclarationPM';
import {DeclarationPMService} from '../../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import {ClientList} from '../../../../../../Customs/EntityLists/ClientList';
import {CustomerIdentifyTypePM} from '../../../../../../Customs/EntityPMs/CustomerIdentifyTypePM';
import { MessageWindow } from '../../../../../../Controls/Windows/MessageWindow';

@Component({

    moduleId: module.id,
    templateUrl: './ImporterDetailsComponent.html',
    selector :'ImporterDetailsComponent',

})
export class ImporterDetailsComponent extends BaseComponent {
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

    public get ImporterEntitlementTypeCode() { return this.EntityPM.ImporterEntitlementTypeCode; }
    public set ImporterEntitlementTypeCode(newValue: string) { this.EntityPM.ImporterEntitlementTypeCode = newValue; }

    public get EntitleImporterName() { return this.EntityPM.EntitleImporterName; }
    public set EntitleImporterName(newValue: string) { this.EntityPM.EntitleImporterName = newValue;  }
    public get ImporterName() { return this.EntityPM.ImporterName; }
    public set ImporterName(newValue: string)
    {
        this.EntityPM.ImporterName = newValue;
    }

    public get TransferImporterName() { return this.EntityPM.TransferImporterName; }
    public set TransferImporterName(newValue: string) { this.EntityPM.TransferImporterName = newValue;  }

    public get ImporterTypeCode() { return this.EntityPM.ImporterTypeCode; }
    public set ImporterTypeCode(newValue: string) {
        this.EntityPM.ImporterTypeCode = newValue;
        this.EntityPM.ImporterCode = null;
      
        this.EntityPM.ImporterPassportNumber = null;
        this.EntityPM.ImporterPassCountryCode = null;
        this.EntityPM.ImporterAddress = null;
        this.EntityPM.MainImporterEntitlemntTypeCode = null;
        this.ImporterName = null;
        this.SetFieldsEditibility(this.ImporterTypeCode, this.type);
    }

    public get ImporterTypeName() { return this.EntityPM.ImporterTypeName; }
    public set ImporterTypeName(newValue: string) {
        this.EntityPM.ImporterTypeName = newValue;
     //   this.SetFieldsEditibility(newValue, this.type);
    }
 

    public get TransferImporterTypeCode() { return this.EntityPM.TransferImporterTypeCode; }
    public set TransferImporterTypeCode(newValue: string)
    {
        this.EntityPM.TransferImporterTypeCode = newValue;
        this.EntityPM.TransferImporterCode = null;
        this.EntityPM.TransferPassportNumber = null;
        this.EntityPM.TransferImporterCountryCode = null;
        this.TransferImporterAddress = null;
        this.TransImporterEntitleTypeCode = null;
        this.TransferImporterName = null;
      
       
    }


    public get TransferImporterTypeName() { return this.EntityPM.TransferImporterTypeName; }
    public set TransferImporterTypeName(newValue: string) {
        this.EntityPM.TransferImporterTypeName = newValue;
        
    }

    public get EntitleImporterTypeCode() { return this.EntityPM.EntitleImporterTypeCode; }
    public set EntitleImporterTypeCode(newValue: string) {
        this.EntityPM.EntitleImporterTypeCode = newValue;
        this.EntityPM.EntitleImporterCode = null;
       
            this.EntityPM.EntitlePassportNumber = null;
            this.EntityPM.EntitleImporterCountryCode = null;
            this.EntitleImporterAddress = null;
            this.ImporterEntitlementTypeCode = null;
            this.EntitleImporterName = null;
        
    }

    public get EntitleImporterTypeName() { return this.EntityPM.EntitleImporterTypeName; }
    public set EntitleImporterTypeName(newValue: string) {
        this.EntityPM.EntitleImporterTypeName = newValue;
      
    }

    public get ImporterPassCountryCode() { return this.EntityPM.ImporterPassCountryCode; }
    public set ImporterPassCountryCode(newValue: string) { this.EntityPM.ImporterPassCountryCode = newValue; }


    public get TransferImporterCountryCode() { return this.EntityPM.TransferImporterCountryCode; }
    public set TransferImporterCountryCode(newValue: string) { this.EntityPM.TransferImporterCountryCode = newValue; }

    public get MainImporterEntitlemntTypeCode() { return this.EntityPM.MainImporterEntitlemntTypeCode; }
    public set MainImporterEntitlemntTypeCode(newValue: string) { this.EntityPM.MainImporterEntitlemntTypeCode = newValue; }

    public get TransImporterEntitleTypeCode() { return this.EntityPM.TransImporterEntitleTypeCode; }
    public set TransImporterEntitleTypeCode(newValue: string) { this.EntityPM.TransImporterEntitleTypeCode = newValue; }

    public get ImporterAddress() { return this.EntityPM.ImporterAddress; }
    public set ImporterAddress(newValue: string) { this.EntityPM.ImporterAddress = newValue; }

    public get EntitleImporterAddress() { return this.EntityPM.EntitleImporterAddress; }
    public set EntitleImporterAddress(newValue: string) { this.EntityPM.EntitleImporterAddress = newValue; }

    public get TransferImporterAddress() { return this.EntityPM.TransferImporterAddress; }
    public set TransferImporterAddress(newValue: string) { this.EntityPM.TransferImporterAddress = newValue; }


    private isImporterAddressEnabled: boolean;
    public get IsImporterAddressEnabled() { return this.isImporterAddressEnabled; }
    public set IsImporterAddressEnabled(newValue: boolean) { this.isImporterAddressEnabled = newValue; }


    private isImporterNameEnabled: boolean;
    public get IsImporterNameEnabled() { return this.isImporterNameEnabled; }
    public set IsImporterNameEnabled(newValue: boolean) { this.isImporterNameEnabled = newValue; }
 
    private isMainImporterEntitlemntTypeCodeEnabled: boolean;
    public get IsMainImporterEntitlemntTypeCodeEnabled() { return this.isMainImporterEntitlemntTypeCodeEnabled; }
    public set IsMainImporterEntitlemntTypeCodeEnabled(newValue: boolean) { this.isMainImporterEntitlemntTypeCodeEnabled = newValue; }

    private isImporterPassCountryCodeEnabled: boolean;
    public get IsImporterPassCountryCodeEnabled() { return this.isImporterPassCountryCodeEnabled; }
    public set IsImporterPassCountryCodeEnabled(newValue: boolean) { this.isImporterPassCountryCodeEnabled = newValue; }


    isImporterPassportNumberEnabled: boolean;
    get IsImporterPassportNumberEnabled() { return this.isImporterPassportNumberEnabled; }
    set IsImporterPassportNumberEnabled(value: boolean) { this.isImporterPassportNumberEnabled = value; }

    isImporterTypeCodeEnabled: boolean;
    get IsImporterTypeCodeEnabled() { return this.isImporterTypeCodeEnabled; }
    set IsImporterTypeCodeEnabled(value: boolean) { this.isImporterTypeCodeEnabled = value; }


    importerVisibility: boolean;
    get ImporterVisibility() { return this.importerVisibility; }
    set ImporterVisibility(value: boolean) { this.importerVisibility = value; }

    entitleVisibility: boolean;
    get EntitleVisibility() { return this.entitleVisibility; }
    set EntitleVisibility(value: boolean) { this.entitleVisibility = value; }

     transferVisibility: boolean;
     get TransferVisibility() { return this.transferVisibility; }
     set TransferVisibility(value: boolean) { this.transferVisibility = value; }
    

 
     get ImporterPassportNumber() { return this.EntityPM.ImporterPassportNumber; }
     set ImporterPassportNumber(value: string) { this.EntityPM.ImporterPassportNumber = value; }


   
     get EntitlePassportNumber (){return this.EntityPM.EntitlePassportNumber; }
     set EntitlePassportNumber(value: string) { this.EntityPM.EntitlePassportNumber= value; }
    

 
     get TransferPassportNumber() { return this.EntityPM.TransferPassportNumber; }
     set TransferPassportNumber(value: string) { this.EntityPM.TransferPassportNumber= value; }


     get EntitleImporterCountryCode() { return this.EntityPM.EntitleImporterCountryCode; }
     set EntitleImporterCountryCode(value: string) { this.EntityPM.EntitleImporterCountryCode = value; }

    //#REGION  Task 42204: שינויים במסך פרטי יבואן בהצהרה
    get isCourierDeclaration() { return this.EntityPM.IsCourierDeclaration; }

    get CasualImporterAddress1() { return this.EntityPM.CasualImporterAddress1; }
    set CasualImporterAddress1(value: string) { this.EntityPM.CasualImporterAddress1 = value; }
    get CasualImporterAddress2() { return this.EntityPM.CasualImporterAddress2; }
    set CasualImporterAddress2(value: string) { this.EntityPM.CasualImporterAddress2 = value; }

    get CasualImporterCity() { return this.EntityPM.CasualImporterCity; }
    set CasualImporterCity(value: string) { this.EntityPM.CasualImporterCity = value; }

    get CasualImporterZipCode() { return this.EntityPM.CasualImporterZipCode; }
    set CasualImporterZipCode(value: string) { this.EntityPM.CasualImporterZipCode= value; }

    

    get CasualImporterFax() { return this.EntityPM.CasualImporterFax; }
    set CasualImporterFax(value: string) { this.EntityPM.CasualImporterFax = value; }

    get CasualImporterEmail() { return this.EntityPM.CasualImporterEmail; }
    set CasualImporterEmail(value: string) { this.EntityPM.CasualImporterEmail= value; }

    get CasualImporterTel() { return this.EntityPM.CasualImporterTel; }
    set CasualImporterTel(value: string) { this.EntityPM.CasualImporterTel = value; }

    get CasualImporterContact() { return this.EntityPM.CasualImporterContact; }
    set CasualImporterContact(value: string) { this.EntityPM.CasualImporterContact = value; }

    
    //#REGION  Task 42204: שינויים במסך פרטי יבואן בהצהרה


     //get TransImporterEntitlementTypeCode() { return this.EntityPM.TransImporterEntitlementTypeCode; }
     //set TransImporterEntitlementTypeCode(value: string) { this.EntityPM.TransImporterEntitlementTypeCode = value; }

     customerIdentifyType: CustomerIdentifyTypePM;
     get CustomerIdentifyType() { return this.customerIdentifyType; }
     set CustomerIdentifyType(value: CustomerIdentifyTypePM) {

         if (this.customerIdentifyType != value) {
             this.customerIdentifyType = value;
         }
         if (!AppTool.IsNullOrEmpty(value)) {
             this.ImporterTypeName = value.EnglishName;


         } else {
             this.ImporterTypeName = null;
             this.ImporterTypeCode = null;
         }

         this.SetFieldsEditibility(this.ImporterTypeCode, this.type);
     }

     entitleCustomerIdentifyType: CustomerIdentifyTypePM;
     get EntitleCustomerIdentifyType() { return this.entitleCustomerIdentifyType; }
     set EntitleCustomerIdentifyType(value: CustomerIdentifyTypePM) {

         if (this.entitleCustomerIdentifyType != value) {
             this.entitleCustomerIdentifyType = value;
         }
         if (!AppTool.IsNullOrEmpty(value)) {
             this.EntitleImporterTypeName = value.EnglishName;


         } else {
             this.EntitleImporterTypeName = null;
             this.EntitleImporterTypeCode = null;
         }
         this.SetFieldsEditibility(this.EntitleImporterTypeCode, this.type);
     }

     transferCustomerIdentifyType: CustomerIdentifyTypePM;
     get TransferCustomerIdentifyType() { return this.transferCustomerIdentifyType; }
     set TransferCustomerIdentifyType(value: CustomerIdentifyTypePM) {

         if (this.transferCustomerIdentifyType != value) {
             this.transferCustomerIdentifyType = value;
         }
         if (!AppTool.IsNullOrEmpty(value)) {
             this.TransferImporterTypeName = value.EnglishName;


         } else {
             this.TransferImporterTypeName = null;
             this.TransferImporterTypeCode = null;
         }
         this.SetFieldsEditibility(this.TransferImporterTypeCode, this.type);
     }

     public IsEntitleImporterEnabled: boolean = true;
     public IsTransferImporterEnabled: boolean = true;
     public IsImporterEnabled: boolean = true;

     SetWindowArgs(args: any) {
         if (!AppTool.IsNullOrEmpty(args)) {
             this.EntityPM = args.EntityPM;
             this.OriginalEntityPM = args.EntityPM;
             this.ClonedEntityPM = this.CloneEntity(args.EntityPM);
             this.type = args.Type;
             this.IsDisplayOnly = args.IsDisplayOnly;

             switch (this.type) {
                 case "Importer": {
                     this.ImporterVisibility = true;
                     this.EntitleVisibility = false;
                     this.TransferVisibility = false;
                     this.SetFieldsEditibility(this.EntityPM.ImporterTypeCode, this.type);
                     if (this.EntityPM.ImporterTypeName == "IL") {
                         if (this.EntityPM.ImporterCode != null || this.EntityPM.ImporterId != null) {
                             this.UIProperties.SetEnabled("ImporterTypeCode", this.ObjectTableName, false);
                         }
                     }
                     break;
                 }

                 case "Transfer": {
                     this.ImporterVisibility = false;
                     this.EntitleVisibility = false;
                     this.TransferVisibility = true;
                     this.SetFieldsEditibility(this.EntityPM.TransferImporterTypeCode, this.type);
                     if (this.EntityPM.TransferImporterTypeCode == "1") {
                         if (this.EntityPM.TransferImporterCode != null || this.EntityPM.TransferImporterId != null) {
                             this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, false);
                         }
                     }
                     break;
                 }

                 case "Entitle": {
                     this.ImporterVisibility = false;
                     this.EntitleVisibility = true;
                     this.TransferVisibility = false;
                     this.SetFieldsEditibility(this.EntityPM.EntitleImporterTypeCode, this.type);
                     if (this.EntityPM.EntitleImporterTypeCode == "1") {
                         if (this.EntityPM.EntitleImporterCode != null || this.EntityPM.EntitleImporterId != null) {
                             this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, false);
                         }
                     }
                     break;
                 }
            
             }

             //Disable fields
             if (this.IsDisplayOnly) {
                 this.SetScreenFieldsEditability();
             }
          
          

         }
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterAddress", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferImporterName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferImporterAddress", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferImporterCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferPassportNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EntitleImporterName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EntitleImporterAddress", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EntitleImporterCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EntitlePassportNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CasualImporterAddress1", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CasualImporterAddress2", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CasualImporterCity", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CasualImporterZipCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CasualImporterFax", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CasualImporterEmail", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CasualImporterTel", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CasualImporterContact", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("MainImporterEntitlemntTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterEntitlementTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransImporterEntitleTypeCode", this.ObjectTableName, !this.IsDisplayOnly);

        if (this.IsDisplayOnly) {
            this.IsImporterEnabled = !this.IsDisplayOnly;
            this.IsTransferImporterEnabled = !this.IsDisplayOnly;
            this.IsImporterEnabled = !this.IsDisplayOnly;
        }
        
    }

     SetFieldsEditibility(xxxTypeCode: string, type: string) {
         
         switch (type) {
             case "Importer": {
                 
                 if (xxxTypeCode == "1") {
                     if (AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {
                         this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, true);
                         this.UIProperties.SetEnabled("ImporterAddress", this.ObjectTableName, true);
                         this.UIProperties.SetEnabled("ImporterTypeCode", this.ObjectTableName, true);
                         // this.EntityPM.ImporterCode = this.ImporterName;
                         
                     }
                     else {
                         this.UIProperties.SetEnabled("ImporterTypeCode", this.ObjectTableName, false);
                         this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
                         this.UIProperties.SetEnabled("ImporterAddress", this.ObjectTableName, false);

                     }
                     
                     this.IsImporterEnabled = false;
                  //   
                     this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, false);
                     
                 }
                 else if (xxxTypeCode == "2" || xxxTypeCode == "3") {
                     this.IsImporterEnabled = true;
                     this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("ImporterAddress", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, true);
                     this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, true);

                 }
                 

                 this.SetFieldsEditibilityCourier();
                 break;
             }

             case "Transfer": {
                 if (xxxTypeCode == "1") {
                     if (AppTool.IsNullOrEmpty(this.EntityPM.TransferImporterCode)  ) {
                         this.UIProperties.SetEnabled("TransferImporterName", this.ObjectTableName, true);
                         this.UIProperties.SetEnabled("TransferImporterAddress", this.ObjectTableName, true);
                         this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, true);
                        // this.EntityPM.TransferImporterCode = this.TransferImporterName;
                      

                     }
                     else {
                         this.UIProperties.SetEnabled("TransferImporterName", this.ObjectTableName, false);
                         this.UIProperties.SetEnabled("TransferImporterAddress", this.ObjectTableName, false);
                         this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, false);
                         

                     }
                     this.IsTransferImporterEnabled = false;
                    // this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("TransferImporterCountryCode", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("TransferPassportNumber", this.ObjectTableName, false);
                 }

                 else if (xxxTypeCode == "2" || xxxTypeCode == "3") {
                     this.IsTransferImporterEnabled = true;
                      this.UIProperties.SetEnabled("TransferImporterTypeCode", this.ObjectTableName, true);
                     this.UIProperties.SetEnabled("TransferImporterName", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("TransferImporterAddress", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("TransferImporterCountryCode", this.ObjectTableName, true);
                     this.UIProperties.SetEnabled("TransferPassportNumber", this.ObjectTableName, true);

                 }
                 

                 break;
             }

             case "Entitle": {
                 if (xxxTypeCode == "1") {
                     if (AppTool.IsNullOrEmpty(this.EntityPM.EntitleImporterCode)) {
                         this.UIProperties.SetEnabled("EntitleImporterName", this.ObjectTableName, true);
                         this.UIProperties.SetEnabled("EntitleImporterAddress", this.ObjectTableName, true);
                         this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, true);
                      //   this.EntityPM.EntitleImporterCode = this.EntitleImporterName;
                        
                     }
                     else {
                         this.UIProperties.SetEnabled("EntitleImporterName", this.ObjectTableName, false);
                         this.UIProperties.SetEnabled("EntitleImporterAddress", this.ObjectTableName, false);
                         this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, false);
                      
                     }
                     this.IsEntitleImporterEnabled = false;
                    // this.UIProperties.SetEnabled("EntitleImporterTypeCode", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("EntitleImporterCountryCode", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("EntitlePassportNumber", this.ObjectTableName, false);
                 }
                 else if (xxxTypeCode == "2" || xxxTypeCode == "3") {
                     this.UIProperties.SetEnabled("EntitleImporterName", this.ObjectTableName, false);
                     this.UIProperties.SetEnabled("EntitleImporterAddress", this.ObjectTableName, false);
                     this.IsEntitleImporterEnabled = true;
                     this.UIProperties.SetEnabled("EntitleImporterCountryCode", this.ObjectTableName, true);
                     this.UIProperties.SetEnabled("EntitlePassportNumber", this.ObjectTableName, true);
                 }
                 break;
             }

         }

         //Disable fields
         if (this.IsDisplayOnly) {
             this.SetScreenFieldsEditability();
         }
     }

    private SetFieldsEditibilityCourier() {
        if (!this.isCourierDeclaration) {
            return;
        }
        //if (AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {
        
        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterAddress1", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterAddress2", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterCity", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterZipCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterFax", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterEmail", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterTel", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CasualImporterContact", this.ObjectTableName, true);
        //}
        if (///!AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode) ||
            this.EntityPM.ImporterTypeCode == "2" /*"P"*/ ||
            this.EntityPM.ImporterTypeCode == "3" /*"F"*/) {

            this.ImporterName = "";
            this.ImporterAddress = "";
            this.EntityPM.CalculatedImporterName = null



            this.CasualImporterAddress1 = "";
            this.CasualImporterAddress2 = "";
            this.CasualImporterCity = "";
            this.CasualImporterZipCode = "";
            this.CasualImporterFax = "";
            this.CasualImporterEmail = "";
            this.CasualImporterTel = "";
            this.CasualImporterContact = "";
            this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterAddress1", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterAddress2", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterCity", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterZipCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterFax", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterEmail", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterTel", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CasualImporterContact", this.ObjectTableName, false);

        }
    }

     ImporterLostFocus(type: any, item: any) {

         if (item != null) {
             var isNumberTooLong: boolean = false;
             switch (type) {
                 case 'Importer': {
                     if ((this.ImporterTypeName == "IL" && item.length > 9) || (this.ImporterTypeName != "IL" && item.length > 15)) {
                         isNumberTooLong = true;
                         this.ImporterPassportNumber = null;
                     }
                     break;
                 }
                 case 'Entitle': {
                     if ((this.EntitleImporterTypeCode == "IL" && item.length > 9) || (this.EntitleImporterTypeCode != "IL" && item.length > 15)) {
                         isNumberTooLong = true;
                         this.EntitlePassportNumber = null;
                     }
                     break;
                 }
                 case 'Transfer': {
                         if ((this.TransferImporterTypeCode == "IL" && item.length > 9) || (this.TransferImporterTypeCode != "IL" && item.length > 15)) {
                         isNumberTooLong = true;
                         this.TransferPassportNumber = null;
                     }
                     break;
                 }
             }
             if (isNumberTooLong) {
                 var messageWindow = new MessageWindow();
                 messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                 messageWindow.Width = 250;
                 messageWindow.Height = 150;
                 messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                 messageWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.TooLongCode"));
                 return;
             }
         }

         switch (type) {
             case 'Importer': {
                 this.ImporterPassportNumber = item;
                 break;
             }
             case 'Transfer': {
                 this.TransferPassportNumber = item;
                 break;
             }
             case 'Entitle': {
                this.EntitlePassportNumber = item;
                 break;
             }
         }

     }

     ImporterNumberTextChanged(type: any, code: string) {
         switch (type) {
             case 'Importer': {
                 this.ImporterPassportNumber = code;
                 break;
             }
             case 'Transfer': {
                 this.TransferPassportNumber = code;
                 break;
             }
             case 'Entitle': {
                 this.EntitlePassportNumber = code;
                 break;
             }
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

     GetPassportNumber(passportNumber: string) {

         if (passportNumber.length <= 13) {
             return passportNumber;
         }
         else if (passportNumber.length == 14) {
             return passportNumber.substring(2);
         }
         else if (passportNumber.length == 15) {
             return passportNumber.substring(3);
         }
     }

    doDisable: boolean;
    DeleteImporterDetailsButtonClicked() {
        //this.ImporterTypeCode = null;
        this.ImporterPassportNumber = null;
        this.ImporterPassCountryCode = null;
        this.MainImporterEntitlemntTypeCode = null;
        this.ImporterName = null;
        this.ImporterTypeName = null;

        this.CasualImporterAddress1 = null;
        this.CasualImporterAddress2 = null;
        this.CasualImporterCity = null;
        this.CasualImporterContact = null;
        this.CasualImporterEmail = null;
        this.CasualImporterFax = null;
        this.CasualImporterTel = null;
        this.CasualImporterZipCode = null;
        this.EntityPM.CalculatedImporterName = null

        //this.OkButtonClicked();
        //SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
        //SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
    }
    Append2ImporterAddress(val:string ) {
        if (!AppTool.IsNullOrEmpty(val)) {
            if (!AppTool.IsNullOrEmpty(this.ImporterAddress)) {
                this.ImporterAddress += " " + val;
            } else {
                this.ImporterAddress = val;
            }
        }
    }
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

                            this.EntityPM.CalculatedImporterName = this.ImporterName;

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

                            this.EntityPM.CalculatedTransferImporterName = this.TransferImporterName;

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

                            this.EntityPM.CalculatedEntitleImporterName = this.EntitleImporterName;
                          
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
