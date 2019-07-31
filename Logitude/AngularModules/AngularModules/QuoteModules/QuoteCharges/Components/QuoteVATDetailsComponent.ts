import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTotalVATPM} from '../../../Quote/EntityPMs/QuoteTotalVATPM';

@Component({
    moduleId: module.id,
    templateUrl: './QuoteVATDetailsComponent.html',
})

export class QuoteVATDetailsComponent {
    public LocalCurrencyCode: string = null;
    public SaleCurrencyCode: string = null;
    public SelectedCurrencyCode: string = null;
    public IsByLocalCurrency: boolean = false;
    public IsCurrencyFilterVisible: boolean = false;
    public ItemsSource: QuoteTotalVATPM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
    }

    SetWindowArgs(args: any) {
        this.IsByLocalCurrency = args['IsLocalCurrency'];
        this.SaleCurrencyCode = args['SaleCurrencyCode'];
        this.IsCurrencyFilterVisible = args['IsCurrencyFilterVisible'];
        this.SelectedCurrencyCode = this.IsByLocalCurrency ? this.LocalCurrencyCode : this.SaleCurrencyCode;
        this.ItemsSource = args['TotalVATs'];
    }

    OnSelectCurrency(myCurrencyCode: string) {
        this.SelectedCurrencyCode = myCurrencyCode;

        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }

        else {
            this.IsByLocalCurrency = false;
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
