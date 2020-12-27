import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { QuotePM } from '../../EntityPMs/QuotePM';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: "QuoteSaleCurrencyTypeComponent",
    templateUrl: "./QuoteSaleCurrencyTypeComponent.html",
    inputs: ['EntityPM'],
})

export class QuoteSaleCurrencyTypeComponent implements OnInit {

    EntityPM: QuotePM;
    ItemsSource: CodeNameClass[] = [];
    @Output() SelectedValueChanged = new EventEmitter();

    ngOnInit() {
        this.FillComboBox();
        this.InitSelectedItem();
    }

    FillComboBox() {
        var hasToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "QCM" && d.TenantNumber == SessionLocator.Tenant)[0]

        this.ItemsSource.push(new CodeNameClass("F", TextCodeTranslator.Translate("Quote.O.Charges.Fixed")));
        this.ItemsSource.push(new CodeNameClass("S", TextCodeTranslator.Translate("Quote.O.Charges.SameAsCost")));

        if (this.EntityPM.IsMultiCurrency || hasToggleFeature) {
            this.ItemsSource.push(new CodeNameClass("M", TextCodeTranslator.Translate("Quote.F.IsMultiCurrency")));
        }
    }

    InitSelectedItem() {
        if (this.EntityPM.IsMultiCurrency) {
            this.selectedItem = this.ItemsSource.filter(f => f.Code == "M")[0];
        }

        if (this.EntityPM.IsSaleCurrencySameAsCost) {
            this.selectedItem = this.ItemsSource.filter(f => f.Code == "S")[0];
        }

        else {
            this.selectedItem = this.ItemsSource.filter(f => f.Code == "F")[0];
        }
    }

    Reset(code: string) {
        this.selectedItem = this.ItemsSource.filter(f => f.Code == code)[0];
    }

    private selectedItem: CodeNameClass;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value: CodeNameClass) {
        if (this.selectedItem != value) {

            var isChanging = true;
            var newCode = value.Code;
            var oldCode = this.selectedItem.Code;

            this.selectedItem = value;

            if (newCode == "S") {
                var itemFrieght = this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT")[0];

                if (this.EntityPM.QuoteCharges.filter(d => d.IsAllIN == true && d.CostCurrencyId != itemFrieght.CostCurrencyId).length > 0) {

                    isChanging = false;

                    var messageWindow = new MessageWindow();

                    var message = "You can't switch to same as cost currency mode till you drop the all-in checks";

                    //if (newCode == "M") {
                    //    message = "You can't switch to multi-currency mode till you drop the all-in checks";
                    //}

                    messageWindow.Show(message);

                    messageWindow.WindowClosed.subscribe((event: any) => {
                        setTimeout(() => this.Reset(oldCode), 1);
                    });
                }
            }

            if (isChanging) {
                this.EntityPM.IsMultiCurrency = false;
                this.EntityPM.IsSaleCurrencySameAsCost = false;

                if (newCode == "M") {
                    this.EntityPM.IsMultiCurrency = true;
                }

                else if (newCode == "S") {
                    this.EntityPM.IsSaleCurrencySameAsCost = true;
                }

                this.SelectedValueChanged.emit(value.Code);
            }
        }
    }


}
