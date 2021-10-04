import { Component } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";
import { NewQuoteDataService } from "./Services/new-quote-data/new-quote-data.service";


@Component({
    templateUrl: './NewQuoteComponent.html',
    styleUrls: ['./NewQuoteComponent.scss'],
})
export class NewQuoteComponent {
    EntityPM: QuoteOPPM = new QuoteOPPM();
    formGroup = new FormGroup({});

    constructor(
        private newQuoteDataService: NewQuoteDataService,
    ) { }

    create() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit('OK')
        this.newQuoteDataService.creatingNewQuote(this.EntityPM)
    }

    cancel() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }


    async ngOnInit() {
    }
}
