import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../Components/LogitudeComponents/BaseComponent';

@Component({
    selector: 'RatioBox',
    moduleId: module.id,
    templateUrl: './RatioBoxComponent.html',
    inputs: ['EntityPM', 'ObjectFieldName', 'ObjectTableName', 'DataContext', 'IsEnabled'],
})

export class RatioBoxComponent extends BaseComponent implements OnInit {
    public EntityPM: any = null;
    public DataContext: any = null;
    public ObjectFieldName: string = null;
    public ObjectTableName: string = null;
    public IDataContext: RatioBoxComponent = this;
    constructor() {
        super();
    }

    ngOnInit() {
        this.iRatio = this.DataContext[this.ObjectFieldName];
    }

    private isEnabled: boolean = true;
    public get IsEnabled() { return this.isEnabled; }
    public set IsEnabled(value: boolean) {
        if (this.isEnabled != value) {
            this.isEnabled = value;
            this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, value);
        }
    }

    private iRatio: number = null;
    public get Ratio() { return this.iRatio; }
    public set Ratio(value: number) {
        if (this.iRatio != value) {
            this.iRatio = value;

            this.DataContext[this.ObjectFieldName] = value;

            if (value <= 10 && value >= 1) {
                this.EntityPM.UIProperties.SetValidity("Ratio", this.ObjectTableName, true, null);
                this.UIProperties.SetValidity("Ratio", this.ObjectTableName, true, null);
            }

            else {
                this.EntityPM.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio must be between 1-10");
                this.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio must be between 1-10");
            }
        }
    }

    NumericButtonClicked(isIncreas: boolean) {
        if (this.IsEnabled) {
            if (isIncreas) {
                if (this.Ratio < 10) {
                    this.Ratio += 1;
                }
            }

            else {
                if (this.Ratio > 1) {
                    this.Ratio -= 1;
                }
            }
        }
    }

}
