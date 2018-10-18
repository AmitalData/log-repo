import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool, DateTool } from '../../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogTab } from '../../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { PaymentOrderPM } from '../../../../../../Customs/EntityPMs/PaymentOrderPM';
import { DepositPM } from '../../../../../../Customs/EntityPMs/DepositPM';
import { DepositConditionPM } from '../../../../../../Customs/EntityPMs/DepositConditionPM';
import { DeclarationList } from '../../../../../../Customs/EntityLists/DeclarationList';
import { TapagList } from '../../../../../../Customs/EntityLists/TapagList';
import { EntityPMService } from '../../../../../../Infrastructure/Services/EntityPMService';
import { MessageToAgentRequestParams } from '../../../../../../Customs/DataContract/RequestParams/MessageToAgentRequestParams';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { TapagMessagesService } from '../../../../../../Customs/Services/WebServices/TapagMessagesService';
import { DeclarationWebService } from '../../../../../../Customs/Services/WebServices/DeclarationWebService';
import { LogitudeWindow } from '../../../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../../../Infrastructure/Services/EntityResourceService';
import { CustomsRequestMenuService } from '../../../../../../Customs/Services/Others/CustomsRequestMenuService';

declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './PaymentOrderDepositDataComponent.html',
    selector: 'PaymentOrderDepositDataComponent',
})

export class PaymentOrderDepositDataComponent extends BaseComponent {
    public EntityPM: DepositPM = new DepositPM();
    public ObjectTableName = "Customs.Deposit";
    public DataContext: PaymentOrderDepositDataComponent = this;
    public ObjectTableId: string;
    public CurrentEditComponentId: string;

    public PaymentOrderPM: PaymentOrderPM = null;
    private Tapag: TapagList = new TapagList();
    IsControlEnabled: any; // html component requires this property. AOT;

    public ConnectedEntitiesList: ObservableCollection;
    public DepositConditions: ObservableCollection;

    public IsTab: boolean = false;
    IsLoaded: boolean = false;

    public tapagMessagesService: TapagMessagesService = new TapagMessagesService();
    public declarationWebService: DeclarationWebService = new DeclarationWebService();

    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        this.DepositConditions = new ObservableCollection([]);
        this.ConnectedEntitiesList = new ObservableCollection([]);

        if (!AppTool.IsNullOrEmpty(entityArgs)) {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.Tapag").subscribe((response: any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.TapagConnectionTable").subscribe((response: any) => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.Deposit").subscribe((response: any) => {
                            this.EntityResourceService.getEntityResourceByTableName("Customs.DepositCondition").subscribe((response: any) => {
                                this.Listen();
                                if (entityArgs.ObjectTableName == "Customs.PaymentOrder") {
                                    this.PaymentOrderPM = entityArgs.EntityPM;
                                    this.LoadDepositData(this.PaymentOrderPM.PaymentNumber, null, this.PaymentOrderPM.Tenant);
                                }
                                this.IsLoaded = true;
                            });
                        });
                    });
                });
            });
        }
    }

    private Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = SessionLocator.CurrentSession.CurrentEditComponent.ComponentId;
            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == SessionLocator.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "PODP") {
                            this.PaymentOrderPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                            this.LoadDepositData(this.PaymentOrderPM.PaymentNumber, null, this.PaymentOrderPM.Tenant);
                        }
                    }
                })
            );
        }
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.Tapag = args.EntityPM;
            this.LoadDepositData(null, this.Tapag.Id, this.Tapag.Tenant);
            this.IsTab = true;
            this.IsLoaded = true;
        }
    }

    LoadDepositData(paymentNumber: string, tapagId: string, tenant: number) {

        if (!AppTool.IsNullOrEmpty(paymentNumber) || !AppTool.IsNullOrEmpty(tapagId)) {
            this.tapagMessagesService.GetDepositPMByPaymentOrderNumberOrTapagId(paymentNumber, tapagId, tenant)
                .subscribe((myResponse: ServiceResponse) => {
                    this.GetDepositPMByPaymentOrderNumberOrTapagIdOp_Completed(myResponse, false);
                });
        }
    }

    GetDepositPMByPaymentOrderNumberOrTapagIdOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            this.EntityPM = myResponse.Result;
            this.FillDepositConditionsList();
        }
    }

    FillDepositConditionsList() {
        this.DepositConditions = new ObservableCollection([]);

        if (this.EntityPM != null && this.EntityPM.DepositConditions.length > 0) {
            this.EntityPM.DepositConditions.forEach((depositConditionItem: DepositConditionPM) => {
                this.DepositConditions.Insert(depositConditionItem);

            });
        }

        if (this.IsTab) {
            this.GetConnectedDeclarations();
        }
        else if (this.EntityPM != null) {
            this.tapagMessagesService.GetSingleTapagList(this.EntityPM.TapagID, this.EntityPM.Tenant)
                .subscribe((myResponse: ServiceResponse) => {
                    this.GetSingleTapagListOp_Completed(myResponse, false);
                });
        }

    }

    GetConnectedDeclarations() {
        if (this.Tapag != null) {
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
                this.ConnectedEntitiesList.Insert(declarationList);

            });
        }
    }

    GetSingleTapagListOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            this.Tapag = myResponse.Result;
            this.GetConnectedDeclarations();
        }
    }

    public get LeadingFileNumber() { return this.EntityPM.LeadingFileNumber; }
    public set LeadingFileNumber(newValue: string) { this.EntityPM.LeadingFileNumber = newValue; }

    public get CustomerName() { return this.EntityPM.CustomerName; }
    public set CustomerName(newValue: string) { this.EntityPM.CustomerName = newValue; }

    public get CustomerId() { return this.Tapag ? this.Tapag.CustomerId : null; }
    public set CustomerId(newValue: string) { this.Tapag.CustomerId = newValue; }

    public get ImporterId() { return this.Tapag ? this.Tapag.ImporterId : null; }
    public set ImporterId(newValue: string) { this.Tapag.ImporterId = newValue; }

    public get ImporterName() { return this.EntityPM.ImporterName; }
    public set ImporterName(newValue: string) { this.EntityPM.ImporterName = newValue; }

    public get ProfessionUnitTypeName() { return this.EntityPM.ProfessionUnitTypeName; }
    public set ProfessionUnitTypeName(newValue: string) { this.EntityPM.ProfessionUnitTypeName = newValue; }

    public get CustomsBranchCode() { return this.Tapag ? this.Tapag.CustomsBranchCode : null; }
    public set CustomsBranchCode(newValue: string) { this.Tapag.CustomsBranchCode = newValue; }

    public get ProfessionUnitTypeCode() { return this.Tapag ? this.Tapag.ProfessionUnitTypeCode : null; }
    public set ProfessionUnitTypeCode(newValue: string) { this.Tapag.ProfessionUnitTypeCode = newValue; }

    public get CustomsBranchName() { return this.EntityPM.CustomsBranchName; }
    public set CustomsBranchName(newValue: string) { this.EntityPM.CustomsBranchName = newValue; }

    public get EntityTypeCode() { return this.EntityPM.EntityTypeCode; }
    public set EntityTypeCode(newValue: string) { this.EntityPM.EntityTypeCode = newValue; }

    public get EntityNumber() { return this.EntityPM.EntityNumber; }
    public set EntityNumber(newValue: string) { this.EntityPM.EntityNumber = newValue; }

    public get DepositEssenceTypeCode() { return this.EntityPM.DepositEssenceTypeCode; }
    public set DepositEssenceTypeCode(newValue: string) { this.EntityPM.DepositEssenceTypeCode = newValue; }

    public get DepositAmount() { return this.EntityPM.DepositAmount; }
    public set DepositAmount(newValue: number) { this.EntityPM.DepositAmount = newValue; }

    public get TradeMarkNumber() { return this.EntityPM.TradeMarkNumber; }
    public set TradeMarkNumber(newValue: string) { this.EntityPM.TradeMarkNumber = newValue; }

    public get LawyerNumber() { return this.EntityPM.LawyerNumber; }
    public set LawyerNumber(newValue: string) { this.EntityPM.LawyerNumber = newValue; }

    public get VehicleChassisNumber() { return this.EntityPM.VehicleChassisNumber; }
    public set VehicleChassisNumber(newValue: string) { this.EntityPM.VehicleChassisNumber = newValue; }

    public get EngineNumber() { return this.EntityPM.EngineNumber; }
    public set EngineNumber(newValue: string) { this.EntityPM.EngineNumber = newValue; }

    public get BirthDate() { return this.EntityPM.BirthDate; }
    public set BirthDate(newValue: Date) { this.EntityPM.BirthDate = newValue; }

    public get PaymentNumber() { return this.EntityPM.PaymentNumber; }
    public set PaymentNumber(newValue: string) { this.EntityPM.PaymentNumber = newValue; }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    BankAccountToRefundButtonClicked() {

        //var logitudeWindow = new LogitudeWindow();
        //var windowArgs: any = {};
        //logitudeWindow.Width = 600;
        //logitudeWindow.Height = 480;
        //logitudeWindow.IsShowCloseButton = false;
        //logitudeWindow.Title = TextCodeTranslator.Translate("Customs.General.O.BankAccountToRefundMessage");;
        //logitudeWindow.WindowArgs = windowArgs;
        //logitudeWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deposit/BankAccountToRefundComponent');


        let customsRequestMenuService = new CustomsRequestMenuService();
        let my = {
            "DeclarationId": this.EntityPM.Id,
        };
        customsRequestMenuService.WindowClosed.subscribe((myarg) => { });
        customsRequestMenuService.ShowModalAsEditMenuAction("2018", my);
    }
}
