import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
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
    public ObjectTableId: string;
    public ProfileCode: string;
    public ProfileId: string;

    constructor() {
        super();
        this.digitalTextService = new DigitalTextService();
        this.LabelsItemsSource = new ObservableCollection([]);
        this.ModifiedLables = new DigitalTextCodeUpdateModel();
        this.ModifiedLables.Lables = [];
        this.IsModifiedLables = false;
    }

    ngOnInit() {
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "ReloadDigitalPortalLabels") {
                this.IsChange = true;
            }
        });
    }

    public BuildItemsSource() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.LabelsItemsSource = new ObservableCollection([]);
        var labelsList = [];
        var _selectedDisplayLangCode = this.customizationEditComponent && this.customizationEditComponent.SelectedMenu ? this.customizationEditComponent.SelectedMenu.LanguageCode : 'EN'
        this.digitalTextService.GetTextCodesByFilters(null, this.ObjectTableId, this.ProfileCode, _selectedDisplayLangCode).subscribe((myResult) => {
            if (!myResult.HasError) {
                var data = myResult.Result;
                this.loadedResults = data;
                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    data = data.filter(f =>
                        (!AppTool.IsNullOrEmpty(f.TextCode) && f.TextCode.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1) ||
                        (!AppTool.IsNullOrEmpty(f.FieldCode) && f.FieldCode.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1) ||
                        (!AppTool.IsNullOrEmpty(f.DisplayText) && f.DisplayText.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1 )||
                        (!AppTool.IsNullOrEmpty(f.DefaultText) && f.DefaultText.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1));
                }
                data.forEach(item => {
                    labelsList.push(new CustomizationLabelItem(this, item));
                });
                this.LabelsItemsSource.InsertCollection(labelsList);
                this.CurrentSession.StopBusyIndicator();
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
                (!AppTool.IsNullOrEmpty(f.TextCode) && f.TextCode.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1) ||
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
            this.ModifiedLables.ObjectTableId = this.ObjectTableId;
            this.ModifiedLables.ProfileId = this.ProfileId;
            this.ModifiedLables.ProfileCode = this.ProfileCode;
            var _selectedDisplayLangCode = this.customizationEditComponent && this.customizationEditComponent.SelectedMenu ? this.customizationEditComponent.SelectedMenu.LanguageCode : 'EN'
            this.ModifiedLables.LanguageCode = _selectedDisplayLangCode;
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
        newLabel.DefaultText = this.defaultText; 
        newLabel.DisplayText = newValue;
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
