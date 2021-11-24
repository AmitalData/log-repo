import { Component, Output, EventEmitter, AfterViewInit} from '@angular/core';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { InterestLastBatchServicePM } from 'Accounting/EntityPMs/InterestLastBatchServicePM';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { InterestReportExtendedListService } from 'Accounting/Services/ExtendedLists/InterestReportExtendedListService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { BatchTaskExecutionList } from 'Infrastructure/EntityLists/BatchTaskExecutionList';
import { BatchTaskExecutionListService } from 'Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';

declare var window: any;
@Component({
    
    templateUrl: './InterestPageComponent.html',
    providers: [EntityPMService],

})

export class InterestPageComponent implements AfterViewInit {

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() ReloadUserQueries = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    public isRTL: boolean = false;
    private interestReportExtendedListService: InterestReportExtendedListService = new InterestReportExtendedListService();
    public _BatchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    public ObjectTableName = "InterestBasesType";
    public bteList: BatchTaskExecutionList;
    constructor() {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    ngAfterViewInit() {
        this.LoadAllScreenData();
    }
    public LoadAllScreenData() {

        this.ReloadUsersQuery();
    }
    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    InitComponent() {
        this.LoadAllScreenData();

    }

    RunNewInterestBasesTypeWizard() {
        var windowArgs: any = {};
        windowArgs.IsNew = true;
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewInterestBases");
        var logWindow = new LogitudeWindow();
        logWindow.Width = 680;
        logWindow.Height = 400;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/EditTabs/Interest/DetailsTab/InterestBasesTypeDetailsTabComponent');
    }

    RunNewInterestReportWizard() {
        var windowArgs: any = {};
        windowArgs.IsNew = true;
        var windowTitle = TextCodeTranslator.Translate("InterestReport");
        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewInterestReportComponent');
    }
    RunBatchInvoicesWizard() {
       this.CurrentSession.StartBusyIndicatorLoading();
            this.interestReportExtendedListService.GetInterestLastBatchServiceByTenant().subscribe((response: ServiceResponse) => {
              this.CurrentSession.StopBusyIndicator();
                var mm: ServiceResponse = response;
                if (!mm.HasError) {
                //  var InterestLastBatchService:InterestLastBatchServicePM  = mm.Result;
                //  if(!InterestLastBatchService || (InterestLastBatchService  && !InterestLastBatchService.CreateInvoicesBatchId)){
                   this.OpenBatchInvoice();
                //  }
                //  else if(InterestLastBatchService && InterestLastBatchService.CreateInvoicesBatchId){
                //   this.CheckBatchTaskExcecutingAndRunBatchInvoicesWizard(InterestLastBatchService.CreateInvoicesBatchId);
                //  }
                }
                else {
                    if(mm.ErrorsArray){
                        var msg = new MessageWindow();
                        msg.RTL = this.isRTL;
                        msg.Width = 400;
                        msg.Show(mm.ErrorsArray[0]);
                    }
                 
                }
        
              });

    }
OpenBatchInvoice(){
    this._entityResourceService.getEntityResourceByTableName("InterestReport", 0).subscribe((response: any) => {

        var windowArgs: any = {};
        windowArgs.IsNew = true;
        var windowTitle = TextCodeTranslator.Translate("InterestReport.O.BatchInvoice");
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1200;
        logWindow.Height = 1000;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        //logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/Others/BatchInvoicesComponent');
    });
}
RunBatchPrintWizard(){
    this._entityResourceService.getEntityResourceByTableName("ARInvoice", 0).subscribe((response: any) => {

        var windowArgs: any = {};
        windowArgs.IsNew = true;
        var windowTitle = TextCodeTranslator.Translate("InterestReport.O.BatchPrint");
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1250;
        logWindow.Height = 1000;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./Accounting/Components/Others/BatchPrintComponent');
    });

}
CheckBatchTaskExcecutingAndRunBatchInvoicesWizard( BatchId:string) {
        this._BatchTaskExecutionListService.getSingle( BatchId).subscribe((myResult:any) => {
            console.log("[_BatchTaskExecutionListService.getSingle]", myResult);
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.bteList = mm.Result;
                if (this.bteList.StatusCode == "D" || this.bteList.StatusCode == "F") // D- Done
                {  
                    this.OpenBatchInvoice();
                }
                else{
                var msg = new MessageWindow();
                msg.RTL = this.isRTL;
                msg.Width = 400;
                msg.Show(TextCodeTranslator.Translate("InterestReport.O.AnotherBatchInvoiceStillInProgress"));
 
                }
       
            }
            else {
            }
        });
      
      
  }
        ViewAccountingQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";

            var filters = new ApiQueryFilters();

            var tableName = "";
            var listArgs = new ListComponentArgs();

            switch (myQueryCode) {
                case "Interest Bases":
                    {
                        displayTitle = TextCodeTranslator.Translate("InterestBasesType.Q.InterestBases");
                        tableName = "InterestBasesType";
                     
                        break;
                    }
                case "Interest Report":
                    {
                        displayTitle = TextCodeTranslator.Translate("InterestReport.Q.InterestReport");
                        tableName = "InterestReport";

                        break;
                    }
                 default: { break; }
            }

            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = tableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Interest");
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response: any) => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }

    OpenBatchReportWindow(){
        this._entityResourceService.getEntityResourceByTableName("InterestReport", 0).subscribe((response: any) => {
        var windowArgs: any = {};
        windowArgs.IsNew = true;
        var windowTitle = TextCodeTranslator.Translate("InterestReport.O.BatchReport");
        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        //logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/Others/CreateInterestReportsForCustomersComponent');
        });
    }

}
