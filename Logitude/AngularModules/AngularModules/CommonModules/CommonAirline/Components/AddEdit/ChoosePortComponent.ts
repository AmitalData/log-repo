import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DestinationClass } from './AddEditAirlineAreaComponent';
import { PortList } from '../../../../Common/EntityLists/PortList';

@Component({
    moduleId: module.id,
    templateUrl: './ChoosePortComponent.html',
})

export class ChoosePortComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: DestinationClass;
    public ObjectTableName = "AirlineAreasPort";
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

        if (this.DataContext.fatherComponent.ItemList.filter(d => d.Code == this.Port.Code).length > 0) {
            errors.push("Port with the same code already added");
        }
        else if (this.Port == null || this.PortId) {
            errors.push("Please Choose port");
        }

    

        this.ValidationErrorsList = errors;
        
        if (errors.length == 0) {
            var newItem: DestinationClass = new DestinationClass(this.DataContext.fatherComponent, this.Port,true)
            this.DataContext.fatherComponent.ItemList.push(newItem);

        }
    }
}
