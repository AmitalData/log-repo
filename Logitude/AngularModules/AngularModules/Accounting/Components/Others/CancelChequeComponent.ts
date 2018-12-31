import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component}  from '@angular/core';
import {PaymentChequePM} from '../../EntityPMs/PaymentChequePM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {PaymentChequePMService} from '../../Services/StandardPMs/PaymentChequePMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { JournalExtendedPMService } from '../../Services/ExtendedPMs/JournalExtendedPMService';



@Component({
    selector: 'CancelChequeComponent',
    moduleId: module.id,
    templateUrl: './CancelChequeComponent.html',
})
export class CancelChequeComponent extends BaseComponent {
    ObjectTableName: string = "PaymentCheque";
    DataContext: any = this;
    entityPM: PaymentChequePM;
    public ValidationErrorsList: string[] = [];

    paymentChequePMService: PaymentChequePMService = new PaymentChequePMService();
    constructor() {
        super();

        this.UIProperties.SetRequired("CancellationRemarks", "PaymentCheque", true);

    }

    SetWindowArgs(args: any) {
        if (args != null) {
          
                this.entityPM = args.PaymentChequePM;
              
           
        }
    }

    get CancellationRemarks() { return this.entityPM.CancellationRemarks; }
    set CancellationRemarks(value: string) {
        if (this.entityPM.CancellationRemarks != value) {
            this.entityPM.CancellationRemarks = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("CancellationRemarks", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("CancellationRemarks", this.ObjectTableName, true);

            }
        }
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
    FIELD_IS_REQUIERD: string;
    OkButtonClicked() {
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        var errors: string[] = [];
        if (AppTool.IsNullOrEmpty(this.CancellationRemarks)) {
            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("PaymentCheque.F.CancellationRemarks"));
            errors.push(s);

        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.entityPM.CancellationRemarks = this.CancellationRemarks;
            this.entityPM.IsCancelled = true;
            this.entityPM.CancelledDate = new Date();
            this.entityPM.PaymentChequeStatusCode = "4";
            this.entityPM.CancelledByUserId = SessionLocator.LoggedUserId;
            SessionLocator.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
            this.paymentChequePMService.update(this.entityPM).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                   
                    let myJournalExtendedPMService: JournalExtendedPMService = new JournalExtendedPMService();
                    myJournalExtendedPMService
                        .VoidJournal(this.entityPM.Tenant, this.entityPM.JournalId, "", "", "")
                        .subscribe((res: ServiceResponse) => {
                            SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");
                            SessionLocator.CurrentSession.StopBusyIndicator();

                            if (res.HasError) {
                                this.ValidationErrorsList = res.ErrorsArray;

                            }
                            
                        });
                  

                }
                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }
}
