import { Component, EventEmitter, Output} from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService, DigitalFeildSecurityObjectModel, DigitalFeildSecurityUpdateModel, DigitalTextCodeObject, DigitalTextCodeUpdateModel } from '../../../Infrastructure/Services/WebServices/DigitalTextService'
import { AppTool } from '../../../Infrastructure/Tools';

@Component({
    templateUrl: './DigitalPortalCustomizationShowHideFieldsComponent.html',
})

export class DigitalPortalCustomizationShowHideFieldsComponent extends BaseComponent {

    private digitalTextService: DigitalTextService;
    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    public FieldsItemsSource: ObservableCollection;
    public loadedFieldsResults: [];
    public ModifiedLables: DigitalFeildSecurityObjectModel;
    public IsModifiedLables = false
    public ModifiedFields: DigitalTextCodeUpdateModel;
    public IsModifiedFields = false;

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
        this.FillDigitalProfileFiltersList();
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
        this.digitalTextService.GetDigitalProfilesObjetTables().subscribe((myResult) => {
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
        this.digitalTextService.GetDigitalProfileName().subscribe((myResult) => {
            if (!myResult.HasError) {
                var objectTables = myResult.Result;
                objectTables.forEach(item => {
                    this.DigitalProfileFilterList.push(new CodeNameClass(item.Id, item.Name));
                });

                this.selectedProfileItem = this.DigitalProfileFilterList[0];
                this.FillObjectTablesFiltersList();
            }
        });
    }

    BuildItemsSource() {
        this.BuildFields();
    }

    BuildFields() {
        var objectTableId = this.SelectedObjectTableItem.Name;
        var profileId = this.SelectedProfileItem.Code;
        this.digitalTextService.GetTextCodesByFilters(null, objectTableId, profileId).subscribe((myResult) => {
            if (!myResult.HasError) {
                this.loadedFieldsResults = myResult.Result.filter(a => !AppTool.IsNullOrEmpty(a['FieldCode']));
                this.BuildFieldsPremissions();
            }
        });
    }

    BuildFieldsPremissions() {
        this.FieldsItemsSource = new ObservableCollection([]);
        var profilesList: ProfileFieldsItem[] = [];
        var objectTableId = this.SelectedObjectTableItem.Name;
        var profileId = this.SelectedProfileItem.Code;
        this.digitalTextService.GetFeildPermissionByFilters(null, objectTableId, profileId).subscribe((myResult) => {
            if (!myResult.HasError) {
                myResult.Result.filter(a => !AppTool.IsNullOrEmpty(a.FieldCode)).forEach(item => {
                    profilesList.push(new ProfileFieldsItem(this, item));
                });
                this.FieldsItemsSource.InsertCollection(profilesList);
            }
        });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    Cancel() {
        this.customizationEditComponent.IsDirty = false;
        if (this.customizationEditComponent.NewSelectedMenu) {
            this.customizationEditComponent.SelectedMenu = this.customizationEditComponent.NewSelectedMenu;
        }
    }

    Save() {
        this.StartBusyIndicator();
        if (this.IsModifiedLables) {
            this.ModifiedLables.ObjectTableId = this.SelectedObjectTableItem.Name;
            this.ModifiedLables.ProfileId = this.SelectedProfileItem.Code;
            var hasHasPersmissionList = this.FieldsItemsSource.Collection.filter(a => a.HasPersmission);

            hasHasPersmissionList.forEach(item => {
                var newLabel = new DigitalFeildSecurityUpdateModel();
                newLabel.FieldCode = item.FieldCode;
                this.ModifiedLables.DefaultSettings.push(newLabel);
            });

            this.digitalTextService.UpdateFeildPermission(this.ModifiedLables).subscribe((myResult) => {
                this.customizationEditComponent.IsDirty = false;
                this.ModifiedLables = new DigitalFeildSecurityObjectModel();
                this.ModifiedLables.DefaultSettings = [];
                this.StopBusyIndicator();
                this.IsModifiedLables = false;
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
            this.ModifiedFields.ObjectTableId = this.SelectedObjectTableItem.Name;
            this.digitalTextService.UpdateDigitalTextCodes(this.ModifiedFields).subscribe((myResult) => {
                this.customizationEditComponent.IsDirty = false;
                this.ModifiedFields = new DigitalTextCodeUpdateModel();
                this.StopBusyIndicator();
                this.IsModifiedFields = false;
                this.CurrentSession.SessionEvent.emit({ Name: "ReloadDigitalPortalLabels" });
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
}

export class ProfileFieldsItem extends BaseComponent {

    public DataContext: ProfileFieldsItem = this;

    constructor(public father: DigitalPortalCustomizationShowHideFieldsComponent, item) {
        super();
        this.hasPersmission = item.HasPersmission;
        this.fieldCode = item.FieldCode;
        var selelectField = father.loadedFieldsResults.filter(a => a['FieldCode'] == this.fieldCode)[0];
        this.textCode = selelectField['TextCode'];
        this.defaultText = selelectField['DefaultText'];
        this.displayText = selelectField['DisplayText'];
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

    private hasPersmission = false;
    get HasPersmission() { return this.hasPersmission; }
    set HasPersmission(value) {
        if (value != this.hasPersmission) {
            this.hasPersmission = value;
            this.UpdatePersmission(value);
        }
    }

    public UpdatePersmission(newValue) {
        this.father.IsModifiedLables = true;
        this.father.customizationEditComponent.IsDirty = true;
    }

    HasPersmissionClicked(item) {
        this.HasPersmission = !item.HasPersmission;
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

    private textCode: string = "";
    get TextCode() {
        return this.textCode;
    }

    set TextCode(value) {
        if (value != this.textCode) {
            this.textCode = value;
        }
    }

    public UpdateModifiedLables(newValue) {

        if (this.father.ModifiedFields.Lables == null) {
            this.father.ModifiedFields.Lables = [];
        }

        this.father.IsModifiedFields = true;
        this.father.customizationEditComponent.IsDirty = true;
        var label = this.father.ModifiedFields?.Lables?.filter(d => d.FieldCode == this.fieldCode)[0];
        var index = this.father.ModifiedFields?.Lables?.indexOf(label);
        if (index != null && index != -1) {
            this.father.ModifiedFields.Lables.splice(index, 1);
        }

        var newLabel = new DigitalTextCodeObject();
        newLabel.TextCode = this.textCode;
        newLabel.FieldCode = this.fieldCode;
        newLabel.DefaultText = newValue;
        newLabel.DisplayText = this.displayText;
        this.father.ModifiedFields.Lables.push(newLabel);
    }

}
