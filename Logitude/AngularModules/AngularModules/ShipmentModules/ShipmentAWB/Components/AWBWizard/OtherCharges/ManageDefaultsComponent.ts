import {Component} from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ChargesTypeList} from '../../../../../Common/EntityLists/ChargesTypeList';
import {ChargesTypeListService} from '../../../../../Common/Services/StandardLists/ChargesTypeListService';
import {CommonDomainService} from '../../../../../Common/Services/CommonDomainService';
import {ApiQueryFilters, FilterItem} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './ManageDefaultsComponent.html',
})

export class ManageDefaultsComponent {    
    public ObjectTableName: string;
    public ShipmentLevelCode: string;
    public AllChargesList: AutoDisplayItemViewModel[];
    public AutoDisplaylist: AutoDisplayItemViewModel[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.AllChargesList = [];
        this.AutoDisplaylist = [];
    }

    SetWindowArgs(ShipmentLevelCode: string) {
        this.ShipmentLevelCode = ShipmentLevelCode;
        this.ObjectTableName = this.ShipmentLevelCode == "C" ? "Consolidation" : "Shipment";

        this.CurrentSession.StartBusyIndicatorLoading();
        this.LoadData();
    }

    private mySearchText: string;
    get SearchText() { return this.mySearchText; }
    set SearchText(newValue: string) {
        if (this.mySearchText != newValue) {
            this.mySearchText = newValue;
            this.LoadData();
        }
    }

    private myService: ChargesTypeListService;
    private LoadData() {

        if (this.myService == null) {
            this.myService = new ChargesTypeListService();
        }

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 50;
        filters.SortBy = "Code";
        filters.SortDirection = "Descending";

        filters.Filter1Name = "IsAir";
        filters.Filter1Value = true;
        filters.Filter1Operator = "Equals";

        filters.Filter2Name = "InActive";
        filters.Filter2Value = false;
        filters.Filter2Operator = "Equals";

        filters.Filter3Name = "ChargesGroupCode";
        filters.Filter3Value = "FRT";
        filters.Filter3Operator = "NotEqual";

        if (!AppTool.IsNullOrEmpty(this.mySearchText)) {
            filters.Filter4Name = "SearchFields";
            filters.Filter4Value = this.mySearchText;
            filters.Filter4Operator = "Contains";
        }

        this.AllChargesList = [];
        this.AutoDisplaylist = [];

        this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {

                    var list: ChargesTypeList[] = myResponse.Result;

                    list.forEach(item => {
                        if (this.ShipmentLevelCode == "C") {
                            if (item.IsAutoDisplayInConsolidation) {
                                this.AutoDisplaylist.push(new AutoDisplayItemViewModel(item));
                            }

                            else {
                                this.AllChargesList.push(new AutoDisplayItemViewModel(item));
                            }
                        }

                        else {
                            if (item.IsAutoDisplayInShipment) {
                                this.AutoDisplaylist.push(new AutoDisplayItemViewModel(item));
                            }

                            else {
                                this.AllChargesList.push(new AutoDisplayItemViewModel(item));
                            }
                        }
                    });
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    get AllChargesHeader() {
        var myResult = "All Charges";

        if (this.AllChargesList != null) {
            myResult += " (" + this.AllChargesList.length + ")";
        }

        return myResult;
    }
    get AutoDisplayHeader() {
        var myResult = "Auto Display";

        if (this.ShipmentLevelCode == "C") {
            myResult += " in Consolidation";
        }

        else {
            myResult += " in Shipment";
        }

        if (this.AllChargesList != null) {
            myResult += " (" + this.AutoDisplaylist.length + ")";
        }

        return myResult;
    }

    public SelectedAllChargesItem: AutoDisplayItemViewModel;
    public SelectedAutoDisplayItem: AutoDisplayItemViewModel;

    AddClicked() {
        var myChargeId: string;
        var item = this.SelectedAllChargesItem;

        if (item != null) {

            myChargeId = item.Id;

            var index1 = this.AllChargesList.indexOf(item);
            if (index1 > -1) {
                this.AllChargesList.splice(index1, 1);
            }

            var index2 = this.AutoDisplaylist.indexOf(item);
            if (index2 == -1) {
                this.AutoDisplaylist.push(item);
            }
        }
        
        this.SelectedAllChargesItem = null;
        this.InvokeEditChargeType(myChargeId, true);
    }
    RemoveClicked() {
        var myChargeId: string;
        var item = this.SelectedAutoDisplayItem;

        if (item != null) {

            myChargeId = item.Id;

            var index1 = this.AutoDisplaylist.indexOf(item);
            if (index1 > -1) {
                this.AutoDisplaylist.splice(index1, 1);
            }

            var index2 = this.AllChargesList.indexOf(item);
            if (index2 == -1) {
                this.AllChargesList.push(item);
            }
        }

        this.SelectedAutoDisplayItem = null;
        this.InvokeEditChargeType(myChargeId, false);
    }

    private myDomainService: CommonDomainService;
    private InvokeEditChargeType(myChargeId: string, isAutoDisplay: boolean) {

        var myPropertyTypeCode = "S";
        if (this.ShipmentLevelCode == "C") {
            myPropertyTypeCode = "C";
        }

        if (this.myDomainService == null) {
            this.myDomainService = new CommonDomainService();
        }

        this.myDomainService.InvokeUpdateAutoDisplay(myChargeId, myPropertyTypeCode, isAutoDisplay).subscribe(myResult => {

        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    EditChargeType(item: AutoDisplayItemViewModel) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Charges Type";
        logWindow.IsFillScreen = true;
        logWindow.ShowEditComponent(item.Id, "ChargesType");
    }
}

class AutoDisplayItemViewModel {
    public Id: string;
    public Code: string;
    public Name: string;
    constructor(private item: ChargesTypeList) {
        this.Id = item.Id;
        this.Code = item.Code;
        this.Name = item.EnglishName;
    }
}
