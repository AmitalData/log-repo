import { Component, Input, OnInit, ChangeDetectorRef, OnDestroy, Directive, Output, EventEmitter} from '@angular/core';
import {LogColumnComponent} from './LogColumnComponent'
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
declare var ElementProperities, MainMenuProperties: any;
declare var SelectingElement: any;

@Component({
    selector: 'log-cell-template',
    moduleId: module.id,
    templateUrl: './LogCellTemplateComponent.html',
    inputs: ['TabIndex', 'IsEnabled', 'CellColor', 'Alignment', 'IsFilled', 'DisableColors', 'IgnoreMods', 'RIndex','IsEditMode']
})

export class LogCellTemplateComponent implements OnDestroy {
    ColumnComponent: LogColumnComponent;
    IgnoreMods: boolean = false;
    RIndex: number = -1;
    ObsNewElementInsertedSub: any;
    @Output() CellClicked = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(CC: LogColumnComponent, private CD: ChangeDetectorRef) {
        this.ColumnComponent = CC;
        //alert(this.ColumnComponent.LogGridId)
        this.ObsNewElementInsertedSub = this.CurrentSession.ObsNewElementInsertedEvent.subscribe((res) => {
            if (res.Id == this.ColumnComponent.LogGridId) {
                //var ctrl = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + 0 + "_" + (res.length));
                //if (ctrl) {
                //    ctrl.focus();
                //}
                var rowIndex = 0;
                var focusSpecificRow = false;
                if (res.RowIndex) {
                    rowIndex = res.RowIndex;
                    focusSpecificRow = true;
                }
                else {
                    if (this.RIndex == -1 || this.RIndex == null) {
                        rowIndex = res.length - 1;
                    }
                    else {
                        rowIndex = res.length;
                    }
                }
                this.focusFirstEditableElement(0, rowIndex, focusSpecificRow);
                //if (this.RIndex == -1 || this.RIndex == null) {
                //    this.focusFirstEditableElement(0, res.length - 1);
                //}
                //else {
                //    this.focusFirstEditableElement(0, res.length);
                //}
            }
        });
    }
    focusFirstEditableElement(i: number, length: number, focusSpecificRow: boolean = false) {
        var ctrl = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + i + "_" + length);//(length - 1 < 0 ? 0 : length - 1)
        if (ctrl == null) {
            ctrl = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + i + "_" + (length - 1));
        }

        if (focusSpecificRow) {
            ctrl = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + i + "_" + length);
        }

        if (ctrl) {
            if (ctrl.tabIndex == 0) {
                ctrl.focus();
                return;
            }
            else {
                this.focusFirstEditableElement(i + 1, length, focusSpecificRow)
            }
        }
    }
    headertext: string;
    bindingfield: string;
    OuterDivId: string;
    HStyle: {};
    cellwidth: string;
    cellvisibility: string;
    isEditable: boolean;
    isrequired: string;
    Editindex: number;
    RowIndex: number = 0;
    TempRowIndex: number = 0;

    public IsFilled: boolean = false;
    SetCellColors() {
        var myTabIndex = 0;
        var myFontColor = "#282E30";
        var myCellColor = "transparent";

        if (this.IsEnabled == false) {
            myTabIndex = -1;

            if (!this.DisableColors) {
                myFontColor = "#A8AAAD";
                myCellColor = "rgba(230, 231, 232, 0.5)";
            }
        }

        this.TabIndex = myTabIndex;
        this.FontColor = myFontColor;
        this.CellColor = myCellColor;
    }

    private isEnabled: boolean = true;
    get IsEnabled() { return this.isEnabled; }
    set IsEnabled(value: boolean) {
        if (this.isEnabled != value) {
            this.isEnabled = value
            this.SetCellColors();
        }
    }

    private cellColor: string = "transparent";
    get CellColor() { return this.cellColor; }
    set CellColor(value: string) {
        if (this.cellColor != value) {
            this.cellColor = value
        }
    }

    private fontColor: string = "#282E30";
    get FontColor() { return this.fontColor; }
    set FontColor(value: string) {
        if (this.fontColor != value) {
            this.fontColor = value
        }
    }

    private disableColors: boolean = false;
    get DisableColors() { return this.disableColors; }
    set DisableColors(value: boolean) {
        if (this.disableColors != value) {
            this.disableColors = value;
            this.SetCellColors();
        }
    }

    private tabIndex: number = 0;
    get TabIndex() { return this.tabIndex; }
    set TabIndex(value: number) {
        if (this.tabIndex != value) {
            this.tabIndex = value;
        }
    }

    private alignment: string = "left";
    get Alignment() { return this.alignment; }
    set Alignment(value: string) {
        if (!AppTool.IsNullOrEmpty(value)) {
            value = value.toLowerCase();
        }

        if (this.alignment != value) {
            this.alignment = value
        }
    }

    private isEditMode: boolean = false;
    get IsEditMode() { return this.isEditMode; }
    set IsEditMode(value: boolean) {
        if (this.isEditMode != value) {
            this.isEditMode = value;

            if (value) {
                this.LeftIndent = this.RightIndent = 0;
            }
        }
    }

    private isDisplayMode: boolean = true;
    get IsDisplayMode() { return this.isDisplayMode; }
    set IsDisplayMode(value: boolean) {
        if (this.isDisplayMode != value) {
            this.isDisplayMode = value;

            if (value) {
                this.LeftIndent = this.RightIndent = 3;
            }
        }
    }

    private leftIndent: number = 3;
    get LeftIndent() { return this.leftIndent; }
    set LeftIndent(value: number) {
        if (!this.IsFilled) {
            if (this.leftIndent != value) {
                this.leftIndent = value;
            }
        }
    }

    private rightIndent: number = 3;
    get RightIndent() { return this.rightIndent; }
    set RightIndent(value: number) {
        if (!this.IsFilled) {
            if (this.rightIndent != value) {
                this.rightIndent = value;
            }
        }
    }

    EventSub: any;
    ngOnInit() {
        if (this.IsFilled) {
            this.leftIndent = this.rightIndent = 0;
        }

        this.HStyle = this.ColumnComponent.HeaderStyle;
        this.headertext = this.ColumnComponent.header;
        this.bindingfield = this.ColumnComponent.binding;
        this.cellwidth = this.ColumnComponent.width;
        this.cellvisibility = this.ColumnComponent.visibility;
        this.isEditable = this.ColumnComponent.Editable;
        this.isrequired = this.ColumnComponent.required;

        //this.Editindex = this.ColumnComponent.Editindex;
        ////this.CurrentSession.ResetRowIndex();
        if (this.RIndex >= 0) {
            this.RowIndex = this.RIndex;
            //console.log("RIndex = " + this.RowIndex + " LogId : " + this.ColumnComponent.LogGridId);
        }
        else {
            this.RowIndex = this.CurrentSession.LogitudeGridHelper.GetNextRowIndex(this.ColumnComponent.LogGridId);
            //this.RowIndex = this.CurrentSession.LogitudeGridHelper.GetRowIndex(this.ColumnComponent.LogGridId);
            //console.log("RowIndex = " + this.RowIndex + " LogId : " + this.ColumnComponent.LogGridId);
        }
        this.TempRowIndex = this.RowIndex;
        this.OuterDivId = this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + this.RowIndex;
        //if (this.RowIndex == 0) {
        //    this.RowIndex = 1;
        //}
        var ind = this.CurrentSession.LogitudeGridHelper.GetEditCellIndex();
        if ((this.ColumnComponent.index + 1) == this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId)) {
            //console.log("IN " + this.RowIndex);
            this.CurrentSession.LogitudeGridHelper.ResetEditCellIndex();
            ind = this.CurrentSession.LogitudeGridHelper.GetEditCellIndex();
            if (this.RIndex < 0) {
                this.RowIndex = this.CurrentSession.LogitudeGridHelper.GetNextRowIndex(this.ColumnComponent.LogGridId);
                this.CurrentSession.LogitudeGridHelper.SetNextRowIndex(this.ColumnComponent.LogGridId);
                //this.RowIndex = this.CurrentSession.LogitudeGridHelper.SetRowIndex(this.ColumnComponent.LogGridId);
            }
            console.log("RowIndex = " + this.RowIndex + " LogId : " + this.ColumnComponent.LogGridId);
            this.TempRowIndex = this.RowIndex;
        }
        this.Editindex = ind;
    }
    //this.Editindex
    KeyUpEvent($event) {
        //if ((this.ColumnComponent.index + 1) == this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId)) {
        //    //console.log("End Of Current Row : " + this.Editindex);
        //    this.CurrentSession.EndOfRowReachedEvent.emit(this.RowIndex + 1);
        //}
        if ($event.keyCode == 9) {
            if ($event.shiftKey) {
                //this.CurrentSession.isShiftClicked = false;
                this.CurrentSession.isTabWithShiftClicked = false;
            }
        }
    }
    i
    KeyDownEvent($event) {
        if ($event.keyCode == 9) {
            if ($event.shiftKey) {
                this.CurrentSession.isTabWithShiftClicked = true;
            }
        }

        var colCount = this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId);
        if ((this.ColumnComponent.index + 1) == colCount && $event.keyCode == 9) {
            //console.log("End Of Current Row : " + this.Editindex);
            this.CurrentSession.CurrentLogGrid = this.ColumnComponent.LogGridId;
            this.CurrentSession.EndOfRowReachedEvent.emit(this.RowIndex + 1);
        }
        if ($event.keyCode == 13) {
            var element = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + (this.TempRowIndex + 1));
            if (element) {
                element.focus();
            }
        }
        if ($event.keyCode == 121) {
            if ($event.altKey) {
                var originalElement = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + (this.TempRowIndex));
                //var nextElement = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + (this.TempRowIndex + 1));
                var originalData = this.ColumnComponent.EditableLogGridComponent.ItemSource.Collection[this.RowIndex - 1][this.ColumnComponent.binding];
                var nextData = this.ColumnComponent.EditableLogGridComponent.ItemSource.Collection[this.RowIndex][this.ColumnComponent.binding];
                var elementinputs = originalElement.getElementsByTagName("input");
                if (elementinputs.length>0){
                    this.CurrentSession.CopiedCell = originalData;

                    this.CurrentSession.CopyCellIntoMemory.emit(elementinputs[0].id);
                }
                //this.ColumnComponent.EditableLogGridComponent.ItemSource.Collection[this.RowIndex][this.ColumnComponent.binding] = originalData;

                if ((this.ColumnComponent.index + 1) == colCount) {
                    this.CurrentSession.CurrentLogGrid = this.ColumnComponent.LogGridId;
                    this.CurrentSession.EndOfRowReachedEvent.emit(this.RowIndex + 1);
                    if (this.ColumnComponent.EditableLogGridComponent.ItemSource.Length > (this.RowIndex + 1)) {
                        var columnIndex = this.ColumnComponent.index + 1;
                        var elementId = this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + 1 + "_" + (this.TempRowIndex+1);
                        var nextElement = this.getNextIndexedElement(this.CurrentSession.SessionIndex, (this.ColumnComponent.index + 1), (this.TempRowIndex));//document.getElementById(elementId);
                        nextElement.focus();
                    }
                }
                else {
                    var columnIndex = this.ColumnComponent.index + 1;
                    var elementId = this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + columnIndex + "_" + (this.TempRowIndex);
                    var nextElement = document.getElementById(elementId);
                    nextElement.focus();
                }

                //var elements = originalElement.getElementsByTagName("logtextbox");
                //var elementinputs = originalElement.getElementsByTagName("input");
                //if (elementinputs != null && elementinputs.length>0 && nextElement != null) {
                //    this.CurrentSession.CopyCellIntoMemory.emit(elementinputs[0].id);

                //    nextElement.focus();
                //}
                //var fieldName = elements[0].getAttributeNode("ng-reflect--object-field-name");
                //var dataContext = elements[0].getAttributeNode("ng-reflect--data-context");

                //var nextElements = nextElement.getElementsByTagName("logtextbox");
                //var nFieldName = nextElements[0].getAttribute("ng-reflect--object-field-name");
                //var nDataContext = nextElements[0].getAttribute("ng-reflect--data-context");

               // nDataContext[nFieldName] = dataContext[fieldName];

            }
        }
    }

    private timerToken: any;
    IsClickedOnce: boolean = false;
  MyElement: HTMLElement;
  private focusTimerToken: any;
    blurevt(_thisComponent: LogCellTemplateComponent) {
      if (!SessionLocator.SustainFocusOnCell) {
        //this.focusTimerToken = setTimeout(() => {
        //  ElementProperities(_thisComponent);
        //}, 1);
          ElementProperities(_thisComponent);
            
            if (_thisComponent.EventSub) {
                _thisComponent.EventSub.unsubscribe();
                _thisComponent.IsClickedOnce = false;
            }
            //console.log("Cell Id : " + _thisComponent.MyElement.id);
            _thisComponent.MyElement.removeEventListener("blur", () => this.blurevt(_thisComponent));
            // SessionLocator.SustainLostFocusOnCell = true;
        }
        //else {

        //    SessionLocator.SustainFocusOnCell = false;
        //    // SessionLocator.SustainLostFocusOnCell = false;
        //}

    }

    OnBlurEventImplementation(_thisComponent: LogCellTemplateComponent) {
        if (!SessionLocator.SustainFocusOnCell) {
        //   this.focusTimerToken = setTimeout(() => {
            
        //   }, 1);
        ElementProperities(_thisComponent);
            if (_thisComponent.EventSub) {
                _thisComponent.EventSub.unsubscribe();
                _thisComponent.IsClickedOnce = false;
            }
            this.CurrentSession.LostFocusEvent.emit("");

        }
        else {
            SessionLocator.SustainFocusOnCell = false;
        }
    }

    OnClick() {

        if (this.CurrentSession.isTabWithShiftClicked == false) {

            if (this.IsEnabled && !this.ColumnComponent.IsReadOnlyGrid) {
                if (this.isEditable && this.IgnoreMods == false) {
                    if (this.TabIndex == 0) {
                        this.IsEditMode = true;
                        this.IsDisplayMode = false;
                        this.CellClicked.emit(this.RowIndex);
                    }
                }
            }
            if (!this.IsClickedOnce) {
                this.SubscribeCellFocus();
            }
            this.IsClickedOnce = true;

        }
        else {
            this.CurrentSession.isTabWithShiftClicked = false;
            //this.CurrentSession.isShiftClicked = false;
            //console.log("isTabWithShiftClicked = false;")
            //this.CurrentSession.isShiftClicked = false
            var element = this.getPrevIndexedElement(this.CurrentSession.SessionIndex, (this.ColumnComponent.index - 1), (this.TempRowIndex));
            if (element) {
                element.focus();
            }
            //else {
            //    this.CurrentSession.AllowShiftTab = false;
            //    //this.CurrentSession.isTabWithShiftClicked = false;
            //    //console.log("isTabWithShiftClicked = false;")
            //}
        }

    }

    SubscribeCellFocus() {
        this.EventSub = this.CurrentSession.SessionEvent.subscribe((res) => {
            if (SessionLocator.SustainFocusOnCell) {
                if (this.OuterDivId == res.OuterDivId) {
                    this.IsEditMode = true;
                    this.IsDisplayMode = false;
                    var element = document.getElementById(res.LogTextBoxId);
                    element.focus();

                }
            }
            if (res.IsCell && !res.IsEnterCLicked) {
                var _thisComponent = this;
                if (res.OnBlurEvent) {
                    res.OnBlurEvent.subscribe((res1) => this.OnBlurEventImplementation(_thisComponent));
                }
                else {
                    this.MyElement = document.getElementById(res.Id);

                    this.MyElement.addEventListener("blur", () => this.blurevt(_thisComponent));
                }

            }
            if (res.IsCell && res.IsEnterCLicked == true) {
                var ctrl = document.getElementById(res.Id);
                var mine = this.checkIt(ctrl);
                if (mine) {
                    this.IsEditMode = false;
                    this.IsDisplayMode = true;
                    var element = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + (this.TempRowIndex + 1));
                    if (element) {
                        element.focus();
                    }
                }
            }

        });
    }
    //SetFirstLostFocusFalse() {
    //    this.IsLostFocusOnce = false;
    //}
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.EventSub);
        AppTool.KillEventEmitter(this.ObsNewElementInsertedSub);

    }



    ObBlure() {
        //console.log("ObBlure");
        //setTimeout(function () {
        //    var focus = document.activeElement;
        //    var MainElement = document.getElementById("OuterDiv");
        //    for (var i = 0; i < MainElement.children.length; i++) {
        //        var tableChild = MainElement.children[i];
        //        if (tableChild.isEqualNode(focus)) {
        //            console.log("still focused");
        //        } else {
        //            this.IsDisplayMode = true;
        //            this.IsEditMode = false;
        //            console.log("No");
        //            this.CD.detectChanges();
        //        }
        //    }
        //}, 0);

    }

    addListner(MainElement: HTMLElement) {
        for (var i = 0; i < MainElement.children.length; i++) {
            var Child = MainElement.children[i] as HTMLElement;
            this.addListner(Child);
        }
        MainElement.addEventListener("blur", function () {
            MainMenuProperties(MainElement);
            //this.IsDisplayMode = true;
            //this.IsEditMode = false;
            //this.CD.detectChanges();
            //console.log("OnCustomBlure");
        });
    }

    //@Input('') active = false;


    checkIt(ctrl: HTMLElement) {
        //var ctrl = document.getElementById(ctrlId);
        if (ctrl.parentElement) {

            if (this.OuterDivId == ctrl.parentElement.id) {
                return true;
            }

            return this.checkIt(ctrl.parentElement);
        }
        else {
            return false;
        }
    }

    getPrevIndexedElement(SessionIndex: number, Columnindex: number, Rowindex: number): HTMLElement {
        //this.CurrentSession.getColumnsCount()
        if (Columnindex >= 0 || Rowindex > 0) {
            var ctrl = document.getElementById(this.ColumnComponent.LogGridId + SessionIndex + "_" + Columnindex + "_" + Rowindex);
            if (Columnindex < 0) {
                var ctrl = document.getElementById(this.ColumnComponent.LogGridId + SessionIndex + "_" + (this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId) - 1) + "_" + (Rowindex - 1));
                if (ctrl == null || ctrl.tabIndex != 0) {
                    return this.getPrevIndexedElement(SessionIndex, (this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId) - 1), Rowindex - 1);
                }
                else {
                    return ctrl;
                }
            }
            if (ctrl == null || ctrl.tabIndex != 0) {
                return this.getPrevIndexedElement(SessionIndex, Columnindex - 1, Rowindex);
            }
            else {
                return ctrl;
            }
        }
        else {
            return null;
        }
    }

    getNextIndexedElement(SessionIndex: number, Columnindex: number, Rowindex: number): HTMLElement {
        //this.CurrentSession.getColumnsCount()
        var colCount = this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId);
        if (Columnindex <= colCount || Rowindex > 0) {
            var ctrl = document.getElementById(this.ColumnComponent.LogGridId + SessionIndex + "_" + Columnindex + "_" + Rowindex);
            if (Columnindex > colCount) {
                var ctrl = document.getElementById(this.ColumnComponent.LogGridId + SessionIndex + "_" + 0 + "_" + (Rowindex + 1));
                if (ctrl == null || ctrl.tabIndex != 0) {
                    return this.getNextIndexedElement(SessionIndex, (Columnindex > 0 ? (0 + 1) : (Columnindex + 1)), Rowindex + 1);
                }
                else {
                    return ctrl;
                }
            }
            if (ctrl == null || ctrl.tabIndex != 0) {
                return this.getNextIndexedElement(SessionIndex, Columnindex + 1, Rowindex);
            }
            else {
                return ctrl;
            }
        }
        else {
            return null;
        }
    }
}
