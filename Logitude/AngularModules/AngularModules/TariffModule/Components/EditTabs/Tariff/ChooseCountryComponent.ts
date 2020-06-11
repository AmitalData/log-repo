import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DestinationClass } from './UpdateSurchargesComponent';
import { CountryList } from '../../../../Common/EntityLists/CountryList';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { PortListService } from '../../../../Common/Services/StandardLists/PortListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { PortList } from '../../../../Common/EntityLists/PortList';

@Component({
    templateUrl: './ChooseCountryComponent.html',
})

export class ChooseCountryComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: ChooseCountryComponent = this;
    public UpdateClass: DestinationClass;
    public ObjectTableName = "Tariff";
    public ForceFocus: any;
    public ValidationErrorsList: string[] = [];
    private TransportModeCode: string = "A"
    constructor() {
        super();
    }

    SetDataContext(dataContext: DestinationClass) {
        this.UpdateClass = dataContext;
        this.SetTransportModeCodeValue();
    }

    private SetTransportModeCodeValue() {
        if (this.UpdateClass.fatherComponent.EntityPM.EntityParentPM.TypeCode == "OSC"
            || this.UpdateClass.fatherComponent.EntityPM.EntityParentPM.TypeCode == "OFS") {
            this.TransportModeCode = "O";
        }
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

        if (this.Country == null || AppTool.IsNullOrEmpty(this.CountryId)) {
            errors.push("Please Choose country");
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

        filters.addAdditionalFilter("TransportModeId", this.TransportModeCode, null, null, "Equals", false, true, false, "Text");
        filters.addAdditionalFilter("CountryId", this.CountryId, null, null, "Equals", false, true, false, "Text");
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");

        filters.GetAll = true;
        var service = new PortListService();
        service.getByFilters(filters).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var portsList: PortList[] = response.Result;
                var counter = 0;
                portsList.forEach(item => {
                    var newItem: DestinationClass = new DestinationClass(this.UpdateClass.fatherComponent, this.UpdateClass.Type, item, null)

                    if (this.UpdateClass.Type == "From") {
                        if (this.UpdateClass.fatherComponent.FromObsList.filter(d => d.Code == item.Code).length == 0) {
                            this.UpdateClass.fatherComponent.FromObsList.push(newItem);
                            counter = counter + 1;
                        }
                    }

                    else {
                        if (this.UpdateClass.fatherComponent.ToObsList.filter(d => d.Code == item.Code).length == 0) {
                            this.UpdateClass.fatherComponent.ToObsList.push(newItem);
                            counter = counter + 1;
                        }
                    }
                });

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
