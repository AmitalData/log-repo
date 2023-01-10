declare var window: any;
import { Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { DigitalCustomizationService, CheckObjectFieldExistenceRequest } from '../../../Infrastructure/Services/WebServices/DigitalCustomizationService';

@Component({
    selector: 'DigitalButtonComponent',
    templateUrl: './DigitalButtonComponent.html',
})

export class DigitalButtonComponent implements OnInit {

    public rowData: any;
    public fieldName: any;
    public TenantPM: TenantPM;
    public entityId: string;
    public InUseVisibile: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    digitalCustomizationService: DigitalCustomizationService;

    constructor(private CD: ChangeDetectorRef, private _entityListService: EntityListService) {
        this.TenantPM = InfraSettings.TenantPM;
        this.digitalCustomizationService = new DigitalCustomizationService();
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.Check();
        this.fieldName = fieldName;
        this.InUseVisibile = this.rowData.InUse;

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    ngOnInit() {

    }

    Check() {
        this.GetInUseObjectField(this.rowData);
    }

    DoItClick() {
        this.entityId = this.rowData.Id;
        this.StartBusyIndicator("Updating" + this.fieldName + " to your list");
        this.GetObjectField();
    }

    RefreshDate() {
        this.InUseVisibile = true;

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
        if (this.IsCompleted) {
            this.FireEvent("TenantImport");
        }
    }

    RefreshDateUpdated() {

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
        if (this.IsCompleted) {
            this.FireEvent("TenantImport");
        }
    }

    public FireEvent(eventArgs: any) {
        this.CurrentSession.SessionEvent.emit(eventArgs);
    }


    private IsCompleted: boolean = false;
    GetObjectField() {
        
    }

    GetInUseObjectField(rowData) {
        var record = new CheckObjectFieldExistenceRequest();
        record.ObjectTableId = rowData.ObjectTableId;
        record.FieldCode = rowData.FieldCode;

        this.digitalCustomizationService.CheckIfFieldInuse(record).subscribe((myResult) => {
            this.InUseVisibile = myResult.Result;
        });

    }

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
}
