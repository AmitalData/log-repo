"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var SessionLocator_1 = require("./SessionLocator");
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../Utilities/TextCodeTranslator");
var FeatureLocator = /** @class */ (function () {
    function FeatureLocator() {
    }
    FeatureLocator.IsPackageEquals = function (myPackageCode) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(myPackageCode)) {
            if (SessionLocator_1.SessionLocator.TenantManagementJS != null) {
                var myPackagesCodes = SessionLocator_1.SessionLocator.TenantManagementJS.PackagesCodes_PK;
                if (myPackagesCodes.length > 0) {
                    var myGroupedList = [];
                    myPackagesCodes.forEach(function (item) {
                        if (myGroupedList.indexOf(item) == -1) {
                            myGroupedList.push(item);
                        }
                    });
                    if (myGroupedList.length == 1) {
                        if (myGroupedList[0] == myPackageCode) {
                            myResult = true;
                        }
                    }
                }
            }
        }
        return myResult;
    };
    FeatureLocator.IsPackageOneOf = function (myCodes) {
        var myResult = false;
        if (myCodes.length > 0) {
            if (SessionLocator_1.SessionLocator.TenantManagementJS != null) {
                var myPackagesCodes = [];
                if (SessionLocator_1.SessionLocator.TenantManagementJS.PackagesCodes_PK.length > 0) {
                    var myGroupedList = [];
                    SessionLocator_1.SessionLocator.TenantManagementJS.PackagesCodes_PK.forEach(function (item) {
                        myPackagesCodes.push(item);
                        if (myGroupedList.indexOf(item) == -1) {
                            myGroupedList.push(item);
                        }
                    });
                    myCodes.forEach(function (item) {
                        var index = myPackagesCodes.indexOf(item);
                        if (index != -1) {
                            myPackagesCodes.splice(index, 1);
                        }
                    });
                    if (myPackagesCodes.length == 0) {
                        myResult = true;
                    }
                }
            }
        }
        return myResult;
    };
    FeatureLocator.IsFeatureGranted = function (featureId) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(featureId)) {
            if (FeatureLocator.Features != null) {
                var myFeature = FeatureLocator.Features.filter(function (d) { return d.Id == featureId; })[0];
                if (myFeature != null) {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    FeatureLocator.IsFeatureGrantedByCode = function (featureCode) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(featureCode)) {
            if (FeatureLocator.Features != null) {
                var myFeature = FeatureLocator.Features.filter(function (d) { return d.Code == featureCode; })[0];
                if (myFeature != null) {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    FeatureLocator.IsPackage_CUST = function () {
        var myResult = false;
        if (FeatureLocator.IsPackageEquals("CUST")) {
            myResult = true;
        }
        return myResult;
    };
    FeatureLocator.IsPackage_IMPO = function () {
        var myResult = false;
        if (FeatureLocator.IsPackageEquals("IMPO")) {
            myResult = true;
        }
        return myResult;
    };
    FeatureLocator.IsPackage_DVMT = function () {
        var myResult = false;
        if (FeatureLocator.IsPackageEquals("DVMT")) {
            myResult = true;
        }
        return myResult;
    };
    FeatureLocator.IsPackage_EAWB = function () {
        var myResult = false;
        if (FeatureLocator.IsPackageEquals("EAWB")) {
            myResult = true;
        }
        return myResult;
    };
    FeatureLocator.IsPackage_BUBK = function () {
        var myResult = false;
        if (FeatureLocator.IsPackageEquals("BUBK")) {
            myResult = true;
        }
        return myResult;
    };
    FeatureLocator.HasEntityPermessions = function (objectTableName, featureCode, showwindow) {
        var haspermession = true;
        var message = "";
        if (window.ObjectTables != null) {
            if (!FeatureLocator.DisableRolesSecurity && SessionLocator_1.SessionLocator.Tenant != 0) {
                var objectTable = window.ObjectTables.filter(function (o) { return o.Name == objectTableName && (o.Tenant == SessionLocator_1.SessionLocator.Tenant || o.Tenant == 0); })[0];
                if (objectTable != null && objectTable.EnableSecurity) {
                    var readfeature = FeatureLocator.Features.filter(function (f) { return f.FeatureTypeCode == "READ" && f.ObjectTableId == objectTable.Id; })[0];
                    var checkedfeature = FeatureLocator.Features.filter(function (f) { return f.FeatureTypeCode == featureCode && f.ObjectTableId == objectTable.Id; })[0];
                    var modulefeature = FeatureLocator.Features.filter(function (f) { return f.FeatureTypeCode == "MODL" && f.ObjectTableId == objectTable.Id; })[0];
                    var windowHeader = "";
                    switch (featureCode) {
                        case "READ":
                            message = "You have no permission to view entities of this type.";
                            break;
                        case "UPDT":
                            windowHeader = "General.O.EditEntity";
                            message = "You have no permission to edit an entity of this type.";
                            break;
                        case "NEW":
                            windowHeader = "General.O.NewEntity";
                            message = "You have no permission to add a new entity of this type.";
                            break;
                    }
                    if (modulefeature != null) {
                        if (checkedfeature == null || readfeature == null) {
                            haspermession = false;
                            if (showwindow) {
                                //message = "You have no permission to add a new entity of this type.";
                                this.ShowNoPermessionWindow(windowHeader, objectTable.Name, message);
                            }
                        }
                    }
                    else {
                        haspermession = false;
                        message = "Your package doesn't include this module..";
                        if (showwindow) {
                            this.ShowNoPermessionWindow("General.O.NewEntity", objectTable.Name, message);
                        }
                    }
                }
            }
        }
        return haspermession;
    };
    FeatureLocator.ShowNoPermessionWindow = function (headercode, objecttablename, messsage) {
        var window = new ConfirmWindow_1.ConfirmWindow();
        window.Width = 450;
        window.Height = 190;
        var str = TextCodeTranslator_1.TextCodeTranslator.Translate(headercode);
        str = str.replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable(objecttablename));
        window.Title = str;
        window.YesButtonText = "Ok";
        window.ShowNoButton = false;
        window.Show(messsage);
    };
    FeatureLocator.HasFeaturePermession = function (objectTableName, featureCode) {
        var myResult = true;
        if (window.ObjectTables != null) {
            if (!FeatureLocator.DisableRolesSecurity) {
                if (!Tools_1.AppTool.IsNullOrEmpty(objectTableName) && !Tools_1.AppTool.IsNullOrEmpty(featureCode)) {
                    var objectTable = window.ObjectTables.filter(function (x) { return x.Name.toLowerCase() === objectTableName.toLowerCase(); })[0];
                    if (objectTable != null) {
                        var myFeature = FeatureLocator.Features.filter(function (f) { return f.ObjectTableId == objectTable.Id && f.Code.toLowerCase() == featureCode.toLowerCase(); })[0];
                        if (myFeature == null) {
                            myResult = false;
                        }
                        else {
                            if (!FeatureLocator.IsFeatureGranted(myFeature.Id)) {
                                myResult = false;
                            }
                        }
                    }
                }
            }
        }
        return myResult;
    };
    FeatureLocator.HasFeaturePermessionByObjectTableId = function (myObjectTableId, myFeatureCode) {
        var myResult = true;
        if (!FeatureLocator.DisableRolesSecurity && SessionLocator_1.SessionLocator.Tenant != 0) {
            if (!Tools_1.AppTool.IsNullOrEmpty(myObjectTableId) && !Tools_1.AppTool.IsNullOrEmpty(myFeatureCode)) {
                var myFeature = FeatureLocator.Features.filter(function (f) { return f.ObjectTableId == myObjectTableId && f.Code.toLowerCase() == myFeatureCode.toLowerCase(); })[0];
                if (myFeature == null) {
                    myResult = false;
                }
                else {
                    if (!FeatureLocator.IsFeatureGranted(myFeature.Id)) {
                        myResult = false;
                    }
                }
            }
        }
        return myResult;
    };
    FeatureLocator.Features = [];
    return FeatureLocator;
}());
exports.FeatureLocator = FeatureLocator;
//# sourceMappingURL=FeatureLocator.js.map