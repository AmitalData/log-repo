declare var keyBoardWhich, keyBoardKey, selectionStart, numberWithCommas: any;
import {Component, OnInit, Output, Input, EventEmitter} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import { ApiQueryFilters, FilterItem} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ClientListService} from '../../../Customs/Services/StandardLists/ClientListService';
import {ControlsIdCounter} from '../../../Infrastructure/Utilities/ControlsIdCounter';
import {UIProperty, UIProperties, UIPropertyArgs} from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {KeyCode} from '../../../Infrastructure/DataContracts/KeyCode';
import {IdGeneratorPipe} from '../../../Controls/Pipes/IdGeneratorPipe';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { EntityListService } from '../../Services/EntityListService';
@Component({
    selector: "SearchBox",
    
    templateUrl: './SearchBox.html',

    inputs:
    [
        'Watermark',
        'ObjectTableName',
        'UseTimer',
        'IsQuickSearch',
        'DisplayMember',
        'DisplayText',
        'HideIcons',
        'IsDisabled',
        'ShowViewAll',
        'AutoClearSearchText',
        'ItemHeight',
        'DropDownWidth',
        'IsSimilarPartner',
        'PartnerTypeId',
        'Background',
        'HideBox',
        'SearchText',
        'Header',
        'HasFilters',
        'IsImporterDetails',
    ],
})

export class SearchBox implements OnInit {
    public Watermark: string = null;
    public ObjectTableName: string = null;
    public UseTimer: boolean = false;
    public IsControlFocused: boolean = false;
    public IsQuickSearch: boolean = false;
    public IsQuickSearchOpened: boolean = false;
    public IsQuickSearchLoading: boolean = false;
    public IsQuickSearchNoResult: boolean = false;
    public IsSearchIconVisible: boolean = false;
    public IsDeleteIconVisible: boolean = false;
    public AutoClearSearchText: boolean = false;
    public DisplayMember: string = null;
    public HideBox: boolean = false;
    public HideIcons: boolean = false;
    public ShowViewAll: boolean = false;
    public DropDownHeight: number = 75;
    public DropDownWidth: number = 300;
    public ItemHeight: number = 25;
    public Header: string = null;
    public Background: string = "white";
    public IsSimilarPartner: boolean = false;
    public PartnerTypeId: string = null;
    private myService: CommonDomainService = null;
    private myCardListService: CardListService = null;
    private clientListService: ClientListService = null;
    public QuickSearchItems: any[] = [];
    public NewId: string;
    emitText: boolean = true;
    LayoutDirection: string = 'ltr';
    public HasFilters: boolean = false;
    public IsImporterDetails: boolean = false;
    @Output() TextChanged = new EventEmitter();
    @Output() ItemClicked = new EventEmitter();
    @Output() ViewAllClicked = new EventEmitter();
    @Output() LostFocus = new EventEmitter();
    ErrorPopUpId: string;
    SearchBoxDivId: string;
    ShowErrorPopup: boolean = false;
    @Input() ErrorMessage: string;
    @Input() MaxPopupItemsCount: number;
    @Input() InputType: string;
    @Input() QueryFilterItems: ApiQueryFilters;
    public showLocals: boolean = false;


    LookUpTable: ObjectTablePM;
    public isRTL: boolean = false;


    constructor(private entityListService: EntityListService ) {
        this.SetControlDisplay();

        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        if(ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        var SearchBoxDivId_counter = ControlsIdCounter.GetNextControlIdCounter("SearchBoxDivId");
        this.SearchBoxDivId = "SearchBoxDivId" + SearchBoxDivId_counter;

        var ErrorPopUpId_counter = ControlsIdCounter.GetNextControlIdCounter("ErrorPopUpId");
        this.ErrorPopUpId = "ErrorPopUpId" + ErrorPopUpId_counter;
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;

    }

    private displayText: string = null;
    get DisplayText() { return this.displayText; }
    set DisplayText(value: string) {
        if (this.displayText != value) {
            this.displayText = value;
            this.searchText = value;
        }
    }

    private isDisabled: boolean = false;
    public get IsDisabled() { return this.isDisabled; }
    public set IsDisabled(value: boolean) {
        if (this.isDisabled != value) {
            this.isDisabled = value;

            if (value == true) {
                //this.SearchText = null;
                this.IsControlFocused = false;
                this.SetControlDisplay();
            }
        }
    }

    ngOnInit() {
        var pipe: IdGeneratorPipe = new IdGeneratorPipe();
        this.NewId =pipe.transform(this.ObjectTableName + '_Search');
        if (this.Watermark == null) {
            var myWatermark = TextCodeTranslator.Translate("General.O.Search");

            if (!AppTool.IsNullOrEmpty(this.ObjectTableName)) {
                var textCode = this.ObjectTableName + ".F.SearchFields";

                if (this.ObjectTableName == "CardGLAccount" || this.ObjectTableName == "CustomerGLAccount" || this.ObjectTableName == "VendorGLAccount") {
                    textCode = "GLAccount.F.SearchFields";
                }

                var waterMark = TextCodeTranslator.Translate(textCode);
                if (!AppTool.IsNullOrEmpty(waterMark)) {
                    myWatermark = waterMark;
                }                
            }

            this.Watermark = myWatermark;
        }

        if (this.IsQuickSearch) {
            this.UseTimer = true;
            this.myService = new CommonDomainService();
            this.myCardListService = new CardListService();
            this.clientListService = new ClientListService();
        }
    }

    OnFucos() {
        this.IsControlFocused = true;

        if (this.searchText)
            this.emitText = false;
        this.SetControlDisplay();
    }
    OnLostFucos() {
        this.LostFocus.emit(this.SearchText);
        if (this.AutoClearSearchText) {
            this.SearchText = null;
        }

        this.IsControlFocused = false;
        this.SetControlDisplay();
    }
    SetControlDisplay() {
        var isSearchIconVisible = false;
        var isDeleteIconVisible = false;
        var isQuickSearchOpened = false;

        if (AppTool.IsNullOrEmpty(this.SearchText)) {

            if (!this.IsControlFocused) {
                isSearchIconVisible = true;
            }

            isQuickSearchOpened = false;
        }

        else {
            isDeleteIconVisible = true;

            if (this.IsQuickSearch) {
                if (this.HideBox) {
                    isQuickSearchOpened = true;
                }

                else {
                    if (this.IsControlFocused) {
                        isQuickSearchOpened = true;
                    }

                    else {
                        isQuickSearchOpened = false;
                    }
                }
            }
        }

        this.IsSearchIconVisible = isSearchIconVisible;
        this.IsDeleteIconVisible = isDeleteIconVisible;
        this.IsQuickSearchOpened = isQuickSearchOpened;
    }
    DeleteClicked() {
        this.SearchText = null;
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;

            this.emitText = true;
            this.SetControlDisplay();
            this.OnSearchTextChanged();
        }
    }

    private timerToken: any;
    private OnSearchTextChanged() {

        //console.log("# SearchTextChanged", this.SelectedItem);

        if (this.SelectedItem) {
            if (this.SelectedItem[this.DisplayMember] != this.SearchText) {
                this.SelectedItem = null;
                this.ItemClicked.emit(null);
            }
        }

        if (this.IsQuickSearch) {
            this.QuickSearchItems = [];
            this.DropDownHeight = 75;
            this.IsQuickSearchLoading = true;
            this.IsQuickSearchNoResult = false;
        }

        if (this.UseTimer) {
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }

            this.timerToken = setTimeout(() => this.RunSearch(), 400);
        }

        else {
            this.RunSearch();
        }
    }

    private MyCallTime: Date;
    private RunSearch() {

        this.highlightedItemIndex = null;
        this.HighlightedItem = null;

        if (this.emitText == true) {
            this.TextChanged.emit(this.SearchText);
        }

        this.emitText = true;

        if (this.IsQuickSearch) {

            if (this.SearchText == "\"") {
                this.SearchText = null;
            }

            if (this.IsSimilarPartner && !AppTool.IsNullOrEmpty(this.PartnerTypeId)) {
                var filters = new ApiQueryFilters();
                filters.PageIndex = 0;
                filters.PageSize = 10;
                filters.SortBy = "EnglishName";
                filters.SortDirection = "Ascending";

                if (this.ObjectTableName == "Customer") {
                    filters.addAdditionalFilter("PartnerTypeId", "CS,PO", null, null, "InList", false, true, false, "string");
                }

                else {
                    filters.addAdditionalFilter("PartnerTypeId", this.PartnerTypeId, null, null, "Equals", false, false, false, "string");
                }

                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("EnglishName", this.SearchText, null, null, "Contains", false, false, false, "string");
                }

                this.myCardListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                    if (this.MyCallTime == null || myResponse.CallTime > this.MyCallTime) {
                        this.MyCallTime = myResponse.CallTime;

                        this.IsQuickSearchLoading = false;
                        this.QuickSearchItems = [];
                        var itemsCount = 0;

                        if (myResponse != null) {
                            itemsCount = myResponse.Result.length;
                            this.QuickSearchItems = myResponse.Result;
                            this.highlightedItemIndex = 0;
                            this.HighlightedItem = this.QuickSearchItems[0];
                        }

                        this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
                        this.SetDropDownheight(itemsCount);
                    }
                });
            }

            else if (this.HasFilters && this.ObjectTableName == "Client") {
                var filters = new ApiQueryFilters();
                filters.PageIndex = 0;
                filters.PageSize = 10;

                filters.addAdditionalFilter("PassportNumber", "", null, null, "NotEqual", false, false, false, "string");
                filters.addAdditionalFilter("Code", "", null, null, "NotEqual", false, false, false, "string");
                filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");

                this.clientListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {

                    this.IsQuickSearchLoading = false;
                    this.QuickSearchItems = [];
                    var itemsCount = 0;

                    if (myResponse != null) {
                        itemsCount = myResponse.Result.length;
                        this.QuickSearchItems = myResponse.Result;

                        this.highlightedItemIndex = 0;
                        this.HighlightedItem = this.QuickSearchItems[0];
                    }

                    this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
                    this.SetDropDownheight(itemsCount);
                });
            }

            else if (!AppTool.IsNullOrEmpty(this.ObjectTableName) && !AppTool.IsNullOrEmpty(this.SearchText)) {

                   if (this.QueryFilterItems == null) {
                     this.myService.GetQuickSearch(this.ObjectTableName, this.SearchText).subscribe((myResponse: ServiceResponse) => {
                        if (this.MyCallTime == null || myResponse.CallTime > this.MyCallTime) {
                            this.MyCallTime = myResponse.CallTime;

                            this.IsQuickSearchLoading = false;
                            this.QuickSearchItems = [];
                            var itemsCount = 0;

                            if (myResponse.Result != null) {
                                itemsCount = myResponse.Result.length;
                                this.QuickSearchItems = myResponse.Result;

                                this.highlightedItemIndex = 0;
                                this.HighlightedItem = this.QuickSearchItems[0];
                            }

                            this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
                            this.SetDropDownheight(itemsCount);
                        }
                      });
                   }else {

                     this.QueryFilterItems.GetCount = true;
                     this.IsQuickSearchLoading = true;
                     this.IsQuickSearchNoResult = false;

                      if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                        this.QueryFilterItems.Filter10Name = "SearchFields";
                        this.QueryFilterItems.Filter10Value = this.SearchText;
                        this.QueryFilterItems.Filter10Operator = "Contains";
                      }

                        var loadPromise = this.entityListService.getByFilters(this.ObjectTableName, this.QueryFilterItems);
                        loadPromise.then((res: any) => {
                          res.subscribe((myResponse: any) => {
                            if (this.MyCallTime == null || myResponse.CallTime > this.MyCallTime) {
                                this.MyCallTime = myResponse.CallTime;

                                this.IsQuickSearchLoading = false;
                                this.QuickSearchItems = [];
                                var itemsCount = 0;

                                if (myResponse != null) {
                                    itemsCount = myResponse.Result.length;
                                    this.QuickSearchItems = myResponse.Result;

                                    this.highlightedItemIndex = 0;
                                    this.HighlightedItem = this.QuickSearchItems[0];
                                }

                                this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
                                this.SetDropDownheight(itemsCount);
                            }
                        })
                     });

                   }

            } else {
                this.QuickSearchItems = [];
            }

        }
    }

    SetDropDownheight(itemsCount: number) {

        if (this.MaxPopupItemsCount)
            itemsCount = this.MaxPopupItemsCount;

        var myHeight = 75;

        if (itemsCount > 0) {
            var itemsHeight = ((itemsCount * this.ItemHeight) + 2);
            if (itemsHeight > myHeight) {
                myHeight = itemsHeight;
            }
        }

        if (this.Header) {
            myHeight += 25;
        }

        if (this.ShowViewAll) {
            myHeight += 20;
        }

        this.DropDownHeight = myHeight;
    }

    ItemSelected(item: any) {
        this.SearchText = null;

        if (item != null) {
            this.SelectedItem = item;
            if (this.DisplayMember != null) {
                this.emitText = false; // disable emitting textChanged event when setting display member value
                this.SearchText = item[this.DisplayMember];
            }
        }

        this.ItemClicked.emit(item);
    }
    OnViewAllClicked() {
        this.ViewAllClicked.emit(true);
    }


    private selectedItem = null;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value: any) {
        if (this.selectedItem != value) {
            this.selectedItem = value;
        }
    }

    private highlightedItem = null;
    get HighlightedItem() { return this.highlightedItem; }
    set HighlightedItem(value: any) {
        if (this.highlightedItem != value) {
            this.highlightedItem = value;
        }
    }

    selectFirst: boolean = false;
    highlightedItemIndex: number;
    OnSearchIputKeyDown(event) {
        //this.selectFirst = false;
        var TAB = 9; var ENTERKEY = 13; var DOWNKEY = 40; var UPKEY = 38; var ESC = 27; var CTRL = 17; var SHIFT = 16;
        //this.timerToken = setTimeout(()=>{
        //    if ($event.keyCode == TABKEY) {
        //        if (this.IsQuickSearchOpened) {
        //            this.ItemSelected(this.QuickSearchItems[0]);
        //        } else {
        //            this.selectFirst = true;
        //        }
        //    }
        //}, 300);

        var key = event.keyCode;
        var keyChar = event.key;

        switch (key) {
            case KeyCode.DOWN: // ↓
                {
                    // navigate to items
                    if (AppTool.IsNullOrEmpty(this.highlightedItemIndex))
                        this.highlightedItemIndex = 0;
                    else
                        this.highlightedItemIndex = ((this.highlightedItemIndex == this.QuickSearchItems.length - 1) ? 0 : this.highlightedItemIndex + 1); // increment index
                    this.HighlightedItem = this.QuickSearchItems[this.highlightedItemIndex];

                    break;
                }
            case KeyCode.UP: // ↑
                {
                    // navigate to items
                    this.highlightedItemIndex = ((this.highlightedItemIndex == 0) ? this.QuickSearchItems.length - 1 : this.highlightedItemIndex - 1); // decrement index
                    this.HighlightedItem = this.QuickSearchItems[this.highlightedItemIndex];

                    break;
                }
            case KeyCode.ENTER:
                {
                    this.ItemSelected(this.HighlightedItem);

                    this.IsQuickSearchOpened = false;

                    break;
                }
            case KeyCode.TAB:
                {
                    if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                        if (this.HighlightedItem) {

                            if (this.SelectedItem)
                            {
                                if (this.SelectedItem != this.HighlightedItem) this.ItemSelected(this.HighlightedItem);
                            }
                            else
                                this.ItemSelected(this.HighlightedItem);
                            
                        } else {

                        }

                    }
                    break;
                }
            case KeyCode.ESCAPE:
                {
                    //if (this.IsOpened) {
                    //    this.CloseDropDown();
                    //}
                    break;
                }
            case KeyCode.BACK_SPACE:
            case KeyCode.DELETE:
                {
                    this.SelectedItem = null;
                    this.ItemClicked.emit(null);
                    break;
                }
            default:
                {
                // Check key type
                if(this.InputType) {

                    var result = this.CheckKey(key, keyChar);

                    if (key == SHIFT) {
                        //this.isShiftKeyDown = false;
                    }

                    if (key == CTRL) {
                        //this.isCtrlKeyDown = true;
                    }

                    if (result != null) {
                        return key;
                    }
                    else {
                        return false;
                    }
                }
                break;
            }
        }

        

    }

    CheckKey(key: number, keyChar: string) {
        var BACKSPACE = 8;
        var SHIFT = 16;
        var DASH = 189;
        var SUBTRACT = 109;
        var DELETE = 46;
        var HOME = 36;
        var END = 35;
        var PAGEUP = 33;
        var PAGEDOWN = 34;
        var LEFT = 37;
        var UP = 38;
        var RIGHT = 39;
        var DOWN = 40;
        var DECIMALPT = 110;
        var PERIOD = 190;
        var ADD = 107;
        var TAB = 9;
        var SPACEBAR = 32;
        var EQUALSIGN = 187;
        var GRAVEACCENT = 192;
        var BACKSLASH = 220;
        var CLOSEBRACKET = 221;
        var OPENBRACKET = 219;
        var SINGLEQOUTE = 222;
        var ENTER = 13;
        var FORWARDSLASH = 191;
        var COMMA = 188;
        var ESC = 27;
        var SEMICOLON = 186;
        var CTRL = 17;
        var EQUAL = 187;

        //(key >= 48 && key <= 57) ARE THE NUMBERS ON TOP || (key >= 96 && key <= 105) ARE THE NUMBERS ON NUMPAD

        //if (key == TAB) {
        //    this.keydown = false;
        //}
        //if (key == SHIFT) {
        //    this.keydown = false;//this.isShiftKeyDown = true; 
        //}
        //if (key == CTRL) {
        //    this.isCtrlKeyDown = true;
        //}
        //if (key == 67 || key == 65 || key == 86 || key == 88) {// ctrl+a,v,a,x
        //    if (this.isCtrlKeyDown) {
        //        return key;
        //    }
        //}

        if (this.InputType) {
            var numChars = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];

            switch (this.InputType.toLowerCase()) {
                case 'integertext':
                    {
                        if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == BACKSPACE || key == TAB || key == DELETE || key == END
                            || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == SUBTRACT || key == DASH) {
                            if (key == SUBTRACT || key == DASH) {

                                if (!AppTool.IsNullOrEmpty(this.SearchText) && this.SearchText.toString().indexOf('-') > -1) {

                                    return null;
                                }
                                else {
                                    var input = document.getElementById(this.NewId);
                                    if (input != null) {
                                        if (selectionStart(input) == 0) {
                                            return key;
                                        }
                                        else {
                                            return null;
                                        }
                                    }
                                }
                            }
                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            return key;
                        }
                        return null;
                    }
                default:
                    return key;
            }
        }
        else {
            return key;
        }
    }
    
}
