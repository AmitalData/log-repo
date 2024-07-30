
import { Component } from '@angular/core';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { AppTool } from 'Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';


@Component({
    templateUrl: './UpdateCertificateOfOriginGeneralFieldComponent.html',
})

export class UpdateCertificateOfOriginGeneralFieldComponent extends BaseComponent {
    DataContext: any = this;
    public ItemsSource: ObservableCollection;
    CertificateOfOriginPM: CertificateOfOriginPM;
    Validate: any;
    UpdateField: string;
    FinalValue: any;
    LookUpTableName: string;
    ObjectTableName: string;
    QueryFilterItems: any;
    IsMultiline: boolean;
    Title: string;
    IsItemsWithNoValue: boolean;
    ItemsWithNoValueTitle: string;
    public ValidationErrorsList: string[] = [];
    IsDisplayOnly: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public hasOcr: boolean;
    public PreceduralFilterItems: ApiQueryFilters;

    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.UIProperties.SetEnabled("FromNumber", null, false);
        this.UIProperties.SetEnabled("ToNumber", null, false);
        this.IsAddButtonEnabled = false;
        this.hasOcr = FeatureLocator.HasFeaturePermession("Customs.Declaration", "OCR");

    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.CertificateOfOriginPM = args.CertificateOfOriginPM;
            this.UpdateField = args.UpdateField;
            this.LookUpTableName = args.LookUpTableName;
            this.ObjectTableName = args.ObjectTableName;
            this.IsMultiline = args.IsMultiline;
            this.QueryFilterItems = args.QueryFilterItems;
            this.Validate = args.Validate;
            this.Title = args.Title;
            this.ItemsWithNoValueTitle = args.ItemsWithNoValueTitle;
            this.IsItemsWithNoValue = args.IsItemsWithNoValue;
            this.PreceduralFilterItems = new ApiQueryFilters();
            this.PreceduralFilterItems.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        }
        this.UpdateItemsWithNoValue = true;
    }

    BuildActualInvoiceLines() {
        this.ItemsSource.Clear();

        var dataString: string = "";
    }

    fromNumber: number;
    get FromNumber() { return this.fromNumber }
    set FromNumber(value: number) { this.fromNumber = value; }

    toNumber: number;
    get ToNumber() { return this.toNumber }
    set ToNumber(value: number) { this.toNumber = value; }

    updateAll: boolean;
    get UpdateAll() { return this.updateAll }
    set UpdateAll(value: boolean) {
        this.updateAll = value;
        if (value) {
            this.UpdateSelected = false;
            this.UpdateItemsWithNoValue = false;
            this.UIProperties.SetEnabled("FromNumber", null, false);
            this.UIProperties.SetEnabled("ToNumber", null, false);
            this.IsAddButtonEnabled = false;
        }
    }
    updateSelected: boolean;
    get UpdateSelected() { return this.updateSelected }
    set UpdateSelected(value: boolean) {
        this.updateSelected = value;
        if (value) {
            this.UpdateAll = false;
            this.UpdateItemsWithNoValue = false;
            this.UIProperties.SetEnabled("FromNumber", null, true);
            this.UIProperties.SetEnabled("ToNumber", null, true);
            this.IsAddButtonEnabled = true;
        }
    }

    updateItemsWithNoValue: boolean;
    get UpdateItemsWithNoValue() { return this.updateItemsWithNoValue }
    set UpdateItemsWithNoValue(value: boolean) {
        this.updateItemsWithNoValue = value;
        if (value) {
            this.UpdateAll = false;
            this.UpdateSelected = false
            this.UIProperties.SetEnabled("FromNumber", null, false);
            this.UIProperties.SetEnabled("ToNumber", null, false);
            this.IsAddButtonEnabled = false;
        }

    }
    isAddButtonEnabled: boolean;
    get IsAddButtonEnabled() { return this.isAddButtonEnabled; }
    set IsAddButtonEnabled(value: boolean) {
        this.isAddButtonEnabled = value;
    }

    UpdateAllRadio(newValue: boolean) {
        this.UpdateAll = newValue;
    }
    UpdateItemsWithNoValueRadio(newValue: boolean) {
        this.UpdateItemsWithNoValue = newValue;
    }
    UpdateSelectedRadio(newValue: boolean) {
        this.UpdateSelected = newValue;

    }

    CancelButtonClicked() {
        this.ItemsSource = null;
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    IsAddMessageVisible: boolean = false;
    AddMessage: string = null;

    AddLines() {
        this.IsAddMessageVisible = false;
        this.AddMessage = null;
        if (this.ToNumber == null) {

            if (this.ItemsSource.Collection.length < this.CertificateOfOriginPM.CertificateOriginItemItems.length) {
                this.ItemsSource.Insert(new SelectedItem(null, this));
            }
            else {
                this.IsAddMessageVisible = true;
                this.AddMessage = "קיימים רק " + this.ItemsSource.Collection.length + " פריטים";

            }
        }
        else if (this.FromNumber != null && this.ToNumber >= this.FromNumber && this.ToNumber > 0 && this.FromNumber >= 0) {
            var toNumber = this.CertificateOfOriginPM.CertificateOriginItemItems.filter(d => d.ItemSerial == this.ToNumber)[0];

            if (!toNumber) {
                this.IsAddMessageVisible = true;
                this.AddMessage = "עד מספר גדול ממספר הפריטים";
            }
            else {
                for (var i = this.FromNumber; i <= this.ToNumber; i++) {
                    var number: number = i;
                    var existed: SelectedItem = this.ItemsSource.Collection.filter(d => d.Number == number)[0];
                    if (existed == null && number != 0) {
                        var line: SelectedItem = new SelectedItem(number, this);
                        this.ItemsSource.Insert(line);
                    }
                }
                this.FromNumber = null;
                this.ToNumber = null;

            }
        }

    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors = [];
        if (this.LookUpTableName && !this.FinalValue) {
            errors.push("בחר ערך מתוך הרשימה");
        }
        if (!this.UpdateAll && !this.UpdateSelected && !this.UpdateItemsWithNoValue) {
            errors.push("בחר פריטים לעדכון");
        }
        if (this.CertificateOfOriginPM == null) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ProcessTypeRequired"));
        }
        if (this.UpdateSelected && this.ItemsSource.Length == 0) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.SelectItems"));
        }

        for (let item of this.ItemsSource.Collection) {
            if (item.Number == null) {
                errors.push("חובה לבחור ערך בשדה מספר");

            }
            else {
                var exist = this.CertificateOfOriginPM.CertificateOriginItemItems.filter(d => d.ItemSerial == item.Number)[0];
                if (!exist) {
                    errors.push("מספר " + item.Number + " אינו קיים בפריטים");
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }


}


export class SelectedItem extends BaseComponent {
    parent: UpdateCertificateOfOriginGeneralFieldComponent;
    DataContext: any = this;
    constructor(number: number, Parent: UpdateCertificateOfOriginGeneralFieldComponent) {
        super();
        this.Number = number;
        this.parent = Parent;
    }

    number: number;
    get Number() { return this.number }
    set Number(value: number) { this.number = value; }


    DeleteButtonClicked() {
        if (this.parent.ItemsSource.Collection.includes(this)) {
            this.parent.ItemsSource.Remove(this);
        }
    }
}
