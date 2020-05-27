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
var EditableListTemplateComponent = /** @class */ (function () {
    function EditableListTemplateComponent(_injector, _elementRef) {
        this._injector = _injector;
        this._elementRef = _elementRef;
        this.clickevent = new core_1.EventEmitter();
        this.onblurEvent = new core_1.EventEmitter();
        this.noComponent = true;
    }
    EditableListTemplateComponent.prototype.onclick = function () {
        if (this.editable == true) {
            this.noComponent = false;
        }
    };
    EditableListTemplateComponent.prototype.onblurevt = function () {
        this.onblurEvent.emit("");
    };
    EditableListTemplateComponent.prototype.handleonblur = function () {
        this.noComponent = true;
        this.onblurEvent.emit(this.parentId);
    };
    EditableListTemplateComponent.prototype.ngOnInit = function () {
        //if (this.format != undefined) {
        //    var numafterdot = this.format[1];
        //    var number = this.rowData[this.fieldName].toFixed(numafterdot);
        //    this.Data = number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        //}
        //else {
        //    this.Data = this.rowData[this.fieldName];
        //}
        //var temp = this.fieldName.split(':');
        //if (temp.length == 2) {
        //    this.fieldName = temp[0];
        //    this.Pipe = temp[1];
        //}
        var _this = this;
        var BackGroundColor = "transparent";
        if (this.editable == false) {
            this.temp = "-1";
            BackGroundColor = "rgba(230, 231, 232, 0.5)";
            this.customestyle = "overflow: hidden; text-overflow: ellipsis; background-color: rgba(230, 231, 232, 0.5);";
        }
        else {
            this.temp = "0";
            this.customestyle = "overflow: hidden; text-overflow: ellipsis; background: transparent none repeat scroll 0 0;";
        }
        if (this.Alignment != undefined) {
            this.customestyle = {
                "overflow": "hidden", "text-overflow": "ellipsis", "height": "25px", "text-align": this.Alignment, "background-color": BackGroundColor
            };
        }
        else {
            this.customestyle = { "overflow": "hidden", "text-overflow": "ellipsis", "height": "25px", "text-align": "right", "background-color": BackGroundColor };
        }
        this.event.subscribe(function (res) {
            if (res == _this.parentId)
                _this.onclick();
        });
        if (this.TabIndex == -1) {
            this.temp = "-1";
        }
        if (this.type == "Image") {
            this.showimg = true;
            this.showtext = false;
            this.showbtn = false;
        }
        else if (this.type == "lookup") {
            this.showlookup = true;
            this.showimg = false;
            this.showtext = false;
            this.showbtn = false;
        }
        else if (this.type == "IconButton") {
            this.showimg = false;
            this.showtext = false;
            this.showbtn = true;
        }
        else {
            this.showimg = false;
            this.showtext = true;
            this.showbtn = false;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableListTemplateComponent.prototype, "clickevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableListTemplateComponent.prototype, "onblurEvent", void 0);
    EditableListTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'edit-list-template',
            //template: `<div style="{{customestyle}}" tabindex={{temp}} (click)="onclick()" (keyup)="keyupHandler($event)"> 
            //           <span><span style="text-overflow: ellipsis" (click)="onclick()" *ngIf="noComponent && showtext">{{rowData[fieldName]}}</span></span>
            //           <img style="vertical-align: middle;" tabindex=1 *ngIf="showimg" src="{{src}}" />
            //           <inner-component  style="width:85%" [rowData]="rowData" [fieldName]="fieldName" (blurevent)="handleonblur()" *ngIf="!noComponent && showtext"></inner-component>
            //           </div>`,
            templateUrl: './EditableListTemplateComponent.html',
            //directives: [InnerComponent, InnerSpanComponent],
            inputs: ['fieldName', 'rowData', 'editable', 'type', 'src', 'event', 'parentId', 'Alignment', 'required', 'ComponentName', 'ComponentUrl', 'ObjectTableName', 'format', 'allowtomove', 'width', 'Id', 'TabIndex'],
        }),
        __metadata("design:paramtypes", [core_1.Injector, core_1.ElementRef])
    ], EditableListTemplateComponent);
    return EditableListTemplateComponent;
}());
exports.EditableListTemplateComponent = EditableListTemplateComponent;
//# sourceMappingURL=EditableListTemplateComponent.js.map