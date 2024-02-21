import { LogitudeWindow } from './../../../Controls/Windows/LogitudeWindow';
declare var window: any;
declare var SelectingElement: any;
import { Directive, ElementRef, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit, OnDestroy, NgZone, ChangeDetectorRef, ApplicationRef, ViewChild } from '@angular/core';
import { BaseComponent } from './BaseComponent';
import { UIProperty, UIProperties, UIPropertyArgs } from './UIProperties';
import { ObjectFieldPM } from '../../EntityPMs/ObjectFieldPM';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { AppTool } from '../../Tools';
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';
import { ControlsIdCounter } from '../../Utilities/ControlsIdCounter';
import { FieldValidator } from '../../Validators/FieldValidator';
import { FormGroup } from '@angular/forms';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';
import { ObjectsLocator } from '../../Locators/ObjectsLocator';
import { fromEvent, timer } from 'rxjs';
import { debounceTime, take } from 'rxjs/operators';
import { isNullOrUndefined } from 'util';
declare var keyBoardWhich, keyBoardKey, selectionStart, numberWithSeparators, numberWithCommas: any;

interface BeforeOnDestroy {
    ngxBeforeOnDestroy();
}

type NgxInstance = BeforeOnDestroy & Object;
type Descriptor = TypedPropertyDescriptor<Function>;
type Key = string | symbol;

export function BeforeOnDestroy(target: NgxInstance, key: Key, descriptor: Descriptor) {
    return {
        value: async function (...args: any[]) {
            await target.ngxBeforeOnDestroy();
            return descriptor.value.apply(target, args);
        }
    }
}

@Component({
    
    selector: 'LogTextBox',
    templateUrl: "./LogTextBoxComponent.html",
    inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext',
        "IsMultiline", "InputType", "HideColumns", "HideLastColumn",
        "DigitsAfterPoint", "FocusOnMe", "IsFreeText", "IsAccumulative",
        "AllowPercentage", "UseArialFont", "DontAllowAutoSelect", 'IsRatioBox', 'EnableKeyDown'],
})

export class LogTextBoxComponent implements BeforeOnDestroy, OnInit, AfterViewInit, OnDestroy {
    public AllowPercentage: boolean;
    public IsAccumulative: boolean;
    public ShowHelp: boolean = false;
    public UseArialFont: boolean = false;
    public ObjectField: ObjectFieldPM;
    public ObjectFieldName: string = null;
    public ObjectFieldHelp: string = null;
    public ObjectTableName: string = null;
    public InputType: string;
    public DataContext: any;
    public IsMultiline: boolean;
    public HideColumns: boolean = false;
    public HideLastColumn: boolean = false;
    public DigitsAfterPoint: number;
    public IsRatioBox: boolean = false;
    public EnableKeyDown: boolean = false;
    CopyValueSubs: any;
    public textboxHeight: string = '100%';
    public DontAllowAutoSelect: boolean = false;
    // private firstDigit: string = ",";
    // private secondDigit: string = ".";
    private thousandsSeparator: string;
    private decimalSeparator: string;
    isFirstTime: boolean = true;
    IsFreeText: boolean = false;
    FocusOnMe: boolean = false;
    private dataContext: BaseComponent;
    public uiProperty: UIProperty;
    private show: boolean;
    IsDisabled: boolean;
    private timerToken: any;
    private textValue;
    public get TextValue() {
        return this.textValue;
    }
    public set TextValue(newValue: string) {
        if (this.TextValue == newValue) {
            return;
        }

        if ((this.textValue == "" || this.textValue == null || this.textValue == undefined) && newValue) {
            this.HasValue.emit(true);
        }
        else if (this.textValue && (newValue == "" || newValue == null || newValue == undefined)) {
            this.HasValue.emit(false);
        }

        this.textValue = newValue;
        this.textValue = this.FormatTextValueNumbers(newValue);
        if (this.IsPasted) {
            this.IsPasted = false;
            switch (this.InputType && this.InputType.toLowerCase()) {
                case 'text':
                case 'ntext':
                    {
                        break;
                    }
                default: {
                    if (newValue && newValue.indexOf(this.thousandsSeparator) > -1) {
                        newValue = newValue.replace(this.thousandsSeparator, '');
                    }
                }

            }
            this.textValue = newValue;
            this.TextValueChanges(newValue);
        }

    }
    ErrorPopUpId: string;
    InputId: string;
    InputDivStyle: any = {};
    InputDivId: string;
    public styles: any;
    ShowErrorPopup: boolean = false;
    IsPasted: boolean = false;
    LayoutDirection: string = 'ltr';
    showLocal: boolean = false;
    IdentityKey: string;
    public isRTL: boolean = false;
    private isRTLNumberTxt: boolean = false;
    public get IsRTLNumberText() {
        this.isRTLNumberTxt = this.isRTL && (this.InputType.toLowerCase() != "text" && this.InputType.toLowerCase() != "ntext");
        return this.isRTLNumberTxt;
    }

    private CurrentSession = SessionLocator.SelectedSession;
    keydown: boolean;
    isCtrlKeyDown: boolean = false;
    counterId: number;
    _debounceTimeSub: any;
    private Retries: number = 0;
    private timerTokenComponent: any;
    private isShiftKeyDown: boolean = false;
    Detach: boolean;
    isMouseOver: boolean = false;
    isExpanded: boolean = false;
    private StaticPlaceHolder = '';


    @Output() KeyUp = new EventEmitter();
    @Output() ValueChanged = new EventEmitter();
    @Output() LostFocus = new EventEmitter();
    @Output() InputIdGenerated = new EventEmitter();
    @Output() Change = new EventEmitter();
    @Output() HasValue = new EventEmitter();
    @Input() DontUseTimer: boolean;
    @Input() DisableZeroPadding: boolean;
    @Input() AllowSpellCheck: boolean;
    @Input() AllowLocalLanguage: boolean;
    @Input() IsNewEntityCall: boolean;
    @Input() NoObjectField: boolean = false;
    @Input() NoValidation: boolean = false;
    @Input() Placeholder: string = "";
    @Input() AlignTextToRight: boolean = false;
    @Input() AddCommasToNumbers: boolean = true;
    @Input() Max: number;
    @Input() Min: number;
    @Input() RowsCount: number;
    private _ForceDirection: string;
    @Input() public get ForceDirection(): string { return this._ForceDirection; }
    public set ForceDirection(v: string) {
        this._ForceDirection = v;
        this.isRTL = this.ForceDirection == "rtl";
    }
    private _ForceDisable: boolean;
    @Input() public get ForceDisable(): boolean { return this._ForceDisable; }
    public set ForceDisable(v: boolean) {
        this._ForceDisable = v;
        if (v == true)
            this.SetDisabled();
        else
            this.SetEnabled();
    }
    private text: any;
    @Input() public get Text() {
        return this.text;
    }
    public set Text(newValue: any) {
        if (this.text != newValue) {
            this.text = newValue;
            if (!this.keydown) {
                if (!this.uiProperty) {
                    this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
                }
                this.DataContextValueChanges(newValue);
                this.ValidateField();
                //this.DetectChanges();



                if (this.uiProperty != null) {
                    this.uiProperty.UIPropertyChanged.emit("valuechanges");
                }

            }
        }
    }
    @Input() LogitudeForm: FormGroup;
    @Output() OriginalText = new EventEmitter();
    @Input() DebounceTime: number;


    constructor(private ngzone: NgZone, private cd: ChangeDetectorRef,
        private appref: ApplicationRef) {
        this.show = false;
        this.IdentityKey = AppTool.GetNewGuid();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.SetNumberFormattingSeparators();
        //this.CurrentSession.isShiftClicked = false;
        //this.CurrentSession.isTabWithShiftClicked = false;

    }

    ngOnInit() {

        this.StaticPlaceHolder = this.Placeholder;
        if (this.IsRatioBox == true) {
            this.DigitsAfterPoint = 3;
            this.InputDivStyle = {};
        }

        var objectFieldAvailable: boolean = true;

        this.counterId = null;
        var baseIdCombination = null;
        if (this.ObjectTableName) {
           
            baseIdCombination = this.ObjectTableName + "_" + this.ObjectFieldName;
        }
        else {
            baseIdCombination = this.ObjectFieldName;
        }
        if (this.CheckIfExists(baseIdCombination)) {
            this.counterId = ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        }

        if (this.counterId != null) {
            baseIdCombination = baseIdCombination + '_' + this.counterId.toString();
        }

        this.SetControlIds(baseIdCombination);

        if (this.FocusOnMe) {// it means it is inside a grid.
            this.CopyValueSubs = this.CurrentSession.CopyCellIntoMemory.subscribe((id) => {
                if (id == this.InputId) {
                    //this.CurrentSession.CopiedCell = this.DataContext[this.ObjectFieldName];
                    this.DataContext[this.ObjectFieldName] = this.CurrentSession.CopiedCell;
                    this.CurrentSession.CopiedCell = null;
                    this.TextValue = this.DataContext[this.ObjectFieldName] != undefined && this.DataContext[this.ObjectFieldName] != null ? this.DataContext[this.ObjectFieldName] + '' : this.DataContext[this.ObjectFieldName];
                    this.GetValueFormatted(this.TextValue);
                }
            });

            if (this.CurrentSession.CopiedCell) {
                //this.DataContext[this.ObjectFieldName] = this.CurrentSession.CopiedCell;
                //this.CurrentSession.CopiedCell = null;
            }
        }

        var table = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
        if (table) {
            this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];

            if (!this.ObjectField) {
                objectFieldAvailable = false;
            }

            else if (this.ObjectField.HelpTextCodeCode != null) {
                this.ObjectFieldHelp = TextCodeTranslator.Translate(this.ObjectField.HelpTextCodeCode);

                if (!AppTool.IsNullOrEmpty(this.ObjectFieldHelp)) {
                    if (this.ObjectFieldHelp.length > 1) {
                        if (!this.IsFreeText) this.ShowHelp = true;
                    }
                }
            }
        }

        else {
            objectFieldAvailable = false;
        }

         this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);

        this.IsDisabled = !this.uiProperty.IsEnabled;

        if (this.IsDisabled || this.ForceDisable) {
            this.SetDisabled();
        }
        else {
            this.SetEnabled();
        }

        this.uiProperty.UIPropertyChanged.subscribe((value) => {
            this.HandleUIPropertyChanged(value);
            //this.DetectChanges();
        });

        // if(this.DataContext.EntityPM){
        //     const pmuiProperty = this.DataContext.EntityPM.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext.EntityPM);
        //     pmuiProperty?.UIPropertyChanged.subscribe((value) => {
        //         this.HandleUIPropertyChanged(value);
        //         //this.DetectChanges();
        //     });
        // }
         
        //if there is an objectfield in the metadata:
        if (objectFieldAvailable) {

            if (!this.DigitsAfterPoint) {
                if (this.ObjectField.DigitsAfterPoint == 0) {
                    this.DigitsAfterPoint = 3;
                }
                else {
                    this.DigitsAfterPoint = this.ObjectField.DigitsAfterPoint;
                }
            }

            if (!this.InputType) {
                this.InputType = this.ObjectField.DataTypeCode.toLowerCase();
            }

            if (this.IsMultiline == undefined) {
                this.IsMultiline = this.ObjectField.MultiLine;
            }

            if (this.ObjectField.UniqueField && !this.IsNewEntityCall) {
                this.IsDisabled = true;
            }
            // this.ValidateField();

        }
        //using it without an objectfield.
        else {
            if (this.DigitsAfterPoint == undefined) {
                this.DigitsAfterPoint = 3;
            }
            if (!this.InputType) {
                this.InputType = 'ntext';
            }
        }

        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " TEXTBOX has no object field!");
        }

        if (!this.IsFreeText) {


            if (this.ObjectField && this.ObjectField.IsCustom) {
                var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    var customRes = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value);//customFieldClass.Value;
                    this.TextValue = (customRes != null && customRes != undefined) ? customRes + '' : customRes;
                }
                else {
                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                }
            }
            else {
                this.TextValue = this.DataContext[this.ObjectFieldName] != undefined && this.DataContext[this.ObjectFieldName] != null ? this.DataContext[this.ObjectFieldName] + '' : this.DataContext[this.ObjectFieldName];
            }
            //this.TextValue = this.FormatTextValueNumbers();
            this.GetValueFormatted(this.TextValue);
        }



    }

    private HandleUIPropertyChanged(value: any) {
        if (value instanceof UIPropertyArgs) {
            var uiPropertyArgs: UIPropertyArgs = value as UIPropertyArgs;
            var uiProperty: UIProperty = uiPropertyArgs.uiProperty as UIProperty;

            if (uiProperty.FieldName == this.ObjectFieldName && uiProperty.ObjectTableName == this.ObjectTableName) {
                if (uiPropertyArgs.property == "IsEnabled") {
                    var isEnabled = uiPropertyArgs.newValue;
                    this.IsDisabled = !isEnabled;
                    this.uiProperty.IsEnabled = isEnabled;
                    if (this.IsDisabled) {
                        this.SetDisabled();
                    }
                    else {
                        this.SetEnabled();
                    }
                }
                else if (uiPropertyArgs.property == "IsRequired" || uiPropertyArgs.property == "IsValid") {
                    if (!this.isFirstTime) {
                        this.ValidateField(false);
                    }
                    else {
                        this.isFirstTime = false;
                    }
                }
            }
        }
    }

    RunComponent() {
        var input = document.getElementById(this.InputId);

        if (input) {
            this.AfterViewInited();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private RunComponentTimer() {
        this.Retries++;

        if (this.timerTokenComponent) {
            clearTimeout(this.timerTokenComponent);
        }

        if (this.Retries < 20) {
            this.timerTokenComponent = setTimeout(() => this.RunComponent(), 1);
        }
    }

    ngAfterViewInit() {
        this.RunComponent();
    }

    AfterViewInited() {
        if (!this.DebounceTime) {
            this.DebounceTime = 100;
        }
        this.ngzone.runOutsideAngular(() => {
            var input = document.getElementById(this.InputId);
          this._debounceTimeSub =
            fromEvent(input, 'keydown').pipe(
                    debounceTime(this.DebounceTime))
                    .subscribe(keyboardEvent => {
                        var which = keyBoardWhich(keyboardEvent);
                        var key = keyBoardKey(keyboardEvent);
                        if (which == 53) {
                            console.log("");
                        }
                        var result = this.CheckKey(which, key);
                        if (result != null || which == 13 || key == '-') {
                            var applyTextValue = true;
                            this.ngzone.run(() => {
                                if ((which == 9 || which == 13) && (this.AllowPercentage || SessionLocator.TenantPM.NumberFormatCode == "DC")) {
                                    applyTextValue = false;
                                }
                                if (applyTextValue) {
                                    this.TextValueChanges(this.TextValue);
                                }
                                if (which == 13) {
                                    //if (this.FocusOnMe) {
                                    //    var element = document.getElementById(this.InputId);
                                    //    if (element) {
                                    //        element.focus();
                                    //    }
                                    //    this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id, IsEnterCLicked: true });
                                    //}
                                }
                            });
                            //this.TextValueChanges(this.TextValue);
                            var isDestroyed: boolean = this.cd["destroyed"];
                            if (this.cd && isDestroyed == false) {
                                this.cd.detectChanges();
                            }
                        }
                        else {
                            return;
                        }
                    });
        });

        if (this.IsDisabled || this.ForceDisable) {
            this.SetDisabled();
        }
        else {
            this.SetEnabled();
        }
        if (this.FocusOnMe) {

            var element = document.getElementById(this.InputId);
            element.focus();

            this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id, IdentityKey: this.IdentityKey });
            this.timerToken = setTimeout(() => {
                if (typeof (SelectingElement) === "undefined") {
                } else {
                    SelectingElement(element);
                }
            }, 1);

        }

        this.setRowsCount();

    }

    public ngxBeforeOnDestroy() {
        //console.log('1. BEFORE ONDESTROY INVOKE METHOD (await 2 sec)');
        return new Promise((resolve) => {
            setTimeout(() => this.WaitFunction(resolve), 100);
        });
    }


    //@BeforeOnDestroy
    async ngOnDestroy() {
        await this.ngxBeforeOnDestroy();
        console.log("LogTextBox:ngOnDestroy");
        this.cd = null;
        if (this._debounceTimeSub) {
            this._debounceTimeSub.unsubscribe();
        }
        if (this.CopyValueSubs) {
            this.CopyValueSubs.unsubscribe();
            this.CopyValueSubs = null;
        }
    }

    private WaitFunction(resolve) {
        //console.log('2. EXECUTE HEAVY FUNCTION (3 sec)');

        const sourcef = timer(100)
            .pipe(take(1))
            .subscribe(() => {
                resolve();
            });

    }

    CheckIfExists(IdCom: string) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    }

    SetControlIds(baseIdCombination: string) {
        this.ErrorPopUpId = 'textboxererrorpop_' + baseIdCombination;
        this.InputId = baseIdCombination;
        this.InputDivId = "textboxdiv_" + baseIdCombination;
        this.InputIdGenerated.emit(this.InputId);
    }
    OnPaste($event) {
        this.IsPasted = true;
        //console.log("paste paste: " + $event.clipboardData.getData('Text'));
        //this.TextValue = $event.clipboardData.getData('Text');
        //this.TextValueChanges(this.TextValue);
    }

    private SetNumberFormattingSeparators() {
        switch (SessionLocator.TenantPM.NumberFormatCode) {
            case "CD": {
                this.thousandsSeparator = ",";
                this.decimalSeparator = ".";
                break;
            }

            case "DC": {
                this.thousandsSeparator = ".";
                this.decimalSeparator = ",";
                break;
            }

            case "AD": {
                this.thousandsSeparator = "'";
                this.decimalSeparator = ".";
                break;
            }

            default:
                {
                    this.thousandsSeparator = ",";
                    this.decimalSeparator = ".";
                    break;
                }
        }
    }

    onFocus() {
        this.Detach = false;
        //this.DetectChanges();
        this.show = true;
        this.Placeholder = '';
        if (this.uiProperty.ValidValue) {
            this.timerToken = setTimeout(() => {
                if (this.IsRatioBox) {
                    this.InputDivStyle = null;
                }

                else {
                    this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
                }

                this.ShowErrorPopup = false;
            }, 300);

        }
        else {
            this.timerToken = setTimeout(() => {
                this.InputDivStyle = { 'border': '1px solid #ff0000' };
                this.ShowErrorPopup = true;
            }, 300);

        }
        if (!this.DontAllowAutoSelect) {
            var input = document.getElementById(this.InputId);

            if (typeof (SelectingElement) === "undefined") {
            } else {
                SelectingElement(input);
            }
        }
    }

    onBlur() {
        this.Placeholder = this.StaticPlaceHolder;
        this.timerToken = setTimeout(() => {
            this.ShowErrorPopup = false;
            if (this.uiProperty.ValidValue) {
                this.InputDivStyle = null;
            }
        }, 300);
        this.Detach = true;
        //this.DetectChanges();
        this.show = false;

        // this.TextValue = this.DataContext[this.ObjectFieldName];
        this.keydown = false;

        this.HandleMinusOnlyValue();

        this.GetValueFormatted(this.TextValue);
        this.LostFocus.emit(this.TextValue);
    }

    HandleMinusOnlyValue() {
        if (this.TextValue + "" == '-' && this.InputType.toLowerCase() == 'sigdouble') {
            this.TextValue = "0";
        }
    }

    OnKeyUp(event) {
        var SHIFT = 16;
        var CTRL = 17;
        var key = event.keyCode;
        this.ClearCtrlAndShiftKeys(key);
        this.KeyUp.emit(event);
    }
    ClearCtrlAndShiftKeys(key: number) {
        var SHIFT = 16;
        var CTRL = 17;
        if (key == SHIFT) {
            this.isShiftKeyDown = false;
        }

        if (key == CTRL) {
            this.isCtrlKeyDown = false;
        }
    }

    OnKeyDown(event) {

        var SHIFT = 16;
        var CTRL = 17;
        var TAB = 9;
        this.keydown = true;

        var key = event.keyCode;
        var keyChar = event.key;

        var result = this.CheckKey(key, keyChar);

        if (key == CTRL) {
            this.isCtrlKeyDown = true;
        }

        if (result != null) {
            return key;
        }
        else {
            return false;
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
        var keyboardAndNumpadNumbers: number[] = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];
        var doubleUnsDecimalKeys: number[] = keyboardAndNumpadNumbers.concat([BACKSPACE, PERIOD, DECIMALPT, TAB, DELETE, END, HOME, SHIFT, PAGEUP, PAGEDOWN, LEFT, UP, RIGHT, DOWN, ADD, EQUAL]);
        var decimalSigdoubleKeys: number[] = keyboardAndNumpadNumbers.concat([BACKSPACE, PERIOD, DECIMALPT, TAB, DELETE, END, HOME, SHIFT, PAGEUP, PAGEDOWN, LEFT, UP, RIGHT, DOWN, SUBTRACT, DASH, ADD, EQUAL, 173]);
        var unsintegerKeys: number[] = keyboardAndNumpadNumbers.concat([BACKSPACE, TAB, DELETE, END, HOME, SHIFT, PAGEUP, PAGEDOWN, LEFT, UP, RIGHT, DOWN, ADD, EQUAL]);
        var integerKeys: number[] = keyboardAndNumpadNumbers.concat([BACKSPACE, TAB, DELETE, END, HOME, SHIFT, PAGEUP, PAGEDOWN, LEFT, UP, RIGHT, DOWN, SUBTRACT, DASH, ADD, EQUAL, 173]);
        if (key == TAB) {
            this.keydown = false;
        }
        if (key == SHIFT) {
            this.keydown = false;
            this.isShiftKeyDown = true;// this is used to check some keys
        }
        if (key == CTRL) {
            this.isCtrlKeyDown = true;
        }
        if (key == 67 || key == 65 || key == 86 || key == 88) {// ctrl+a,v,a,x
            if (this.isCtrlKeyDown) {
                return key;
            }
        }

        if (this.InputType) {
            var numChars = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];

            switch (this.InputType.toLowerCase()) {
                case 'double':
                case 'unsdecimal':
                    {
                        if (doubleUnsDecimalKeys.indexOf(key) > -1 || keyChar == this.decimalSeparator) {


                            if (key == 53) {
                                if (keyChar == "%") {
                                    if (this.AllowPercentage && !AppTool.IsNullOrEmpty(this.TextValue)) {
                                        return key;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            }

                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            if (key == ADD) {
                                if (this.IsAccumulative) {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            if (key == EQUAL) {
                                if (this.IsAccumulative && keyChar == "+") {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }

                            if (keyChar == this.decimalSeparator) {
                                return key;
                            }
                            if (keyChar == this.thousandsSeparator) {
                                return null;
                            }
                            return key;
                        }
                        return null;
                    }
                case 'unsinteger':
                    {
                        if (unsintegerKeys.indexOf(key) > -1) {

                            if (key == 53) {
                                if (keyChar == "%") {
                                    if (this.AllowPercentage && !AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.indexOf("%") < 0) {
                                        return key;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            }

                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            if (key == ADD) {
                                if (this.IsAccumulative) {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            if (key == EQUAL) {
                                if (this.IsAccumulative && keyChar == "+") {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            return key;
                        }
                        return null;
                    }
                case 'decimal':
                case 'sigdouble':
                    {
                        if (decimalSigdoubleKeys.indexOf(key) > -1 || keyChar == this.decimalSeparator) {

                            if (key == SUBTRACT || key == DASH || key == 173) {

                                if (!AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.toString().indexOf('-') > -1) {
                                    var selection = window.getSelection().toString();
                                    if (selection == this.TextValue) {
                                        return key;
                                    }
                                    return null;
                                }
                                else {
                                    var input = document.getElementById(this.InputId);
                                    if (input != null) {
                                        if (selectionStart(input) == 0 && !this.isShiftKeyDown) {
                                            return key;
                                        }
                                        else {
                                            return null;
                                        }
                                    }
                                }
                            }

                            if (key == ADD) {
                                if (this.IsAccumulative) {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            if (key == EQUAL) {
                                if (this.IsAccumulative && keyChar == "+") {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }

                            if (key == 53) {
                                if (keyChar == "%") {
                                    if (this.AllowPercentage && !AppTool.IsNullOrEmpty(this.TextValue)) {
                                        return key;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            }

                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }

                            if (keyChar == this.decimalSeparator) {
                                return key;
                            }
                            if (keyChar == this.thousandsSeparator) {
                                return null;
                            }

                            return key;
                        }
                        return null;
                    }
                case 'integer':
                    {
                        if (integerKeys.indexOf(key) > -1) {
                            if (key == SUBTRACT || key == DASH || key == 173) {

                                if (!AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.toString().indexOf('-') > -1) {
                                    var selection = window.getSelection().toString();
                                    if (selection == this.TextValue) {
                                        return key;
                                    }
                                    return null;
                                }
                                else {
                                    var input = document.getElementById(this.InputId);
                                    if (input != null) {
                                        if (selectionStart(input) == 0 && !this.isShiftKeyDown) {
                                            return key;
                                        }
                                        else {
                                            return null;
                                        }
                                    }
                                }
                            }

                            if (key == 53) {
                                if (keyChar == "%") {
                                    if (this.AllowPercentage && !AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.indexOf("%") < 0) {
                                        return key;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            }

                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            if (key == ADD) {
                                if (this.IsAccumulative) {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }

                            if (key == EQUAL) {
                                if (this.IsAccumulative && keyChar == "+") {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }



                            return key;
                        }
                        return null;
                    }
                case 'text': {
                    if (!this.AllowLocalLanguage) {
                        //key == 173 || key == 61 || key == 59 : - AND = AND ; IN FIREFOX.
                        if (keyChar == "@") {
                            return key;
                        }
                        var engChars = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

                        if (key == SPACEBAR || key == BACKSPACE || key == TAB || key == DELETE || key == END || key == HOME || key == SHIFT || key == PAGEUP
                            || key == PAGEDOWN || key == PERIOD || key == DECIMALPT || key == EQUALSIGN || key == DASH || key == GRAVEACCENT || key == BACKSLASH
                            || key == CLOSEBRACKET || key == OPENBRACKET || key == FORWARDSLASH || key == COMMA || key == ESC || key == ENTER || key == SEMICOLON || key == SINGLEQOUTE
                            || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == ADD || key == 173 || key == 61 || key == 59)
                            return key;
                        if (48 <= key && key <= 57)
                            return key;
                        if (65 <= key && key <= 90) {
                            if (engChars.indexOf(keyChar) > -1) {
                                return key;
                            }
                            return null;
                        }
                        if (96 <= key && key <= 122)
                            return key;
                        return null;
                    }
                    return key;

                }

                case 'ntext': {
                    return key;
                }

                case 'salepriceandmarkup': {
                    var isOk = false;
                    var input = document.getElementById(this.InputId);

                    if (key == BACKSPACE || key == DELETE || key == END || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == TAB) {
                        isOk = true
                    }

                    else {

                        if (!AppTool.IsNullOrEmpty(this.TextValue)) {
                            var selection = window.getSelection().toString();
                            if (selection == this.TextValue) {
                                return key;
                            }
                        }

                        if (keyChar == "+") {
                            if (selectionStart(input) == 0) {
                                if (!AppTool.IsNullOrEmpty(this.TextValue)) {
                                    if (this.TextValue.indexOf("+") == -1) {
                                        isOk = true;
                                    }
                                }

                                else {
                                    isOk = true;
                                }
                            }
                        }

                        if (keyChar == "-") {
                            if (selectionStart(input) == 0) {
                                if (!AppTool.IsNullOrEmpty(this.TextValue)) {
                                    if (this.TextValue.indexOf("-") == -1) {
                                        isOk = true;
                                    }
                                }

                                else {
                                    isOk = true;
                                }
                            }
                        }

                        else {
                            if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == PERIOD || key == DECIMALPT || key == COMMA || key == ADD || key == SUBTRACT || key == DASH || key == 173 || (this.isShiftKeyDown && (key == 187 || key == 53))) {

                                if (numChars.indexOf(keyChar) > -1) {
                                    if (!AppTool.IsNullOrEmpty(this.TextValue)) {
                                        if (this.TextValue.indexOf("%") > -1) {
                                            if (selectionStart(input) != this.TextValue.length) {
                                                isOk = true;
                                            }
                                        }

                                        else if ((this.TextValue.indexOf("+") > -1) || (this.TextValue.indexOf("-") > -1)) {
                                            if (selectionStart(input) != 0) {
                                                isOk = true;
                                            }
                                        }

                                        else {
                                            isOk = true;
                                        }
                                    }

                                    else {
                                        isOk = true;
                                    }
                                }

                                //////////////////////////////////////
                                if (keyChar == this.decimalSeparator) {
                                    if (!AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.indexOf(this.decimalSeparator) == -1) {
                                        if (this.TextValue.indexOf("%") > -1) {
                                            if (selectionStart(input) != this.TextValue.length) {
                                                isOk = true;
                                            }
                                        }

                                        else {
                                            isOk = true;
                                        }
                                    }

                                    else {
                                        isOk = true;
                                    }
                                }

                                if (keyChar == this.decimalSeparator) {
                                    isOk = true;
                                }
                                if (keyChar == this.thousandsSeparator) {
                                    isOk = false;
                                }

                                //////////////////////////////////////
                                if (keyChar == "%") {
                                    if (!AppTool.IsNullOrEmpty(this.TextValue) && selectionStart(input) != 0 && selectionStart(input) == this.TextValue.length && this.TextValue.indexOf("%") == -1) {
                                        if ((this.TextValue.indexOf("+") > -1) || (this.TextValue.indexOf("-") > -1)) {
                                            isOk = true;
                                        }
                                    }
                                }

                                //////////////////////////////////////
                                if (keyChar == "-") {
                                    if (selectionStart(input) == 0) {
                                        if (!AppTool.IsNullOrEmpty(this.TextValue)) {
                                            if (this.TextValue.indexOf("-") == -1 && this.TextValue.indexOf("+") == -1) {
                                                isOk = true;
                                            }
                                        }
                                    }

                                    else {
                                        isOk = true;
                                    }
                                }
                            }
                        }
                    }

                    if (isOk) {
                        return key;
                    }

                    else {
                        return null;
                    }

                }
                case 'integertext':
                    {
                        if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == BACKSPACE || key == TAB || key == DELETE || key == END
                            || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == SUBTRACT || key == DASH) {
                            if (key == SUBTRACT || key == DASH) {

                                if (!AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.toString().indexOf('-') > -1) {
                                    var selection = window.getSelection().toString();
                                    if (selection == this.TextValue) {
                                        return key;
                                    }
                                    return null;
                                }
                                else {
                                    var input = document.getElementById(this.InputId);
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

    onchange(event) {
        this.Change.emit(this.TextValue);
    }

    GetValueFormatted(valueFromField: any) {
        if (this.TextValue) {
            this.OriginalText.emit(this.TextValue);
            if (this.InputType) {
                switch (this.InputType.toLowerCase()) {
                    case 'double':
                    case 'sigdouble':
                    case 'decimal':
                    case 'unsdecimal':
                        {
                            var isSignOk: boolean = true;
                            if (this.InputType == 'unsdecimal' || this.InputType == 'double') {
                                var text = this.TextValue + "";
                                if (text.indexOf('-') > -1) {
                                    isSignOk = false;
                                }
                            }
                            var val: number;
                            if (this.IsAccumulative && (this.TextValue + "").indexOf('+') > -1) {
                                var accString: string[] = this.TextValue.split('+');
                                var accumulativeAmount: number = 0;
                                accString.forEach((accitem) => {
                                    var v = this.GetNumber(accitem);
                                    accumulativeAmount = accumulativeAmount + v;
                                });
                                val = accumulativeAmount;
                            }
                            else if (this.AllowPercentage && (this.TextValue + "").indexOf('%') > -1) {
                                var txt = this.TextValue.replace('%', '');
                                val = this.GetNumber(txt) / 100;
                                //val = val / 100;

                            }
                            else {
                                // if ((this.TextValue + "").indexOf(this.thousandsSeparator) == -1) {
                                //     var txtwithDot=this.ReplaceDecimalSeparatorWithADot(this.TextValue);
                                val = this.GetNumber(this.TextValue);
                                // }
                                if (this.AddCommasToNumbers) {
                                    // if ((this.TextValue + "").indexOf(this.thousandsSeparator) > -1) {
                                    //     //var txtval = this.TextValue.replace(/,/g, "");
                                    //     var txtval = this.TextValue.split(this.thousandsSeparator).join('');//.replace(new RegExp(this.thousandsSeparator, 'g'), '');
                                    val = this.GetNumber(this.TextValue);
                                    // }
                                }

                            }


                            if (isNaN(val) || !isSignOk) {
                                this.SetValidity(false, TextCodeTranslator.Translate("General.O.InvalidInput"));//"Invalid Input");
                            }
                            else {
                                if (!this.DisableZeroPadding) {                                    
                                    if (AppTool.IsNullOrEmpty(this.DigitsAfterPoint)) {
                                        this.DigitsAfterPoint = 3;
                                    }
                                    if (this.AllowPercentage && (this.TextValue + "").indexOf('%') > -1){
                                        this.TextValue = val.toFixed(4);
                                    }
                                    else {
                                       this.TextValue = AppTool.Round(val, this.DigitsAfterPoint).toString();
                                       // this.TextValue = val.toFixed(this.DigitsAfterPoint);
                                       
                                    }
                                    if (this.TextValue.indexOf('.') > -1 && this.decimalSeparator != '.') {
                                        this.TextValue = this.TextValue.split('.').join(this.decimalSeparator);//.replace(new RegExp('.', 'g'), this.decimalSeparator);
                                    }
                                }
                            }

                            //this.FormatNumbers();
                            if (this.AddCommasToNumbers) {
                                var txtNum: number;

                                // if ((this.TextValue + "").indexOf(this.thousandsSeparator) > -1) {
                                //     //var txtval = this.TextValue.replace(/,/g, "");
                                //     var txtval = this.TextValue.split(this.thousandsSeparator).join('');//replace(new RegExp(this.thousandsSeparator, 'g'), '');
                                //     txtNum = this.GetNumber(txtval);
                                // }
                                // else {
                                txtNum = this.GetNumber(this.TextValue);
                                // }
                                if (this.ObjectField?.IsCustom) {
                                    const customField: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                                    if (customField?.ResolvedValue !== txtNum)
                                        this.TextValueChanges(this.TextValue);
                                }
                                else {
                                    if (this.DataContext[this.ObjectFieldName] != txtNum) {
                                        this.TextValueChanges(this.TextValue);
                                    }
                                }
                                //if (this.thousandsSeparator == ",") {
                                var textWithCommas: string = numberWithSeparators(this.TextValue, this.thousandsSeparator);
                                if (textWithCommas.indexOf(this.decimalSeparator) > -1) {
                                    var textWithCommasArr: string[] = textWithCommas.split(this.decimalSeparator);
                                    var beforeDot: string = textWithCommasArr[0];
                                    var afterDot: string = textWithCommasArr[1];
                                    if (afterDot.indexOf(this.thousandsSeparator) > -1) {
                                        afterDot = afterDot.replace(this.thousandsSeparator, "");
                                    }
                                    textWithCommas = beforeDot + this.decimalSeparator + afterDot;
                                }

                                this.TextValue = textWithCommas;
                            }
                            // }

                            break;
                        }
                    case 'unsinteger':
                    case 'integer':
                        {
                            var isSignOk: boolean = true;
                            if (this.InputType == 'unsinteger') {
                                var text = this.TextValue + "";
                                if (text.indexOf('-') > -1) {
                                    isSignOk = false;
                                }
                            }

                            var val: number;
                            val = this.GetNumber(this.TextValue);



                            if (isNaN(val) || !isSignOk) {
                                this.SetValidity(false, TextCodeTranslator.Translate("General.O.InvalidInput"));//"Invalid Input");
                            }
                            else {
                                this.TextValue = val.toFixed(0);
                            }

                            break;
                        }
                    case "integertext": {
                        if (isNaN(this.GetNumber(this.TextValue))) {
                            this.SetValidity(false, TextCodeTranslator.Translate("General.O.InvalidInput"));
                        }

                        break;
                    }
                    default:
                        {
                            break;
                        }

                }

            }
        }

    }

    FormatTextValue(txtValue: any) {
        let valueFromField  = txtValue;
        if (!AppTool.IsNullOrEmpty(valueFromField)) {
            this.OriginalText.emit(valueFromField);
            if (this.InputType) {
                switch (this.InputType.toLowerCase()) {
                    case 'double':
                    case 'sigdouble':
                    case 'decimal':
                    case 'unsdecimal':
                        {
                            var isSignOk: boolean = true;
                            if (this.InputType == 'unsdecimal' || this.InputType == 'double') {
                                var text = valueFromField + "";
                                if (text.indexOf('-') > -1) {
                                    isSignOk = false;
                                }
                            }
                            var val: number;
                            if (this.IsAccumulative && (valueFromField + "").indexOf('+') > -1) {
                                var accString: string[] = valueFromField.split('+');
                                var accumulativeAmount: number = 0;
                                accString.forEach((accitem) => {
                                    var v = this.GetNumber(accitem);
                                    accumulativeAmount = accumulativeAmount + v;
                                });
                                val = accumulativeAmount;
                            }
                            else if (this.AllowPercentage && (valueFromField + "").indexOf('%') > -1) {
                                var txt = valueFromField.replace('%', '');
                                val = this.GetNumber(txt) / 100;
                                //val = val / 100;

                            }
                            else {
                                // if ((this.TextValue + "").indexOf(this.thousandsSeparator) == -1) {
                                //     var txtwithDot=this.ReplaceDecimalSeparatorWithADot(this.TextValue);
                                val = this.GetNumber(valueFromField);
                                // }
                                if (this.AddCommasToNumbers) {
                                    // if ((this.TextValue + "").indexOf(this.thousandsSeparator) > -1) {
                                    //     //var txtval = this.TextValue.replace(/,/g, "");
                                    //     var txtval = this.TextValue.split(this.thousandsSeparator).join('');//.replace(new RegExp(this.thousandsSeparator, 'g'), '');
                                    val = this.GetNumber(valueFromField);
                                    // }
                                }

                            }


                            if (isNaN(val) || !isSignOk) {
                                //this.SetValidity(false, TextCodeTranslator.Translate("General.O.InvalidInput"));//"Invalid Input");
                            }
                            else {
                                if (!this.DisableZeroPadding) {                                    
                                    if (AppTool.IsNullOrEmpty(this.DigitsAfterPoint)) {
                                        this.DigitsAfterPoint = 3;
                                    }

                                    valueFromField = val.toFixed(this.DigitsAfterPoint);
                                    if (valueFromField.indexOf('.') > -1 && this.decimalSeparator != '.') {
                                        valueFromField = valueFromField.split('.').join(this.decimalSeparator);//.replace(new RegExp('.', 'g'), this.decimalSeparator);
                                    }
                                }
                            }

                            //this.FormatNumbers();
                            if (this.AddCommasToNumbers) {
                                var txtNum: number;

                                // if ((this.TextValue + "").indexOf(this.thousandsSeparator) > -1) {
                                //     //var txtval = this.TextValue.replace(/,/g, "");
                                //     var txtval = this.TextValue.split(this.thousandsSeparator).join('');//replace(new RegExp(this.thousandsSeparator, 'g'), '');
                                //     txtNum = this.GetNumber(txtval);
                                // }
                                // else {
                                txtNum = this.GetNumber(valueFromField);
                                // }

                                 
                                //if (this.thousandsSeparator == ",") {
                                var textWithCommas: string = numberWithSeparators(valueFromField, this.thousandsSeparator);
                                if (textWithCommas.indexOf(this.decimalSeparator) > -1) {
                                    var textWithCommasArr: string[] = textWithCommas.split(this.decimalSeparator);
                                    var beforeDot: string = textWithCommasArr[0];
                                    var afterDot: string = textWithCommasArr[1];
                                    if (afterDot.indexOf(this.thousandsSeparator) > -1) {
                                        afterDot = afterDot.replace(this.thousandsSeparator, "");
                                    }
                                    textWithCommas = beforeDot + this.decimalSeparator + afterDot;
                                }

                                valueFromField = textWithCommas;
                            }
                            // }

                            break;
                        }
                    case 'unsinteger':
                    case 'integer':
                        {
                            var isSignOk: boolean = true;
                            if (this.InputType == 'unsinteger') {
                                var text = valueFromField + "";
                                if (text.indexOf('-') > -1) {
                                    isSignOk = false;
                                }
                            }

                            var val: number;
                            val = this.GetNumber(valueFromField);



                            if (isNaN(val) || !isSignOk) {
                                //this.SetValidity(false, TextCodeTranslator.Translate("General.O.InvalidInput"));//"Invalid Input");
                            }
                            else {
                                valueFromField = val.toFixed(0);
                            }

                            break;
                        }
                    case "integertext": {
                         
                        break;
                    }
                    default:
                        {
                            break;
                        }

                }

            }
        }

        return valueFromField;

    }

    TextValueChanges(res: any) {
        let newValue = this.DataContext[this.ObjectFieldName];
        if(!isNullOrUndefined(this.ObjectField) && this.ObjectField.IsCustom)
        {
            const customField:CustomFieldClass = this.DataContext[this.ObjectFieldName];
            if (customField != null && customField != undefined) {
                newValue = customField.GetFieldDataTypeValue(this.ObjectField, customField.Value);
                newValue =  this.FormatTextValue(newValue);
            }
        }

        if (newValue + "" != this.TextValue) {

            if (this.TextValue) {
                switch (this.InputType) {
                    case 'double':
                    case 'sigdouble':
                    case 'decimal':
                    case 'unsdecimal':
                    case 'unsinteger':
                    case 'integer':
                        {
                            var value;
                            if (this.IsAccumulative && (this.TextValue + "").indexOf('+') > -1) {
                                var accString: string[] = this.TextValue.split('+');
                                var accumulativeAmount: number = 0;
                                accString.forEach((accitem) => {
                                    var v = this.GetNumber(accitem);
                                    accumulativeAmount = accumulativeAmount + v;
                                });
                                value = accumulativeAmount;
                            }
                            else if (this.AllowPercentage && (this.TextValue + "").indexOf('%') > -1) {
                                var txt = this.TextValue.replace('%', '');
                                value = this.GetNumber(txt) / 100;
                            }
                            else {

                                value = this.GetNumber(this.TextValue);
                            }


                            if (this.TextValue + "" == '-' && this.InputType.toLowerCase() == 'sigdouble')
                                this.DataContext[this.ObjectFieldName] = 0;

                            if (!isNaN(value)) {
                                var isok: boolean = true;
                                if (this.InputType == 'unsdecimal' || this.InputType == 'unsinteger' || this.InputType == 'double') {
                                    if (this.TextValue.indexOf('-') > -1) {
                                        isok = false;
                                    }
                                }
                                if (isok) {
                                    if (this.ObjectField && this.ObjectField.IsCustom) {
                                        var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                                        if (customFieldClass != null && customFieldClass != undefined) {
                                            customFieldClass.Value = customFieldClass.SetFieldDataType(this.ObjectField, this.TextValue);// this.TextValue;
                                        }
                                        else {
                                            console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                        }

                                        this.DataContext[this.ObjectFieldName] = customFieldClass;
                                    }
                                    else {

                                        this.DataContext[this.ObjectFieldName] = value;
                                    }
                                }
                            }

                            break;
                        }
                    case "integertext": {

                        if (!isNaN(this.GetNumber(this.TextValue))) {
                            if (this.ObjectField && this.ObjectField.IsCustom) {
                                var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    customFieldClass.Value = this.TextValue;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                }

                                this.DataContext[this.ObjectFieldName] = customFieldClass;
                            }
                            else {
                                this.DataContext[this.ObjectFieldName] = (this.TextValue);
                            }
                        }


                        break;
                    }
                    default:
                        {
                            if (this.ObjectField && this.ObjectField.IsCustom) {
                                var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    customFieldClass.Value = customFieldClass.SetFieldDataType(this.ObjectField, this.TextValue);// this.TextValue;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                }

                                this.DataContext[this.ObjectFieldName] = customFieldClass;
                            }
                            else {
                                this.DataContext[this.ObjectFieldName] = (this.TextValue);
                            }
                            break;
                        }
                }


                // validate min max value
                if (this.Max) {
                    if (this.GetNumber(this.TextValue) > this.Max)
                        this.TextValue = this.Max + "";
                }
                if (this.Min) {
                    if (this.GetNumber(this.TextValue) < this.Min)
                        this.TextValue = this.Min + "";
                }

            }
            else {
                if (this.TextValue == "") {
                    this.TextValue = null;
                }
                if (this.ObjectField && this.ObjectField.IsCustom) {
                    var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null && customFieldClass != undefined) {
                        customFieldClass.Value = customFieldClass.SetFieldDataType(this.ObjectField, this.TextValue);// this.TextValue;
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                    }

                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                }
                else {
                    this.DataContext[this.ObjectFieldName] = (this.TextValue);
                }
            }
            this.ValidateField();
            this.ValueChanged.emit(this.TextValue);
        }
    }

    DataContextValueChanges(res: any) {
        var dataContextValue = this.DataContext[this.ObjectFieldName];

        if (!this.ObjectField) {
            var table = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
            if (table) {
                this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
            }
        }

        if (this.ObjectField && this.ObjectField.IsCustom) {
            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                dataContextValue = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }

        }

        if (dataContextValue != this.TextValue || this.IsFreeText) {
            if (this.IsFreeText) this.TextValue = res;
            else {
                if (this.ObjectField && this.ObjectField.IsCustom) {
                    var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null && customFieldClass != undefined) {
                        var customRes = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value);//customFieldClass.Value;
                        this.TextValue = (customRes != null && customRes != undefined) ? customRes + '' : customRes;
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                    }

                }
                else {
                    this.TextValue = this.DataContext[this.ObjectFieldName] != undefined && this.DataContext[this.ObjectFieldName] != null ? this.DataContext[this.ObjectFieldName] + '' : this.DataContext[this.ObjectFieldName];
                }
            }

            this.GetValueFormatted(this.TextValue);
        }
    }

    SetValidity(validValue: boolean, errorMessage) {

        if (this.uiProperty != null) {
            this.uiProperty.ValidValue = validValue;
            this.uiProperty.ValidationError = errorMessage;
        }
        if (!validValue) {
            this.InputDivStyle = { 'border': '1px solid #ff0000' };
            if (this.show) {
                this.ShowErrorPopup = true;
            }
        }

        else {
            this.ShowErrorPopup = false;
            if (this.show) {
                if (this.IsRatioBox) {
                    this.InputDivStyle = null;
                }

                else {
                    this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
                }
            }
            else {
                this.InputDivStyle = null;
            }
        }


    }

    SetDisabled() {
        var inputDiv = document.getElementById(this.InputDivId);//("DatePickerInputDiv");
        if (inputDiv && !this.IsMultiline) {
            inputDiv.classList.add("InputDivDisabled");
        }
        if (this.IsMultiline) {
            var input = document.getElementById(this.InputId);
            if (input) {
                input.classList.add("TextAreaDisabled");
                inputDiv.classList.add("InputDivDisabled");

            }
        }
    }

    SetEnabled() {
        var inputDiv = document.getElementById(this.InputDivId);//("DatePickerInputDiv");
        if (inputDiv && !this.IsMultiline) {
            inputDiv.classList.remove("InputDivDisabled");
        }
        if (this.IsMultiline) {
            var input = document.getElementById(this.InputId);
            if (input) {
                input.classList.remove("TextAreaDisabled");
                inputDiv.classList.remove("InputDivDisabled");

            }
        }
    }

    ValidateField(emitPropertyChanged: boolean = true) {
        var suppressValidation: boolean = false;
        if (this.InputType) {
            switch (this.InputType.toLowerCase()) {
                case 'text':
                case 'ntext':
                    {
                        break;
                    }
                default: {
                    var val: number;

                    if (this.TextValue) {
                        val = this.GetNumber(this.TextValue);


                        if (this.InputType.toLowerCase() == 'sigdouble' && ((this.TextValue + "") == '-')) {
                            // skip for minus only
                        } else if (isNaN(val)) {
                            this.SetValidity(false, TextCodeTranslator.Translate("General.O.InvalidInput"));
                            suppressValidation = true;
                        }
                        else {
                            this.SetValidity(true, null);
                            suppressValidation = false;
                        }
                    }
                }

            }
        }


        if (!this.NoValidation && this.uiProperty != null && !suppressValidation) {
            var errors = null;
            var table = window.ObjectTables.filter(d => d.Name === this.uiProperty.ObjectTableName)[0];
            if (table) {
                var field: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.uiProperty.FieldName)[0];
                var fieldValidator: FieldValidator = new FieldValidator();
                errors = fieldValidator.Validate(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            }
            if (this.uiProperty.IsValidManually == false) {
                this.SetValidity(false, this.uiProperty.ManualValidationError);
            }
            else if (errors) {
                if (errors.length > 0) {
                    this.SetValidity(false, errors[0]);
                }
                //else if (this.uiProperty.IsValidManually === false) {
                //    this.SetValidity(false, this.uiProperty.ManualValidationError);
                //}
                else {
                    this.SetValidity(true, null);
                }

            }
            //else if (this.uiProperty.IsValidManually == false) {
            //    this.SetValidity(false, this.uiProperty.ManualValidationError);
            //}
            else {
                this.SetValidity(true, null);
            }
            if (emitPropertyChanged) {
                this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
            }
        }
    }


    OnMouseOver() {
        this.isMouseOver = true;
        if (this.IsDisabled) {
            if (this.IsRatioBox) {
                this.InputDivStyle = null;
            }

            else {
                this.InputDivStyle = { 'border': '1px solid #AAAAAA' };
            }
        }
    }
    OnMouseLeave() {
        this.isMouseOver = false;
    }



    get multlineTextBoxLines() {
        var length = 0;
        if (this.textValue) length = this.textValue.split(/\r*\n/).length;
        return length;
    }

    ExpandButtonClicked() {
        console.log("[ExpandButtonClicked]");

        //this.isExpanded = !this.isExpanded;

        this.showMultiLineWindow();






        // if(this.CurrentSession.CurrentWindow){
        //     var wd = document.getElementsByClassName("LogitudeWindow")[0];
        // }

        // var rect = wd.getBoundingClientRect();

        // var left = rect.left;
        // var right = rect.right;
        // var top = rect.top;
        // var bottom = rect.bottom;

        // this.InputDivStyle =
        // {
        //     'z-index': '9999',
        //     'position': 'fixed',
        //     'left': left+'px',
        //     'right': right+'px',
        //     'top': top+'px',
        //     'bottom': bottom+'px',
        //     'width': wd.clientWidth+'px',
        //     'height': wd.clientHeight+'px',

        // };



    }

    showMultiLineWindow() {
        // show window
        var windowArgs: any = {};
        // windowArgs.ObjectTableName = this.ObjectTableName;
        // windowArgs.ObjectFieldName = this.ObjectFieldName;
        windowArgs.TextValue = this.TextValue;
        windowArgs.RowsCount = this.RowsCount;
        windowArgs.IsTextBoxRTL = this.isRTL;
        windowArgs.EnableKeyDown = this.EnableKeyDown;
        var wind = new LogitudeWindow();
        // wind.IsFullScreen = true;
        wind.Width = 960;
        wind.Height = 570;
        wind.WindowArgs = windowArgs;

        if (this.ObjectField)
            wind.Title = this.showLocal ? this.ObjectField.FullNameTextCodeLocalDefaultText : this.ObjectField.FullNameTextCodeDefaultText;
        else
            wind.Title = "";

        wind.Show("./Infrastructure/Component/LogitudeComponents/MultilineTextBoxWindow");
        wind.WindowClosed.subscribe((res:any) => {
            console.log("Rsukt--", res);
            if (res != "<!#cancelled>") {
                this.TextValue = res;
                this.TextValueChanges(this.TextValue);
            }
        });
    }

    setRowsCount() {
        //check rowscount
        // if (this.IsMultiline) { // commented due single line expand window

        setTimeout(() => {
            if (this.RowsCount) {
                //calculate height: (rowcount * 18 row height) + 8 padding
                this.textboxHeight = ((this.RowsCount * 18) + 8) + 'px';
            }
            else {
                var _element = document.getElementById(this.InputId);

                if (_element) {
                    var elHeight = _element.clientHeight;
                    var calculatedRowsCount = (elHeight / 18);
                    var ___roundedCalculatedRowsCountHaha = Math.trunc(calculatedRowsCount);
                    this.RowsCount = ___roundedCalculatedRowsCountHaha;
                    this.textboxHeight = ((this.RowsCount * 18) + 8) + 'px';
                }
            }
        }, 100);
    }

    FormatTextValueNumbers(textValue: string) {//60,5 ---- 60.5   1.111-----1,111
        var formattedTxt = textValue;
        var textnumber = Number(textValue);
        if (this.DataContext[this.ObjectFieldName] == textnumber && SessionLocator.TenantPM.NumberFormatCode == "DC") {

            if (this.InputType != "text" && this.InputType != "ntext") {
                if (!AppTool.IsNullOrEmpty(this.TextValue)) {
                    if (this.TextValue.indexOf('.') > -1) {
                        var textparts = this.TextValue.split('.');
                        formattedTxt = textparts[0] + this.decimalSeparator + textparts[1];
                    }
                    // var text=this.RemoveThousandsSeparator(this.TextValue)
                    // text= this.ReplaceDecimalSeparatorWithADot(text);

                    // var value = Number(this.TextValue);
                    // var textval = value + "";

                    // if (textval.indexOf('.') > -1) {
                    //     var textparts = textval.split('.');
                    //     formattedTxt = textparts[0] + this.decimalSeparator + textparts[1];
                    // }
                }
            }
        }
        return formattedTxt;
    }

    ReplaceDecimalSeparatorWithADot(value: string) {
        if (this.decimalSeparator != '.') {
            value = value.split(this.decimalSeparator).join('.');
        }
        return value;
    }

    RemoveThousandsSeparator(value: string) {
        if ((this.TextValue + "").indexOf(this.thousandsSeparator) > -1) {
            value = value.split(this.thousandsSeparator).join('');
        }
        return value;
    }

    GetNumber(numberText: string) {
        var numberValue = NaN;
        if (!AppTool.IsNullOrEmpty(numberText)) {
            numberText = this.RemoveThousandsSeparator(numberText);
            numberText = this.ReplaceDecimalSeparatorWithADot(numberText);
            numberValue = Number(numberText);
        }
        return numberValue;
    }

    CheckIfPercentageKeyAllowed(key: number, keyChar: string) {
        var allowed = false;
        if (key == 53) {
            if (keyChar == "%") {
                if (this.AllowPercentage && !AppTool.IsNullOrEmpty(this.TextValue)) {
                    allowed = true;
                }
            }
        }
        return allowed;
    }

}

