declare var window: any;
import { Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { PartnersDomainService } from '../../../Common/Services/PartnersDomainService';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CardList } from '../../../Common/EntityLists/CardList';
import { CachedDataManager } from '../../../Infrastructure/Utilities/CachedDataManager';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
@Component({
    moduleId: module.id,

    selector: 'btnUpdateComponent',
    templateUrl: './btnUpdateComponent.html',
})

export class btnUpdateComponent implements OnInit {

    public rowData: any;
    public fieldName: any;
    public TenantPM: TenantPM;
    public entityId: string;
    public InUseVisibile: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, private _entityListService: EntityListService) {
        this.TenantPM = InfraSettings.TenantPM;
        this.LoadShippingLineListMethod();
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.InUseVisibile = this.rowData.InUse;

        //this.Check();
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    ngOnInit() {

    }

    Check() {
        switch (this.fieldName) {
            case "ShippingLine":
                {
                    this.GetInUseCarrier("SL", this.rowData.Code);
                    break;
                }

            case "Airline":
                {
                    this.GetInUseCarrier("AL", this.rowData.Code);
                    break;
                }

            case "Port":
                {

                    break;
                }          
        }
    }

    DoItClick() {
        this.entityId = this.rowData.Id;
        switch (this.fieldName) {
            case "ShippingLine":
            case "Airline":
                {
                    this.StartBusyIndicator("Updating" + this.fieldName + " to your list");
                    this.GetCarrierUpdate();
                    break;
                }
        }
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
    GetCarrierUpdate() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetCarrierUpdate(this.entityId).subscribe(myResult => {
            var mm: ServiceResponse = myResult;
            this.StopBusyIndicator();
            if (!mm.HasError) {
                CachedDataManager.RefreshTableData(this.fieldName, true);
                CachedDataManager.RefreshTableData("Carrier", true);
                this.IsCompleted = true;
                this.RefreshDateUpdated();

            }
        });
    }

    GetInUseCarrier(type: string, code: string) {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetInUseCarrier(type, code).subscribe(myResult => {
            this.InUseVisibile = myResult.Result;

        });
    }

    public ShippingLinesList: CardList[] = [];

    private LoadShippingLineListMethod() {
        var m = new CardList();
        m.Code = "ACLU";
        this.ShippingLinesList.push(m);
    }

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
}
