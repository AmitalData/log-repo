
import {Component,ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
//import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ReconciliationLineListTemplate.html',
})

export class ReconciliationLineListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public fieldValue: any;

    public isRTL: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;

    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    GetTransacionAmount(){
        if (AppTool.IsNullOrZero(this.rowData.ForeignAmountCredit)) {
            return  -1 * this.rowData.ForeignAmountDebit;
        } else {
            return this.rowData.ForeignAmountCredit;
        }
    }

    OpenJournal(id) {
        // open Journal screen
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                });
        }
    }



}
