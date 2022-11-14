declare var System: any;
declare var window: any;
import { Component, OnInit, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenLayoutComponent } from '../ScreenLayoutComponent';
import { ObjectFieldPM } from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    selector: 'GridScreenComponent',
    templateUrl: './GridScreenComponent.html',
})

export class GridScreenComponent extends BaseComponent implements OnInit, AfterViewInit
{
    public ScreenLayoutComponent: ScreenLayoutComponent;
    public screenWidth: string;
    private screenLayoutwidth = 650;
    private CurrentSession = SessionLocator.SelectedSession;
    public SortedTypes: string[] = [];
    public SelectedSortedByField: ObjectFieldPM;
    public SearchFieldsId: string;
    public AllFields: any[] = [];
    @Output() onSelectedDataLoadedEvent = new EventEmitter();
    @Output() onUnSelectedDataLoadedEvent = new EventEmitter();
    @Output() onDataSourceChangedEvent = new EventEmitter();
    @Output() onUnselectedDataSourceChangedEvent = new EventEmitter();

    constructor()
    {
        super();
        this.Initialize();
    }

    ngAfterViewInit(): void {
        this.onUnselectedDataSourceChangedEvent.emit(this.UnSelectedFields);
        this.onDataSourceChangedEvent.emit(this.ScreenLayoutComponent.GridScreenSelectedFields);
    }

    private Initialize() {
        this.screenWidth = (window.innerWidth - this.screenLayoutwidth) + "px";
        this.FillSortedTypes();
        this.SearchFieldsId = this.GetSearchFieldsId();
    }

    private FillSortedTypes() {
        this.SortedTypes.push("Ascending");
        this.SortedTypes.push("Descending");
    }

    private GetSearchFieldsId() {
        if (this.CurrentSession == null) {
            return "GridSearchFields_-1_-1";
        }

        return "GridSearchFields_" + this.CurrentSession.GetNewId("GridSearchFields");
    }

    ngOnInit(): void
    {
       
    }

    get SelectedScreen(){
        return this.ScreenLayoutComponent.SelectedItem.ScreenPM;
    }

    get SelectedFields() {
        return this.ScreenLayoutComponent.GridScreenSelectedFields;
    }

    get IsFirstSelectedItem() {
        return this.SelectedItem && this.SelectedItem.IndexOrder == 0;
    }

    get IsLastSelectedItem() {
        return this.SelectedItem && this.SelectedItem.IndexOrder == this.ScreenLayoutComponent.GridScreenSelectedFields.length - 1;
    }

    UnSelectedFields: ObjectFieldPM[];
    FixedUnSelectedFields: ObjectFieldPM[];

    SortedTypeChanged(selectedSortType) {
        this.SelectedScreen.SortedType = selectedSortType;
        this.ScreenLayoutComponent.Modified = true;
    }

    SortedByField(selectedSortByObjectField) {
        this.SelectedSortedByField = selectedSortByObjectField;
        this.SelectedScreen.SortedByFieldCode = selectedSortByObjectField?.FieldCode;
        this.ScreenLayoutComponent.Modified = true;
    }

    Run(screenLayoutComponent: ScreenLayoutComponent)
    {
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.ScreenLayoutComponent = screenLayoutComponent;
        this.UnSelectedFields = this.ScreenLayoutComponent.banckStackFields;
        this.FixedUnSelectedFields = this.UnSelectedFields;
        this.FillSelectedFields();
        this.SetSelectedSortedByField();
        this.CurrentSession.StopBusyIndicator();
    }

    FillSelectedFields() {
        this.ScreenLayoutComponent.GridScreenSelectedFields = [];
        this.ScreenLayoutComponent.currentScreenFields = this.ScreenLayoutComponent.currentScreenFields.sort((a, b) => { return (a.Column === b.Column) ? 0 : (a.Column < b.Column) ? -1 : 1 });
        this.ScreenLayoutComponent.currentScreenFields.forEach((screenField, index) => {
            this.AddScreenField(screenField, index);
        });
        this.AllFields = this.SelectedFields.concat(this.ScreenLayoutComponent.AllbanckStackFields);
    }

    private AddScreenField(screenField: any, index: number) {
        let screenObjectField = window.ObjectFields.filter(objectField => objectField.FieldCode == screenField.ObjectFieldCode)[0];
        if (!screenObjectField) return;
        screenObjectField.IndexOrder = index;
        this.ScreenLayoutComponent.GridScreenSelectedFields.push(screenObjectField);
    }

    SetSelectedSortedByField() {
        let selectedSortedByField = this.AllFields.filter(field => field.FieldCode == this.SelectedScreen.SortedByFieldCode);
        if (!selectedSortedByField) return;
        if (!selectedSortedByField[0]) return;
        this.SelectedSortedByField = selectedSortedByField[0];
    }

    private isbtnAddDisabled: boolean = true;
    public get IsbtnAddDisabled() { return this.isbtnAddDisabled; }
    public set IsbtnAddDisabled(newValue: boolean) {
        this.isbtnAddDisabled = newValue;
    }

    private isbtnRemoveDisabled: boolean = true;
    public get IsbtnRemoveDisabled() { return this.isbtnRemoveDisabled; }
    public set IsbtnRemoveDisabled(newValue: boolean) {
        this.isbtnRemoveDisabled = newValue;
    }

    private isbtnDownDisabled: boolean = true;
    public get IsbtnDownDisabled() { return this.isbtnDownDisabled; }
    public set IsbtnDownDisabled(newValue: boolean) {
        this.isbtnDownDisabled = newValue;
    }

    private isbtnUpDisabled: boolean = true;
    public get IsbtnUpDisabled() { return this.isbtnUpDisabled; }
    public set IsbtnUpDisabled(newValue: boolean) {
        this.isbtnUpDisabled = newValue;
    }

    onSelectedItemChanged(item) {
        this.SelectedItem = item;
        this.IsbtnAddDisabled = true;
        this.IsbtnRemoveDisabled = false;
        this.IsbtnDownDisabled = false;
        this.IsbtnUpDisabled = false;
        if (this.UnSelectedFields.length != 0) {
            this.onUnSelectedDataLoadedEvent.emit(null);
        }
    }

    onNeedToSelectedItemChanged(item) {
        this.NeedToSelectedItem = item;
        this.IsbtnAddDisabled = false;
        this.IsbtnRemoveDisabled = true;
        this.IsbtnDownDisabled = true;
        this.IsbtnUpDisabled = true;
        if (this.SelectedFields.length != 0) {
            this.onSelectedDataLoadedEvent.emit(null);
        }
    }

    DisableAllButtons() {
        this.IsbtnAddDisabled = true;
        this.IsbtnRemoveDisabled = true;
        this.IsbtnDownDisabled = true;
        this.IsbtnUpDisabled = true;
    }

    private searchText: string; 
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        let searchedUnSelectedFields;
        if (newValue != null && newValue != "") {
            searchedUnSelectedFields = this.GetSearchedUnSelectedFields(newValue);
        }
        else {
            searchedUnSelectedFields = this.FixedUnSelectedFields;
        }
        this.onUnselectedDataSourceChangedEvent.emit(searchedUnSelectedFields);
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    }

    FillPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = TextCodeTranslator.Translate("General.O.Search");
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    }

    private selectedItem: ObjectFieldPM;
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(newValue: ObjectFieldPM) {
        this.selectedItem = newValue;
    }

    private needToSelectedItem: ObjectFieldPM;
    public get NeedToSelectedItem() { return this.needToSelectedItem; }
    public set NeedToSelectedItem(newValue: ObjectFieldPM) {
        this.needToSelectedItem = newValue;
    }

    IsAddBtnClicked: boolean;
    btnAdd_Click() {
        if (!this.NeedToSelectedItem) return;
        this.NeedToSelectedItem.IndexOrder = this.ScreenLayoutComponent.GridScreenSelectedFields.length;
        let selectedFieldIndex = this.UnSelectedFields.indexOf(this.NeedToSelectedItem);
        this.UnSelectedFields.splice(selectedFieldIndex, 0);
        this.UnSelectedFields = this.UnSelectedFields.filter(d => d.FieldCode != this.NeedToSelectedItem.FieldCode);
        this.FixedUnSelectedFields = this.FixedUnSelectedFields.filter(d => d.FieldCode != this.NeedToSelectedItem.FieldCode);
        this.ScreenLayoutComponent.GridScreenSelectedFields.push(this.NeedToSelectedItem);
        if (this.SearchText != null && this.SearchText != "") {
            this.UnSelectedFields = this.GetSearchedUnSelectedFields(this.SearchText);
        }
        if (this.UnSelectedFields.length != 0)
            this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        this.onUnselectedDataSourceChangedEvent.emit(this.UnSelectedFields);
        this.onSelectedItemChanged(this.NeedToSelectedItem);
        this.onSelectedDataLoadedEvent.emit(this.NeedToSelectedItem);
        this.DisableAllButtons();
        this.ScreenLayoutComponent.ReloadGridSections = true;
        this.ScreenLayoutComponent.Modified = true;
        this.IsAddBtnClicked = true;
    }

    private GetSearchedUnSelectedFields(searchValue): ObjectFieldPM[] {
        return this.FixedUnSelectedFields.filter(f => TextCodeTranslator.Translate(f.FullNameTextCodeCode).toLowerCase().indexOf(searchValue.toLowerCase()) > -1);
    }

    btnRemove_Click() {
        if (!this.SelectedItem) return;
        let selectedFieldIndex = this.ScreenLayoutComponent.GridScreenSelectedFields.indexOf(this.SelectedItem);
        this.ScreenLayoutComponent.GridScreenSelectedFields.splice(selectedFieldIndex, 0);
        this.ScreenLayoutComponent.GridScreenSelectedFields = this.ScreenLayoutComponent.GridScreenSelectedFields.filter(d => d.FieldCode != this.SelectedItem.FieldCode);
        this.UnSelectedFields.push(this.SelectedItem);
        if (this.IsAddBtnClicked) this.FixedUnSelectedFields.push(this.SelectedItem);
        this.ReorderAllSelectedFields();
        if (this.SelectedFields.length != 0)
            this.onSelectedDataLoadedEvent.emit(this.NeedToSelectedItem);
        this.onDataSourceChangedEvent.emit(this.ScreenLayoutComponent.GridScreenSelectedFields);
        this.onNeedToSelectedItemChanged(this.SelectedItem);
        this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        this.DisableAllButtons();
        this.ScreenLayoutComponent.ReloadGridSections = true;
        this.ScreenLayoutComponent.Modified = true;
    }

    private ReorderAllSelectedFields() {
        this.ScreenLayoutComponent.GridScreenSelectedFields.forEach((objectField, index) => {
            objectField.IndexOrder = index;
        });
    }

    btnUp_Click() {
        if (this.SelectedItem == null) return;
        let selectedFieldIndex = this.ScreenLayoutComponent.GridScreenSelectedFields.indexOf(this.SelectedItem);
        if (selectedFieldIndex <= 0) return;
        this.ScreenLayoutComponent.GridScreenSelectedFields = this.ScreenLayoutComponent.GridScreenSelectedFields.filter(d => d.FieldCode != this.SelectedItem.FieldCode);
        let downField = this.ScreenLayoutComponent.GridScreenSelectedFields.filter(o => o.IndexOrder == selectedFieldIndex - 1)[0];
        if (!downField) return;
        this.ScreenLayoutComponent.GridScreenSelectedFields.filter(o => o.IndexOrder == selectedFieldIndex - 1)[0].IndexOrder = selectedFieldIndex;
        this.SelectedItem.IndexOrder = selectedFieldIndex - 1;
        this.ScreenLayoutComponent.GridScreenSelectedFields.splice(selectedFieldIndex - 1, 0, this.SelectedItem);
        this.onDataSourceChangedEvent.emit(this.ScreenLayoutComponent.GridScreenSelectedFields);
        this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
        this.ScreenLayoutComponent.ReloadGridSections = true;
        this.ScreenLayoutComponent.Modified = true;
    }

    btnDown_Click() {
        if (this.SelectedItem == null) return;
        let selectedFieldIndex = this.ScreenLayoutComponent.GridScreenSelectedFields.indexOf(this.SelectedItem);
        if (selectedFieldIndex >= this.ScreenLayoutComponent.GridScreenSelectedFields.length - 1) return;
        this.ScreenLayoutComponent.GridScreenSelectedFields = this.ScreenLayoutComponent.GridScreenSelectedFields.filter(d => d.FieldCode != this.SelectedItem.FieldCode);
        let upField = this.ScreenLayoutComponent.GridScreenSelectedFields.filter(o => o.IndexOrder == selectedFieldIndex + 1)[0];
        if (!upField) return;
        this.ScreenLayoutComponent.GridScreenSelectedFields.filter(o => o.IndexOrder == selectedFieldIndex + 1)[0].IndexOrder = selectedFieldIndex;
        this.SelectedItem.IndexOrder = selectedFieldIndex + 1;
        this.ScreenLayoutComponent.GridScreenSelectedFields.splice(selectedFieldIndex + 1, 0, this.SelectedItem);
        this.onDataSourceChangedEvent.emit(this.ScreenLayoutComponent.GridScreenSelectedFields);
        this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
        this.ScreenLayoutComponent.ReloadGridSections = true;
        this.ScreenLayoutComponent.Modified = true;
    }
}
