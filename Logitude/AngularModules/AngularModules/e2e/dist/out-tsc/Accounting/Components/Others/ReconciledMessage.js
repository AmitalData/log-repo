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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var ReconciledMessage = /** @class */ (function () {
    function ReconciledMessage() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ReconciledMessage.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.RecoPM = args.ReconciliationPM;
        }
    };
    ReconciledMessage.prototype.OpenReco = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.RecoPM.Id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.RecoPM.Id, ObjectTableName: 'Reconciliation', BackButtonLabel: 'Back' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    _this.CurrentSession.CloseCurrentWindow();
                });
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
    };
    ReconciledMessage.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ReconciledMessage = __decorate([
        core_1.Component({
            selector: 'ReconciledMessage',
            template: "\n    <style>\n    .ConfirmIcon {\n        margin-top: 17px;\n        left: 10px;\n        width: 60px;\n        height: 60px;\n        line-height: 64px;\n        color: white;\n        font-size: 36px;\n        font-weight: bold;\n        font-family: Arial;\n        text-align: center;\n        vertical-align: middle;\n        -webkit-border-radius: 50px;\n        -moz-border-radius: 50px;\n        border-radius: 50px;\n        /* Permalink - use to edit and share this gradient: http://colorzilla.com/gradient-editor/#429b30+0,b8ddb8+100 */\n        background: #429b30; /* Old browsers */\n        background: -moz-linear-gradient(top,  #429b30 0%, #b8ddb8 100%); /* FF3.6-15 */\n        background: -webkit-linear-gradient(top,  #429b30 0%,#b8ddb8 100%); /* Chrome10-25,Safari5.1-6 */\n        background: linear-gradient(to bottom,  #429b30 0%,#b8ddb8 100%); /* W3C, IE10+, FF16+, Chrome26+, Opera12+, Safari7+ */\n        filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#429b30', endColorstr='#b8ddb8',GradientType=0 ); /* IE6-9 */\n\n        }\n    .RedButton{\n        position: absolute;\n        right: 10px;\n        bottom: 10px;\n        width: 65px;\n    }\n    </style>\n\n    <div class=\"LeftCenter ConfirmIcon\" >&#10003;</div>\n\n    <div style= \"padding: 35px 10px 10px 90px;font-size: 13px;\" >\n       Reconciliation <a href= \"#\"(click) = \"OpenReco()\" > {{RecoPM.Number }}</a> was created successfully.\n    </div>\n\n    <!--<button class=\"RedButton\" (click)=\"OkButtonClicked()\">Ok</button>-->\n\n            "
        }),
        __metadata("design:paramtypes", [])
    ], ReconciledMessage);
    return ReconciledMessage;
}());
exports.ReconciledMessage = ReconciledMessage;
//# sourceMappingURL=ReconciledMessage.js.map