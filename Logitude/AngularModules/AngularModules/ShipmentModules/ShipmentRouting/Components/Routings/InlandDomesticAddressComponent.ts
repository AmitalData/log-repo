import { Component } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ShipmentPM } from 'Shipment/EntityPMs/ShipmentPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';

@Component({
    templateUrl: './InlandDomesticAddressComponent.html',
})

export class InlandDomesticAddressComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public ObjectTableName: string;
    DataContext: ShipmentPM;
    ShipmentPM: ShipmentPM;
    IsToAddress: boolean;

    Address1FieldName: string;
    Address2FieldName: string;
    StateIdFieldName: string;
    PhoneFieldName: string;
    FaxFieldName: string;
    CountryIdFieldName: string;

    SetWindowArgs(args: any) {
        this.DataContext = ServiceHelper.CloneEntityPM(args['shipmentPM']);
        this.ShipmentPM = args['shipmentPM'];
        this.ObjectTableName = args['objectTableName'];
        this.IsToAddress = args['addressType'] == 'To';

        this.SetFieldNames();
    }

    SetFieldNames() {
        this.Address1FieldName = this.IsToAddress ? 'InlandDomesticToAddress1' : 'InlandDomesticFromAddress1';
        this.Address2FieldName = this.IsToAddress ? 'InlandDomesticToAddress2' : 'InlandDomesticFromAddress2';
        this.StateIdFieldName = this.IsToAddress ? 'InlandDomesticToStateId' : 'InlandDomesticFromStateId';
        this.PhoneFieldName = this.IsToAddress ? 'InlandDomesticToPhone' : 'InlandDomesticFromPhone';
        this.FaxFieldName = this.IsToAddress ? 'InlandDomesticToFax' : 'InlandDomesticFromFax';
        this.CountryIdFieldName = this.IsToAddress ? 'InlandDomesticToCountryId' : 'InlandDomesticFromCountryId';
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private IsDirty: boolean;
    OkButtonClicked() {
        this.IsDirty = false;
        this.ChangeFieldValue(this.Address1FieldName);
        this.ChangeFieldValue(this.Address2FieldName);
        this.ChangeFieldValue(this.StateIdFieldName);
        this.ChangeFieldValue(this.PhoneFieldName);
        this.ChangeFieldValue(this.FaxFieldName);

        if (this.IsDirty) this.ShipmentPM.IsDirty = this.IsDirty;
        this.CurrentSession.CloseCurrentWindow();
    }

    ChangeFieldValue(fieldName: string) {
        if (this.ShipmentPM[fieldName] == this.DataContext[fieldName]) return;
        this.ShipmentPM[fieldName] = this.DataContext[fieldName];
        this.IsDirty = true;
    }

}