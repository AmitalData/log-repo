import { Component, ChangeDetectorRef } from '@angular/core';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({

    template: `<div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;">
                      {{MyLabel}}
               </div>
            `
})

export class SupplierConsigneeListTemplate {

    public rowData: any;
    public fieldName: any;
    public MyLabel: string = "";
    public Direction: string = "";
    constructor(private CD: ChangeDetectorRef) {

    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.Direction = this.rowData['DirectionId'];
        this.MyLabel = '';

        if (this.Direction)
             
            switch (this.Direction) {
                case 'E': {
                    this.ShowConsigneeName();
                    break;
                }
                case 'C':  
                default: {
                    this.ShowShipperName();
                    break;
                }
            }
          
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    private ShowShipperName() {
        this.MyLabel = this.HasValue(this.rowData['ShipperName']) ? this.rowData['ShipperName'] : '';
    }

    private ShowConsigneeName() {
        this.MyLabel = this.HasValue(this.rowData['ConsigneeName']) ? this.rowData['ConsigneeName'] : '';
    }

    HasValue(field: any) {
        return !AppTool.IsNullOrEmpty(field);
    }

}
