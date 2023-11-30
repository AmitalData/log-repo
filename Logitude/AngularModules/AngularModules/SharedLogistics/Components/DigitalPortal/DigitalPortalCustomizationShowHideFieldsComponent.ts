import { Component, EventEmitter, Output, OnInit} from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService, DigitalFeildSecurityObjectModel, DigitalFeildSecurityUpdateModel, DigitalTextCodeObject, DigitalTextCodeUpdateModel } from '../../../Infrastructure/Services/WebServices/DigitalTextService'
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    templateUrl: './DigitalPortalCustomizationShowHideFieldsComponent.html',
})

export class DigitalPortalCustomizationShowHideFieldsComponent extends BaseComponent implements OnInit {

    private digitalTextService: DigitalTextService;
    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    public FieldsItemsSource: ObservableCollection;
    public loadedFieldsResults: [];
    public ModifiedLables: DigitalFeildSecurityObjectModel;
    public IsModifiedLables = false
    public ModifiedFields: DigitalTextCodeUpdateModel;
    public IsModifiedFields = false; 
    public IsChange: boolean = false;
    public ObjectTableId: string;
    public ProfileCode: string;
    public ProfileId: string;
    public IsWindowMode = false;
    public IsDirty = false; 
    public ParentObjectTableId: string;
    public IsDataReady: boolean = false;

    @Output() LostFocus: EventEmitter<boolean> = new EventEmitter<boolean>();

    constructor() {
        super();
        this.digitalTextService = new DigitalTextService();
        this.FieldsItemsSource = new ObservableCollection([]);
        this.loadedFieldsResults = [];
        this.ModifiedLables = new DigitalFeildSecurityObjectModel();
        this.ModifiedLables.DefaultSettings = [];
        this.IsModifiedLables = false;
        this.ModifiedFields = new DigitalTextCodeUpdateModel();
        this.ModifiedFields.Lables = [];
        this.IsModifiedFields = false;
    }

    SetWindowArgs(args: any) {
        if (Object.keys(args).length > 0) {
            this.IsWindowMode = true;
            this.ObjectTableId = args.ObjectTableId;
            this.ProfileCode = args.ProfileCode;
            this.ProfileId = args.ProfileId;
            this.ParentObjectTableId = args.ParentObjectTableId;
            this.customizationEditComponent = args.customizationEditComponent;
            this.BuildItemsSource();
        }
    }

    ngOnInit() {
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "ReloadDigitalPortalPermissions") {
                this.IsChange = true;
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

    public BuildItemsSource() {
        this.BuildFields();
    }

    BuildFields() {
        this.IsDataReady = false; 
        this.CurrentSession.StartBusyIndicatorLoading();
        var _selectedDisplayLangCode = this.customizationEditComponent && this.customizationEditComponent.SelectedMenu ? this.customizationEditComponent.SelectedMenu.LanguageCode : 'EN'
        this.digitalTextService.GetTextCodesByFilters(null, this.ObjectTableId, this.ProfileCode, _selectedDisplayLangCode).subscribe((myResult) => {
            if (!myResult.HasError) {
                this.loadedFieldsResults = myResult.Result.filter(a => !AppTool.IsNullOrEmpty(a['FieldCode']));
                this.BuildFieldsPremissions();
            }
        });
    }

    BuildFieldsPremissions() {
        this.FieldsItemsSource = new ObservableCollection([]);
        var profilesList: ProfileFieldsItem[] = [];
        this.digitalTextService.GetFeildPermissionByFilters(null, this.ObjectTableId, this.ProfileCode).subscribe((myResult) => {
            if (!myResult.HasError) {
                myResult.Result.filter(a => !AppTool.IsNullOrEmpty(a.FieldCode)).forEach(item => {
                    profilesList.push(new ProfileFieldsItem(this, item));
                });

                this.loadedResults = profilesList;
                this.FieldsItemsSource.InsertCollection(profilesList);
                this.CurrentSession.StopBusyIndicator();
                this.IsDataReady = true;
            }
        });
    }

    loadedResults = [];
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
                labelsList.push(new ProfileFieldsItem(this, item));
            });

            this.FieldsItemsSource = new ObservableCollection([]);
           
            this.FieldsItemsSource.InsertCollection(labelsList);
        }
        else {
            this.FieldsItemsSource = new ObservableCollection([]);
            this.loadedResults.forEach(item => {
                labelsList.push(new ProfileFieldsItem(this, item));
            });
            this.FieldsItemsSource.InsertCollection(labelsList);
        }
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    Cancel() {
        this.IsDirty = false;

        if (this.customizationEditComponent == null) return;

        this.customizationEditComponent.IsDirty = false;
        if (this.customizationEditComponent.NewSelectedMenu) {
            this.customizationEditComponent.SelectedMenu = this.customizationEditComponent.NewSelectedMenu;
        }
    }

    Save() {
        this.StartBusyIndicator();
        if (this.IsModifiedLables) {
            this.ModifiedLables.ObjectTableId = this.ObjectTableId;
            this.ModifiedLables.ProfileId = this.ProfileId;
            this.ModifiedLables.ProfileCode = this.ProfileCode;
            this.ModifiedLables.ParentObjectTableId = this.ParentObjectTableId;
            var hasHasPermissionList = this.FieldsItemsSource.Collection;
            
            this.loadedResults = this.loadedResults.map(el => {
                var updateItem = hasHasPermissionList.find(f => f.FieldCode === el.FieldCode);
                if (updateItem) {
                    var newLabel = new DigitalFeildSecurityUpdateModel();
                    newLabel.FieldCode = updateItem.FieldCode;
                    newLabel.CreatedBy = updateItem.CreatedBy;
                    newLabel.CreatedOn = updateItem.CreatedOn;
                    newLabel.HasPermission = updateItem.HasPermission;
                    newLabel.ModifiedBy = updateItem.ModifiedBy;
                    newLabel.ModifiedOn = updateItem.ModifiedOn;
                    newLabel.IsList = updateItem.IsList;
                    newLabel.IsPm = updateItem.IsPm;
                    this.ModifiedLables.DefaultSettings.push(newLabel);
                    return newLabel;
                } else {
                    var newLabel2 = new DigitalFeildSecurityUpdateModel();
                    newLabel2.FieldCode = el.FieldCode;
                    newLabel2.CreatedBy = el.CreatedBy;
                    newLabel2.CreatedOn = el.CreatedOn;
                    newLabel2.HasPermission = el.HasPermission;
                    newLabel2.ModifiedBy = el.ModifiedBy;
                    newLabel2.ModifiedOn = el.ModifiedOn;
                    newLabel2.IsList = el.IsList;
                    newLabel2.IsPm = el.IsPm;
                    this.ModifiedLables.DefaultSettings.push(newLabel2);
                    return newLabel2;
                }
            });

            this.digitalTextService.UpdateFeildPermission(this.ModifiedLables).subscribe((myResult) => {
             
                this.IsDirty = false;
                this.ModifiedLables = new DigitalFeildSecurityObjectModel();
                this.ModifiedLables.DefaultSettings = [];
                this.StopBusyIndicator();
                this.IsModifiedLables = false;

                if (this.customizationEditComponent == null) return;
                this.customizationEditComponent.IsDirty = false;
                if (this.customizationEditComponent.NewSelectedMenu) {
                    this.customizationEditComponent.SelectedMenu = this.customizationEditComponent.NewSelectedMenu;
                }
            });

            this.UpdateModifiedFields();
        }
        else {
            this.UpdateModifiedFields();
        }
    }

    UpdateModifiedFields() {
        if (this.IsModifiedFields) {
            this.ModifiedFields.ObjectTableId = this.ObjectTableId;
            this.ModifiedFields.ProfileId = this.ProfileId;
            this.ModifiedFields.ProfileCode = this.ProfileCode;
            var _selectedDisplayLangCode = this.customizationEditComponent && this.customizationEditComponent.SelectedMenu ? this.customizationEditComponent.SelectedMenu.LanguageCode : 'EN'
            this.ModifiedFields.LanguageCode = _selectedDisplayLangCode;
            this.digitalTextService.UpdateDigitalTextCodes(this.ModifiedFields).subscribe((myResult) => {
                if (this.customizationEditComponent != null) this.customizationEditComponent.IsDirty = false;
                this.IsDirty = false;
                this.ModifiedFields = new DigitalTextCodeUpdateModel();
                this.ModifiedFields.Lables = [];
                this.StopBusyIndicator();
                this.IsModifiedFields = false;
                this.CurrentSession.SessionEvent.emit({ Name: "ReloadDigitalPortalLabels" });
                this.BuildItemsSource();
            });
        }
    }

    StartBusyIndicator() {
        if (this.IsModifiedLables || this.IsModifiedFields) {
            this.CurrentSession.StartBusyIndicatorLoading();
        }
    }

    StopBusyIndicator() {
        if (this.IsModifiedLables || this.IsModifiedFields) {
            this.CurrentSession.StopBusyIndicator();
        }
    }

    OnLostFocus() {
        this.LostFocus.emit(true);
    }

    AddFieldClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 800;
        var windowArgs: any = {};
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ParentObjectTableId = this.ParentObjectTableId;
        windowArgs.ProfileCode = this.ProfileCode;
        windowArgs.ProfileId = this.ProfileId;
        logWindow.Title = "Add a field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./SharedLogistics/Components/DigitalPortal/AddDigitalLogitudeFieldComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {
                this.BuildItemsSource();
            }
        });
    }
}

export class ProfileFieldsItem extends BaseComponent {

    public DataContext: ProfileFieldsItem = this;
    public Background = "transparent";
    public DefaultTextBackground = "rgba(230, 231, 232, 0.5)";
    constructor(public father: DigitalPortalCustomizationShowHideFieldsComponent, item) {
        super();
        this.isList = item.IsList;
        this.isPm = item.IsPm;
        this.hasPermission = item.HasPermission;
        this.fieldCode = item.FieldCode;
        var selelectField = father.loadedFieldsResults.filter(a => a['FieldCode'] == this.fieldCode)[0];
        this.textCode = selelectField && !AppTool.IsNullOrUndefined(selelectField['TextCode']) ? selelectField['TextCode'] : "";
        this.defaultText = selelectField && !AppTool.IsNullOrUndefined(selelectField['DefaultText']) ? selelectField['DefaultText'] : "";
        this.displayText = selelectField && !AppTool.IsNullOrUndefined(selelectField['DisplayText']) ? selelectField['DisplayText'] : "";
        this.createdBy = item.CreatedBy;
        this.modifiedBy = item.ModifiedBy;
        this.modifiedOn = item.ModifiedOn;
        this.SetBackgroundColor();
    }

    SetBackgroundColor() {
        if (this.CreatedBy?.toLowerCase() != "system") {
            this.Background = "rgba(255, 171, 3, 0.6)";
            this.DefaultTextBackground = "rgba(255, 171, 3, 0.6)";
        }
    }

    private isList: boolean =false;
    get IsList() {
        return this.isList;
    }
    
    private isPm: boolean = false;
    get IsPm() {
        return this.isPm;
    }

    private fieldCode: string = "";
    get FieldCode() {
        return this.fieldCode;
    }
    set FieldCode(value) {
        if (value != this.fieldCode) {
            this.fieldCode = value;
        }
    }

    private hasPermission = false;
    get HasPermission() { return this.hasPermission; }
    set HasPermission(value) {
        if (value != this.hasPermission) {
            this.hasPermission = value;
            this.UpdatePermission(value);
        }
    }

    public UpdatePermission(newValue) {
        this.father.IsModifiedLables = true;
        this.ModifiedBy = SessionLocator.LoggedUserPM.EnglishName;
        this.ModifiedOn = DateTool.GetCurrentDateTimeAsUtc();
        this.father.IsDirty = true;
        if (this.father.customizationEditComponent != null) this.father.customizationEditComponent.IsDirty = true;
    }

    HasPermissionClicked(item) {
        this.HasPermission = !item.HasPermission;
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
            this.UpdateModifiedLables(value);
        }
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

    private createdBy = "";
    get CreatedBy() { return this.createdBy; }
    set CreatedBy(value) {
        if (value != this.createdBy) {
            this.createdBy = value;
        }
    }

    private modifiedBy = "";
    get ModifiedBy() { return this.modifiedBy; }
    set ModifiedBy(value) {
        if (value != this.modifiedBy) {
            this.modifiedBy = value;
        }
    }

    private modifiedOn = null;
    get ModifiedOn() { return this.modifiedOn; }
    set ModifiedOn(value: Date) {
        if (value != this.modifiedOn) {
            this.modifiedOn = value;
        }
    }

    public UpdateModifiedLables(newValue) {

        if (this.father.ModifiedFields.Lables == null) {
            this.father.ModifiedFields.Lables = [];
        }

        this.father.IsModifiedFields = true;
        this.father.IsModifiedLables = true;
        this.father.IsDirty = true;
        if (this.father.customizationEditComponent!= null) this.father.customizationEditComponent.IsDirty = true;
        var label = this.father.ModifiedFields?.Lables?.filter(d => d.FieldCode == this.fieldCode)[0];
        var index = this.father.ModifiedFields?.Lables?.indexOf(label);
        if (index != null && index != -1) {
            this.father.ModifiedFields.Lables.splice(index, 1);
        }

        var newLabel = new DigitalTextCodeObject();
        newLabel.TextCode = this.textCode;
        newLabel.FieldCode = this.fieldCode;
        newLabel.DefaultText = this.defaultText;
        newLabel.DisplayText = this.displayText;
        this.ModifiedBy = SessionLocator.LoggedUserPM.EnglishName;
        this.ModifiedOn = DateTool.GetCurrentDateTimeAsUtc();
        this.father.ModifiedFields.Lables.push(newLabel);
    }

}
