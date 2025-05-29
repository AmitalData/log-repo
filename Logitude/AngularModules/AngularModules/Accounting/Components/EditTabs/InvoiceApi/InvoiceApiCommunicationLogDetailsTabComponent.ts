import { ChangeDetectorRef, Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { DownloadManager } from 'Infrastructure/Utilities/DownloadManager';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { InvoiceApiStepListService } from 'Accounting/Services/StandardLists/InvoiceApiStepLogListService';
import { LogTab } from 'Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { CommunicationLogStepDataViewModel } from 'InfrastructureModules/InfrastructureCommunications/Components/CommunicationLog/ViewModel/CommunicationLogStepDataViewModel';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { InvoiceApiCommunicationLogViewModel } from './InvoiceApiCommunicationLogViewModel';
import { InvoiceApiStatusListService } from 'Accounting/Services/StandardLists/InvoiceApiStatusListService';
import { InvoiceApiCommunicationLogListService } from 'Accounting/Services/StandardLists/InvoiceApiCommunicationLogListService';
import { InvoiceApiCommunicationLogExtendedPMService } from 'Accounting/Services/ExtendedPMs/InvoiceApiCommunicationLogExtendedPMService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';




@Component({
    selector: 'InvoiceApiCommunicationLogDetailsTabComponent',

    templateUrl: './InvoiceApiCommunicationLogDetailsTabComponent.html',
})


export class InvoiceApiCommunicationLogDetailsTabComponent extends BaseComponent {

    public EntityPM: any;


    public ObjectTableName: string = "InvoiceApiStep";
    public DataContext: any = this;
    public IsDisplayOnly: boolean = false;

    public columns: any[] = null;
    public invoiceApiStepListService: InvoiceApiStepListService;
    public invoiceApiStatusListService: InvoiceApiStatusListService;
    public invoiceApiCommunicationLogListService :InvoiceApiCommunicationLogListService;
    public invoiceApiCommunicationLogExtendedPMService :InvoiceApiCommunicationLogExtendedPMService;
    private CurrentSession = SessionLocator.SelectedSession;

    public isRTL: boolean = false;
    InvoiceApiStepList: ObservableCollection;
    private cachedSteps: any[] = []; 
    private cachedStatuses: any[] = []; 

    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this.invoiceApiStepListService = new InvoiceApiStepListService();
        this.invoiceApiStatusListService = new InvoiceApiStatusListService();
        this.invoiceApiCommunicationLogListService =new InvoiceApiCommunicationLogListService()
        this.invoiceApiCommunicationLogExtendedPMService = new InvoiceApiCommunicationLogExtendedPMService();
        this.EntityPM = this.entityArgs.EntityPM;
        this.InvoiceApiStepList = new ObservableCollection([]);
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        
        

    }
    ngOnInit() {

        this.LoadInvocieApiSteps();

    }


    ViewXMLClicked() {


        if (this.EntityPM.CommunicationId) {

            DownloadManager.DownloadPage("1-1000249587");
        }
       


    }
    ResendClicked() {
        this.CurrentSession.StartBusyIndicator("Resending communication...");

        this.invoiceApiCommunicationLogExtendedPMService.ReSendCommunication(this.EntityPM.Id).subscribe((response: any) => {
            this.CurrentSession.StopBusyIndicator();

            if (!response.HasError && response.Result) {
                this.RefreshButtonClicked();
                const messageWindow=new MessageWindow
                messageWindow.ShowSuccessIcon = true;
                messageWindow.Show("Communication Resent Successfully")
            }
            

        })



    }
    RefreshButtonClicked() {
        this.invoiceApiCommunicationLogListService.getSingle(this.EntityPM.Id).subscribe((response: any) => {
            const serviceResponse: ServiceResponse = response;
            if (!serviceResponse.HasError) {
                this.EntityPM = serviceResponse.Result;
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
    // LoadInvocieApiSteps() {
        
    //         this.invoiceApiStepListService.getAll().subscribe((invoiceApiStepRes: any) => {

    //             var pmResponse: ServiceResponse = invoiceApiStepRes;
    //             if (!pmResponse.HasError) {
    //                 var result = pmResponse.Result;
    //                 if (result) {
    //                     var invoiceApiCommunicationLogViewModel = [];

    //                     result.sort((a, b) => a.Code.localeCompare(b.Code))
    //                         .forEach((item) => {
    //                             invoiceApiCommunicationLogViewModel.push(new InvoiceApiCommunicationLogViewModel(item, this.EntityPM,result));
    //                         });
    //                     this.InvoiceApiStepList.InsertCollection(invoiceApiCommunicationLogViewModel);

    //                     this.cd.detectChanges();


    //                 }
    //             }


    //         });
        
    // }


    LoadInvocieApiSteps() {
        this.invoiceApiStatusListService.getAll().subscribe((statusRes: any) => {
            const statusResponse: ServiceResponse = statusRes;
            if (!statusResponse.HasError) {
                this.cachedStatuses = statusResponse.Result;   
            }

            this.invoiceApiStepListService.getAll().subscribe((invoiceApiStepRes: any) => {
                const pmResponse: ServiceResponse = invoiceApiStepRes;
                if (!pmResponse.HasError) {
                    const result = pmResponse.Result;
                    if (result) {
                        this.cachedSteps = result;
                        
                        const invoiceApiCommunicationLogViewModel = this.BuildInvoiceApiStepList(result);
                        this.InvoiceApiStepList.InsertCollection(invoiceApiCommunicationLogViewModel);
                        this.cd.detectChanges();
                    }
                }
            });
        });
    }
    private BuildInvoiceApiStepList(result: any[]): InvoiceApiCommunicationLogViewModel[] {
        const invoiceApiCommunicationLogViewModel = [];

        result.sort((a, b) => a.Code.localeCompare(b.Code))
            .forEach((item) => {
                invoiceApiCommunicationLogViewModel.push(new InvoiceApiCommunicationLogViewModel(item, this.EntityPM,this.cachedStatuses));
            });

        return invoiceApiCommunicationLogViewModel;
    }



}



