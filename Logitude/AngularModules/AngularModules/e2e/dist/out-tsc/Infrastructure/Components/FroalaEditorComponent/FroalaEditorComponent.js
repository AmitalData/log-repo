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
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var FroalaEditorComponent = /** @class */ (function () {
    function FroalaEditorComponent(elementRef, cd) {
        this.cd = cd;
        this.ComponentFroalaReady = new core_1.EventEmitter();
        this.IsDisableMode = false;
        this.FroalaReady = new core_1.EventEmitter();
        this.elementRef = elementRef;
    }
    FroalaEditorComponent.prototype.ngOnInit = function () {
        this.EditorfroalaSetting.froalaEditorComponent = this;
        this.Id = this.EditorfroalaSetting.Id;
        this.PreviewDivId = this.EditorfroalaSetting.Id + "Div";
        this.PreviewDivHeight = this.EditorfroalaSetting.Height + "px";
        this.PreviewDivHeight2 = (this.EditorfroalaSetting.Height - 10) + "px";
        if (this.EditorfroalaSetting.IsDisableEdit) {
            this.IsDisableMode = true;
        }
    };
    FroalaEditorComponent.prototype.ngAfterViewInit = function () {
        //FileLoader.LoadFroalaResources().then((isLoaded: boolean) => {
        //    this.ResourcesLoaded.emit(true);
        //    this.ShowEditor(); 
        //});     
        this.ShowEditor();
    };
    FroalaEditorComponent.prototype.ShowEditor = function (height) {
        if (height === void 0) { height = this.EditorfroalaSetting.Height; }
        if (this.EditorfroalaSetting.HtmlString)
            this.HtmlString = this.EditorfroalaSetting.HtmlString;
        if (!this.HtmlString)
            this.HtmlString = "";
        //  this.HtmlString = this.CheckHtmlStyle(this.HtmlString);
        //Froala Editor
        var HtmlID = getHTMLID(this.Id);
        RegisterCustomFroalaEditorButtom(this);
        var froalakey = ObjectsLocator_1.ObjectsLocator.GlobalSetting != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment == "cloud" ? "8A-9pwkamE5f1kG4ok==" : "ubd1wxffppaxjuE-11A2C-9rs==";
        if (HtmlID.data('froala.editor'))
            HtmlID.froalaEditor('destroy');
        HtmlID.froalaEditor({
            allowedImageTypes: ["jpeg", "jpg", "png"],
            toolbarButtons: this.EditorfroalaSetting.PageType == "Send" ? ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'paragraphStyle', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'insertTable', 'undo', 'redo', 'selectAll', 'rightToLeft', 'leftToRight'] : ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'paragraphStyle', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'insertTable', 'undo', 'redo', 'selectAll', 'insertLink', 'rightToLeft', 'leftToRight', 'PageBreak'],
            toolbarButtonsMD: this.EditorfroalaSetting.PageType == "Send" ? ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'paragraphStyle', 'paragraphFormat', 'align', 'formatOL', 'insertTable', 'rightToLeft', 'leftToRight'] : ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'paragraphStyle', 'paragraphFormat', 'align', 'formatOL', 'insertTable', 'insertLink', 'rightToLeft', 'leftToRight', 'PageBreak'],
            toolbarButtonsSM: this.EditorfroalaSetting.PageType == "Send" ? ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'insertTable', 'align', 'rightToLeft', 'leftToRight'] : ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'align', 'insertTable', 'rightToLeft', 'leftToRight', 'PageBreak'],
            toolbarButtonsXS: this.EditorfroalaSetting.PageType == "Send" ? ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'insertTable', 'align', 'rightToLeft', 'leftToRight'] : ['bold', 'italic', 'underline', 'fontFamily', 'fontSize', 'color', 'inlineStyle', 'align', 'insertTable', 'rightToLeft', 'leftToRight', 'PageBreak'],
            lineBreakerTags: ['table', 'hr', 'form'],
            pluginsEnabled: null,
            height: height,
            heightMax: height,
            iframe: true,
            charCounterCount: false,
            inlineMode: false,
            zIndex: -1,
            direction: '',
            key: froalakey,
            useClasses: false,
            tableStyles: {
                All: 'All',
                Box: 'Box',
                None: 'None',
                Red: 'Border red',
                Blue: 'Border blue',
                DarkBlue: 'Border dark blue',
                //Green: 'Border green',
                //Yellow: 'Border yellow',
                Brown: 'Border brown',
                Maroon: 'Border maroon',
                Black: 'Border Black',
                Gray: 'Border gray',
                LightGray: 'Border light gray',
                White: 'Border white',
            },
            //scrollableContainer: '#' + this.Id,
            tableMultipleStyles: true,
            tableCellStyles: {
                BorderLeft: 'Remove border left',
                BorderRight: 'Remove border right',
                BorderBottom: 'Remove border bottom',
                BorderTop: 'Remove border top',
            },
        });
        HtmlID.froalaEditor('html.set', this.HtmlString);
        if (this.IsDisableMode) {
            HtmlID.froalaEditor('edit.off');
            HtmlID.froalaEditor('toolbar.hide');
        }
        this.EditorfroalaSetting.FroalaEditorIsReady = true;
    };
    FroalaEditorComponent.prototype.getHtml = function () {
        var html = "";
        var HtmlID = getHTMLID(this.Id);
        if (this.EditorfroalaSetting.FroalaEditorIsReady) {
            if (HtmlID) {
                html = HtmlID.froalaEditor('html.get');
            }
            else
                html = this.HtmlString;
            this.EditorfroalaSetting.HtmlString = html;
        }
        return html;
    };
    FroalaEditorComponent.prototype.SetHtml = function (html) {
        this.HtmlString = this.EditorfroalaSetting.HtmlString = html;
        //  this.HtmlString = this.CheckHtmlStyle(this.HtmlString);
        if (this.EditorfroalaSetting.FroalaEditorIsReady) {
            var HtmlID = getHTMLID(this.Id);
            if (HtmlID) {
                HtmlID.froalaEditor('html.set', html);
            }
        }
    };
    FroalaEditorComponent.prototype.SetHeight = function (height) {
        this.ShowEditor(height);
    };
    FroalaEditorComponent.prototype.InSertHtml = function (html) {
        if (!this.IsDisableMode) {
            if (this.EditorfroalaSetting.FroalaEditorIsReady) {
                var HtmlID = getHTMLID(this.Id);
                if (HtmlID) {
                    HtmlID.froalaEditor('html.insert', html, true);
                }
            }
        }
    };
    FroalaEditorComponent.prototype.DestroyfroalaEditor = function () {
        if (this.EditorfroalaSetting.FroalaEditorIsReady) {
            var HtmlID = getHTMLID(this.Id);
            if (HtmlID && HtmlID.data('froala.editor')) {
                HtmlID.froalaEditor('destroy');
            }
        }
    };
    FroalaEditorComponent.prototype.CheckHtmlStyle = function (html) {
        if (html.indexOf(".Class1") == -1) {
            var styles = "<style>.class1{border-collapse: collapse;}.class1 td, th{border: 1px solid red;line-height:21px;}.class1 td{font-size: 14px; padding-left: 4px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;}.class2{border-collapse: collapse;}.class2 td, th{border: 1px solid blue;line-height:21px;}.class2 td{font-size: 14px; padding-left: 4px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;}table thead tr th, table tbody tr td  {font - size: 14px; padding - left: 4px; overflow: hidden; white - space: nowrap; text - overflow: ellipsis; border: 1px solid lightgray; line - height:18px; } table{border-collapse: collapse; }</style>";
            html = styles + html;
        }
        return html;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], FroalaEditorComponent.prototype, "ComponentFroalaReady", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], FroalaEditorComponent.prototype, "FroalaReady", void 0);
    FroalaEditorComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'FroalaEditor',
            templateUrl: './FroalaEditorComponent.html',
            inputs: ['EditorfroalaSetting']
        }),
        __metadata("design:paramtypes", [core_1.ElementRef, core_1.ChangeDetectorRef])
    ], FroalaEditorComponent);
    return FroalaEditorComponent;
}());
exports.FroalaEditorComponent = FroalaEditorComponent;
//# sourceMappingURL=FroalaEditorComponent.js.map