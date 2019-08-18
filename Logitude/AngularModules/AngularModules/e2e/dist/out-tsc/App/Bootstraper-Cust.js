"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_dynamic_1 = require("@angular/platform-browser-dynamic");
var Module_APP_CUST_1 = require("./Module_APP_CUST");
var environment_1 = require("../environments/environment");
if (environment_1.environment.production) {
    core_1.enableProdMode();
}
platform_browser_dynamic_1.platformBrowserDynamic().bootstrapModule(Module_APP_CUST_1.AppModule).catch(function (err) { return console.log(err); });
//# sourceMappingURL=Bootstraper-Cust.js.map