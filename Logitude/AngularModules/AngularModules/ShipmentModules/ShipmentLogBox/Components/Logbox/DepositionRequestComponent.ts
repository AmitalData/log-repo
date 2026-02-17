declare var CopyText: any;

import {ShipmentArchiveFilter} from '../../../../Controls/ShipmentArchiveFilter';
import {TransportsFilter} from '../../../../Controls/TransportsFilter';
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SearchTextBox} from '../../../../Controls/SearchTextBox';
import {IconButton} from '../../../../Controls/IconButton';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import {Http, Response} from '@angular/http';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ShipmentComputedFieldExtendedService} from '../../../../Shipment/Services/ExtendedPMs/ShipmentComputedFieldExtendedService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
@Component({
    moduleId: module.id,
    templateUrl: './DepositionRequestComponent.html',

})

export class DepositionRequestComponent extends BaseComponent implements OnInit {

   shipmentComputedFieldExtendedService: ShipmentComputedFieldExtendedService;
    ShipmentId: string;
    VendorCodeId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityListService: EntityListService) {
        super();
  
        this.shipmentComputedFieldExtendedService = new ShipmentComputedFieldExtendedService();
    }

    VendorCode: string;
    VendorName: string;

    ngOnInit() {

    }

    ForwardershipmentNumber: string;
    DirectionId: string;
    ForwarderPartnerId: string;
    SetWindowArgs(args: any) {
        this.VendorCodeId = Guid.newGuid();
        this.ShipmentId = !AppTool.IsNullOrEmpty(args.ShipmentId) ? args.ShipmentId : null;
        this.ForwardershipmentNumber = !AppTool.IsNullOrEmpty(args.ForwarderShipmentNumber) ? args.ForwarderShipmentNumber : null;
        this.DirectionId = !AppTool.IsNullOrEmpty(args.DirectionId) ? args.DirectionId : null;
        this.ForwarderPartnerId = !AppTool.IsNullOrEmpty(args.ForwarderPartnerId) ? args.ForwarderPartnerId : null;
        
        if (!AppTool.IsNullOrEmpty(args.ImporterDepositionRequestDetails)) {
            var result = args.ImporterDepositionRequestDetails.indexOf("^") > -1 ? args.ImporterDepositionRequestDetails.split("^") : args.ImporterDepositionRequestDetails.split(",");
            if (result && result.length > 0) {
                this.VendorCode = result[0];
                this.VendorName = result.length > 1 ? result[1]:"";
            }
        }



    }

    CopyTextButtonClicked() {
 
        CopyText(this.VendorCodeId);
    }
    NewDepositionFormClcik() {
        ServiceLocator.SendTotangoUserActivity("CustomsShipper", "Deposition Link");
        var link = "https://forms.gov.il/globaldata/getsequence/getHtmlForm.aspx?formType=SOVE01_hasava@taxes.gov.il";
        var win = window.open(link, '_blank');
        win.focus();
    }

    MarkAsComplete() {
        if (!AppTool.IsNullOrEmpty(this.ShipmentId)) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

            this.shipmentComputedFieldExtendedService.GetMarkCompleteDepositionRequest(this.ShipmentId, this.DirectionId , this.ForwardershipmentNumber , this.ForwarderPartnerId).subscribe(myResult => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var pmResponse: ServiceResponse = myResult;
                if (!pmResponse.HasError) {
                    this.CurrentSession.CurrentWindow.Close("DepositionRequest");
                } else {
                  
                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                        var messageWindow: MessageWindow = new MessageWindow();
                        messageWindow.Show(pmResponse.ErrorsArray[0]);
                    }   
                }

            });
       
  
        } else this.CloseButtonClicked();
    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
