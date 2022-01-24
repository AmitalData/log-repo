import { ElementRef, Injectable } from '@angular/core';
import { FormGroup, FormArray, AbstractControl } from '@angular/forms';
import { PackageTypeList } from 'Common/EntityLists/PackageTypeList';
import { QuoteOPPackagePM } from 'QuoteOPM/EntityPMs/QuoteOPPackagePM';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { QuoteOPPropertiesPM } from 'QuoteOPM/EntityPMs/QuoteOPPropertiesPM';
import { NewQuoteDataShareService } from '../new-quote-data-share/new-quote-data-share.service';

@Injectable()
export class NewQuoteHandleLinkedDataService {

    constructor(
        private dataShareService: NewQuoteDataShareService,
    ) { }

    addAutoProperties() {
        const entityPM: QuoteOPPM = this.dataShareService.EntityPM;

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
        const entityPM: QuoteOPPM = this.dataShareService.EntityPM;
        const formGroup: FormGroup = this.dataShareService.formGroup

        const propertyForm: FormGroup["controls"] = ((formGroup.controls.properties as FormArray).at(0) as FormGroup).controls;
        const deliveryValue: any = (propertyForm.delivery as FormGroup).value;
        const pickupValue: any = (propertyForm.pickup as FormGroup).value;

        if (deliveryValue.include) {
            entityPM.FromAddressCity = deliveryValue.city
            entityPM.FromAddressCountryId = deliveryValue.country?.Id
            entityPM.FromAddressZipCode = deliveryValue.zipCode
            entityPM.ToAddressId = deliveryValue.address?.Id
            entityPM.IncludeDelivery = deliveryValue.include
        }

        if (pickupValue.include) {
            entityPM.ToAddressCity = pickupValue.city
            entityPM.ToAddressCountryId = pickupValue.country?.Id
            entityPM.ToAddressZipCode = pickupValue.zipCode
            entityPM.FromAddressId = pickupValue.address?.Id
            entityPM.IncludePickUp = pickupValue.include
        }

        entityPM.ToPortId = propertyForm.toPort.value?.Code
        entityPM.FromPortId = propertyForm.fromPort.value?.Code
        entityPM.MainCarriageCarrierId = propertyForm.mainCarriageCarrier.value?.AIRLINE_ID
        entityPM.IncotermId = propertyForm.incoterm.value?.PTERMID
        entityPM.SpecialServiceId = propertyForm.specialService.value?.SERVLEVEL_ID
    }

    updatePropertiesTable(): void {
        const entityPM: QuoteOPPM = this.dataShareService.EntityPM;
        const formGroup: FormGroup = this.dataShareService.formGroup
        const propertiesForms: AbstractControl[] = (formGroup.controls.properties as FormArray).controls.filter((propertyForm: FormGroup) => propertyForm.valid);
        entityPM.QuoteProperties = [];

        propertiesForms.forEach((propertyFormGroup: FormGroup) => {
            const propertyForm: FormGroup["controls"] = propertyFormGroup.controls;
            const deliveryValue: any = (propertyForm.delivery as FormGroup).value;
            const pickupValue: any = (propertyForm.pickup as FormGroup).value;
            const propertiesPM: QuoteOPPropertiesPM = new QuoteOPPropertiesPM(entityPM);

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

            entityPM.QuoteProperties.push(propertiesPM) 
        });
    }

    attachPackages() {
        const entityPM: QuoteOPPM = this.dataShareService.EntityPM;
        const formGroup: FormGroup = this.dataShareService.formGroup;
        entityPM.QuotePackages = [];

        (<FormArray>formGroup.controls.packages).controls.forEach((form: FormGroup) => {
            const pack: QuoteOPPackagePM = new QuoteOPPackagePM(entityPM);
            const values: any = form.getRawValue();

            pack.Quantity = values.quantity;
            pack.Volume = values.volume;
            pack.GrossWeight = values.grossWeight;
            pack.PackageTypeId = (<PackageTypeList>values.packageType)?.Id;

            entityPM.AddQuoteOPPackage(pack)
        });
    }
}
