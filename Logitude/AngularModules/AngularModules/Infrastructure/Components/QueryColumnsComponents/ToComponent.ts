declare var window: any;
import {Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters'; 
import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Component({
    moduleId: module.id,

    selector: 'ToComponent',
    templateUrl: './ToComponent.html',
})

export class ToComponent implements OnInit {
    //@Output() Toevent = new EventEmitter();
    public rowData: any;
    public fieldName: any;
    public AddButtonEnabled: boolean = true;
    public TenantPM: TenantPM;
    public entityId: string;
    IsChecked: boolean = false;
 
    Key: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef,  private _entityListService: EntityListService) {
        this.TenantPM = InfraSettings.TenantPM;

    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.Key = Guid.newGuid() + fieldName;

        if (this.rowData.Email) {

            if (fieldName == "To" && window.ToEmailLists) {
                var item = window.ToEmailLists.filter(d=> d.toLowerCase() == this.rowData.Email.toLowerCase())[0];
                if (item) {
                    this.IsChecked = true;
                } else this.IsChecked = false;

            }
            else if (fieldName == "Cc" && window.CcEmailLists) {
                var item = window.CcEmailLists.filter(d=> d.toLowerCase() == this.rowData.Email.toLowerCase())[0];
                if (item) {
                    this.IsChecked = true;
                } else this.IsChecked = false;
            }

            else if (fieldName == "Bcc" && window.BccEmailLists) {
                var item = window.BccEmailLists.filter(d=> d.toLowerCase() == this.rowData.Email.toLowerCase())[0];
                if (item) {
                    this.IsChecked = true;
                } else this.IsChecked = false;
            }
            this.Destroyed();
        }

        if (this.rowData.Id) {

            if (fieldName == "SelectedUser" && window.ToEmailLists) {
                var item = window.ToEmailLists.filter(d => d.toLowerCase() == this.rowData.Id.toLowerCase())[0];
                if (item) {
                    this.IsChecked = true;
                } else this.IsChecked = false;

            }
          
            this.Destroyed();
        }
   
    }

    ngOnInit() {

    }

 

    Checkclick(item: any) {

        //this.IsChecked = !this.IsChecked;
        if (item.Email) {
            var select = new ParameterInput(this.fieldName, item.Email, true, item.Id);
            this.Destroyed();
            this.CurrentSession.SessionEvent.emit(select);
        }
    }

    Destroyed() {
        var isDestroyed: boolean = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    }


}


class ParameterInput {
    FieldName: string;
    Email: string;
    IsCheck: boolean;
    UserId: string;
    constructor(fieldName: string, email: any, isCheck: boolean,userId:string = null) {
        this.FieldName = fieldName;
        this.Email = email;
        this.IsCheck = isCheck;
        this.UserId = userId;
    }


}
