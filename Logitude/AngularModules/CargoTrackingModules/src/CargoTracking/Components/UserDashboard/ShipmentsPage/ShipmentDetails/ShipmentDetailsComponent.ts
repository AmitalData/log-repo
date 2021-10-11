import {Component,ViewChild, ElementRef, AfterViewInit, HostListener, Input, EventEmitter} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from 'src/CargoTracking/EntityLists/CargoTrackingShipmentList';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { CargoTrackingShipmentWithMilestones, Milestone } from 'src/CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import { CargoTrackingPortService } from '../../../../Services/Others/CargoTrackingPortService';
import { CargoTrackingShipmentService } from '../../../../Services/Others/CargoTrackingShipmentService';
import { CargoTrackingShipmentCustomsData } from "../../../../DataContracts/CargoTrackingShipmentCustomsData";
import { DocumentDownloadService } from '../../../../Services/Others/DocumentDownloadService';
import { MessageWindowComponent } from '../../../../../Infrastructure/Components/MessageWindow/MessageWindowComponent';
import { CargoTrackingShipmentOrderService } from '../../../../Services/Others/CargoTrackingShipmentOrderService';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'ShipmentDetailsComponent',
    templateUrl: './ShipmentDetailsComponent.html',
    styleUrls: ['./ShipmentDetailsComponent.css']
})
export class ShipmentDetailsComponent implements AfterViewInit
{

    @ViewChild('SliderWrapper') SliderWrapperElement: ElementRef;

    @Input() DetailsSectionToggleEvent: EventEmitter<any> = new EventEmitter();
 
    public isLoading: boolean = true;
    showMoreReferences: boolean = false;
    SecurityKey: string = "";
    Shipment: CargoTrackingShipmentWithMilestones = null;
    public ShipmentWithMilestones: CargoTrackingShipmentWithMilestones;
    public toPortCode: string;
    public fromPortCode: string;
    ShipmentReferences: string[] = [];
    SearchText: string = "";
    CustomsBrokerReference: string;
    ShipmentPM: any ;
    ShipmentOrder: any ;
    ShipmentPackages: any[];
    DocumentsFilings: any[];
    PartnerCards: PartnerCard[] = [];
    HasReferences: boolean = false;
    HasContainersDetails: boolean = false;
    InlandTransportMode = 'I';
    WarehouseTransportMode ='W'
    OceanTransportMode = 'O';
    AirTransportMode = 'A';
    ContainersNumbers: string[] = [];
    ShowDetailsSection: boolean = false;
    TitleOfCustomsOrForwarder: string = "";
    ValueOfCustomsOrForwarder: string = "";
    CustomsEntityType: string = "C";
    ForwardingEntityType: string = "F";
    OrderEntityType: string = "O";
    public OverviewPanelTitle: string;
    public TypeTitle: string;
    PartnerCardTypesOfShipmentTransportMode = {
        'A': "AIRLINES",
        'I': "TRUCKER",
        'O': "SHIPPING LINES"
    }


    ShipmentCustomsData: CargoTrackingShipmentCustomsData = null;
    get tenant()
    {
        return CargoTrackingBrandingData.Tenant;
    }
    constructor(private router: Router,
        private route: ActivatedRoute,
        private searchService: CargoTrackingSearchService,
        private cargoTrackingPortService: CargoTrackingPortService,
        private cargoTrackingShipmentService: CargoTrackingShipmentService,
        private cargoTrackingShipmentOrderService: CargoTrackingShipmentOrderService,
        private documentDownloadService: DocumentDownloadService,
        public dialog: MatDialog,
        private datePipe: DatePipe)
    {

        this.GetIdFromURI();

    }
    ngAfterViewInit(): void
    {
        this.LoadShipment();
        this.CreatePartnerCardsFromShipmentPM();
    }
    private SetOverviewPanelTitle() {
        if (this.Shipment.ShipmentList.EntityType == this.OrderEntityType) {
            this.OverviewPanelTitle = "Order Overview";

        }
        else {
            this.OverviewPanelTitle = "Overview";
        }
    }
    @HostListener('window:resize', ['$event'])
    onResize()
    {
        //event.target.innerWidth;
        this.InitSlider();
    }

    onMousewheel(event: WheelEvent)
    {
        event.preventDefault();
        if (event.deltaY > 0) {
            this.MoveSlider('left');
        }
        if (event.deltaY < 0) {
            this.MoveSlider('right');
        }
    }

    logPan(i)
    {
        console.log(i);

    }
    InitSlider()
    {
        var PAGERS_WIDTH = 200; // 100 * 2 pager
        var mobilePagersWidth = 30; // 100 * 2 pager
        var screenwidth = window.innerWidth;

        var sliderWrapperWidth = this.SliderWrapperElement.nativeElement.offsetWidth;
        const maxWidthForMobileScreen = 470;
        if (screenwidth > maxWidthForMobileScreen)
            var count = Math.floor((sliderWrapperWidth - PAGERS_WIDTH) / this.sliderCardWidth);

        if (screenwidth <= maxWidthForMobileScreen)
        var mobileCount = this.CalculateVisibleSliderCardsCount(sliderWrapperWidth, mobilePagersWidth);
        this.sliderVisibleCardsCount = count == undefined ? mobileCount: count;

        this.sliderVisibleCardsWidth = count * this.sliderCardWidth;
        this.sliderMarginCardCount = 0;
        this.sliderMarginLeft = screenwidth < maxWidthForMobileScreen ? (this.sliderCardWidth - 60) * -1 : 0; // mobile: add

    }

    private CalculateVisibleSliderCardsCount(sliderWrapperWidth: number, mobilePagersWidth: number ) {
        return Math.floor((sliderWrapperWidth - mobilePagersWidth) / this.sliderMobileCardWidth);
    }
    LoadShipment()
    {
        //this.isLoading = true;
        this.searchService.getUserShipment(this.SecurityKey, this.tenant).subscribe((result: any) =>
        {
            this.isLoading = false;
            console.log("[getUserShipment]", result);
            this.ShipmentWithMilestones = result;
            if (this.ShipmentWithMilestones) {
                this.Shipment = result;
                this.ShipmentReferences = result.ShipmentList.CustomerReference ? result.ShipmentList.CustomerReference.split(',') : null;
                this.HasReferences = this.SetHasReferences();

                this.SetRoutingVariables();

                if (this.Shipment.ShipmentList.EntityType == this.OrderEntityType) {
                    this.GetShipmentOrder();
                }
                else {
                    this.GetShipmentPM();
                    this.GetShipmentCustomsData();
                    this.GetShipmentPackages();
                    this.GetDocumentsFilingsConnectedToShipment();
                }
               
                this.SetCustomsOrForwarderFields();
                this.SetOverviewPanelTitle();
                this.SetTypeTitle();

            }

            setTimeout(() =>
            {
                this.InitSlider();
                this.BuildSliderCards();

            }, 200);
        });
    }
    GetShipmentOrder() {
        this.cargoTrackingShipmentOrderService.get(this.Shipment.ShipmentList.EntityId).subscribe((result: any) => {
            if (result) {
                this.ShipmentOrder = result;

                this.GetPartnersAddresses();
                this.InitRoutes();
            }
        });
    }
    SetTitleForSupplierOrClient(directionId: string) {
        var title;
        switch (directionId) {
            case ShipmentDirections.Export: {
                title = "CLIENT"
                break;
            }

            case ShipmentDirections.Import:
            case ShipmentDirections.Customs: {
                title = "SUPPLIER"
                break;
            }
        }

        return title;
    }
    SetSupplierOrCleintValueByDirection(shipmentList) {
        if(shipmentList.DirectionId == ShipmentDirections.Export) 
        {
            return shipmentList.ShipperName;
        } else if(shipmentList.DirectionId == ShipmentDirections.Import) {
            return shipmentList.ConsigneeName;   
        }
        return '';
    }
    SetTypeTitle() {
        if (this.Shipment.ShipmentList.TransportModeId == "A") {
            this.TypeTitle = "PACKAGE TYPE";

        }
         if (this.Shipment.ShipmentList.TransportModeId != "A") {
            this.TypeTitle = "CONTAINER TYPE";
        }
         if (this.Shipment.ShipmentList.EntityType == 'O') {
            this.TypeTitle = "SHIPMENT TYPE";
        }
    }
    SetHasReferences() {
        return this.ShipmentReferences == null ? false : true;
    }
    SetHasContainersDetails() {
        return this.ShipmentPackages.length==0  ? false : true;
    }
    SetRoutingVariables()
    {
        this.SetFromPortCode(this.Shipment.ShipmentList.FromPortId);
        this.SetToPortCode(this.Shipment.ShipmentList.ToPortId);
    }

    private SetFromPortCode(id: string)
    {
        this.cargoTrackingPortService.get(id).subscribe((result: any) =>
        {
            this.fromPortCode = result.Code;
        })
    }

    private SetToPortCode(id: string)
    {
        this.cargoTrackingPortService.get(id).subscribe((result: any) =>
        {
            this.toPortCode = result.Code;
        })
    }


    GetShipmentPM()
    {
        this.cargoTrackingShipmentService.get(this.Shipment.ShipmentList.EntityId).subscribe((result: any) =>
        {
            if (result) {
                this.ShipmentPM = result;
                console.log("ShipmentPM", this.ShipmentPM);
                this.GetPartnersAddresses();
                this.FillCustomsBrokerReferenceFromShipmentPM();
                this.SetContainersNumbers(result);
                this.InitRoutes();
            }
        });
    }

    GetShipmentPackages() {
        this.cargoTrackingShipmentService.GetShipmentPackages(this.Shipment.ShipmentList.EntityId).subscribe((result: any) => {
            if (result) {
                this.ShipmentPackages = result;
                this.HasContainersDetails = this.SetHasContainersDetails();
                console.log("GetShipmentPackages", this.ShipmentPackages);
            }
        });
    }

    GetDocumentsFilingsConnectedToShipment() {
        this.cargoTrackingShipmentService.GetDocumentsFilingsConnectedToShipment(this.Shipment.ShipmentList.EntityId).subscribe((result: any) => {
            if (result) {
                this.DocumentsFilings = result.map(d => (
                    {
                        ShowDetailsMenu: false,
                        ...d }
                    ));
                this.isLoading = false;

            }
        });
    }

    private SetContainersNumbers(result: any) {
        this.ContainersNumbers = [];
        this.ContainersNumbers = result.ContainersNumbers ? result.ContainersNumbers.split(',') : null;
    }

    SetCustomsOrForwarderFields() {
        this.SetTitleOfCustomsOrForwarder();
        this.SetValueOfCustomsOrForwarder();
    }

    SetTitleOfCustomsOrForwarder() {
        if (this.Shipment.ShipmentList.EntityType == this.CustomsEntityType) {
            this.TitleOfCustomsOrForwarder = "Customs Broker References";
        }

        if (this.Shipment.ShipmentList.EntityType == this.ForwardingEntityType) {
            this.TitleOfCustomsOrForwarder = "Forwarder Reference";
        }
        if (this.Shipment.ShipmentList.EntityType == this.OrderEntityType) {
            this.TitleOfCustomsOrForwarder = "Order References";
        }
    }

    SetValueOfCustomsOrForwarder() {
        if (this.Shipment.ShipmentList.EntityType == this.ForwardingEntityType) {
            this.ValueOfCustomsOrForwarder = this.Shipment.ShipmentList.ShipmentNumber;
        }

        if (this.Shipment.ShipmentList.EntityType == this.CustomsEntityType || this.Shipment.ShipmentList.EntityType == this.OrderEntityType) {
            var ForwardingShipmentNumber = this.Shipment.ShipmentList.ForwardingShipmentHeaderId != null ? this.Shipment.ShipmentList.ForwardingShipmentNumber != null ? "\n" + this.Shipment.ShipmentList.ForwardingShipmentNumber : "": "";
            this.ValueOfCustomsOrForwarder = this.Shipment.ShipmentList.ShipmentNumber + ForwardingShipmentNumber;
        }

    }

    PartnersAddresses: any[] = [];
    GetPartnersAddresses(){
        var partnersIds = this.GetShipmentPartnersIds();
        this.cargoTrackingShipmentService.GetPartnersAddresses(partnersIds).subscribe((result: any) =>
        {
            if (result) {
                this.PartnersAddresses = result;

                this.CreatePartnerCardsFromShipmentPM();
            }
        });
    }
    private GetShipmentOrderPartnerIds() {
       return [
            this.ShipmentOrder.ShipperId,
            this.ShipmentOrder.ConsigneeId,
            this.ShipmentOrder.AgentId,
            this.ShipmentOrder.CarrierId,

        ];
    }
    private GetShipmentPartnersIds()
    {
        if (this.Shipment.ShipmentList.EntityType == "O") {
         var partnersIds=  this.GetShipmentOrderPartnerIds();
        }
        else {
            var partnersIds = [
                this.ShipmentPM.ShipperId,
                this.ShipmentPM.ConsigneeId,
                this.ShipmentPM.FreightForwarderId,
                this.ShipmentPM.CustomerId,
                this.ShipmentPM.AgentId,
                this.ShipmentPM.IssuingCarrierAgentId,
                this.ShipmentPM.CustomAgentExportId,
                this.ShipmentPM.CustomAgentImportId,
                this.ShipmentPM.Notify1Id,
                this.ShipmentPM.Notify2Id,
                this.ShipmentPM.ShipperNotExporterId,
                this.ShipmentPM.ConsigneeNotImporterId,
                this.ShipmentPM.CustomClearancePointId,
                this.ShipmentPM.ColoaderId,
                this.ShipmentPM.FreelancerId,
                this.ShipmentPM.ConsolidatorId,
                this.ShipmentPM.ReleasingAgentId,
                this.ShipmentPM.MainCarriageCarrierId,
                this.ShipmentPM.WarehouseLegWarehouseId,
                this.Shipment.ShipmentList.ShipperId
            ];

            for (let routing of this.ShipmentPM.ShipmentDeliveries) {
                partnersIds.push(routing.CarrierId)
            }

            for (let routing of this.ShipmentPM.ShipmentPickUps) {
                partnersIds.push(routing.CarrierId)
            }
        }
        return partnersIds.filter(p=>p);
    }

    loadingCustomsData: boolean = false;
    NoTaxDetails: boolean = false;
    GetShipmentCustomsData() {
        this.loadingCustomsData = true;
        this.cargoTrackingShipmentService.GetShipmentCustomsData(this.Shipment.ShipmentList.EntityId).subscribe((result: any) => {
            if (result) {
                this.ShipmentCustomsData = result;
                this.loadingCustomsData = false;
                this.NoTaxDetails = this.ShipmentCustomsData.TaxDetails.length == 0 ? true : false;
            }
        }, () => {
            this.loadingCustomsData = false;

        });
        this.loadingCustomsData = false;

    }

    private FillCustomsBrokerReferenceFromShipmentPM()
    {
        this.CustomsBrokerReference = this.ShipmentPM.CustomFileNumber;
    }
    FillShipmentOrderPartnerAddresses() {
        if (this.ShipmentOrder) {
            if (this.ShipmentOrder.ShipperId)
                this.PartnerCards.push(this.CreateShipperPartnerCard());
            if (this.ShipmentOrder.ConsigneeId)
                this.PartnerCards.push(this.CreateConsigneePartnerCard());
            if (this.ShipmentOrder.AgentId)
                this.PartnerCards.push(this.CreateAgentPartnerCard());
            if (this.ShipmentOrder.CarrierId)
                this.PartnerCards.push(this.CreateMainCarriageCarrierPartnerCard());

        }
    }
    CreatePartnerCardsFromShipmentPM()
    {
        if (this.Shipment.ShipmentList.EntityType == "O") {
            this.FillShipmentOrderPartnerAddresses();
        }
       else if (this.ShipmentPM) {
            if (this.Shipment.ShipmentList.ShipperId)
                this.PartnerCards.push(this.CreateShipperPartnerCard());

            if (this.ShipmentPM.ConsigneeId)
                this.PartnerCards.push(this.CreateConsigneePartnerCard());

            if (this.ShipmentPM.FreightForwarderId)
                this.PartnerCards.push(this.CreateFreightForwarderPartnerCard());

            // if (this.ShipmentPM.CustomerId)
            //     this.PartnerCards.push(this.CreateCustomerPartnerCard());

            if (this.ShipmentPM.AgentId)
                this.PartnerCards.push(this.CreateAgentPartnerCard());

            if (this.ShipmentPM.IssuingCarrierAgentId)
                this.PartnerCards.push(this.CreateIssuingCarrierAgentPartnerCard());

            if (this.ShipmentPM.CustomAgentExportId)
                this.PartnerCards.push(this.CreateCustomAgentExportPartnerCard());

            if (this.ShipmentPM.CustomAgentImportId)
                this.PartnerCards.push(this.CreateCustomAgentImportPartnerCard());

            if (this.ShipmentPM.Notify1Id)
                this.PartnerCards.push(this.CreateNotify1PartnerCard());

            if (this.ShipmentPM.Notify2Id)
                this.PartnerCards.push(this.CreateNotify2PartnerCard());

            if (this.ShipmentPM.ShipperNotExporterId)
                this.PartnerCards.push(this.CreateShipperNotExporterPartnerCard());

            if (this.ShipmentPM.ConsigneeNotImporterId)
                this.PartnerCards.push(this.CreateConsigneeNotImporterPartnerCard());

            if (this.ShipmentPM.CustomClearancePointId)
                this.PartnerCards.push(this.CreateCustomClearancePointPartnerCard());

            if (this.ShipmentPM.ColoaderId)
                this.PartnerCards.push(this.CreateColoaderPartnerCard());

            if (this.ShipmentPM.FreelancerId)
                this.PartnerCards.push(this.CreateFreelancerPartnerCard());

            if (this.ShipmentPM.ConsolidatorId)
                this.PartnerCards.push(this.CreateConsolidatorPartnerCard());

            if (this.ShipmentPM.ReleasingAgentId)
                this.PartnerCards.push(this.CreateReleasingAgentPartnerCard());

            if (this.ShipmentPM.MainCarriageCarrierId)
                this.PartnerCards.push(this.CreateMainCarriageCarrierPartnerCard());

            if (this.ShipmentPM.WarehouseLegWarehouseId)
                this.PartnerCards.push(this.CreateWarehouseLegPartnerCard());

            for (let routing of this.ShipmentPM.ShipmentDeliveries) {
                this.AddTruckerPartnerCardIfCarrierIdExist(routing);
            }

            for (let routing of this.ShipmentPM.ShipmentPickUps) {
                this.AddTruckerPartnerCardIfCarrierIdExist(routing);
            }
        }
    }
   

    private GetIdFromURI()
    {

        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;
        return _id;
    }

    //#region Slider
    SliderCards: MilestoneCard[] = [];
    sliderMarginLeft: number = 0;
    sliderMarginCardCount: number = 0;
    sliderCardWidth: number = 200;
    sliderMobileCardWidth: number = 145;
    sliderVisibleCardsCount: number = 5;
    sliderVisibleCardsWidth: number = 0;
    NoMilstonesFound: boolean = false;
    BuildSliderCards()
    {
        // this.Shipment.Milestones.forEach((milstone:Milestone) => {
        //     var newCard = new MilestoneCard();
        //     newCard.Code = milstone.Code;
        //     newCard.Date = milstone.EstimationDate || milstone.Date;
        //     newCard.Title = milstone.Name;
        //     newCard.Description = milstone.Notes;
        //     newCard.IsDimmed = milstone.IsEstimation;
        //     newCard.IsActive = milstone.Code == this.Shipment.ShipmentList.CurrentMilestoneCode;
        //     this.SliderCards.push(newCard);
        // });


        this.SliderCards = this.Shipment.Milestones
            .filter(milstone =>
            {
                var date = milstone.EstimationDate || milstone.Date;
                if (date)
                    return true;
                return false;
            })
            .sort((a, b) =>
            {
                if (a.Id > b.Id) return 1;
                if (a.Id < b.Id) return -1;
                return 0;
            })
            .map((milstone: Milestone) =>
            {
                var newCard = new MilestoneCard();
                var CurrentMilestoneExceptions= this.Shipment.ShipmentList.CurrentMilestoneExceptions;
                newCard.Date = milstone.Done ? milstone.Date : (milstone.EstimationDate || milstone.Date);
                newCard.Code = 'No. ' + milstone.Id;
                newCard.Title = milstone.Name;
                newCard.Description = milstone.Notes;
                newCard.IsDimmed = milstone.IsEstimation && !milstone.Done;
                newCard.IsActive = milstone.Id + '' == this.Shipment.ShipmentList.CurrentMilestoneCode;
                newCard.HasWarning = milstone.IsCurrent && CurrentMilestoneExceptions != null;
                newCard.WarningMessage = newCard.HasWarning ? CurrentMilestoneExceptions.substring(CurrentMilestoneExceptions.indexOf(',')+1,) : null;
                newCard.WarningDate = newCard.HasWarning ? this.datePipe.transform(CurrentMilestoneExceptions?.split(',')[0], 'dd/MM/yyyy, HH:mm'): null;
                return newCard;
            });
        this.SetNoMilstonesFound();
        // .sort((a, b) => {
        //     if (a.Date > b.Date) return 1;
        //     if (a.Date < b.Date) return -1;
        //      return 0;
        //     });
    }
    SetNoMilstonesFound() {
        if (this.SliderCards.length == 0) this.NoMilstonesFound = true;
    }
    MoveSlider(dir)
    {

        if (dir == 'right' && this.sliderMarginLeft == 0)
            return;

        if (dir == 'left' && ((this.sliderMarginCardCount + this.sliderVisibleCardsCount) >= this.SliderCards.length) || (this.sliderVisibleCardsCount >= this.SliderCards.length))
            return;

        var margin = this.sliderMarginLeft;
        // inc\dec
        if (dir == 'left') {
            margin -= this.sliderCardWidth;
            this.sliderMarginCardCount++;
        }
        else {
            margin += this.sliderCardWidth;
            this.sliderMarginCardCount--;
        }

        // limit boundary
        if (margin > 0)
            this.sliderMarginLeft = 0;
        else if (margin < this.sliderVisibleCardsWidth * -1)
            this.sliderMarginLeft = this.sliderVisibleCardsWidth;
        else
            this.sliderMarginLeft = margin;

        var screenwidth = window.innerWidth;
        if (screenwidth < 470)
            this.sliderMarginLeft - 55;


    }
    //#endregion

    GetModeIcon()
    {
        var iconPath = "";
        switch (this.Shipment.ShipmentList.TransportModeId) {
            case 'A':
                iconPath = "./assets/images/misc/plane.svg";
                break;

            case 'O':
                iconPath = "./assets/images/misc/ship.svg";
                break;

            default:
            case 'L':
                iconPath = "./assets/images/misc/Truck.svg";
                break;

        }

        return iconPath;
    }

    selectedNavButton: string = "Overview";
    PanelsNavigatorClicked(panelName: string)
    {
        this.ScrollToPanel(panelName);
    }

    private ScrollToPanel(panelName: string)
    {
        this.selectedNavButton = panelName;
        var panelElement = document.getElementById(panelName) as HTMLElement;
        if (panelElement){
            panelElement.scrollIntoView();
            document.getElementsByTagName('html')[0].scrollTop -= 103;

    }


    }

    BackLinkClicked()
    {
        this.router.navigate(['cargo-tracking', 'shipments']);
    }


    ShipmentRouteSteps: RoutingStep[] = [];
    private CreateReleasingAgentPartnerCard()
    {
        let releasingAgent = new PartnerCard();
        releasingAgent.Type = "Releasing Agent";
        releasingAgent.Name = this.ShipmentPM.ReleasingAgentName;
        releasingAgent.Address = this.GetPartnerAddress(this.ShipmentPM.ReleasingAgentId);
        releasingAgent.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.ReleasingAgentId);
        releasingAgent.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.ReleasingAgentId);

        return releasingAgent;
    }

    private CreateMainCarriageCarrierPartnerCard() {
        let mainCarriageCarrier = new PartnerCard();
        mainCarriageCarrier.Type = "Carrier";
        mainCarriageCarrier.Name = this.ShipmentOrder ? this.ShipmentOrder.CarrierName :  this.ShipmentPM.MainCarriageCarrierName;
        mainCarriageCarrier.Address = this.GetPartnerAddress(this.ShipmentOrder ? this.ShipmentOrder.CarrierId: this.ShipmentPM.MainCarriageCarrierId);
        mainCarriageCarrier.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentOrder ? this.ShipmentOrder.CarrierId : this.ShipmentPM.MainCarriageCarrierId);
        mainCarriageCarrier.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentOrder ? this.ShipmentOrder.CarrierId : this.ShipmentPM.MainCarriageCarrierId);
        mainCarriageCarrier.Type = this.PartnerCardTypesOfShipmentTransportMode[this.ShipmentOrder.TransportModeId];
        return mainCarriageCarrier;
    }


    private CreateWarehouseLegPartnerCard() {
        let warehouseLeg = new PartnerCard();
        warehouseLeg.Type = "WAREHOUSE";
        warehouseLeg.Name = this.ShipmentPM.WarehouseLegTerminalName;
        warehouseLeg.Address = this.GetPartnerAddress(this.ShipmentPM.WarehouseLegWarehouseId);
        warehouseLeg.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.WarehouseLegWarehouseId);
        warehouseLeg.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.WarehouseLegWarehouseId);

        return warehouseLeg;
    }

    private AddTruckerPartnerCardIfCarrierIdExist(routing: any) {
        if (routing.CarrierId) {
            this.PartnerCards.push(this.CreateTruckerPartnerCard(routing.CarrierId, routing.CarrierName));
        }
    }

    private CreateTruckerPartnerCard(truckerId, carrierName) {
        let trucker = new PartnerCard();
        trucker.Type = "TRUCKER";
        trucker.Name = carrierName;
        trucker.Address = this.GetPartnerAddress(truckerId);
        trucker.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(truckerId);
        trucker.FaxNumber = this.GetPartnerFaxNumberFromAddress(truckerId);

        return trucker;
    }

    private CreateConsolidatorPartnerCard()
    {
        let consolidator = new PartnerCard();
        consolidator.Type = "Consolidator";
        consolidator.Name = this.ShipmentPM.ConsolidatorName;
        consolidator.Address = this.GetPartnerAddress(this.ShipmentPM.ConsolidatorId);
        consolidator.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.ConsolidatorId);
        consolidator.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.ConsolidatorId);

        return consolidator;
    }

    private CreateFreelancerPartnerCard()
    {
        let freelancer = new PartnerCard();
        freelancer.Type = "Freelancer";
        freelancer.Name = this.ShipmentPM.FreelancerName;
        freelancer.Address = this.GetPartnerAddress(this.ShipmentPM.FreelancerId);
        freelancer.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.FreelancerId);
        freelancer.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.FreelancerId);
        return freelancer;
    }

    private CreateColoaderPartnerCard()
    {
        let coloader = new PartnerCard();
        coloader.Type = "Coloader";
        coloader.Name = this.ShipmentPM.ColoaderName;
        coloader.Address = this.GetPartnerAddress(this.ShipmentPM.ColoaderId);
        coloader.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.ColoaderId);
        coloader.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.ColoaderId);
        return coloader;
    }

    private CreateCustomClearancePointPartnerCard()
    {
        let customClearancePoint = new PartnerCard();
        customClearancePoint.Type = "Custom Clearance Point";
        customClearancePoint.Name = this.ShipmentPM.CustomClearancePointName;
        customClearancePoint.Address = this.GetPartnerAddress(this.ShipmentPM.CustomClearancePointId);
        customClearancePoint.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.CustomClearancePointId);
        customClearancePoint.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.CustomClearancePointId);
        return customClearancePoint;
    }

    private CreateConsigneeNotImporterPartnerCard()
    {
        let consigneeNotImporter = new PartnerCard();
        consigneeNotImporter.Type = "Consignee Not Importer";
        consigneeNotImporter.Name = this.ShipmentPM.ConsigneeNotImporterName;
        consigneeNotImporter.Address = this.GetPartnerAddress(this.ShipmentPM.ConsigneeNotImporterId);
        consigneeNotImporter.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.ConsigneeNotImporterId);
        consigneeNotImporter.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.ConsigneeNotImporterId);
        return consigneeNotImporter;
    }

    private CreateShipperNotExporterPartnerCard()
    {
        let shipperNotExporter = new PartnerCard();
        shipperNotExporter.Type = "Shipper Not Exporter";
        shipperNotExporter.Name = this.ShipmentPM.ShipperNotExporterName;
        shipperNotExporter.Address = this.GetPartnerAddress(this.ShipmentPM.ShipperNotExporterId);
        shipperNotExporter.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.ShipperNotExporterId);
        shipperNotExporter.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.ShipperNotExporterId);
        return shipperNotExporter;
    }

    private CreateNotify2PartnerCard()
    {
        let notify2 = new PartnerCard();
        notify2.Type = "Notify 2";
        notify2.Name = this.ShipmentPM.Notify2Name;
        notify2.Address = this.GetPartnerAddress(this.ShipmentPM.Notify2Id);
        notify2.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.Notify2Id);
        notify2.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.Notify2Id);
        return notify2;
    }

    private CreateNotify1PartnerCard()
    {
        let notify1 = new PartnerCard();
        notify1.Type = "Notify 1";
        notify1.Name = this.ShipmentPM.Notify1Name;
        notify1.Address = this.GetPartnerAddress(this.ShipmentPM.Notify1Id);
        notify1.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.Notify1Id);
        notify1.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.Notify1Id);
        return notify1;
    }

    private CreateCustomAgentImportPartnerCard()
    {
        let customAgentImport = new PartnerCard();
        customAgentImport.Type = "Custom Agent Import";
        customAgentImport.Name = this.ShipmentPM.CustomAgentImportName;
        customAgentImport.Address = this.GetPartnerAddress(this.ShipmentPM.CustomAgentImportId);
        customAgentImport.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.CustomAgentImportId);
        customAgentImport.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.CustomAgentImportId);
        return customAgentImport;
    }

    private CreateCustomAgentExportPartnerCard()
    {
        let customAgentExport = new PartnerCard();
        customAgentExport.Type = "Custom Agent Export";
        customAgentExport.Name = this.ShipmentPM.CustomAgentExportName;
        customAgentExport.Address = this.GetPartnerAddress(this.ShipmentPM.CustomAgentExportId);
        customAgentExport.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.CustomAgentExportId);
        customAgentExport.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.CustomAgentExportId);
        return customAgentExport;
    }

    private CreateIssuingCarrierAgentPartnerCard()
    {
        let issuingCarrierAgent = new PartnerCard();
        issuingCarrierAgent.Type = "Issuing Carrier's Agent";
        issuingCarrierAgent.Name = this.ShipmentPM.IssuingCarrierAgentName;
        issuingCarrierAgent.Address = this.GetPartnerAddress(this.ShipmentPM.IssuingCarrierAgentId);
        issuingCarrierAgent.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.IssuingCarrierAgentId);
        issuingCarrierAgent.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.IssuingCarrierAgentId);
        return issuingCarrierAgent;
    }

    private CreateAgentPartnerCard()
    {
        let agent = new PartnerCard();
        agent.Type = "Agent";
        agent.Name = this.ShipmentOrder ? this.ShipmentOrder.AgentName : this.ShipmentPM.AgentName;
        agent.Address = this.GetPartnerAddress(this.ShipmentOrder ? this.ShipmentOrder.AgentId : this.ShipmentPM.AgentId);
        agent.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentOrder ? this.ShipmentOrder.AgentId : this.ShipmentPM.AgentId);
        agent.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentOrder ? this.ShipmentOrder.AgentId : this.ShipmentPM.AgentId);
        return agent;
    }

    private CreateCustomerPartnerCard()
    {
        let customer = new PartnerCard();
        customer.Type = "Customer";
        customer.Name = this.ShipmentPM.CustomerName;
        customer.Address = this.GetPartnerAddress(this.ShipmentPM.CustomerId);
        customer.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.CustomerId);
        customer.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.CustomerId);
        return customer;
    }


    private CreateFreightForwarderPartnerCard()
    {
        let freightForwarder = new PartnerCard();
        freightForwarder.Type = "Freight Forwarder";
        freightForwarder.Name = this.ShipmentPM.FreightForwarderName;
        freightForwarder.Address = this.GetPartnerAddress(this.ShipmentPM.FreightForwarderId);
        freightForwarder.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.FreightForwarderId);
        freightForwarder.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.FreightForwarderId);
        return freightForwarder;
    }

    private CreateConsigneePartnerCard()
    {
        var consignee = new PartnerCard();
        consignee.Type = "consignee";
        consignee.Name = this.ShipmentOrder ? this.ShipmentOrder.ConsigneeName : this.ShipmentPM.ConsigneeName;
        consignee.Address = this.GetPartnerAddress(this.ShipmentOrder ? this.ShipmentOrder.ConsigneeId: this.ShipmentPM.ConsigneeId);
        consignee.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentOrder ? this.ShipmentOrder.ConsigneeId : this.ShipmentPM.ConsigneeId);
        consignee.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentOrder ? this.ShipmentOrder.ConsigneeId : this.ShipmentPM.ConsigneeId);
        return consignee;
    }

    private CreateShipperPartnerCard()
    {
        var shipper = new PartnerCard();
        shipper.Type = "shipper";
        shipper.Name = this.ShipmentOrder ? this.ShipmentOrder.ShipperName: this.ShipmentPM.ShipperName;
        shipper.Address = this.GetPartnerAddress(this.ShipmentOrder ? this.ShipmentOrder.ShipperId : this.ShipmentPM.ShipperId);
        shipper.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentOrder ? this.ShipmentOrder.ShipperId : this.ShipmentPM.ShipperId);
        shipper.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentOrder ? this.ShipmentOrder.ShipperId : this.ShipmentPM.ShipperId);
        return shipper;
    }


    private GetPartnerPhoneNumberFromAddress(cardId: any)
    {
        var address = this.PartnersAddresses.find(a => a.CardId == cardId);
        if (address)
            var phoneNumber = address.PhoneNumber;
        return phoneNumber;
    }
    private GetPartnerFaxNumberFromAddress(cardId: any)
    {
        var address = this.PartnersAddresses.find(a => a.CardId == cardId);
        if (address)
            var faxNumber = address.FaxNumber;
        return faxNumber;
    }
    private GetPartnerAddress(cardId: any)
    {
        var address = this.PartnersAddresses.find(a => a.CardId == cardId);
        if (address) {
            var addressLines = [
                address.Address1,
                address.Address2,
                address.ZipCode,
                address.City + ',' + address.CountryName
            ];
            return addressLines.filter(a=>a).join('<br>');
        }
        return null;
    }

    InitRoutes()
    {
        if (this.ShipmentPM) {
            this.SetShipmentRoutesAccordingToShipmentDirection();
        }

        if (this.ShipmentOrder) {
            if (this.ShipmentOrder.GatewayId) {
                this.AddShipmentRouteStep(this.CreateShipmentOrderOriginRoute());
                this.AddShipmentRouteStep(this.CreateShipmentOrderDestinationRoute());
            }
            else {
                this.AddShipmentRouteStep(this.CreateShipmentOrderNoGatewayRoute());
            }
        }
    }
    private SetShipmentRoutesAccordingToShipmentDirection() {
        this.CreatePickupsRoutesFromShipmentPM();
        this.ReorderRoutesAccordingToShipmentDirection();
        this.CreateShipmentDeliveriesRoutesFromShipmentPM();
    }

    private ReorderRoutesAccordingToShipmentDirection() {
        if (this.Shipment.ShipmentList.DirectionId == ShipmentDirections.Export) {
            this.CreateWarehouseLegRoutesFromShipmentPMIfExist();
            this.CreateMainCarriageLegsRoutesFromShipmentPM();
        }
        else {
            this.CreateMainCarriageLegsRoutesFromShipmentPM();
            this.CreateWarehouseLegRoutesFromShipmentPMIfExist();
        }
    }

    CreateShipmentOrderNoGatewayRoute() {
        var step = new RoutingStep();
        step.TransportModeCode = this.ShipmentOrder.TransportModeId;
        step.Description = "MainCarriageLeg";
        step.FromPortLabel = this.ShipmentOrder.OriginPortCode;
        step.ToPortLabel = this.ShipmentOrder.DestinationPortCode;

        step.Directions = this.BuildRouteDirections(this.ShipmentOrder);
        return step;
    }
    private CreatePickupsRoutesFromShipmentPM() {
        for (let i = 0; i < this.ShipmentPM.ShipmentPickUps.length; i++) {
            this.AddShipmentRouteStep(this.CreateSinglePickupsRoute(i));
        }
    }

    private CreateWarehouseLegRoutesFromShipmentPMIfExist() {
        if (this.ShipmentPM.WarehouseLegTerminalName != null) {
            this.AddShipmentRouteStep(this.CreateSingleWarehouseLegRoute());
        }
    }

    private CreateMainCarriageLegsRoutesFromShipmentPM() {
        for (let i = 0; i < this.ShipmentPM.MainCarriageLegs.length; i++)
            this.AddShipmentRouteStep(this.CreateSingleMainCarriageLegsRoute(i));
    }

    private CreateShipmentDeliveriesRoutesFromShipmentPM() {
        for (let i = 0; i < this.ShipmentPM.ShipmentDeliveries.length; i++) {
            this.AddShipmentRouteStep(this.CreateSingleShipmentDeliveriesRoute(i));
        }
    }

    private CreateSinglePickupsRoute(i: number) {
        var step = new RoutingStep();
        step.TransportModeCode = this.InlandTransportMode;
        step.Description = this.ShipmentPM.ShipmentPickUps[i].CarrierLocalName != null ? "Via " + this.ShipmentPM.ShipmentPickUps[i].CarrierLocalName : null;

        this.SetFromAndToLabelsForShipmentRoutes(this.ShipmentPM.ShipmentPickUps[i], step);
        step.Directions = this.BuildRouteDirections(this.ShipmentPM.ShipmentPickUps[i]);
        return step;
    }
    private CreateShipmentOrderOriginRoute() {
        var step = new RoutingStep();
        step.TransportModeCode = this.ShipmentOrder.TransportModeId;
        step.Description ="MainCarriageLeg 1";
        this.SetFromAndToLabelsForShipmentOrderRoutes( step);      
        return step;
    }
    private CreateShipmentOrderDestinationRoute() {
        var step = new RoutingStep();
        step.TransportModeCode = this.ShipmentOrder.TransportModeId;
        step.Description = "MainCarriageLeg 2";
        this.SetFromAndToLabelsForShipmentOrderOriginRoutes(step);
        return step;
    }
    private CreateSingleWarehouseLegRoute() {
        var step = new RoutingStep();
        step.TransportModeCode = this.WarehouseTransportMode;
        step.Description = this.ShipmentPM.WarehouseLegRemarks == null ? "WarehouseLeg" : this.ShipmentPM.WarehouseLegRemarks;
        step.FromPortLabel = this.ShipmentPM.WarehouseLegTerminalName;

        this.SetWarehouseLegDirections(step);
        return step;
    }

    private CreateSingleMainCarriageLegsRoute(i: number) {
        var step = new RoutingStep();
        step.TransportModeCode = this.ShipmentPM.TransportModeId;
        step.Description = this.ShipmentPM.MainCarriageCarrierName != null ? "Via " + this.ShipmentPM.MainCarriageCarrierName : null;
        step.FromPortLabel = this.ShipmentPM.MainCarriageFromPortCode;
        step.ToPortLabel = this.ShipmentPM.MainCarriageToPortCode;

        step.Directions = this.BuildRouteDirections(this.ShipmentPM.MainCarriageLegs[i]);
        return step;
    }

    private CreateSingleShipmentDeliveriesRoute(i: number) {
        var step = new RoutingStep();
        step.TransportModeCode = this.InlandTransportMode;
        step.Description = this.ShipmentPM.ShipmentDeliveries[i].CarrierLocalName != null ? "Via " + this.ShipmentPM.ShipmentDeliveries[i].CarrierLocalName : null;

        this.SetFromAndToLabelsForShipmentRoutes(this.ShipmentPM.ShipmentDeliveries[i], step);
        step.Directions = this.BuildRouteDirections(this.ShipmentPM.ShipmentDeliveries[i]);
        return step;
    }
    private SetFromAndToLabelsForShipmentOrderRoutes(step: RoutingStep) {
        step.FromPortLabel = this.ShipmentOrder.OriginPortCode;      
        step.ToPortLabel = this.ShipmentOrder.GatewayCode;
    
    }
    private SetFromAndToLabelsForShipmentOrderOriginRoutes(step: RoutingStep) {
        step.FromPortLabel = this.ShipmentOrder.GatewayCode;     
        step.ToPortLabel = this.ShipmentOrder.DestinationPortCode;
    }
    private SetFromAndToLabelsForShipmentRoutes(shipmentRoute: any, step: RoutingStep) {
        step.FromPortLabel = this.GetFromPortLabel(shipmentRoute);
        step.ToolTipFromPortLabel = this.GetToolTipFromPortLabel(shipmentRoute);

        step.ToPortLabel = this.GetToPortLabel(shipmentRoute);
        step.ToolTipToPortLabel = this.GetToolTipToPortLabel(shipmentRoute);
    }

    private GetFromPortLabel(shipmentRoute: any) {
        if (shipmentRoute.PickUpDeliveryFromTypeCode == "PORT") {
            return shipmentRoute.FromPortCode;
        }

        else if (shipmentRoute.PickUpDeliveryFromTypeCode == "PART") {
            return shipmentRoute.FromLocation.toString().split(" ")[0];
        }

        else {
           return shipmentRoute.FromAddressCountryCode;
        }
    }

    private GetToolTipFromPortLabel(shipmentRoute: any) {

        if (shipmentRoute.PickUpDeliveryFromTypeCode == "PART") {
            return "Partner: \n" + shipmentRoute.FromLocation.toString().split("\r")[0];
        }

        else if (shipmentRoute.PickUpDeliveryFromTypeCode == "CASL") {
            return "Address: \n" + (shipmentRoute.FromAddressCity_Dummy != null ? shipmentRoute.FromAddressCity_Dummy + ',' :"")  + shipmentRoute.FromAddressCountryName;
        }
    }

    private GetToPortLabel(shipmentRoute: any) {

        if (shipmentRoute.PickUpDeliveryToTypeCode == "PORT") {
           return shipmentRoute.ToPortCode;
        }

        else if (shipmentRoute.PickUpDeliveryToTypeCode == "PART") {
            return shipmentRoute.ToLocation.toString().split(" ")[0];
        }

        else {
            return shipmentRoute.ToAddressCountryCode;
        }
    }

    private GetToolTipToPortLabel(shipmentRoute: any) {
        if (shipmentRoute.PickUpDeliveryToTypeCode == "PART") {
            return "Partner: \n" + shipmentRoute.ToLocation.toString().split("\r")[0];
        }

        else if (shipmentRoute.PickUpDeliveryToTypeCode == "CASL") {
            return "Address: \n" + (shipmentRoute.ToAddressCity_Dummy != null ? shipmentRoute.ToAddressCity_Dummy + ',' : "" ) + shipmentRoute.ToAddressCountryName;
        }
    }

    BuildRouteDirections(shipmentRoute) {
        var directions = [];

        var direction = this.BuildExportRouteDirection(shipmentRoute.ETD, "ETD");
        if (direction)
            directions.push(direction);

        var direction = this.BuildImportRouteDirection(shipmentRoute.ETA, "ETA");
        if (direction)
            directions.push(direction);

        var direction = this.BuildExportRouteDirection(shipmentRoute.ATD, "ATD");
        if (direction)
            directions.push(direction);

        var direction = this.BuildImportRouteDirection(shipmentRoute.ATA, "ATA");
        if (direction)
            directions.push(direction);

        return directions;
    }

    private SetWarehouseLegDirections(step: RoutingStep) {
        if (this.ShipmentPM.WarehouseLegExpectedEntryDate != null) {
            step.Directions.push(this.BuildExportRouteDirection(this.ShipmentPM.WarehouseLegExpectedEntryDate,"ETD"));
        }

        if (this.ShipmentPM.WarehouseLegExpectedReleaseDate != null) {
            step.Directions.push(this.BuildImportRouteDirection(this.ShipmentPM.WarehouseLegExpectedReleaseDate,"ETA"));
        }

        if (this.ShipmentPM.WarehouseLegActualEntryDate != null) {
            step.Directions.push(this.BuildExportRouteDirection(this.ShipmentPM.WarehouseLegActualEntryDate, "ATD"));
        }

        if (this.ShipmentPM.WarehouseLegActualReleaseDate != null) {
            step.Directions.push(this.BuildImportRouteDirection(this.ShipmentPM.WarehouseLegActualReleaseDate, "ATA"));
        }
    }

    BuildExportRouteDirection(fieldValue, fieldName: string) {
        if (fieldValue != null) {
            return new RouteDirection(fieldValue, fieldName, "out");
        }
    }

    BuildImportRouteDirection(fieldValue, fieldName: string) {
        if (fieldValue != null) {
            return new RouteDirection(fieldValue, fieldName, "in");
        }
    }

    private AddShipmentRouteStep(step: RoutingStep) {
        this.ShipmentRouteSteps.push(step);
    }

    DownloadDocument(documentId: string, documentTypeName: string) {
        if (documentId)
            this.documentDownloadService.DownloadPage(documentId, this.Shipment.ShipmentList.ShipmentNumber + '-' + documentTypeName);
    }

    DownloadAllClick(entityId: string, securityKey: string) {
        this.documentDownloadService.DownloadAllPages(entityId, securityKey);
    }

    ShowMoreLinkClicked() {
        this.ShowDetailsSection = !this.ShowDetailsSection;
        this.DetailsSectionToggleEvent.emit();
    }

    OpenMessageWindow(messageDescription, messageDate) {
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'Exception',
                date: messageDate,
                description: messageDescription,
            }
        });
    }

    OpenReferencesWindow(references) {
        references = references.map(x => x.trim());
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'References',
                description: references.join("\n"),
            }
        });
    }

    ShowMoreWindow(text) {
        console.log(text)
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: '',
                description: text,
            }
        });
    }
}


export class MilestoneCard
{
    Code: string;
    Date: Date;
    Title: string;
    Description: string;
    IsActive: boolean;
    HasWarning: boolean;
    IsDimmed: boolean;
    WarningMessage: string;
    WarningDate: string;
}

export class RoutingStep
{
    IsActive: boolean;
    FromPortLabel;
    ToPortLabel;
    ToolTipFromPortLabel;
    ToolTipToPortLabel;
    Description: string;
    TransportModeCode;
    Directions: RouteDirection[] = [];
}
export class RouteDirection
{
    constructor(date: Date, label: string, direction: 'in' | 'out')
    {
        this.Date = date;
        this.Label = label;
        this.Direction = direction;
    }
    Date: Date;
    Label: string;
    Direction: 'in' | 'out' = 'in';
}

export class PartnerCard
{
    constructor()
    {
    }
    Name: string;
    Type: string;
    Address: string;
    PhoneNumber: string;
    FaxNumber: string;
    ShowDetails: boolean = false;
}

enum ShipmentDirections {
    Import = "I",
    Export = "E",
    Customs = "C"
}
