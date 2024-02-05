import { Component, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LocationDirective } from '../../../../../Infrastructure/Utilities/LocationDirective';
import { AppTool, FontTool } from '../../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { ClaimPMService } from '../../../../../Customs/Services/StandardPMs/ClaimPMService';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import {ObjectsLocator} from '../../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './ClaimRelatedEntityTabComponent.html',
})
export class ClaimRelatedEntityTabComponent extends BaseComponent {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    LayoutDirection: string = 'ltr';
    public DataContext: ClaimRelatedEntityTabComponent = this;
    public ObjectTableName: string = "Customs.Claim";
    public ValidationErrorsList: string[] = [];
    //public EntityPM: ClaimsRelatedEntityPM;
    public EntityCounterKey: number;
    public ClaimPM: ClaimPM;
    public TabsItemsSource: TabItem[] = [];
    public IsDisplayOnly: boolean = false;
    public IsNewEntity: boolean = false;
    FIELD_IS_REQUIERD: string;

    // services
    public entityResourceService: EntityResourceService = new EntityResourceService();
    ClaimPMService: ClaimPMService = new ClaimPMService();

    InvoiceItemsMessage: string;
    takenItems: number; 
    WindowTitle: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef) {
        super();

        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    SetWindowArgs(args: any) {

        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityCounterKey = args.EntityCounterKey;
            this.ClaimPM = args.ClaimPM;
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.IsNewEntity = args.IsNewEntity;
            this.WindowTitle = args.WindowTitle;

        }
        this.entityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe((response:any) => {
            this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe((response:any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesAmount").subscribe((response:any) => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntsReasonsExp").subscribe((response:any) => {
                        this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesReason").subscribe((response:any) => {
                            this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntsExpDeclar").subscribe((response:any) => {
                                this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesSeizure").subscribe((response:any) => {
                                    this.entityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntitiesRefund").subscribe((response:any) => {
                                        this.BuildTabs();
                                        this.RunComponent();
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("FileData", "Customs.Claim.O.FileData"));
        this.TabsItemsSource.push(new TabItem("ReasonsAndExplanitaions", "Customs.Claim.TH.ReasonsAndExplanitaions"));
        this.TabsItemsSource.push(new TabItem("ExportDeclaration", "Customs.Claim.O.ClaimsRelatedEntityAdditional"));
        this.TabsItemsSource.push(new TabItem("ClaimDecision", "Customs.Claim.TH.ClaimDecision"));
        this.TabsItemsSource.push(new TabItem("CustomAnswer", "Customs.Claim.TH.CustomAnswer"));
        this.selectedTabCode = "FileData";
    }

    RunComponent() {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private isViewInited = false;
    InitializeComponent() {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    private FileData: any = null;
    private ExportDeclaration: any = null;
    private ReasonsAndExplanitaions: any = null;
    private CustomAnswer: any = null;
    private ClaimDecision: any = null;
    public SelectedTab: TabItem;
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {

                    case "FileData": {
                        if (this.FileData == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityGeneralTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.FileData = cmpRef.instance;
                                    this.FileData.InitTab(this.ClaimPM.ClaimsRelatedEntities.filter(d => d.EntityCounterKey == this.EntityCounterKey)[0], this.ClaimPM, !this.IsDisplayOnly);
                                });
                        }
                        break;
                    }
                    case "ReasonsAndExplanitaions": {
                        if (this.ReasonsAndExplanitaions == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityReasonsTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.ReasonsAndExplanitaions = cmpRef.instance;
                                    this.ReasonsAndExplanitaions.InitTab(this.ClaimPM.ClaimsRelatedEntities.filter(d => d.EntityCounterKey == this.EntityCounterKey)[0], this.ClaimPM, !this.IsDisplayOnly);
                                });
                        }
                        break;
                    }
                    case "ExportDeclaration": {
                        if (this.ExportDeclaration == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityAdditionalDataTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.ExportDeclaration = cmpRef.instance;
                                    this.ExportDeclaration.InitTab(this.ClaimPM.ClaimsRelatedEntities.filter(d => d.EntityCounterKey == this.EntityCounterKey)[0], this.ClaimPM, !this.IsDisplayOnly);
                                });
                        }
                        break;
                    }
                    case "CustomAnswer": {
                        if (this.CustomAnswer == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityCustomAnswerTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.CustomAnswer = cmpRef.instance;
                                    this.CustomAnswer.InitTab(this.ClaimPM.ClaimsRelatedEntities.filter(d => d.EntityCounterKey == this.EntityCounterKey)[0], this.ClaimPM, !this.IsDisplayOnly);
                                });
                        }
                        break;
                    }
                    case "ClaimDecision": {
                        if (this.ClaimDecision == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityClaimDecisionTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.ClaimDecision = cmpRef.instance;
                                    this.ClaimDecision.InitTab(this.ClaimPM.ClaimsRelatedEntities.filter(d => d.EntityCounterKey == this.EntityCounterKey)[0], this.ClaimPM, !this.IsDisplayOnly);
                                });
                        }
                        break;
                    }
                }
            }
        }
    }

    itemsLineNumbers: string;

    // Buttons Handlers
    OkButtonClicked() {
        this.SaveButtonClicked();
    }

    CancelButtonClicked() {
        this.ClaimPM.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    }

    //#region Save Code
    loadingNextItems: boolean = false;
    closeWindow: boolean;

    sendMode: boolean;
    public get SendMode() { return this.sendMode; }
    public set SendMode(value: boolean) {
        this.sendMode = value;
    }

    SaveButtonClicked() {
        var valid = this.PreSaveValidate();

        this.closeWindow = true;
        if (valid) {
            this.SaveChanges();
        }
    }

    private SaveChanges() {
        this.CurrentSession.StartBusyIndicatorSaving();
        //if (this.IsNewEntity) {
        //    this.ClaimPMService.insert(this.ClaimPM).subscribe((myResult:any) => {
        //        var res: ServiceResponse = myResult;
        //        if (!res.HasError) {
        //            var entity = res.Result;
        //            console.log("..Saved Successfully ", entity);
        //            if (this.closeWindow) {
        //                this.CurrentSession.CloseCurrentWindow();
        //            }
        //        }
        //        else {
        //            this.ValidationErrorsList = res.ErrorsArray;
        //        }
        //        this.CurrentSession.StopBusyIndicator();
        //        return false;
        //    });

        //} else {
    
        


        //this.ClaimPMService.update(this.ClaimPM)
        this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((myResult:any) => {
            var res: ServiceResponse = myResult;

            // EditComponent.SaveCompleted returns true on success, false on error
            // if (!res.HasError) {
            if (res) {
                var entity = res.Result;

                console.log("..Saved Successfully ", entity);
                if (this.closeWindow) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.CloseCurrentWindowEmit('ok');
                }
            }
            else {
                this.ValidationErrorsList = res.ErrorsArray;
            }
            this.CurrentSession.StopBusyIndicator();
            return false;
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges();

        //}
    }

    PreSaveValidate() {
        var errors = [];
        Validator.TryValidateObject(this.ClaimPM, this.ObjectTableName, errors);

        var entityPM: ClaimsRelatedEntityPM = this.ClaimPM.ClaimsRelatedEntities.filter(d => d.EntityCounterKey == this.EntityCounterKey)[0];

        if (AppTool.IsNullOrEmpty(entityPM.ClaimEntityTypeCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntity.F.ClaimEntityTypeCode"));
        }
        if (AppTool.IsNullOrEmpty(entityPM.ClaimEntityNumber)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntity.F.ClaimEntityNumber"));
        }
        //if (AppTool.IsNullOrEmpty(this.EntityPM.IsFinancialRefundDemand)) {
        //    errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntity.F.IsFinancialRefundDemand"));
        //}
        if (AppTool.IsNullOrEmpty(entityPM.ClaimExplanation)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntity.F.ClaimExplanation"));
        }

        if (entityPM.IsFinancialRefundDemand == true) {
            if (entityPM.ClaimsRelatedEntitiesAmounts != null && entityPM.ClaimsRelatedEntitiesAmounts.length > 0) {
                for (let amountItem of entityPM.ClaimsRelatedEntitiesAmounts) {
                    if (AppTool.IsNullOrEmpty(amountItem.PaymentTypeCode)) {
                        errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntitiesAmount.F.PaymentTypeCode"));
                    }
                    if (AppTool.IsNullOrEmpty(amountItem.Amount)) {
                        errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntitiesAmount.F.Amount"));
                    }
                }
            }
        }

        if (entityPM.ClaimsRelatedEntitiesReasons != null && entityPM.ClaimsRelatedEntitiesReasons.length > 0) {
            for (let reasonItem of entityPM.ClaimsRelatedEntitiesReasons) {
                if (AppTool.IsNullOrEmpty(reasonItem.ReasonListTypeCode)) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.ClaimsRelatedEntitiesReason.F.ReasonListTypeCode"));
                }
            }
        }

        if (errors.length == 0) {
            return true;
        } else {
            this.ValidationErrorsList = errors;
            return false;
        }
    }

    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }

    //#endregion

    LogMe(mess) {
        var alertIt = false;
        if (alertIt) {
            alert(mess);
        } else {
            console.log(mess);
        }

    }

}

class TabItem {
    public code: string;
    public textCode: string;
    constructor(Code: string, TextCode: string) {
        this.code = Code;

        this.textCode = TextCode;
    }
}
