import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {VesselPM} from '../../../EntityPMs/VesselPM';
import {EntityArgs} from  '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './VesselGeneralTabComponent.html',
})

export class VesselGeneralTabComponent extends BaseComponent {
    public EntityPM: VesselPM;
    public DataContext: VesselGeneralTabComponent = this;
    public ObjectTableName: string = "Vessel";
    public IsNewEntity: boolean = true;
    constructor(public args: EntityArgs) {
        super();

        this.EntityPM = args.EntityPM;

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.IsNewEntity = true;
        }

        else {
            this.IsNewEntity = false;
        }
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    get IMOCode() { return this.EntityPM.IMOCode; }
    set IMOCode(value: string) {
        if (this.EntityPM.IMOCode != value) {
            this.EntityPM.IMOCode = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get CountryId() { return this.EntityPM.CountryId; }
    set CountryId(value: string) {
        if (this.EntityPM.CountryId != value) {
            this.EntityPM.CountryId = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }
    
    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }    
}