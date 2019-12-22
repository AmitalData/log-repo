import {Component} from '@angular/core';
import {GeneralDomainService, FieldsTranslations} from '../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {TextCodePM} from '../../../../Infrastructure/EntityPMs/TextCodePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CustomPickListList} from '../../../../Infrastructure/EntityLists/CustomPickListList';
import {CustomPickListListService} from '../../../../Infrastructure/Services/StandardLists/CustomPickListListService';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {ObjectFieldPMService} from '../../../../Infrastructure/Services/StandardPMs/ObjectFieldPMService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CachedDataManager} from '../../../../Infrastructure/Utilities/CachedDataManager';
import {LoginService} from '../../../../Infrastructure/Services/LoginService';
import {Headers} from '@angular/http';

declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './AddEditCustomFieldComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class AddEditCustomFieldComponent extends BaseComponent {

    DataContext: AddEditCustomFieldComponent = this;
    private myService: GeneralDomainService;
    private _customPickListListService = new CustomPickListListService();
    private _ObjectFieldPMService = new ObjectFieldPMService();
    ValidationErrorsList: any[];
    DataTypeCollection: any[];
    LookUpTables: any[];
    IsNew: boolean = true;
    objectField: ObjectFieldPM;
    ContolFieldsList1: any[];
    ContolFieldsList2: any[];
    CustomPickListsList: string[];
    loginService: LoginService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.loginService = new LoginService();
        this.ContolFieldsList1 = [];
        this.ContolFieldsList2 = [];
        this.CustomPickListsList = [];
        this.LookUpTables = window.ObjectTables.filter(o => o.IsLookUp && !AppTool.IsNullOrEmpty(o.LookUp1));
        var picklistslist = [];
        this._customPickListListService.getAll().subscribe(response => {
            var temp = response.Result.filter(p => p.Tenant == SessionLocator.Tenant);
            var list = new GroupByPipe().transform(temp, "Code");
            list.forEach((value, key) => {
                this.CustomPickListsList.push(value.key);
            });

            //this.TenantCustomPickLists = response.Result;
            //this.CurrentSession.StopBusyIndicator();
            //if (this.TenantCustomPickLists != null) {
            //    this.TenantCustomPickLists.forEach(p => {
            //        if (this.CustomPickLists.indexOf(p.Code) === - 1) {
            //            this.CustomPickLists.push(p.Code);
            //        }
            //    });
            //} 
        });
        //IEnumerable < IGrouping < string, CustomPickListList >> list = CustomPickListDataProvider.GetCachedList<CustomPickListList>().Where(t => t.Tenant == TenantContext.Current.Id).GroupBy(f => f.Code);
        //foreach(IGrouping < string, CustomPickListList > group in list)
        //{
        //    picklistslist.Add(group.Key);
        //}

        //CustomPickListsList = new ObservableCollection<string>(picklistslist);
        this.UIProperties.SetRequired("Code", "ObjectField", true);
    }

    SetWindowArgs(args: any) {
        //this.ShipmentList = args.SelectedShipment;
        this.IsNew = args.IsNew;
        this.objectField = args.objectField;
        this.DataTypeCollection = args.DataTypeCollection;
        if (this.IsNew) {

        }
        else {
            this.DataTypeSelectionMethod({ Code: this.objectField.DataTypeCode });
            this.LookUpTablesSelectionMethod("");
            this.PickListSelectionMethod(this.objectField.CustomPickListCode);
            this.UIProperties.SetEnabled("Code", "ObjectField", false);
        }
    }
    public get CustomFieldDataType() {
        var fieldDataType = this.DataTypeCollection.filter(d => d.Code == this.objectField.DataTypeCode)[0];
        return fieldDataType;
    }
    public set CustomFieldDataType(newValue: any) {
        this.objectField.DataTypeCode = newValue.Code;
    }

    public get FieldLable() {
        if (this.IsNew) {
            return this.objectField.FullNameTextCodeCode;
        }

        else {
            return this.objectField.FullNameTextCodeDefaultText;
        }
    }


    public set FieldLable(newValue: string) {
        if (this.IsNew) {
            this.objectField.FullNameTextCodeCode = newValue;
            this.objectField.ListTextCodeCode = newValue;

            //if (AppTool.IsNullOrEmpty(this.Code) && !AppTool.IsNullOrEmpty(newValue)) {
            this.Code = AppTool.Replace(newValue, " ", "");
            //}
        }

        else {
            this.objectField.FullNameTextCodeDefaultText = newValue;
            this.objectField.ListTextCodeDefaultText = newValue;
        }
    }
    //private code: string;
    public get Code() {
        if (this.IsNew) {
            return this.objectField.Code;
        }

        else {
            return this.objectField.Code;
        }
    }


    public set Code(newValue: string) {
        if (newValue != this.objectField.Code) {
            this.objectField.Code = newValue;
        }
    }

    public get HelpText() {
        if (!this.IsNew) {
            if (this.objectField.HelpTextCodeDefaultText != null) {
                return this.objectField.HelpTextCodeDefaultText;
            }

            else {
                return this.objectField.HelpTextCodeCode;
            }
        }

        return this.objectField.HelpTextCodeCode;
    }
    public set HelpText(value: string) {
        if (!this.IsNew) {
            if (this.objectField.HelpTextCodeDefaultText != null) {
                this.objectField.HelpTextCodeDefaultText = value;
            }

            else {
                this.objectField.HelpTextCodeCode = value;
            }
        }

        else {
            this.objectField.HelpTextCodeCode = value;
        }
    }

    public get LookUpTableId() {
        return this.objectField.LookUpTableId;
    }
    public set LookUpTableId(value: string) {
        if (value) {
            this.objectField.LookUpTableId = value;
        }
    }
    lookUpTable: any;
    public get LookUpTable() {
        return this.lookUpTable;
    }
    public set LookUpTable(value: any) {
        this.lookUpTable = value;
    }

    public get MinLength() {
        return this.objectField.MinLength;
    }
    public set MinLength(value: number) {
        this.objectField.MinLength = value;
    }

    public get MaxLength() {
        return this.objectField.MaxLength;
    }
    public set MaxLength(value: number) {
        this.objectField.MaxLength = value;
    }

    public get IsMultiline() {
        return this.objectField.MultiLine;
    }
    public set IsMultiline(value: boolean) {
        this.objectField.MultiLine = value;
    }

    PickListItem: string;
    //public get PickListItem() {
    //    if (this.objectField.DataTypeCode == "PickList") {
    //        if (!AppTool.IsNullOrEmpty(this.objectField.CustomPickListCode)) {
    //            this._customPickListListService.getAll().subscribe(response => {
    //                var temp = response.Result.filter(p => p.Code == this.objectField.CustomPickListCode);
    //                if (temp.length > 0) {
    //                    this.pickListItem = temp[0].Code;
    //                }
    //                //this.TenantCustomPickLists = response.Result;
    //                //this.CurrentSession.StopBusyIndicator();
    //                //if (this.TenantCustomPickLists != null) {
    //                //    this.TenantCustomPickLists.forEach(p => {
    //                //        if (this.CustomPickLists.indexOf(p.Code) === - 1) {
    //                //            this.CustomPickLists.push(p.Code);
    //                //        }
    //                //    });
    //                //} 
    //            }); 
    //        }
    //    }
    //    return this.pickListItem;
    //}
    //public set PickListItem(value: string) {
    //    if (value != null) {
    //        this.objectField.CustomPickListCode = value;
    //    }
    //    else {
    //        this.objectField.CustomPickListCode = null;
    //    }
    //} 

    PickListSelectionMethod(item) {
        if (this.objectField.DataTypeCode == "PickList") {
            if (!AppTool.IsNullOrEmpty(this.objectField.CustomPickListCode)) {
                this._customPickListListService.getAll().subscribe(response => {
                    var temp = response.Result.filter(p => p.Code == this.objectField.CustomPickListCode);
                    if (temp.length > 0) {
                        this.PickListItem = temp[0].Code;
                    }
                });
            }
            this.objectField.CustomPickListCode = item;
        }
    }

    controlField1: ObjectFieldPM;
    public get ControlField1() {
        if (this.objectField.ControlField1 != null) {
            this.controlField1 = this.ContolFieldsList1.filter(f => f.FieldName == this.objectField.ControlField1)[0];
        }
        return this.controlField1;
    }
    public set ControlField1(value: ObjectFieldPM) {
        this.controlField1 = value;
        if (this.controlField1 != null) {

            this.objectField.ControlField1 = this.controlField1.FieldName;
        }
        else {
            this.objectField.ControlField1 = null;
        }
    }
    ContolFieldsList1SelectionMethod(item) {
        this.objectField.ControlField1 = item.FieldName;
    }

    controlField2: ObjectFieldPM;
    public get ControlField2() {
        if (this.objectField.ControlField2 != null) {
            this.controlField2 = this.ContolFieldsList2.filter(f => f.FieldName == this.objectField.ControlField2)[0];
        }
        return this.controlField2;
    }
    public set ControlField2(value: ObjectFieldPM) {
        this.controlField2 = value;
        if (this.controlField2 != null) {

            this.objectField.ControlField2 = this.controlField2.FieldName;
        }
        else {
            this.objectField.ControlField2 = null;
        }
    }

    ContolFieldsList2SelectionMethod(item) {
        this.objectField.ControlField2 = item.FieldName;
    }



    PickListVisibile: boolean = false;
    ControlField1Visibile: boolean = false;
    ControlField2Visibile: boolean = false;

    onIsMultilineChange(event) {
        this.IsMultiline = event;
    }

    LookUpTablesSelectionMethod(item) {
        this.ContolFieldsList1 = [];
        this.ContolFieldsList2 = [];
        
        if (!AppTool.IsNullOrEmpty(this.LookUpTableId)) {
            this.LookUpTableId = item.Id;
            var lookupTable: ObjectTablePM = window.ObjectTables.filter(t => t.Id == this.LookUpTableId)[0];
            this.LookUpTable = lookupTable;
            if (lookupTable != null) {
                this.ContolFieldsList1 = window.ObjectFields.filter(f => f.FieldName == lookupTable.DependencyFilter1 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.objectField.ObjectTableId);
                this.ContolFieldsList2 = window.ObjectFields.filter(f => f => f.FieldName == lookupTable.DependencyFilter2 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.objectField.ObjectTableId);

            }
        }
        else {
            this.LookUpTableId = item.Id;
            var lookupTable: ObjectTablePM = window.ObjectTables.filter(t => t.Id == item.Id)[0];
            this.LookUpTable = lookupTable;
            if (lookupTable != null) {
                this.ContolFieldsList1 = window.ObjectFields.filter(f => f.FieldName == lookupTable.DependencyFilter1 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.objectField.ObjectTableId);
                this.ContolFieldsList2 = window.ObjectFields.filter(f => f => f.FieldName == lookupTable.DependencyFilter2 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.objectField.ObjectTableId);

            }
        }
    }

    DataTypeSelectionMethod(fieldDataType: any) {
        if (fieldDataType == null) {

        }

        else {
            this.objectField.DataTypeCode = fieldDataType.Code;
            switch (fieldDataType.Code) {
                case "LookUp":
                    {
                        this.ControlField1Visibile = true;
                        this.ControlField2Visibile = true;
                        this.PickListVisibile = false;
                        break;
                    }
                case "nText":
                    {
                        this.ControlField1Visibile = false;
                        this.ControlField2Visibile = false;
                        this.PickListVisibile = false;
                        this.LookUpTableId = null;
                        this.objectField.ControlField1 = null;
                        this.objectField.ControlField2 = null;
                        break;
                    }

                case "Text":
                    {
                        this.ControlField1Visibile = false;
                        this.ControlField2Visibile = false;
                        this.PickListVisibile = false;
                        this.LookUpTableId = null;
                        this.objectField.ControlField1 = null;
                        this.objectField.ControlField2 = null;
                        break;
                    }
                case "PickList":
                    {
                        this.ControlField1Visibile = false;
                        this.ControlField2Visibile = false;
                        this.PickListVisibile = true;
                        break;
                    }

                default:
                    {
                        this.ControlField1Visibile = false;
                        this.ControlField2Visibile = false;
                        this.PickListVisibile = false;
                        this.objectField.ControlField1 = null;
                        this.objectField.ControlField2 = null;
                        this.LookUpTableId = null;
                        break;
                    }
            }
        }

        //FirePropertyChanged("DataTypeVisibility_LookUp");
        //FirePropertyChanged("DataTypeVisibility_Text");

        if (fieldDataType.Code == "LookUp") {

        }
        else {

        }
    }

    public authHeader;
    SaveChanges() {

        this.ValidationErrorsList = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty(this.objectField.DataTypeCode)) {
            this.ValidationErrorsList.push("Data Type is Required");
        }

        if (AppTool.IsNullOrEmpty(this.Code)) {
            this.ValidationErrorsList.push("Code is Required");
        }

        if (AppTool.IsNullOrEmpty(this.FieldLable)) {
            this.ValidationErrorsList.push("Field Label is Required");
        }

        if (this.objectField.DataTypeCode == "LookUp" && this.objectField.LookUpTableId == null) {
            this.ValidationErrorsList.push("LookUp table is Required");
        }

        if ((this.objectField.DataTypeCode == "Text" || this.objectField.DataTypeCode == "nText") && this.objectField.MaxLength > 2000) {
            this.ValidationErrorsList.push("Maximum length of the text is 2000");
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.authHeader = new Headers();
            this.authHeader.append('Content-Type', 'application/json');
            this.authHeader.append('Accept', 'application/json');
            this.loginService.AuthHeader = this.authHeader;
            this.loginService.CurrentTenant = SessionLocator.Tenant;
            if (this.IsNew == true) {
                this._ObjectFieldPMService.insert(this.objectField).subscribe(Fieldresponse => {
                    if (Fieldresponse.HasError) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.ValidationErrorsList = Fieldresponse.ErrorsArray;
                    }
                    else {
                        CachedDataManager.RefreshTenantTextCodes().subscribe(response => {
                            var item = Fieldresponse.Result;
                            var oldItem = window.ObjectFields.filter(t => t.Id == item.Id)[0];
                            if (oldItem) {
                                var index = window.ObjectFields.indexOf(oldItem);
                                window.ObjectFields.splice(index, 1);
                            }
                            window.ObjectFields.push(item);
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            this.CurrentSession.CloseCurrentWindow();
                            //    this.loginService.GetObjectFields().subscribe(myResult => {
                            //        if (myResult != null) { 
                            //            window.ObjectFields = myResult;
                            //            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            //            this.CurrentSession.CloseCurrentWindow();
                            //        }
                            //    });  
                            //});
                        });
                    }
                });
            }
            else {
                this._ObjectFieldPMService.update(this.objectField).subscribe(Fieldresponse => {
                    if (Fieldresponse.HasError) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.ValidationErrorsList = Fieldresponse.ErrorsArray;
                    }
                    else {
                        CachedDataManager.RefreshTenantTextCodes().subscribe(response => {
                            var item = Fieldresponse.Result;
                            var oldItem = window.ObjectFields.filter(t => t.Id == item.Id)[0];
                            if (oldItem) {
                                var index = window.ObjectFields.indexOf(oldItem);
                                window.ObjectFields.splice(index, 1);
                            }
                            window.ObjectFields.push(item);
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            this.CurrentSession.CloseCurrentWindow();
                            //this.loginService.GetObjectFields().subscribe(myResult => {
                            //    if (myResult != null) {
                            //        window.ObjectFields = myResult;
                            //        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            //        this.CurrentSession.CloseCurrentWindow();
                            //    }
                            //});
                        });

                    }
                });
            }
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    EditPickListButtonClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add Custom Pick List";
        var windowArgs: any = {};
        windowArgs.isNew = false;
        windowArgs.Code = this.objectField.CustomPickListCode;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditPickListComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            this._customPickListListService.getAll().subscribe(response => {
                var temp = response.Result.filter(p => p.Tenant == SessionLocator.Tenant);
                var list = new GroupByPipe().transform(temp, "Code");
                this.CustomPickListsList = [];
                list.forEach((value, key) => {
                    this.CustomPickListsList.push(value.key);
                });
            });
        });
    }

    AddPickListButtonClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add Custom Pick List";
        var windowArgs: any = {};
        windowArgs.isNew = true;
        windowArgs.PickListCode = null;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditPickListComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            this._customPickListListService.getAll().subscribe(response => {
                var temp = response.Result.filter(p => p.Tenant == SessionLocator.Tenant);
                var list = new GroupByPipe().transform(temp, "Code");
                this.CustomPickListsList = [];
                list.forEach((value, key) => {
                    this.CustomPickListsList.push(value.key);
                });
            });
        });
    }
}
