declare var window: any;

import { Component, ViewContainerRef, OnInit, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { DWObjectFieldPM } from '../../../../Infrastructure/EntityPMs/DWObjectFieldPM';
import { DWObjectTablePM } from '../../../../Infrastructure/EntityPMs/DWObjectTablePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { DWSubQueryPM } from '../../../../Infrastructure/EntityPMs/DWSubQueryPM';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { DWObjectTableListService } from '../../../../Infrastructure/Services/StandardLists/DWObjectTableListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DWQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWQueryPMService';
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { CustomEntityArgs } from '../../../../Infrastructure/Components/LogitudeComponents/DWLogSearchWindowComponent';
import { DWQueryBuilderBaseComponent, ObjectFieldOperator, DWFieldsGroup, MultiSelectedValue, ValueDetails, DWObjectFieldsDetails } from '../../../../InfrastructureModules/InfrastructureBIReport/Components/Workspaces/DWQueryBuilderBaseComponent';

@Component({
    selector: 'DWQueryBuilder',
    templateUrl: './DWQueryBuilderComponent.html',
})

export class DWQueryBuilderComponent extends DWQueryBuilderBaseComponent {
    public _DWObjectTablePMService: DWObjectTablePMService;
    public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _DWSubQueryPMService: DWSubQueryPMService;
    public _DWObjectTableListService: DWObjectTableListService;
    public _DWQueryPMService: DWQueryPMService;
    public ObsList: any[] = [];
    public ObsListAll: any[] = [];
    public AllFieldsObsList: any[] = [];
    public SelectedFiltersDataSourceChanged: any;
    public _DWQueryBuilderHelper: DWQueryBuilderHelper;
    public IconPath: string = "./Images/Help.png";
    public IconBackground: string = null;
    public Width: number = 200;
    public Height: number = 110;
    public IconSize: number = 17;

    private IsCopy: boolean = false;
    private CopyBIReportsFromTenant: number;
    private FactTableName: string;
    private FactTableCode: string;
    private ParentFactTableCode: string;
    private DWObjectTablePivotCode: string;
    private GroupChargesAdditionalColumns: any;
    private DWObjectFields: any;
    private ComponentRef;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    DataSource: any[];
    AllFieldsDataSource: DWObjectFieldsDetails[];
    AllGroupsDataSource: DWFieldsGroup[];
    SelectedFieldsDataSource: DWObjectFieldsDetails[] = [];
    SelectedFiltersDataSource: DWObjectFieldsDetails[] = [];
    AllFieldsWithChildrenDataSource: DWObjectFieldsDetails[];
    DataContext: any = this;
    QueryId: string;
    AllTables: any[] = [];
    ObjectTable: any;
    SearchFieldsId: string;
    HasChanges: boolean = false;
    IsBIReportWorkspace: boolean = false;
    IsBIReportEditScreen: boolean = false;
    FolderId: string;
    HeightFilterArea: number;
    HeightPreviewArea: number;
    IsLoadShipmentEntityResource: boolean = false;
    IsLoadShipmentMasterResource: boolean = false;
    IsLoadShipmentARInvoiceResource: boolean = false;
    IsLoadShipmentAPInvoiceResource: boolean = false;
    IsLoadShipmentShipmentComputedFieldsResource: boolean = false;
    IsLoadShipmentShipmentPayableResource: boolean = false;
    IsLoadShipmentChargesTypeResource: boolean = false;
    KPIFeatureToggle: any;

    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();

    mouseover(MyItem) {
        if (MyItem.HelpText) {
            var item = document.getElementById(MyItem.TooltipId);
            var itemRect = item.getBoundingClientRect();
            var isToRight = true;
            var ApplicationSession = document.getElementById("ApplicationSession");
            if (ApplicationSession) {
                var appWidth = ApplicationSession.clientWidth;
                var appHeight = ApplicationSession.clientHeight;
                if ((itemRect.left + this.Width) > appWidth) {
                    isToRight = false;
                }
            }
            document.getElementById(MyItem.TooltipContentId).style.position = "fixed";
            document.getElementById(MyItem.TooltipContentId).style.top = (itemRect.top - this.Height + 7) + 'px';
            if (isToRight) {
                document.getElementById(MyItem.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/TootipCenter.png')";
                document.getElementById(MyItem.TooltipContentId).style.left = (itemRect.left + 14) + 'px';
            }
            else {
                document.getElementById(MyItem.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/TootipFlipped.png')";
                document.getElementById(MyItem.TooltipContentId).style.left = (itemRect.left - this.Width) + 'px';
            }
        }
    }

    constructor(private CDR: ChangeDetectorRef) {
        super(CDR);

        this.InitializeService();
        this.LoadEntityResources();
        this.KPIFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "KPI")[0];
    }

    InitializeService() {
        this._DWObjectTablePMService = new DWObjectTablePMService();
        this._DWQueryPMService = new DWQueryPMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        this._DWQueryBuilderService = new DWQueryBuilderService();
        this._DWSubQueryPMService = new DWSubQueryPMService();
        this._DWObjectTableListService = new DWObjectTableListService();
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
    }

    LoadEntityResources() {
        this._entityResourceService.getEntityResourceByTableName("Shipment").subscribe((response: any) => { this.IsLoadShipmentEntityResource = true; this.CompleteLoadEntityResources() });
        this._entityResourceService.getEntityResourceByTableName("Master").subscribe((response: any) => { this.IsLoadShipmentMasterResource = true; this.CompleteLoadEntityResources() });
        this._entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe((response: any) => { this.IsLoadShipmentARInvoiceResource = true; this.CompleteLoadEntityResources() });
        this._entityResourceService.getEntityResourceByTableName("APInvoice").subscribe((response: any) => { this.IsLoadShipmentAPInvoiceResource = true; this.CompleteLoadEntityResources() });
        this._entityResourceService.getEntityResourceByTableName("ShipmentComputedFields").subscribe((response: any) => { this.IsLoadShipmentShipmentComputedFieldsResource = true; this.CompleteLoadEntityResources() });
        this._entityResourceService.getEntityResourceByTableName("ShipmentPayable").subscribe((response: any) => { this.IsLoadShipmentShipmentPayableResource = true; this.CompleteLoadEntityResources() });
        this._entityResourceService.getEntityResourceByTableName("ChargesType").subscribe((response: any) => { this.IsLoadShipmentChargesTypeResource = true; this.CompleteLoadEntityResources() });
    }

    CompleteLoadEntityResources() {
        if (this.IsLoadShipmentEntityResource && this.IsLoadShipmentMasterResource && this.IsLoadShipmentARInvoiceResource && this.IsLoadShipmentAPInvoiceResource && this.IsLoadShipmentShipmentComputedFieldsResource && this.IsLoadShipmentShipmentPayableResource && this.IsLoadShipmentChargesTypeResource) {
            this.Start();
        }
    }

    Start() {
        var heightScreen = 548;
        this.HeightFilterArea = window.innerHeight / 2.86;
        this.HeightPreviewArea = heightScreen - this.HeightFilterArea;

        console.log("AbedHeightX", this.HeightFilterArea);
        console.log("AbedHeighty", this.HeightPreviewArea);

        if (this.CurrentSession == null) {
            this.SearchFieldsId = "SearchFields_-1_-1";
        }
        else {
            this.SearchFieldsId = "DWQueryBuilderSearchFields_" + this.CurrentSession.GetNewId("DWQueryBuilderSearchFields");
        }
        this.ObsList = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ..");
        this._DWObjectTableListService.getAll().subscribe((myResult: any) => {
            this.AllTables = myResult.Result;
            this._DWObjectTablePMService.get(this.FactTableName).subscribe((myResult: any) => {
                if (!myResult.HasError) {
                    this.FactTableCode = myResult.Result.Code;
                    this.ParentFactTableCode = myResult.Result.ParentFactCode;
                    this.DWObjectFields = window.DWObjectFields.filter(d => d.FactTableCode == this.FactTableCode || (!AppTool.IsNullOrEmpty(this.ParentFactTableCode) && d.FactTableCode == this.ParentFactTableCode));
                    this.FillGroupChargesValues(myResult);
                    this._DWObjectFieldPMService.GetDWObjectFieldsByDWTableIdGroupedByCategory(this.FactTableCode).subscribe((Result: ServiceResponse) => {//getDWObjectFieldsByDWTableId
                        if (!Result.HasError) {
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            var MyGroups = [];
                            var MyAllGroups = [];
                            Result.Result.forEach((Group) => {
                                if (Group.FieldsList.filter(a => a.DisplayInQueryBuilder == true).length > 0) {
                                    var view = new DWFieldsGroup(Group.Key, Group.FieldsList);
                                    if (MyGroups.length == 0) {
                                        view.IsDetailesOpened = true;
                                        view.DetailsIcon = "./Images/CellIcons/Arrowup.png";
                                    }
                                    else {
                                        view.IsDetailesOpened = false;
                                        view.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                                    }
                                    var MyInnerList = [];
                                    view.FieldsList.forEach((field) => {
                                        if (this.DisplayFieldInQueryBuilder(field)) {
                                            var MyItem = new DWObjectFieldsDetails(field, this);
                                            MyItem.ParentDataTypeCode = field.DataTypeCode;
                                            MyItem.Category1 = field.Category1;
                                            MyItem.Category2 = field.Category2;
                                            MyInnerList.push(MyItem);
                                            this.ObsList.push(MyItem);
                                            this.ObsListAll.push(MyItem);
                                        }
                                    });

                                    var view1 = new DWFieldsGroup(Group.Key, Group.FieldsList);
                                    if (MyAllGroups.length == 0) {
                                        view1.IsDetailesOpened = true;
                                        view1.DetailsIcon = "./Images/CellIcons/Arrowup.png";
                                    }
                                    else {
                                        view1.IsDetailesOpened = false;
                                        view1.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                                    }
                                    var MyInnerList1 = [];
                                    view1.FieldsList.forEach((field) => {
                                        if (this.DisplayFieldInQueryBuilder(field)) {
                                            var MyItem = new DWObjectFieldsDetails(field, this);
                                            MyItem.ParentDataTypeCode = field.DataTypeCode;
                                            MyItem.Category1 = field.Category1;
                                            MyItem.Category2 = field.Category2;
                                            MyInnerList1.push(MyItem);
                                            this.ObsList.push(MyItem);
                                            this.ObsListAll.push(MyItem);
                                        }
                                    });

                                    view.FieldsList = MyInnerList;
                                    view1.FieldsList = MyInnerList1;
                                    MyGroups.push(view);
                                    MyAllGroups.push(view1);
                                }
                            });
                            this.DataSource = MyGroups;
                            this.AllGroupsDataSource = MyAllGroups;
                            this.AllFieldsDataSource = this.ObsList;
                        }
                    });
                    this.FillAllFieldsWithChildrenDataSource(this.DWObjectFields);

                    if (this.QID) {
                        this.EditBIReport();
                    }
                }
            });
        });
    }

    DisplayFieldInQueryBuilder(field) {
        var displayField: boolean = field.DisplayInQueryBuilder == true;
        var fieldsNeedKPIFeature: Array<string> = ["[Booking Confirmation Sent]", "[Pre Alert Sent]", "[Delivery Notice Sent]", "[Expected Arrival Notice Sent]", "[Arrival Notice Sent]", "[T1 Received]"];
        if (fieldsNeedKPIFeature.find(f => f == field.Code)) {
            if (!this.KPIFeatureToggle)
                displayField = false;
        }
        return displayField;
    }

    FillGroupChargesValues(myResult: any) {
        this.DWObjectTablePivotCode = myResult.Result.PivotFieldCode;
        this.GroupChargesAdditionalColumns = window.DWObjectFields.filter(d => d.FactTableCode == "Fact_Charges" && d.DWObjectTableCode == this.DWObjectTablePivotCode && d.Code == '[Code]')[0];
    }

    FillAllFieldsWithChildrenDataSource(objectFieldList: any) {
        var TempObsList = [];
        objectFieldList.forEach((field) => {
            if (field.DisplayInQueryBuilder == true || field.IsPrimaryKey == true) {
                var view = new DWObjectFieldsDetails(field, this);
                view.ParentDataTypeCode = field.DataTypeCode;
                this.AllFieldsObsList.push(field);
                TempObsList.push(view);
            }
        });
        this.AllFieldsWithChildrenDataSource = TempObsList.sort((a, b) => a.DisplayName.localeCompare(b.DisplayName));
    }

    SetWindowArgs(args: any) {
        this.QID = args.DWQueryId;
        this.IsBIReportWorkspace = args.IsBIReportWorkspace;
        this.IsBIReportEditScreen = args.IsBIReportEditScreen;
        this.FolderId = args.FolderId;
        this.IsCopy = args.IsCopy;
        this.ComponentRef = args.ComponentRef;
        this.BackCompleted = args.BackCompleted;
        this.CopyBIReportsFromTenant = args.BIReportsTenant;
        this.FactTableName = args.FactTableName;
    }

    EditBIReport() {
        if (this.CopyBIReportsFromTenant || this.CopyBIReportsFromTenant == 0) {
            this._DWSubQueryPMService.getByQueryIdFromTenant(this.QID, this.CopyBIReportsFromTenant).subscribe((myResult: any) => {
                if (!myResult.HasError) {
                    this.ID = myResult.Result.SubQueryData.Id;
                    this.EditButtonClicked();
                }
            });
        }
        else {
            this._DWSubQueryPMService.getByQueryId(this.QID).subscribe((myResult: any) => {
                if (!myResult.HasError) {
                    this.ID = myResult.Result.SubQueryData.Id;
                    this.EditButtonClicked();
                }
            });
        }
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        temp.select();
    }

    FillPlaceHolder() {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
            temp.placeholder = TextCodeTranslator.Translate("General.O.Search");
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
    }

    OnDeleteValue() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    }

    Run() {
    }

    SetSelectedItem(item) {
        this.SelectedItem = item;
        this.FieldSelectedItem = null;
        this.FilterSelectedItem = null;
        this.IsbtnAddEnabled = true;
        this.IsbtnRemoveEnabled = false;
        this.IsbtnAddFilterEnabled = true;
        this.IsbtnRemoveFilterEnabled = false;
        this.IsbtnUpEnabled = false;
        this.IsbtnDownEnabled = false;
    }

    SetFieldSelectedItem(item) {
        this.FieldSelectedItem = item;
        this.SelectedItem = null;
        this.FilterSelectedItem = null;
        this.IsbtnAddEnabled = false;
        this.IsbtnRemoveEnabled = true;
        this.IsbtnUpEnabled = true;
        this.IsbtnDownEnabled = true;
        this.IsbtnAddFilterEnabled = false;
        this.IsbtnRemoveFilterEnabled = false;
    }

    SetFilterSelectedItem(item) {
        this.FilterSelectedItem = item;
        this.SelectedItem = null;
        this.FieldSelectedItem = null;
        this.IsbtnAddFilterEnabled = false;
        this.IsbtnRemoveFilterEnabled = true;
        this.IsbtnAddEnabled = false;
        this.IsbtnRemoveEnabled = false;
    }

    SelectChargesClicked(item) {
        var args = new CustomEntityArgs();
        args.ObjectTableName = this.DWObjectTablePivotCode;
        args.DisplayFieldsFromList = this.GroupChargesAdditionalColumns.Code;
        args.LOVAdditionalColumns = this.GroupChargesAdditionalColumns.LOVAdditionalColumns;
        args.DataContext = item;
        args.SelectedFieldsDataSource = this.SelectedFieldsDataSource;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 900;
        logitudeWindow.Height = 600;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Title = "Create Group of Charge Types";
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/DWLogSearchWindowComponent');

        logitudeWindow.WindowClosed.subscribe((event) => {
            if (event != "Cancel") this.OnGroupChargsClosed(item, event);
        });
    }

    OnGroupChargsClosed(item, columnName) {
        item.DisplayName = columnName;
        item.Name = columnName;
    }

    private notes: string;
    public get Notes() { return this.notes; }
    public set Notes(newValue: string) {
        this.notes = newValue;
    }

    private timerToken: any;
    private searchText: string;
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        if (newValue != null && newValue != "") {
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            this.timerToken = setTimeout(() => this.SearchFieldChanged(newValue), 600);
        }
        else {
            this.OnEmptySearchField();
        }
    }

    GetIdWithoutSpecialCharacters(value: string){
        let specialCharacters = ['/','(',')'];
        specialCharacters.forEach(ch => {
            value = value.replace(ch,'');
        });
        return value;
    }

    private SearchFieldChanged(newValue: string) {
        this.AllGroupsDataSource.forEach((Group) => {
            var temp = Group.FieldsList.filter(a => a.Name.toLowerCase().indexOf(newValue.toLowerCase()) > -1 || a.DisplayName.toLowerCase().indexOf(newValue.toLowerCase()) > -1);
            this.DataSource.filter(a => a.Key == Group.Key)[0].FieldsList = temp;

            if (temp.length == 0)
                this.OpenCloseGroup(Group, false);
            else
                this.OpenCloseGroup(Group, true);
        });

        var selectedDimensionDWObjectFields = this.DWObjectFields.filter(d => d.DisplayName.toLowerCase().includes(newValue.toLowerCase()));
        selectedDimensionDWObjectFields = selectedDimensionDWObjectFields.filter(
            (thing, i, arr) => arr.findIndex(t => t.DimensionTableDisplayName === thing.DimensionTableDisplayName) === i
        );
        let showGroup = false;
        selectedDimensionDWObjectFields.forEach((field) => {
            showGroup = true;
            if (field.DimensionTableDisplayName) {
                this.AllGroupsDataSource.forEach((Group) => {
                    var temp = Group.FieldsList.filter(a => a.DataTypeCode == "Dimension" && !a.HideTree && (a.Name.toLowerCase() == field.DimensionTableDisplayName.toLowerCase() || a.DisplayName.toLowerCase() == field.DimensionTableDisplayName.toLowerCase()))[0];
                    if (temp) {
                        if (showGroup) {
                            this.OpenCloseGroup(Group, true);
                            showGroup = false;
                        }
                        this.DataSource.filter(a => a.Key == Group.Key)[0].FieldsList.push(temp);
                        this.DataSource.filter(a => a.Key == Group.Key)[0].FieldsList = this.DataSource.filter(a => a.Key == Group.Key)[0].FieldsList.filter(
                            (thing, i, arr) => arr.findIndex(t => t.DisplayName === thing.DisplayName) === i
                        );
                        temp.IsViewTree = true;
                        temp.LoadWithSearchValue(temp, newValue);
                    }
                });
            }
        });
    }

    private OnEmptySearchField() {
        var index = 0;
        this.AllGroupsDataSource.forEach((Group) => {
            if (index == 0)
                this.OpenCloseGroup(Group, true);
            else
                this.OpenCloseGroup(Group, false);
            this.DataSource.filter(a => a.Key == Group.Key)[0].FieldsList = Group.FieldsList;
            Group.FieldsList.forEach((fieldsList) => {
                fieldsList.IsViewTree = false;
            });
            index++;
        });
    }

    private OpenCloseGroup(Group, open) {
        let detailsIcon = open ? "./Images/CellIcons/Arrowup.png" : "./Images/CellIcons/Arrowdown.png";
        this.DataSource.filter(a => a.Key == Group.Key)[0].IsDetailesOpened = open;
        this.DataSource.filter(a => a.Key == Group.Key)[0].DetailsIcon = detailsIcon;
    }

    private selectedItem: DWObjectFieldsDetails;
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(newValue: DWObjectFieldsDetails) {
        this.selectedItem = newValue;
    }

    private fieldSelectedItem: DWObjectFieldsDetails;
    public get FieldSelectedItem() { return this.fieldSelectedItem; }
    public set FieldSelectedItem(newValue: DWObjectFieldsDetails) {
        this.fieldSelectedItem = newValue;
    }

    private filterSelectedItem: DWObjectFieldsDetails;
    public get FilterSelectedItem() { return this.filterSelectedItem; }
    public set FilterSelectedItem(newValue: DWObjectFieldsDetails) {
        this.filterSelectedItem = newValue;
    }

    private isbtnAddEnabled: boolean = false;
    public get IsbtnAddEnabled() { return this.isbtnAddEnabled; }
    public set IsbtnAddEnabled(newValue: boolean) {
        this.isbtnAddEnabled = newValue;
    }

    private isbtnRemoveEnabled: boolean = false;
    public get IsbtnRemoveEnabled() { return this.isbtnRemoveEnabled; }
    public set IsbtnRemoveEnabled(newValue: boolean) {
        this.isbtnRemoveEnabled = newValue;
    }

    private isbtnAddFilterEnabled: boolean = false;
    public get IsbtnAddFilterEnabled() { return this.isbtnAddFilterEnabled; }
    public set IsbtnAddFilterEnabled(newValue: boolean) {
        this.isbtnAddFilterEnabled = newValue;
    }

    private isbtnRemoveFilterEnabled: boolean = false;
    public get IsbtnRemoveFilterEnabled() { return this.isbtnRemoveFilterEnabled; }
    public set IsbtnRemoveFilterEnabled(newValue: boolean) {
        this.isbtnRemoveFilterEnabled = newValue;
    }

    private isbtnUpEnabled: boolean = true;
    public get IsbtnUpEnabled() { return this.isbtnUpEnabled; }
    public set IsbtnUpEnabled(newValue: boolean) {
        this.isbtnUpEnabled = newValue;
    }

    private isbtnDownEnabled: boolean = true;
    public get IsbtnDownEnabled() { return this.isbtnDownEnabled; }
    public set IsbtnDownEnabled(newValue: boolean) {
        this.isbtnDownEnabled = newValue;
    }

    ReorderColumnsList() {
        var queryColumnList = this.SelectedFieldsDataSource.sort((a, b) => {
            return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1
        });
        var i = 0;
        for (; i < queryColumnList.length; i++) {
            queryColumnList[i].IndexOrder = i;
        }
    }

    btnUp_Click(selectedItem) {
        var item = selectedItem;
        if (item != null) {
            var i = this.SelectedFieldsDataSource.indexOf(item);
            this.ReorderColumnsList();
            var upColumn = this.SelectedFieldsDataSource.filter(d => d.DisplayName == item.DisplayName)[0];
            if (i > 0) {
                this.SelectedFieldsDataSource = this.SelectedFieldsDataSource.filter(d => d.DisplayName != upColumn.DisplayName);
                this.SelectedFieldsDataSource.filter(o => o.IndexOrder == i - 1)[0].IndexOrder = i;
                upColumn.IndexOrder = i - 1;
                this.SelectedFieldsDataSource.splice(i - 1, 0, upColumn);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
            this.ClearData();
        }
    }

    ShowOrderArrows(item) {
        this.FieldSelectedItem = item;
    }

    HideOrderArrows(item) {
        this.FieldSelectedItem = null;
    }

    btnDown_Click(selectedItem) {
        var item = selectedItem;
        if (item != null) {
            var i = this.SelectedFieldsDataSource.indexOf(item);
            this.ReorderColumnsList();
            var downColumn = this.SelectedFieldsDataSource.filter(d => d.DisplayName == item.DisplayName)[0];
            if (i < this.SelectedFieldsDataSource.length - 1) {
                this.SelectedFieldsDataSource = this.SelectedFieldsDataSource.filter(d => d.DisplayName != item.DisplayName);

                this.SelectedFieldsDataSource.filter(o => o.IndexOrder == i + 1)[0].IndexOrder = i;
                downColumn.IndexOrder = i + 1;
                this.SelectedFieldsDataSource.splice(i + 1, 0, downColumn);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
        }
    }

    ResetIndexes(TempArray: any[]) {
        var index = 0;
        TempArray.forEach((field) => {
            field.IndexOrder = index;
            index++;
        });
        return TempArray;
    }

    btnAdd_Click(item) {
        this.SelectedItem = item.IsMultipleSelection ? new DWObjectFieldsDetails(item) : item;
        var myCurrentItem = this.SelectedFieldsDataSource.filter(a => a.DisplayName == this.SelectedItem.DisplayName);
        if (this.MatchAddColumnConditions(myCurrentItem)) {
            this.AddSelectedField();
            this.ClearData();
        }
    }

    private MatchAddColumnConditions(myCurrentItem: DWObjectFieldsDetails[]) {
        return this.SelectedItem && myCurrentItem && (myCurrentItem.length == 0 || (this.SelectedItem.IsMultipleSelection && !this.IsExistColumnName(myCurrentItem)));
    }

    IsExistColumnName(myCurrentItem: DWObjectFieldsDetails[]) {
        var isExist = false;
        if (this.SelectedFieldsDataSource) {
            this.SelectedFieldsDataSource.forEach(field => {
                if (myCurrentItem.length > 0 && field.DisplayName == myCurrentItem[0].DisplayName) {
                    isExist = true;
                }
            });
        }
        return isExist;
    }

    AddSelectedField() {
        if (this.SelectedItem.Code == '[Full Date]' || this.SelectedItem.Code == '[Full Date US]') {
            this.SelectedItem.ParentDataTypeCode = "LookUp";
            this.SelectedItem.DataTypeCode = "Date";
            this.SelectedItem.HasTree = true;
        }
        if (this.SelectedItem.Name == 'Full Date') {
            this.SelectedItem.HasTree = false;
        }
        var tempData = this.SelectedFieldsDataSource;
        tempData.push(this.SelectedItem);
        this.SelectedFieldsDataSource = this.ResetIndexes(tempData);
    }

    ClearData() {
        this.SampleData = [];
        this.Notes = "";
        this.IsPreview = true;
        this.IsDataReturened = true;
    }

    btnRemove_Click(item) {
        this.FieldSelectedItem = item;
        this.FieldSelectedItem = item;
        if (this.FieldSelectedItem) {
            var index = this.SelectedFieldsDataSource.indexOf(this.FieldSelectedItem);
            if (index !== -1) {
                this.SelectedFieldsDataSource.splice(index, 1);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
            this.ClearData();
        }
    }

    RootGroups: DWObjectFieldsDetails[] = [];
    btnAddFilter_Click(item: DWObjectFieldsDetails) {
        if (item.CannotFilter == true) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Not available for filtering";
            this.messageWindow.Message = "This field is not available for filtering, you can use the code";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        var view = new DWObjectFieldsDetails(item.BaseDWObjectField, this);
        if (item.HasTree && item.DataTypeCode != "DateTime") {
            var defaultItem: any = this.DWObjectFields.filter(d => d.DWObjectTableCode == (view.DWObjectTableCode) && d.Code == '[Code]')[0];
            if (defaultItem) {
                view.Code = defaultItem.Code;
                view.LOVAdditionalColumns = defaultItem.LOVAdditionalColumns;
            }
        }
        if (view.DWObjectTableCode.indexOf("DIM_") != -1 && (view.DataTypeCode != "DateTime" || view.DWObjectTableCode.indexOf("DIM_Date") != -1)) {
            if (view.Code == '[Full Date]' || view.Code == '[Full Date US]') {
                view.ParentDataTypeCode = "Date";
                view.DataTypeCode = "Date";
                view.HasTree = true;
            }
            else {
                view.ParentDataTypeCode = "LookUp";
            }
            if (view.Name == 'Full Date') {
                view.HasTree = false;
            }
            if (view.DataTypeCode == "Boolean") view.ParentDataTypeCode = "Boolean";

            if (item.BaseDWObjectField.DataTypeCode == "LookUp" || item.BaseDWObjectField.DataTypeCode == "Dimension") {
                view.ParentDimTabelName = item.BaseDWObjectField.DimensionTableCode;
            }
            else {
                view.ParentDimTabelName = item.BaseDWObjectField.DWObjectTableCode;
            }
        }
        else {
            view.ParentDataTypeCode = item.BaseDWObjectField.DataTypeCode;
        }
        view.DisplayName = item.DisplayName;
        view.DimensionTableDisplayName = item.DimensionTableDisplayName;
        this.SelectedItem = view;
        if (this.SelectedItem && this.SelectedFiltersDataSource.indexOf(this.SelectedItem) == -1) {//&& this.SelectedItem.DataTypeCode != "LookUp" && this.SelectedItem.DataTypeCode != "Dimension"
            if (this.SelectedFiltersDataSource.length == 0) {
                var DWObjectField = new DWObjectFieldsDetails(null, this);
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
                DWObjectField.FilterItems.push(this.SelectedItem);
                this.SelectedFiltersDataSource.push(DWObjectField);
            }
            else {
                var tempData = this.SelectedFiltersDataSource[0].FilterItems;
                tempData.push(this.SelectedItem);
                this.SelectedFiltersDataSource[0].FilterItems = tempData;
                var tempDataNew = this.SelectedFiltersDataSource;
                this.SelectedFiltersDataSource = [];
                this.SelectedFiltersDataSource = tempDataNew;
            }
            this.RunDetectChanges();
            if (this.SelectedItem.DataTypeCode == "Boolean") {
                this.ClearData();
            }
            this.ClearData();
        }
    }

    btnRemoveFilter_Click() {
        if (this.FilterSelectedItem) {
            var index = this.SelectedFiltersDataSource.indexOf(this.FilterSelectedItem);
            if (index !== -1) {
                this.SelectedFiltersDataSource.splice(index, 1);
            }
            this.ClearData();
        }
    }

    WhereStmt: string = " where ";
    GetWhereStmtForFiltersList(FiltersList: DWObjectFieldsDetails[], AndOr: string) {
        FiltersList.forEach((Myfilter) => {
            var isHaveMultiSelect = false;
            if (Myfilter.FilterItems.length > 0) {
                if (this.GetIfFiltersHaveValues(Myfilter.FilterItems) == true) {
                    this.WhereStmt = this.WhereStmt + " ( ";
                }
                this.GetWhereStmtForFiltersList(Myfilter.FilterItems, Myfilter.AndOr);
                if (this.WhereStmt == " where ") {
                    this.WhereStmt = "";
                }
                else if (this.WhereStmt.substring(this.WhereStmt.length - 4).indexOf("And") != -1 || this.WhereStmt.substring(this.WhereStmt.length - 4).indexOf("Or") != -1) {
                    this.WhereStmt = this.WhereStmt.substring(0, this.WhereStmt.length - 4);
                }
                if (this.WhereStmt != "" && this.GetIfFiltersHaveValues(Myfilter.FilterItems) == true) {
                    this.WhereStmt = this.WhereStmt + " ) ";
                }
            }
            else {
                if (!AppTool.IsNullOrEmpty(Myfilter.TextValue)) {
                    var filter = Myfilter;
                    var OperationSimpol = "";
                    if (filter.Operation.Code == filter.MyParentClass.equalsOp.Code) {
                        if (filter.DataTypeCode == 'Integer' || filter.DataTypeCode == 'Double' || filter.DataTypeCode == 'Decimal') {
                            OperationSimpol = " = @@ ";
                        }
                        else {
                            OperationSimpol = " IN ( '";
                            OperationSimpol = this.BuildMultiValueSql(filter.TextValue, OperationSimpol);
                            isHaveMultiSelect = true;
                        }
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.notEqualsOp.Code) {
                        if (filter.DataTypeCode == 'Integer' || filter.DataTypeCode == 'Double' || filter.DataTypeCode == 'Decimal') {
                            OperationSimpol = " <> @@ ";
                        }
                        else {
                            OperationSimpol = " not IN ( '";
                            OperationSimpol = this.BuildMultiValueSql(filter.TextValue, OperationSimpol);
                            isHaveMultiSelect = true;
                        }
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.startsWithOp.Code) {
                        OperationSimpol = " like '@@%' ";
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.IsNullOp.Code) {
                        OperationSimpol = " is null ";
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.IsNotNullOp.Code) {
                        OperationSimpol = " is not null ";
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.greaterThanOrEqualOp.Code) {
                        OperationSimpol = " >= @@ ";
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.largerThanOp.Code) {
                        OperationSimpol = " > @@ ";
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.lessThanOp.Code) {
                        OperationSimpol = " < @@ ";
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.lessThanOrEqualOp.Code) {
                        OperationSimpol = " <= @@ ";
                    }
                    if (filter.Operation.Code == filter.MyParentClass.IsNullOp.Code) {
                        this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is null or " + (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " = '' " + " " + AndOr + " ";
                    }
                    else if (filter.Operation.Code == filter.MyParentClass.IsNotNullOp.Code) {
                        this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is not null and " + (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " <> '' " + " " + AndOr + " ";
                    }
                    else {
                        var operation = !isHaveMultiSelect ? OperationSimpol.replace("@@", filter.TextValue) : OperationSimpol;
                        this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + operation + " " + AndOr + " ";//" = " + "'" + filter.TextValue + "' and ";
                    }
                }
                else {
                }
            }
        });
    }

    BuildMultiValueSql(textValue: any, operationSimpol: string) {
        var result = operationSimpol;
        if (textValue) {
            var values: string[] = textValue.toString().split(';');
            if (values.length > 0) {
                values.forEach((item) => {
                    if (item) {
                        result += (item + "','");
                    }
                });
                result += ")";
                result = result.replace(",')", ")");
            } else result += " ')";

        } else result += " ')";

        return result;
    }

    GetIfFiltersHaveValues(FiltersList: DWObjectFieldsDetails[]) {
        return FiltersList.filter(a => !AppTool.IsNullOrEmpty(a.TextValue)).length > 0;
    }

    GetWhereJoined(FiltersList: DWObjectFieldsDetails[]) {
        FiltersList.forEach((Myfilter) => {
            if (Myfilter.FilterItems.length > 0) {
                this.GetWhereJoined(Myfilter.FilterItems);
            }
            else {
                if (Myfilter.ParentDimTabelName != null && this.InnerTables.filter(a => a.ParentDimTabelName == Myfilter.ParentDimTabelName).length == 0) {
                    this.InnerTables.push(Myfilter);
                }
            }
        });
    }

    TempFilters: any[] = [];
    DeleteField(Item: DWObjectFieldsDetails, ListItems: DWObjectFieldsDetails[]) {
        ListItems.forEach((Myfilter) => {
            if (Myfilter.FilterItems.length > 0) {
                this.DeleteField(Item, Myfilter.FilterItems);
            }
            else {
                if (Myfilter == Item) {
                    ListItems = ListItems.filter(a => a != Item);
                }
            }
        });
    }

    AddFilterToGroup() {
        var DWObjectField = new DWObjectFieldsDetails(null, this);
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;

        var tempData = this.SelectedFiltersDataSource[0].FilterItems;
        tempData.push(this.SelectedItem);
        this.SelectedFiltersDataSource[0].FilterItems = tempData;
    }

    AddGroup() {
        var DWObjectField = new DWObjectFieldsDetails(null, this);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var DWInnerObjectField = new DWObjectFieldsDetails(null, this);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        var tempData = this.SelectedFiltersDataSource[0].FilterItems;
        tempData.push(this.SelectedItem);
        this.SelectedFiltersDataSource[0].FilterItems = tempData;
    }

    InnerTables: any[] = [];
    public DWQueryData: DWQueryData;
    SaveChanges(StopPreview: boolean = false) {
        this.DWQueryData = new DWQueryData();
        this.DWQueryData.Filters = this.SelectedFiltersDataSource[0];
        this.DWQueryData.Columns = this.SelectedFieldsDataSource;
        this.DWQueryData.FactTableName = this.FactTableName;
        this.DWQueryData.PageIndex = 0;
        this.DWQueryData.PageSize = 100;
        this.SelectedFieldsDataSource.forEach(field => {
            if (field.IsMultipleSelection)
                this.DWQueryData.PageSize = 1000;
        });
        if (this.SelectedFiltersDataSource.length > 0 && this.ValidFiltersValues(this.SelectedFiltersDataSource[0]) != true) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Invalid Filters";
            this.messageWindow.Message = "There is an invalid input in one of the filters";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        if (this.SelectedFieldsDataSource.length > 0) {
            this.StartBusyIndicator("Loading ..");
            this.IsPreview = !StopPreview;
            this._DWQueryBuilderService.GetNewDWQueryData(this.DWQueryData).subscribe((myResult: ServiceResponse) => {
                this.StopBusyIndicator();
                if (!myResult.HasError) {
                    if (StopPreview == true) {
                        this.SampleData = [];
                        var MySql = myResult.Result.SQLString.split("ORDER BY")[0];
                        this.Notes = MySql;
                    }
                    else {
                        this.PreviewData(StopPreview, myResult.Result.SQLDataResult);
                    }
                } else {
                    if (myResult.ErrorsArray && myResult.ErrorsArray.length > 0) {
                        var messageWindow: MessageWindow = new MessageWindow();
                        messageWindow.Show(myResult.ErrorsArray[0]);
                    }
                }
            });
        }
    }

    SampleData: any[] = [];

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    IsPreview: boolean = true;
    IsDataReturened: boolean = true;

    PreviewData(StopPreview: boolean = false, Data: any[]) {
        if (StopPreview == true) {
            this.SampleData = [];
            this.IsPreview = false;
            return;
        }
        if (Data.length > 0) {
            this.IsDataReturened = true;
        }
        else {
            this.IsDataReturened = false;
        }
        this.IsPreview = true;
        this.SampleData = Data;
    }

    ShowSQL() {
        this.SaveChanges(true);
    }

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public StartBusyIndicator(myText: string) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
        this.RunDetectChanges();
    }

    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    public FiltersBusyIndicatorText: string = null;
    public FiltersShowBusyIndicator: boolean = false;
    public StartFiltersBusyIndicator(myText: string) {
        this.FiltersBusyIndicatorText = myText;
        this.FiltersShowBusyIndicator = true;
        this.RunDetectChanges();
    }

    public StopFiltersBusyIndicator() {
        this.FiltersBusyIndicatorText = null;
        this.FiltersShowBusyIndicator = false;
    }

    private qID: string;
    public get QID() { return this.qID; }
    public set QID(newValue: string) {
        this.qID = newValue;
    }

    private iD: string;
    public get ID() { return this.iD; }
    public set ID(newValue: string) {
        this.iD = newValue;
    }

    private messageWindow: MessageWindow = new MessageWindow();
    public QueryData: DWQueryData;

    SaveButtonClicked() {
        if (this.SelectedFieldsDataSource.length == 0) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Invalid Query";
            this.messageWindow.Message = "The query should contain at least one column.";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ..");
        this._DWObjectTablePMService.get(this.FactTableName).subscribe((myResult:any) => {
            if (!myResult.HasError) {
                var MySubQuery = new DWSubQueryPM();
                MySubQuery.Tenant = SessionLocator.Tenant;
                MySubQuery.DWFactTableCode = myResult.Result.Code;
                MySubQuery.SQLString = this.Notes;
                this.QueryData = new DWQueryData();
                this.QueryData.SubQueryData = MySubQuery;
                this.QueryData.Columns = this.SelectedFieldsDataSource;
                this.QueryData.Filters = this.SelectedFiltersDataSource[0];
                if (AppTool.IsNullOrEmpty(this.ID) || this.IsCopy) {
                    this._DWSubQueryPMService.insertDWQueryData(this.QueryData).subscribe((myResult:any) => {
                        this.ID = myResult.Result.Id;
                        this.QID = myResult.Result.DWQueryId
                        this.EditButtonClicked(true);

                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (this.IsBIReportWorkspace) {
                            this.CurrentSession.CloseCurrentWindowEmit("ok");
                        }
                    });
                }
                else {
                    MySubQuery.Id = this.ID;
                    if (this.NotExist == true) {
                        this.messageWindow.Width = 300;
                        this.messageWindow.Height = 150;
                        this.messageWindow.Title = "Query Doesn't Exist";
                        this.messageWindow.Message = "Query With the Id " + this.ID + " does not exist";
                        this.messageWindow.Show(this.messageWindow.Message);
                        this.NotExist = true;
                    }
                    this._DWSubQueryPMService.UpdateDWQueryData(this.QueryData).subscribe((myResult:any) => {

                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (this.IsBIReportWorkspace || this.IsBIReportEditScreen) {
                            this.CurrentSession.CloseCurrentWindowEmit("ok");
                        }
                    });
                }
            }
        });
    }

    NotExist: boolean = true;
    EditButtonClicked(getSingle: boolean = false) {
        this.SelectedFieldsDataSource = [];
        this.SelectedFiltersDataSource = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ..");
        if (!AppTool.IsNullOrEmpty(this.ID)) {
            if ((this.CopyBIReportsFromTenant || this.CopyBIReportsFromTenant == 0) && !getSingle) {
                this._DWSubQueryPMService.getFromTenant(this.ID, this.CopyBIReportsFromTenant).subscribe((myResult:any) => {
                    this.BuildData(myResult);
                });
            }
            else {
                this._DWSubQueryPMService.get(this.ID).subscribe((myResult:any) => {
                    this.BuildData(myResult);
                });
            }
        }
    }

    BuildData(myResult) {
        var QueryData = new DWQueryData();
        if (myResult.Result == null) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Query Doesn't Exist";
            this.messageWindow.Message = "Query With the Id " + this.ID + " does not exist";
            this.messageWindow.Show(this.messageWindow.Message);
            this.NotExist = true;
            return;
        }
        else {
            this.NotExist = false;
        }
        if (!myResult.HasError) {
            QueryData = myResult.Result;
            var tempColumns = this.SelectedFieldsDataSource;
            QueryData.Columns.forEach((field) => {
                var view = new DWObjectFieldsDetails(field, this);
                view.ParentDataTypeCode = field.ParentDataTypeCode;
                view.DisplayName = field.DisplayName;
                view.DimensionTableDisplayName = field.DimensionTableDisplayName;
                view.IsMultipleSelection = field.IsMultipleSelection;
                view.MultiSelectedValueLists = this.MapMultiSelectedValueLists(field.MultiSelectedValueLists);

                view.ParentCode = field.ParentCode;
                view.ParentDimTabelName = field.ParentDimTabelName;
                tempColumns.push(view);
            });

            this.SelectedFieldsDataSource = this.ResetIndexes(tempColumns);
            if (QueryData.Filters) {
                var DWObjectField = new DWObjectFieldsDetails(null, this);
                DWObjectField.IsGroup = true;
                DWObjectField.setAndOrOperation(QueryData.Filters.AndOr, false);
                DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
                var MyFilter = this.RestoreFilters(QueryData.Filters, DWObjectField);
                var temp = [];
                temp.push(MyFilter);
                this.SelectedFiltersDataSource = temp;
            }
            else {
                this.SelectedFiltersDataSource = [];
            }
            if (this.CurrentSession.CurrentWindow) {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        }
    }

    RestoreFilters(BaseFilter: DWObjectFieldsDetails, MyFilter: DWObjectFieldsDetails) {
        BaseFilter.FilterItems.forEach((field) => {
            var view = new DWObjectFieldsDetails(field, this);
            view.DisplayName = field.DisplayName;
            if (view.HasTree && view.DataTypeCode != "DateTime") {
                var defaultItem: any = this.DWObjectFields.filter(d => d.DWObjectTableCode == (view.DWObjectTableCode) && d.Code == '[Code]')[0];
                if (defaultItem) {
                    view.Code = defaultItem.Code;
                    view.LOVAdditionalColumns = defaultItem.LOVAdditionalColumns;
                }
            }
            if (field.FilterItems.length == 0) {
                if (field.DWObjectTableCode && (field.DWObjectTableCode.indexOf("DIM_") != -1 && (field.DataTypeCode != "DateTime" || field.DWObjectTableCode.indexOf("DIM_Date") != -1))) {
                    if (view.Code == '[Full Date]' || view.Code == '[Full Date US]') {
                        view.ParentDataTypeCode = "Date";
                        view.DataTypeCode = "Date";
                    }
                    else {
                        view.ParentDataTypeCode = "LookUp";
                    }
                    view.ParentDimTabelName = field.DWObjectTableCode;

                    if (view.DataTypeCode == "Boolean") view.ParentDataTypeCode = "Boolean";
                }
                else {
                    view.ParentDataTypeCode = field.DataTypeCode;
                }
            }
            if (field.FilterItems.length == 0) {
                view.setTextValue(field.TextValue, false);
                view.MultiSelectedValueLists = this.MapMultiSelectedValueLists(field.MultiSelectedValueLists);
                view.Operation = new ObjectFieldOperator(field.OperationCode, field.OperationName);
                MyFilter.FilterItems.push(view);
            }
            else {
                var DWObjectField = new DWObjectFieldsDetails(null, this);
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = MyFilter.FilterItems.length;
                DWObjectField.setAndOrOperation(field.AndOr, false);
                this.RestoreFilters(field, DWObjectField);
                MyFilter.FilterItems.push(DWObjectField);
            }
        });

        return MyFilter;
    }

    MapMultiSelectedValueLists(lists: MultiSelectedValue[]) {
        var result: MultiSelectedValue[] = [];
        if (lists) {
            lists.forEach((field) => {
                var item: MultiSelectedValue = new MultiSelectedValue();
                var i = "";
                var j = 0;
                while (field["Value" + i]) {
                    var valueDetails: ValueDetails = new ValueDetails();
                    valueDetails.Header = field["Value" + i].Header;
                    valueDetails.Row = field["Value" + i].Row;
                    item["Value" + i] = valueDetails;
                    j += 1;
                    i = j.toString();
                }
                result.push(item);
            });
            return result;
        }
    }

    RunDetectChanges() {
        if (this.CDR) {
            var isDestroyed: boolean = this.CDR['destroyed'];
            if (!isDestroyed) {
                this.CDR.detectChanges();
            }
        }
    }
}
