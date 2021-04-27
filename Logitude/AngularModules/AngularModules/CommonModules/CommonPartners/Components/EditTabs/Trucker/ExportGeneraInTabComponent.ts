declare var window: any;
import { Component, OnInit } from '@angular/core';
import { TruckerPM } from '../../../../../Common/EntityPMs/TruckerPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: './ExportGeneraInTabComponent.html',
})

export class ExportGeneraInTabComponent extends BaseComponent implements OnInit {
    public EntityPM: any = null;
    public ObjectTableName: string = "Trucker";
    public DataContext: this;
    public _IsReady: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(private entityArgs: EntityArgs) {
        super();
        this._entityResourceService.getEntityResourceByTableName("Trucker").subscribe(response => {
            this.EntityPM = this.entityArgs.EntityPM;
            this._IsReady = true;
            //this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        });
    }

    ngOnInit() {
    }

    get Code() { return this.EntityPM.Code; }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }
     
    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get Website() { return this.EntityPM.Website; }
    set Website(newValue: string) {
        if (this.EntityPM.Website != newValue) {
            this.EntityPM.Website = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }

    get TransmitToPort() { return this.EntityPM.TransmitToPort; }
    set TransmitToPort(newValue: boolean) {
        if (this.EntityPM.TransmitToPort != newValue) {
            this.EntityPM.TransmitToPort = newValue;
        }
    }

    get Remark() { return this.EntityPM.Remark; }
    set Remark(newValue: string) {
        if (this.EntityPM.Remark != newValue) {
            this.EntityPM.Remark = newValue;
        }
    }
}
