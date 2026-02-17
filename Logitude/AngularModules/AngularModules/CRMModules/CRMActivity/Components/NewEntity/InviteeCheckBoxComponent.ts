declare var window: any;
import {Component, ChangeDetectorRef} from '@angular/core';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {EntityPMServiceResponse} from '../../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    moduleId: module.id,
    selector: 'InviteeCheckBoxComponent',
    templateUrl: './InviteeCheckBoxComponent.html',
})

export class InviteeCheckBoxComponent extends BaseComponent {
    public DataContext = this;
    public rowData: any;
    public fieldName: any;
    public AddButtonEnabled: boolean = true;
    public TenantPM: TenantPM;
    public entityId: string;

    private isChecked: boolean = false;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            this.isCheckedFlag = true;
            this.FireCheckedEvent();
        }
    }

    Key: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef, private _entityListService: EntityListService) {
        super();
        this.TenantPM = InfraSettings.TenantPM;
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.Key = Guid.newGuid() + fieldName;

        if (this.rowData.Email) {
            if (fieldName == "Required" && window.RequiredList) {
                var item = window.RequiredList.filter(d => d.Email.toLowerCase() == this.rowData.Email.toLowerCase() && d.ContactId == this.rowData.Id)[0];
                if (item) {
                    this.IsChecked = true;
                } else {
                    this.IsChecked = false;
                }
            }
            else if (fieldName == "Optional" && window.OptionalList) {
                var item = window.OptionalList.filter(d => d.Email.toLowerCase() == this.rowData.Email.toLowerCase() && d.ContactId == this.rowData.Id)[0];
                if (item) {
                    this.IsChecked = true;
                } else {
                    this.IsChecked = false;
                }
            }
     
            this.Destroyed();
            this.isCheckedFlag = false;
        }
    }

    Checkclick(item: any) {
        if (item.Email) {
            this.cd.detectChanges();
            this.isClickedFlag = true;
            this.selectedItem = item;
            this.FireCheckedEvent();
        }
    }

    private isCheckedFlag: boolean = false;
    private isClickedFlag: boolean = false;
    private selectedItem: any = null;
    FireCheckedEvent() {
        if (this.selectedItem != null && this.isCheckedFlag && this.isClickedFlag) {
            var select = new InviteeParameterInput(this.fieldName, this.selectedItem.Email, this.selectedItem.Id, this.selectedItem.EnglishName, this.IsChecked);
            this.Destroyed();
            this.CurrentSession.SessionEvent.emit({ Name: "InviteeCheckBoxComponent", select: select });
            this.selectedItem = null;
            this.isCheckedFlag = false;
            this.isClickedFlag = false;
        }
    }
    Destroyed() {
        var isDestroyed: boolean = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    }

}

class InviteeParameterInput {
    FieldName: string;
    Email: string;
    ContactId: string;
    ContactName: string;
    IsCheck: boolean;
    constructor(fieldName: string, email: any,contactid:string, contactname:string, isCheck: boolean) {
        this.FieldName = fieldName;
        this.Email = email;
        this.ContactId = contactid;
        this.ContactName = contactname;
        this.IsCheck = isCheck;
    }
}
