import { Component, ChangeDetectorRef } from '@angular/core';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './SupplierInvoiceItemsTaxListTemplate.html',
})

export class SupplierInvoiceItemsTaxListTemplate {

    public rowData: any;
    public fieldName: any;

    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.CD.detectChanges();
    }
    ShowTaxMore() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton =logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.ToShowCloseButton(true);
        
        
        //logitudeWindow.Title = TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");;
        logitudeWindow.WindowArgs = this.rowData;
        logitudeWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Taxes/ItemTaxesMoreFieldsComponent');
    }


    public get MyTaxToPay() {

        let taxToPay = (this.rowData.TaxAmount == null ? 0 : this.rowData.TaxAmount) + (this.rowData.DeferedTaxAmount == null ? 0 : this.rowData.DeferedTaxAmount);

        if (taxToPay == 0) {
            return null;
        }
        return taxToPay;

    }
}
