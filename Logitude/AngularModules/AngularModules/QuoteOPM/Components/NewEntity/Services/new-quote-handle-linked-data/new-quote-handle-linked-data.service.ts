import { ElementRef, Injectable } from '@angular/core';
import { FormGroup, FormArray, AbstractControl } from '@angular/forms';
import { PackageTypeList } from 'Common/EntityLists/PackageTypeList';
import { QuoteOPPackagePM } from 'QuoteOPM/EntityPMs/QuoteOPPackagePM';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { QuoteOPPropertiesPM } from 'QuoteOPM/EntityPMs/QuoteOPPropertiesPM';
import { NewQuoteDataShareService } from '../new-quote-data-share/new-quote-data-share.service';

@Injectable()
export class NewQuoteHandleLinkedDataService {
    EntityPM: QuoteOPPM = null as any;
    newQuoteRef: ElementRef = null as any;
    formGroup: FormGroup = null as any;

    constructor(
        private dataShareService: NewQuoteDataShareService,
    ) { }

    ngOnInit() {
        this.EntityPM = this.dataShareService.EntityPM;
        this.newQuoteRef = this.dataShareService.newQuoteRef;
    }

    addAutoProperties() {
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

    addProperty(): void {
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

    updatePropertiesTable(): void {
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
}
