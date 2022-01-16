import { Component, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ChargesTypePM } from '../../../EntityPMs/ChargesTypePM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({

    templateUrl: './ChargesTypeQuotationsTabComponent.html',
})

export class ChargesTypeQuotationsTabComponent extends BaseComponent  {
    public EntityPM: ChargesTypePM;
    public DataContext: ChargesTypeQuotationsTabComponent = this;
    public ObjectTableName: string = "ChargesType";
    public IsQuotationFeatureOn = false; 
    constructor(public args: EntityArgs) {
        super();
        this.EntityPM = args.EntityPM;
        this.CheckQuotationFeature();
    }
    private CheckQuotationFeature() {
        if (FeatureLocator.HasFeaturePermession("Quote", "QUOTEQUOTATION")) {
            this.IsQuotationFeatureOn = true;
        }
    }
    get QuoteChargesGroupId() { return this.EntityPM.QuoteChargesGroupId; }
    set QuoteChargesGroupId(value: string) {
        if (this.EntityPM.QuoteChargesGroupId != value) {
            this.EntityPM.QuoteChargesGroupId = value;
        }
    }
}
