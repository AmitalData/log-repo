import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { BaseComponent } from  '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from  '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from  '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { MorningMessageRequestParams } from  '../../../Customs/DataContract/RequestParams/MorningMessageRequestParams';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { MorningMessageResponseData, MorningMessageResult } from '../../../Customs/DataContract/ResponseData/MorningMessageResponseData';
import { Validator } from                           '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomMessageWrapperComponent } from       '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent';
import { CustomMessageProgressComponent } from      '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';


import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
@Component({
    
    selector: 'MorningMessageComponent',
    
    templateUrl: './MorningMessageComponent.html',
})
     

export class MorningMessageComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent, OnInit { 
    public DataContext: MorningMessageComponent = this;
    public ObjectTableName: string = "Customs.Declaration";


    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    _LastFetchDeclarationList: DeclarationList;
    public MorningMessageObservableList: ObservableCollection;
    ///public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.MorningMessageObservableList = new ObservableCollection([]);       
    }

    ngOnInit() {
        super.ngOnInit();
    }

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new MorningMessageRequestParams();
        }
        
        
                
        if (this.ResponseData && this.ResponseData.MorningMessageList) {
            var myArr = this.ResponseData.MorningMessageList;
            var fast = true;
            if (!fast) {
                this.ResponseData.MorningMessageList.forEach((itemMess) => {
                    this.MorningMessageObservableList.Insert(itemMess);
                });
            } else {

                this.MorningMessageObservableList.InsertCollection(myArr);
            }
        }

        
    }
    
    OnRowLoaded(Row: any) {
        var isExpandaple = false;
        if (Row) {
            var item: MorningMessageResult = Row.rowData;
            if (item.Content) {
                if (item.Content.split('\n').length > 1) {
                    item.NeedExpandaple = isExpandaple = true;
                }
            }

            ///Row.SetExpandaple(isExpandaple);
        }

    }
    ShowMore(itemContent, itemSubject) {
        //alert(itemContent);
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = itemSubject;//TextCodeTranslator.Translate("CommunicationLogSteps.O.Log");
        logitudeWindow.WindowArgs = itemContent;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/LogFieldComponent');

        //var messageWindow = new MessageWindow();
        //messageWindow.Width = 800;
        //messageWindow.Height = 300;
        //messageWindow.Title = itemSubject;
        //messageWindow.Show(itemContent);
    }
    get FromDate() { return this.RequestParams.FromDate; }
    set FromDate(value: Date) {
        if (this.RequestParams.FromDate != value) {
            this.RequestParams.FromDate = value;
        }
    }

    get ToDate() { return this.RequestParams.ToDate; }
    set ToDate(value: Date) {
        if (this.RequestParams.ToDate != value) {
            this.RequestParams.ToDate = value;
        }
    }

    get SubjectText() { return this.RequestParams.SubjectText; }
    set SubjectText(value: string) {
        if (this.RequestParams.SubjectText != value) {
            this.RequestParams.SubjectText = value;
        }
    }

    get ContentText() { return this.RequestParams.ContentText; }
    set ContentText(value: string) {
        if (this.RequestParams.ContentText != value) {
            this.RequestParams.ContentText = value;
        }
    }

    get Category() { return this.RequestParams.Category; }
    set Category(value: string) {
        if (this.RequestParams.Category != value) {
            this.RequestParams.Category = value;
        }
    }







    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        ///alert(customSendOptionsArgs.Option);
     
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        errors.forEach((err) => { this.ValidationErrorsList.push(err); });

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        
        if (this.MorningMessageObservableList.Length > 0) {
            this.MorningMessageObservableList.Clear();    
        }
        


        var currRequestParams = new MorningMessageRequestParams();///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.FromDate = this.FromDate;
        currRequestParams.ToDate = this.ToDate;
        currRequestParams.Category = this.Category;
        currRequestParams.ContentText = this.ContentText;
        currRequestParams.SubjectText = this.SubjectText;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;



        

        
        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא להודעות בוקר", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._IIGGeneralMessagesService.PostMorningMessages(currRequestParams)
            //.subscribe((myServiceResponse: ServiceResponse) => {
            //this.CurrentSession.StopBusyIndicator();
            //console.log(myServiceResponse);
            //this.ResponseData = myServiceResponse.Result;
            //this.OnMassageDisplayMethod();
            .subscribe(() => { }
            )
            ;
    }



}

class MyMorningMessageResult extends MorningMessageResult {
    public Toggle: boolean = false;
    public Indicator: string = "-";
    
    public ToggleIt() {
        this.Toggle = !this.Toggle;
        if (this.Toggle) {
            this.Indicator = "+++";
        } else {
            this.Indicator = "---";
        }
    }
}
