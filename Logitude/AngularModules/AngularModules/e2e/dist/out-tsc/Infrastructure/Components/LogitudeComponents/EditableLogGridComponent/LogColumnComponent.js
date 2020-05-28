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
var LogFooterComponent_1 = require("./LogFooterComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogColumnComponent = /** @class */ (function () {
    function LogColumnComponent() {
        this.format = null;
        this.LogGridId = "";
        this.IsReadOnlyGrid = false;
        this.IgnoreColumn = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //this.EditableLogGridComponent = ELG;
    }
    //@ContentChildren(LogCellTemplateComponent) panes: QueryList<LogCellTemplateComponent>;
    //@ContentChild(forwardRef(() => LogCellTemplateComponent))
    //private navComponent: LogCellTemplateComponent;
    //@ViewChildren(LogCellTemplateComponent) cellChildren: QueryList<LogCellTemplateComponent>;
    LogColumnComponent.prototype.ngAfterContentInit = function () {
        this.ColId = this.CurrentSession.LogitudeGridHelper.GetColumnId();
        var temp = this.childChildren;
        if (temp) {
            this.hasFootertemplate = true;
            this.FooterContentTemplate = temp.myChild;
            this.innerContentTpl = this.innerContentTpl.filter(function (a) { return a != temp.myChild; });
        }
        else {
            this.hasFootertemplate = false;
            this.innerContentTpl = this.innerContentTpl.toArray();
        }
        // get all active tabs
        //if (this.childChildren.length > 0) {
        //    //alert("Yea");
        //}
        this.hastemplate = this.innerContentTpl.length == 0 ? false : true;
        this.hasHeadertemplate = this.innerContentTpl.length < 2 ? false : true;
        // var temp = this.navComponent;
        this.CurrentSession.SubscriptionAdd(this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res.IsCell) {
                if (res.IsEnterCLicked == true) {
                    var element = document.getElementById(res.Id);
                    element.blur();
                    //alert(res.Id);
                }
            }
        }));
        //if (this.hastemplate) {
        //    this.Editindex = this.CurrentSession.GetEditCellIndex();
        //}
    };
    //@ContentChildren(TemplateRef) contentTpl: any;
    LogColumnComponent.prototype.ngAfterViewInit = function () {
        //var temp = this.navComponent;
        //var ss = this.cellChildren;
        //alert(this.LogGridId);
    };
    LogColumnComponent.prototype.ngOnInit = function () {
        //this.CurrentSession.LogitudeGridHelper.SetColumnsCount(false, this.LogGridId);
        //this.headerStyle = {
        //    'width': (this.ViewWidth) + 'px',
        //    'min-width': (this.ViewWidth) + 'px'
        //};
        this.Style = {
            width: +this.width + 'px',
            'background-color': this.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
            'text-align': this.Alignment ? this.Alignment : 'right',
            'display': this.visibility == 'hidden' ? 'none' : 'inline'
        };
        this.HeaderStyle = {
            width: +this.width + 'px',
            'min-width': this.width + 'px'
            //'display': this.visibility == 'hidden' ? 'none' : 'inline-block'
        };
    };
    Object.defineProperty(LogColumnComponent.prototype, "visibility", {
        get: function () { return this.visi; },
        set: function (newValue) {
            if (this.visi != newValue) {
                this.visi = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ContentChildren(core_1.TemplateRef),
        __metadata("design:type", Object)
    ], LogColumnComponent.prototype, "innerContentTpl", void 0);
    __decorate([
        core_1.ContentChild(LogFooterComponent_1.LogFooterComponent),
        __metadata("design:type", LogFooterComponent_1.LogFooterComponent)
    ], LogColumnComponent.prototype, "childChildren", void 0);
    LogColumnComponent = __decorate([
        core_1.Component({
            selector: 'log-column',
            template: '<ng-content></ng-content>',
            inputs: [
                'header: header',
                'binding: binding',
                'HeaderStyle: HeaderStyle',
                'width: width',
                'visibility: visibility',
                'Editable: Editable',
                'Style: Style',
                'Alignment: Alignment',
                'required: required',
                'AllowDisableCells:AllowDisableCells',
                'Disabled: Disabled',
                'format: format',
                'SortFieldName:SortFieldName',
                'IgnoreColumn:IgnoreColumn'
            ]
        })
        //@Directive({ selector: 'log-cell-template' })
        ,
        __metadata("design:paramtypes", [])
    ], LogColumnComponent);
    return LogColumnComponent;
}());
exports.LogColumnComponent = LogColumnComponent;
//# sourceMappingURL=LogColumnComponent.js.map