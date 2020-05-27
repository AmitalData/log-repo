"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var SendButton = /** @class */ (function () {
    function SendButton(_CD, myElement) {
        this._CD = _CD;
        this.ButtonText = "Send";
        this.IsMouseOver = false;
        this._DropdownDisplay = 'none';
        this.ControlId = null;
        this._IsLoaded = false;
        this.Binding = null;
        this.Text = null;
        this.IsEnabled = true;
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.LostFocus = new core_1.EventEmitter();
        this.selectedItem = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Width = -30;
        this.Height = -20;
        this.ItemsSource = [];
        this._ElementRef = myElement;
        var idIndex = this.CurrentSession.GetNewId("SenButton");
        this._CustomSendOptionsComponentId = "CustomSendOptionsComponent_" + idIndex;
        this._CustomSendOptionsComponentMenuId = "CustomSendOptionsComponentMenuId_" + idIndex;
        this.ControlId = "ComboBox_" + idIndex;
        this.ListControlId = "List_" + idIndex;
    }
    Object.defineProperty(SendButton.prototype, "ButtonCodeText", {
        get: function () { return this._ButtonCodeText; },
        set: function (val) {
            if (this._ButtonCodeText == val)
                return;
            this._ButtonCodeText = val;
            this.ButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate(val);
            this._CD.detectChanges();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendButton.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
                this.SetDisplayText();
            }
        },
        enumerable: true,
        configurable: true
    });
    SendButton.prototype.ItemClicked = function (clickedItem) {
        if (clickedItem != null && clickedItem.IsEnabled) {
            if (this.SelectedItem != clickedItem) {
                this.SelectedItem = clickedItem;
                this.SetDisplayText();
                this.SelectedItemChanged.emit(this.SelectedItem);
                this.DropdownDisplayClose();
            }
        }
    };
    SendButton.prototype.SetDisplayText = function () {
        var myDisplayText = null;
        if (this.SelectedItem != null) {
            this.IsEnabled = this.SelectedItem.IsEnabled;
            if (this.Binding == null) {
                myDisplayText = this.SelectedItem;
            }
            else {
                myDisplayText = this.SelectedItem[this.Binding];
            }
        }
        this.Text = myDisplayText;
    };
    SendButton.prototype.handleClick = function (event) {
        var clickedComponent = event.target;
        var inside = false;
        var conter = 0;
        do {
            if (clickedComponent === this._ElementRef.nativeElement) {
                inside = true;
                break;
            }
            if (conter > 10) {
                break;
            }
            conter++;
            clickedComponent = clickedComponent.parentNode;
        } while (clickedComponent);
        if (inside) {
        }
        else {
            if (this._DropdownDisplay == 'block') {
                this.dropdowndisplayToggle();
            }
            //alert("outside");
        }
    };
    SendButton.prototype.ngOnInit = function () {
        this._IsLoaded = true;
        if (this.SelectedItem != null) {
            this.SetDisplayText();
        }
    };
    SendButton.prototype.DropdownDisplayClose = function () {
        this._DropdownDisplay = 'none';
    };
    SendButton.prototype.dropdowndisplayToggle = function () {
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._CustomSendOptionsComponentId);
            var itemRect = item.getBoundingClientRect();
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.top = (itemRect.top + 24 ) + 'px';
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.left = (itemRect.left + 24 - this.Width) + 'px';
            document.getElementById(this._CustomSendOptionsComponentMenuId).style.top =
                itemRect.top + 'px';
            var DDLHeight = 67; //    height: 22px; * 3 +30 
            var Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
            if (itemRect.bottom + DDLHeight > this.getScreenHeight()) { //this.PaintTop = true                
                document.getElementById(this._CustomSendOptionsComponentMenuId).style.top =
                    (itemRect.top - DDLHeight - Extra) + 'px';
            }
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.left =
            //    (itemRect.left + 80) + 'px';//min-width: 80px
            this._DropdownDisplay = 'block';
        }
        else {
            this._DropdownDisplay = 'none';
        }
    };
    SendButton.prototype.getScreenHeight = function () {
        if (self.innerHeight) {
            return self.innerHeight;
        }
        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }
        if (document.body) {
            return document.body.clientHeight;
        }
    };
    SendButton.prototype.mousedownGreenButton = function () {
        var clickedItem = this.SelectedItem;
        if (clickedItem != null) {
            var sendButton = document.getElementById(this.ControlId);
            if (sendButton != null) {
                sendButton.blur();
            }
            this.OnLostFocus();
            this.SetDisplayText();
            this.SelectedItemChanged.emit(this.SelectedItem);
        }
    };
    SendButton.prototype.OnLostFocus = function () {
        this.DropdownDisplayClose();
        this.LostFocus.emit(true);
    };
    SendButton.prototype.OnButtonLostFocus = function () {
        if (this.IsMouseOver) {
            document.getElementById(this._CustomSendOptionsComponentMenuId).focus();
        }
        else {
            this.DropdownDisplayClose();
        }
    };
    SendButton.prototype.OnFocus = function () {
    };
    SendButton.prototype.OnKeyDown = function ($event) {
        if ($event) {
            switch ($event.keyCode) {
                case 8:
                case 46:
                    {
                        this.SelectedItem = null;
                        this.SelectedItemChanged.emit(null);
                        break;
                    }
            }
        }
    };
    SendButton.MyId = 0;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], SendButton.prototype, "IsDisabled", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], SendButton.prototype, "ButtonText", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String),
        __metadata("design:paramtypes", [String])
    ], SendButton.prototype, "ButtonCodeText", null);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SendButton.prototype, "SelectedItemChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SendButton.prototype, "LostFocus", void 0);
    SendButton = __decorate([
        core_1.Component({
            selector: 'SendButton',
            moduleId: module.id,
            templateUrl: './SendButton.html',
            inputs: ['ItemsSource', 'SelectedItem', 'Binding'],
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, core_1.ElementRef])
    ], SendButton);
    return SendButton;
}());
exports.SendButton = SendButton;
//# sourceMappingURL=SendButton.js.map