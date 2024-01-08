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
    }

    public get IsDispalyOnly() { 
       return this.isDispalyOnlyStatusList.includes(Number(this.entityPM.CooStatusCode))
    }
   
   
}
