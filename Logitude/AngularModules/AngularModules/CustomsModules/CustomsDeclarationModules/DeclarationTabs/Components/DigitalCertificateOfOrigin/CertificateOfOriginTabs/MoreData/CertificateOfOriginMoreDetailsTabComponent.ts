import { Component } from '@angular/core';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { CertificateOfOriginInvoicePM } from 'Customs/EntityPMs/CertificateOfOriginInvoicePM';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';



@Component({

    templateUrl: './CertificateOfOriginMoreDetailsTabComponent.html',
})

export class CertificateOfOriginMoreDetailsTabComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.CertificateOfOrigin";
    public DataContext = this;
    public entityPM: CertificateOfOriginPM;
    public currentDeclaration:DeclarationPM;
    isNew: boolean;
    controlEnabled: boolean;
    isDispalyOnlyStatusList:number[] = [4,8];
    constructor() {
        super();
    }

    InitTab(EntityPM: CertificateOfOriginPM, currentDeclaration: DeclarationPM ,IsNew: boolean) {
        
        this.entityPM = EntityPM;
        this.currentDeclaration = currentDeclaration;
        this.isNew = IsNew;
        this.controlEnabled = IsNew;
        this.SetPropertiesEnabled();


       
    }
    
    SetPropertiesEnabled() {

        // ADD to do Name FILDES 101507
        var enabled = !this.IsDispalyOnly;
        this.UIProperties.SetEnabled("CooTypeCode", this.ObjectTableName, enabled);
        
   
        
            // First set of fields            
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
   
   
}
