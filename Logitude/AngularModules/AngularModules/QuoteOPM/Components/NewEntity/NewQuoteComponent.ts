import { Component, isDevMode } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { add } from "cypress/types/lodash";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { MessageService } from "primeng/api";
import { QuoteOPPMInitService } from "QuoteOPM/EntityPMInitServices/QuoteOPPMInitService";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";
import { QuoteOPPMService } from "QuoteOPM/Services/StandardPMs/QuoteOPPMService";
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
        private messageService: MessageService,
    ) { }

    async ngOnInit() {
        this.CreateNewQuote();

        // if (isDevMode())
        //     this.test()
    }

    async create() {
        if (this.formGroup.invalid) return;

        SessionLocator.SelectedSession.CurrentWindow.StartBusyIndicator('Create new quote... ');

        this.addProperty()
        this.newQuoteDataService.creatingNewQuote(this.EntityPM)
            .then(() => SessionLocator.SelectedSession.CloseCurrentWindowEmit('OK'))
            .catch((err: string[]) => this.messageService.add({ severity: 'error', summary: 'Create new quote failed', detail: err.join(', ')}))
            .finally(() => SessionLocator.SelectedSession.CurrentWindow.StopBusyIndicator())

        // console.log(this.EntityPM)
    }

    cancel() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    private CreateNewQuote() {
        this.EntityPM = new QuoteOPPMService().GetNewEntityPM();
        QuoteOPPMInitService.InitValues(this.EntityPM, true);
    }

    private addProperty() {
        const entityPM: QuoteOPPM = this.EntityPM;

        entityPM.QuoteCustomerTypeCode = entityPM.DirectionId === 'I' ? 'CON' : 'SHI';
        entityPM.IsCopyExchangeRates = false;
        entityPM.ExchangeRate = 1;

        if (entityPM.DirectionId === 'I') {
            entityPM.QuoteCustomerTypeCode = 'CON';
            entityPM.CustomerId = entityPM.ConsigneeId;
        } else {
            entityPM.QuoteCustomerTypeCode = 'SHI';
            entityPM.CustomerId = entityPM.ShipperId;
        }
    }

    // private test() {
    //     console.log('*************  test mode ****************')
    //     Object.keys(testData).forEach(propName => this.EntityPM[propName] = testData[propName])
    // }
}


// const testData = {
//     branchId: "1-29",
//     businessUnitId: "1",
//     chargeableWeight: 1,
//     chargeableWeightUnitCode: "KG",
//     consigneeContactId: "1-6387",
//     consigneeId: "1-4221",
//     consigneeName: "LIRAN TEST",
//     consigneeNote: null,
//     createdByUserId: "1-6390",
//     departmentId: "1-32",
//     dimensionsUnitCode: "Cm",
//     directionId: "E",
//     expirationDate: new Date(),
//     expirationDays: 1,
//     fromAddressCity: "sdfsd",
//     fromAddressCountryId: "1-449",
//     fromAddressZipCode: 1,
//     fromPortId: "AAC",
//     grossWeight: 1,
//     grossWeightUnitCode: "KG",
//     includeDelivery: true,
//     includePickUp: true,    
//     incotermId: "DDU",
//     isClosed: false,
//     moveTypeId: "1-13",
//     numberOfPackages: 1,
//     openDate: new Date(),
//     quoteCharges: [],
//     quoteCostCharges: [],
//     quotePackages: [],
//     quoteSaleCharges: [],
//     quoteSalesTotals: [],
//     quoteTypeCode: "A",
//     ratingCode: "N",
//     saleCurrencyId: "1-3",
//     shipmentTypeId: "FTL",
//     shipperContactId: "1-6387",
//     shipperId: "1-4221",
//     shipperName: "LIRAN TEST",
//     shipperNote: null,
//     specialServiceId: "BAK",
//     startDate: new Date(),
//     tenant: 1,
//     toAddressCity: "fsdf",
//     toAddressCountryId: "1-449",
//     totalVATPerQuote: [],
//     toPortId: "AAC",
//     transportModeId: "O",
//     updateDate: new Date(),
//     updatedByUserId: "1-6390",
//     volume: 1,
//     volumeUnitCode: "CBM",
// }