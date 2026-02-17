import {Component, OnInit} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceViewModelData} from './CustomerGeneralTabComponent';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
@Component({
    moduleId: module.id,
    templateUrl: './EditCustomerAdditionalServiceComponent.html',
})

export class EditCustomerAdditionalServiceComponent extends BaseComponent  {
    public EntityPM: any = null;
    public ObjectTableName = "Customer";
    public DataContext: EditCustomerAdditionalServiceComponent = this;
    public ObjectTableId: string;
    public ShowRadioButtons: boolean = true;
    public get Potential() {
        if (this.EntityPM != null)
        {
            return this.EntityPM.Potential;
        }
    }
    public set Potential(value: boolean) {
        if (this.EntityPM != null)
        {
            if (value != this.EntityPM.Potential)
                this.EntityPM.Potential = value;
        }
    }

    public get Notes() {
        if (this.EntityPM != null)
            return this.EntityPM.Notes;
    }

    public set Notes(value: string) {
        if (this.EntityPM != null) {
            if (value != this.EntityPM.Notes)
                this.EntityPM.Notes = value;
        }
    }
    public CustomerAdditionalServiceRadio: string = "CustomerAdditionalServiceRadio_";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.CustomerAdditionalServiceRadio += this.CurrentSession.GetNewId("RadioButton");
    }


    public get InUse() {
        if (this.EntityPM != null) {
            return this.EntityPM.InUse;
        }
    }
    public set InUse(value: boolean) {
        if (this.EntityPM != null)
        {
            if (value != this.EntityPM.InUse)
                this.EntityPM.InUse = value;
        }
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }
    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("ok");

    }

 
}
