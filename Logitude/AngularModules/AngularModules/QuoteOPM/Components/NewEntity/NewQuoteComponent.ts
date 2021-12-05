import { Component, isDevMode } from "@angular/core";
import { AbstractControl, FormArray, FormGroup } from "@angular/forms";
import { PackageTypeList } from "Common/EntityLists/PackageTypeList";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { MessageService } from "primeng/api";
import { QuoteOPPMInitService } from "QuoteOPM/EntityPMInitServices/QuoteOPPMInitService";
import { QuoteOPPackagePM } from "QuoteOPM/EntityPMs/QuoteOPPackagePM";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";
import { QuoteOPPropertiesPM } from "QuoteOPM/EntityPMs/QuoteOPPropertiesPM";
import { QuoteOPPMService } from "QuoteOPM/Services/StandardPMs/QuoteOPPMService";
import { NewQuoteDataService } from "./Services/new-quote-data/new-quote-data.service";
import { NewQuoteValidateEntityService } from "./Services/new-quote-validate-entity/new-quote-validate-entity.service";


@Component({
    templateUrl: './NewQuoteComponent.html',
    styleUrls: ['./NewQuoteComponent.scss'],
})
export class NewQuoteComponent {
    EntityPM: QuoteOPPM;
    formGroup = new FormGroup({});
    isSubmit: boolean = false;

    constructor(
        private newQuoteDataService: NewQuoteDataService,
        private msg: MessageService,
        private ValidateService: NewQuoteValidateEntityService,
    ) { }

    async ngOnInit() {
        this.CreateNewQuote();

        // if (isDevMode())
        //     this.test()
    }

    async create() {
        this.isSubmit = true;
        // if (!this.ValidateService.validate(this.formGroup)) return;
        if (this.formGroup.invalid) return;

        const currentWindow:LogitudeWindow = SessionLocator.SelectedSession.CurrentWindow;

        try {
            currentWindow.StartBusyIndicator('Create new quote... ');

            this.addAutoProperties();
            this.addProperty();
            this.updatePropertiesTable();
            this.attachPackages();

            this.newQuoteDataService.creatingNewQuote(this.EntityPM)
                .then(() => SessionLocator.SelectedSession.CloseCurrentWindowEmit(this.EntityPM))
                .catch((err: string[]) => this.msg.add({ severity: 'error', summary: 'Create new quote failed', detail: err?.join(', ') }))
                .finally(() => currentWindow.StopBusyIndicator())

        } catch(err) {
            this.msg.add({ severity: 'error', summary: 'Create new quote failed', detail: err?.join(', ') });
            currentWindow.StopBusyIndicator()
        }
        // console.log(this.EntityPM)
    }

    cancel() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    private CreateNewQuote() {
        this.EntityPM = new QuoteOPPMService().GetNewEntityPM();
        QuoteOPPMInitService.InitValues(this.EntityPM, true);
    }


    private addAutoProperties() {
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

    private addProperty(): void {
        const propertyForm: FormGroup["controls"] = ((this.formGroup.controls.properties as FormArray).at(0) as FormGroup).controls;
        const deliveryValue: any = (propertyForm.delivery as FormGroup).value;
        const pickupValue: any = (propertyForm.pickup as FormGroup).value;

        if (deliveryValue.include) {
            this.EntityPM.FromAddressCity = deliveryValue.city
            this.EntityPM.FromAddressCountryId = deliveryValue.country?.Id
            this.EntityPM.FromAddressZipCode = deliveryValue.zipCode
            this.EntityPM.ToAddressId = deliveryValue.address?.Id
            this.EntityPM.IncludeDelivery = deliveryValue.include
        }
        
        if (pickupValue.include) {
            this.EntityPM.ToAddressCity = pickupValue.city
            this.EntityPM.ToAddressCountryId = pickupValue.country?.Id
            this.EntityPM.ToAddressZipCode = pickupValue.zipCode
            this.EntityPM.FromAddressId = pickupValue.address?.Id
            this.EntityPM.IncludePickUp = pickupValue.include
        }

        this.EntityPM.ToPortId = propertyForm.toPort.value?.Code
        this.EntityPM.FromPortId = propertyForm.fromPort.value?.Code
        this.EntityPM.MainCarriageCarrierId = propertyForm.mainCarriageCarrier.value?.AIRLINE_ID
        this.EntityPM.IncotermId = propertyForm.incoterm.value?.PTERMID
        this.EntityPM.SpecialServiceId = propertyForm.specialService.value?.SERVLEVEL_ID
    }

    private updatePropertiesTable(): void {
        const propertiesForms: AbstractControl[] = (this.formGroup.controls.properties as FormArray).controls.filter((propertyForm: FormGroup) => propertyForm.valid);
        
        propertiesForms.forEach((propertyFormGroup: FormGroup) => {
            const propertyForm: FormGroup["controls"] = propertyFormGroup.controls;
            const deliveryValue: any = (propertyForm.delivery as FormGroup).value;
            const pickupValue: any = (propertyForm.pickup as FormGroup).value;
            const propertiesPM: QuoteOPPropertiesPM = new QuoteOPPropertiesPM(this.EntityPM);

            if (deliveryValue.include) {
                propertiesPM.FromAddressCity = deliveryValue.city
                propertiesPM.FromAddressCountryId = deliveryValue.country?.Id
                propertiesPM.FromAddressZipCode = deliveryValue.zipCode
                propertiesPM.FromAddressId = deliveryValue.address?.Id
            }

            if (pickupValue.include) {
                propertiesPM.ToAddressCity = pickupValue.city
                propertiesPM.ToAddressCountryId = pickupValue.country?.Id
                propertiesPM.ToAddressZipCode = pickupValue.zipCode
                propertiesPM.ToAddressId = pickupValue.address?.Id                
            }

            propertiesPM.ToPortId = propertyForm.toPort.value?.Code
            propertiesPM.FromPortId = propertyForm.fromPort.value?.Code
            propertiesPM.MainCarriageCarrierId = propertyForm.mainCarriageCarrier.value?.AIRLINE_ID
            propertiesPM.IncotermId = propertyForm.incoterm.value?.PTERMID
            propertiesPM.SpecialServiceID = propertyForm.specialService.value?.SERVLEVEL_ID;

            this.EntityPM.QuoteProperties.push(propertiesPM)
        });
    }

    attachPackages() {
        (<FormArray>this.formGroup.controls.packages).controls.forEach((form: FormGroup) => {
            const pack: QuoteOPPackagePM = new QuoteOPPackagePM(this.EntityPM);
            const values: any = form.getRawValue();

            pack.Quantity = values.quantity;
            pack.Volume = values.volume;
            pack.GrossWeight = values.grossWeight;
            pack.PackageTypeId = (<PackageTypeList>values.packageType)?.Id;

            this.EntityPM.AddQuoteOPPackage(pack)
        });
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