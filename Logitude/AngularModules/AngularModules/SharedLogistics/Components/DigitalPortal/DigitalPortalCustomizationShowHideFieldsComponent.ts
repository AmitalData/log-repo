import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DigitalPortalCustomizationMainComponent } from './DigitalPortalCustomizationMainComponent';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService, DigitalTextCodeUpdateModel } from '../../../Infrastructure/Services/WebServices/DigitalTextService'

@Component({
    templateUrl: './DigitalPortalCustomizationShowHideFieldsComponent.html',
})

export class DigitalPortalCustomizationShowHideFieldsComponent extends BaseComponent {

    private digitalTextService: DigitalTextService;
    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: DigitalPortalCustomizationMainComponent;
    public FieldsItemsSource: ObservableCollection;
    public ModifiedLables: DigitalTextCodeUpdateModel;
    public IsModifiedLables = false;

    constructor() {
        super();
        this.digitalTextService = new DigitalTextService();
        this.FieldsItemsSource = new ObservableCollection([]);
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

    BuildItemsSource() {
        
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    Save() {
        if (this.IsModifiedLables) {
            this.ModifiedLables.ObjectTableId = this.SelectedObjectTableItem.Name;
            //this.customizationEditComponent.CurrentSession.CloseCurrentWindow();
            this.customizationEditComponent.IsDirty = false;
        }
    }
}
