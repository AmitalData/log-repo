declare var System: any;
declare var window: any;
import { Component, OnInit } from '@angular/core';
import { AppTool, ArrayTool, DateTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { PaymentOrderWebService } from '../../../../../Customs/Services/WebServices/PaymentOrderWebService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from              '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from    '../../../../../Infrastructure/Utilities/ObservableCollection';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { PaymentOrderList } from '../../../../../Customs/EntityLists/PaymentOrderList';
import { PaymentOrderPMService } from '../../../../../Customs/Services/StandardPMs/PaymentOrderPMService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';


@Component({
    moduleId: module.id,
    templateUrl: './DeclarationPaymentOrderTabComponent.html',
})

export class DeclarationPaymentOrderTabComponent
    extends BaseComponent
    implements OnInit {
    public EntityPM: DeclarationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;

    public CurrentEditComponentId: string;
    public paymentOrderlist: ObservableCollection;

    private paymentOrderWebService: PaymentOrderWebService = new PaymentOrderWebService;
    private paymentOrderPMService: PaymentOrderPMService = new PaymentOrderPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.paymentOrderlist = new ObservableCollection([]);
        
        this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;

                this.LoadPaymentOrders();
                this.Listen();            
            });
        });
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadPaymentOrders();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCPO") {
                            //this.LoadPaymentOrders();
                        }
                    }
                })
            );
        }
    }

    private LoadPaymentOrders() {
        this.paymentOrderlist = new ObservableCollection([]);

        this.paymentOrderWebService.GetPaymentOrderByPaymentOrderConnection("D", this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.GetPaymentOrderByPaymentOrderConnectionOp_Completed(myResponse, false);
            });
    }

    private GetPaymentOrderByPaymentOrderConnectionOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            var paymentOrderResult: PaymentOrderList[] = myResponse.Result;
            if (paymentOrderResult.length > 1)
                paymentOrderResult.sort(
                    (a, b) => { return (DateTool.GetDateFromDate(a.CreateDate) === DateTool.GetDateFromDate(b.CreateDate)) ? 0 : (DateTool.GetDateFromDate(a.CreateDate) > DateTool.GetDateFromDate(b.CreateDate)) ? -1 : 1 });
            }

        paymentOrderResult.forEach((item) => {
            this.paymentOrderlist.Insert(item);
        });
    }

    EditButtonClicked(item: PaymentOrderList) {
        if (!AppTool.IsNullOrEmpty(item)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.paymentOrderPMService.get(item.Id).subscribe(response => {
                this.CurrentSession.StopBusyIndicator();
                var windowArgs: any = {};
                windowArgs.EntityPM = response.Result;
                windowArgs.declarationPM = this.EntityPM;

                var logWindow = new LogitudeWindow();
                logWindow.Width = 1200;
                logWindow.Height = 850;
                logWindow.ShowCloseButton = false;
                logWindow.ShowHeaderButtons = true;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(($event: any) => {
                    this.LoadPaymentOrders()
                });
                logWindow.ShowEditComponent(item.Id, "Customs.PaymentOrder", "POGN");
            });

        }

    }
}
