import { ChangeDetectorRef, Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { DownloadManager } from 'Infrastructure/Utilities/DownloadManager';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { InvoiceApiStepListService } from 'Accounting/Services/StandardLists/InvoiceApiStepLogListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { InvoiceApiCommunicationLogViewModel } from './InvoiceApiCommunicationLogViewModel';
import { InvoiceApiStatusListService } from 'Accounting/Services/StandardLists/InvoiceApiStatusListService';
import { InvoiceApiCommunicationLogListService } from 'Accounting/Services/StandardLists/InvoiceApiCommunicationLogListService';
import { InvoiceApiCommunicationLogExtendedPMService } from 'Accounting/Services/ExtendedPMs/InvoiceApiCommunicationLogExtendedPMService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { InvoiceApiCommunicationLogPM } from 'Accounting/EntityPMs/InvoiceApiCommunicationLogPM';
import { InvoiceApiStepList } from 'Accounting/EntityLists/InvoiceApiStepList';
import { InvoiceApiStatusList } from 'Accounting/EntityLists/InvoiceApiStatusList';




@Component({
    selector: 'InvoiceApiCommunicationLogDetailsTabComponent',

    templateUrl: './InvoiceApiCommunicationLogDetailsTabComponent.html',
})


export class InvoiceApiCommunicationLogDetailsTabComponent extends BaseComponent {

    public EntityPM: InvoiceApiCommunicationLogPM;


    public readonly ObjectTableName: string = "InvoiceApiStep";
    public IsDisplayOnly: boolean = false;

    public invoiceApiStepListService: InvoiceApiStepListService;
    public invoiceApiStatusListService: InvoiceApiStatusListService;
    public invoiceApiCommunicationLogListService: InvoiceApiCommunicationLogListService;
    public invoiceApiCommunicationLogExtendedPMService: InvoiceApiCommunicationLogExtendedPMService;
    private readonly CurrentSession = SessionLocator.SelectedSession;

    public isRTL: boolean = false;
    InvoiceApiStepList: ObservableCollection;
    private cachedSteps: InvoiceApiStepList[] = [];
    private cachedStatuses: InvoiceApiStatusList[] = [];

    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this.invoiceApiStepListService = new InvoiceApiStepListService();
        this.invoiceApiStatusListService = new InvoiceApiStatusListService();
        this.invoiceApiCommunicationLogListService = new InvoiceApiCommunicationLogListService()
        this.invoiceApiCommunicationLogExtendedPMService = new InvoiceApiCommunicationLogExtendedPMService();
        this.EntityPM = this.entityArgs.EntityPM;
        this.InvoiceApiStepList = new ObservableCollection([]);
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");



    }
    ngOnInit() {

        this.LoadInvoiceApiSteps();

    }
    ngOnDestroy() {

    }

    ViewXMLClicked() {


        if (this.EntityPM?.DocumentId) {

            DownloadManager.DownloadPage(this.EntityPM.DocumentId);
        }



    }
    ResendClicked() {
        this.CurrentSession.StartBusyIndicator("Resending communication...");

        this.invoiceApiCommunicationLogExtendedPMService.ReSendCommunication(this.EntityPM.Id).subscribe({
            next: (response) => {
                this.CurrentSession.StopBusyIndicator();
                if (!response.HasError && response.Result) {
                    this.RefreshButtonClicked();
                    const messageWindow = new MessageWindow();
                    messageWindow.ShowSuccessIcon = true;
                    messageWindow.Show("Communication Resent Successfully");
                }
            },
            error: (err) => {
                this.CurrentSession.StopBusyIndicator();
                const messageWindow = new MessageWindow();
                messageWindow.ShowErrorIcon = true;
                messageWindow.Show("Failed to resend communication: " + err.message);
            }
        });





    }
    RefreshButtonClicked() {
        this.invoiceApiCommunicationLogListService.getSingle(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!response?.HasError) {
                this.EntityPM = response.Result;
                this.RefreshSteps();
            }
        })

    }
    RefreshSteps() {
        if (this.cachedSteps.length > 0 && this.cachedStatuses.length > 0) {

            const invoiceApiCommunicationLogViewModel = this.BuildInvoiceApiStepList(this.cachedSteps);
            this.InvoiceApiStepList.Clear();
            this.InvoiceApiStepList.InsertCollection(invoiceApiCommunicationLogViewModel);

            this.cd.detectChanges();
        }
    }


    LoadInvoiceApiSteps() {
        this.invoiceApiStatusListService.getAll().subscribe((statusRes: ServiceResponse) => {
            if (statusRes.HasError) return;
            this.cachedStatuses = statusRes.Result;

            this.invoiceApiStepListService.getAll().subscribe((stepRes: ServiceResponse) => {
                if (stepRes.HasError || !stepRes.Result) return;
                this.cachedSteps = stepRes.Result;
                const invoiceApiCommunicationLogViewModel = this.BuildInvoiceApiStepList(this.cachedSteps);
                this.InvoiceApiStepList.InsertCollection(invoiceApiCommunicationLogViewModel);
                this.cd.detectChanges();


            });
        });
    }
    private BuildInvoiceApiStepList(result: InvoiceApiStepList[]): InvoiceApiCommunicationLogViewModel[] {
        const invoiceApiCommunicationLogViewModel = [];

        result.sort((a, b) => a.Code.localeCompare(b.Code))
            .forEach((item) => {
                invoiceApiCommunicationLogViewModel.push(new InvoiceApiCommunicationLogViewModel(item, this.EntityPM, this.cachedStatuses));
            });

        return invoiceApiCommunicationLogViewModel;
    }



}



