import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    selector: 'SupportTabComponent',
    templateUrl: './SupportTabComponent.html',
})

export class SupportTabComponent extends BaseComponent implements OnInit {
    public DataContext: SupportTabComponent = this;
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    public IsSupportDomainVisible: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }

    public IsEditingAllowed: boolean = false;
    ngOnInit() {
        if (this.EntityPM != null) {            
            this.SetUIProperties();
        }
    }

    private SetUIProperties() {
        if (this.SupportActivated && !AppTool.IsNullOrEmpty(this.EntityPM.SupportEmail) && AppTool.IsNullOrEmpty(this.SupportDomain)) {
            this.IsSupportDomainVisible = true;
        }

        this.UIProperties.SetEnabled("SupportDomain", this.ObjectTableName, this.SupportActivated);
    }

    get SupportDomain() { return this.EntityPM.SupportDomain; }
    set SupportDomain(newValue: string) {
        if (this.EntityPM.SupportDomain != newValue) {
            this.EntityPM.SupportDomain = newValue;
        }
    }

    get SupportActivated() { return this.EntityPM.SupportActivated; }
    set SupportActivated(newValue: boolean) {
        if (this.EntityPM.SupportActivated != newValue) {
            this.EntityPM.SupportActivated = newValue;

            this.SetUIProperties();
        }
    }

    //UpdateDomainClicked() {
    //    var domain: string;

    //    var email_splited: string[] = this.EntityPM.SupportEmail.split("@");

    //    if (email_splited.length > 0) {
    //        domain = email_splited[1];
    //    }

    //    this.SupportDomain = domain;
    //    this.SetUIProperties();
    //}
}
