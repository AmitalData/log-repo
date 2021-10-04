import { Component } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { CardList } from "Common/EntityLists/CardList";
import { ContactList } from "Common/EntityLists/ContactList";
import { CardListService } from "Common/Services/StandardLists/CardListService";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";
import { transporations } from "./components/new-quote-left-side/new-quote-left-side.component";

@Component({
    templateUrl: './NewQuoteComponent.html',
    styleUrls: ['./NewQuoteComponent.scss'],
})
export class NewQuoteComponent {
    public EntityPM: QuoteOPPM = new QuoteOPPM();

    formGroup = new FormGroup({});
    transporations = transporations;

    constructor() { }

    async ngOnInit() {
    }    
}
