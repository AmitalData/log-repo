import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool, DateTool, FontTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {TenantManagementPMService} from '../../../../Infrastructure/Services/StandardPMs/TenantManagementPMService';
import {QueueMessagesWebService} from '../../../../Infrastructure/Services/WebServices/QueueMessagesWebService';

@Component({
    
    selector: 'TenantManagementStatisticsTabComponent',
    templateUrl: './TenantManagementStatisticsTabComponent.html',
})

export class TenantManagementStatisticsTabComponent extends BaseComponent implements OnInit {
    public DataContext: TenantManagementStatisticsTabComponent = this;
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }

    public IsEditingAllowed: boolean = false;
    ngOnInit() {
        if (this.EntityPM != null) {
            this.IsEditingAllowed = this.IsTenantManagementEditable();   

            this.SetData();
        }
    }

    private IsTenantManagementEditable() {
        var myResult = false;

        if (FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            myResult = true;
        }

        return myResult;
    }

    private SetData() {
        this.StatisticsUpdateDate = this.EntityPM.StatisticsUpdateDate;
        
        this.LastFWBSentDate = this.EntityPM.LastFWBSentDate;
        this.LastFHLSentDate = this.EntityPM.LastFHLSentDate;
        this.LastFFRSentDate = this.EntityPM.LastFFRSentDate;
        this.LastFWBCargonautSentDate = this.EntityPM.LastFWBCargonautSentDate;
        this.LastFHLCargonautSentDate = this.EntityPM.LastFHLCargonautSentDate;
        this.FSRLastSentDate = this.EntityPM.FSRLastSentDate;
        this.FSULastReceivedDate = this.EntityPM.FSULastReceivedDate;
        this.FSALastReceivedDate = this.EntityPM.FSALastReceivedDate;

        this.CustomerLastDate = this.EntityPM.CustomerLastDate;
        this.CustomerTotalLastWeek = this.EntityPM.CustomerTotalLastWeek;
        this.CustomerTotalLastMonth = this.EntityPM.CustomerTotalLastMonth;

        this.ShipmentLastDate = this.EntityPM.ShipmentLastDate;
        this.ShipmentTotalLastWeek = this.EntityPM.ShipmentTotalLastWeek;
        this.ShipmentTotalLastMonth = this.EntityPM.ShipmentTotalLastMonth;

        this.ARInvoiceLastDate = this.EntityPM.ARInvoiceLastDate;
        this.ARInvoiceTotalLastWeek = this.EntityPM.ARInvoiceTotalLastWeek;
        this.ARInvoiceTotalLastMonth = this.EntityPM.ARInvoiceTotalLastMonth;

        this.QuoteLastDate = this.EntityPM.QuoteLastDate;
        this.QuoteTotalLastWeek = this.EntityPM.QuoteTotalLastWeek;
        this.QuoteTotalLastMonth = this.EntityPM.QuoteTotalLastMonth;

        this.APInvoiceLastDate = this.EntityPM.APInvoiceLastDate;
        this.APInvoiceTotalLastWeek = this.EntityPM.APInvoiceTotalLastWeek;
        this.APInvoiceTotalLastMonth = this.EntityPM.APInvoiceTotalLastMonth;

        this.OpportunityLastDate = this.EntityPM.OpportunityLastDate;
        this.OpportunityTotalLastWeek = this.EntityPM.OpportunityTotalLastWeek;
        this.OpportunityTotalLastMonth = this.EntityPM.OpportunityTotalLastMonth;

        this.ActivityLastDate = this.EntityPM.ActivityLastDate;
        this.ActivityTotalLastWeek = this.EntityPM.ActivityTotalLastWeek;
        this.ActivityTotalLastMonth = this.EntityPM.ActivityTotalLastMonth;

        this.ShardLogisticLastDate = this.EntityPM.ShardLogisticLastDate;
        this.ShardLogisticTotalLastWeek = this.EntityPM.ShardLogisticTotalLastWeek;
        this.ShardLogisticTotalLastMonth = this.EntityPM.ShardLogisticTotalLastMonth;

        this.MobileLastDate = this.EntityPM.MobileLastDate;
        this.MobileTotalLastWeek = this.EntityPM.MobileTotalLastWeek;
        this.MobileTotalLastMonth = this.EntityPM.MobileTotalLastMonth;


		this.AgentSharedLogisticsStatisticsLastDate = this.EntityPM.AgentSharedLogisticsStatisticsLastDate;
        this.AgentSharedLogisticsStatisticsLastWeek = this.EntityPM.AgentSharedLogisticsStatisticsLastWeek;
        this.AgentSharedLogisticsStatisticsLastMonth = this.EntityPM.AgentSharedLogisticsStatisticsLastMonth;

        this.LastEbookingSentDate = this.EntityPM.LastEbookingSentDate;
        this.LastSISentDate = this.EntityPM.LastSISentDate;
        this.NumberOfBookingSentLastWeek = this.EntityPM.NumberOfBookingSentLastWeek;
        this.NumberOfSISentLastWeek = this.EntityPM.NumberOfSISentLastWeek;
        this.LastContainerStatusReceived = this.EntityPM.LastContainerStatusReceived;

        this.LastTariffUpdateDate = this.EntityPM.LastTariffUpdateDate;
        this.LastTariffUsageDate = this.EntityPM.LastTariffUsageDate;
        this.LastWeekCreatedTariffs = this.EntityPM.LastWeekCreatedTariffs;
        this.LastMonthCreatedTariffs = this.EntityPM.LastMonthCreatedTariffs;

        this.DigitalPortalLastDate = this.EntityPM.DigitalPortalLastDate;
        this.DigitalPortalMobTotalLastMonth = this.EntityPM.DigitalPortalMobTotalLastMonth;
        this.DigitalPortalMobTotalLastWeek = this.EntityPM.DigitalPortalMobTotalLastWeek;
        this.DigitalPortalMobileLastDate = this.EntityPM.DigitalPortalMobileLastDate;
        this.DigitalPortalTotalLastMonth = this.EntityPM.DigitalPortalTotalLastMonth;
        this.DigitalPortalTotalLastWeek = this.EntityPM.DigitalPortalTotalLastWeek;

        this.SetColors();
    }

    private SetColors() {
        var todayDate = DateTool.GetCurrentDateAsUtc();
        var date7 = DateTool.AddDays(todayDate, -7).valueOf();

        this.CustomerLastDateColor = FontTool.Gray;
        this.ShipmentLastDateColor = FontTool.Gray;
        this.ARInvoiceLastDateColor = FontTool.Gray;
        this.QuoteLastDateColor = FontTool.Gray;
        this.APInvoiceLastDateColor = FontTool.Gray;
        this.OpportunityLastDateColor = FontTool.Gray;
        this.ActivityLastDateColor = FontTool.Gray;
        this.ShardLogisticLastDateColor = FontTool.Gray;
        this.MobileLastDateColor = FontTool.Gray;
        this.DigitalPortalMobileLastDateColor = FontTool.Gray;
        this.DigitalPortalLastDateColor = FontTool.Gray;
		this.AgentSharedLogisticsStatisticsLastDateColor = FontTool.Gray;
		

        if (this.EntityPM.CustomerLastDate != null) {
            var myDate = new Date(this.EntityPM.CustomerLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.CustomerLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.ShipmentLastDate != null) {
            var myDate = new Date(this.EntityPM.ShipmentLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.ShipmentLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.ARInvoiceLastDate != null) {
            var myDate = new Date(this.EntityPM.ARInvoiceLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.ARInvoiceLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.QuoteLastDate != null) {
            var myDate = new Date(this.EntityPM.QuoteLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.QuoteLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.APInvoiceLastDate != null) {
            var myDate = new Date(this.EntityPM.APInvoiceLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.APInvoiceLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.OpportunityLastDate != null) {
            var myDate = new Date(this.EntityPM.OpportunityLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.OpportunityLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.ActivityLastDate != null) {
            var myDate = new Date(this.EntityPM.ActivityLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.ActivityLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.ShardLogisticLastDate != null) {
            var myDate = new Date(this.EntityPM.ShardLogisticLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.ShardLogisticLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.MobileLastDate != null) {
            var myDate = new Date(this.EntityPM.MobileLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.MobileLastDateColor = FontTool.Red;
            }
        }

        if (this.EntityPM.DigitalPortalLastDate != null) {
            var myDate = new Date(this.EntityPM.DigitalPortalLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.DigitalPortalLastDateColor = FontTool.Red;
            }
        }
        
        if (this.EntityPM.DigitalPortalMobileLastDate != null) {
            var myDate = new Date(this.EntityPM.DigitalPortalMobileLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.DigitalPortalMobileLastDateColor = FontTool.Red;
            }
        }
		
		if (this.EntityPM.AgentSharedLogisticsStatisticsLastDate != null) {
            var myDate = new Date(this.EntityPM.AgentSharedLogisticsStatisticsLastDate.valueOf()).valueOf();

            if (myDate < date7) {
                this.AgentSharedLogisticsStatisticsLastDateColor = FontTool.Red;
            }
        }

    }

    public StatisticsUpdateDate: Date;

    public LastFWBSentDate: Date;
    public LastFHLSentDate: Date;
    public LastFFRSentDate: Date;
    public LastFWBCargonautSentDate: Date;
    public LastFHLCargonautSentDate: Date;
    public FSRLastSentDate: Date;
    public FSULastReceivedDate: Date;
    public FSALastReceivedDate: Date;

    public CustomerLastDate: Date;
    public CustomerTotalLastWeek: number;
    public CustomerTotalLastMonth: number;
    public CustomerLastDateColor: string

    public ShipmentLastDate: Date;
    public ShipmentTotalLastWeek: number;
    public ShipmentTotalLastMonth: number;
    public ShipmentLastDateColor: string

    public ARInvoiceLastDate: Date;
    public ARInvoiceTotalLastWeek: number;
    public ARInvoiceTotalLastMonth: number;
    public ARInvoiceLastDateColor: string

    public QuoteLastDate: Date;
    public QuoteTotalLastWeek: number;
    public QuoteTotalLastMonth: number;
    public QuoteLastDateColor: string

    public APInvoiceLastDate: Date;
    public APInvoiceTotalLastWeek: number;
    public APInvoiceTotalLastMonth: number;
    public APInvoiceLastDateColor: string

    public OpportunityLastDate: Date;
    public OpportunityTotalLastWeek: number;
    public OpportunityTotalLastMonth: number;
    public OpportunityLastDateColor: string

    public ActivityLastDate: Date;
    public ActivityTotalLastWeek: number;
    public ActivityTotalLastMonth: number;
    public ActivityLastDateColor: string

    public ShardLogisticLastDate: Date;
    public ShardLogisticTotalLastWeek: number;
    public ShardLogisticTotalLastMonth: number;
    public ShardLogisticLastDateColor: string

    public MobileLastDate: Date;
    public MobileTotalLastWeek: number;
    public MobileTotalLastMonth: number;
    public MobileLastDateColor: string
        
	public AgentSharedLogisticsStatisticsLastDate: Date;
    public AgentSharedLogisticsStatisticsLastWeek: number;
    public AgentSharedLogisticsStatisticsLastMonth: number;
    public AgentSharedLogisticsStatisticsLastDateColor: string

    public LastEbookingSentDate: Date;
    public LastSISentDate: Date;
    public NumberOfBookingSentLastWeek: number;
    public NumberOfSISentLastWeek: number;
    public LastContainerStatusReceived: Date;

    public LastTariffUpdateDate: Date;
    public LastTariffUsageDate: Date;
    public LastWeekCreatedTariffs: number;
    public LastMonthCreatedTariffs: number;

    DigitalPortalLastDate: Date;
    DigitalPortalTotalLastWeek: number;
    DigitalPortalTotalLastMonth: number;
    DigitalPortalMobileLastDate: Date;
    DigitalPortalMobTotalLastWeek: number;
    DigitalPortalMobTotalLastMonth: number;

    public DigitalPortalMobileLastDateColor: string
    public DigitalPortalLastDateColor: string

    RefreshClicked() {
        this.CurrentSession.StartBusyIndicator("Refreshing....");
        var service: TenantManagementPMService = new TenantManagementPMService();
        service.get(this.EntityPM.Id).subscribe((response:ServiceResponse) => {
            if (!response.HasError) {
                this.EntityPM = response.Result;

                if (this.EntityPM != null) {
                    this.SetData();
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    UpdateClicked() {
        var service = new QueueMessagesWebService();

        service.UpdateTenantManagementStatistics(this.EntityPM.Id).subscribe((myResult: ServiceResponse) => {
            //var myResponse: ServiceResponse = myResult;

            //if (!myResponse.HasError) {
                                
            //}
        });
    }
}
