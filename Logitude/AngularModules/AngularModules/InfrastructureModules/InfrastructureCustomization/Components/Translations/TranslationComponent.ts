import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GeneralDomainService, TextCodeType, FieldsTranslations, FieldsUpdateHelper} from '../../../../Infrastructure/Services/GeneralDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {CachedDataManager} from '../../../../Infrastructure/Utilities/CachedDataManager';

@Component({
    
    templateUrl: './TranslationComponent.html',
})

export class TranslationComponent extends BaseComponent  {
    public DataContext: TranslationComponent = this;
    private myService: GeneralDomainService;
    public DirtyItems: FieldsTranslations[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.ItemsSource = new ObservableCollection([]);
        this.myService = new GeneralDomainService();
        this.DirtyItems = [];
        this.LoadTextCodeTypes();
    }
    
    public Count: number;

    private LoadTextCodeTypes() {
        this.myService.GetTextCodeTypes().subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                this.BuildComponentComboList(myResponse.Result);
            }
        });
    }

    public ComponentComboList: TextCodeType[];
    private BuildComponentComboList(list: TextCodeType[]) {
        this.ComponentComboList = [];

        list.forEach((item) => {
            this.ComponentComboList.push(item);
        });
    }

    private selectedComponentFilter: TextCodeType;
    get SelectedComponentFilter() { return this.selectedComponentFilter; }
    set SelectedComponentFilter(value: TextCodeType) {
        if (this.selectedComponentFilter != value) {
            this.selectedComponentFilter = value;

            this.LoadTranslations();
        }
    }

    private objectTableId: string;
    get ObjectTableId() { return this.objectTableId; }
    set ObjectTableId(value: string) {
        if (this.objectTableId != value) {
            this.objectTableId = value;

            if (!AppTool.IsNullOrEmpty(value)) {
                this.LoadTranslations();
            }
        }
    }

    public SearchText: string = null;
    OnSearchTextChanged(text: string) {
        this.SearchText = text;

        this.BuildItemsSource();
    }

    public ItemsSource: ObservableCollection;
    private loadedTranslations: FieldsTranslations[];
    private isLoading: boolean = false;
    private LoadTranslations() {
        var code = null;
        if (this.SelectedComponentFilter != null) {
            code = this.SelectedComponentFilter.Code;
        }

        if (!this.isLoading) {
            this.isLoading = true;
            this.CurrentSession.StartBusyIndicatorLoading();

            this.myService.LoadAllFieldsTranslations(SessionLocator.TenantPM.Language, this.ObjectTableId, code).subscribe((myResult: ServiceResponse) => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.loadedTranslations = myResponse.Result;
                    //this.isLoading = false;

                    if (this.loadedTranslations != null) {
                        this.BuildItemsSource();
                    }
                }
            });
        }
    }

    BuildItemsSource() {
        this.ItemsSource.Clear();

        var list: TranslationItem[] = [];

        if (AppTool.IsNullOrEmpty(this.SearchText)) {
            this.loadedTranslations.forEach((item) => {
                list.push(new TranslationItem(item, this));
            })
        }

        else {
            this.loadedTranslations.filter(d => !AppTool.IsNullOrEmpty(d.DefaultText) && d.DefaultText.toUpperCase().startsWith(this.SearchText.toUpperCase())
                || !AppTool.IsNullOrEmpty(d.TranslatedText) && d.TranslatedText.toUpperCase().startsWith(this.SearchText.toUpperCase())
                || !AppTool.IsNullOrEmpty(d.TranslatedTextPlural) && d.TranslatedTextPlural.toUpperCase().startsWith(this.SearchText.toUpperCase()))
                .forEach((item) => {
                    list.push(new TranslationItem(item, this));
                });
        }

        this.isLoading = false;
        this.ItemsSource.InsertCollection(list);
        this.Count = this.ItemsSource.Length;

        this.CurrentSession.StopBusyIndicator();
    }

    public SelectedRow: FieldsTranslations = null;
    OnRowSelected(itemComponent: FieldsTranslations) {
        this.SelectedRow = itemComponent;
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    
    SaveClicked() {
        if (this.DirtyItems.length > 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var myServiceHelper = new FieldsUpdateHelper();
            myServiceHelper.Tenant = SessionLocator.Tenant;
            myServiceHelper.Items = this.DirtyItems;

            var generalService: GeneralDomainService = new GeneralDomainService();
            generalService.UpdateFieldsTranslations(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {                    
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    CachedDataManager.RefreshTenantTextCodes().subscribe((response:any) => {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                    });
                }
            });
        }
    }
}

export class TranslationItem extends BaseComponent {
    public Entity: FieldsTranslations;
    public ObjectTableName: string = "FieldsTranslation";
    public DataContext: TranslationItem = this;

    constructor(entity: FieldsTranslations, public fatherComponent: TranslationComponent) {
        super();
        this.Entity = entity;
    }

    get Code() { return this.Entity.Code; }
    get DefaultText() { return this.Entity.DefaultText; }
    
    get TranslatedText() { return this.Entity.TranslatedText; }
    set TranslatedText(newValue: string) {
        if (this.Entity.TranslatedText != newValue) {
            this.Entity.TranslatedText = newValue;

            if (this.fatherComponent.DirtyItems.indexOf(this.Entity) == -1) {
                this.fatherComponent.DirtyItems.push(this.Entity);
            } 
        }
    }

    get TranslatedTextPlural() { return this.Entity.TranslatedTextPlural; }
    set TranslatedTextPlural(newValue: string) {
        if (this.Entity.TranslatedTextPlural != newValue) {
            this.Entity.TranslatedTextPlural = newValue;

            if (this.fatherComponent.DirtyItems.indexOf(this.Entity) == -1) {
                this.fatherComponent.DirtyItems.push(this.Entity);
            } 
        }
    }
}
