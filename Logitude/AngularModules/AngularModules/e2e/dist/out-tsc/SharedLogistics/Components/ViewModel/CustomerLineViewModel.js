"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ContactPMService_1 = require("../../../Common/Services/StandardPMs/ContactPMService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var CustomerLineViewModel = /** @class */ (function () {
    function CustomerLineViewModel(item, trigger) {
        var _this = this;
        this.IsEnableEditContact = false;
        this.entityPM = item;
        this.Trigger = trigger;
        if (this.contactPMService == null) {
            this.contactPMService = new ContactPMService_1.ContactPMService();
        }
        this.contactPMService.get(this.entityPM.ContactId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.contactPM = myResult;
                    _this.IsEnableEditContact = true;
                }
            }
        });
    }
    Object.defineProperty(CustomerLineViewModel.prototype, "Name", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.Name;
            }
            else
                return "";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "InviteButtonContent", {
        get: function () {
            if (this.InternetAccess) {
                return "Invite again";
            }
            else
                return "Invite";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "InternetAccess", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.InternetAccess;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.InternetAccess = value;
                this.Trigger.SaveChanges(this.entityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "EnglishName", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.EnglishName;
            }
            else
                return "";
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "Email", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.Email;
            }
            else
                return "";
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.Email = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "Position", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.Position;
            }
            else
                return "";
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.Position = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "BusinessPhone", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.BusinessPhone;
            }
            else
                return "";
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.BusinessPhone = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "Mobile", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.Mobile;
            }
            else
                return "";
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.Mobile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "Fax", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.Fax;
            }
            else
                return "";
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.Fax = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerLineViewModel.prototype, "LastLoginDate", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.LastLoginDate;
            }
            else
                return "";
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.LastLoginDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerLineViewModel.prototype.InviteButtonButtonclick = function () {
        if (!this.Email) {
            if (this.Trigger) {
                this.Trigger.ShowMessageWindow("The Contact you want to invite has no email! \nplease fill the email then press invite", "", "#1B90CB", 130);
            }
        }
        else {
            this.InternetAccess = true;
        }
    };
    CustomerLineViewModel.prototype.BlockAccessButtonclick = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Are you sure you want block this contact's access ?");
        confirmWindow.ShowCancelButton = true;
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Contact", "Mobile blocked");
                _this.InternetAccess = false;
            }
        });
    };
    return CustomerLineViewModel;
}());
exports.CustomerLineViewModel = CustomerLineViewModel;
//# sourceMappingURL=CustomerLineViewModel.js.map