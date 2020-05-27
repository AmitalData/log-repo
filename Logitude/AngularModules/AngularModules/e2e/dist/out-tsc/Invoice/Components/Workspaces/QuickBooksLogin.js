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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuickBooksLogin = /** @class */ (function () {
    function QuickBooksLogin() {
        this.MyPage = "";
        this.theFrame = "theFrame";
        this.source = 'https://appcenter.intuit.com/Connect/Begin?oauth_token=qyprdjn7a5B5J4YiS26DmCe2WjlGQ1kjgujdDoCX7bAft6K9&oauth_callback=http%3A%2F%2Flocalhost%3A65281%2FOauthManager.aspx%3F';
        this.link = "";
        this.onLoadFunc();
        //var win = window.open(link, '_blank');
    }
    QuickBooksLogin.prototype.onLoadFunc = function () {
        this.link = Tools_1.AppTool.GetLogitudeURL() + "Quickbooksonline.aspx?connect=true&tenant=" + SessionLocator_1.SessionLocator.Tenant;
        // var win = window.open(this.link , 'theFrame', "location = 1, status = 1, scrollbars = 1, width = 400, height = 400");
        //   win.focus();
    };
    QuickBooksLogin = __decorate([
        core_1.Component({
            selector: 'QuickBooksLogin',
            moduleId: './Invoice/Components/Workspaces/',
            template: "\n\n<iframe src=\"link\" style=\"width:100%;height:100%\"></iframe>\n\n\n "
        }),
        __metadata("design:paramtypes", [])
    ], QuickBooksLogin);
    return QuickBooksLogin;
}());
exports.QuickBooksLogin = QuickBooksLogin;
//# sourceMappingURL=QuickBooksLogin.js.map