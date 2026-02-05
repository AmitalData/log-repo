import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ARInvoicePM} from '../../EntityPMs/ARInvoicePM';
import {ARInvoicePMService} from '../../Services/StandardPMs/ARInvoicePMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { ListComponentArgs } from 'Infrastructure/Args';

@Component({
    
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent extends BaseComponent {
    public Entity: any;
    public FieldName: string;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public IsHeaderScreenTemplate: boolean = false;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public DataContext: FieldTemplateComponent = this;
    public DisplaySATFields: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private ListComponentArgs: ListComponentArgs;

    public NumberFieldRightPadding = "20px";
    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATFields = true;
        }
        this.ListComponentArgs = SessionLocator.SelectedSession?.CurrentListComponent._ListComponentArgs;

    }

    public IsUnpaidInvoice: boolean = false;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorMessage: string;
    private myService: ARInvoicePMService;
    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];

        if (this.Entity != null) {
            if (this.FieldName != null) {
                this.FieldValue = this.Entity[this.FieldName];
            }

            if (this.IsSpotLightTemplate) {
                if (this.ObjectTableName == "ARInvoice") {
                    this.myService = new ARInvoicePMService();

                    if (this.Entity.StatusCode != "DR" && this.Entity.StatusCode != "VD" && !this.Entity.IsClosed) {
                        this.IsUnpaidInvoice = true;
                    }                    

                    this.ShowBusyIndicator = true;
                    this.BusyIndicatorMessage = "Loading...";
                    this.LoadARInvoicePM();
                }
            }
        }
    }

    //AR Invoice
    public IsEntityLoaded: boolean = false;
    public MainEntityStatus: string;
    public EntityPM: ARInvoicePM;
    private LoadARInvoicePM() {
        this.myService.get(this.Entity.Id).subscribe((myResponse:any) => {
            if (!myResponse.HasError) {
                this.ShowBusyIndicator = false;
                this.EntityPM = myResponse.Result;
                if (this.EntityPM != null) {
                    this.IsEntityLoaded = true;
                    this.MainEntityStatus = this.EntityPM.MainEntityStatus;
                    this.ExpectedPaymentDate = DateTool.GetDateParts(this.EntityPM.ExpectedPaymentDate).DateObject;
                }
            }

            else {
                this.ShowBusyIndicator = false;
            }

        }
            , error => {
                this.ShowBusyIndicator = false;
            });
    }

    get InternalNotes() { return this.EntityPM == null ? null : this.EntityPM.InternalNotes; }
    set InternalNotes(newValue: string) {
        if (this.EntityPM.InternalNotes != newValue) {
            this.EntityPM.InternalNotes = newValue;
        }
    }

    get ExpectedPaymentDate() { return this.EntityPM == null ? null : this.EntityPM.ExpectedPaymentDate; }
    set ExpectedPaymentDate(newValue: Date) {
        if (this.EntityPM.ExpectedPaymentDate != newValue) {
            this.EntityPM.ExpectedPaymentDate = newValue;
        }
    }

    ViewEntityClicked(entityType: string) {
        if (this.Entity != null) {
            var tableName: string;
            var entityId: string;

            if (entityType == "SHI") {
                tableName = "Shipment";
                entityId = this.Entity.MainEntityId;
            }

            else if (entityType == "INV") {
                tableName = "ARInvoice";
                entityId = this.Entity.Id;
            }

            else if (entityType == "PAY") {
                tableName = "ARPayment";
                entityId = this.arPaymentId;
            }
            else if(entityType == "GLAC") {
                tableName = "GLAccount";
                entityId = this.Entity.GLAccountId;
                this.ListComponentArgs.SuppressOnRowSelectedField = true;
            }
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, });
                        
                    let isEditComponentSaved = false;
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        }
                    });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });
                });
        }
    }

    SaveInvoiceChanges() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length == 0) {
            this.ShowBusyIndicator = true;
            this.BusyIndicatorMessage = "Saving...";

            this.myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                }

                this.ShowBusyIndicator = false;
            });
        }
    }

    private arPaymentId: string;
    NewPaymentClicked() {
        if (FeatureLocator.HasFeaturePermession("APPayment", "NEW")) {
            if (this.EntityPM.StatusCode == "DR") {
                var messageText = "Cant add payment for Draft invoice";

                var window: MessageWindow = new MessageWindow();
                window.Width = 300;
                window.Height = 150;
                window.Show(messageText);
            }
            else if (this.EntityPM.StatusCode === "PR") {
                const messageText = "Cant add payment for processing invoice";

                let window: MessageWindow = new MessageWindow();
                window.Width = 300;
                window.Height = 150;
                window.Show(messageText);
            }
            else if (this.EntityPM.AmountDue <= 0) {
                var messageText = "Amount paid equals or bigger than invoice amount";

                var window: MessageWindow = new MessageWindow();
                window.Width = 400;
                window.Height = 150;
                window.Show(messageText);
            }

            else {
                var logWindow = new LogitudeWindow();
                logWindow.WindowArgs = { ARInvoice: this.EntityPM};
                logWindow.Title = "Create new Payment";
                logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
                logWindow.ComponentLoaded.subscribe(comp => {
                    logWindow.WindowClosed.subscribe(s => {
                        if (s) {
                            this.arPaymentId = comp.newARPaymentPM.Id;
                            this.ViewEntityClicked("PAY");
                        }
                    });
                });
            }
        }
    }

    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)){
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }
    OpenInterestReport(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'InterestReport' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }
}
