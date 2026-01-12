import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Component} from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { MasavInterfacePM } from 'Invoices/EntityPMs/MasavInterfacePM';
import { MasavInterfacePMService } from 'Invoice/Services/StandardPMs/MasavInterfacePMService';

@Component({
    templateUrl: './NewMasavInterfaceComponent.html',
})

export class NewMasavInterfaceComponent extends BaseComponent  {
    ObjectTableName: string = "MasavInterface";
    DataContext: any = this;
    entityPM: MasavInterfacePM = new MasavInterfacePM();
    masavInterfacePMService: MasavInterfacePMService = new MasavInterfacePMService();
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
    get PaymentDate() { return this.entityPM.PaymentDate; }
    set PaymentDate(value: Date) {
        if (this.entityPM.PaymentDate != value) {
            this.entityPM.PaymentDate = value;
            
           
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

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("");
            this.masavInterfacePMService.insert(this.entityPM).subscribe((myResult:any) => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;

                    this.CurrentSession.CloseCurrentWindowEmit("ok");

                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                        this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                                this.CancelButtonClicked();
                            });
                        });
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
             });


         }



    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }
}
