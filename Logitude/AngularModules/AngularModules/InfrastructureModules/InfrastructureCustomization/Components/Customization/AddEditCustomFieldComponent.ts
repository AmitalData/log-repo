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
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObjectFieldPMExtendedService } from '../../../../Infrastructure/Services/ExtendedPMs/ObjectFieldPMExtendedService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';


declare var window: any;

@Component({
    
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
    DefaultAdditionalTreeFilters: any;
    ShownAdditionalFilters: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    EntityResourceService: EntityResourceService = new EntityResourceService();
    AdditionalFiltersData: any = [];
    ShownAdditionalFiltersSettings: boolean;
    ObjectTableName: string;
    private objectFieldPMExtendedService = new ObjectFieldPMExtendedService();
    public IsPartner: boolean = false;
    public PartnersDictionary: { [key: string]: any } = {};
    public partnerTypes: string[];
    constructor() {
        super();
        this.loginService = new LoginService();
        this.ContolFieldsList1 = [];
        this.ContolFieldsList2 = [];
        this.CustomPickListsList = [];
        this.LookUpTables = window.ObjectTables.filter(o => o.IsLookUp && !AppTool.IsNullOrEmpty(o.LookUp1));
        var picklistslist = [];
        this._customPickListListService.getAllFromCache().subscribe((response:any) => {
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
        this.CurrentSession.SessionEvent.subscribe((res) => {
            if (res.Name == "RefreshAdditionalFiltersData") {
                this.AdditionalFiltersData = res.Value;
            }
        });
        this.UIProperties.SetRequired("Code", "ObjectField", true);
    }
    SetCheckedPartners() {
        if (this.IsNew)
            return;
        let partnersHaveThisField = this.GetPartnersHaveThisField();
        this.CheckPartners(partnersHaveThisField);
    }
    CheckPartners(partners: any) {
        partners.forEach((partner) => {
            this.PartnersDictionary[partner] = 1;
        });
    }
    GetPartnersHaveThisField() : any {
        let fieldCopies: any[] = window.ObjectFields.filter(field => this.IsFieldCopy(field));
        let PartnersName = Array<string>();
        fieldCopies.forEach((element) => { PartnersName.push(element.ObjectTableName) });
        return PartnersName;
    }
    IsFieldCopy(field: any) {
        return (field.FieldName == this.objectField.FieldName) && (this.partnerTypes.indexOf(field.ObjectTableName) > -1);
    }

    InitializeDictionary() {
        this.partnerTypes.forEach((partnerName) => {
            this.PartnersDictionary[partnerName] = 0;
        });
    }
    IsCardObjectTable() {
        if (this.ObjectTableName == "Card")
            return true;
        return false;
    }

    ShowAdditionalFiltersSettingsClicked() {
        this.ShownAdditionalFiltersSettings = !this.ShownAdditionalFiltersSettings;
    }

    SetWindowArgs(args: any) {
        this.InitLookUpTables();
        this.IsNew = args.IsNew;
        this.objectField = args.objectField;
        this.LookUpTableId = this.objectField.LookUpTableId;
        this.DataTypeCollection = args.DataTypeCollection.filter(dataType => dataType.Code != "Time");
        this.ObjectTableName = args.ObjectTableName;
        this.IsPartner = args.IsPartner;
        this.partnerTypes = ["AccountingPartner", "Agent", "Airline", "CustomClearance", "CustomAgent", "CustomsShipper", "Coloader", "Customer", "Freelancer", "PotentialCustomer", "Participant",
            "ShippingAgent", "ShippingLine", "Trucker", "Vendor", "Warehouse"];
        this.InitializeDictionary();
        if (this.IsNew) {
            
        }
        else {
            this.DataTypeSelectionMethod({ Code: this.objectField.DataTypeCode });
            //this.LookUpTablesSelectionMethod("");
            this.PickListSelectionMethod(this.objectField.CustomPickListCode);
            this.UIProperties.SetEnabled("Code", "ObjectField", false);
            this.SetCheckedPartners();
        }
    }

    LookUpTablesFilterItems: ApiQueryFilters;
    InitLookUpTables() {
        this.LookUpTablesFilterItems = new ApiQueryFilters();
        this.LookUpTablesFilterItems.addAdditionalFilter("IsLookUp", true, null, null, "Equals", false, false, false, "boolean");
        this.LookUpTablesFilterItems.addAdditionalFilter("LookUp1", "", null, null, "IsNotNull", false, false, false, "string");
        this.LookUpTablesFilterItems.addAdditionalFilter("ClientModuleName", "", null, null, "IsNotNull", false, false, false, "string");
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
        if (!AppTool.IsNullOrEmpty(value) && !AppTool.IsNullOrEmpty(this.LookUpTableId) && this.LookUpTableId != value) {
            this.AdditionalFiltersData = [];
        }

        if (value) {
            this.objectField.LookUpTableId = value;
        }
        else {
            this.lookUpTableName = "";
        }
    }


    public get DisplayOnly() {
        return this.objectField.DisplayOnly;
    }
    public set DisplayOnly(value: boolean) {
        this.objectField.DisplayOnly = value;

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

    public get NumberOfDigits() {
        return this.objectField.NumberOfDigits;
    }
    public set NumberOfDigits(value: number) {
        this.objectField.NumberOfDigits = value;
    }

    public get DigitsAfterPoint() {
        return this.objectField.DigitsAfterPoint;
    }
    public set DigitsAfterPoint(value: number) {
        this.objectField.DigitsAfterPoint = value;
    }

    public get IsMultiline() {
        return this.objectField.MultiLine;
    }
    public set IsMultiline(value: boolean) {
        this.objectField.MultiLine = value;
    }
    public get IsRequired() {
        return this.objectField.IsRequiered;
    }
    public set IsRequired(value: boolean) {
        this.objectField.IsRequiered = value;
    }
    PickListItem: string;
    //public get PickListItem() {
    //    if (this.objectField.DataTypeCode == "PickList") {
    //        if (!AppTool.IsNullOrEmpty(this.objectField.CustomPickListCode)) {
    //            this._customPickListListService.getAll().subscribe((response:any) => {
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
                this._customPickListListService.getAllFromCache().subscribe((response:any) => {
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

    onIsRequiredChange(event) {
        this.IsRequired = event;
    }

    lookUpTableName: string;
    public get LookUpTableName() { return this.lookUpTableName; }
    public set LookUpTableName(newValue: string) {
        this.lookUpTableName = newValue;
    }

    LookUpTablesSelectionMethod(item) {
        if (!item) {
            this.LookUpTableId = "";
            return;
        }
        this.ContolFieldsList1 = [];
        this.ContolFieldsList2 = [];
        this.LookUpTableName = "";
        
        if (!AppTool.IsNullOrEmpty(this.LookUpTableId)) {
            this.LookUpTableId = item.Id;
            var lookupTable: ObjectTablePM = window.ObjectTables.filter(t => t.Id == this.LookUpTableId)[0];
            this.LookUpTable = lookupTable;
            if (lookupTable != null) {
                this.ContolFieldsList1 = window.ObjectFields.filter(f => f.FieldName == lookupTable.DependencyFilter1 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.objectField.ObjectTableId);
                this.ContolFieldsList2 = window.ObjectFields.filter(f => f => f.FieldName == lookupTable.DependencyFilter2 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.objectField.ObjectTableId);
                this.LookUpTableName = lookupTable.Name;
            }
        }
        else {
            this.LookUpTableId = item.Id;
            var lookupTable: ObjectTablePM = window.ObjectTables.filter(t => t.Id == item.Id)[0];
            this.LookUpTable = lookupTable;
            if (lookupTable != null) {
                this.ContolFieldsList1 = window.ObjectFields.filter(f => f.FieldName == lookupTable.DependencyFilter1 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.objectField.ObjectTableId);
                this.ContolFieldsList2 = window.ObjectFields.filter(f => f => f.FieldName == lookupTable.DependencyFilter2 && f.DataTypeCode == "LookUp" && f.ObjectTableId == this.objectField.ObjectTableId);
                this.LookUpTableName = lookupTable.Name;
            }
        }
    }

    DataTypeSelectionMethod(fieldDataType: any) {
        if (fieldDataType == null) {

        }

        else {
            this.objectField.DataTypeCode = fieldDataType.Code;
            this.ShownAdditionalFilters = false;
            switch (fieldDataType.Code) {
                case "LookUp":
                    {
                        this.ControlField1Visibile = true;
                        this.ControlField2Visibile = true;
                        this.PickListVisibile = false;
                        this.ShowAdditionalFilters();
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
        if (fieldDataType.Code == "Decimal" || fieldDataType.Code == "Text" || fieldDataType.Code == "nText") {
            this.SetCustomFieldsLengths();
        }

    }

    ShowAdditionalFilters() {
        this.LoadDefaultAdditionalFilters();
    }

    LoadDefaultAdditionalFilters() {
        this.LookUpTableName = this.LookUpTableName ? this.LookUpTableName : window.ObjectTables.filter(t => t.Id == this.LookUpTableId)[0]?.Name;
        //if (this.LookUpTableName) {
        //    this.LoadLookUpTableResources();
        //}
        //else {
            this.StartLoadDefaultAdditionalFilters();
        //}
    }

    LoadLookUpTableResources() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
        this.EntityResourceService.getEntityResourceByTableName(this.LookUpTableName).subscribe((response: any) => {
            //this.CurrentSession.StopBusyIndicator();
            this.StartLoadDefaultAdditionalFilters();
        }); 
    }

    StartLoadDefaultAdditionalFilters() {
        if (!this.objectField || !this.objectField.Id || (this.AdditionalFiltersData && this.AdditionalFiltersData.length == 1)) {
            this.ShownAdditionalFilters = true;
            return;
        }


        if (!this.LookUpTableId) return;
        if (!this.LookUpTableName) return;
        this.objectFieldPMExtendedService.GetDefaultAdditionalFiltersById(this.objectField.Id, this.objectField.Tenant).subscribe((serviceResponse: any) => {
            var result: ServiceResponse = serviceResponse;
            if (!result.HasError) {
                this.FillAdditionalFiltersData(result);
            }
        });
    }

    private FillAdditionalFiltersData(result: ServiceResponse) {
        let additionalFiltersData = [];
        if (result.Result) {
            additionalFiltersData.push(result.Result);
        }
        this.AdditionalFiltersData = additionalFiltersData;
        this.ShownAdditionalFilters = true;
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

        if (this.objectField.DataTypeCode == "LookUp" && (AppTool.IsNullOrEmpty(this.objectField.LookUpTableId) || AppTool.IsNullOrEmpty(this.LookUpTableName))) {
            this.ValidationErrorsList.push("LookUp table is Required");
        }

        this.ValidateObjectFields();

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.authHeader = new Headers();
            this.authHeader.append('Content-Type', 'application/json');
            this.authHeader.append('Accept', 'application/json');
            this.loginService.AuthHeader = this.authHeader;
            this.loginService.CurrentTenant = SessionLocator.Tenant;
            this.objectField.DefaultAdditionalTreeFilters = this.AdditionalFiltersData[0];
            if (this.IsNew == true) {
                if (this.IsCardTable(this.objectField.ObjectTableId)) {
                    this.objectField.RelatedEntities = this.GetRelatedEntities();
                }
                this._ObjectFieldPMService.insert(this.objectField).subscribe(Fieldresponse => {
                    if (Fieldresponse.HasError) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.ValidationErrorsList = Fieldresponse.ErrorsArray;
                    }
                    else {
                        CachedDataManager.RefreshTenantTextCodes().subscribe((response:any) => {
                            var item = Fieldresponse.Result;
                            var oldItem = window.ObjectFields.filter(t => t.Id == item.Id)[0];
                            if (oldItem) {
                                var index = window.ObjectFields.indexOf(oldItem);
                                window.ObjectFields.splice(index, 1);
                            }
                            item.ObjectTable_LookUpTableName = this.LookUpTableName;
                            item.ObjectTableName = this.ObjectTableName;
                            window.ObjectFields.push(item);
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            this.CurrentSession.CloseCurrentWindowEmit("Refresh");
                            this.CurrentSession.SessionEvent.emit({ Name: "ReloadGridComponent" });

                        });
                    }
                });
            }
            else {
                if (this.IsCardTable(this.objectField.ObjectTableId)) {
                    this.objectField.RelatedEntities = this.GetRelatedEntities();
                }
                this._ObjectFieldPMService.update(this.objectField).subscribe(Fieldresponse => {
                    if (Fieldresponse.HasError) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.ValidationErrorsList = Fieldresponse.ErrorsArray;
                    }
                    else {
                        CachedDataManager.RefreshTenantTextCodes().subscribe((response:any) => {
                            var item = Fieldresponse.Result;
                            var oldItem = window.ObjectFields.filter(t => t.Id == item.Id)[0];
                            if (oldItem) {
                                var index = window.ObjectFields.indexOf(oldItem);
                                window.ObjectFields.splice(index, 1);
                            }
                            item.ObjectTable_LookUpTableName = this.LookUpTableName;
                            item.ObjectTableName = this.ObjectTableName;
                            window.ObjectFields.push(item);
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            this.CurrentSession.CloseCurrentWindowEmit("Refresh");
                            //this.loginService.GetObjectFields().subscribe((myResult:any) => {
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

    public GetRelatedEntities() : string{
        let selectedPartners = "";
        for (let key in this.PartnersDictionary) {
            if (this.PartnersDictionary[key])
                selectedPartners += key + ',';
        }
        return selectedPartners.substring(0, selectedPartners.length - 1);
    }
    public IsCardTable(ObjectTableId): boolean {
        let objectTableName = window.ObjectTables.filter(table => table.Id == ObjectTableId)[0].Name;
        return objectTableName == "Card";
    }
    public ChangeCheckBoxValue(objectTableName: string, event) {
        if (this.PartnersDictionary[objectTableName] && !this.IsNew) return;
        this.PartnersDictionary[objectTableName] = event;
    }
    public IsEnable(text: string): boolean {
        if (this.IsPartner)
            return false;
        if (this.IsNew)
            return true;
        //if (!this.PartnersDictionary[text])
        //    return true;
        if (!(window.ObjectFields.filter(field => field.FieldName == this.objectField.FieldName && field.ObjectTableName == text)[0]))
            return true;
        false;
    }
    private ValidateObjectFields() {
        this.ValidateTextObjectField();
        this.ValidateNumberObjectField();
    }

    private ValidateTextObjectField() {
        if (this.objectField.DataTypeCode != "Text" && this.objectField.DataTypeCode != "nText") return;

        if (this.objectField.MaxLength > 2000) {
            this.ValidationErrorsList.push("Maximum length of the text is 2000");
        }

        if (this.objectField.MinLength > 2000) {
            this.ValidationErrorsList.push("Minimum length of the text is 2000");
        }

        if (this.objectField.MaxLength == 0) {
            this.ValidationErrorsList.push("Max length number shouldn't be 0");
        }

        if (AppTool.IsNullOrEmpty(this.objectField.MinLength)) {
            this.ValidationErrorsList.push("Min length Field is Required");
        }

        if (AppTool.IsNullOrEmpty(this.objectField.MaxLength)) {
            this.ValidationErrorsList.push("Max length Field is Required");
        }

        if (!AppTool.IsNullOrEmpty(this.objectField.MaxLength) &&!AppTool.IsNullOrEmpty(this.objectField.MinLength) && this.objectField.MinLength > this.objectField.MaxLength) {
            this.ValidationErrorsList.push("Min length number shouldn't be more than Max length number");
        }

        if (!AppTool.IsNullOrEmpty(this.objectField.MinLength) && this.objectField.MinLength < 0) {
            this.ValidationErrorsList.push("Min length number shouldn't be less than 0");
        }

        if (!AppTool.IsNullOrEmpty(this.objectField.MaxLength) && this.objectField.MaxLength < 0) {
            this.ValidationErrorsList.push("Max length number shouldn't be less than 0");
        }
    }

    private ValidateNumberObjectField() {
        if (this.objectField.DataTypeCode != "Decimal") return;

        if (this.objectField.NumberOfDigits > 12) {
            this.ValidationErrorsList.push("Maximum length of the Number is 12");
        }
        if (this.objectField.NumberOfDigits == 0) {
            this.ValidationErrorsList.push("Length of the Number shouldn't be 0");
        }
        if (AppTool.IsNullOrEmpty(this.objectField.NumberOfDigits)) {
            this.ValidationErrorsList.push("Length Field is Required");
        }
        if (!AppTool.IsNullOrEmpty(this.objectField.NumberOfDigits) && this.objectField.NumberOfDigits < 0) {
            this.ValidationErrorsList.push("Length of the Number shouldn't be less than 0");
        }
        if (AppTool.IsNullOrEmpty(this.objectField.DigitsAfterPoint)) {
            this.ValidationErrorsList.push("Decimal digits Field is Required");
        }
        if (this.objectField.DigitsAfterPoint > 3) {
            this.ValidationErrorsList.push("Maximum decimal digits is 3");
        }
        if (!AppTool.IsNullOrEmpty(this.objectField.DigitsAfterPoint) && this.objectField.DigitsAfterPoint < 0) {
            this.ValidationErrorsList.push("Decimal digits of the Number shouldn't be less than 0");
        }

    }

    private SetCustomFieldsLengths() {
        this.SetTextMaxMinLengths();
        this.SetNumberLengthAndDecimalDigits();
    }

    private SetTextMaxMinLengths() {
        if (this.objectField.DataTypeCode != "Text" && this.objectField.DataTypeCode != "nText") return;

        this.objectField.MaxLength = (AppTool.IsNullOrEmpty(this.objectField.MaxLength)) ? 2000 : this.objectField.MaxLength;
        this.objectField.MinLength = (AppTool.IsNullOrEmpty(this.objectField.MinLength)) ? 0 : this.objectField.MinLength;
    }

    private SetNumberLengthAndDecimalDigits() {
        if (this.objectField.DataTypeCode != "Decimal") return;

        this.objectField.NumberOfDigits = (AppTool.IsNullOrEmpty(this.objectField.NumberOfDigits)) ? 12 : this.objectField.NumberOfDigits;
        this.objectField.DigitsAfterPoint = (AppTool.IsNullOrEmpty(this.objectField.DigitsAfterPoint)) ? 0 : this.objectField.DigitsAfterPoint;
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
            this._customPickListListService.getAllFromCache().subscribe((response:any) => {
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
            this._customPickListListService.getAllFromCache().subscribe((response:any) => {
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
