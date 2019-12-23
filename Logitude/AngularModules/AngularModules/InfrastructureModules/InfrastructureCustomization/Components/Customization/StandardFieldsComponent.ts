import {Component} from '@angular/core';
import {GeneralDomainService, FieldsTranslations} from '../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {TextCodePM} from '../../../../Infrastructure/EntityPMs/TextCodePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {ObjectFieldPMService} from '../../../../Infrastructure/Services/StandardPMs/ObjectFieldPMService';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './StandardFieldsComponent.html',
})

export class StandardFieldsComponent {
    private myService: GeneralDomainService;
    private ObjecttableId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityListService: EntityListService) {
        this.myService = new GeneralDomainService();
    }

    SetWindowArgs(windowArgs: any) {
        this.ObjecttableId = windowArgs['ObjectTableId'];
        this.BuildTabsItemsSource();
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
        this.SelectedTabItem.BuildItemsSource(text);
    }

    public Tabs: Array<TabItem>;
    private BuildTabsItemsSource() {
        var objectTablePM: ObjectTablePM;
        var tableName: string;
        this.Tabs = [];

        objectTablePM = window.ObjectTables.filter(d => d.Id == this.ObjecttableId)[0];
        if (objectTablePM != null) {
            this.Tabs.push(new TabItem(objectTablePM, this));
        }

        var tableIds: string[] = [];
        var mulityList: ObjectFieldPM[] = window.ObjectFields.filter(d => d.ObjectTableId == this.ObjecttableId && d.IsMulti);

        mulityList.forEach((item) => {
            var index = tableIds.indexOf(item.MultiTableId);

            if (index == -1) {
                tableIds.push(item.MultiTableId);

                objectTablePM = window.ObjectTables.filter(d => d.Id == item.MultiTableId)[0];
                if (objectTablePM != null) {
                    this.Tabs.push(new TabItem(objectTablePM, this));
                }
            }
        });

        this.SelectedTabItem = this.Tabs[0];
    }

    private selectedTabItem: TabItem;
    get SelectedTabItem() { return this.selectedTabItem; }
    set SelectedTabItem(newValue: TabItem) {
        if (this.selectedTabItem != newValue) {
            this.selectedTabItem = newValue;
            this.selectedTabItem.LoadStandardFields();
        }
    }

    SelectionChanged(clickdTab: TabItem) {
        if (clickdTab != null) {
            if (this.SelectedTabItem != clickdTab) {
                this.SelectedTabItem = clickdTab;
            }
        }
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class TabItem {
    public ObjectTablePM: ObjectTablePM;
    public ObjectTableId: string;
    public Header: string;
    public FieldsItemsSource: StandardFieldItem[];
    private myService: GeneralDomainService;
    public EntityTranslations: FieldsTranslations[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(objectTablePM: ObjectTablePM, public fatherComponent: StandardFieldsComponent) {
        this.ObjectTablePM = objectTablePM;
        this.ObjectTableId = objectTablePM.Id;
        this.myService = new GeneralDomainService();

        this.SetTabHeader();
    }

    private SetTabHeader() {
        this.Header = TextCodeTranslator.TranslateTable(this.ObjectTablePM.Name);
    }

    private loadedFields: ObjectFieldPM[];
    public LoadStandardFields() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myService.GetStandardFieldsByTableId(this.ObjectTableId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                this.loadedFields = myResponse.Result;
                if (this.loadedFields != null) {
                    this.LoadTranslationsForMultiEntity();
                }
            }
        });
    }

    private LoadTranslationsForMultiEntity() {
        this.myService.GetTranslationsByParam(null, this.ObjectTableId, SessionLocator.TenantPM.Language).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this.EntityTranslations = myResponse.Result;
                this.BuildItemsSource();
            }
        });
    }

    public BuildItemsSource(searchText: string = null) {
        this.FieldsItemsSource = [];

        if (AppTool.IsNullOrEmpty(searchText)) {
            this.loadedFields.forEach((item) => {
                this.FieldsItemsSource.push(new StandardFieldItem(item, this.loadedFields, this.EntityTranslations));
            });
        }

        else {
            this.loadedFields.forEach((item) => {
                if (!AppTool.IsNullOrEmpty(item.FullNameTextCodeDefaultText) && item.FullNameTextCodeDefaultText.toUpperCase().indexOf(searchText.toUpperCase()) > -1) {
                    this.FieldsItemsSource.push(new StandardFieldItem(item, this.loadedFields, this.EntityTranslations));
                }
            });
        }

        this.CurrentSession.StopBusyIndicator();
    }

    public EditField(editedItem: StandardFieldItem) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit Standard Field";
        logitudeWindow.WindowArgs = editedItem;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/EditStandardFieldComponent');

        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "Ok") {
                this.LoadStandardFields();
            }

        });
    }
}

export class StandardFieldItem {
    private ObjectField: ObjectFieldPM;
    public ObjectFieldId: string;

    public fullLabelObject: FieldsTranslations = new FieldsTranslations();
    public shortLabelObject: FieldsTranslations = new FieldsTranslations();
    public listLabelObject: FieldsTranslations = new FieldsTranslations();
    public helpLabelObject: FieldsTranslations = new FieldsTranslations();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(field: ObjectFieldPM, public loadedFields: ObjectFieldPM[], public fieldsTranslations: FieldsTranslations[]) {
        this.ObjectField = field;
        this.ObjectFieldId = field.Id;
        //Fix ObjectFieldTextCodeId when you add TextCodeCode to Translations table.
        this.fullLabelObject = this.fieldsTranslations.filter(f => f.TextCodeId == field.FullNameTextCodeId)[0];
        this.shortLabelObject = this.fieldsTranslations.filter(f => f.TextCodeId == field.ShortNameTextCodeId)[0];
        this.listLabelObject = this.fieldsTranslations.filter(f => f.TextCodeId == field.ListTextCodeId)[0];
        this.helpLabelObject = this.fieldsTranslations.filter(f => f.TextCodeId == field.HelpTextCodeId)[0];
    }

    get DefaultText() { return this.ObjectField.FullNameTextCodeDefaultText; }
    get IsRequiered() { return this.ObjectField.IsRequiered; }
    get FullLabelText() { return this.fullLabelObject == null ? "" : this.fullLabelObject.TranslatedText; }
    get HelpTextText() { return this.helpLabelObject == null ? "" : this.helpLabelObject.TranslatedText; }

    get DataTypeText() {
        var result: string = this.ObjectField.DataTypeCode;

        if (!AppTool.IsNullOrEmpty(this.ObjectField.ObjectTable_LookUpTableName) && this.ObjectField.DataTypeCode.toLowerCase() == "lookup") {
            result += " (" + this.ObjectField.ObjectTable_LookUpTableName + ")";
        }

        return result;
    }

    get IsEdited() {
        var result: boolean = false;

        if (this.ObjectField.SystemRequired || this.ObjectField.SystemMaxLength > 0) {
            result = true;
        }

        return result;
    }

    //private LoadObjects() {
    //    var generalService: GeneralDomainService = new GeneralDomainService();

    //    generalService.(this.ObjectField.FullNameTextCodeId).subscribe(myResult => {
    //        var myResponse: ServiceResponse = myResult;
    //        if (!myResponse.HasError) {
    //            this.fullLabelObject = myResponse.Result;
    //        }
    //    });

    //    generalService.GetSingleObjectFieldFromZeroTenant(this.ObjectField.ShortNameTextCodeId).subscribe(myResult => {
    //        var myResponse: ServiceResponse = myResult;
    //        if (!myResponse.HasError) {
    //            this.shortLabelObject = myResponse.Result;
    //        }
    //    });

    //    generalService.GetSingleObjectFieldFromZeroTenant(this.ObjectField.ListTextCodeId).subscribe(myResult => {
    //        var myResponse: ServiceResponse = myResult;
    //        if (!myResponse.HasError) {
    //            this.listLabelObject = myResponse.Result;
    //        }
    //    });

    //    generalService.GetSingleObjectFieldFromZeroTenant(this.ObjectField.HelpTextCodeId).subscribe(myResult => {
    //        var myResponse: ServiceResponse = myResult;
    //        if (!myResponse.HasError) {
    //            this.helpLabelObject = myResponse.Result;
    //        }
    //    });
    //}
}
