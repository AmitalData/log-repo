declare var window: any;
import {Component, OnInit, EventEmitter, Output} from '@angular/core';
import {FormControl} from '@angular/forms';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../Infrastructure/Tools';
import {ObjectsLocator} from '../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'SearchTextBox',
    template: `<input [attr.data-cy]="DataCy" type="text" [disabled]="IsDisabled" [id]="SearchFieldsId" placeholder="{{PlaceHolder}}" (focus)="ClearPlaceHolder();" (blur)="FillPlaceHolder();" [ngStyle]="textValueStyle" [(ngModel)]="SearchText" style="background: url(Images/Search.png) no-repeat scroll;background-color: white;background-position: right center;font-style: italic;" [ngStyle]="LayoutDirection == 'rtl' ? {'padding-left': '30px'} : {'padding-right': '30px'}" />
               <img *ngIf="SearchText" [className]="LayoutDirection == 'rtl' ? 'DeleteButton LeftCenter' : 'DeleteButton RightCenter'" [ngStyle]="LayoutDirection == 'rtl' ? {'left': '15px'} : {'right': '15px'}" src="Images/RedX.png" (click)="OnDeleteValue()" />
              `,
    inputs: ['ObjectTableName', 'PlaceHolder', 'SearchText', 'IsDisabled', 'DataCy'],
})

export class SearchTextBox implements OnInit {
    //SearchFields//HelpTextDefaultText
    DataCy: string;
    SearchFieldsId: string;
    PlaceHolder: string;
    ObjectTableName: string;
    ObjectTableId: string;
    ObjectField: any;
    private timerToken: any;
    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = !AppTool.IsNullOrEmpty(newValue) ? newValue : null;

            //this.SetControlDisplay();
            if (this.searchText != null) {
                this.ClearPlaceHolder();
            }
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }

            //if (AppTool.IsNullOrEmpty(newValue)) {
            //    this.OnDataLoaded();
            //}

            //else {
            this.timerToken = setTimeout(() => this.LoadData(), 500);
            //}
        }
    }

    LoadData() {
        this.SearchTextChangeEvent.emit(this.SearchText);
        this.TextChanged.emit(this.SearchText);
    }
    @Output() SearchTextChangeEvent = new EventEmitter();
    @Output() TextChanged = new EventEmitter();

    public SearchTextValue: FormControl;
    LayoutDirection: string = 'ltr';
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.SearchFieldsId = "SearchFieldsId_-1_-1";
        }

        else {
            this.SearchFieldsId = "SearchFieldsId_" + this.CurrentSession.GetNewId("SearchFieldsId");
        }
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.SetStyles();
    }

    ngOnInit() {

        if (!this.PlaceHolder) {

            if (this.ObjectTableName == null) {
                this.PlaceHolder = "Search ...";
            }

            
            else {
                var ObjectTable = this.GetObjectTableName(this.ObjectTableName);
                var textCode = (ObjectTable =="DocumentTypeTemplate" ? "DocumentType" : ObjectTable) + ".F.SearchFields";
                var waterMark = TextCodeTranslator.Translate(textCode);
                if (!AppTool.IsNullOrEmpty(waterMark)) {
                    this.PlaceHolder = waterMark;
                }
                else {
                    this.PlaceHolder = TextCodeTranslator.Translate("General.O.Search");
                }

                //var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
                //this.ObjectTableId = ObjectTable.Id;
                //this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTableId && d.FieldName === "SearchFields")[0];
                //if (this.ObjectField) {
                //    if (this.ObjectField.FullNameTextCodeDefaultText) {
                //        this.PlaceHolder = this.ObjectField.FullNameTextCodeDefaultText;
                //    }
                //    else {
                //        this.PlaceHolder = "Search ...";
                //    }
                //}
            }
        }

        //this.SearchTextValue = new FormControl();
        //this.SearchTextValue.valueChanges
        //    .debounceTime(500)
        //    .distinctUntilChanged()
        //    .subscribe((search: string): any => {
        //        this.SearchText = search != "" ? search : null;
        //        this.SearchTextChangeEvent.emit(this.SearchText);
        //        this.TextChanged.emit(this.SearchText);
        //    });
    }

    private isDisabled: boolean = false;
    public get IsDisabled() { return this.isDisabled; }
    public set IsDisabled(value: boolean) {
        if (this.isDisabled != value) {
            this.isDisabled = value;

            //if (value == true) {
            //    this.SearchText = null;
            //    this.IsControlFocused = false;
            //    this.SetControlDisplay();
            //}
        }
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        if (temp) {
            temp.placeholder = "";
            temp.style.background = "rgba(0, 0, 0, 0)";
            temp.style.backgroundColor = "white";
        }
    }

    FillPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = this.PlaceHolder;
        if (!this.SearchText) {
            temp.style.background = "url(Images/Search.png) 6px 2px  no-repeat scroll";
            temp.style.backgroundColor = "white";
            temp.style.backgroundPosition = this.LayoutDirection == "rtl" ? '6px 2px' : "right center";
        }
        //temp.style.backgroundPosition = "right center";

        //temp.style.paddingRight = "30px";
        this.SetStyles();
    }

    OnDeleteValue() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.value = null;
        this.SearchText = null;
        temp.focus();
        this.SearchTextChangeEvent.emit(null);
    }

    GetObjectTableName(theObjectTableName: string) {
        let objectTable = window.ObjectTables.filter(obejctTable => obejctTable.Name == theObjectTableName)[0];
        if (objectTable && objectTable.IsCustom && AppTool.IsNullOrEmpty(objectTable.ParentObjectTableId)){
            return this.GetCustomObjectRelatedTableName(objectTable);
        }
        var cardTables = ["customer", "agent", "shippingagent", "customagent", "vendor", "airline", "trucker", "shippingline", "warehouse"];

        if (cardTables.indexOf(theObjectTableName.toLowerCase()) > -1) {
            return "Card";
        }
        else {
            return theObjectTableName;
        }
    }
    GetCustomObjectRelatedTableName(objectTable: any) {
        if (objectTable.ObjectTableTypeCode == "MD") return "ReferenceCustomObject";
        return "DataCustomObject";
    }

    textValueStyle: any;
    SetStyles() {
        if (this.LayoutDirection == "rtl") {
            this.textValueStyle = {
                'background-image': 'url(Images/Search.png)',
                'background-repeat': 'no-repeat',
                'background-color': 'white',
                'background-attachment': 'scroll',
                'background-position': '6px 2px',
                'padding-left': '30px',
                'font-style': 'italic',
            };
        } else {
            this.textValueStyle = {
                'background-image': 'url(Images/Search.png)',
                'background-repeat': 'no-repeat',
                'background-color': 'white',
                'background-attachment': 'scroll',
                'background-position': 'right center',
                'padding-right': '30px',
                'font-style': 'italic',
            };
        }
    }
}
