import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {MoveTypePM} from '../../../../Infrastructure/EntityPMs/MoveTypePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';

@Component({
    selector: 'MoveTypeGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './MoveTypeGeneralTabComponent.html',
})

export class MoveTypeGeneralTabComponent extends BaseComponent implements OnInit {
    public DataContext: MoveTypeGeneralTabComponent = this;
    public ObjectTableName: string = "MoveType";
    public EntityPM: MoveTypePM;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }

    ngOnInit() {
        if (this.EntityPM != null) {


        }
    }
    
    // Properties
    get Code() { return this.EntityPM.Code; }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }

    get MoveTypeEnglishName() { return this.EntityPM.MoveTypeEnglishName; }
    set MoveTypeEnglishName(newValue: string) {
        if (this.EntityPM.MoveTypeEnglishName != newValue) {
            this.EntityPM.MoveTypeEnglishName = newValue;
        }
    }

    get MoveTypeLocalName() { return this.EntityPM.MoveTypeLocalName; }
    set MoveTypeLocalName(newValue: string) {
        if (this.EntityPM.MoveTypeLocalName != newValue) {
            this.EntityPM.MoveTypeLocalName = newValue;
        }
    }
    
    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }

    get IsAir() { return this.EntityPM.IsAir; }
    set IsAir(newValue: boolean) {
        if (this.EntityPM.IsAir != newValue) {
            this.EntityPM.IsAir = newValue;
        }
    }

    get IsInland() { return this.EntityPM.IsInland; }
    set IsInland(newValue: boolean) {
        if (this.EntityPM.IsInland != newValue) {
            this.EntityPM.IsInland = newValue;
        }
    }

    get IsOcean() { return this.EntityPM.IsOcean; }
    set IsOcean(newValue: boolean) {
        if (this.EntityPM.IsOcean != newValue) {
            this.EntityPM.IsOcean = newValue;
        }
    }

    get TransportModeId() { return this.EntityPM.TransportModeId; }    
    set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue;
        }
    }

    SetTransportMode(value: string) {
        this.TransportModeId = value;
    }
}