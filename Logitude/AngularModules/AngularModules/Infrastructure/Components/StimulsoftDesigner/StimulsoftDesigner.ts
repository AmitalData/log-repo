import { Component, NgZone} from '@angular/core'
import {AppTool} from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ReportService } from 'Common/Services/ExtendedLists/ReportService';
import { List } from 'Infrastructure/DataContracts/Dashboard/List';
import { MessageWindow } from 'Controls/Windows/MessageWindow';

//import { SignalRChannelService } from '../../Services/SignalRServices/SignalRChannelService';
declare var window: any;
declare var startLinking;
declare var Stimulsoft: any;

@Component({
    selector: 'DropBoxLogin',
    
    templateUrl: './StimulsoftDesigner.html',
//    template: `
 
//<iframe id="stimuldesignerframeId" name="stimuldesignerframe" [src]="URI | SafePipe" style="width:100%;height:100%"></iframe>


// `    
})

export class StimulsoftDesigner {

   //signalRChannelService: SignalRChannelService;
    public URI: string = "";
    private windowArgs: any;
    public TemplateId: string;
    public ReportTemplateId: string = "";
    public ReportsTemplateId: string = "";
    public TemplateType: string = "";
    ProcessType: string;
    private CurrentSession = SessionLocator.SelectedSession;
    reportService : ReportService = new ReportService();
    constructor(private _ngZone: NgZone) {
        window.stimulsoftDesignerComponentRef = {
            zone: this._ngZone,
            componentFn: (value) => this.stimuldesignerFinished(value),
            component: this
        };
        //this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx";
       
      //this.signalRChannelService = new SignalRChannelService();

         
    }

    stimuldesignerFinished(value) {
        // this.zone.run(() => {
        this.CurrentSession.CurrentWindow.Close(this.TemplateId);
        // });
    }

    ngOnDestroy() {
        window.stimulsoftDesignerComponentRef = null;
    }

    SetWindowArgs(args: any) {
        var sessionId: string = Guid.newGuid();
        this.windowArgs = args;
        this.TemplateId = !AppTool.IsNullOrEmpty(this.windowArgs.TemplateId) ? this.windowArgs.TemplateId:"";
        this.ReportTemplateId = !AppTool.IsNullOrEmpty(this.windowArgs.ReportTemplateId) ? this.windowArgs.ReportTemplateId : "";
        this.ReportsTemplateId = !AppTool.IsNullOrEmpty(this.windowArgs.ReportsTemplateId) ? this.windowArgs.ReportsTemplateId : "";
        this.TemplateType = !AppTool.IsNullOrEmpty(this.windowArgs.TemplateType) ? this.windowArgs.TemplateType : "";
        this.ProcessType = !AppTool.IsNullOrEmpty(this.windowArgs.ProcessType) ? this.windowArgs.ProcessType : "";

        //this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo.Token + "&tenant=" + SessionInfo.LoggedUserTenant + "&templateId=" + this.TemplateId + "&sessionId=" + sessionId + "&reportTemplateId=" + this.ReportTemplateId + "&processType=" + this.ProcessType + "&reportsTemplateId=" + this.ReportsTemplateId + "&templateType=" + this.TemplateType;


      //var observable = this.signalRChannelService.subscribeChannel("User" + SessionInfo.LoggedUserId + SessionInfo.LoggedUserTenant + sessionId).subscribe(
      //      (ev: any) => {

      //          if (ev.EventName === "StimulSaved") {
      //              observable.unsubscribe();
      //              this.CurrentSession.CurrentWindow.Close(this.TemplateId);
      //          } else if (ev.EventName === "StimulReportSaved") {
      //              observable.unsubscribe();
      //            this.CurrentSession.CurrentWindow.Close(this.ReportTemplateId);
      //            //this.signalRChannelService.unSubscribeChannel
      //          }

      //      },
      //      (error: any) => {
      //          console.warn("Attempt to join channel failed!", error);
      //      }
        //)
       // this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo.Token;

        //var WindowHeight = window.innerHeight - 100;
        //var WindowWidth = window.innerWidth - 100;

        //window.open(this.URI, 'Stimulsoft Designer', 'left=300, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=' + WindowHeight + ',height=' + WindowWidth);

        //this.windowArgs.TemplateId = item.Id;
        //this.windowArgs.Tenant = item.Entity.Tenant;

        //this.URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.html"//;?token=" + SessionInfo.Token + "&tenant=" + this.windowArgs.Tenant + "&templateId=" + this.windowArgs.TemplateId;
        
    }
    async ngAfterViewInit() {

        const options = new Stimulsoft.Designer.StiDesignerOptions();     
        options.toolbar.showSaveDialog = false;
        if (this.ProcessType === "ReportPreview" || this.TemplateType === "E") {
            options.toolbar.showSaveButton = false;
            options.toolbar.showSaveDialog = false;
            options.toolbar.showDictionary = false;
            options.toolbar.showReportTree = false;
            options.appearance.showPanel = false;
            options.toolbar.showTooltips = false;
            options.toolbar.showTooltipsHelp = false;
            options.toolbar.showFileMenu = false;
            options.toolbar.showInsertButton = false;
            options.toolbar.showLayoutButton = false;
            options.toolbar.showPreviewButton = this.TemplateType !== "E";
            options.appearance.viewStateMode = Stimulsoft.ViewStateMode.Disabled;
            options.appearance.enabled = false;
        }
        const originalDescriptor = Object.getOwnPropertyDescriptor(document, 'title');
        Object.defineProperty(document, 'title', {
            set: function(value) {},
            get: function() {
                return originalDescriptor?.get?.call(document) || "";
            },
            configurable: true
        });
    
    
        const designer = new Stimulsoft.Designer.StiDesigner(options, 'StiDesigner', false);

        const res  = await this.reportService.getReportTemplate(
            this.ProcessType,
            this.ReportTemplateId,
            this.ReportsTemplateId,
            this.TemplateType,
            this.TemplateId
        ).toPromise();
        if (res.HasError) {
            return;
        }
        const mrtText = decodeURIComponent(
          Array.prototype.map.call(
            atob(res.Result.TemplateBase64),
            (c: string) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
          ).join('')
        );
    
        const cleanMrtText =
          mrtText.charCodeAt(0) === 0xFEFF ? mrtText.slice(1) : mrtText;
        const report = new Stimulsoft.Report.StiReport();       
        report.load(cleanMrtText); 

        if(res.Result.DataSource ){
          // const ds = res.Result.DataSource;
          // const row: any = {};
          // ds.Columns.forEach((c: any) => {
          //   row[c.Name] = null;
          // });
        
          // const data = [row];
          // var dataSet = new Stimulsoft.System.Data.DataSet("JSON");
          // dataSet.readJson(data);
          // report.regData(ds.Name, ds.Name, dataSet);  
          // report.regBusinessObject("MyBO", [
          //   { Dummy: null }
          // ]);
          // report.dictionary.synchronize();

        
        }

        designer.report = report;
           

        designer.renderHtml('designerPlace');

        designer.onSaveReport = async  (e) =>{
          
          const layoutJson = e.report.saveToJsonString({
            saveDataSources: false,
            saveBusinessObjects: false,
            saveVariables: false,
            saveDictionary: false
          });
          const reportBase64 = btoa(
            unescape(encodeURIComponent(layoutJson))
          );
        
          const res = await this.reportService
            .saveReportTemplate(
              this.TemplateId,
              this.ReportTemplateId,
              this.ProcessType,
              reportBase64
            )
            .toPromise();
        
          if (res.HasError) {
            const messageWindow = new MessageWindow();
            messageWindow.ShowErrorIcon = true;
            messageWindow.Show("Failed to save report: " + res.ErrorsArray?.join(", "));
          } else {
            const messageWindow = new MessageWindow();
            messageWindow.ShowSuccessIcon = true;
            messageWindow.Show("Report saved successfully.");
          }
          
        };

    }
   
  
     addDataSourcesForPreview(report: any) {
        if (!report.dictionary.businessObjects || report.dictionary.businessObjects.count === 0) return;
      
        for (let i = 0; i < report.dictionary.businessObjects.count; i++) {
          const bo = report.dictionary.businessObjects.getByIndex(i);
      
          // Root
          bo.parentBusinessObject = null;
      
          // אם כבר קיים DataSource בשם זהה, דילוג
          if (!report.dictionary.dataSources.getByName(bo.name)) {
            const ds = new Stimulsoft.Report.Dictionary.StiBusinessObjectSource();
            ds.name = bo.name;
            ds.alias = bo.alias || bo.name;
            ds.businessObjectName = bo.name;
      
            // מוסיפים ל-DataSources
            report.dictionary.dataSources.add(ds);
      
            // מוסיפים ערכי דמה עבור Preview
            if (!bo.businessObjectValue || bo.businessObjectValue.length === 0) {
              const dummyRow: any = {};
              bo.columns.list.forEach((col: any) => {
                switch (col.type.name) {
                  case "System.String":
                    dummyRow[col.name] = col.name + "_Sample";
                    break;
                  case "System.DateTime":
                    dummyRow[col.name] = new Date();
                    break;
                  case "System.Boolean":
                    dummyRow[col.name] = true;
                    break;
                  default:
                    dummyRow[col.name] = 0;
                }
              });
              bo.businessObjectValue = [dummyRow]; // מערך עם שורה אחת של דמה
            }
          }
      
          // טיפול ב-Child BOs nested (recursion)
          if (bo.childBusinessObjects && bo.childBusinessObjects.count > 0) {
            this.addDataSourcesForPreview({dictionary: {businessObjects: bo.childBusinessObjects, dataSources: report.dictionary.dataSources}});
          }
        }
      
        console.log("DataSources prepared for Preview:", report.dictionary.dataSources.list);
      }
      
    
    onClose() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Report Designer";
            confirmWindow.Show("Do you want to close Report Designer? Changes you made may not be saved.");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {

                    if (this.TemplateId) {
                        this.CurrentSession.CurrentWindow.Close(this.TemplateId);
                    }
                    else {
                        this.CurrentSession.CurrentWindow.Close(this.ReportTemplateId);
                    }

                }
                else {

                }


            });
    }



     
    
     

}
class BusinessEntity {
    public Name: string;
    public Alias: string;

    constructor(name: string, alias: string) {
        this.Name = name;
        this.Alias = alias;
    }
}

