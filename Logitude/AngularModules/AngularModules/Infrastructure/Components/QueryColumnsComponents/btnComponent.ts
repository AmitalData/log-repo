declare var window: any;
import {Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CardList} from '../../../Common/EntityLists/CardList';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
@Component({
    moduleId: module.id,

    selector: 'btnComponent',
    templateUrl: './btnComponent.html',
})

export class btnComponent implements OnInit {

    public rowData: any;
    public fieldName: any;
    public InUseVisibile: boolean = true;
    public TenantPM: TenantPM;
    public entityId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, private _entityListService: EntityListService) {
        this.TenantPM = InfraSettings.TenantPM;
        this.LoadShippingLineListMethod();
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        //this.Check();
        this.InUseVisibile = this.rowData.InUse;
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
                    this.GetInUseCarrier( "SL", this.rowData.Code);
                    break;
                }

            case "Airline":
                {
                    this.GetInUseCarrier("AL", this.rowData.Code);
                    break;
                }

            case "Port":
                {
                    var y = window.Ports.filter(x => x.Tenant === this.TenantPM.Id && x.Code == this.rowData.Code && x.CountryCode == this.rowData.CountryCode);
                    if (y.length != 0) {
                       // this.AddButtonVisibile = false;
                    }

                    else {
                       // this.AddButtonVisibile = true;
                    }

                    break;
                }

            case "Warehouse":
                {
                    this.GetInUseCarrier("WH", this.rowData.Code);
                    break;
                }
        }
    }

    DoItClick() {
        this.entityId = this.rowData.Id;
        switch (this.fieldName) {
            case "ShippingLine":
            case "Airline":
            case "Warehouse":
                {
                    this.StartBusyIndicator("Adding " + this.fieldName + " to your list");
                    this.GetCarrierCopyToCurrentTenant();
                    break;
                }
            case "Port":
                {
                    this.StartBusyIndicator("Adding " + this.fieldName + " to your list");
                    this.GetPortCopyToCurrentTenant();
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

    public FireEvent(eventArgs: any) {
        this.CurrentSession.SessionEvent.emit(eventArgs);
    }

    GetPortCopyToCurrentTenant() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetPortCopyToCurrentTenant(this.entityId).subscribe(myResult => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {

                CachedDataManager.RefreshTableData(this.fieldName, true);

                this.IsCompleted = true;
                this.RefreshDate();
            }
            this.StopBusyIndicator();
        });
    }

    private IsCompleted: boolean = false;
    GetCarrierCopyToCurrentTenant() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetCarrierCopyToCurrentTenant(this.entityId).subscribe(myResult => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {

                CachedDataManager.RefreshTableData(this.fieldName, true);
                CachedDataManager.RefreshTableData("Carrier", true);
                this.IsCompleted = true;
                this.RefreshDate();

            }
            this.StopBusyIndicator();
        });
    }
    
    GetInUseCarrier( type: string, code: string) {
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
