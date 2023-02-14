import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ARInvoicePM } from '../../EntityPMs/ARInvoicePM';
import { ARInvoiceLinePM } from '../../EntityPMs/ARInvoiceLinePM';
import { FormatTool } from '../../../Infrastructure/Tools';

@Component({
    templateUrl: './ARInvoiceLinesVATAmountsAdjustmentsComponent.html',
})

export class ARInvoiceLinesVATAmountsAdjustmentsComponent {
    private ARInvoicePM: ARInvoicePM;
    private ARInvoiceLinesVATAmountsAdjustments: any;
    private CurrentSession = SessionLocator.SelectedSession;
    public MessageHeader: string = "Due to the SAT dispositions, VAT amount are calculated per line. The following lines should be adjusted to resolve the differences between the total of lines VAT and VAT of Invoice total.";
    public MessageLines: ARInvoiceLinesVATAmountsMessageLines[] = [];
    constructor() {
    }

    SetWindowArgs(args: any) {
        this.ARInvoicePM = args['ARInvoicePM'];
        this.ARInvoiceLinesVATAmountsAdjustments = args['ARInvoiceLinesVATAmountsAdjustments'];
        this.BuildARInvoiceLinesVATAmountsMessageLines();
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    ContinueClicked() {
        this.UpdateARInvoiceVatAmounts();
        this.CurrentSession.CloseCurrentWindowEmit("UpdateInvoice");
    }


    private BuildARInvoiceLinesVATAmountsMessageLines() {
        if (!this.ARInvoiceLinesVATAmountsAdjustments) return;
        this.ARInvoiceLinesVATAmountsAdjustments.CorrectedARInvoiceTrasladoLines.forEach(line => {
            let originalLine = this.GetOriginalInvoiceLineToBeCorrected(line);
            if (originalLine && line) this.AddMessageLine(originalLine, line);
        });

        this.ARInvoiceLinesVATAmountsAdjustments.CorrectedARInvoiceRetencionLines.forEach(line => {
            let originalLine = this.GetOriginalInvoiceLineToBeCorrected(line);
            if (originalLine && line) this.AddMessageLine(originalLine, line);
        });
    }
    AddMessageLine(originalLine: ARInvoiceLinePM, line: any) {
        let newLine = new ARInvoiceLinesVATAmountsMessageLines();
        newLine.OriginalAmount = FormatTool.FormatNumber(originalLine.InvoiceCurrencyAmount);
        newLine.NewAmount = FormatTool.FormatNumber(line.InvoiceCurrencyAmount);
        newLine.DifferenceAmount = FormatTool.FormatNumber(originalLine.InvoiceCurrencyAmount - line.InvoiceCurrencyAmount);
        newLine.Description = originalLine.Description;
        this.MessageLines.push(newLine);
    }

    private UpdateARInvoiceVatAmounts() {
        if (!this.ARInvoiceLinesVATAmountsAdjustments) return;
        this.ARInvoiceLinesVATAmountsAdjustments.CorrectedARInvoiceTrasladoLines.forEach(line => {
            this.UpdateARInvoiceLineVatAmount(line);
        });

        this.ARInvoiceLinesVATAmountsAdjustments.CorrectedARInvoiceRetencionLines.forEach(line => {
            this.UpdateARInvoiceLineVatAmount(line);
        });
    }

    private UpdateARInvoiceLineVatAmount(line: any) {
        let originalLine = this.GetOriginalInvoiceLineToBeCorrected(line);
        if (!originalLine) return;
        if (originalLine.ForiegnCurrencyAmount == originalLine.InvoiceCurrencyAmount) {
            originalLine.ForiegnCurrencyAmount = line.InvoiceCurrencyAmount;
        }
        originalLine.InvoiceCurrencyAmount = line.InvoiceCurrencyAmount;
        this.ARInvoicePM.VatsAmountsManulAdjuested = true;
    }

    GetOriginalInvoiceLineToBeCorrected(line: any) {
        if (!line) return;
        return this.ARInvoicePM.InvoiceLines.filter(Invoiceline => Invoiceline.ProfitCurrencyAmount == line.ProfitCurrencyAmount && Invoiceline.VatTypeId == line.VatTypeId && Invoiceline.InvoiceCurrencyAmount != line.InvoiceCurrencyAmount)[0];
    }
}

class ARInvoiceLinesVATAmountsMessageLines {
    public OriginalAmount: string;
    public NewAmount: string;
    public DifferenceAmount: string;
    public Description: string;
}
