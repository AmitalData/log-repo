import {Component, ViewChildren, QueryList,OnInit, OnDestroy} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { TariffDomainService, TariffSummery, TariffFilterParameter} from '../../Services/TariffDomainService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { BatchTaskExecutionPM } from '../../../Infrastructure/EntityPMs/BatchTaskExecutionPM';
import { BatchTaskExecutionListService } from '../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from '../../../Infrastructure/EntityLists/BatchTaskExecutionList';
import { AppTool } from '../../../Infrastructure/Tools';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';

declare var ResultAsArray: any;

@Component({
    selector: 'TariffModuleWorkspaceComponent',
    moduleId: module.id,
    templateUrl: './TariffModuleWorkspaceComponent.html',
    providers: [EntityResourceService, TariffDomainService],
})

export class TariffModuleWorkspaceComponent implements OnInit, OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    private DocumentExtendedService: DocumentsFilingExtendedPMService;

    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService, private tariffDomainService: TariffDomainService) {
        this.RunComponent();
    }
    public AirFreightCount: string;
    public AirSurchargeCount: string;

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    ngOnInit() {
        if (this.CurrentSession == null)
            this.CurrentSession = SessionLocator.SelectedSession;
        this.InitComponent();
    }
    ngOnDestroy() {
        this.StopTimer();
    }

    LoadAllScreenData() {
        this.LoadQueriesCounts();
    }
    LoadQueriesCounts() {
        this.tariffDomainService.GetTariffsCounts().subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult: TariffSummery = myResponse.Result;

                        if (myResult != null) {
                            this.AirFreightCount = myResult.AirFreightCount > 1000 ? "1000+" : myResult.AirFreightCount.toString();
                            this.AirSurchargeCount = myResult.AirSurchargeCount > 1000 ? "1000+" : myResult.AirSurchargeCount.toString();

                             }
                    }
                }
            });
        
    }
    CheckAirfreightCost() {
        this._entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
            var logWindow = new LogitudeWindow();
            logWindow.IsFillScreenHeight = true;
            logWindow.Width = 900;
            logWindow.Title = "Price Check";
            logWindow.Show("./TariffModule/Components/Workspaces/TariffSearchAirFreightPricesComponent");
        });      
    }

    public NewTariff(code: string) {
        switch (code) {
            case "A": {
                this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe(response => {

                    var windowTitle = "New Air Freight Cost";

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 850;
                    logWindow.Height = 500;
                    logWindow.Title = windowTitle;
                    logWindow.WindowClosed.subscribe(($event: any) => {
                        this.LoadQueriesCounts();
                    });
                    logWindow.ComponentLoaded.subscribe(comp => {
                        comp.SetWindowArgs({ TypeCode: "AFC"});
                    });
                        logWindow.Show('./TariffModule/Components/NewEntity/NewAirFreightCostComponent');
                });
                break;
            }

            case "AS": {
                this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe(response => {

                    var windowTitle = "New Air Surcharges Cost";

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 850;
                    logWindow.Height = 500;
                    logWindow.Title = windowTitle;
                    logWindow.WindowClosed.subscribe(($event: any) => {
                        this.LoadQueriesCounts();
                    });

                    logWindow.ComponentLoaded.subscribe(comp => {
                        comp.SetWindowArgs({ TypeCode: "ASC" });
                    });

                    logWindow.Show('./TariffModule/Components/NewEntity/NewAirFreightCostComponent');
                });
                break;
            }

            default: {
                break;
            }
        }       
    }
    public ViewTariffs(code: string) {

        switch (code) {
            case "A": {
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = "Air Freight Cost Tariffs";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Air Freight Cost Tariffs";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
                break;
            }

            case  "AS":{
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = "Air Surcharges Cost Tariffs";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Air Surcharges Cost";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
                break;
            }
            default: {
                break;
            }

        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private InitComponent() {
        this.DocumentExtendedService = new DocumentsFilingExtendedPMService();
        this.LoadAllScreenData();
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
        }
    }
    
    TariffSettingsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = "Tariff Settings";
        logWindow.Show('./TariffModule/Components/Workspaces/TariffSettingComponent');
    }

    private timer: any;
    private timerInterval: number = 5000;
    private IsLoading: boolean = false;
    private batchEntity: BatchTaskExecutionPM;
    GenerateTariffsClicked() {
        this.CurrentSession.StartBusyIndicator("Generating...");

        this.tariffDomainService.GenerateTariffs().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                this.batchEntity = myResponse.Result;

                if (this.batchEntity != null) {
                    this.timer = setInterval(() => { this.GetBTE(); }, this.timerInterval);
                }
            }

            else {
                this.CurrentSession.StopBusyIndicator();
                var window = new MessageWindow();
                window.Show(myResponse.ErrorsArray[0]);
            }
        });
    }

    // Generate from Excel 
    private FileName: string;
    GenerateExcelTariffsClicked(fileEvent) {
        var file = fileEvent.target.files[0];

        if (file) {
            var extension: string = file.name.split('.')[1];

            if (extension.includes("xls")) {
                var file = fileEvent.target.files[0];
                this.UploadExcel(file);
            }

            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("You have to upload excel files only");
            }
        } 
    }
    UploadExcel(file: any) {
        this.FileName = null;
        if (!AppTool.IsNullOrEmpty(file.name)) {
            var name = file.name.split('.');
            if (name.length == 2) {
                this.FileName = name[0];
            }
        }
        if (file && file.size > 0) {
            this.DocumentExtendedService.GetFileSizeAndUnit(file.size).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        this.StartUploadingExcelFile(file);
                    }
                }
            });
        }
    }
    StartUploadingExcelFile(file: any) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    }
    ConvertArrayBufferToBase64(file: any, context: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            var filter = new TariffFilterParameter();
            filter.FileData = window.btoa(binary);
            filter.FileName = context.FileName;
            context.SendExcelToServer(filter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
    SendExcelToServer(filter: any) {
        this.CurrentSession.StartBusyIndicator("Generating...");
        this.tariffDomainService.GenerateTariffsFromExcel(filter).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                //this.batchEntity = response.Result;

                //if (this.batchEntity != null) {
                //    this.timer = setInterval(() => { this.GetBTE(); }, this.timerInterval);
                //}
                this.CurrentSession.StopBusyIndicator();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                var window = new MessageWindow();
                window.Show(response.ErrorsArray[0]);
            }
        });
    }

    GetBTE() {
        if (!this.IsLoading) {
            this.IsLoading = true;

            var bteList: BatchTaskExecutionList;
            var myService: BatchTaskExecutionListService = new BatchTaskExecutionListService();

            myService.getSingle(this.batchEntity.Id).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    bteList = myResponse.Result;

                    if (bteList.StatusCode == "D") {
                        this.LoadQueriesCounts();
                        this.CurrentSession.StopBusyIndicator();
                        this.StopTimer();
                    }

                    else if (bteList.StatusCode == "F") {
                        this.CurrentSession.StopBusyIndicator();
                        this.StopTimer();                       
                    }
                }

                else {
                    this.CurrentSession.StopBusyIndicator();
                    this.StopTimer();

                    var window = new MessageWindow();
                    window.Show(myResponse.ErrorsArray[0]);

                }

                this.IsLoading = false;
            });
        }
    }

    StopTimer() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    }
}

