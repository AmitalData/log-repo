import {Component, ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools'; 

@Component({

    template: `<div title={{MyLabel}} style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;">
                      {{MyLabel}}
               </div>
            `
})

export class CustomReferenceListTemplate {

    public rowData: any;
    public fieldName: any;
    public MyLabel: string = "";
    public isPrivateLabel: boolean = SessionLocator.PrivateLableSettings ? true : false;

    constructor(private CD: ChangeDetectorRef) {

    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.MyLabel = "";
        this.GetLogboxReferenceLabel();
        this.GetPrivateLabelReferenceLabel();

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }
    GetPrivateLabelReferenceLabel() {
        if (!this.isPrivateLabel) return; 
         
        if (AppTool.IsNullOrEmpty(this.rowData['ForwarderShipmentNumber'])) {
            this.GetMyShipmentsCustomerReferenceColumn(); 
        } else {
            this.GetAgentShipmentsCustomerReferenceColumn();
        }
    }


    private GetAgentShipmentsCustomerReferenceColumn() {
        this.MyLabel = this.rowData['CustomerReference3']; 
    }

    private GetMyShipmentsCustomerReferenceColumn() {
        var cstomerReference = AppTool.IsNullOrEmpty(this.rowData['CustomerReference3']) ? "" : this.rowData['CustomerReference3'];
        var invoiceNumberReference = AppTool.IsNullOrEmpty(this.rowData['PrivateLabelInvoiceNumber']) ? "" : this.rowData['PrivateLabelInvoiceNumber'];
        this.MyLabel = cstomerReference + (AppTool.IsNullOrEmpty(invoiceNumberReference) ? "" : ("/"+invoiceNumberReference));
    }

    private GetLogboxReferenceLabel() {
        if (this.isPrivateLabel) return;

        if (AppTool.IsNullOrEmpty(this.rowData['ForwarderShipmentNumber'])) {
            this.MyLabel = this.GetCustomerReference3Or1And2();
        } else {
            this.MyLabel = this.rowData['CustomerReference3']; 
        }
    }

    private GetCustomerReference3Or1And2() {
        return AppTool.IsNullOrEmpty(this.rowData['CustomerReference3']) ? this.GetCustomerReference1And2() : this.rowData['CustomerReference3'];
    }

    private GetCustomerReference1And2() {
        if (!AppTool.IsNullOrEmpty(this.rowData['CustomerReference1']) && !AppTool.IsNullOrEmpty(this.rowData['CustomerReference2'])) {
            return this.rowData['CustomerReference1'] + ' / ' + this.rowData['CustomerReference2'];
        }

        if (!AppTool.IsNullOrEmpty(this.rowData['CustomerReference1'])) {
            return this.rowData['CustomerReference1'];
        }

        if (!AppTool.IsNullOrEmpty(this.rowData['CustomerReference2'])) {
            return this.rowData['CustomerReference2'];
        }

        return '';
    }
}
