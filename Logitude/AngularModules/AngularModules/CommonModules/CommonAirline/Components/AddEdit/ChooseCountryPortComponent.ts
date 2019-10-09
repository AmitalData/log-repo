/// <reference path="../../../../common/services/standardlists/portlistservice.ts" />
import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DestinationClass } from './AddEditAirlineAreaComponent';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { PortListService } from '../../../../Common/Services/StandardLists/PortListService';
import { CountryList } from '../../../../Common/EntityLists/CountryList';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';


@Component({
    moduleId: module.id,
    templateUrl: './ChooseCountryPortComponent.html',
})

export class ChooseCountryPortComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: ChooseCountryPortComponent = this;
    public ParentClass: DestinationClass;
    public ObjectTableName = "AirlineAreasPort";
    public ValidationErrorsList: string[] = [];
    public ForceFocus: any;
    constructor() {
        super();
    }

    SetDataContext(dataContext: DestinationClass) {
        this.ParentClass = dataContext;
    }

    KeyDownEvent(event) {
        if (event == 13 && (this.Country != null && this.CountryId != null)) {
            this.AddButtonClicked();
        }
    }

    private countryId: string;
    get CountryId() {
        return this.countryId;
    }
    set CountryId(value: string) {
        if (this.countryId != value) {
            this.countryId = value;
            this.CountryPortsMessageCount = "";
        }
    }

    private country: CountryList = null;
    get Country() { return this.country; }
    set Country(newValue: CountryList) {
        if (this.country != newValue) {
            this.country = newValue;
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    AddButtonClicked() {
        var errors: string[] = [];
        if (this.Country == null || this.CountryId == null) {
            errors.push("Please Choose Country");
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.LoadPortListMethod();
            this.ForceFocus = this.CountryId;
            this.Country = null;
            this.CountryId = null;
        }
    }
    public CountryPortsMessageCount = ""; 
    private LoadPortListMethod() {
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("TransportModeId", "A", null, null, "Equals", false, true, false, "Text");
        filters.addAdditionalFilter("CountryId", this.CountryId, null, null, "Equals", false, true, false, "Text");
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");

        filters.GetAll = true;
        var service = new PortListService();
        service.getByFilters(filters).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var portsList: PortList[] = response.Result;
                var counter = 0;
                portsList.forEach(item => {
                    if (this.ParentClass.fatherComponent.ItemList.filter(d => d.Code == item.Code).length == 0) {
                        var newItem: DestinationClass = new DestinationClass(this.ParentClass.fatherComponent, item, true);
                        this.ParentClass.fatherComponent.ItemList.push(newItem);
                        counter = counter + 1;
                    }
                });
                this.ParentClass.fatherComponent.ItemList = this.ParentClass.fatherComponent.ItemList.sort((a, b) => { return (a.CountryCode === b.CountryCode) ? 0 : (a.CountryCode < b.CountryCode) ? -1 : 1 });
                this.CountryPortsMessageCount = counter + " ports added";
                this.CurrentSession.StopBusyIndicator();
            }
            else {
                for (var k in response.ErrorsArray) {
                    this.ValidationErrorsList.push(response.ErrorsArray[k]);
                }
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
