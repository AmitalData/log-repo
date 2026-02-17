import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentsFilingPM} from '../../../../Common/EntityPMs/DocumentsFilingPM';

@Component({
    moduleId: module.id,
    templateUrl: './DocumentsFilingGeneralTabComponent.html',
})

export class DocumentsFilingGeneralTabComponent extends BaseComponent implements OnInit {

    public EntityPM: DocumentsFilingPM;
    public ObjectTableName: string = "DocumentsFiling";
    public LabelColumnWidth: number = 160;
    public ControlColumnWidth: number = 240;
    public DataContext: DocumentsFilingGeneralTabComponent = this;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
       
    }
 
    //Props
    //get Name() { return this.EntityPM.Name; }
    //set Name(newValue: string) {
    //    if (this.EntityPM.Name != newValue) {
    //        this.EntityPM.Name = newValue;
    //    }
    //}

    //get LocalName() { return this.EntityPM.LocalName; }
    //set LocalName(newValue: string) {
    //    if (this.EntityPM.LocalName != newValue) {
    //        this.EntityPM.LocalName = newValue;
    //    }
    //}

    //get ShortName() { return this.EntityPM.ShortName; }
    //set ShortName(newValue: string) {
    //    if (this.EntityPM.ShortName != newValue) {
    //        this.EntityPM.ShortName = newValue;
    //    }
    //}

    get Code() { return this.EntityPM.Code; }
    get Description() { return this.EntityPM.Description; }
    get Notes() { return this.EntityPM.Notes; }
    //get InActive() { return this.EntityPM.InActive; }
    //get IsContainer() { return this.EntityPM.IsContainer; }
    //get IsContainerMeasurement() { return this.EntityPM.IsContainerMeasurement; }
}