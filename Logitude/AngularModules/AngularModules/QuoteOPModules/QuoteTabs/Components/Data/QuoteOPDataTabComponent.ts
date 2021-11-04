import { Component } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";

@Component({
    selector: 'QuoteOPDataTabComponent',
    templateUrl: './QuoteOPDataTabComponent.html',
    styleUrls: ['./QuoteOPDataTabComponent.scss'],
})

export class QuoteOPDataTabComponent extends BaseComponent  {
    public EntityPM: QuoteOPPM ;
    formGroup = new FormGroup({});
    public ObjectTableName: string = "QuoteOP";
    public DataContext = this;
    public IsSubjectVisible: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }
}