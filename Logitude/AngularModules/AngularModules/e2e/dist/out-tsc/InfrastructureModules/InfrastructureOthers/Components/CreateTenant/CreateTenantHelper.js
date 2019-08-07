"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ContactListService_1 = require("../../../../Common/Services/StandardLists/ContactListService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CreateTenantHelper = /** @class */ (function () {
    function CreateTenantHelper(objecttablename, customerId, customerName, accountingCard, primaryContactId, vatNumber, countryName, countryCode) {
        this.ObjecttableName = objecttablename;
        this.AccountingCard = accountingCard;
        this.PrimaryContactId = primaryContactId;
        this.CustomerId = customerId;
        this.VatNumber = vatNumber;
        this.CustomerName = customerName;
        this.CountryName = countryName;
        this.CountryCode = countryCode;
    }
    CreateTenantHelper.prototype.CreateTenantMethod = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AccountingCard)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PrimaryContactId)) {
                var myService = new ContactListService_1.ContactListService();
                myService.getSingle(this.PrimaryContactId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var contactList = myResponse.Result;
                        if (contactList != null) {
                            var Msg = "";
                            if (_this.ObjecttableName == "Customer") {
                                if ((ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator_1.ObjectsLocator.GlobalSetting.DeploymentStage == "Test2")) {
                                    if (Tools_1.AppTool.IsNullOrEmpty(contactList.BusinessPhone) && Tools_1.AppTool.IsNullOrEmpty(contactList.Mobile)) {
                                        Msg = "Business Phone Field Or Mobile Phone Field is Required";
                                    }
                                    else if (Tools_1.AppTool.IsNullOrEmpty(_this.CustomerName))
                                        Msg = "Customer Name is Required";
                                    else if (Tools_1.AppTool.IsNullOrEmpty(_this.VatNumber))
                                        Msg = "VatNumber is Required";
                                    if (!Tools_1.AppTool.IsNullOrEmpty(Msg)) {
                                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                        var windowArgs = {};
                                        windowArgs.MessageError = Msg;
                                        logWindow.WindowArgs = windowArgs;
                                        logWindow.Width = 510;
                                        logWindow.Height = 340;
                                        logWindow.Title = "Validation";
                                        logWindow.Show("./InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantValidationScreenComponent");
                                    }
                                }
                            }
                            if (Tools_1.AppTool.IsNullOrEmpty(Msg)) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(contactList.Email) && !Tools_1.AppTool.IsNullOrEmpty(contactList.EnglishName) && !Tools_1.AppTool.IsNullOrEmpty(_this.CustomerName)) {
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    var windowArgs = {};
                                    windowArgs.CustomerId = _this.CustomerId;
                                    windowArgs.Email = contactList.Email;
                                    windowArgs.ContactName = contactList.EnglishName;
                                    windowArgs.CustomerName = _this.CustomerName;
                                    windowArgs.Phone = contactList.Mobile;
                                    windowArgs.CountryName = _this.CountryName;
                                    windowArgs.CountryCode = _this.CountryCode;
                                    windowArgs.ObjecttableName = _this.ObjecttableName;
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.Width = 400;
                                    logWindow.Height = 120;
                                    logWindow.Title = "Package Type";
                                    logWindow.Show("./InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantPackageSelectionComponent");
                                }
                                else
                                    _this.FillErrorsList(contactList.Email, contactList.EnglishName);
                            }
                        }
                        else
                            _this.FillErrorsList();
                    }
                });
            }
            else
                this.FillErrorsList();
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            var validationErrorMessage = "note that this customer has a tenant already " + this.AccountingCard + ", please erase the tenant# in order to create a new one" + " (" + this.AccountingCard + " = External ID)";
            messageWindow.Show(validationErrorMessage);
        }
    };
    CreateTenantHelper.prototype.FillErrorsList = function (email, contactname) {
        if (email === void 0) { email = ""; }
        if (contactname === void 0) { contactname = ""; }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        windowArgs.ContactEmail = email;
        windowArgs.ContactName = contactname;
        windowArgs.CustomerName = this.CustomerName;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 510;
        logWindow.Height = 340;
        logWindow.Title = "Validation";
        logWindow.Show("./InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantValidationScreenComponent");
    };
    return CreateTenantHelper;
}());
exports.CreateTenantHelper = CreateTenantHelper;
//# sourceMappingURL=CreateTenantHelper.js.map