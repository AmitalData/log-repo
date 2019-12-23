import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { CarrierAreasPortPM } from '../../../../Common/EntityPMs/CarrierAreasPortPM';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { DateTool } from '../../../../Infrastructure/Tools';
import { AreaItemClass } from '../EditTabs/AreasTabComponent';

@Component({
    moduleId: module.id,
    templateUrl: './ChoosePortComponent.html',
})

export class ChoosePortComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: ChoosePortComponent = this;
    public ParentClass: AreaItemClass;
    public ObjectTableName = "CarrierAreasPort";
    public ValidationErrorsList: string[] = [];
    public ForceFocus: any ;
    constructor() {
        super();
    }

    SetDataContext(dataContext: AreaItemClass) {
        this.ParentClass = dataContext;        
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
        if (this.Port == null || this.PortId == null) {
            errors.push("Please Choose port");
        }

        else if (this.ParentClass.PortItemsList.filter(d => d.Code == this.Port.Code).length > 0) {
            errors.push("Port with the same code already added");
        }
 
        this.ValidationErrorsList = errors;
        
        if (errors.length == 0) {
            var newPort: CarrierAreasPortPM = new CarrierAreasPortPM(this.ParentClass.EntityPM);
            newPort.Tenant = SessionLocator.Tenant;
            newPort.CarrierAreaId = this.ParentClass.EntityPM.Id;
            newPort.Name = this.Port.EnglishName;
            newPort.Code = this.Port.Code;
            newPort.CountryCode = this.Port.CountryCode;
            newPort.PortId = this.PortId;
            newPort.AddedByUserId = SessionInfo.LoggedUserId;
            newPort.AddedDate = DateTool.GetCurrentDateAsUtc();
            this.ParentClass.EntityPM.AddCarrierAreasPortPM(newPort);
            this.ParentClass.BuildPortItemsList();

            this.ForceFocus = this.PortId;
            this.Port = null;
            this.PortId = null;
        }
    }
}
