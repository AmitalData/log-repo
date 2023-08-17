import {
    Component,
    ViewChild,
    ElementRef,
    AfterViewInit,
    HostListener,
    Input,
    EventEmitter,
    OnInit,
    Inject
} from '@angular/core';
import {Router, ActivatedRoute, Data} from '@angular/router';

import {FormBuilder} from '@angular/forms';
import {CargoTrackingSearchService} from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import {CargoTrackingShipmentList} from 'src/CargoTracking/EntityLists/CargoTrackingShipmentList';
import {CargoTrackingBrandingData} from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import {
    CargoTrackingShipmentWithMilestones,
    Milestone
} from 'src/CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import {CargoTrackingPortService} from '../../../../Services/Others/CargoTrackingPortService';
import {CargoTrackingShipmentService} from '../../../../Services/Others/CargoTrackingShipmentService';
import {CargoTrackingShipmentCustomsData} from "../../../../DataContracts/CargoTrackingShipmentCustomsData";
import {DocumentDownloadService} from '../../../../Services/Others/DocumentDownloadService';
import {MessageWindowComponent} from '../../../../../Infrastructure/Components/MessageWindow/MessageWindowComponent';
import {CargoTrackingShipmentOrderService} from '../../../../Services/Others/CargoTrackingShipmentOrderService';
import {MatDialog} from '@angular/material/dialog';
import {DatePipe} from '@angular/common';
import {
    CargoTrackingShipmentExtendedService
} from 'src/CargoTracking/Services/Others/CargoTrackingShipmentExtendedService';
import {CargoTrackingShipmentMappedPM} from 'src/CargoTracking/DataContracts/CargoTrackingShipmentMappedPM';
import {
    CargoTrackingBrandingDataExtendedService
} from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import {ServiceHelper} from 'src/CargoTracking/Utilities/ServiceHelper';
import {ServiceResponse} from 'src/CargoTracking/DataContracts/ServiceResponse';
import {DeclarationApprovalArgs} from 'src/CargoTracking/DataContracts/DeclarationApprovalArgs';
import {RootContext} from 'src/CargoTracking/Utilities/RootContext';
import {MilestoneCodes} from 'src/CargoTracking/Constants/MilestoneCodes';

const mobileScreenMaxWidth = 470;

const approvalResponseMessage = 'הצהרה זו אושרה ע"י';
const declineResponseMessage = 'הצהרה זו נדחתה';

const orderShipmentTypeCode = 'O';
const declineText = 'דחיה';
const declineMessageText = 'אנא רשם/י סיבת הדחיה ואת שמך';
const confirmText = 'אישור';
const approvedByLabelText = 'שם המאשר/ת';
const cancelText = 'ביטול';

@Component({
    selector: 'ShipmentDetailsComponent',
    templateUrl: './ShipmentDetailsComponent.html',
    styleUrls: ['./ShipmentDetailsComponent.css',
        '../../UserDashboardComponent.css']
})
export class ShipmentDetailsComponent implements OnInit, AfterViewInit {

    @ViewChild('SliderWrapper') SliderWrapperElement: ElementRef;
    @ViewChild('slider') SliderElement: ElementRef;
    @ViewChild('RoutingSliderWrapper') RoutingSliderWrapperElement: ElementRef;


    @Input() DetailsSectionToggleEvent: EventEmitter<any> = new EventEmitter();

    cargoTrackingShipmentPM: CargoTrackingShipmentMappedPM = new CargoTrackingShipmentMappedPM();

    public isLoading: boolean = true;
    showMoreReferences: boolean = false;
    SecurityKey: string = "";
    Shipment: CargoTrackingShipmentWithMilestones = null;
    public ShipmentWithMilestones: CargoTrackingShipmentWithMilestones;
    public toPortCode: string;
    public fromPortCode: string;
    ShipmentReferences: string[] = [];
    SearchText: string = "";
    ShipmentPM: any;
    ShipmentOrder: any;
    HasReferences: boolean = false;
    HasContainersDetails: boolean = false;
    InlandTransportMode = 'I';
    WarehouseTransportMode = 'W'
    OceanTransportMode = 'O';
    AirTransportMode = 'A';
    ContainersNumbers: string[] = [];
    ShowDetailsSection: boolean = false;
    TitleOfCustomsOrForwarder: string = "";
    TitleOfCustomsOrForwarder_MB: string = "";
    ValueOfCustomsOrForwarder: string = "";
    CustomsEntityType: string = "C";
    ForwardingEntityType: string = "F";
    OrderEntityType: string = "O";
    RoutingPagersWidth: number = 100;
    RoutingMobilePagersWidth: number = 60;
    MaxWidthForMobileScreenForRouting: number = 470;
    RoutingMobileMarginLeft: number = 55;

    NoTaxDetails: boolean = false;
    public OverviewPanelTitle: string;
    public TypeTitle: string;
    isSharedLink: boolean = false;
    isDeclarationLink: boolean = false;
    hasApprovalDeclineResponse: boolean = false;
    noShipmentFound: boolean = false;
    approvalMessage: string;
    PartnersPanel: string = "PartnersPanel";
    EventsPanel: string = "EventsPanel";
    MaxHeightForPartnersPanel: number = 600;
    MaxNumberOfCarachterForMobile: number = 15;
    MobileReferencesViewCount = 1;
    WebReferencesViewCount = 3;
    PartnerCardTypesOfShipmentTransportMode = {
        'A': "AIRLINES",
        'I': "TRUCKER",
        'O': "SHIPPING LINES"
    };
    EntityType_Customs = "C";
    _tenant;
    focusOnPanel;
    baseURL;
    isBrandingDataLoaded;
    IFrameURI: string = "";
    IsPDF: boolean = false;
    get tenant() {
        return this._tenant || CargoTrackingBrandingData.Tenant;
    }

    get DeclarationApprovalEnabled() {
        const isCustomShipment = this.cargoTrackingShipmentPM.EntityType == this.EntityType_Customs;
        const haveResponse = this.cargoTrackingShipmentPM.ApprovedDate || this.cargoTrackingShipmentPM.DenyReason;
        const responseRequired = this.cargoTrackingShipmentPM.IsImporterApprovalRequried;
        return isCustomShipment
            && this.cargoTrackingShipmentPM.ActivatedForDeclarationApprove
            && (responseRequired || haveResponse);
    }
    get ShowDocumentMNO() {
        return this.cargoTrackingShipmentPM.ShowMoneyOrder;
    }
    get ShowShipmentAsDeclaration() {
        return this.isDeclarationLink && this.DeclarationApprovalEnabled;
    }

    get ShowNoShipmentFoundMessage() {
        const noDeclarationShipmentFound = this.isDeclarationLink && this.DeclarationApprovalEnabled && this.noShipmentFound;
        const noNormalShipmentFound = !this.isDeclarationLink && this.noShipmentFound;
        return noDeclarationShipmentFound || noNormalShipmentFound;
    }

    get ShowNotSupportedDeclarationMessage() {
        return this.isDeclarationLink && !this.DeclarationApprovalEnabled;
    }

    get IsOrderShipment() {
        return this.cargoTrackingShipmentPM.EntityType == orderShipmentTypeCode;
    }
    get ShowEventPanel() {
        return this.cargoTrackingShipmentPM.CargoTrackingPrivateShowEvents;
    }
    
    constructor(private router: Router,
                private route: ActivatedRoute,
                private cargoTrackingShipmentService: CargoTrackingShipmentService,
                private cargoTrackingShipmentExtendedService: CargoTrackingShipmentExtendedService,
                private brandingService: CargoTrackingBrandingDataExtendedService,
                private documentDownloadService: DocumentDownloadService,
                public dialog: MatDialog,
                private datePipe: DatePipe,
                @Inject('BASE_URL') baseUrl: string) {
        this.baseURL = baseUrl;

        this.GetIdFromURI();

    }

    ngOnInit(): void {

    }

    ngAfterViewInit(): void {
        this.LoadCargoShipmentPM();
    }


    private GetIdFromURI() {

        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;

        this.route.queryParams.subscribe(params => {
            this.SecurityKey = params['SecurityKey'] || this.SecurityKey;
            this._tenant = params['Tenant'];
            this.focusOnPanel = params['Panel'];
        });

        const data: Data = this.route.snapshot.data;
        this.isSharedLink = data?.isSharedLink;
        this.isDeclarationLink = data?.isDeclaration;
        if (this.isSharedLink) {
            this.GetBrandingData();
        }
    }


    LoadCargoShipmentPM() {
        if (this.isSharedLink) {
            this.GetMainShipmentByShipmentSecurityKey();
        } else {
            this.GetShipmentBySecurityKey();
        }

    }

    private GetShipmentBySecurityKey() {
        this.cargoTrackingShipmentExtendedService.GetCargoShipmentPMBySecurityKey(this.SecurityKey, this.tenant)
            .subscribe((result: any) => {
                this.isLoading = false;

                console.log("GetCargoShipmentPMBySecurityKey", this.ShipmentPM);
                if (result) {
                    this.InitializeComponent(result);
                } else {
                    this.noShipmentFound = true;
                }
            }, (error) => {
                this.noShipmentFound = true;
                this.isLoading = false;

            });
    }

    private GetMainShipmentByShipmentSecurityKey() {
        this.cargoTrackingShipmentExtendedService.GetMainCargoShipmentPMBySecurityKey(this.SecurityKey, this.tenant)
            .subscribe((result: any) => {
                this.isLoading = false;

                console.log("GetMainCargoShipmentPMBySecurityKey", this.ShipmentPM);
                if (result) {
                    this.InitializeComponent(result);
                } else {
                    this.noShipmentFound = true;
                }
            }, (error) => {
                this.noShipmentFound = true;
                this.isLoading = false;

            });
    }

    private InitializeComponent(result: any) {
        this.cargoTrackingShipmentPM = result; 
        this.BuildShipmentReferences();

        this.SetCustomsOrForwarderFields();
        this.SetOverviewPanelTitle();
        this.SetTypeTitle();

        this.SetContainersNumbers(result);
        this.SetHasContainersDetails();

        this.NoTaxDetails = this.cargoTrackingShipmentPM?.CustomsData?.TaxDetails?.length == 0 ? true : false;


        setTimeout(() => {
            this.InitSlider();
            this.InitRoutingSlider();
            this.BuildSliderCards();

            if (this.isSharedLink && this.focusOnPanel)
                this.ScrollToPanel(this.focusOnPanel);

        }, 200);


        this.SetDeclarationMessage();
        this.SetDocumentPDF();
    }

   
    private SetDocumentPDF() {
       
        var DocumentToShow = this.cargoTrackingShipmentPM.DocumentsFilings.filter(x => x.DocumentTypeCode == "MNO");
        this.cargoTrackingShipmentPM.DocumentsFilings=this.cargoTrackingShipmentPM.DocumentsFilings.sort((a, b) => {
            if (a.CreateDate < b.CreateDate) {
              return 1; 
            }
            if (a.CreateDate > b.CreateDate) {
              return -1; 
            }
            return 0; 
          });
          
          
          
        if(DocumentToShow==null||DocumentToShow.length==0) {
            this.IsPDF=false;
        }
        else 
        {
         
            if(DocumentToShow.length>1) DocumentToShow = DocumentToShow.sort((a, b) => <any>new Date(b.CreateDate) - <any>new Date(a.CreateDate));

            this.cargoTrackingShipmentExtendedService.GetFilingAttachPdfReport(DocumentToShow[0].DocumentId,this.tenant).subscribe((response: ServiceResponse) => {
               if (response) {
                this.IsPDF=true

                   var buffer = this.base64ToBufferConvertor(response.toString());
                   var blob = new Blob([buffer], { type: 'application/pdf' });
                   var objectURL = URL.createObjectURL(blob);
                   this.IFrameURI = objectURL;
               }
            });
        
        }

      


      
    }
    public  base64ToBufferConvertor(str: string) {
        str = window.atob(str); // creates a ASCII string
        var buffer = new ArrayBuffer(str.length),
          view = new Uint8Array(buffer);
        for (var i = 0; i < str.length; i++) {
          view[i] = str.charCodeAt(i);
        }
    
        return buffer;
    
    }
    private BuildShipmentReferences() {
        this.ShipmentReferences = this.cargoTrackingShipmentPM.CustomerReference ?
            this.cargoTrackingShipmentPM.CustomerReference.split(',').filter(d => d) : [];

        this.ShipmentReferences = this.ShipmentReferences.map((el) => {
            return el.trim();
        });

        if (this.cargoTrackingShipmentPM.EntityType === 'O') {
            this.AddShipmentReferencesForOrderShipment();
        }
        this.ShipmentReferences = this.ShipmentReferences.filter((el, i, a) => i === a.indexOf(el));
    }

    private AddShipmentReferencesForOrderShipment() {
        if (this.cargoTrackingShipmentPM.ShipmentOrderPONumber != null)
            this.ShipmentReferences.push(this.cargoTrackingShipmentPM.ShipmentOrderPONumber);
        if (this.cargoTrackingShipmentPM.SHOBookingConfirmationNumber != null)
            this.ShipmentReferences.push(this.cargoTrackingShipmentPM.SHOBookingConfirmationNumber);
    }

    OpenReferencesMessageWindow(references: any[], isMobile: boolean ,event) {
        if (!references)
            return;

        references = references.filter(d => d).map(x => x.trim());
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'References',
                description: isMobile ? references.slice(1, references.length + 1).join("\n") : references.slice(3, references.length + 1).join("\n"),
            },
            
            position: {
                top: event.clientY + 'px',
                left: event.clientX + 'px',
              },
          
        });
    }

    private SetDeclarationMessage() {
        if (this.isSharedLink && this.isDeclarationLink) {
            const isCustomShipment = this.cargoTrackingShipmentPM.EntityType == this.EntityType_Customs;
            this.hasApprovalDeclineResponse = isCustomShipment && this.cargoTrackingShipmentPM.ActivatedForDeclarationApprove && !this.cargoTrackingShipmentPM.IsImporterApprovalRequried;

            if (this.hasApprovalDeclineResponse) {
                if (this.cargoTrackingShipmentPM.ApprovedDate)
                    this.SetApprovedDeclarationMessage();
                else
                    this.SetDeniedDeclarationMessage();

            } else {
                this.approvalMessage = this.cargoTrackingShipmentPM.TenantDeclarationMessage;
            }

        }
    }

    private SetDeniedDeclarationMessage() {
        const denyDate = this.datePipe.transform(this.cargoTrackingShipmentPM.DenyDate, 'dd/MM/yyyy, HH:mm');
        this.approvalMessage = declineResponseMessage + ' "' + this.cargoTrackingShipmentPM.DenyReason + '"' + ' תאריך ' + denyDate;
    }

    private SetApprovedDeclarationMessage() {
        const approvedDate = this.datePipe.transform(this.cargoTrackingShipmentPM.ApprovedDate, 'dd/MM/yyyy, HH:mm');
        this.approvalMessage = approvalResponseMessage + ' ' + this.cargoTrackingShipmentPM.ApprovedByUserName + ' בתאריך ' + approvedDate;
    }

    private GetBrandingData() {
        this.brandingService
            .GetUserDashboardBrandingData(ServiceHelper.GetcargoTrackingDataRequest(this.baseURL))
            .subscribe((response: ServiceResponse) => {
                if (response.Result) {
                    if (response?.Result?.ForceHttps)
                        this.RedirectAppToHttps();

                    ServiceHelper.SetCargoTrackingDate(response.Result, this.baseURL);

                    this.isBrandingDataLoaded = true;
                }
            });
    }

    RedirectAppToHttps() {
        const isLocally = window.location.origin.indexOf('localhost') > -1;

        if (!isLocally && location.protocol === 'http:') {
            window.location.href = location.href.replace('http', 'https');
        }
    }

    private SetContainersNumbers(result: any) {
        this.ContainersNumbers = [];
        this.ContainersNumbers = result.ContainersNumbers ? result.ContainersNumbers.split(',') : null;
    }

    get CompanyLogo() {
        return CargoTrackingBrandingData.ComapnylogoURL;
    }

    get BrowserIcon() {
        return CargoTrackingBrandingData.BrowserIconURL;
    }

    get BackGroundImg() {
        return CargoTrackingBrandingData.BackgroundURL;
    }

    get ShipmentHeaderImage() {
        return CargoTrackingBrandingData.ShipmentHeaderURL;
    }

    public get InvertedLogoURL() {
        return CargoTrackingBrandingData.InvertedLogoURL;
    }


    private SetOverviewPanelTitle() {
        if (this.cargoTrackingShipmentPM.EntityType == this.OrderEntityType) {
            this.OverviewPanelTitle = "SHO Overview";

        } else {
            this.OverviewPanelTitle = "Overview";
        }
    }

    @HostListener('window:resize', ['$event'])
    onResize() {
        //event.target.innerWidth;
        this.InitSlider();
        this.InitRoutingSlider();

    }

    onMousewheelOnMilestonesSlider(event: WheelEvent) {
        let scrollEnds = false;
        if (event.deltaY > 0) {
            scrollEnds = this.MoveSlider('left');
        }
        if (event.deltaY < 0) {
            scrollEnds = this.MoveSlider('right');
        }

        if (!scrollEnds)
            event.preventDefault();

    }

    onMousewheelOnRoutingSlider(event: WheelEvent) {
        let scrollEnds = false;
        if (event.deltaY > 0) {
            scrollEnds = this.MoveRoutingSlider('left');
        }
        if (event.deltaY < 0) {
            scrollEnds = this.MoveRoutingSlider('right');
        }

        if (!scrollEnds)
            event.preventDefault();
    }

    logPan(i) {
        console.log(i);

    }

    InitSlider() {
        if (!this.SliderWrapperElement)
            return;

        this.SetSliderVisibleCardsWrapperWidth();

        this.sliderMarginCardCount = 0;
        this.sliderMarginLeft = 0;
    }

    private SetSliderVisibleCardsWrapperWidth() {
        const webPagersWidth = 200;
        const mobilePagersWidth = 60;

        const sliderWrapperWidth = this.SliderWrapperElement.nativeElement.offsetWidth;

        const extraOffset = 20;
        if (this.IsMobileView)
            var mobileCount = Math.floor((sliderWrapperWidth - mobilePagersWidth) / this.sliderMobileCardWidth);

        else
            var webCount = Math.floor((sliderWrapperWidth - webPagersWidth) / this.sliderCardWidth);

        this.sliderVisibleCardsCount = mobileCount || webCount;

        if (this.IsMobileView) {
            const extraOffset = 10;
            this.sliderVisibleCardsWidth = this.sliderMobileCardWidth * this.sliderVisibleCardsCount + extraOffset;
        } else {
            this.sliderVisibleCardsWidth = this.sliderVisibleCardsCount * this.sliderCardWidth;
        }
    }

    InitRoutingSlider() {
        var screenwidth = window.innerWidth;
        var sliderWrapperWidth = this.RoutingSliderWrapperElement.nativeElement.offsetWidth;
        var count = this.CalculateRoutingSliderVisibleCardsWidth(screenwidth, sliderWrapperWidth);

        //this.routingSliderVisibleCardsWidth = this.routingSliderVisibleCardsCount * this.routingSliderCardWidth;
        this.routingSliderMarginCardCount = 0;
        this.routingSliderMarginLeft = 0;

    }

    private CalculateRoutingSliderVisibleCardsWidth(screenwidth: number, sliderWrapperWidth: any) {
        if (screenwidth > this.MaxWidthForMobileScreenForRouting) {
            this.CalculateVisibleSliderCardsCountForWeb(sliderWrapperWidth);
        }

        if (screenwidth <= this.MaxWidthForMobileScreenForRouting) {
            this.CalculateVisibleSliderCardsCountForMobile(sliderWrapperWidth);
        }
    }

    private CalculateVisibleSliderCardsCountForWeb(sliderWrapperWidth: any) {
        this.routingSliderVisibleCardsCount = (sliderWrapperWidth - this.RoutingPagersWidth) / this.routingSliderCardWidth;
        this.routingSliderVisibleCardsWidth = this.routingSliderVisibleCardsCount * this.routingSliderCardWidth;

    }

    private CalculateVisibleSliderCardsCountForMobile(sliderWrapperWidth: number) {
        this.routingSliderCardWidth = 300;
        this.RoutingMobilePagersWidth = sliderWrapperWidth - this.routingSliderCardWidth;
        this.routingSliderVisibleCardsWidth = 300;
        this.routingSliderVisibleCardsCount = (sliderWrapperWidth - this.RoutingMobilePagersWidth) / this.routingSliderCardWidth;
    }


    SetTitleForSupplierOrClient(shipment: any) {
        let title;

        switch (shipment.DirectionId) {
            case ShipmentDirections.Import: {
                title = 'SHIPPER';
                break;
            }
            case ShipmentDirections.Export: {
                title = 'CLIENT';
                break;
            }
            case ShipmentDirections.Drop: {
                if (shipment.ShipmentNumber.startsWith('EF') || shipment.ShipmentNumber.startsWith('MF')) {
                    title = 'CLIENT';
                } else {
                    title = 'SHIPPER';
                }
                break;
            }
        }

        if (shipment.EntityType === this.EntityType_Customs) {
            title = 'SHIPPER';
        }

        return title;
    }

    SetSupplierOrClientValueByDirection(shipment) {
        let name;

        switch (shipment.DirectionId) {
            case ShipmentDirections.Import: {
                name = shipment.ShipperName;
                break;
            }
            case ShipmentDirections.Export: {
                name = shipment.ConsigneeName;
                break;
            }
            case ShipmentDirections.Drop: {
                if (shipment.ShipmentNumber.startsWith('EF') || shipment.ShipmentNumber.startsWith('MF')) {
                    name = shipment.ConsigneeName;
                } else {
                    name = shipment.ShipperName;
                }
                break;
            }
        }
        if (shipment.EntityType === this.EntityType_Customs) {
            name = shipment.ShipperName;
        }
        return name;
    }

    SetTypeTitle() {
        
       if (this.cargoTrackingShipmentPM.EntityType == 'O') {
            this.TypeTitle = "SHIPMENT TYPE";
        }
        else 
        this.TypeTitle = "PACKAGE TYPE";
    }

    SetHasContainersDetails() {
        return this.cargoTrackingShipmentPM.Packages.length == 0 ? false : true;
    }


    SetCustomsOrForwarderFields() {
        this.SetTitleOfCustomsOrForwarder();
        this.SetValueOfCustomsOrForwarder();
    }

    SetTitleOfCustomsOrForwarder() {
        if (this.cargoTrackingShipmentPM.EntityType == this.CustomsEntityType) {
            this.TitleOfCustomsOrForwarder = "Customs Broker References";
            this.TitleOfCustomsOrForwarder_MB = "Customs Broker Ref.";
        }
        if (this.cargoTrackingShipmentPM.EntityType == this.ForwardingEntityType) {
            this.TitleOfCustomsOrForwarder = "Forwarder Reference";
            this.TitleOfCustomsOrForwarder_MB = "Forwarder Ref.";
        }
        if (this.cargoTrackingShipmentPM.EntityType == this.OrderEntityType) {
            this.TitleOfCustomsOrForwarder = "SHO References";
            this.TitleOfCustomsOrForwarder_MB = "SHO References";
        }
    }

    SetValueOfCustomsOrForwarder() {
        this.ValueOfCustomsOrForwarder = this.cargoTrackingShipmentPM.ShipmentNumber;
        if (!this.cargoTrackingShipmentPM.ConnectedShipmentsNumbers) {
            return;
        }
        this.ValueOfCustomsOrForwarder = this.cargoTrackingShipmentPM.ShipmentNumber + "\n" + this.cargoTrackingShipmentPM.ConnectedShipmentsNumbers;

    }

    //#region Slider
    SliderCards: MilestoneCard[] = [];
    sliderMarginLeft: number = 0;
    sliderMarginCardCount: number = 0;
    sliderCardWidth: number = 164;
    sliderMobileCardWidth: number = 133;
    sliderVisibleCardsCount: number = 5;
    sliderVisibleCardsWidth: number = 0;
    NoMilstonesFound: boolean = false;

    BuildSliderCards() {
        let currentDate = new Date();
        if (!this.cargoTrackingShipmentPM.Milestones)
            return;
        this.SliderCards = this.cargoTrackingShipmentPM.Milestones
            .filter(milstone => {
                var date = milstone.EstimationDate || milstone.Date;
                if (date && !milstone.InActive)
                    return true;
                return false;
            })
            .sort((a, b) => {
                if (a.Weight > b.Weight) return 1;
                if (a.Weight < b.Weight) return -1;
                return 0;
            })
            .map((milstone: Milestone) => {
                var newCard = new MilestoneCard();
                var CurrentMilestoneExceptions = this.cargoTrackingShipmentPM.CurrentMilestoneExceptions;
                newCard.Date = milstone.Done ? (milstone.Date || milstone.EstimationDate) : (milstone.EstimationDate || milstone.Date);
                newCard.ExpectedDate = milstone.Date ? null : milstone.EstimationDate;
                newCard.Code = 'No. ' + milstone.Code;
                newCard.Title = milstone.Name;

                let cardDateTime = new Date(newCard.Date ? newCard.Date : newCard.ExpectedDate);
                var cardDate = new Date(cardDateTime.getFullYear(), cardDateTime.getMonth(), cardDateTime.getDate());

                newCard.IsDimmed = (milstone.IsEstimation && !milstone.Done) || cardDate.getTime() > currentDate.getTime();
                newCard.IsActive = milstone.Code + '' == this.cargoTrackingShipmentPM.CurrentMilestoneCode;
                newCard.HasWarning = milstone.IsCurrent && CurrentMilestoneExceptions != null;
                newCard.WarningMessage = newCard.HasWarning ? CurrentMilestoneExceptions.substring(CurrentMilestoneExceptions.indexOf(',') + 1,) : null;
                newCard.WarningDate = newCard.HasWarning ? this.datePipe.transform(CurrentMilestoneExceptions?.split(',')[0], 'dd/MM/yyyy, HH:mm') : null;
                return newCard;
            });
        this.SetNoMilstonesFound();

        this.ScrollIntoLastSliderCard();


    }

    ScrollIntoLastSliderCard() {
        const hiddenCardsCount = this.SliderCards.length - this.sliderVisibleCardsCount;
        const width = this.IsMobileView ? this.sliderMobileCardWidth : this.sliderCardWidth;

        if (hiddenCardsCount > 0) {
            this.sliderMarginLeft = hiddenCardsCount * width * -1;
            this.sliderMarginCardCount = hiddenCardsCount;
        }
    }

    SetNoMilstonesFound() {
        if (this.SliderCards.length == 0) this.NoMilstonesFound = true;
    }

    MoveSlider(dir) {

        if (dir == 'right' && this.sliderMarginLeft == 0)
            return true;

        if (dir == 'left' && ((this.sliderMarginCardCount + this.sliderVisibleCardsCount) >= this.SliderCards.length) || (this.sliderVisibleCardsCount >= this.SliderCards.length))
            return true;

        let margin = this.sliderMarginLeft;
        const cardWidth = this.IsMobileView ? this.sliderMobileCardWidth : this.sliderCardWidth

        // inc\dec
        if (dir == 'left') {
            margin -= cardWidth;
            this.sliderMarginCardCount++;
        } else {
            margin += cardWidth;
            this.sliderMarginCardCount--;
        }

        // limit boundary
        if (margin > 0)
            this.sliderMarginLeft = 0;
        else
            this.sliderMarginLeft = margin;

        console.log("sliderMarginCardCount:sliderMarginLeft === ", this.sliderMarginCardCount, '\t', this.sliderMarginLeft);


    }


    get IsMobileView() {
        const isPortrait = window.innerHeight > window.innerWidth;
        return (window.innerWidth <= mobileScreenMaxWidth && isPortrait)
            || (window.innerHeight <= mobileScreenMaxWidth && !isPortrait);
    }

    get IsNotMobileView() {
        return !this.IsMobileView
    }

    //#endregion

    //#region Routing Slider
    routingSliderMarginLeft: number = 0;
    routingSliderMarginCardCount: number = 0;
    routingSliderCardWidth: number = 200;
    routingSliderMobileCardWidth: number = 180;
    routingSliderVisibleCardsCount: number = 1;
    routingSliderVisibleCardsWidth: number = 0;


    MoveRoutingSlider(direction) {

        if (direction == 'right' && this.routingSliderMarginLeft == 0)
            return true;

        if (direction == 'left' && ((this.routingSliderMarginCardCount + this.routingSliderVisibleCardsCount) >= this.cargoTrackingShipmentPM.RoutingSteps.length) || (this.routingSliderVisibleCardsCount >= this.cargoTrackingShipmentPM.RoutingSteps.length))
            return true;

        var margin = this.SetRoutingSliderMarginBasedOnDirection(direction);

        this.SetRoutingSliderMarginLeft(margin);

        var screenwidth = window.innerWidth;
        if (screenwidth < this.MaxWidthForMobileScreenForRouting)
            this.routingSliderMarginLeft - this.RoutingMobileMarginLeft;


    }

    private SetRoutingSliderMarginLeft(margin: number) {
        if (margin > 0)
            this.routingSliderMarginLeft = 0;

        else
            this.routingSliderMarginLeft = margin;
    }

    private SetRoutingSliderMarginBasedOnDirection(direction: any) {
        var margin = this.routingSliderMarginLeft;

        if (direction == 'left') {
            margin -= this.routingSliderCardWidth;
            this.routingSliderMarginCardCount++;
        } else {
            margin += this.routingSliderCardWidth;
            this.routingSliderMarginCardCount--;
        }
        return margin;
    }

    //#endregion

    GetSlice(text: string, numberOfCharacter) {

        var result = text
        if (text?.length > numberOfCharacter && !this.IsMobileView) {
            if (!this.ContainsHebrew(text))
                result = text.slice(0, numberOfCharacter) + "..."
            else
                result = "..." + text.slice(0, numberOfCharacter)

        }
        if (text?.length > this.MaxNumberOfCarachterForMobile && this.IsMobileView) {
            if (!this.ContainsHebrew(text))
                result = text.slice(0, this.MaxNumberOfCarachterForMobile) + "..."
            else
                result = "..." + text.slice(0, this.MaxNumberOfCarachterForMobile)
        }
        return result;

    }

    ContainsHebrew(str: string) {
        return (/[\u0590-\u05FF]/).test(str)
    }

    GetModeIcon() {
        var iconPath = "";
        switch (this.cargoTrackingShipmentPM.TransportModeId) {
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

    PanelsNavigatorClicked(panelName: string) {
        this.ScrollToPanel(panelName);
    }

    private ScrollToPanel(panelName: string) {
        this.selectedNavButton = panelName;
        var panelElement = document.getElementById(panelName) as HTMLElement;
        if (panelElement) {
            panelElement.scrollIntoView();
            if ((panelName == this.PartnersPanel && panelElement.clientHeight > this.MaxHeightForPartnersPanel&&!this.ShowEventPanel) || (panelName != this.EventsPanel&&this.ShowEventPanel)||(panelName != this.PartnersPanel&&!this.ShowEventPanel) )
                document.getElementsByTagName('html')[0].scrollTop -= 113;

        }


    }

    BackLinkClicked() {
        this.router.navigate(['cargo-tracking', 'shipments']);
    }

    DownloadAllClick(securityKey: string) {
        this.documentDownloadService.ExternalDownloadAllDocuments(securityKey, this.cargoTrackingShipmentPM.ForwardingShipmentHeaderId, this.tenant);
    }

    DownloadDocument(item) {
        this.documentDownloadService.ExternalDownloadPage(item.SecurityId, this.tenant, this.cargoTrackingShipmentPM.ShipmentNumber + '_' + item.DocumentTypeName);
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
        references = references.map(x => x.trim()).filter(d => d);
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

    ChangeShowDetailsSectionValue(newValue) {
        this.ShowDetailsSection = newValue;
    }

    ApproveDeclaration() {
        const confirmMessage = this.ShowDeclarationApproveConfirmMessage();
        confirmMessage.afterClosed().subscribe(windowArgs => {
            const button = windowArgs?.button;
            if (button == 'ok') {
                this.SendDeclarationApproveRequest(windowArgs.textValue);
            }
        });
    }

    private SendDeclarationApproveRequest(approvedBy: string) {
        RootContext.StartBusyIndicatorLoading();

        const args = this.BuildDeclarationApprovalArguments(approvedBy);
        this.cargoTrackingShipmentExtendedService.PostDeclarationApprovalResponse(args)
            .subscribe(
                res => {
                    this.ShowDeclarationApprovedMessage();
                    this.GetMainShipmentByShipmentSecurityKey();
                    RootContext.StopBusyIndicator();
                },
                err => {
                    this.ShowFailureMessage(err);
                    RootContext.StopBusyIndicator();
                }
            );
    }

    DenyDeclaration() {

        const denyConfirmMessage = this.ShowDeclarationDeclineConfirmMessage();
        denyConfirmMessage.afterClosed().subscribe(windowArgs => {
            const button = windowArgs?.button;
            if (button == 'ok') {
                this.SendDeclarationDeclineRequest(windowArgs.textValue);
            }
        });
    }

    private ShowDeclarationDeclineConfirmMessage() {
        return this.dialog.open(MessageWindowComponent, {
            data: {
                title: declineText,
                description: declineMessageText,
                showOkButton: true,
                okButtonText: confirmText,
                showCancelButton: true,
                cancelButtonText: cancelText,
                showMultilineTextBox: true,
                inputRequired: true
            }
        });
    }

    private ShowDeclarationApproveConfirmMessage() {
        return this.dialog.open(MessageWindowComponent, {
            data: {
                title: confirmText,
                description: approvedByLabelText,
                showOkButton: true,
                okButtonText: confirmText,
                showCancelButton: true,
                cancelButtonText: cancelText,
                showTextBox: true,
                inputRequired: true

            }
        });
    }

    private SendDeclarationDeclineRequest(denyMessage: string) {
        RootContext.StartBusyIndicatorLoading();

        const args = this.BuildDeclarationDeclineArguments(denyMessage);
        this.cargoTrackingShipmentExtendedService.PostDeclarationApprovalResponse(args)
            .subscribe(arg => {
                RootContext.StopBusyIndicator();

                this.dialog.open(MessageWindowComponent, {
                    data: {
                        description: 'Declaration Declined',
                        showOkButton: true
                    }
                });
                this.GetMainShipmentByShipmentSecurityKey();

            }, (error) => {
                RootContext.StopBusyIndicator();
                this.ShowFailureMessage(error);
            });
    }

    private BuildDeclarationDeclineArguments(denyMessage: string) {
        let args = new DeclarationApprovalArgs();
        args.Tenant = Number(this.tenant);
        args.Denied = true;
        args.ShipmentSecurityKey = this.SecurityKey;
        args.DenyReason = denyMessage;
        return args;
    }

    private BuildDeclarationApprovalArguments(approvedBy: string) {
        let args = new DeclarationApprovalArgs();
        args.Tenant = Number(this.tenant);
        args.Approved = true;
        args.ApprovedBy = approvedBy;
        args.ShipmentSecurityKey = this.SecurityKey;
        return args;
    }

    private ShowDeclarationApprovedMessage() {
        return this.dialog.open(MessageWindowComponent, {
            data: {
                description: 'Approved Successfully',
                showOkButton: true
            }
        });
    }

    private ShowFailureMessage(error: any) {
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'Failed',
                description: error || 'Something went bad'
            }
        });
    }

    masterLabel = 'Master';
    houseLabel = 'House';

    SetMasterOrHouseLabel() {
        if (this.cargoTrackingShipmentPM.EntityType == ShipmentEntityTypes.Customs && this.cargoTrackingShipmentPM.ForwardingShipmentHeaderId && this.cargoTrackingShipmentPM.ForwardingShipmentLevelCode == ShipmentLevelCodes.House && this.cargoTrackingShipmentPM.ForwardingHouse) {
            return this.houseLabel;
        } else if (this.cargoTrackingShipmentPM.EntityType == ShipmentEntityTypes.Customs && this.cargoTrackingShipmentPM.ForwardingShipmentHeaderId && this.cargoTrackingShipmentPM.ForwardingShipmentLevelCode == ShipmentLevelCodes.Direct && this.cargoTrackingShipmentPM.ForwardingMaster) {
            return this.masterLabel;
        } else if (this.cargoTrackingShipmentPM.EntityType == ShipmentEntityTypes.Forwarding && this.cargoTrackingShipmentPM.ShipmentLevelCode == ShipmentLevelCodes.House && this.cargoTrackingShipmentPM.House) {
            return this.houseLabel;
        } else if (this.cargoTrackingShipmentPM.EntityType == ShipmentEntityTypes.Forwarding && this.cargoTrackingShipmentPM.ShipmentLevelCode == ShipmentLevelCodes.Direct && this.cargoTrackingShipmentPM.Master) {
            return this.masterLabel;
        } else if (this.cargoTrackingShipmentPM.House) {
            return this.houseLabel;
        } else if (this.cargoTrackingShipmentPM.Master) {
            return this.masterLabel;
        }
        return null;

    }

    SetMasterOrHouseValue() {
        if (this.cargoTrackingShipmentPM.EntityType == ShipmentEntityTypes.Customs && this.cargoTrackingShipmentPM.ForwardingShipmentHeaderId && this.cargoTrackingShipmentPM.ForwardingShipmentLevelCode == ShipmentLevelCodes.House && this.cargoTrackingShipmentPM.ForwardingHouse) {
            return this.cargoTrackingShipmentPM.ForwardingHouse;
        } else if (this.cargoTrackingShipmentPM.EntityType == ShipmentEntityTypes.Customs && this.cargoTrackingShipmentPM.ForwardingShipmentHeaderId && this.cargoTrackingShipmentPM.ForwardingShipmentLevelCode == ShipmentLevelCodes.Direct && this.cargoTrackingShipmentPM.ForwardingMaster) {
            return this.cargoTrackingShipmentPM.ForwardingMaster;
        } else if (this.cargoTrackingShipmentPM.EntityType == ShipmentEntityTypes.Forwarding && this.cargoTrackingShipmentPM.ShipmentLevelCode == ShipmentLevelCodes.House && this.cargoTrackingShipmentPM.House) {
            return this.cargoTrackingShipmentPM.House;
        } else if (this.cargoTrackingShipmentPM.EntityType == ShipmentEntityTypes.Forwarding && this.cargoTrackingShipmentPM.ShipmentLevelCode == ShipmentLevelCodes.Direct && this.cargoTrackingShipmentPM.Master) {
            return this.cargoTrackingShipmentPM.Master;
        } else if (this.cargoTrackingShipmentPM.House) {
            return this.cargoTrackingShipmentPM.House;
        } else if (this.cargoTrackingShipmentPM.Master) {
            return this.cargoTrackingShipmentPM.Master;
        }

        return null;

    }

    checkPartnerTypePermissionSharedAccess(partnerTypeName): boolean {
        if (this.cargoTrackingShipmentPM.SharedLogisticsSetting) {
            switch (partnerTypeName) {
                case PartnerTypeNames.COLLECTOR:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCollectorShared;
                case PartnerTypeNames.SALESMAN:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsSalesmanShared;
                case PartnerTypeNames.ACCOUNTMANAGER:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsAccountManagerShared;
                case PartnerTypeNames.ReleasingAgent:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsReleasingAgentShared;
                case PartnerTypeNames.Customer:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCustomerShared;
                case PartnerTypeNames.IssuingCarrierAgent:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsIssuingCarrierAgentShared;
                case PartnerTypeNames.Consolidator:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsConsolidatorShared;
                case PartnerTypeNames.CustomClearancePoint:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCustomClearancePoinShared;
                case PartnerTypeNames.CustomAgentImport:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCustomsAgentImportShared;
                case PartnerTypeNames.CustomAgentExport:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCustomsAgentExportShared;
                case PartnerTypeNames.Coloader:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsColoaderShared;
                case PartnerTypeNames.FreightForwarder:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsFreightForwarderShared;
                case PartnerTypeNames.Notify1:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsNotify1Shared;
                case PartnerTypeNames.Notify2:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsNotify2Shared;
                case PartnerTypeNames.ConsigneeNotImporter:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsConsigneeNotImporterShared;
                case PartnerTypeNames.ShipperNotExporter:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsShipperNotExporterShared;
                case PartnerTypeNames.Agent:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsAgentShared;
                case PartnerTypeNames.consignee:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsConsigneeShared;
                case PartnerTypeNames.shipper:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsShipperShared;
                case PartnerTypeNames.Carrier:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsMainCarrierShared;
                case PartnerTypeNames.Trucker:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsPickDelivCarriesShared;
                default:
                    return false
            }
        }
        return true;
    }

    checkPartnerTypePermissionContactAccess(partnerTypeName): boolean {
        if (this.cargoTrackingShipmentPM.SharedLogisticsSetting) {
            switch (partnerTypeName) {
                case PartnerTypeNames.COLLECTOR:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCollectorShowContactTS;
                case PartnerTypeNames.SALESMAN:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsSalesmanShowContactTS;
                case PartnerTypeNames.ACCOUNTMANAGER:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsAccountManagerShowContactTS;
                case PartnerTypeNames.ReleasingAgent:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsReleasingAgentShowContactTS;
                case PartnerTypeNames.Customer:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCustomerShowContactTS;
                case PartnerTypeNames.IssuingCarrierAgent:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsIssuingCarAgentShowContactTS;
                case PartnerTypeNames.Consolidator:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsConsolidatorShowContactTS;
                case PartnerTypeNames.CustomClearancePoint:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCustomCleaPointShowContactTS;
                case PartnerTypeNames.CustomAgentImport:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCustomAgentImShowContactTS;
                case PartnerTypeNames.CustomAgentExport:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsCustomAgentExShowContactTS;
                case PartnerTypeNames.Coloader:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsColoaderShowContactTS;
                case PartnerTypeNames.FreightForwarder:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsFreightForwardShowContactTS;
                case PartnerTypeNames.Notify1:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsNotify1ShowContactTS;
                case PartnerTypeNames.Notify2:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsNotify2ShowContactTS;
                case PartnerTypeNames.ConsigneeNotImporter:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsConsigneeNotImShowContactTS;
                case PartnerTypeNames.ShipperNotExporter:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsShipperNotExShowContactTS;
                case PartnerTypeNames.Agent:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsAgentShowContactTS;
                case PartnerTypeNames.consignee:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsConsigneeShowContactTS;
                case PartnerTypeNames.shipper:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsShipperShowContactTS;
                case PartnerTypeNames.Carrier:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsMainCarShowContactTS;
                case PartnerTypeNames.Trucker:
                    return this.cargoTrackingShipmentPM.SharedLogisticsSetting.IsPickDelivCarShowContactTS;
                default:
                    return true
            }
        }
        return true;
    }
}


export class MilestoneCard {
    Code: string;
    Date: Date;
    ExpectedDate: Date;
    Title: string;
    Description: string;
    IsActive: boolean;
    HasWarning: boolean;
    IsDimmed: boolean;
    WarningMessage: string;
    WarningDate: string;
}

export enum ShipmentDirections {
    Import = "I",
    Export = "E",
    Drop = "R"
}

export enum PartnerTypeNames {
    COLLECTOR = 'COLLECTOR',
    SALESMAN = 'SALESMAN',
    ACCOUNTMANAGER = 'ACCOUNT MANAGER',
    ReleasingAgent = 'Releasing Agent',
    Customer = 'Customer',
    IssuingCarrierAgent = "Issuing Carrier's Agent",
    Consolidator = 'Consolidator',
    CustomClearancePoint = 'Custom Clearance Point',
    CustomAgentImport = 'Custom Agent Import',
    CustomAgentExport = 'Custom Agent Export',
    Coloader = 'Coloader',
    FreightForwarder = 'Freight Forwarder',
    Notify1 = 'Notify 1',
    Notify2 = 'Notify 2',
    ConsigneeNotImporter = 'Consignee Not Importer',
    ShipperNotExporter = 'Shipper Not Exporter',
    Agent = 'Agent',
    consignee = 'consignee',
    shipper = 'shipper',
    Carrier = 'Carrier',
    Trucker = 'Trucker'
}

export enum ShipmentEntityTypes {
    Order = 'O',
    Customs = 'C',
    Forwarding = 'F'
}

export enum ShipmentLevelCodes {
    House = 'H',
    Direct = 'D',
    Customs = 'A'
}
