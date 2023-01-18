import {Component, ChangeDetectorRef} from '@angular/core';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { DataCustomObjectList } from '../../EntityLists/DataCustomObjectList';
import { ReferenceCustomObjectList } from '../../EntityLists/ReferenceCustomObjectList';

@Component({
    
    templateUrl: './InfrastructureFieldTemplateComponent.html',
})

export class InfrastructureFieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public isRTL: boolean = false;
    public IsCustomObjectTable: boolean = false;
    constructor(private cd: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        
    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        this.IsCustomObjectTable = this.Entity instanceof ReferenceCustomObjectList || this.Entity instanceof DataCustomObjectList;

        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];

            this.SetPrivateLabelIdFieldValue();

            if (this.cd) {
                var isDestroyed: boolean = this.cd['destroyed'];
                if (!isDestroyed) {
                    this.cd.detectChanges();
                }
            }
        }
    }

    private SetPrivateLabelIdFieldValue() {
        if (this.ObjectTableName == "TenantManagement" && this.FieldName == "PrivateLabelId") {
            this.FieldValue = this.Entity["PrivateLabelName"];
        }
    }

    GetStatusColor(){
        if (this.Entity.StatusCode == "D") { // D- Done
            return 'green';
        } else if (this.Entity.StatusCode == "F") { // F- Failed
            return 'red';
        } else if (this.Entity.StatusCode == "I") { // I- In Progress
            return 'blue';
        } else{
            return 'black';
        }
    }

}
