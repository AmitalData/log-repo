import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {MeasurementPM} from '../../../EntityPMs/MeasurementPM';

@Component({
    moduleId: module.id,
    templateUrl: './MeasurementGeneralComponent.html',
})

export class MeasurementGeneralComponent extends BaseComponent implements OnInit {

    public EntityPM: MeasurementPM;
    public ObjectTableName: string = "Measurement";
    public LabelColumnWidth: number = 160;
    public ControlColumnWidth: number = 240;
    public DataContext: MeasurementGeneralComponent = this;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IsContainer", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IsContainerMeasurement", this.ObjectTableName, false);
    }

    //Props
    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get ShortName() { return this.EntityPM.ShortName; }
    set ShortName(newValue: string) {
        if (this.EntityPM.ShortName != newValue) {
            this.EntityPM.ShortName = newValue;
        }
    }

    get Code() { return this.EntityPM.Code; }
    get InActive() { return this.EntityPM.InActive; }
    get IsContainer() { return this.EntityPM.IsContainer; }
    get IsContainerMeasurement() { return this.EntityPM.IsContainerMeasurement; }
}