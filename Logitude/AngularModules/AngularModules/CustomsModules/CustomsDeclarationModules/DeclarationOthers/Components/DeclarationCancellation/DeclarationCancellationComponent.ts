import {Component, ChangeDetectorRef, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'DeclarationCancellationComponent',
    
    templateUrl: './DeclarationCancellationComponent.html',
    providers: [DeclarationPMService]
})

export class DeclarationCancellationComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: DeclarationCancellationComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    ValidationErrorsList: string[];
    readonly: boolean = false;
     get CancelRequestNumber() { return this.EntityPM.CancelRequestNumber; }
    set CancelRequestNumber(value: number) {
        if (this.EntityPM.CancelRequestNumber != value) {
            this.EntityPM.CancelRequestNumber = value;
         }
    }


    get CancelRequestReasonCode() { return this.EntityPM.CancelRequestReasonCode; }
    set CancelRequestReasonCode(value: string) {
        if (this.EntityPM.CancelRequestReasonCode != value) {
            this.EntityPM.CancelRequestReasonCode = value;
        }

        if (value) {
            this.UIProperties.SetRequired("CancelRequestReasonCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("CancelRequestReasonCode", this.ObjectTableName, true);
        }
    }

    get CancelRequestReasonExplanation() { return this.EntityPM.CancelRequestReasonExplanation; }
    set CancelRequestReasonExplanation(value: string) {
        if (this.EntityPM.CancelRequestReasonExplanation != value) {
            this.EntityPM.CancelRequestReasonExplanation = value;
        }
    }


    get CancelRequestStatusCode() { return this.EntityPM.CancelRequestStatusCode; }
    set CancelRequestStatusCode(value: string) {
        if (this.EntityPM.CancelRequestStatusCode != value) {
            this.EntityPM.CancelRequestStatusCode = value;
        }
    }

    get CustomCancelRequestRemarks() { return this.EntityPM.CustomCancelRequestRemarks; }
    set CustomCancelRequestRemarks(value: string) {
        if (this.EntityPM.CustomCancelRequestRemarks != value) {
            this.EntityPM.CustomCancelRequestRemarks = value;
        }
    }

    get CancelRequestApproveDate() { return this.EntityPM.CancelRequestApproveDate; }
    set CancelRequestApproveDate(value: Date) {
        if (this.EntityPM.CancelRequestApproveDate != value) {
            this.EntityPM.CancelRequestApproveDate = value;
        }
    }


    get CancelRequestRejectionReason() { return this.EntityPM.CancelRequestRejectionReason; }
    set CancelRequestRejectionReason(value: string) {
        if (this.EntityPM.CancelRequestRejectionReason != value) {
            this.EntityPM.CancelRequestRejectionReason = value;
        }
    }
    constructor(private EntityResourceService: EntityResourceService,private _declarationPMService: DeclarationPMService) {
        super();
      

    }
    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.ValidationErrorsList = null;
        SessionLocator.SelectedSession.StartBusyIndicator("");
        this._declarationPMService.update(this.EntityPM).subscribe(x => {
            SessionLocator.SelectedSession.StopBusyIndicator();
        });
    }

    FillErrors() {
        var errors: string[] = [];
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.CancelRequestReasonCode)) {
            var msg = "קוד סיבת ביטול שדה חובה.";//TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierTypeCode");
            this.ValidationErrorsList.push(msg);
        }
    }
    SetWindowArgs(args: any) {
        this.EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
           
            this.EntityPM = args.Declaration as DeclarationPM;
            this.UIProperties.SetEnabled("CancelRequestStatusCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomCancelRequestRemarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CancelRequestApproveDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CancelRequestRejectionReason", this.ObjectTableName, false);
            if (this.CancelRequestStatusCode == "5" || this.CancelRequestStatusCode == "2") {
                this.UIProperties.SetEnabled("CancelRequestReasonExplanation", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("CancelRequestReasonCode", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("CancelRequestNumber", this.ObjectTableName, false);
                this.readonly = true;
            }
        });

    }

    ngOnInit() {

    }

    
}
