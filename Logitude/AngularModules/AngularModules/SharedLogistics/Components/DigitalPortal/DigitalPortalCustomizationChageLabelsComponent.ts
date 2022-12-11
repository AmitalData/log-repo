import { Component } from '@angular/core';
import { GeneralDomainService } from '../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService } from '../../../Infrastructure/Services/WebServices/DigitalTextService'
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
declare var window: any;

@Component({
    templateUrl: './DigitalPortalCustomizationChageLabelsComponent.html',
})

export class DigitalPortalCustomizationChageLabelsComponent extends BaseComponent {
    private digitalTextService: DigitalTextService;
    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    public LabelsItemsSource: ObservableCollection;

    constructor(private _entityListService: EntityListService) {
        super();
        this.digitalTextService = new DigitalTextService();
        this.LabelsItemsSource = new ObservableCollection([]);
    }

    SetWindowArgs(args: any) {
        this.FillObjectTablesFiltersList();
        this.BuildItemsSource();
    }

    public ObjectTablesFilterList: CodeNameClass[];
    private selectedObjectTableItem: CodeNameClass;
    get SelectedObjectTableItem() { return this.selectedObjectTableItem; }
    set SelectedObjectTableItem(value: CodeNameClass) {
        if (this.selectedObjectTableItem != value) {
            this.selectedObjectTableItem = value;
            this.BuildItemsSource();
        }
    }

    private FillObjectTablesFiltersList() {
        this.ObjectTablesFilterList = [];
        var objectTbaleName = "Shipment";
        var objectTable = window.ObjectTables.filter(d => d.Name === objectTbaleName)[0];
        this.ObjectTablesFilterList.push(new CodeNameClass("Shipments", objectTable.Id));

        objectTbaleName = "ARInvoice";
        objectTable = window.ObjectTables.filter(d => d.Name === objectTbaleName)[0];
        this.ObjectTablesFilterList.push(new CodeNameClass("Invoices", objectTable.Id));

        objectTbaleName = "Quote";
        objectTable = window.ObjectTables.filter(d => d.Name === objectTbaleName)[0];
        this.ObjectTablesFilterList.push(new CodeNameClass("Quotes", objectTable.Id));

        this.ObjectTablesFilterList.push(new CodeNameClass("General", null));
        this.selectedObjectTableItem = this.ObjectTablesFilterList[0];
    }

    BuildItemsSource(searchText: string = null) {
        var labelsList: CustomizationLabelItem[] = [];
        var objectTableId = this.SelectedObjectTableItem.Name;
        this.digitalTextService.GetTextCodesByFilters(null, objectTableId).subscribe((myResult) => {
            if (!myResult.HasError) {
                labelsList = myResult.Result;
                this.LabelsItemsSource.InsertCollection(labelsList);
            }
        });
    }

    private searchText: string = null;
    public get SearchText() { return this.searchText; }
    public set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
    }

    SearchTextChanged(text: string) {
        this.SearchText = text;
        this.BuildItemsSource(text);
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    Save() {
        if (!this.customizationEditComponent.IsDirty && this.customizationEditComponent.IsSaveAndClose) {
            this.customizationEditComponent.CurrentSession.CloseCurrentWindow();
            this.customizationEditComponent.IsSaveAndClose = false;
        }
    }
}

export class CustomizationLabelItem {
    constructor() {
        
    }

    private defaultText = "";
    get DefaultText() { return this.defaultText; }

    private displayLabel = "";
    get DisplayLabel() { return this.displayLabel; }
    set DisplayLabel(value) {
        if (value != this.displayLabel)
            this.displayLabel = value;
    }

    private code: string = "";
    get Code() {
        return this.code;
    }
}
