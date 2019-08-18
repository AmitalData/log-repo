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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteTemplateSectionExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var PreviewQuoteTemplateReportComponent = /** @class */ (function () {
    function PreviewQuoteTemplateReportComponent() {
        this.PdfDivKey = Guid_1.Guid.newGuid();
        this.isFromLibrary = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService_1.QuoteTemplateSectionExtendedPMService();
    }
    PreviewQuoteTemplateReportComponent.prototype.ngOnInit = function () {
    };
    PreviewQuoteTemplateReportComponent.prototype.ngAfterViewInit = function () {
        this.GetQuoteTemplatePdfReport();
    };
    PreviewQuoteTemplateReportComponent.prototype.GetQuoteTemplatePdfReport = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.quoteTemplateSectionExtendedPMService.GetQuoteTemplatePdfReport(this.QuoteId, this.QuoteTemplateId, SessionLocator_1.SessionLocator.LoggedUserId, this.isFromLibrary).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                var buffer = EntityResourceService_1.EntityResourceService.base64ToBufferConvertor(pmResponse.Result);
                var blob = new Blob([buffer], { type: 'application/pdf' });
                var objectURL = URL.createObjectURL(blob);
                var doc = document.getElementById(_this.PdfDivKey);
                if (doc) {
                    doc.innerHTML = "<iframe id='fred' style='border: 1px solid gray;' frameborder='1' scrolling='auto' height=" + _this.HeightPdf + " width='100%' src=" + objectURL + "> </iframe>";
                }
            }
        });
    };
    PreviewQuoteTemplateReportComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PreviewQuoteTemplateReportComponent.prototype.SetWindowArgs = function (args) {
        this.QuoteTemplateId = args.QuoteTemplateId;
        this.QuoteId = args.QuoteId;
        this.HeightPdf = (this.CurrentSession.CurrentWindow.Height - 100);
        if (args.AreaName == "FromLibrary")
            this.isFromLibrary = true;
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], PreviewQuoteTemplateReportComponent.prototype, "viewContainerRef", void 0);
    PreviewQuoteTemplateReportComponent = __decorate([
        core_1.Component({
            selector: 'PreviewQuoteTemplateReportComponent',
            moduleId: module.id,
            templateUrl: './PreviewQuoteTemplateReportComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PreviewQuoteTemplateReportComponent);
    return PreviewQuoteTemplateReportComponent;
}());
exports.PreviewQuoteTemplateReportComponent = PreviewQuoteTemplateReportComponent;
//# sourceMappingURL=PreviewQuoteTemplateReportComponent.js.map