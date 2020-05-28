"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CertificateTicket_1 = require("../../../../../Customs/DataContract/CertificateTicket");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var MultiCertificatesService_1 = require("../../../../../Customs/Services/Others/MultiCertificatesService");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var CertificateSelectionComponent = /** @class */ (function (_super) {
    __extends(CertificateSelectionComponent, _super);
    function CertificateSelectionComponent() {
        var _this = _super.call(this) || this;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.connectedItems = [];
        _this.ExcludedItems = [];
        _this.multiCertificatesService = new MultiCertificatesService_1.MultiCertificatesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ThereIsNoCertificates = false;
        return _this;
    }
    CertificateSelectionComponent.prototype.SetWindowArgs = function (args) {
        this.ItemsSource.InsertCollection(args.certificateList);
        if (this.ItemsSource.Length == 0) {
            this.ThereIsNoCertificates = true;
            this.GridHeight = 35;
        }
        this.connectedItems = args.ConnectedItems;
        this.ExcludedItems = args.ExcludedItems;
        this.ticket = args.Ticket;
    };
    CertificateSelectionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("no");
        this.CurrentSession.CloseCurrentWindow();
    };
    CertificateSelectionComponent.prototype.OnRowSelected = function (selectedRow) {
        this.SelectedRow = selectedRow;
    };
    CertificateSelectionComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedRow)) {
            this.CurrentSession.StartBusyIndicator("");
            var certificateTicket = new CertificateTicket_1.CertificateTicket();
            certificateTicket.DeclarationId = this.ticket.DeclarationId;
            certificateTicket.InvoiceNumber = null;
            certificateTicket.AttachmentTypeCode = this.SelectedRow.AttachmentTypeCode;
            certificateTicket.CertificateNumber = this.SelectedRow.CertificateNumber;
            certificateTicket.ResConfirmationTypeCode = this.SelectedRow.ResConfirmationTypeCode;
            certificateTicket.CertificateExemptionTypeCode = this.SelectedRow.CertificateExemptionTypeCode;
            certificateTicket.ReqConfirmationTypeCode = this.SelectedRow.ReqConfirmationTypeCode;
            certificateTicket.CustomsAttachmentId = this.SelectedRow.CustomsAttachmentId;
            certificateTicket.oldAttachment = this.ticket.AttachmentTypeCode;
            certificateTicket.oldCertificateExempt = this.ticket.CertificateExemptionTypeCode;
            certificateTicket.oldCertificateNumber = this.ticket.CertificateNumber;
            certificateTicket.oldResConfirmation = this.ticket.ResConfirmationTypeCode;
            certificateTicket.IsAllSelected = this.ticket.IsAllSelected;
            certificateTicket.SelectedItems = [];
            //if (!certificateTicket.IsAllSelected) {
            certificateTicket.SelectedItems = this.connectedItems;
            //}
            certificateTicket.ConnectedItemsKeys = "";
            certificateTicket.SelectedItems.forEach(function (item) {
                certificateTicket.ConnectedItemsKeys = certificateTicket.ConnectedItemsKeys + "," + item.DeclarationId + ";" + item.InvoiceCounterKey + ";" + item.LineNumber + ";" + item.ItemCertificateCounterKey;
            });
            certificateTicket.ConnectedItemsKeys = certificateTicket.ConnectedItemsKeys.substr(1, certificateTicket.ConnectedItemsKeys.length - 1);
            certificateTicket.ExcludedItemsKeys = "";
            this.ExcludedItems.forEach(function (item) {
                certificateTicket.ExcludedItemsKeys = certificateTicket.ExcludedItemsKeys + "," + item.DeclarationId + ";" + item.InvoiceCounterKey + ";" + item.LineNumber + ";" + item.ItemCertificateCounterKey;
            });
            certificateTicket.ExcludedItemsKeys = certificateTicket.ExcludedItemsKeys.substr(1, certificateTicket.ExcludedItemsKeys.length - 1);
            this.multiCertificatesService.PutCertificateTickets(certificateTicket)
                .subscribe(function (response) {
                if (!response.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    };
    CertificateSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CertificateSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CertificateSelectionComponent);
    return CertificateSelectionComponent;
}(BaseComponent_1.BaseComponent));
exports.CertificateSelectionComponent = CertificateSelectionComponent;
//# sourceMappingURL=CertificateSelectionComponent.js.map