import
    {
        Component,
        ViewChild, ElementRef, AfterViewInit, HostListener
    } from '@angular/core';
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
@Component({
    selector: 'ShipmentDetailsComponent',
    templateUrl: './ShipmentDetailsComponent.html',
    styleUrls: ['./ShipmentDetailsComponent.css']
})
export class ShipmentDetailsComponent implements AfterViewInit
{

    @ViewChild('SliderWrapper') SliderWrapperElement: ElementRef;



    isLoading: boolean = false;
    showMoreReferences: boolean = false;
    SecurityKey: string = "";
    Shipment: CargoTrackingShipmentWithMilestones = null;
    public ShipmentWithMilestones: CargoTrackingShipmentWithMilestones;
    public toPortCode: string;
    public fromPortCode: string;
    ShipmentReferences: string[] = [];
    SearchText: string = "";
    CustomsBrokerReference: string;
    ShipmentPM: any;
    ShipmentPackages: any[];
    DocumentsFilings: any[];
    PartnerCards: PartnerCard[] = [];
    InlandTransportMode = 'I';
    OceanTransportMode = 'O';
    AirTransportMode = 'A';

    ShipmentCustomsData: CargoTrackingShipmentCustomsData;
    get tenant()
    {
        return CargoTrackingBrandingData.Tenant;
    }
    constructor(private router: Router,
        private route: ActivatedRoute,
        private searchService: CargoTrackingSearchService,
        private cargoTrackingPortService: CargoTrackingPortService,
        private cargoTrackingShipmentService: CargoTrackingShipmentService,
        private documentDownloadService: DocumentDownloadService)
    {

        this.GetIdFromURI();

    }
    ngAfterViewInit(): void
    {
        this.LoadShipment();
        setTimeout(() =>
        {
            this.InitSlider();
            this.BuildSliderCards();

        }, 200);
        this.CreatePartnerCardsFromShipmentPM();

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
        var screenwidth = window.innerWidth;

        var sliderWrapperWidth = this.SliderWrapperElement.nativeElement.offsetWidth;

        if (screenwidth > 470)
            var count = Math.floor((sliderWrapperWidth - PAGERS_WIDTH) / this.sliderCardWidth);


        this.sliderVisibleCardsCount = count;

        this.sliderVisibleCardsWidth = count * this.sliderCardWidth;
        this.sliderMarginCardCount = 0;
        this.sliderMarginLeft = screenwidth < 470 ? (this.sliderCardWidth + 55) * -1 : 0; // mobile: add

    }

    LoadShipment()
    {
        this.isLoading = true;
        this.searchService.getShipment(this.SecurityKey, this.tenant).subscribe((result: any) =>
        {
            this.isLoading = false;
            console.log("[getShipment]", result);
            this.ShipmentWithMilestones = result;
            if (this.ShipmentWithMilestones) {
                this.Shipment = result;
                this.ShipmentReferences = result.ShipmentList.CustomerReference ? result.ShipmentList.CustomerReference.split(',') : null;
                this.SetRoutingVariables();
                this.GetShipmentPM();
                this.GetShipmentCustomsData();
                this.GetShipmentPackages();
                this.GetDocumentsFilingsConnectedToShipment();

            }

            setTimeout(() =>
            {
                this.InitSlider();
                this.BuildSliderCards();

            }, 200);
        });
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
                this.InitRoutes();
            }
        });
    }

    GetShipmentPackages() {
        this.cargoTrackingShipmentService.GetShipmentPackages(this.Shipment.ShipmentList.EntityId).subscribe((result: any) => {
            if (result) {
                this.ShipmentPackages = result;
                console.log("GetShipmentPackages", this.ShipmentPackages);
            }
        });
    }

    GetDocumentsFilingsConnectedToShipment() {
        this.cargoTrackingShipmentService.GetDocumentsFilingsConnectedToShipment(this.Shipment.ShipmentList.EntityId).subscribe((result: any) => {
            if (result) {
                this.DocumentsFilings = result;
                console.log("DocumentsFilings", this.DocumentsFilings);
                this.isLoading = false;

            }
        });
    }
    PartnersAddresses: any[] = [];
    GetPartnersAddresses(){
        var partnersIds = this.GetShipmentPMPartnersIds();
        this.cargoTrackingShipmentService.GetPartnersAddresses(partnersIds).subscribe((result: any) =>
        {
            if (result) {
                this.PartnersAddresses = result;

                this.CreatePartnerCardsFromShipmentPM();
            }
        });
    }
    private GetShipmentPMPartnersIds()
    {
        var partnersIds =  [
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
        ];

        return partnersIds.filter(p=>p);
    }

    loadingCustomsData: boolean = false;
    GetShipmentCustomsData() {
        this.loadingCustomsData = true;
        this.cargoTrackingShipmentService.GetShipmentCustomsData(this.Shipment.ShipmentList.EntityId).subscribe((result: any) => {
            if (result) {
                this.ShipmentCustomsData = result;
                this.loadingCustomsData = false;
            }
        }, () => {
            this.loadingCustomsData = false;

        });
    }

    private FillCustomsBrokerReferenceFromShipmentPM()
    {
        this.CustomsBrokerReference = this.ShipmentPM.CustomFileNumber;
    }

    CreatePartnerCardsFromShipmentPM()
    {
        if (this.ShipmentPM) {

            if (this.ShipmentPM.ShipperId)
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
    sliderVisibleCardsCount: number = 5;
    sliderVisibleCardsWidth: number = 0;
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
                newCard.Date = milstone.Done ? milstone.Date : (milstone.EstimationDate || milstone.Date);
                newCard.Code = 'No. ' + milstone.Id;
                newCard.Title = milstone.Name;
                newCard.Description = milstone.Notes || 'This milestone does not have descriptions';
                newCard.IsDimmed = milstone.IsEstimation && !milstone.Done;
                newCard.IsActive = milstone.Id + '' == this.Shipment.ShipmentList.CurrentMilestoneCode;
                newCard.HasWarning = newCard.IsActive;
                return newCard;
            });
        // .sort((a, b) => {
        //     if (a.Date > b.Date) return 1;
        //     if (a.Date < b.Date) return -1;
        //      return 0;
        //     });
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
        if (panelElement)
            panelElement.scrollIntoView();
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
        agent.Name = this.ShipmentPM.AgentName;
        agent.Address = this.GetPartnerAddress(this.ShipmentPM.AgentId);
        agent.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.AgentId);
        agent.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.AgentId);
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
        consignee.Name = this.ShipmentPM.ConsigneeName;
        consignee.Address = this.GetPartnerAddress(this.ShipmentPM.ConsigneeId);
        consignee.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.ConsigneeId);
        consignee.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.ConsigneeId);
        return consignee;
    }

    private CreateShipperPartnerCard()
    {
        var shipper = new PartnerCard();
        shipper.Type = "shipper";
        shipper.Name = this.ShipmentPM.ShipperName;
        shipper.Address = this.GetPartnerAddress(this.ShipmentPM.ShipperId);
        shipper.PhoneNumber = this.GetPartnerPhoneNumberFromAddress(this.ShipmentPM.ShipperId);
        shipper.FaxNumber = this.GetPartnerFaxNumberFromAddress(this.ShipmentPM.ConsigneeId);
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
        this.CreatePickupsRoutesFromShipmentPM();
        this.CreateWarehouseLegRoutesFromShipmentPM();
        this.CreateMainCarriageLegsRoutesFromShipmentPM();
        this.CreateShipmentDeliveriesRoutesFromShipmentPM();
    }

    private CreatePickupsRoutesFromShipmentPM() {
        for (let i = 0; i < this.ShipmentPM.ShipmentPickUps.length; i++) {
            var step = new RoutingStep();
            step.TransportModeCode = this.InlandTransportMode;
            step.Description = "Via "+this.ShipmentPM.ShipmentPickUps[i].CarrierName;

            this.SetFromAndToLabelsForShipmentPickUpsRoutes(this.ShipmentPM.ShipmentPickUps[i], step);
            step.Directions = this.BuildRouteDirections(this.ShipmentPM.ShipmentPickUps[i]);

            this.ShipmentRouteSteps.push(step);
        }
    }

    private CreateWarehouseLegRoutesFromShipmentPM() {
        var step = new RoutingStep();
        step.TransportModeCode = this.InlandTransportMode;
        step.Description = this.ShipmentPM.WarehouseLegRemarks == null ? "WarehouseLeg" : this.ShipmentPM.WarehouseLegRemarks;
        step.FromPortLabel = this.ShipmentPM.WarehouseLegTerminalName;

        this.SetWarehouseLegDirections(step);

        this.ShipmentRouteSteps.push(step);
    }

    private CreateMainCarriageLegsRoutesFromShipmentPM() {
        for (let i = 0; i < this.ShipmentPM.MainCarriageLegs.length; i++) {
            var step = new RoutingStep();
            step.TransportModeCode = this.ShipmentPM.TransportModeId;
            step.Description = "Via " + this.ShipmentPM.MainCarriageCarrierName;
            step.FromPortLabel = this.ShipmentPM.MainCarriageFromPortCode;
            step.ToPortLabel = this.ShipmentPM.MainCarriageToPortCode;

            step.Directions = this.BuildRouteDirections(this.ShipmentPM.MainCarriageLegs[i]);

            this.ShipmentRouteSteps.push(step);
        }
    }

    private CreateShipmentDeliveriesRoutesFromShipmentPM() {
        for (let i = 0; i < this.ShipmentPM.ShipmentDeliveries.length; i++) {
            var step = new RoutingStep();
            step.TransportModeCode = this.InlandTransportMode;
            step.Description = "Via " + this.ShipmentPM.ShipmentDeliveries[i].CarrierName;
            step.FromPortLabel = this.ShipmentPM.ShipmentDeliveries[i].FromPortCode;
            step.ToPortLabel = this.ShipmentPM.ShipmentDeliveries[i].ToPortCode;

            step.Directions = this.BuildRouteDirections(this.ShipmentPM.ShipmentDeliveries[i]);

            this.ShipmentRouteSteps.push(step);
        }
    }

    private SetFromAndToLabelsForShipmentPickUpsRoutes(shipmentPickup: any, step: RoutingStep) {

        if (shipmentPickup.PickUpDeliveryFromTypeCode == "PORT")
        {
            step.FromPortLabel = shipmentPickup.FromPortCode;
        }

        else if (shipmentPickup.PickUpDeliveryFromTypeCode == "PART")
        {
            step.FromPortLabel = shipmentPickup.FromLocation.toString().split(" ")[0];
            step.ToolTipFromPortLabel = "Partner: \n" + shipmentPickup.FromLocation.toString().split("\r")[0];
        }

        else
        {
            step.FromPortLabel = shipmentPickup.FromAddressCountryCode;
            step.ToolTipFromPortLabel = "Address: \n" + shipmentPickup.FromAddressCity_Dummy + ',' + shipmentPickup.FromAddressCountryName;
        }

        if (shipmentPickup.PickUpDeliveryToTypeCode == "PORT")
        {
            step.ToPortLabel = shipmentPickup.ToPortCode;
        }

        else if (shipmentPickup.PickUpDeliveryToTypeCode == "PART")
        {
            step.ToPortLabel = shipmentPickup.ToLocation.toString().split(" ")[0];
            step.ToolTipToPortLabel = "Partner: \n" + shipmentPickup.ToLocation.toString().split("\r")[0];
        }

        else
        {
            step.ToPortLabel = shipmentPickup.ToAddressCountryCode;
            step.ToolTipToPortLabel = "Address: \n" + shipmentPickup.ToAddressCity_Dummy + ',' + shipmentPickup.ToAddressCountryName;
        }
    }

    BuildRouteDirections(shipmentRoute) {
        var directions = [];

        var direction = this.BuildExportRouteDirection(shipmentRoute, "ETD");
        if (direction)
            directions.push(direction);

        var direction = this.BuildImportRouteDirection(shipmentRoute, "ETA");
        if (direction)
            directions.push(direction);

        var direction = this.BuildExportRouteDirection(shipmentRoute, "ATD");
        if (direction)
            directions.push(direction);

        var direction = this.BuildImportRouteDirection(shipmentRoute, "ATA");
        if (direction)
            directions.push(direction);


        return directions;
    }

    private SetWarehouseLegDirections(step: RoutingStep) {
        if (this.ShipmentPM.WarehouseLegExpectedEntryDate != null) {
            step.Directions.push(new RouteDirection(this.ShipmentPM.WarehouseLegExpectedEntryDate, "ETD", "out"));
        }

        if (this.ShipmentPM.WarehouseLegExpectedReleaseDate != null) {
            step.Directions.push(new RouteDirection(this.ShipmentPM.WarehouseLegExpectedReleaseDate, "ETA", "in"));
        }

        if (this.ShipmentPM.WarehouseLegActualEntryDate != null) {
            step.Directions.push(new RouteDirection(this.ShipmentPM.WarehouseLegActualEntryDate, "ATD", "out"));
        }

        if (this.ShipmentPM.WarehouseLegActualReleaseDate != null) {
            step.Directions.push(new RouteDirection(this.ShipmentPM.WarehouseLegActualReleaseDate, "ATA", "in"));
        }
    }

    BuildExportRouteDirection(shipmentDelivary, fieldName: string) {
        if (shipmentDelivary[fieldName] != null) {
            return new RouteDirection(shipmentDelivary[fieldName], fieldName, "out");
        }
    }

    BuildImportRouteDirection(shipmentDelivary, fieldName: string) {
        if (shipmentDelivary[fieldName] != null) {
            return new RouteDirection(shipmentDelivary[fieldName], fieldName, "in");
        }
    }

    DownloadDocument(document: string) {
        this.documentDownloadService.DownloadPage(document);
    }

    DownloadAllClick(entityId: string) {
        this.documentDownloadService.DownloadAllPages(entityId);
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
