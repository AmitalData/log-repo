import { Component, OnInit } from '@angular/core';
import { CarrierAreaPM } from '../../../../Common/EntityPMs/CarrierAreaPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AreaItemClass } from '../EditTabs/AreasTabComponent';
import { CarrierAreaPMService } from '../../../../Common/Services/StandardPMs/CarrierAreaPMService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { CarrierAreaExtendedPMService, CarrierAreaParameters} from '../../../../Common/Services/ExtendedPMs/CarrierAreaExtendedPMService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { CarrierAreasPortPM } from '../../../../Common/EntityPMs/CarrierAreasPortPM';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
declare var ResultAsArray: any;

@Component({    
    templateUrl: './AddEditAreaComponent.html',
})

export class AddEditAreaComponent extends BaseComponent implements OnInit {
    public EntityPM: CarrierAreaPM;
    public ObjectTableName: string = "CarrierArea";
    public DataContext: AreaItemClass;
    public IsNew: boolean;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private carrierAreaExtendedPMService: CarrierAreaExtendedPMService;
    constructor() {
        super();
    }

    ngOnInit() {
        if (this.DataContext != null) {
            this.DataContext.SetUIProperties();
        }
    }

    SetDataContext(dataContext: AreaItemClass) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNew = dataContext.IsNewEntity;

        this.carrierAreaExtendedPMService = new CarrierAreaExtendedPMService();
        this.Clone();
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.ValidationErrorsList);

        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name is required");
        }

        if (this.EntityPM.CarrierAreasPorts.filter(p => p.ChangeSetOp != "3")[0] == null) {
            this.ValidationErrorsList.push("At Least one port is required");
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var service: CarrierAreaPMService = new CarrierAreaPMService();

            if (this.IsNew) {
                service.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.SaveAreasCompleted(myResponse);
                });
            }

            else {
                service.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.SaveAreasCompleted(myResponse);
                });
            }
        }
    }

    private SaveAreasCompleted(myResponse: ServiceResponse) {
        if (myResponse.HasError) {
            this.CurrentSession.StopBusyIndicator();
            this.ValidationErrorsList = myResponse.ErrorsArray;
        }

        else {
            this.DataContext.fatherComponent.LoadData();
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('Name');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.EntityPM.CarrierAreasPorts);
        this.EntityPM.CarrierAreasPorts.forEach(p => {
            this.myCloner.AddEntity(p);
        });
    }
    private RejectChanges() {
        this.DataContext.ResetAreaPorts();
        this.myCloner.RejectChanges();
    }

    DownloadClicked() {
        this.carrierAreaExtendedPMService.DownloadCarrierAreaPorts(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var fileName = myResponse.Result;

                var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + fileName;
                {
                    window.open(url);
                }
            }
        });
    }

    private fileName: string;
    private fileExtension: string;
    OnFileChanged(fileEvent) {
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
        this.CurrentSession.StartBusyIndicator("Uploading...");

        this.fileName = null;
        this.fileExtension = null;

        if (!AppTool.IsNullOrEmpty(file.name)) {
            var name = file.name.split('.');
            if (name.length == 2) {
                this.fileName = name[0];
                this.fileExtension = name[1];
            }
        }
        if (file && file.size > 0) {
            var documentExtendedService = new DocumentsFilingExtendedPMService();
            documentExtendedService.GetFileSizeAndUnit(file.size).subscribe((response: ServiceResponse) => {
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

            var filter = new CarrierAreaParameters();
            filter.Tenant = context.EntityPM.Tenant;
            filter.FileData = window.btoa(binary);
            filter.CarrierAreaId = context.EntityPM.Id;
            filter.TransportMode = context.EntityPM.TransportModeCode;
            filter.FileName = context.FileName;
            filter.FileExtension = context.FileExtension;
            context.SendExcelToServer(filter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };

        reader.readAsArrayBuffer(file);
        context.EntityPM.FileUploadedName = this.fileName;
    }
    SendExcelToServer(filters: CarrierAreaParameters) {
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

        this.carrierAreaExtendedPMService.PostUploadCarrierAreaPortsExcelFile(filters).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                filters = response.Result;

                this.FillPorts(filters);

                this.CurrentSession.StopBusyIndicator();
                CachedDataManager.RefreshTableData("Port", true);
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }

            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
            }
        });
    }

    private FillPorts(filters: CarrierAreaParameters) {
        var notFoundErrorMessage: string;
        var alreadyAddedErrorMessage: string;
        var addedPortsFromExcel: number = 0;

        if (filters.ExcelPorts.length > 0) {
            filters.ExcelPorts.forEach(item => {
                if (item.HasError) {
                    if (AppTool.IsNullOrEmpty(notFoundErrorMessage)) {
                        notFoundErrorMessage = item.ExcelPortCode
                    }

                    else {
                        notFoundErrorMessage = notFoundErrorMessage + ", " + item.ExcelPortCode;
                    }
                }

                else {
                    if (this.EntityPM.CarrierAreasPorts.filter(d => d.PortId == item.PortId).length > 0) {
                        var areaPort: CarrierAreasPortPM = this.EntityPM.CarrierAreasPorts.filter(d => d.PortId == item.PortId)[0];

                        if (AppTool.IsNullOrEmpty(alreadyAddedErrorMessage)) {
                            alreadyAddedErrorMessage = areaPort.Code;
                        }

                        else {
                            alreadyAddedErrorMessage = alreadyAddedErrorMessage + ", " + areaPort.Code;
                        }
                    }

                    else {
                        addedPortsFromExcel += 1;

                        var newPort: CarrierAreasPortPM = new CarrierAreasPortPM(this.EntityPM);
                        newPort.Tenant = SessionLocator.Tenant;
                        newPort.CarrierAreaId = this.EntityPM.Id;
                        newPort.Name = item.PortName;
                        newPort.Code = item.PortCode;
                        newPort.CountryCode = item.PortCountryCode;
                        newPort.PortId = item.PortId;
                        newPort.AddedByUserId = SessionInfo.LoggedUserId;
                        newPort.AddedDate = DateTool.GetCurrentDateAsUtc();
                        this.EntityPM.AddCarrierAreasPortPM(newPort);
                        //this.BuildPortItemsList();
                    }
                }
            });

            var message: string = "Ports added: " + addedPortsFromExcel + " out of " + filters.RowsCount;
            
            if (!AppTool.IsNullOrEmpty(notFoundErrorMessage)) {
                if (AppTool.IsNullOrEmpty(message)) {
                    message = notFoundErrorMessage + " not found";
                }

                else {
                    message = message + '\n' + notFoundErrorMessage + " not found";
                }
            }

            if (!AppTool.IsNullOrEmpty(alreadyAddedErrorMessage)) {
                if (AppTool.IsNullOrEmpty(message)) {
                    message = alreadyAddedErrorMessage + " already added";
                }

                else {
                    message = message + '\n' + alreadyAddedErrorMessage + " already added";
                }
            }

            if (!AppTool.IsNullOrEmpty(message)) {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.IsMessageMultiLine = true;
                messageWindow.Show(message);
            }
        }
    }
}
