import { Component } from '@angular/core';
import { GeneralDomainService } from '../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

declare var window: any;

@Component({
    templateUrl: './DigitalPortalCustomizationChageLabelsComponent.html',
})

export class DigitalPortalCustomizationChageLabelsComponent extends BaseComponent {
    private myService: GeneralDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    public LabelsItemsSource: ObservableCollection;

    constructor(private _entityListService: EntityListService) {
        super();
        this.myService = new GeneralDomainService();
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
        }
    }

    private FillObjectTablesFiltersList() {
        this.ObjectTablesFilterList = [];
        var objectTbaleName = "Shipment";
        var objectTableId = window.ObjectTables.filter(d => d.Name === objectTbaleName)[0];
        this.ObjectTablesFilterList.push(new CodeNameClass("Shipments", objectTableId));

        objectTbaleName = "ARInvoice";
        objectTableId = window.ObjectTables.filter(d => d.Name === objectTbaleName)[0];
        this.ObjectTablesFilterList.push(new CodeNameClass("Invoices", objectTableId));

        objectTbaleName = "Quote";
        objectTableId = window.ObjectTables.filter(d => d.Name === objectTbaleName)[0];
        this.ObjectTablesFilterList.push(new CodeNameClass("Quotes", objectTableId));

        objectTbaleName = "General";
        objectTableId = window.ObjectTables.filter(d => d.Name === objectTbaleName)[0];
        this.ObjectTablesFilterList.push(new CodeNameClass("General", null));
        this.selectedObjectTableItem = this.ObjectTablesFilterList[0];
    }

    BuildItemsSource(searchText: string = null) {
        var labelsList: CustomizationLabelItem[] = [];

        this.LabelsItemsSource.InsertCollection(labelsList);
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
