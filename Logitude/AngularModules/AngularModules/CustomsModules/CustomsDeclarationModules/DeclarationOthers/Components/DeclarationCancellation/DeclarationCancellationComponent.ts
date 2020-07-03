import {Component, ChangeDetectorRef, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';

@Component({
    selector: 'DeclarationCancellationComponent',
    
    templateUrl: './DeclarationCancellationComponent.html',
})

export class DeclarationCancellationComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Customs.Declaration";

    private CurrentSession = SessionLocator.SelectedSession;
    
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
    constructor(private EntityResourceService: EntityResourceService) {
        super();
 

    }

    SetWindowArgs(args: any) {
        this.EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {

            this.EntityPM = args.Declaration as DeclarationPM;

        });

    }

    ngOnInit() {

    }

    
}
