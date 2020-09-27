import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool, DateTool } from '../../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogTab } from '../../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { PaymentOrderPM } from '../../../../../../Customs/EntityPMs/PaymentOrderPM';
import { DeficitPM } from '../../../../../../Customs/EntityPMs/DeficitPM';
import { DeclarationList } from '../../../../../../Customs/EntityLists/DeclarationList';
import { DeficitConnFileParagraphTypeList } from '../../../../../../Customs/EntityLists/DeficitConnFileParagraphTypeList';
import { TapagList } from '../../../../../../Customs/EntityLists/TapagList';
import { EntityPMService } from '../../../../../../Infrastructure/Services/EntityPMService';
import { MessageToAgentRequestParams } from '../../../../../../Customs/DataContract/RequestParams/MessageToAgentRequestParams';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { TapagMessagesService } from '../../../../../../Customs/Services/WebServices/TapagMessagesService';
import { DeclarationWebService } from '../../../../../../Customs/Services/WebServices/DeclarationWebService';
import { LogitudeWindow } from '../../../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../../../Infrastructure/Services/EntityResourceService';
import { MessageWindow } from '../../../../../../Controls/Windows/MessageWindow';
declare var window: any;

@Component({
    
    templateUrl: './PaymentOrderDeficitComponent.html',
    selector: 'PaymentOrderDeficitComponent',
})

export class PaymentOrderDeficitComponent extends BaseComponent {
    public EntityPM: DeficitPM = new DeficitPM();
    public ObjectTableName = "Customs.Deficit";
    public DataContext: PaymentOrderDeficitComponent = this;
    public ObjectTableId: string;
    public CurrentEditComponentId: string;
    IsControlEnabled: any; // html component requires this property. AOT

    public PaymentOrderPM: PaymentOrderPM = null;
    private Tapag: TapagList = null;
    public ConnectedEntitiesList: ObservableCollection;

    public IsTab: boolean = false;
    IsLoaded: boolean = false;

    public tapagMessagesService: TapagMessagesService = new TapagMessagesService();
    public declarationWebService: DeclarationWebService = new DeclarationWebService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        this.ConnectedEntitiesList = new ObservableCollection([]);
        this.UIProperties.SetRequired("LeadingFileNumber", 'Customs.Tapag', false);

        if (!AppTool.IsNullOrEmpty(entityArgs)) {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.Tapag").subscribe((response: any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.TapagConnectionTable").subscribe((response: any) => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.Deficit").subscribe((response: any) => {
                            this.Listen();
                            if (entityArgs.ObjectTableName == "Customs.PaymentOrder") {
                                this.PaymentOrderPM = entityArgs.EntityPM;
                                this.LoadDeficitData(this.PaymentOrderPM.PaymentNumber, null, this.PaymentOrderPM.Tenant);
                            }
                            this.IsLoaded = true;
                        });
                    });
                });
            });
        }
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
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "PODF") {
                            this.PaymentOrderPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                            this.LoadDeficitData(this.PaymentOrderPM.PaymentNumber, null, this.PaymentOrderPM.Tenant);
                        }
                    }
                })
            );
        }
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.Tapag = args.EntityPM;
            this.LoadDeficitData(null, this.Tapag.Id, this.Tapag.Tenant);
            this.IsTab = true;
            this.IsLoaded = true;
        }
    }

    LoadDeficitData(paymentNumber: string, tapagId: string, tenant: number){

        if (!AppTool.IsNullOrEmpty(paymentNumber) || !AppTool.IsNullOrEmpty(tapagId)) {
            this.tapagMessagesService.GetDeficitPMByPaymentOrderNumberOrTapagId(paymentNumber, tapagId, tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.GetDeficitPMByPaymentOrderNumberOrTapagIdOp_Completed(myResponse, false);
            });
        }
    }

    GetDeficitPMByPaymentOrderNumberOrTapagIdOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            this.EntityPM = myResponse.Result;

            if (this.IsTab) {
                this.GetConnectedDeclarations();
            }
            else if (this.EntityPM != null) {
                this.tapagMessagesService.GetSingleTapagList(this.EntityPM.TapagId, this.EntityPM.Tenant)
                    .subscribe((myResponse: ServiceResponse) => {
                        this.GetSingleTapagListOp_Completed(myResponse, false);
                    });
            }
            //to do.....
            //else if (entityPM.CustomsEntityTypeCode == "11122" && !string.IsNullOrEmpty(entityPM.FirstEntityID)) {
            //    LoadOperation tapagOp = context.Load(context.GetSingleTapagByLeadingFileNumberQuery(entityPM.FirstEntityID, entityPM.Tenant), LoadBehavior.RefreshCurrent, true);
            //    tapagOp.Completed += tapagOp_Completed;

            //}
        }
    }

    GetSingleTapagListOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            this.Tapag = myResponse.Result;
            this.GetConnectedDeclarations();
        }
    }

    GetConnectedDeclarations() {
        if (this.Tapag != null) {
            //this.UIProperties.SetRequired("LeadingFileNumber", 'Customs.Tapag', false);
            this.EntityPM.LeadingFileNumber = this.Tapag.LeadingFileNumber;
            this.declarationWebService.GetDeclarationByTapagConnectionConnection(this.Tapag.Id)
                .subscribe((myResponse: ServiceResponse) => {
                    this.GetDeclarationByTapagConnectionConnectionOp_Completed(myResponse, false);
                });
        }
    }

    GetDeclarationByTapagConnectionConnectionOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this.ConnectedEntitiesList = new ObservableCollection([]);

        if (myResponse.Result != null) {
            myResponse.Result.forEach((declarationList: DeclarationList) => {
                this.ConnectedEntitiesList.Insert(new ConnectedEntityLineComponent(declarationList, this.EntityPM));
            });
        }
    }

    public get LeadingFileNumber() { return this.EntityPM.LeadingFileNumber; }
    public set LeadingFileNumber(newValue: string) { this.EntityPM.LeadingFileNumber = newValue; }

    public get DebtNotificationNumber() { return this.EntityPM.DebtNotificationNumber; }
    public set DebtNotificationNumber(newValue: string) { this.EntityPM.DebtNotificationNumber = newValue; }

    public get CustomerId() { return this.Tapag ? this.Tapag.CustomerId : null; }
    public set CustomerId(newValue: string) { this.Tapag.CustomerId = newValue; }

    public get ImporterId() { return this.Tapag ? this.Tapag.ImporterId : null; }
    public set ImporterId(newValue: string) { this.Tapag.ImporterId = newValue; }

    public get ImporterName() { return this.EntityPM.ImporterName; }
    public set ImporterName(newValue: string) { this.EntityPM.ImporterName = newValue; }

    public get NotificationTypeName() { return this.EntityPM.NotificationTypeName; }
    public set NotificationTypeName(newValue: string) { this.EntityPM.NotificationTypeName = newValue; }

    public get NotificationTypeCode() { return this.EntityPM.NotificationTypeCode; }
    public set NotificationTypeCode(newValue: string) { this.EntityPM.NotificationTypeCode = newValue; }

    public get ProductionDate() { return this.EntityPM.ProductionDate; }
    public set ProductionDate(newValue: Date) { this.EntityPM.ProductionDate = newValue; }

    public get RealesGoodsDescription() { return this.EntityPM.RealesGoodsDescription; }
    public set RealesGoodsDescription(newValue: string) { this.EntityPM.RealesGoodsDescription = newValue; }

    public get CustomsBranchName() { return this.EntityPM.CustomsBranchName; }
    public set CustomsBranchName(newValue: string) { this.EntityPM.CustomsBranchName = newValue; }

    public get CustomsBranchCode() { return this.Tapag ? this.Tapag.CustomsBranchCode : null; }
    public set CustomsBranchCode(newValue: string) { this.Tapag.CustomsBranchCode = newValue; }

    public get ProfessionUnitTypeCode() { return this.Tapag ? this.Tapag.ProfessionUnitTypeCode : null; }
    public set ProfessionUnitTypeCode(newValue: string) { this.Tapag.ProfessionUnitTypeCode = newValue; }

    public get DebtNotificationReason() { return this.EntityPM.DebtNotificationReason; }
    public set DebtNotificationReason(newValue: string) { this.EntityPM.DebtNotificationReason = newValue; }

    public get PaymentOrderNumber() { return this.EntityPM.PaymentOrderNumber; }
    public set PaymentOrderNumber(newValue: string) { this.EntityPM.PaymentOrderNumber = newValue; }

    public get ValidityDateTo() { return this.EntityPM.ValidityDateTo; }
    public set ValidityDateTo(newValue: Date) { this.EntityPM.ValidityDateTo = newValue; }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    DeficitDecisionButtonClicked(item: ConnectedEntityLineComponent) {
        var windowArgs: any = {};
        windowArgs.EntityPM = item.deficitPM;
        windowArgs.DeclarationId = item.entityPM.Id;

        if (item.deficitPM != null && item.deficitPM.DeficitDecisions != null && item.deficitPM.DeficitDecisions.length > 0) {
            windowArgs.DeficitDecisionItem = item.deficitPM.DeficitDecisions.filter(d => d.DeclarationId == item.entityPM.Id);
            if (windowArgs.DeficitDecisionItem != null) {
                windowArgs.DeficitDecisionItem = windowArgs.DeficitDecisionItem[0];
            }
        }

        if (windowArgs.DeficitDecisionItem == null) {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show("טרם התקבלה החלטת מכס בגין הגרעון");
            return;
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 400;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        //logWindow.Title = TextCodeTranslator.Translate("Customs.PaymentOrder.TH.Deficits");
        logWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deficit/DeficitDecisionComponent');
    }
}

export class ConnectedEntityLineComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimsRelatedEntity";
    public DataContext = this;
    private totalTax: string = "0.0";
    public AmountsList: ObservableCollection;

    public tapagMessagesService: TapagMessagesService = new TapagMessagesService();

    constructor(public entityPM: DeclarationList, public deficitPM: DeficitPM) {
        super();
        this.AmountsList = new ObservableCollection([]);
        this.LoadDeficitConnectedFileParagraphTypes();
    }

    public get CustomFileNo() { return this.entityPM.CustomFileNo; }
    public set CustomFileNo(newValue: string) { this.entityPM.CustomFileNo = newValue; }

    public get DeclarationNumber() { return this.entityPM.DeclarationNumber; }
    public set DeclarationNumber(newValue: string) { this.entityPM.DeclarationNumber = newValue; }

    public get PaymentDate() { return this.entityPM.PaymentDate; }
    public set PaymentDate(newValue: Date) { this.entityPM.PaymentDate = newValue; }

    public get CustomsTapagNumeral() { return this.entityPM.CustomsTapagNumeral; }
    public set CustomsTapagNumeral(newValue: string) { this.entityPM.CustomsTapagNumeral = newValue; }

    public get TotalTax() { return this.totalTax; }
    public set TotalTax(newValue: string) { this.totalTax = newValue; }

    LoadDeficitConnectedFileParagraphTypes() {
        if (this.entityPM != null && this.deficitPM != null) {
            this.tapagMessagesService.GetDeficitConnectedFileParagraphTypeList(this.entityPM.Id, this.deficitPM.Id, this.deficitPM.Tenant)
                .subscribe((myResponse: ServiceResponse) => {
                    this.GetDeclarationByTapagConnectionConnectionOp_Completed(myResponse, false);
                });
        }
    }

    GetDeclarationByTapagConnectionConnectionOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this.AmountsList = new ObservableCollection([]);

        if (myResponse.Result != null) {
            let total: number = 0;
            myResponse.Result.forEach((item) => {
                this.AmountsList.Insert(item);
                total = Number(total) + Number(item.Amount);
            });
            this.TotalTax = total.toString();
        }
    }

}
