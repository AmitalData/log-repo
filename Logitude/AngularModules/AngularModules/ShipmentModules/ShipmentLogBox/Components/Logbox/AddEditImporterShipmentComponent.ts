import {ShipmentArchiveFilter} from '../../../../Controls/ShipmentArchiveFilter';
import {TransportsFilter} from '../../../../Controls/TransportsFilter';
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SearchTextBox} from '../../../../Controls/SearchTextBox';
import {IconButton} from '../../../../Controls/IconButton';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import {Http, Response} from '@angular/http';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogBoxDocumentsComponent} from './LogBoxDocumentsComponent';
import {ShipmentDomainService, ImporterQueriesDataCounts} from '../../../../Shipment/Services/ShipmentDomainService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityStatusListService} from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import {BranchListService} from '../../../../Common/Services/StandardLists/BranchListService';
import {DepartmentListService} from '../../../../Common/Services/StandardLists/DepartmentListService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {PortExtendedPMService} from '../../../../Common/Services/ExtendedPMs/PortExtendedPMService';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditImporterShipmentComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class AddEditImporterShipmentComponent extends BaseComponent implements OnInit, AfterViewInit {
    private myShipmentDomainService: ShipmentDomainService;
    EntityPM: ShipmentPM = new ShipmentPM();
    DataContext: AddEditImporterShipmentComponent = this;
    ValidationErrorsList: any[];
    IsNew: boolean = true; 
    public _EntityStatusListService: EntityStatusListService;
    public _DepartmentListService: DepartmentListService;
    public _BranchListService: BranchListService;
    public _ShipmentPMService: ShipmentPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityListService: EntityListService) {
        super();
        this.ValidationErrorsList = [];
        this.myShipmentDomainService = new ShipmentDomainService();
        this._EntityStatusListService = new EntityStatusListService();
        this._DepartmentListService = new DepartmentListService();
        this._BranchListService = new BranchListService();
        this._ShipmentPMService = new ShipmentPMService();
    }
    ngOnInit() {
        
    }
    ngAfterViewInit() {

    }
    SetWindowArgs(args: any) {
        //this.ShipmentList = args.SelectedShipment;
        this.IsNew = args.IsNew;
        if (this.IsNew) {
            this._EntityStatusListService.getAll().subscribe(myResult => {
                if (!myResult.HasError) {
                    this.StatusId = myResult.Result.filter(a => a.Code == "OPOP")[0].Id;
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
            this._DepartmentListService.getAll().subscribe(myResult => {
                if (!myResult.HasError) {
                    this.DepartmentId = myResult.Result.filter(a => a.Tenant == SessionLocator.Tenant)[0].Id;
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
            this._BranchListService.getAll().subscribe(myResult => {
                if (!myResult.HasError) {
                    this.BranchId = myResult.Result.filter(a => a.Tenant == SessionLocator.Tenant)[0].Id;
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
        }
        if (args.EntityPM) {
            this.EntityPM = args.EntityPM;
        }
    }
    public get ShipperName() { return this.EntityPM.ShipperName }
    public set ShipperName(newValue: string) { this.EntityPM.ShipperName = newValue; }

    public get CustomerReference1() { return this.EntityPM.CustomerReference1 }
    public set CustomerReference1(newValue: string) { this.EntityPM.CustomerReference1 = newValue; }

    public get CustomerReference2() { return this.EntityPM.CustomerReference2 }
    public set CustomerReference2(newValue: string) { this.EntityPM.CustomerReference2 = newValue; }
        
    public get CustomerId() { return this.EntityPM.CustomerId }
    public set CustomerId(newValue: string) { this.EntityPM.CustomerId = newValue; }

    public get StatusDate() { return this.EntityPM.StatusDate }
    public set StatusDate(newValue: any) { this.EntityPM.StatusDate = newValue; }

    public get StatusId() { return this.EntityPM.StatusId }
    public set StatusId(newValue: string) { this.EntityPM.StatusId = newValue; }

    public get ForwarderPartnerId() { return this.EntityPM.ForwarderPartnerId }
    public set ForwarderPartnerId(newValue: string) { this.EntityPM.ForwarderPartnerId = newValue; }

    public get DepartmentId() { return this.EntityPM.DepartmentId }
    public set DepartmentId(newValue: string) { this.EntityPM.DepartmentId = newValue; }

    public get BranchId() { return this.EntityPM.BranchId }
    public set BranchId(newValue: string) { this.EntityPM.BranchId = newValue; }

    public get ShipmentLevelCode() { return this.EntityPM.ShipmentLevelCode }
    public set ShipmentLevelCode(newValue: string) { this.EntityPM.ShipmentLevelCode = newValue; }

    public get DirectionId() { return this.EntityPM.DirectionId }
    public set DirectionId(newValue: string) { this.EntityPM.DirectionId = newValue; }
        
    public get TransportModeId() { return this.EntityPM.TransportModeId }
    public set TransportModeId(newValue: string) { this.EntityPM.TransportModeId = newValue; }

    public get ToPortId() { return this.EntityPM.ToPortId }
    public set ToPortId(newValue: string) { this.EntityPM.ToPortId = newValue; }

    public get OtherPrepaidCollectId() { return this.EntityPM.OtherPrepaidCollectId }
    public set OtherPrepaidCollectId(newValue: string) { this.EntityPM.OtherPrepaidCollectId = newValue; }

    public get FreightPrepaidCollectId() { return this.EntityPM.FreightPrepaidCollectId }
    public set FreightPrepaidCollectId(newValue: string) { this.EntityPM.FreightPrepaidCollectId = newValue; }

    public get FromPortId() { return this.EntityPM.FromPortId }
    public set FromPortId(newValue: string) { this.EntityPM.FromPortId = newValue; }
    public _PortExtendedPMService: PortExtendedPMService;

    isSaveClicked: boolean = false;
    SaveChanges() {

        if (this.isSaveClicked == true) {
            return;
        }
        this.isSaveClicked = true;
        this.ValidationErrorsList = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.TransportModeId)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Transportation Type"));
        }

        if (AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Order Number"));
        }

        if (AppTool.IsNullOrEmpty(this.ForwarderPartnerId)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Agent"));
        }
        this._PortExtendedPMService = new PortExtendedPMService();
        if (AppTool.IsNullOrEmpty(this.FromPortId)) {
            //this.ValidationErrorsList.push(msg.replace("%FieldName", "Gatway"));
            //this._PortExtendedPMService.getSinglePort("---", "IL", SessionLocator.Tenant).subscribe(Result => {
            //    this.FromPortId = Result.Result.Id;
            //});
        }

        if (AppTool.IsNullOrEmpty(this.ToPortId)) {
            //this.ValidationErrorsList.push(msg.replace("%FieldName", "Destination"));
        }

        //FillErrors(errors);

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.EntityPM.IsImporterShipment = true;
            this.EntityPM.MainCarriageFromPortId = this.EntityPM.FromPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.ToPortId;
            this.EntityPM.GrossWeightUnitCode = "KG";
            this.EntityPM.DimensionsUnitCode = "Cm";
            this.EntityPM.ChargeableWeightUnitCode = "KG";
            this.EntityPM.VolumeUnitCode = "CBF";
            if (this.IsNew) {
                this.EntityPM.FreightPrepaidCollectId = "C";
                this.EntityPM.ShipmentCustomerTypeCode = "SHI";
                this.EntityPM.OtherPrepaidCollectId = "C";
                this.EntityPM.DirectionId = "C";
                this.EntityPM.ShipmentLevelCode = "A";
                this.EntityPM.OrderIsDangerouseGoods = false;
                this.EntityPM.StatusDate = DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.CustomerId = SessionLocator.TenantPM.CustomerId;
                this.EntityPM.CustomerName = SessionLocator.TenantPM.CustomerId;
                this.EntityPM.ConsigneeId = SessionLocator.TenantPM.CustomerId;
                this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
                this.EntityPM.NewConcurrencyGUID = Guid.newGuid();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this._ShipmentPMService.insert(this.EntityPM).subscribe(myResult => {
                    if (!myResult.HasError) {
                        ServiceLocator.SendTotangoUserActivity("LogBox", "New Shipment");
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded");
                        //ParentViewModel.setImporterFilter();
                        //ParentViewModel.LoadAllData();
                    }
                    else {
                        this.ValidationErrorsList = myResult.ErrorsArray;
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                    this.isSaveClicked = false;
                });
            }
            else {
                this._ShipmentPMService.update(this.EntityPM).subscribe(myResult => {
                    if (!myResult.HasError) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded"); 
                    }
                    else {
                        this.ValidationErrorsList = myResult.ErrorsArray;
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                    this.isSaveClicked = false;
                });
            }
            //ShipmentContext.SubmitChanges().Completed += AddEditImporterShipmentViewModel_Completed;

        }
        else {
            this.isSaveClicked = false;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
        
}
