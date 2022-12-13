import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService, DigitalTextCodeUpdateModel, DigitalTextCodeObject } from '../../../Infrastructure/Services/WebServices/DigitalTextService'

@Component({
    templateUrl: './DigitalPortalCustomizationChageLabelsComponent.html',
})

export class DigitalPortalCustomizationChageLabelsComponent extends BaseComponent {
    private digitalTextService: DigitalTextService;
    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    public LabelsItemsSource: ObservableCollection;
    public ModifiedLables: DigitalTextCodeUpdateModel;
    public IsModifiedLables = false;

    constructor() {
        super();
        this.digitalTextService = new DigitalTextService();
        this.LabelsItemsSource = new ObservableCollection([]);
        this.ModifiedLables = new DigitalTextCodeUpdateModel();
        this.ModifiedLables.Lables = [];
        this.IsModifiedLables = false;
    }

    SetWindowArgs(args: any) {
        this.FillObjectTablesFiltersList();
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
        this.digitalTextService.GetDigitalTextCodesObjetTables().subscribe((myResult) => {
            if (!myResult.HasError) {
                var objectTables = myResult.Result;
                objectTables.forEach(item => {
                    this.ObjectTablesFilterList.push(new CodeNameClass(item.ObjectTableName, item.ObjectTableId));
                });

                this.selectedObjectTableItem = this.ObjectTablesFilterList[0];
                this.BuildItemsSource();
            }
        });
    }

    BuildItemsSource(searchText: string = null) {
        var labelsList: CustomizationLabelItem[] = [];
        var objectTableId = this.SelectedObjectTableItem.Name;
        this.digitalTextService.GetTextCodesByFilters(null, objectTableId).subscribe((myResult) => {
            if (!myResult.HasError) {
                myResult.Result.forEach(item => {
                    labelsList.push(new CustomizationLabelItem(this, item));
                });
                this.LabelsItemsSource.InsertCollection(labelsList);
            }
        });
    }

    private searchText: string = null;
    public get SearchText() { return this.searchText; }
    public set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
            this.BuildItemsSource(value);
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
        if (this.IsModifiedLables) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.ModifiedLables.ObjectTableId = this.SelectedObjectTableItem.Name;
            this.digitalTextService.UpdateDigitalTextCodes(this.ModifiedLables).subscribe((myResult) => {
                //this.customizationEditComponent.CurrentSession.CloseCurrentWindow();
                this.customizationEditComponent.IsDirty = false;
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    Cancel() {

    }
}

export class CustomizationLabelItem extends BaseComponent {

    public DataContext: CustomizationLabelItem = this;

    constructor(public father: DigitalPortalCustomizationChageLabelsComponent, item) {
        super();
        this.code = item.Code;
        this.displayText = item.DisplayText;
        this.displayLable = item.DisplayLable;
    }

    private displayText = "";
    get DisplayText() { return this.displayText; }
    set DisplayText(value) {
        if (value != this.displayText) {
            this.displayText = value;
        }
    }

    private displayLable = "";
    get DisplayLable() { return this.displayLable; }
    set DisplayLable(value) {
        if (value != this.displayLable) {
            this.displayLable = value;
        }
    }

    public UpdateModifiedLables(newValue) {
        this.father.IsModifiedLables = true;
        this.father.customizationEditComponent.IsDirty = true;
        var newLabel = new DigitalTextCodeObject();
        newLabel.Code = this.Code;
        newLabel.DisplayText = newValue;
        this.father.ModifiedLables.Lables.push(newLabel);
    }

    private code: string = "";
    get Code() {
        return this.code;
    }
    set Code(value) {
        if (value != this.code) {
            this.code = value;
        }
    }
}
