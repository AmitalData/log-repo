declare var window: any;
import {AppTool} from '../Tools';
import {FeaturePM} from '../EntityPMs/FeaturePM';
import {SessionLocator} from './SessionLocator';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';

export class FeatureLocator {
    public static DisableRuleValidation: boolean;
    public static DisableRolesSecurity: boolean;
    public static DisableFieldsValidation: boolean;
    public static DisableEntityValidation: boolean;
    public static Features: FeaturePM[] = [];

    public static IsPackageEquals(myPackageCode: string) {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(myPackageCode)) {
            if (SessionLocator.TenantManagementJS != null) {
                var myPackagesCodes: string[] = SessionLocator.TenantManagementJS.PackagesCodes_PK;

                if (myPackagesCodes.length > 0) {
                    var myGroupedList: string[] = [];
                    myPackagesCodes.forEach(item => {
                        if (myGroupedList.indexOf(item) == -1) {
                            myGroupedList.push(item);
                        }
                    });

                    if (SessionLocator.TenantManagementJS.MainAdditionalPackageApplied) {
                        if (myGroupedList.indexOf(myPackageCode) > -1) {
                            myResult = true;
                        }
                    }

                    else {
                        if (myGroupedList.length == 1) {
                            if (myGroupedList[0] == myPackageCode) {
                                myResult = true;
                            }
                        }
                    }
                }
            }
        }

        return myResult;
    }
    public static IsPackageOneOf(myCodes: string[]) {
        var myResult = false;

        if (myCodes.length > 0) {
            if (SessionLocator.TenantManagementJS != null) {
                var myPackagesCodes: string[] = [];

                if (SessionLocator.TenantManagementJS.PackagesCodes_PK.length > 0) {
                    var myGroupedList: string[] = [];

                    SessionLocator.TenantManagementJS.PackagesCodes_PK.forEach(item => {
                        myPackagesCodes.push(item);

                        if (myGroupedList.indexOf(item) == -1) {
                            myGroupedList.push(item);
                        }
                    });

                    myCodes.forEach(item => {
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
    }
    public static IsFeatureGranted(featureId: string) {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(featureId)) {
            if (FeatureLocator.Features != null) {
                var myFeature: FeaturePM = FeatureLocator.Features.filter(d => d.Id == featureId)[0];

                if (myFeature != null) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }
    public static IsFeatureGrantedByCode(featureCode: string) {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(featureCode)) {
            if (FeatureLocator.Features != null) {
                var myFeature: FeaturePM = FeatureLocator.Features.filter(d => d.Code == featureCode)[0];

                if (myFeature != null) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }
    public static IsFeatureGrantedByUniqeCode(featureUniqeCode: string) {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(featureUniqeCode)) {
            if (FeatureLocator.Features != null) {
                var myFeature: FeaturePM = FeatureLocator.Features.filter(d => d.FeatureUniqeCode == featureUniqeCode)[0];

                if (myFeature != null) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }
    public static IsPackage_CUST() {
        var myResult = false;

        if (FeatureLocator.IsPackageEquals("CUST")) {
            myResult = true;
        }

        return myResult;
    }
    public static IsPackage_IMPO() {
        var myResult = false;

        if (FeatureLocator.IsPackageEquals("IMPO")) {
            myResult = true;
        }

        return myResult;
    }
    public static IsPackage_DVMT() {
        var myResult = false;

        if (FeatureLocator.IsPackageEquals("DVMT")) {
            myResult = true;
        }

        return myResult;
    }
    public static IsPackage_EAWB() {
        var myResult = false;

        if (FeatureLocator.IsPackageEquals("EAWB")) {
            myResult = true;
        }

        return myResult;
    }
    public static IsPackage_BUBK() {
        var myResult = false;

        if (FeatureLocator.IsPackageEquals("BUBK")) {
            myResult = true;
        }

        return myResult;
    }

    public static HasEntityPermessions(objectTableName: string, featureCode: string, showwindow: boolean) {
        var haspermession = true;
        var message = "";
        if (window.ObjectTables != null) {
            if (!FeatureLocator.DisableRolesSecurity && SessionLocator.Tenant != 0) {
                var objectTable = window.ObjectTables.filter(o => o.Name == objectTableName && (o.Tenant == SessionLocator.Tenant || o.Tenant == 0))[0];
                if (objectTable != null && objectTable.EnableSecurity) {
                    var readfeature: FeaturePM = FeatureLocator.Features.filter(f => f.FeatureTypeCode == "READ" && f.ObjectTableId == objectTable.Id)[0];
                    var checkedfeature: FeaturePM = FeatureLocator.Features.filter(f => f.FeatureTypeCode == featureCode && f.ObjectTableId == objectTable.Id)[0];
                    var modulefeature: FeaturePM = FeatureLocator.Features.filter(f => f.FeatureTypeCode == "MODL" && f.ObjectTableId == objectTable.Id)[0];
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
    }
    public static ShowNoPermessionWindow(headercode: string, objecttablename: string, messsage: string) {
        var window = new ConfirmWindow();
        window.Width = 450;
        window.Height = 190;
        var str = TextCodeTranslator.Translate(headercode);
        str = str.replace("%Entity", TextCodeTranslator.TranslateTable(objecttablename));
        window.Title = str;
        window.YesButtonText = "Ok";
        window.ShowNoButton = false;
        window.Show(messsage);
    }

    public static HasFeaturePermession(objectTableName: string, featureCode: string) {
        var myResult = true;

        if (window.ObjectTables != null) {
            if (!FeatureLocator.DisableRolesSecurity) {
                if (!AppTool.IsNullOrEmpty(objectTableName) && !AppTool.IsNullOrEmpty(featureCode)) {
                    var objectTable: any = window.ObjectTables.filter(x => x.Name.toLowerCase() === objectTableName.toLowerCase())[0];
                    if (objectTable != null) {
                        var myFeature: FeaturePM = FeatureLocator.Features.filter(f => f.ObjectTableId == objectTable.Id && f.Code.toLowerCase() == featureCode.toLowerCase())[0];

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
    }
    public static HasFeaturePermessionByObjectTableId(myObjectTableId: string, myFeatureCode: string) {
        var myResult = true;

        if (!FeatureLocator.DisableRolesSecurity && SessionLocator.Tenant != 0) {
            if (!AppTool.IsNullOrEmpty(myObjectTableId) && !AppTool.IsNullOrEmpty(myFeatureCode)) {

                var myFeature: FeaturePM = FeatureLocator.Features.filter(f => f.ObjectTableId == myObjectTableId && f.Code.toLowerCase() == myFeatureCode.toLowerCase())[0];

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
    }

}

