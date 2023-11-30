import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {GeneralDomainService, FieldsTranslations, FieldsUpdateHelper} from '../../../../Infrastructure/Services/GeneralDomainService';
import {TranslateLablesService, TranslateLabelsAPIHelper} from '../../../../Infrastructure/Services/TranslateLablesService';
import {CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass'; 
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {CachedDataManager} from '../../../../Infrastructure/Utilities/CachedDataManager';

@Component({
    
    templateUrl: './TranslateLabelsComponent.html',
})

export class TranslateLabelsComponent extends BaseComponent {    
    public DataContext: TranslateLabelsComponent = this;
    public ItemsSource: ObservableCollection;
    public ValidationErrorsList: string[] = [];
    public ComponentId: string;
    TranslateLablesService: TranslateLablesService;
    public HasChanges: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ComponentId = "TranslateLabels_" + this.CurrentSession.GetNewId("TranslateLabels");

        this.TranslateLablesService = new TranslateLablesService();
        this.ItemsSource = new ObservableCollection([]);
        this.BuildFilters();
    }

    IsFromCustomizedScreen: boolean;
    private selectedLanguageCode: string;
    private allTranslationsList: FieldsTranslations[];
    SetWindowArgs(args: any) {
        this.selectedLanguageCode = args.TranslationLanguageCode;
        if (args.ObjectTableId) {
            this.IsFromCustomizedScreen = true;
            this.ObjectTableId = args.ObjectTableId;
        }

        this.allTranslationsList = [];
        this.LoadAllTranslationMethod();
    }

    LoadAllTranslationMethod() {
        this.CurrentSession.StartBusyIndicatorLoading();

        var myServiceHelper = new TranslateLabelsAPIHelper();
        myServiceHelper.Language = this.selectedLanguageCode;
        myServiceHelper.ObjectTableId = this.ObjectTableId;
        myServiceHelper.SpellCheckedFilterCode = this.SpellCheckedSelectedItem == null ? null : this.SpellCheckedSelectedItem.Code;
        myServiceHelper.CheckDateFilerCode = this.DateCheckedSelectedItem == null ? null : this.DateCheckedSelectedItem.Code;
        myServiceHelper.TextCodeTypeCode = this.TextCodeTypeSelectedItem == null ? null : this.TextCodeTypeSelectedItem.Code;
        myServiceHelper.SelectedCheckDate = this.SelectedCheckDate;
        myServiceHelper.SearchText = this.SearchText;
        myServiceHelper.SkipDigit = this.SkipDigit;
        myServiceHelper.TakeDigit = this.TakeDigit;

        this.TranslateLablesService.Post(myServiceHelper).subscribe((myResult: ServiceResponse) => {
            if (myResult.HasError) {
                this.ValidationErrorsList = myResult.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                var list: TranslateLabelsAPIHelper = myResult.Result;
                if (list != null) {
                    this.allTranslationsList = list.Translations;
                    this.TextCodesCount = list.CountAll;

                    this.GetFilterdData();
                }
            }
        });
    }

    GetFilterdData() {
        if (this.SkipDigit < 0) {
            this.SkipDigit = 0;
        }

        if (this.TextCodesCount != null) {
            if (this.SkipDigit > this.TextCodesCount) {
                this.SkipDigit = this.TextCodesCount;
            }
        }

        this.SetButons();
        this.BuildData();        
    }

    private itemsCollection: TranslateLabelsItem[];
    private filterdranslationsList: FieldsTranslations[];
    BuildData() { 
        this.CurrentSession.StartBusyIndicatorLoading();

        this.ItemsSource = new ObservableCollection([]);
        this.itemsCollection = [];
        
        this.filterdranslationsList = this.allTranslationsList;
        this.StartSearchFilter();
    }
    
    private SkipDigit: number = 0;
    private TakeDigit: number = 20;
    private TextCodesCount: number = null;    
    public StartSearchFilter() {
        if (this.filterdranslationsList == null) {
            return;
        }

        this.filterdranslationsList.forEach(field => {
            this.itemsCollection.push(new TranslateLabelsItem(field, this));
        });

        this.ItemsSource.InsertCollection(this.itemsCollection);

        var resultStart = 0;
        if (this.TextCodesCount != null) {
            if (this.TextCodesCount > (this.SkipDigit + this.TakeDigit)) {
                resultStart = this.SkipDigit + this.TakeDigit;
            }

            else {
                resultStart = this.TextCodesCount;
            }
        }

        this.CountText = resultStart + " of " + this.TextCodesCount;
        
        this.CurrentSession.StopBusyIndicator();
        this.HasChanges = false;
    }
    
    public SelectedRow: TranslateLabelsItem = null;
    OnRowSelected(itemComponent: TranslateLabelsItem) {
        this.SelectedRow = itemComponent;
    }

    private countText: string;
    get CountText() { return this.countText; }
    set CountText(value: string) {
        if (this.countText != value) {
            this.countText = value;
        }
    }

    public SearchText: string = null;
    OnSearchTextChanged(mySearchText: string) {
        this.SearchText = mySearchText;
        this.LoadAllTranslationMethod();
    }
    
    public SpellCheckedList: CodeNameClass[] = [];
    public DateCheckedList: CodeNameClass[] = [];
    public TextCodeTypesList: CodeNameClass[] = [];
    BuildFilters() {
        this.SpellCheckedList = [];
        this.SpellCheckedList.push(new CodeNameClass("None", "None"));
        this.SpellCheckedList.push(new CodeNameClass("True", "Yes"));
        this.SpellCheckedList.push(new CodeNameClass("False", "No"));
        this.spellCheckedSelectedItem = this.SpellCheckedList[0];

        this.DateCheckedList = [];
        this.DateCheckedList.push(new CodeNameClass("None", "None"));
        this.DateCheckedList.push(new CodeNameClass("Equals", "Equals"));
        this.DateCheckedList.push(new CodeNameClass("Bigger", "Bigger"));
        this.DateCheckedList.push(new CodeNameClass("Less", "Less"));
        this.dateCheckedSelectedItem = this.DateCheckedList[0];

        this.TextCodeTypesList = [];
        this.TextCodeTypesList.push(new CodeNameClass("All", "All"));
        this.TextCodeTypesList.push(new CodeNameClass("F", "Fields"));
        this.TextCodeTypesList.push(new CodeNameClass("CH", "Column Headers"));
        this.TextCodeTypesList.push(new CodeNameClass("B", "Buttons And Actions"));
        this.TextCodeTypesList.push(new CodeNameClass("H", "Help Text"));
        this.TextCodeTypesList.push(new CodeNameClass("M", "Messages"));
        this.TextCodeTypesList.push(new CodeNameClass("MH", "Menu Headers"));
        this.TextCodeTypesList.push(new CodeNameClass("MC", "Maintenance"));
        this.TextCodeTypesList.push(new CodeNameClass("L", "Links"));
        this.TextCodeTypesList.push(new CodeNameClass("O", "Others"));
        this.TextCodeTypesList.push(new CodeNameClass("G", "General"));
        this.textCodeTypeSelectedItem = this.TextCodeTypesList[0];

        this.StartSearchFilter();
    }

    private objectTableId: string = null;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(value: string) {
        if (this.objectTableId != value) {
            this.objectTableId = value;
            this.LoadAllTranslationMethod();
        }
    }

    private spellCheckedSelectedItem: CodeNameClass = null;
    public get SpellCheckedSelectedItem() { return this.spellCheckedSelectedItem; }
    public set SpellCheckedSelectedItem(value: CodeNameClass) {
        if (this.spellCheckedSelectedItem != value) {
            this.spellCheckedSelectedItem = value;
            this.LoadAllTranslationMethod();
        }
    }

    private dateCheckedSelectedItem: CodeNameClass = null;
    public get DateCheckedSelectedItem() { return this.dateCheckedSelectedItem; }
    public set DateCheckedSelectedItem(value: CodeNameClass) {
        if (this.dateCheckedSelectedItem != value) {
            this.dateCheckedSelectedItem = value;

            if (value == null) {
                this.selectedCheckDate = null;
            }

            else if (value.Code == "None") {
                this.selectedCheckDate = null;
            }

            else {
                this.selectedCheckDate = DateTool.GetCurrentDateAsUtc();
            }

            this.LoadAllTranslationMethod();
        }
    }

    private textCodeTypeSelectedItem: CodeNameClass = null;
    public get TextCodeTypeSelectedItem() { return this.textCodeTypeSelectedItem; }
    public set TextCodeTypeSelectedItem(value: CodeNameClass) {
        if (this.textCodeTypeSelectedItem != value) {
            this.textCodeTypeSelectedItem = value;
            this.LoadAllTranslationMethod();
        }
    }

    private selectedCheckDate: Date = null;
    public get SelectedCheckDate() { return this.selectedCheckDate; }
    public set SelectedCheckDate(value: Date) {
        if (this.selectedCheckDate != value) {
            this.selectedCheckDate = value;

            if (value == null) {
                this.dateCheckedSelectedItem = this.DateCheckedList.filter(d => d.Code == "None")[0];
            }

            else if (this.dateCheckedSelectedItem == null) {
                this.dateCheckedSelectedItem = this.DateCheckedList.filter(d => d.Code == "Equals")[0];
            }

            else if (this.dateCheckedSelectedItem.Code == "None") {
                this.dateCheckedSelectedItem = this.DateCheckedList.filter(d => d.Code == "Equals")[0];
            }

            this.LoadAllTranslationMethod();
        }
    }
        
    public PreviousButtonIsEnabled: boolean = false;
    public NextButtonIsEnabled: boolean = false;
    SetButons() {
        this.PreviousButtonIsEnabled = this.SkipDigit > 0 ? true : false;
        this.NextButtonIsEnabled = (this.SkipDigit + this.TakeDigit) < this.TextCodesCount ? true : false;
    }

    SaveAndPreviousClicked() {
        if (this.HasChanges) {
            this.SkipDigit = this.SkipDigit - this.TakeDigit;
            this.SaveChanges();
        }

        else {
            this.SkipDigit = this.SkipDigit - this.TakeDigit;
            this.LoadAllTranslationMethod();
        }
    }
    SaveAndNextClicked() {
        if (this.HasChanges) {
            this.SkipDigit = this.SkipDigit + this.TakeDigit;
            this.SaveChanges();            
        }

        else {
            this.SkipDigit = this.SkipDigit + this.TakeDigit;
            this.LoadAllTranslationMethod();
        }
    }
    CloseClicked() {
        if (this.HasChanges) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.DontSave");
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", "Data"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {

                    this.SaveChanges(true);
                }

                else if (confirmWindow.No) {
                    this.CloseWindow();
                }
            });
        }

        else {
            this.CloseWindow();
        }
    }
    CloseWindow() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveChanges(isClosing: boolean = false) {
        var list: FieldsTranslations[] = this.GetDirtyFieldsTranslations();

        if (list.length > 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var myServiceHelper = new FieldsUpdateHelper();
            myServiceHelper.Tenant = SessionLocator.Tenant;
            myServiceHelper.Items = list;

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

    UpdateHasChanges() {
        var list: FieldsTranslations[] = this.GetDirtyFieldsTranslations();

        if (list.length == 0) {
            this.HasChanges = false;
        }

        else {
            this.HasChanges = true;
        }
    }

    private GetDirtyFieldsTranslations() {
        var myResult: FieldsTranslations[] = [];

        this.ItemsSource.Collection.forEach((item: TranslateLabelsItem) => {
            if (item.fieldsTranslations.IsDirty) {
                myResult.push(item.fieldsTranslations);
            }
        });

        return myResult;
    }
}

export class TranslateLabelsItem extends BaseComponent{
    public fieldsTranslations: FieldsTranslations;
    private isTranslatedOrigin: boolean;
    private defaultTextOrigin: string;
    private translatedTextOrigin: string;
    private translateDateOrigin: Date;
    private translatedByUserIdOrigin: string;
    public CellBackgroundColor = "#E6E7E8";
    private DefaultText_Cloned: string;
    constructor(item: FieldsTranslations, public fatherComponent: TranslateLabelsComponent) {
        super();
        this.fieldsTranslations = item;
        this.isTranslatedOrigin = item.IsTranslated;
        this.defaultTextOrigin = item.DefaultText;
        this.translatedTextOrigin = item.TranslatedText;
        this.translateDateOrigin = item.TranslateDate;
        this.translatedByUserIdOrigin = item.TranslatedByUserId;
        this.TranslatedBy = this.translatedByUserIdOrigin;
    }

    get Code() { return this.fieldsTranslations.Code; }
    get DefaultText() { return this.fieldsTranslations.DefaultText; }

    get TranslatedText() { return this.fieldsTranslations.TranslatedText; }
    set TranslatedText(value: string) {
        if (this.fieldsTranslations.TranslatedText != value) {
            this.fieldsTranslations.TranslatedText = value;

            if (value == this.defaultTextOrigin) {
                this.IsTranslated = false;
                this.TranslateDate = null;
                this.TranslatedBy = null;
            }

            else if (value == this.translatedTextOrigin) {
                this.IsTranslated = this.isTranslatedOrigin;
                this.TranslateDate = this.translateDateOrigin;
                this.TranslatedBy = this.translatedByUserIdOrigin;
            }

            else {
                this.IsTranslated = true;
                this.TranslateDate = DateTool.GetCurrentDateAsUtc();
                this.TranslatedBy = SessionLocator.LoggedUserId;
            }

            this.OnDataChanged();
        }
    }

    get IsTranslated() { return this.fieldsTranslations.IsTranslated; }
    set IsTranslated(value: boolean) {
        if (this.fieldsTranslations.IsTranslated != value) {
            this.fieldsTranslations.IsTranslated = value;
        }
    }

    get TranslateDate() { return this.fieldsTranslations.TranslateDate; }
    set TranslateDate(value: Date) {
        if (this.fieldsTranslations.TranslateDate != value) {
            this.fieldsTranslations.TranslateDate = value;
        }
    }

    private translatedBy: string;
    get TranslatedBy() { return this.translatedBy; }
    set TranslatedBy(value: string) {
        this.fieldsTranslations.TranslatedByUserId = value;
        this.translatedBy = "";
        if (!AppTool.IsNullOrEmpty(this.fieldsTranslations.TranslatedByUserId)) {
            //UserList user = UserDataProvider.GetCachedList<UserList>().Where(d => d.Id == fieldsTranslations.TranslatedByUserId).FirstOrDefault();
            //if (user != null) {
            //    translatedBy = user.EnglishName;
            //}
        }
    }

    OnDataChanged() {
        var isDirty = false;

        if (this.TranslatedText != this.translatedTextOrigin) {
            isDirty = true;
        }
        
        if (this.fieldsTranslations.IsDirty != isDirty) {
            this.fieldsTranslations.IsDirty = isDirty;
        }

        this.fatherComponent.UpdateHasChanges();
    }
}
