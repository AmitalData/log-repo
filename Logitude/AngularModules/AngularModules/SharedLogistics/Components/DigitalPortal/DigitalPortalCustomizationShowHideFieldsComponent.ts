import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService, DigitalFeildSecurityObjectModel, DigitalFeildSecurityUpdateModel} from '../../../Infrastructure/Services/WebServices/DigitalTextService'

@Component({
    templateUrl: './DigitalPortalCustomizationShowHideFieldsComponent.html',
})

export class DigitalPortalCustomizationShowHideFieldsComponent extends BaseComponent {

    private digitalTextService: DigitalTextService;
    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    public FieldsItemsSource: ObservableCollection;
    public ModifiedLables: DigitalFeildSecurityObjectModel;
    public IsModifiedLables = false;

    constructor() {
        super();
        this.digitalTextService = new DigitalTextService();
        this.FieldsItemsSource = new ObservableCollection([]);
        this.ModifiedLables = new DigitalFeildSecurityObjectModel();
        this.ModifiedLables.Lables = [];
        this.IsModifiedLables = false;
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
        var profilesList: [];
        var objectTableId = this.SelectedObjectTableItem.Name;
        var profileId = this.SelectedProfileItem.Code;
        this.digitalTextService.GetFeildPermissionByFilters(null, objectTableId, profileId).subscribe((myResult) => {
            if (!myResult.HasError) {
                profilesList = myResult.Result;
                this.FieldsItemsSource.InsertCollection(profilesList);
            }
        });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    Save() {
        if (this.IsModifiedLables) {
            this.ModifiedLables.ObjectTableId = this.SelectedObjectTableItem.Name;
            this.ModifiedLables.ProfileId = this.SelectedProfileItem.Code;
            this.digitalTextService.UpdateFeildPermission(this.ModifiedLables).subscribe((myResult) => {
                //this.customizationEditComponent.CurrentSession.CloseCurrentWindow();
                this.customizationEditComponent.IsDirty = false;

            });
        }
    }
}

export class CustomizationLabelItem extends BaseComponent {

    public DataContext: CustomizationLabelItem = this;

    constructor(public father: DigitalPortalCustomizationShowHideFieldsComponent, item) {
        super();
        this.hasPersmission = item.HasPersmission;
        this.field = item.Field;
    }

    private field: string = "";
    get Field() {
        return this.field;
    }
    set Field(value) {
        if (value != this.field) {
            this.field = value;
        }
    }

    private hasPersmission = false;
    get HasPersmission() { return this.hasPersmission; }
    set HasPersmission(value) {
        if (value != this.hasPersmission) {
            this.hasPersmission = value;
            this.UpdateModifiedLables(value);
        }
    }

    public UpdateModifiedLables(newValue) {
        this.father.IsModifiedLables = true;
        this.father.customizationEditComponent.IsDirty = true;
        var newLabel = new DigitalFeildSecurityUpdateModel();
        newLabel.Field = this.Field;
        this.father.ModifiedLables.Lables.push(newLabel);
    }
}
