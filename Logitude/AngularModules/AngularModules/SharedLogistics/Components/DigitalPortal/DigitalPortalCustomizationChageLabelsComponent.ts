import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService, DigitalTextCodeUpdateModel, DigitalTextCodeObject } from '../../../Infrastructure/Services/WebServices/DigitalTextService'
import { AppTool } from '../../../Infrastructure/Tools'; 

@Component({
    templateUrl: './DigitalPortalCustomizationChageLabelsComponent.html',
})

export class DigitalPortalCustomizationChageLabelsComponent extends BaseComponent implements OnInit {
    private digitalTextService: DigitalTextService;
    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    public LabelsItemsSource: ObservableCollection;
    public ModifiedLables: DigitalTextCodeUpdateModel;
    public IsModifiedLables = false;
    public IsChange: boolean = false;

    constructor() {
        super();
        this.digitalTextService = new DigitalTextService();
        this.LabelsItemsSource = new ObservableCollection([]);
        this.ModifiedLables = new DigitalTextCodeUpdateModel();
        this.ModifiedLables.Lables = [];
        this.IsModifiedLables = false;
    }

    SetWindowArgs(args: any) {
        this.FillDigitalProfileFiltersList();
    }

    ngOnInit() {
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "ReloadDigitalPortalLabels") {
                this.IsChange = true;
            }
        });
    }

    public DigitalProfileFilterList: CodeNameClass[];
    private selectedProfileItem: CodeNameClass;
    get SelectedProfileItem() { return this.selectedProfileItem; }
    set SelectedProfileItem(value: CodeNameClass) {
        if (this.selectedProfileItem != value) {
            this.selectedProfileItem = value;
            this.BuildItemsSource();
        }
    }

    private FillDigitalProfileFiltersList() {
        this.DigitalProfileFilterList = [];
        this.digitalTextService.GetDigitalProfileName(SessionLocator.Tenant).subscribe((myResult) => {
            if (!myResult.HasError) {
                var objectTables = myResult.Result;
                objectTables.forEach(item => {
                    this.DigitalProfileFilterList.push(new CodeNameClass(item.Id, item.Name, item.Code));
                });

                this.selectedProfileItem = this.DigitalProfileFilterList[0];
                this.FillObjectTablesFiltersList();
            }
        });
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


    BuildItemsSource() {
        this.LabelsItemsSource = new ObservableCollection([]);
        var labelsList = [];
        var objectTableId = this.SelectedObjectTableItem.Name;
        var profileCode = this.SelectedProfileItem.LocalName;
        this.digitalTextService.GetTextCodesByFilters(null, objectTableId, profileCode).subscribe((myResult) => {
            if (!myResult.HasError) {
                var data = myResult.Result;
                this.loadedResults = data.filter(a => !AppTool.IsNullOrEmpty(a['FieldCode']));
                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    data = data.filter(f =>
                        (!AppTool.IsNullOrEmpty(f.FieldCode) && f.FieldCode.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1) ||
                        (!AppTool.IsNullOrEmpty(f.DisplayText) && f.DisplayText.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1 )||
                        (!AppTool.IsNullOrEmpty(f.DefaultText) && f.DefaultText.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1));
                }
                data.filter(a => !AppTool.IsNullOrEmpty(a['FieldCode'])).forEach(item => {
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
            this.BuildSearchItems();
        }
    }

    loadedResults: CustomizationLabelItem[] = [];
    BuildSearchItems() {
        var labelsList = [];
        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            var data = this.loadedResults;
            data = data.filter(f =>
                (!AppTool.IsNullOrEmpty(f.FieldCode) && f.FieldCode.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1) ||
                (!AppTool.IsNullOrEmpty(f.DisplayText) && f.DisplayText.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1) ||
                (!AppTool.IsNullOrEmpty(f.DefaultText) && f.DefaultText.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1));

            data.forEach(item => {
                labelsList.push(new CustomizationLabelItem(this, item));
            });
            this.LabelsItemsSource = new ObservableCollection([]);
            this.LabelsItemsSource.InsertCollection(labelsList);
        }
        else {
            this.LabelsItemsSource = new ObservableCollection([]);
            this.loadedResults.forEach(item => {
                labelsList.push(new CustomizationLabelItem(this, item));
            });
            this.LabelsItemsSource.InsertCollection(labelsList);
        }
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    Save() {
        if (this.IsModifiedLables) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.ModifiedLables.ObjectTableId = this.SelectedObjectTableItem.Name;
            this.ModifiedLables.ProfileId = this.SelectedProfileItem.Code;
            this.digitalTextService.UpdateDigitalTextCodes(this.ModifiedLables).subscribe((myResult) => {
                this.customizationEditComponent.IsDirty = false;
                this.ModifiedLables = new DigitalTextCodeUpdateModel();
                this.CurrentSession.StopBusyIndicator();
                this.BuildItemsSource();
                if (this.customizationEditComponent.NewSelectedMenu) {
                    this.customizationEditComponent.SelectedMenu = this.customizationEditComponent.NewSelectedMenu;
                }
            });
        }
    }

    Cancel() {
        this.customizationEditComponent.IsDirty = false;
        if (this.customizationEditComponent.NewSelectedMenu) {
            this.customizationEditComponent.SelectedMenu = this.customizationEditComponent.NewSelectedMenu;
        }
    }
}

export class CustomizationLabelItem extends BaseComponent {

    public DataContext: CustomizationLabelItem = this;

    constructor(public father: DigitalPortalCustomizationChageLabelsComponent, item) {
        super();
        this.textCode = item.TextCode;
        this.defaultText = item.DefaultText;
        this.displayText = item.DisplayText;
        this.fieldCode = item.FieldCode;
    }

    private defaultText = "";
    get DefaultText() { return this.defaultText; }
    set DefaultText(value) {
        if (value != this.defaultText) {
            this.defaultText = value;
        }
    }

    private displayText = "";
    get DisplayText() { return this.displayText; }
    set DisplayText(value) {
        if (value != this.displayText) {
            this.displayText = value;
        }
    }
    
    private fieldCode = "";
    get FieldCode() { return this.fieldCode; }
    set FieldCode(value) 
    {
        if (value != this.fieldCode) 
        {
            this.fieldCode = value;
        }
    }

    public UpdateModifiedLables(newValue) {

        if (this.father.ModifiedLables.Lables == null) 
        {
            this.father.ModifiedLables.Lables = [];
        }

        this.father.IsModifiedLables = true;
        this.father.customizationEditComponent.IsDirty = true;
        var label = this.father.ModifiedLables?.Lables?.filter(d => d.TextCode == this.textCode)[0];
        var index = this.father.ModifiedLables?.Lables?.indexOf(label);
        if (index != null && index != -1) {
            this.father.ModifiedLables.Lables.splice(index, 1);
        }

        var newLabel = new DigitalTextCodeObject();
        newLabel.TextCode = this.textCode;
        newLabel.FieldCode = this.fieldCode;
        newLabel.DefaultText = newValue;
        newLabel.DisplayText = this.displayText;
        this.father.ModifiedLables.Lables.push(newLabel);
    }

    private textCode: string = "";
    get TextCode() {
        return this.textCode;
    }

    set TextCode(value) {
        if (value != this.textCode) {
            this.textCode = value;
        }
    }
}
