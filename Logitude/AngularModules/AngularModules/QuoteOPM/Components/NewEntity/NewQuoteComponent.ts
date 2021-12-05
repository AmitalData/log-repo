import { Component, ElementRef, isDevMode } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { MessageService } from "primeng/api";
import { QuoteOPPMInitService } from "QuoteOPM/EntityPMInitServices/QuoteOPPMInitService";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";
import { QuoteOPPMService } from "QuoteOPM/Services/StandardPMs/QuoteOPPMService";
import { NewQuoteDataShareService } from "./Services/new-quote-data-share/new-quote-data-share.service";
import { NewQuoteDataService } from "./Services/new-quote-data/new-quote-data.service";
import { NewQuoteFocusErrorService } from "./Services/new-quote-focus-error/new-quote-focus-error.service";
import { NewQuoteHandleLinkedDataService } from "./Services/new-quote-handle-linked-data/new-quote-handle-linked-data.service";
import { NewQuoteValidateEntityService } from "./Services/new-quote-validate-entity/new-quote-validate-entity.service";


@Component({
    templateUrl: './NewQuoteComponent.html',
    styleUrls: ['./NewQuoteComponent.scss'],
})
export class NewQuoteComponent {
    EntityPM: QuoteOPPM;
    formGroup:FormGroup = new FormGroup({});
    isSubmit: boolean = false;

    constructor(
        private newQuoteDataService: NewQuoteDataService,
        private msg: MessageService,
        private dataShareService: NewQuoteDataShareService,
        private ValidateService: NewQuoteValidateEntityService,
        private elmRef: ElementRef,
        private focusErrorService: NewQuoteFocusErrorService,
        private handleLinkedDataService: NewQuoteHandleLinkedDataService,
    ) { }

    async ngOnInit() {
        this.dataShareService.EntityPM = this.EntityPM;
        this.dataShareService.newQuoteRef = this.elmRef;
        this.dataShareService.formGroup = this.formGroup;

        this.CreateNewQuote();
    }

    async create() {
        this.isSubmit = true;
        if (this.formGroup.invalid) {
            this.focusErrorService.focusError(this.formGroup, this.elmRef);
            return;
        }

        // if (!this.ValidateService.validate(this.formGroup)) return;

        const currentWindow:LogitudeWindow = SessionLocator.SelectedSession.CurrentWindow;

        try {
            currentWindow.StartBusyIndicator('Create new quote... ');

            this.handleLinkedDataService.addAutoProperties();
            this.handleLinkedDataService.addProperty();
            this.handleLinkedDataService.updatePropertiesTable();
            this.handleLinkedDataService.attachPackages();

            this.newQuoteDataService.creatingNewQuote(this.EntityPM)
                .then(() => SessionLocator.SelectedSession.CloseCurrentWindowEmit(this.EntityPM))
                .catch((err: string[]) => this.msg.add({ severity: 'error', summary: 'Create new quote failed', detail: err?.join(', ') }))
                .finally(() => currentWindow.StopBusyIndicator())

        } catch(err) {
            this.msg.add({ severity: 'error', summary: 'Create new quote failed', detail: err?.join(', ') });
            currentWindow.StopBusyIndicator()
        }
    }

    cancel() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    private CreateNewQuote() {
        this.EntityPM = new QuoteOPPMService().GetNewEntityPM();
        QuoteOPPMInitService.InitValues(this.EntityPM, true);
    }
}
