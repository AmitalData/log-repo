import {Component, ChangeDetectorRef, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {MultiCertificatesService} from '../../../Customs/Services/Others/MultiCertificatesService';
import {CertificateConnectedItem} from '../../../Customs/DataContract/CertificateConnectedItem';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {DeclarationEventManager} from '../../../Customs/Utilities/DeclarationEventManager';

@Component({
    moduleId: module.id,

    selector: 'CertificateTextBoxComponent',
    templateUrl: './CertificateTextBoxComponent.html',
})


export class CertificateTextBoxComponent
    extends BaseComponent
    implements OnDestroy
{


    public rowData: any;
    public fieldName: any;

    public DataContext: any = this;
    public entityId: string;
    multiCertificatesService: MultiCertificatesService = new MultiCertificatesService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef) {
        super();
       
    }


    
    private _SubDisplayModeChanged;
    ngOnDestroy() {
        console.log("CertificateTextBoxComponent:ngOnDestroy");
    
        if (this._SubDisplayModeChanged) {
            this._SubDisplayModeChanged.unsubscribe();
            this._SubDisplayModeChanged = null;
        }


    }
    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
   
        this.CatalogNumber = rowData.CatalogNumber;
        this._SubDisplayModeChanged=
        DeclarationEventManager.DisplayModeChanged.subscribe((IsDisplayOnly: any) => {
            if (IsDisplayOnly) {
                this.UIProperties.SetEnabled("CatalogNumber", "Customs.SupplierInvoiceItem", false);
            } else {
                this.UIProperties.SetEnabled("CatalogNumber", "Customs.SupplierInvoiceItem", true);

            }

        });

    }
    private catalogNumber: string;
    public get CatalogNumber() { return this.catalogNumber; }
    public set CatalogNumber(newValue: string) { this.catalogNumber = newValue; }


    public cellClicked;
    clicked() {
        this.CurrentSession.PseventRowSelectEvent.emit("certificate");
        this.cellClicked = true;
        this.cd.detectChanges();
    }

    onBlur() {
        this.cellClicked = false;
        this.cd.detectChanges();
        //InvokeOperation op = trigger.CustomContext.UpdateSuppkierInvoiceItemCatalogNumber(DeclarationId, CatalogNumber, InvoiceCounterKey, LineNumber, trigger.entityPM.Tenant);
        var item: CertificateConnectedItem = this.rowData;
        item.CatalogNumber = this.CatalogNumber;
        this.multiCertificatesService.PutSupplierInvoiceItemCatalogNumber(item)
            .subscribe((response: ServiceResponse) => {
                if (!response.HasError) {

                
                }
            });
    
    }
  




}
