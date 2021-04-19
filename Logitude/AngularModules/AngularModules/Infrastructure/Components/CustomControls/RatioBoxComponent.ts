import { Component, OnInit, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../Tools';
import { SessionLocator } from '../../Utilities/SessionLocator';

@Component({
    selector: 'RatioBox',
    
    templateUrl: './RatioBoxComponent.html',
    inputs: ['EntityPM', 'ObjectFieldName', 'ObjectTableName', 'DataContext', 'IsEnabled'],
})

export class RatioBoxComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: any = null;
    public DataContext: any = null;
    public ObjectFieldName: string = null;
    public ObjectTableName: string = null;
    public IDataContext: RatioBoxComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.Listen();
    }

    private Listen() {
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "RatioBoxValueChanged") {
                this.iRatio = this.DataContext[this.ObjectFieldName];
                //this.Validate();
            }            
        });
    }

    private SessionEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    ngOnInit() {
        this.iRatio = this.DataContext[this.ObjectFieldName];
        this.Validate();
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
            this.Validate();

            this.CurrentSession.FireEvent("RatioBoxValueChanged");
        }
    }

    Validate() {
        if (!this.iRatio) {
            this.EntityPM.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio field is required");
            this.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio field is required");
        }

        else {
            if (this.iRatio <= 10 && this.iRatio >= 1) {
                this.EntityPM.UIProperties.SetValidity("Ratio", this.ObjectTableName, true, null);
                this.UIProperties.SetValidity("Ratio", this.ObjectTableName, true, null);
            }

            else {
                this.EntityPM.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio must be between 1-10");
                this.UIProperties.SetValidity("Ratio", this.ObjectTableName, false, "Ratio must be between 1-10");
            }
        }
    }

    OnLostFocus() {
        this.iRatio = null;

        setTimeout(() => {
            this.iRatio = this.DataContext[this.ObjectFieldName];
            this.Validate();
        }, 1)

        //if (AppTool.IsNullOrEmpty(this.Ratio)) {
        //    if (this.EntityPM['IsDirty']) {

        //        this.iRatio = 0;

        //        setTimeout(() => {
        //            this.NumericButtonClicked(true);
        //        }, 1)
        //    }
        //}
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
