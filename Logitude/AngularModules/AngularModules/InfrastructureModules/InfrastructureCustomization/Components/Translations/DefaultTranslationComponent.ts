import {Component} from '@angular/core';
import {TextCodePM} from '../../../../Infrastructure/EntityPMs/TextCodePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DefaultTranslationService, DefaultTranslationAPIHelper} from '../../../../Infrastructure/Services/DefaultTranslationService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './DefaultTranslationComponent.html',
})

export class DefaultTranslationComponent extends BaseComponent {
    public DataContext = this;
    public ItemsSource: ObservableCollection;
    public HasChanges: boolean = false;
    public ValidationErrorsList: string[] = [];
    public ComponentId: string;
    private myService: DefaultTranslationService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.ComponentId = "DefaultTranslation_" + this.CurrentSession.GetNewId("DefaultTranslation");
        this.myService = new DefaultTranslationService();
        this.ItemsSource = new ObservableCollection([]);
        this.BuildFilters();
    }

    public SelectedRow: DefaultTranslationItem = null;
    OnRowSelected(itemComponent: DefaultTranslationItem) {
        this.SelectedRow = itemComponent;
    }

    private SearchText: string = null;
    OnSearchTextChanged(mySearchText: string) {
        this.SearchText = mySearchText;
    }

    SearchButtonClicked() {
        this.StartNewSearch();
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

        this.StartNewSearch();
    }

    private objectTableId: string = null;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(value: string) {
        if (this.objectTableId != value) {
            this.objectTableId = value;
            this.StartNewSearch();
        }
    }

    private spellCheckedSelectedItem: CodeNameClass = null;
    public get SpellCheckedSelectedItem() { return this.spellCheckedSelectedItem; }
    public set SpellCheckedSelectedItem(value: CodeNameClass) {
        if (this.spellCheckedSelectedItem != value) {
            this.spellCheckedSelectedItem = value;
            this.StartNewSearch();
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

            this.StartNewSearch();
        }
    }

    private textCodeTypeSelectedItem: CodeNameClass = null;
    public get TextCodeTypeSelectedItem() { return this.textCodeTypeSelectedItem; }
    public set TextCodeTypeSelectedItem(value: CodeNameClass) {
        if (this.textCodeTypeSelectedItem != value) {
            this.textCodeTypeSelectedItem = value;
            this.StartNewSearch();
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

            this.StartNewSearch();
        }
    }

    public SkipDigit: number = 0;
    public TakeDigit: number = 20;
    public TextCodesCount: number = null;
    public StartPagerDigit: number = null;
    public SearchResultCount: number = 0;
    public PageItemsCount: number = 0;
    StartNewSearch() {
        this.SkipDigit = 0;
        this.LoadTextCodes(true);
    }
    LoadTextCodes(isNewSearching: boolean = false) {

        this.CurrentSession.StartBusyIndicatorLoading();

        this.ItemsSource.Clear();

        if (this.SkipDigit < 0) {
            this.SkipDigit = 0;
        }

        if (this.SkipDigit > this.SearchResultCount) {
            this.SkipDigit = this.SearchResultCount;
        }

        var myServiceHelper = new DefaultTranslationAPIHelper();
        myServiceHelper.ObjectTableId = this.ObjectTableId;
        myServiceHelper.SpellCheckedFilterCode = this.SpellCheckedSelectedItem == null ? null : this.SpellCheckedSelectedItem.Code;
        myServiceHelper.CheckDateFilerCode = this.DateCheckedSelectedItem == null ? null : this.DateCheckedSelectedItem.Code;
        myServiceHelper.TextCodeTypeCode = this.TextCodeTypeSelectedItem == null ? null : this.TextCodeTypeSelectedItem.Code;
        myServiceHelper.SelectedCheckDate = this.SelectedCheckDate;
        myServiceHelper.SearchText = this.SearchText;
        myServiceHelper.SkipDigit = this.SkipDigit;
        myServiceHelper.TakeDigit = this.TakeDigit;
        myServiceHelper.IsNewSearching = isNewSearching;

        if (!isNewSearching) {
            myServiceHelper.Count = this.SearchResultCount;
        }

        this.myService.Post(myServiceHelper).subscribe((myResponse: ServiceResponse) => {

            this.ItemsSource.Clear();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                this.OnDataLoaded(myResponse.Result);
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
    SaveChanges(isClosing: boolean = false) {

        this.CurrentSession.StartBusyIndicatorSaving();

        if (this.SkipDigit < 0) {
            this.SkipDigit = 0;
        }

        if (this.SkipDigit > this.SearchResultCount) {
            this.SkipDigit = this.SearchResultCount;
        }

        var myServiceHelper = new DefaultTranslationAPIHelper();
        myServiceHelper.ObjectTableId = this.ObjectTableId;
        myServiceHelper.SpellCheckedFilterCode = this.SpellCheckedSelectedItem == null ? null : this.SpellCheckedSelectedItem.Code;
        myServiceHelper.CheckDateFilerCode = this.DateCheckedSelectedItem == null ? null : this.DateCheckedSelectedItem.Code;
        myServiceHelper.TextCodeTypeCode = this.TextCodeTypeSelectedItem == null ? null : this.TextCodeTypeSelectedItem.Code;
        myServiceHelper.SelectedCheckDate = this.SelectedCheckDate;
        myServiceHelper.SearchText = this.SearchText;
        myServiceHelper.SkipDigit = this.SkipDigit;
        myServiceHelper.TakeDigit = this.TakeDigit;
        myServiceHelper.IsNewSearching = false;
        myServiceHelper.Count = this.SearchResultCount;
        myServiceHelper.UpdatedTextCodes = this.GetDirtyTextCodes();

        if (isClosing) {
            myServiceHelper.IsUpdatingOnly = true;
        }

        this.myService.Post(myServiceHelper).subscribe((myResponse: ServiceResponse) => {

            this.ItemsSource.Clear();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                if (isClosing) {
                    this.CloseWindow();
                }

                else {
                    this.OnDataLoaded(myResponse.Result);
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
    OnDataLoaded(myResultHelper: DefaultTranslationAPIHelper) {
        if (myResultHelper) {

            this.SearchResultCount = myResultHelper.Count;

            var loadedItems: TextCodePM[] = myResultHelper.TextCodes;

            var itemsCollection: DefaultTranslationItem[] = [];

            loadedItems.forEach(item => {
                itemsCollection.push(new DefaultTranslationItem(item, this));
            });

            this.ItemsSource.InsertCollection(itemsCollection);
        }


        var myPageItemsCount = 0;

        if (this.SearchResultCount) {
            if (this.SearchResultCount > (this.SkipDigit + this.TakeDigit)) {
                myPageItemsCount = this.SkipDigit + this.TakeDigit;
            }

            else {
                myPageItemsCount = this.SearchResultCount;
            }
        }

        this.PageItemsCount = myPageItemsCount;
        this.SetButons();
        this.HasChanges = false;
    }

    public PreviousButtonIsEnabled: boolean = false;
    public NextButtonIsEnabled: boolean = false;
    SetButons() {
        this.PreviousButtonIsEnabled = this.SkipDigit > 0 ? true : false;
        this.NextButtonIsEnabled = (this.SkipDigit + this.TakeDigit) < this.SearchResultCount ? true : false;
    }

    SaveAndPreviousClicked() {
        if (this.HasChanges) {
            var isDataValid = this.ValidateData();
            if (isDataValid) {
                this.SkipDigit = this.SkipDigit - this.TakeDigit;
                this.SaveChanges();
            }
        }

        else {
            this.SkipDigit = this.SkipDigit - this.TakeDigit;
            this.LoadTextCodes();
        }
    }
    SaveAndNextClicked() {
        if (this.HasChanges) {
            var isDataValid = this.ValidateData();
            if (isDataValid) {
                this.SkipDigit = this.SkipDigit + this.TakeDigit;
                this.SaveChanges();
            }
        }

        else {
            this.SkipDigit = this.SkipDigit + this.TakeDigit;
            this.LoadTextCodes();
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

                    var isDataValid = this.ValidateData();

                    if (isDataValid) {
                        this.SaveChanges(true);
                    }
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
    GetDirtyTextCodes() {
        var myResult: TextCodePM[] = [];

        this.ItemsSource.Collection.forEach((item: DefaultTranslationItem) => {
            if (item.EntityPM.IsDirty) {
                myResult.push(item.EntityPM);
            }
        });

        return myResult;
    }
    ValidateData() {
        var myResult = true;

        var errors: string[] = [];

        var list: TextCodePM[] = this.GetDirtyTextCodes();
        list.forEach(item => {
            Validator.TryValidateObject(item, "TextCode", errors);
        });

        this.ValidationErrorsList = errors;

        if (errors.length > 0) {
            myResult = false;
        }

        return myResult;
    }
    UpdateHasChanges() {
        var list: TextCodePM[] = this.GetDirtyTextCodes();

        if (list.length == 0) {
            this.HasChanges = false;
        }

        else {
            this.HasChanges = true;
        }
    }
}
export class DefaultTranslationItem extends BaseComponent {
    public EntityPM: TextCodePM = null;
    public ObjectTableName: string = "TextCode";

    constructor(item: TextCodePM, private fatherComponent: DefaultTranslationComponent) {
        super();
        this.EntityPM = item;
        this.CloneData();
    }

    private DefaultText_Cloned: string;
    private DefaultTextPlural_Cloned: string;
    private LocalDefaultText_Cloned: string;
    private IsSpellChecked_Cloned: boolean;
    private SpellCheckDate_Cloned: Date;
    private SpellCheckedByUserId_Cloned: string;
    private SpellCheckedByUserName_Cloned: string;
    CloneData() {
        this.DefaultText_Cloned = this.EntityPM.DefaultText;
        this.DefaultTextPlural_Cloned = this.EntityPM.DefaultTextPlural;
        this.LocalDefaultText_Cloned = this.EntityPM.LocalDefaultText;
        this.IsSpellChecked_Cloned = this.EntityPM.IsSpellChecked;
        this.SpellCheckDate_Cloned = this.EntityPM.SpellCheckDate;
        this.SpellCheckedByUserId_Cloned = this.EntityPM.SpellCheckedByUserId;
        this.SpellCheckedByUserName_Cloned = this.EntityPM.SpellCheckedByUserName;
    }
    OnDataChanged() {
        var isDirty = false;

        if (this.DefaultText != this.DefaultText_Cloned) {
            isDirty = true;
        }

        else if (this.DefaultTextPlural != this.DefaultTextPlural_Cloned) {
            isDirty = true;
        }

        else if (this.LocalDefaultText != this.LocalDefaultText_Cloned) {
            isDirty = true;
        }

        else if (this.IsSpellChecked != this.IsSpellChecked_Cloned) {
            isDirty = true;
        }

        if (isDirty == false) {
            if (this.SpellCheckDate != this.SpellCheckDate_Cloned) {
                this.SpellCheckDate = this.SpellCheckDate_Cloned;
            }

            if (this.SpellCheckedByUserId != this.SpellCheckedByUserId_Cloned) {
                this.SpellCheckedByUserId = this.SpellCheckedByUserId_Cloned;
            }

            if (this.SpellCheckedByUserName != this.SpellCheckedByUserName_Cloned) {
                this.SpellCheckedByUserName = this.SpellCheckedByUserName_Cloned;
            }
        }

        if (this.EntityPM.IsDirty != isDirty) {
            this.EntityPM.IsDirty = isDirty;
        }

        this.fatherComponent.UpdateHasChanges();
    }

    public get Code() { return this.EntityPM.Code; }

    public get DefaultText() { return this.EntityPM.DefaultText; }
    public set DefaultText(value: string) {
        if (this.EntityPM.DefaultText != value) {
            this.EntityPM.DefaultText = value;
            this.OnDataChanged();
        }
    }

    public get DefaultTextPlural() { return this.EntityPM.DefaultTextPlural; }
    public set DefaultTextPlural(value: string) {
        if (this.EntityPM.DefaultTextPlural != value) {
            this.EntityPM.DefaultTextPlural = value;
            this.OnDataChanged();
        }
    }

    public get LocalDefaultText() { return this.EntityPM.LocalDefaultText; }
    public set LocalDefaultText(value: string) {
        if (this.EntityPM.LocalDefaultText != value) {
            this.EntityPM.LocalDefaultText = value;
            this.OnDataChanged();
        }
    }

    public get IsSpellChecked() { return this.EntityPM.IsSpellChecked; }
    public set IsSpellChecked(value: boolean) {
        if (this.EntityPM.IsSpellChecked != value) {
            this.EntityPM.IsSpellChecked = value;

            if (value) {
                this.SpellCheckDate = DateTool.GetCurrentDateAsUtc();
                this.SpellCheckedByUserId = SessionLocator.LoggedUserId;
                this.SpellCheckedByUserName = SessionLocator.LoggedUserPM.EnglishName;
            }

            else {
                this.SpellCheckDate = null;
                this.SpellCheckedByUserId = null;
                this.SpellCheckedByUserName = null;
            }

            this.OnDataChanged();
        }
    }

    public get SpellCheckDate() { return this.EntityPM.SpellCheckDate; }
    public set SpellCheckDate(value: Date) {
        if (this.EntityPM.SpellCheckDate != value) {
            this.EntityPM.SpellCheckDate = value;
        }
    }

    public get SpellCheckedByUserId() { return this.EntityPM.SpellCheckedByUserId; }
    public set SpellCheckedByUserId(value: string) {
        if (this.EntityPM.SpellCheckedByUserId != value) {
            this.EntityPM.SpellCheckedByUserId = value;
        }
    }

    public get SpellCheckedByUserName() { return this.EntityPM.SpellCheckedByUserName; }
    public set SpellCheckedByUserName(value: string) {
        if (this.EntityPM.SpellCheckedByUserName != value) {
            this.EntityPM.SpellCheckedByUserName = value;
        }
    }
}
