import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DecCargoSplitConPM } from '../../../../Customs/EntityPMs/DecCargoSplitConPM';
import { DecCargoSplitConsItemPM } from '../../../../Customs/EntityPMs/DecCargoSplitConsItemPM';
import { DeclarationCargoSplitPM } from '../../../../Customs/EntityPMs/DeclarationCargoSplitPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { CollateralsRequestFileCondPM } from '../../../../Customs/EntityPMs/CollateralsRequestFileCondPM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
declare var window: any;
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import { ClientList } from '../../../../Customs/EntityLists/ClientList';
import { TreatmentWayPM } from  '../../../../Customs/EntityPMs/TreatmentWayPM';
import { TreatmentWayListService } from  '../../../../Customs/Services/StandardLists/TreatmentWayListService';
import { DeclarationPMService } from '../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { DecCargoSplitConExtendedPMService } from '../../../../Customs/Services/ExtendedPMs/DecCargoSplitConExtendedPMService';

@Component({
    
    templateUrl: './DecCargoSplitConComponent.html',
})
export class DecCargoSplitConComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.DecCargoSplitCon";
    public DataContext: any = this;
    public IsDisplayOnly: boolean = false;
    public DisplayOnlyMessage: string = "";
    public EntityPM: DecCargoSplitConPM;
    public PreceduralFilterItems: ApiQueryFilters;
    public IsImporerCodeEnabled: boolean = true;
    private currentEditComponentId: string;
    declarationCargoSplitPM: DeclarationCargoSplitPM;
    answerFileFilterItems: ApiQueryFilters;
    ItemsList: ObservableCollection;
    IsClosed: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public CD: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();
        this.ItemsList = new ObservableCollection([]);
        
        this.SetDisplayFields();
        //EntityResourceService.getEntityResourceByTableName("Customs.TreatmentWay", 0).subscribe((res: any) => {
            //var entityListService: TreatmentWayListService = new TreatmentWayListService();
            //entityListService.getAllFromCache().subscribe((res: any) => {
                //this.Listen();
            //});
        //});
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

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
            //this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
            //    this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
            //        if (this.currentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
            //            if (tabCode == "DEGC") {
            //                this.DisplayOnlyCheck();
            //            }
            //        }
            //    })
            //);
        }
    }

    ParentCargoConsinmentItemList: any;
    decCargoSplitConExtendedPMService: DecCargoSplitConExtendedPMService;
    DeclarationDirection: string;
    declarationExtendedListService: DeclarationExtendedListService;
    DecCargoSplitConStatusVisibility: boolean;

    RefreshTabs(direction) {
        this.DeclarationDirection = direction;
        if (this.DeclarationDirection == "E") {
            this.PreceduralFilterItems = new ApiQueryFilters();
            this.PreceduralFilterItems.addAdditionalFilter("Code", "1000000,8000000,4000000", null, null, "InListExact", false, false, false, "string", false, true);
        }
        this.SetDisplayFields();

    }
    SetTabArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.declarationCargoSplitPM = args.Parent;
        if (this.declarationCargoSplitPM != null) {
            this.declarationExtendedListService = new DeclarationExtendedListService();
            this.declarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.declarationCargoSplitPM.CustomFileNo).subscribe((response: any) => {
                if (response.Result != null) {
                    this.DeclarationDirection = response.Result.Direction;
                    if (this.DeclarationDirection == "E") {
                        this.PreceduralFilterItems = new ApiQueryFilters();
                        this.PreceduralFilterItems.addAdditionalFilter("Code", "1000000,8000000,4000000", null, null, "InListExact", false, false, false, "string", false, true);
                    }
                    this.decCargoSplitConExtendedPMService = new DecCargoSplitConExtendedPMService();
                    this.decCargoSplitConExtendedPMService.GetConsiPackageSequeList(response.Result.Id).subscribe((responseCon: any) => {
                        if (responseCon != null) {
                            this.ParentCargoConsinmentItemList = responseCon.Result;
                        }
                    });
                    this.SetDisplayFields();
                }
            });
        }
    
        this.IsClosed = this.declarationCargoSplitPM.IsClosed;
        this.IsDisplayOnly = args.Disabled;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.DecCargoSplitConsItems)) {
            for (let conItem of this.EntityPM.DecCargoSplitConsItems) {
                if (this.ParentCargoConsinmentItemList != null && this.ParentCargoConsinmentItemList[0] != null) {
                    item.ParentCargoConsinmentItem = this.ParentCargoConsinmentItemList[0];
                }
                var item = new DecCargoSplitConsItemModel(conItem);
                this.ItemsList.Insert(item);
            }
        }
        this.CurrentSession.SubscriptionAdd(
        this.CurrentSession.CollateralAnswerRefreshEvent.subscribe((res) => {
            this.SetClosedDeclarationCargoSplitScreesn(res.IsClosed);
            })
        );
            if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
                this.PreceduralFilterItems = new ApiQueryFilters();
                this.PreceduralFilterItems.addAdditionalFilter("IsImport", true, null, null, "Equals", false, false, false, "boolean",false,false);
            }

        this.firstTime = true;
      
        this.SetClosedDeclarationCargoSplitScreesn(this.declarationCargoSplitPM.IsClosed);

        if (this.declarationCargoSplitPM.IsClosed) {
                this.IsClosed = true;
        }

        else {
            this.DisplayOnlyMessageVisibility = false;
        }
        this.SetDisplayFields();
        /*
        if (this.EntityPM.CustomsTapgFile != null) {
            this.RequestNumberLabelVisibility = true;
            this.TapagFileLabelVisibility = true;
            if (!AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && !AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagFile + "/" + this.EntityPM.RequestedTapagNumeral;
            }

            else if (!AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagFile;
            }

            else if (AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && !AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagNumeral;
            }


            if (!AppTool.IsNullOrEmpty(this.CustomsTapgFile) && !AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsTapgFile + "/" +this.CustomsNumeral;
            }

            else if (!AppTool.IsNullOrEmpty(this.CustomsTapgFile) && AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsTapgFile;
            }

            else if (AppTool.IsNullOrEmpty(this.CustomsTapgFile) && !AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsNumeral;
            }
        }
        


        if (this.EntityPM.PaymentOrderId != null) {
            this.PaymentOrderVsibility = true;
            this.PaymentOrder = this.EntityPM.PaymentOrderNumber;
            this.PaymentOrderStatus = this.EntityPM.PaymentOrderStatus;

           
        }
        */
        
    }

    SetDisplayFields() {
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessageVisibility = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ConditionCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, false);
            this.DisplayOnlyMessageVisibility = true;
        }
        else {
            this.DisplayOnlyMessageVisibility = false;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ConditionCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, true);
            this.DisplayOnlyMessageVisibility = false;
        }
        if (this.DeclarationDirection == "E") {
            this.UIProperties.SetEnabled("ConditionCode", this.ObjectTableName, false);
        }
        this.IsImporerCodeEnabled = !this.IsDisplayOnly;
    }


    SetClosedDeclarationCargoSplitScreesn(IsClosed: boolean) {
        if (IsClosed) {
            this.IsClosed = true;

            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ConditionCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, false);

            //if (this.EntityPM.NewFileRequest) {
            //    this.IsNewFile = true;
            //}
            // else {

            //if (this.EntityPM.AnswerEntityTypeCode != null && this.EntityPM.AllocatedAmount != null) {
            //    this.IsTapag = true;
            //}
            //}


        }
        else {
            this.IsClosed = false;
            /*
            if (!this.EntityPM.NewFileRequest) {

                if (this.EntityPM.AnswerEntityTypeCode != null && this.EntityPM.AllocatedAmount != null) {
                    this.IsTapag = true;
                }

                else if (this.EntityPM.AnswerEntityTypeCode == null && this.EntityPM.AllocatedAmount == null) {
                    return;

                }


                this.IsTapag = true;
                if (this.EntityPM.AnswerEntityTypeCode == null) {
                    this.AnswerEntityRedIconVisibility = true;
                }
                else {
                    this.AnswerEntityRedIconVisibility = false;
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile)) {
                    this.CustomsTapgFileRedIconVisibility = true;
                }
                else {
                    this.CustomsTapgFileRedIconVisibility = false;
                }

                if (this.EntityPM.AllocatedAmount == null) {
                    this.AllocatedAmountRedIconVisibility = true;
                }
                else {
                    this.AllocatedAmountRedIconVisibility = false;
                }
            }

            if (this.EntityPM.NewFileRequest) {
                this.IsNewFile = true;

                if (this.EntityPM.RequestFileTypeCode != null) {
                    this.RequestFileCodeRedIconVisibility = false;
                }
                else {
                    this.RequestFileCodeRedIconVisibility = true;
                }

                if (this.EntityPM.RequestFileAmount != null) {
                    this.RequestFileRedIconVisibility = false;
                }
                else {
                    this.RequestFileRedIconVisibility = true;
                }

            }
        }
            */
        }
    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            return;
        }
        this.IsImporerCodeEnabled = !this.IsDisplayOnly;
       /*
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe((response: any) => {
            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
            this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            
        });
        */
    }

    checkImportersVisibility() {
        if (this.IsDisplayOnly) return;

        if (!AppTool.IsNullOrEmpty(this.ImporterCode))
            if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                this.IsImporerCodeEnabled = false;
                this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            }

    }


    //#region Commands + Handlers
    private timerToken: any;
    private isImporterClicked: boolean = false;

    ImporterClicked(client: ClientList) {
        if (client) {
            this.isImporterClicked = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            this.ImporterCode = client.Code;
        }

    }

    ImporterLostFocus(item: any, importerSearchBox: any) {

        this.isImporterClicked = false;
        this.ImporterCode = item;
    }

    AddItem() {
        if (!this.IsDisplayOnly) {
            var counter: number = 0;
            if (this.EntityPM.DecCargoSplitConsItems.length > 0) {

                var items = this.EntityPM.DecCargoSplitConsItems.sort((a, b) => { return (a.ItemLine === b.ItemLine) ? 0 : (a.ItemLine < b.ItemLine) ? -1 : 1 });
                if (items.length == 0) counter = 0;
                else {
                    counter = items[this.EntityPM.DecCargoSplitConsItems.length - 1].ItemLine;
                }

            }

            counter += 1;

            var item: DecCargoSplitConsItemPM = new DecCargoSplitConsItemPM(this.EntityPM);

            item.DeclarationCargoSplitId = this.EntityPM.DeclarationCargoSplitId;
            item.Tenant = this.EntityPM.Tenant;
            item.DecCargoSplitConsLineNo = this.EntityPM.LineNumber;
            item.ItemLine = counter;
            if (this.ParentCargoConsinmentItemList != null && this.ParentCargoConsinmentItemList[0] != null) {
                item.ParentCargoConsinmentItem = this.ParentCargoConsinmentItemList[0];
            }
            if (!this.EntityPM.DecCargoSplitConsItems.includes(item)) {
                this.EntityPM.AddDecCargoSplitConsItem(item);
                var line = new DecCargoSplitConsItemModel(item);
                this.ItemsList.Insert(line);
            }

        }

    }

    OnRowEnded($event) {
        console.log("this.ItemsList.Length : " + this.ItemsList.Length);
        if (($event) == this.ItemsList.Length) {
            this.AddItem();
        }
    }

    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
        if (this.ItemsList.Length == 0) {
            this.AddItem();
        }
    }

    OnFocus() {
        if (this.ItemsList.Length == 0) {
            this.AddItem();
        }
    }

    DeleteButtonClicked(item: any) {

        //this.DeleteSelected(item);
        //return;
        let confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");

        confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeletePackage"));
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.DeleteSelected(item);
            }
            else if (confirmWindow.No) {

            }
        });
                //this.customsDocumentPointerService.GetCheckForPointers(item.DeclarationId, item.InvoiceCounterKey).subscribe((myResponse: ServiceResponse) => {
                //    var exist = myResponse;
                //   if (myResponse.Result) {
                //       var confirmWindow1 = new ConfirmWindow();

                //       //     confirmWindow.DisplayWariningIconImage();
                //       confirmWindow1.Width = 400;

                //       confirmWindow1.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                //       confirmWindow1.Height = 190;
                //       confirmWindow1.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                //       confirmWindow1.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
                //       confirmWindow1.ShowWarningImage = true;
                //       confirmWindow1.ShowNoButton
                //       confirmWindow1.Show(TextCodeTranslator.Translate("Customs.General.O.InvoiceRelatedPoiner"));
                //       confirmWindow1.WindowClosed.subscribe((event: any) => {
                //           if (confirmWindow1.Yes) {
                //               this.DeleteSelected(item);
                //           } else if (confirmWindow1.No) {

                //           }
                ////       });
                ////   }

                //   else {
                //this.DeleteSelected(item);
            //}
                    //}

          //      });

        //    } else if (confirmWindow.No) {

        //    }
        //});
        
    }

    lastDeletedItem: DecCargoSplitConsItemPM;
    DeleteSelected(item: any) {
        //this.CurrentSession.StartBusyIndicator("");
        this.lastDeletedItem = item.EntityPM;

        //var SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            //SaveCompletedEvent.unsubscribe();
            this.ItemsList.Remove(item);
            this.EntityPM.RemoveDecCargoSplitConsItem(item.EntityPM);
            /*
            this.supplierInvoiceExtendedPMService.delete(item.DeclarationId, item.InvoiceCounterKey).subscribe((myResponse: ServiceResponse) => {

                if (!myResponse.HasError) {
                    this.ItemsSource.Remove(item);
                    
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.StopBusyIndicator();

                }


            });
            */
        //});

        //this.CurrentSession.CurrentEditComponent.SaveChanges();
        
    }

    ParentCargoConsinmentItemSelectionChanged(item, value) {
        if (item != null) {
            item.ParentCargoConsinmentItem = value;
        }
    }

    EditButtonClicked(item) {

        if (!AppTool.IsNullOrEmpty(item.EntityPM) && !AppTool.IsNullOrEmpty(item.EntityPM.ItemLine)) {
            var windowArgs: any = {};
            windowArgs.DecCargoSplitConsItemPM = item.EntityPM;
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            windowArgs.DeclarationCargoSplitPM = this.declarationCargoSplitPM;
            windowArgs.DeclarationDirection = this.DeclarationDirection
            var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ConsignmentPackages");

            var logWindow = new LogitudeWindow();
            logWindow.Width = 750;
            logWindow.Height = 350;
            logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.ConsignmentPackages");
            if (item.EntityPM.ParentCargoConsinmentItem != null) {
                logWindow.Title = logWindow.Title + " סידורי " + item.ParentCargoConsinmentItem;
            }

            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "ok") {
                    //this.CD.reattach();
                    this.RefreshEntity();
                }
                this.SetStatusVisibility()
            });
            logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/DecCargoSplitConsPackDetComponent');
            
        }
        /*
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", errors);

        for (let item of this.EntityPM.Consignments) {
            for (let line of item.ConsignmentPackages) {
                if (line.MarksNumbers == null && line.PackageMeasureQualifierCode == null && line.PackageQuantity == null && line.PackageTypeCode == null && line.GrossMassMeasure == null) {
                    var errorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.EmptyConsignmentPackage");
                    if (!AppTool.IsNullOrEmpty(errorMessage)) {
                        errors.push(errorMessage);

                    }
                }

            }
        }


        if (errors.length > 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }


        else {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicator("");
                this.declarationPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                    var declaration = response.Result;
                    this.CurrentSession.StopBusyIndicator();
                    if (!AppTool.IsNullOrEmpty(declaration)) {
                        if (!AppTool.IsNullOrEmpty(item)) {
                            this.EditItem(item);

                        }
                    }

                });
            }
            else {
                this.EditItem(item);
            }

        }
        */
    }

    RefreshEntity() {
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
        
    }


    SetStatusVisibility() {


        if (this.SelectedRow.EntityPM.ChangeSetOp = "Update") {
            this.conItemPackDetStatusVisibility = true;
        }

        else {

            this.conItemPackDetStatusVisibility = false;
        }
    }

    private conItemPackDetStatus = false;
    get ConItemPackDetStatus() { return this.conItemPackDetStatus; }
    set ConItemPackDetStatus(value: boolean) { this.conItemPackDetStatus = value; }

    private conItemPackDetStatusVisibility = false;
    get ConItemPackDetStatusVisibility() { return this.conItemPackDetStatusVisibility; }
    set ConItemPackDetStatusVisibility(value: boolean) { this.conItemPackDetStatusVisibility = value; }

    EditItem(item: DecCargoSplitConsItemPM) {
        this.CurrentSession.StartBusyIndicator("");
        /*
        var supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();

        this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.Id, item.InvoiceCounterKey, 0, this.NumberOfLoadedItems, "parent").subscribe((response:any) => {

            var windowArgs: any = {};
            windowArgs.EntityPM = response.Result;
            windowArgs.declarationPM = this.EntityPM;
            windowArgs.NumberOfLoadedItems = this.NumberOfLoadedItems;
            var windowTitle = "Supplier Invoice";

            var logWindow = new LogitudeWindow();
            logWindow.Width = 995; // don't change this width!
            logWindow.Height = 600;

            if (!AppTool.IsNullOrEmpty(item.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditItem") + " " + item.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

            }
            else if (AppTool.IsNullOrEmpty(item.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditItem") + " " + this.EntityPM.DeclarationNumber;

            }
            else if (!AppTool.IsNullOrEmpty(item.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditItem") + " " + item.InvoiceNumber;

            }
            else if (AppTool.IsNullOrEmpty(item.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditItem");

            }
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            this.CD.detach();
            logWindow.WindowClosed.subscribe((event: any) => {
                if (event != 'cancel') {

                    this.RefreshEntity();
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                else {
                    this.ReloadMyScreen();
                }
                this.CD.reattach();

            });
            logWindow.IsHideHeader = true;
            logWindow.Show('./Customs/Components/Declaration/EditTabs/SupplierInvoices/AddEditSupplierInvoiceComponent');
            this.CurrentSession.StopBusyIndicator();
        });
        */
    }

    public get ImporterCode() { return this.EntityPM ? this.EntityPM.ImporterCode : null; }
    public set ImporterCode(newValue: string) {
        this.EntityPM.ImporterCode = newValue;
        //this.UIProperties.SetRequired("Code", "Customs.Client", AppTool.IsNullOrEmpty(newValue));
        this.UIProperties.SetRequired("ImporterCode", "Customs.DecCargoSplitCon", AppTool.IsNullOrEmpty(newValue));
    }

    public get ProcedureCurrentCode() { return this.EntityPM ? this.EntityPM.ProcedureCurrentCode : null; }
    public set ProcedureCurrentCode(newValue: string) {
        this.EntityPM.ProcedureCurrentCode = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            //this.RequestFileRedIconVisibility = true;
        }
        else {
            //this.RequestFileRedIconVisibility = false;
        }
    }

    public get ConditionCode() { return this.EntityPM ? this.EntityPM.ConditionCode : null; }
    public set ConditionCode(newValue: string) {
        this.EntityPM.ConditionCode = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            //this.RequestFileRedIconVisibility = true;
        }
        else {
            //this.RequestFileRedIconVisibility = false;
        }
    }

    /*
    private timerToken: any;
    UseRequestNewFile(newValue: string) {

        if (!this.DeclarationCargoSplitPM.IsClosed) {
            this.IsNewFile = null;
            this.IsTapag = null;

            if (this.EntityPM.AnswerEntityTypeCode != null || !AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile) || this.EntityPM.AllocatedAmount != null || !AppTool.IsNullOrEmpty(this.EntityPM.CustomsNumeral) || !AppTool.IsNullOrEmpty(this.EntityPM.Remarks)) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteDeclarationCargoSplit"));
                this.timerToken = setTimeout(() => {
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.IsNewFile = true;
                            this.IsTapag = false;
                            if (this.EntityPM.NewFileRequest) {
                                this.CustomsTapgFile = null;
                                this.AllocatedAmount = null;
                                this.AnswerEntityTypeCode = null;
                                this.CustomsNumeral = null;
                                this.Remarks = null;
                                this.AnswerEntityRedIconVisibility = false;
                                this.AllocatedAmountRedIconVisibility = false;
                                this.CustomsTapgFileRedIconVisibility = false;
                            }
                            else {
                                this.RequestFileAmount = null;
                                this.RequestFileTypeCode = null;
                                this.RequestFileCodeRedIconVisibility = false;
                                this.RequestFileRedIconVisibility = false;
                            }
                        }
                        else {
                            this.IsNewFile = false;
                            this.IsTapag = true;
                        }

                    });
                }, 200);






            }

            else {
                this.IsNewFile = true;
                this.IsTapag = false;

                this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, true);
            }

            this.AnswerEntityRedIconVisibility = false;
            this.AllocatedAmountRedIconVisibility = false;
            this.CustomsTapgFileRedIconVisibility = false;


            //else {
            //    this.AnswerEntityRedIconVisibility = false;
            //    this.AllocatedAmountRedIconVisibility = false;
            //    this.CustomsTapgFileRedIconVisibility = false;

            //}

        }
    }
    
    UseExistingTapagFile(newValue: string) {

        if (!this.DeclarationCargoSplitPM.IsClosed) {

            this.IsTapag = null;
            this.IsNewFile = null;



            if (this.EntityPM.RequestFileAmount != null || this.EntityPM.RequestFileTypeCode != null) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteDeclarationCargoSplit"));
                this.timerToken = setTimeout(() => {
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.IsTapag = true;
                            this.IsNewFile = false;
                            if (this.EntityPM.NewFileRequest) {
                                this.CustomsTapgFile = null;
                                this.AllocatedAmount = null;
                                this.AnswerEntityTypeCode = null;
                                this.CustomsNumeral = null;
                                this.Remarks = null;
                                this.AnswerEntityRedIconVisibility = false;
                                this.AllocatedAmountRedIconVisibility = false;
                                this.CustomsTapgFileRedIconVisibility = false;
                            }
                            else {
                                this.RequestFileAmount = null;
                                this.RequestFileTypeCode = null;
                                this.RequestFileCodeRedIconVisibility = false;
                                this.RequestFileRedIconVisibility = false;
                            }
                        }
                        else {
                            this.IsTapag = false;
                            this.IsNewFile = true;
                        }

                    });
                }, 1000);
            }
            else {
                this.IsTapag = true;
                this.IsNewFile = false;
                this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, true);

            }


            this.RequestFileCodeRedIconVisibility = false

            this.RequestFileRedIconVisibility = false;



            //this.RequestFileCodeRedIconVisibility = false

            //this.RequestFileRedIconVisibility = false;

        }
        
    }
    */
    //#region properties
    /*
    private paymentOrderStatus: string;
    public get PaymentOrderStatus() { return this.paymentOrderStatus; }
    public set PaymentOrderStatus(newValue: string) {
        this.paymentOrderStatus = newValue;
    }

    private paymentOrder: string;
    public get PaymentOrder() { return this.paymentOrder; }
    public set PaymentOrder(newValue: string) {
        this.paymentOrder = newValue;
    }
    */
    firstTime: boolean;

    private displayOnlyMessageVisibility: boolean;
    public get DisplayOnlyMessageVisibility() { return this.displayOnlyMessageVisibility; }
    public set DisplayOnlyMessageVisibility(newValue: boolean) { this.displayOnlyMessageVisibility = newValue; }

    /*
    
    private isNewFile: boolean;
    public get IsNewFile() { return this.isNewFile; }
    public set IsNewFile(newValue: boolean)
    {
        this.isNewFile = newValue;
        if (newValue && !this.DeclarationCargoSplitPM.IsClosed) {

            this.IsTapag = false;
            this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, true);

            this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, false);
            this.EntityPM.NewFileRequest = true;

            if (this.EntityPM.RequestFileTypeCode != null) {
                this.RequestFileCodeRedIconVisibility = false;
            }
            else {
                this.RequestFileCodeRedIconVisibility = true;
            }

            if (this.EntityPM.RequestFileAmount != null) {
                this.RequestFileRedIconVisibility = false;
            }
            else {
                this.RequestFileRedIconVisibility = true;
            }

          
        //    this.firstTime = false;

            if ((this.EntityPM.CollateralsRequestFileConds == null || this.EntityPM.CollateralsRequestFileConds.length == 0) && (this.collateralPM != null && this.collateralPM.DeclarationCargoSplitsConditions != null)) 
            {
                for (var item of this.collateralPM.DeclarationCargoSplitsConditions)
                {
                    var collateralsRequestFileCond: CollateralsRequestFileCondPM = new CollateralsRequestFileCondPM(this.collateralPM);
                    collateralsRequestFileCond.DeclarationCargoSplitId = this.EntityPM.DeclarationCargoSplitId;
                    collateralsRequestFileCond.LineNumber = this.EntityPM.LineNumber;
                    collateralsRequestFileCond.Tenant = this.EntityPM.Tenant;
                    collateralsRequestFileCond.ConditionCode = item.ConditionCode;
                    collateralsRequestFileCond.ConditionName = item.ConditionName;
                    collateralsRequestFileCond.RequestedAmount = item.RequestedAmount;
                    this.EntityPM.AddCollateralsRequestFileCond(collateralsRequestFileCond);
                }
            }

        }
    }
   

   private isTapag: boolean;
     public get IsTapag() { return this.isTapag; }
     public set IsTapag(newValue: boolean) {
         this.isTapag = newValue;

         if (newValue && !this.collateralPM.IsClosed) {

             this.IsNewFile = false;
             this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, false);
             this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, false);

             this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, true);
             this.EntityPM.NewFileRequest = false;

             if (this.EntityPM.AnswerEntityTypeCode == null) {
                 this.AnswerEntityRedIconVisibility = true;
             }
             else {
                 this.AnswerEntityRedIconVisibility = false;
             }

             if (AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile)) {
                 this.CustomsTapgFileRedIconVisibility = true;
             }
             else {
                 this.CustomsTapgFileRedIconVisibility = false;
             }

             if (this.EntityPM.AllocatedAmount == null) {
                 this.AllocatedAmountRedIconVisibility = true;
             }
             else {
                 this.AllocatedAmountRedIconVisibility = false;
             }
            
           //  this.firstTime = false;

         }
     }
 

     

    private tapagFile: string;
    public get TapagFile() { return this.tapagFile; }
    public set TapagFile(newValue: string) { this.tapagFile = newValue; }

    private requestNumber: string;
    public get RequestNumber() { return this.requestNumber; }
    public set RequestNumber(newValue: string) { this.requestNumber = newValue; }

    public get AnswerEntityTypeCode() { return this.EntityPM ? this.EntityPM.AnswerEntityTypeCode : null; }
    public set AnswerEntityTypeCode(newValue: string) {
        this.EntityPM.AnswerEntityTypeCode = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.AnswerEntityRedIconVisibility = true;
        }
        else {
            this.AnswerEntityRedIconVisibility = false;
        }
    }

    public get AnswerForCollateralStatusName() { return this.EntityPM ? this.EntityPM.AnswerForCollateralStatusName : null; }

    public get AllocatedAmount() { return this.EntityPM ? this.EntityPM.AllocatedAmount : null; }
    public set AllocatedAmount(newValue: number) {
        this.EntityPM.AllocatedAmount = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.AllocatedAmountRedIconVisibility = true;
        }
        else {
            this.AllocatedAmountRedIconVisibility = false;
        }
    }


    public get CustomsTapgFile() { return this.EntityPM ? this.EntityPM.CustomsTapgFile : null; }
    public set CustomsTapgFile(newValue: string) {
        this.EntityPM.CustomsTapgFile = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.CustomsTapgFileRedIconVisibility = true;
        }
        else {
            this.CustomsTapgFileRedIconVisibility = false;
        }
    }

    public get CustomsNumeral() { return this.EntityPM ? this.EntityPM.CustomsNumeral : null; }
    public set CustomsNumeral(newValue: string) { this.EntityPM.CustomsNumeral = newValue; }

    public get Remarks() { return this.EntityPM ? this.EntityPM.Remarks : null; }
    public set Remarks(newValue: string) { this.EntityPM.Remarks = newValue; }

    public get RequestFileTypeName() { return this.EntityPM ? this.EntityPM.RequestFileTypeName : null; }
    public set RequestFileTypeName(newValue: string) { this.EntityPM.RequestFileTypeName = newValue; }

    public get RequestFileTypeCode() { return this.EntityPM ? this.EntityPM.RequestFileTypeCode : null; }
    public set RequestFileTypeCode(newValue: string) {
        this.EntityPM.RequestFileTypeCode = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.RequestFileCodeRedIconVisibility = true;
        }
        else {
            this.RequestFileCodeRedIconVisibility = false;
        }
    }


    public get RequestFileAmount() { return this.EntityPM ? this.EntityPM.RequestFileAmount : null; }
    public set RequestFileAmount(newValue: number) {
        this.EntityPM.RequestFileAmount = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.RequestFileRedIconVisibility = true;
        }
        else {
            this.RequestFileRedIconVisibility = false;
        }
    }

    public get NewFileRequest() { return this.EntityPM ? this.EntityPM.NewFileRequest : null; }
    public set NewFileRequest(newValue: boolean) {
        this.EntityPM.NewFileRequest = newValue;

    }

    private customsTapgFileRedIconVisibility: boolean;
    public get CustomsTapgFileRedIconVisibility() { return this.customsTapgFileRedIconVisibility; }
    public set CustomsTapgFileRedIconVisibility(newValue: boolean) { this.customsTapgFileRedIconVisibility = newValue; }

    private allocatedAmountRedIconVisibility: boolean;
    public get AllocatedAmountRedIconVisibility() { return this.allocatedAmountRedIconVisibility; }
    public set AllocatedAmountRedIconVisibility(newValue: boolean) { this.allocatedAmountRedIconVisibility = newValue; }

    private answerEntityRedIconVisibility: boolean;
    public get AnswerEntityRedIconVisibility() { return this.answerEntityRedIconVisibility; }
    public set AnswerEntityRedIconVisibility(newValue: boolean) { this.answerEntityRedIconVisibility = newValue; }

    private requestFileRedIconVisibility: boolean;
    public get RequestFileRedIconVisibility() { return this.requestFileRedIconVisibility; }
    public set RequestFileRedIconVisibility(newValue: boolean) { this.requestFileRedIconVisibility = newValue; }

    private requestFileCodeRedIconVisibility: boolean;
    public get RequestFileCodeRedIconVisibility() { return this.requestFileCodeRedIconVisibility; }
    public set RequestFileCodeRedIconVisibility(newValue: boolean) { this.requestFileCodeRedIconVisibility = newValue; }

    private paymentOrderVsibility: boolean;
    public get PaymentOrderVsibility() { return this.paymentOrderVsibility; }
    public set PaymentOrderVsibility(newValue: boolean) { this.paymentOrderVsibility = newValue; }

    private tapagFileLabelVisibility: boolean;
    public get TapagFileLabelVisibility() { return this.tapagFileLabelVisibility; }
    public set TapagFileLabelVisibility(newValue: boolean) { this.tapagFileLabelVisibility = newValue; }

    private requestNumberLabelVisibility: boolean;
    public get RequestNumberLabelVisibility() { return this.requestNumberLabelVisibility; }
    public set RequestNumberLabelVisibility(newValue: boolean) { this.requestNumberLabelVisibility = newValue; }


    private answerSentTextVisibility: boolean;
    public get AnswerSentTextVisibility() { return this.answerSentTextVisibility; }
    public set AnswerSentTextVisibility(newValue: boolean) { this.answerSentTextVisibility = newValue; }

    currentScreenCode: string;
    OpenPaymentOrder() {
        this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe((response:any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe((response:any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderProtestReason").subscribe((response:any) => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsSetting").subscribe((response:any) => {

                            this.EditEntity("Customs.PaymentOrder", this.EntityPM.PaymentOrderId, null, "POGN");
                        });
                    });
                });
            });
        });


                        }


    */

    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {

        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe((res:any) => {
            

        });

    }
    
    //#endregion
}


export class DecCargoSplitConsItemModel extends BaseComponent {
    public EntityPM: DecCargoSplitConsItemPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(line: DecCargoSplitConsItemPM) {
        super();
        this.EntityPM = line;
    }

    //#region Properties
    public get CargoDescription() { return this.EntityPM.CargoDescription; }
    public set CargoDescription(newValue: string) { this.EntityPM.CargoDescription = newValue; }
    
    public get RequestReasonName() { return this.EntityPM.RequestReasonName; }
    public set RequestReasonName(newValue: string) { this.EntityPM.RequestReasonName = newValue; }

    public get RequestReasonCode() { return this.EntityPM.RequestReasonCode; }
    public set RequestReasonCode(newValue: string) { this.EntityPM.RequestReasonCode = newValue; }

    public get ParentCargoConsinmentItem() { return this.EntityPM.ParentCargoConsinmentItem; }
    public set ParentCargoConsinmentItem(newValue: string) { this.EntityPM.ParentCargoConsinmentItem = newValue; }

    public get GrossMassMeasure() { return this.EntityPM.GrossMassMeasure; }
    public set GrossMassMeasure(newValue: number) { this.EntityPM.GrossMassMeasure = newValue; }
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }

    valid: boolean = true;

    ParentCargoConsinmentItemKeyUp(event, logCellTemplate: any, parentCargoConsinmentItemTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnParentCargoConsinmentItemLostFocus(logCellTemplate, parentCargoConsinmentItemTextBox);
        }
    }

    OnParentCargoConsinmentItemLostFocus(logCellTemplate: any, parentCargoConsinmentItemTextBox: any) {
        var newValue = this.ParentCargoConsinmentItem;
        this.valid = true;
        this.UIProperties.SetValidity("ParentCargoConsinmentItem", "Customs.DecCargoSplitConsItem", true, "");

        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("ParentCargoConsinmentItem", "Customs.DecCargoSplitConsItem", true, "");
        }
        else if (newValue.toString().length > 3) {
            this.valid = false;
            this.UIProperties.SetValidity("ParentCargoConsinmentItem", "Customs.DecCargoSplitConsItem", false, "סידורי במטען אב לא יכול להיות ארוך משלושה תווים");
        }
        else {
            this.valid = true;
            this.UIProperties.SetValidity("ParentCargoConsinmentItem", "Customs.DecCargoSplitConsItem", true, "");
        }

        if (this.valid != true) {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: parentCargoConsinmentItemTextBox.InputId });

        }
        //this.ParentCargoConsinmentItem = newValue;
        //parentCargoConsinmentItemTextBox.TextValue = newValue;
    }

    GrossMassMeasureKeyUp(event, logCellTemplate: any, grossMassMeasureTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnGrossMassMeasureLostFocus(logCellTemplate, grossMassMeasureTextBox);
        }
    }

    OnGrossMassMeasureLostFocus(logCellTemplate: any, grossMassMeasureTextBox: any) {
        var newValue = this.GrossMassMeasure;
        this.valid = true;
        this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
        if (!AppTool.IsNullOrEmpty(newValue)) {
            var strValue = newValue.toString();
            if (strValue.indexOf(".") > -1) strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
            if (strValue.length > 11) {
                this.valid = false;
                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", false, "משקל לא יכול להיות ארוך מאחד עשר תווים");
            }
        }
        if (this.valid != true) {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: grossMassMeasureTextBox.InputId });

        }
    }
}
