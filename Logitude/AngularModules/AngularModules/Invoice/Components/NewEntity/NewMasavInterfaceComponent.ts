import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { APPaymentPM } from '../../EntityPMs/APPaymentPM';
import { APInvoicePM } from '../../EntityPMs/APInvoicePM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DateTool, AppTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { CurrencyRatesService, LastRate } from '../../../Common/Services/CurrencyRatesService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { CardListService } from '../../../Common/Services/StandardLists/CardListService';
import { CardList } from '../../../Common/EntityLists/CardList';
import { PartnersDomainService } from '../../../Common/Services/PartnersDomainService';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AccountingPaymentMethodList } from '../../EntityLists/AccountingPaymentMethodList';
import { AccountingPaymentMethodListService } from '../../Services/StandardLists/AccountingPaymentMethodListService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { APPaymentInvoicePM } from '../../EntityPMs/APPaymentInvoicePM';
import { MasavInterfacePM } from 'Invoices/EntityPMs/MasavInterfacePM';

@Component({
    templateUrl: './NewMasavInterfaceComponent.html',
})

export class NewMasavInterfaceComponent extends BaseComponent  {
    ObjectTableName: string = "MasavInterface";
    DataContext: any = this;
    entityPM: MasavInterfacePM = new MasavInterfacePM();
    //OpenFormatReportPMService: OpenFormatReportPMService = new OpenFormatReportPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    testingMode:any;
    constructor() {
        super();
        this.entityPM.Tenant = SessionLocator.Tenant;     
      
    }
   

  
    get FromDate() { return this.entityPM.FromDate; }
    set FromDate(value: Date) {
        if (this.entityPM.FromDate != value) {
            this.entityPM.FromDate = value;
            if (this.ToDate < value) {
                this.entityPM.UIProperties.SetValidity("FromoDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
            }
        }
    }



    get ToDate() { return this.entityPM.ToDate; }
    set ToDate(value: Date) {
        if (this.entityPM.ToDate != value) {
            this.entityPM.ToDate = value;
            if (this.FromDate > value) {
                this.entityPM.UIProperties.SetValidity("ToDate", this.ObjectTableName,false, TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
            }
           
        }
    }
    

    ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
        
        var errors: string[] = [];
        if (this.ToDate < this.FromDate) {
            errors.push(TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
        }

        this.ValidationErrorsList = errors;

        // if (this.ValidationErrorsList.length == 0) {
        //     this.CurrentSession.StartBusyIndicator("");
        //     this.OpenFormatReportPMService.insert(this.entityPM).subscribe((myResult:any) => {

        //         var mm: ServiceResponse = myResult;
        //         if (!mm.HasError) {
        //             var entity = mm.Result;

        //             this.CurrentSession.CloseCurrentWindowEmit("ok");

        //             SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
        //                 this.CurrentSession.SessionLocation.viewContainerRef)
        //                 .then(cmpRef => {
        //                     cmpRef.instance.ComponentRef = cmpRef;
        //                     cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
        //                     cmpRef.instance.BackCompleted.subscribe(($event: any) => {
        //                         this.CancelButtonClicked();
        //                     });
        //                 });
        //             this.CurrentSession.StopBusyIndicator();
        //         }

        //         else {
        //             this.ValidationErrorsList = mm.ErrorsArray;
        //             this.CurrentSession.StopBusyIndicator();
        //         }
        //     });


        // }



    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }
}
