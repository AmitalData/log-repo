import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './ARInvoiceCancellationReasionComponent.html',
})

export class ARInvoiceCancellationReasionComponent extends BaseComponent {
    public DataContext: ARInvoiceCancellationReasionComponent = this;
    public ObjectTableName: string = "ARInvoice";
    public SATCancelReasons: SATCancelReasonDetails[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.FillSATCancelReasonss();
    }

    SetWindowArgs(){
    }

    FillSATCancelReasonss() {
        this.SATCancelReasons.push(new SATCancelReasonDetails("01", "Comprobante emitido con errores con relación"));
        this.SATCancelReasons.push(new SATCancelReasonDetails("02", "Comprobante emitido con errores sin relación"));
        this.SATCancelReasons.push(new SATCancelReasonDetails("03", "No se llevó a cabo la operación"));
    }

    private selectdSATCancelReason: SATCancelReasonDetails;
    get SelectdSATCancelReason() { return this.selectdSATCancelReason; }
    set SelectdSATCancelReason(value: SATCancelReasonDetails) {
        if (this.selectdSATCancelReason != value) {
            this.selectdSATCancelReason = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Reject");
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit(this.SelectdSATCancelReason.Code);
    }
}

class SATCancelReasonDetails {
    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }
    Code: string;
    Name: string;
}
