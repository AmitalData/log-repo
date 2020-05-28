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
var http_1 = require("@angular/http");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var AmitalGatewayUtil_1 = require("../../Utilities/AmitalGatewayUtil");
var TotangoService = /** @class */ (function () {
    function TotangoService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TotangoService';
    }
    TotangoService.prototype.SendTotangoUserActivity = function (module, activity) {
        try {
            //if (ScriptableGatewayUtil.AmitalBrowserInUse) {
            //    var requset = new Dictionary<string, string>();
            //    requset.Add("module", module);
            //    requset.Add("activity", activity);
            //    ScriptableGatewayUtil.SendRequestToUnifreightAsync("", "", "SendTotangoUserActivity", new UnifreightMessageM() { Requset = requset }, "");
            //}
            if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.SendTotangoUserActivity(module, activity);
            }
            if (!SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                if (module == "Agent" || module == "CustomAgent" || module == "ShippingAgent" || module == "Customer" || module == "PotentialCustomer"
                    || module == "Airline" || module == "Trucker" || module == "Shippingline") {
                    module = "Card";
                }
                if (module == null) {
                }
                //string uri = App.Current.Host.Source.AbsoluteUri;
                //uri = uri.Replace("/ClientBin/Simplog.Infrastructure.xap", "/WebServices/TotangoService.asmx");
                //TotangoServiceSoapClient totangoService = new TotangoServiceSoapClient();
                //totangoService.Endpoint.Address = new System.ServiceModel.EndpointAddress(uri);
                //BasicHttpBinding binding = BindingInfo.GetBindingInfo();
                //if (totangoService.Endpoint.Address.Uri.Scheme == "https") {
                //    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                //}
                //else {
                //    binding.Security.Mode = BasicHttpSecurityMode.None;
                //}
                //totangoService.Endpoint.Binding = binding;
                //(string organizationId, string orgDisplayName, string userName, string module, string activity, string contactId, int tenant, bool isSharedLogisticsContact, string cardId, string partnerTypeId)
                var data = new TotangoActivityInfo();
                data.UserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                data.Module = "(A) " + module;
                data.Activity = "(A) " + activity;
                data.ContactId = SessionLocator_1.SessionLocator.LoggedUserId;
                data.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
                data.IsSharedLogisticsContact = false;
                var orgDisplayName = SessionLocator_1.SessionLocator.TenantPM.Company + (SessionLocator_1.SessionLocator.TenantPM.CountryName != null ? ("-" + SessionLocator_1.SessionLocator.TenantPM.CountryName.trim()) : "");
                if (SessionLocator_1.SessionLocator.TenantPM.Id == 65 || SessionLocator_1.SessionLocator.TenantPM.Id == 153) {
                    orgDisplayName = SessionLocator_1.SessionLocator.LoggedUserPM.Notes;
                    data.OrganizationId = SessionLocator_1.SessionLocator.LoggedUserId;
                    //totangoService.SendUserActivityAsync(SessionInfo.LoggedUserId, orgDisplayName, SessionInfo.LoggedUserPM.EnglishName, module, activity, SessionInfo.LoggedUserId, InfraSettings.TenantPM.Id, false, null, null);
                }
                else {
                    data.OrganizationId = SessionLocator_1.SessionLocator.TenantPM.Id.toString();
                    //totangoService.SendUserActivityAsync(InfraSettings.TenantPM.Id.ToString(), orgDisplayName, SessionInfo.LoggedUserPM.EnglishName, module, activity, SessionInfo.LoggedUserId, InfraSettings.TenantPM.Id, false, null, null);
                }
                data.OrgDisplayName = orgDisplayName;
                var authHeader = new http_1.Headers();
                authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
                authHeader.append('Content-Type', 'application/json');
                this._http.post(this._apiUrl, JSON.stringify(data), { headers: authHeader }).subscribe(function (response) {
                }, function (error) {
                    console.error(error);
                });
            }
        }
        catch (e) {
            console.error(e);
        }
    };
    TotangoService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], TotangoService);
    return TotangoService;
}());
exports.TotangoService = TotangoService;
var TotangoActivityInfo = /** @class */ (function () {
    function TotangoActivityInfo() {
    }
    return TotangoActivityInfo;
}());
exports.TotangoActivityInfo = TotangoActivityInfo;
//# sourceMappingURL=TotangoService.js.map