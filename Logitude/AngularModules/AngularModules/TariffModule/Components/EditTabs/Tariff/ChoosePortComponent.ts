import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DestinationClass } from './UpdateSurchargesComponent';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './ChoosePortComponent.html',
})

export class ChoosePortComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: ChoosePortComponent = this;
    public UpdateClass: DestinationClass;
    public ObjectTableName = "Tariff";
    public ForceFocus: any;
    public OriginDependencyFilterValue: string = "A";

    public ValidationErrorsList: string[] = [];
    constructor() {
        super();
    }

    SetDataContext(dataContext: DestinationClass) {
        this.UpdateClass = dataContext;
        this.SetOriginDependencyFilterValue();
    }


    SetOriginDependencyFilterValue() {
        if (this.UpdateClass.fatherComponent.EntityPM.EntityParentPM.TypeCode == "OLC" || this.UpdateClass.fatherComponent.EntityPM.EntityParentPM.TypeCode == "OSC"
            || this.UpdateClass.fatherComponent.EntityPM.EntityParentPM.TypeCode == "OFS") {
            this.OriginDependencyFilterValue = "O";
        }
    }


    KeyDownEvent(event) {
        if (event == 13 && (this.Port != null && this.PortId != null)) {
            this.AddButtonClicked();
        }
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

        if (this.Port == null || AppTool.IsNullOrEmpty(this.PortId)) {
            errors.push("Please Choose port");
        }

        else {
            if (this.UpdateClass.Type == "From") {
                if (this.UpdateClass.fatherComponent.FromObsList.filter(d => d.Code == this.Port.Code).length > 0) {
                    errors.push("Port with the same code already added");
                }
            }

            else {
                if (this.UpdateClass.fatherComponent.ToObsList.filter(d => d.Code == this.Port.Code).length > 0) {
                    errors.push("Port with the same code already added");
                }
            }
        }

        this.ValidationErrorsList = errors;
        
        if (errors.length == 0) {
            var newItem: DestinationClass = new DestinationClass(this.UpdateClass.fatherComponent, this.UpdateClass.Type, this.Port, null)

            if (this.UpdateClass.Type == "From") {
                this.UpdateClass.fatherComponent.FromObsList.push(newItem);
            }

            else {
                this.UpdateClass.fatherComponent.ToObsList.push(newItem);
            }

            this.ForceFocus = this.PortId;


            this.Port = null;
            this.PortId = null;
        }
    }
}
