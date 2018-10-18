import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BusinessUnitPM} from '../../../EntityPMs/BusinessUnitPM';
import {BusinessUnitPMService} from '../../../Services/StandardPMs/BusinessUnitPMService';
import {BusinessUnitListService} from '../../../Services/StandardLists/BusinessUnitListService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    selector: 'BusinessUnitGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './BusinessUnitGeneralTabComponent.html',
})

export class BusinessUnitGeneralTabComponent extends BaseComponent {

    public EntityPM: BusinessUnitPM;
    public ObjectTableName: string = "BusinessUnit";
    public DataContext: BusinessUnitGeneralTabComponent = this;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();
    }

    private SetUIProperties() {
        this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
    }

    //Properties
    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get ParentId() { return this.EntityPM.ParentId; }
    set ParentId(value: string) {
        if (this.EntityPM.ParentId != value) {
            this.EntityPM.ParentId = value;
            this.UpdateParentName();
        }
    }

    get ParentName() { return this.EntityPM.ParentName; }
    set ParentName(value:string)
    {
        if (this.EntityPM.ParentName != value) {
            this.EntityPM.ParentName = value;
        }
    }

    private UpdateParentName() {
        if (AppTool.IsNullOrEmpty(this.ParentId)) {
            this.ParentName = null;
        }
        else {
            var service: BusinessUnitListService = new BusinessUnitListService();
            service.getAll().subscribe((response :ServiceResponse)=> {
                if (!response.HasError) {
                    var list = response.Result;
                    var businessunit = list.filter(d => d.Id == this.ParentId)[0];
                    if (businessunit != null) {
                        this.ParentName = list.Name;
                    }
                }
            });
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }
}