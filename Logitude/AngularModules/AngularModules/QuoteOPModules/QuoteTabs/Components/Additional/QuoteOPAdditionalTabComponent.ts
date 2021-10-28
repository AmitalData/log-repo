import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";

@Component({
    selector: 'QuoteOPAdditionalTabComponent',

    templateUrl: './QuoteOPAdditionalTabComponent.html',
})

export class QuoteOPAdditionalTabComponent extends BaseComponent implements OnInit {
    public EntityPM: QuoteOPPM;
    public ObjectTableName: string = "QuoteOP";
    public DataContext = this;
    isReady = false;
    public IsSubjectVisible: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(public entityArgs: EntityArgs) {
        super();
        this._entityResourceService.getEntityResourceByTableName("QuoteOP").subscribe(response => {
            this.EntityPM = this.entityArgs.EntityPM;
        });
    }

    ngOnInit(): void {
        this.isReady = true; 
    }
 


    get ETD() { return this.EntityPM.ETD; }
    set ETD(newValue: Date) {
        if (this.EntityPM.ETD != newValue) {
            this.EntityPM.ETD = newValue;
        }
    }

    get ETA() { return this.EntityPM.ETA; }
    set ETA(newValue: Date) {
        if (this.EntityPM.ETA != newValue) {
            this.EntityPM.ETA = newValue;
        }
    }

    get SalesmanUserId() { return this.EntityPM ? this.EntityPM.SalesmanUserId : ""; }
    set SalesmanUserId(newValue: string) {
        if (this.EntityPM.SalesmanUserId != newValue) {
            this.EntityPM.SalesmanUserId = newValue;
        }
    }
    get CreatedByUserId() { return this.EntityPM.CreatedByUserId; }
    set CreatedByUserId(newValue: string) {
        if (this.EntityPM.CreatedByUserId != newValue) {
            this.EntityPM.CreatedByUserId = newValue;
        }
    }
    get DepartmentId() { return this.EntityPM.DepartmentId; }
    set DepartmentId(newValue: string) {
        if (this.EntityPM.DepartmentId != newValue) {
            this.EntityPM.DepartmentId = newValue;
        }
    }
    get BranchId() { return this.EntityPM.BranchId; }
    set BranchId(newValue: string) {
        if (this.EntityPM.BranchId != newValue) {
            this.EntityPM.BranchId = newValue;
        }
    }
    get ValueOfGoods() { return this.EntityPM.ValueOfGoods; }
    set ValueOfGoods(newValue: number) {
        if (this.EntityPM.ValueOfGoods != newValue) {
            this.EntityPM.ValueOfGoods = newValue;
        }
    }
    get ValueOfGoodsCurrencyId() { return this.EntityPM.ValueOfGoodsCurrencyId; }
    set ValueOfGoodsCurrencyId(newValue: string) {
        if (this.EntityPM.ValueOfGoodsCurrencyId != newValue) {
            this.EntityPM.ValueOfGoodsCurrencyId = newValue;
        }
    }
    get TransitTime() { return this.EntityPM.TransitTime; }
    set TransitTime(newValue: string) {
        if (this.EntityPM.TransitTime != newValue) {
            this.EntityPM.TransitTime = newValue;
        }
    }
    get DepartureFrequency() { return this.EntityPM.DepartureFrequency; }
    set DepartureFrequency(newValue: string) {
        if (this.EntityPM.DepartureFrequency != newValue) {
            this.EntityPM.DepartureFrequency = newValue;
        }
    }
}