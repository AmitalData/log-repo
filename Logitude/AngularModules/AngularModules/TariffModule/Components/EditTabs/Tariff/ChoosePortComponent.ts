import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DestinationClass } from './UpdateSurchargesComponent';
import { PortList } from '../../../../Common/EntityLists/PortList';

@Component({
    moduleId: module.id,
    templateUrl: './ChoosePortComponent.html',
})

export class ChoosePortComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: DestinationClass;
    public ObjectTableName = "Tariff";
    public ValidationErrorsList: string[] = [];
    constructor() {
        super();
    }

    SetDataContext(dataContext: DestinationClass) {
        this.DataContext = dataContext;        
    }

    private portId: string;
    get PortId() {
        return this.portId;
    }
    set PortId(value: string) {
        if (this.portId != value) {
            this.portId = value;
        }
    }

    private port: PortList = null;
    get Port() { return this.port; }
    set Port(newValue: PortList) {
        if (this.port != newValue) {
            this.port = newValue;
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    AddButtonClicked() {
        var errors: string[] = [];

        if (this.DataContext.Type == "From") {
            if (this.DataContext.fatherComponent.FromObsList.filter(d => d.Code == this.Port.Code).length > 0) {
                errors.push("Port with the same code already added");
            }
        }

        else {
            if (this.DataContext.fatherComponent.ToObsList.filter(d => d.Code == this.Port.Code).length > 0) {
                errors.push("Port with the same code already added");
            }
        }

        this.ValidationErrorsList = errors;
        
        if (errors.length == 0) {
            var newItem: DestinationClass = new DestinationClass(this.DataContext.fatherComponent, this.DataContext.Type, this.Port)

            if (this.DataContext.Type == "From") {
                this.DataContext.fatherComponent.FromObsList.push(newItem);
            }

            else {
                this.DataContext.fatherComponent.ToObsList.push(newItem);
            }
        }
    }
}
