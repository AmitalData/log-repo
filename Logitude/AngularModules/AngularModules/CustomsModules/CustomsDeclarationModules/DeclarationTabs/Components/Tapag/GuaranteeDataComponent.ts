import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool, DateTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { GuaranteePM } from '../../../../../Customs/EntityPMs/GuaranteePM';
import { RequiredGuaranteeTypePM } from '../../../../../Customs/EntityPMs/RequiredGuaranteeTypePM';
import { GuaranteeConditionPM } from '../../../../../Customs/EntityPMs/GuaranteeConditionPM';
import { DeclarationList } from '../../../../../Customs/EntityLists/DeclarationList';
import { TapagList } from '../../../../../Customs/EntityLists/TapagList';
import { EntityPMService } from '../../../../../Infrastructure/Services/EntityPMService';
import { MessageToAgentRequestParams } from '../../../../../Customs/DataContract/RequestParams/MessageToAgentRequestParams';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { TapagMessagesService } from '../../../../../Customs/Services/WebServices/TapagMessagesService';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './GuaranteeDataComponent.html',
    selector: 'GuaranteeDataComponent',
})

export class GuaranteeDataComponent extends BaseComponent {
    public EntityPM: GuaranteePM = new GuaranteePM();
    public ObjectTableName = "Customs.Guarantee";
    public DataContext: GuaranteeDataComponent = this;
    public ObjectTableId: string;
    public CurrentEditComponentId: string;
    private Tapag: TapagList = new TapagList();
    IsControlEnabled: any; // html component requires this property. AOT

    public ConnectedEntitiesList: ObservableCollection;
    public RequiredGuaranteeTypes: ObservableCollection;
    public GuaranteeConditions: ObservableCollection;

    public tapagMessagesService: TapagMessagesService = new TapagMessagesService();
    public declarationWebService: DeclarationWebService = new DeclarationWebService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.RequiredGuaranteeTypes = new ObservableCollection([]);
        this.GuaranteeConditions = new ObservableCollection([]);
        this.ConnectedEntitiesList = new ObservableCollection([]);

        if (!AppTool.IsNullOrEmpty(entityArgs)) {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Tapag").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.TapagConnectionTable").subscribe((response: any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.Guarantee").subscribe((response: any) => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.GuaranteeCondition").subscribe((response: any) => {
                            this.EntityResourceService.getEntityResourceByTableName("Customs.RequiredGuaranteeType").subscribe((response: any) => {
                                this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response: any) => {
                                    this.Listen();
                                });
                            });
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
                        if (tabCode == "PODP") {
                        }
                    }
                })
            );
        }
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.Tapag = args.EntityPM;
            this.LoadGuaranteeData();
        }
    }

    LoadGuaranteeData() {

        if (!AppTool.IsNullOrEmpty(this.Tapag.Id)) {
            this.tapagMessagesService.GetGuaranteeByTapagId(this.Tapag.Id, this.Tapag.Tenant)
                .subscribe((myResponse: ServiceResponse) => {
                    this.GetGuaranteeByTapagIdOp_Completed(myResponse, false);
                });
        }
    }

    GetGuaranteeByTapagIdOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            this.EntityPM = myResponse.Result;
            this.FillGuaranteeDataList();
        }
    }

    FillGuaranteeDataList() {
        this.RequiredGuaranteeTypes = new ObservableCollection([]);
        this.GuaranteeConditions = new ObservableCollection([]);

        if (this.EntityPM != null && this.EntityPM.RequiredGuaranteeTypes.length > 0) {
            this.EntityPM.RequiredGuaranteeTypes.forEach((requiredGuaranteeTypePM: RequiredGuaranteeTypePM) => {
                this.RequiredGuaranteeTypes.Insert(requiredGuaranteeTypePM);
            });
        }

        if (this.EntityPM != null && this.EntityPM.GuaranteeConditions.length > 0) {
            this.EntityPM.GuaranteeConditions.forEach((guaranteeConditionPM: GuaranteeConditionPM) => {
                this.GuaranteeConditions.Insert(guaranteeConditionPM);
            });
        }

        this.GetConnectedDeclarations();
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

    public get LeadingFileNumber() { return this.EntityPM.LeadingFileNumber; }
    public set LeadingFileNumber(newValue: string) { this.EntityPM.LeadingFileNumber = newValue; }

    public get CustomerName() { return this.Tapag.CustomerName; }
    public set CustomerName(newValue: string) { this.Tapag.CustomerName = newValue; }

    public get CustomerId() { return this.Tapag.CustomerId; }
    public set CustomerId(newValue: string) { this.Tapag.CustomerId = newValue; }

    public get ImporterId() { return this.Tapag.ImporterId; }
    public set ImporterId(newValue: string) { this.Tapag.ImporterId = newValue; }

    public get ImporterName() { return this.EntityPM.ImporterName; }
    public set ImporterName(newValue: string) { this.EntityPM.ImporterName = newValue; }

    public get ClientActivityCode() { return this.EntityPM.ClientActivityCode; }
    public set ClientActivityCode(newValue: string) { this.EntityPM.ClientActivityCode = newValue; }

    public get GuaranteeRequestStatusCode() { return this.EntityPM.GuaranteeRequestStatusCode; }
    public set GuaranteeRequestStatusCode(newValue: string) { this.EntityPM.GuaranteeRequestStatusCode = newValue; }

    public get GuaranteeRequestNumber() { return this.EntityPM.GuaranteeRequestNumber; }
    public set GuaranteeRequestNumber(newValue: string) { this.EntityPM.GuaranteeRequestNumber = newValue; }

    public get CustomsBranchCode() { return this.Tapag.CustomsBranchCode; }
    public set CustomsBranchCode(newValue: string) { this.Tapag.CustomsBranchCode = newValue; }

    public get BrandNumber() { return this.EntityPM.BrandNumber; }
    public set BrandNumber(newValue: string) { this.EntityPM.BrandNumber = newValue; }

    public get ProfessionUnitTypeCode() { return this.Tapag.ProfessionUnitTypeCode; }
    public set ProfessionUnitTypeCode(newValue: string) { this.Tapag.ProfessionUnitTypeCode = newValue; }

    public get LawyerNumber() { return this.EntityPM.LawyerNumber; }
    public set LawyerNumber(newValue: string) { this.EntityPM.LawyerNumber = newValue; }

    public get CustomEntityTypeCode() { return this.EntityPM.CustomEntityTypeCode; }
    public set CustomEntityTypeCode(newValue: string) { this.EntityPM.CustomEntityTypeCode = newValue; }

    public get VehicleChassisNumber() { return this.EntityPM.VehicleChassisNumber; }
    public set VehicleChassisNumber(newValue: string) { this.EntityPM.VehicleChassisNumber = newValue; }

    public get CustomEntityNumber() { return this.EntityPM.CustomEntityNumber; }
    public set CustomEntityNumber(newValue: string) { this.EntityPM.CustomEntityNumber = newValue; }

    public get EngineNumber() { return this.EntityPM.EngineNumber; }
    public set EngineNumber(newValue: string) { this.EntityPM.EngineNumber = newValue; }

    public get MsgID() { return this.EntityPM.MsgID; }
    public set MsgID(newValue: string) { this.EntityPM.MsgID = newValue; }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    GuaranteeReturnRequestClicked() {
        //GuaranteeReturnRequestViewModel viewModel = new GuaranteeReturnRequestViewModel(tapag);
        //GuaranteeReturnRequestControl control = new GuaranteeReturnRequestControl() { DataContext = viewModel };

        //simplogWindow.Height = 900;
        //simplogWindow.Width = 1000;
        //simplogWindow.Add(control);
        //simplogWindow.ShowCloseButtonOnly = true;

        //simplogWindow.Show();
    }
}
