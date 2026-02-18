import { Component, OnInit } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { CustomsPartnerFtpPM } from '../../../Customs/EntityPMs/CustomsPartnerFtpPM';
import { CustomsPartnerFtpList } from '../../../Customs/EntityLists/CustomsPartnerFtpList';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsPartnerFtpPMService } from '../../../Customs/Services/StandardPMs/CustomsPartnerFtpPMService';
import { CustomsPartnerFtpListService } from '../../../Customs/Services/StandardLists/CustomsPartnerFtpListService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsPartnerFtpExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomsPartnerFtpExtendedPMService';
import { FTPDetailPMService } from '../../../Common/Services/StandardPMs/FTPDetailPMService';
import { FTPDetailPM } from '../../../common/EntityPMs/FTPDetailPM';
import { KeyValuePair } from '../../CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent';

@Component({    
    templateUrl: './CustomsPartnerFtpListComponent.html',
})
/// itzik:  bad pattren - Due Design paper - How to copy from  CustomsDocumentsDefinitionComponent - DING DING DING SHAME SHAME!!!
export class CustomsPartnerFtpListComponent extends BaseComponent implements OnInit {
  public IsDisplayOnly: boolean = false;

    ngOnInit(): void {
       
    }
    Search: any;
    public DataContext: CustomsPartnerFtpListComponent = this;
    public EntityPM: CustomsPartnerFtpPM = new CustomsPartnerFtpPM();
    public ObjectTableName: string = "Customs.CustomsPartnerFtp";
    private isControlEnabled: boolean = true;
    public IsLoaded: boolean = false;

    public _EntityResourceService: EntityResourceService = new EntityResourceService();

    private _CustomsPartnerFtpPMService: CustomsPartnerFtpPMService = new CustomsPartnerFtpPMService();
    private _CustomsPartnerFtpListService: CustomsPartnerFtpListService = new CustomsPartnerFtpListService();
    _CustomsPartnerFtpExtendedPMService: CustomsPartnerFtpExtendedPMService = new CustomsPartnerFtpExtendedPMService();
    public _FetchCustomsPartnerFtpResultList: ObservableCollection ;
    
    public ValidationErrorsList: string[] = [];
    _TypeCodeItems: KeyValuePair[] = [];
    _InterfaceNameItems: KeyValuePair[] = [];
    _PartnerCodeItems: KeyValuePair[] = [];
    _InterfaceDetailsItems: InterfaceDetails[];
    _InEditMode: boolean = false;

    private myFTPService: FTPDetailPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this._FetchCustomsPartnerFtpResultList = new ObservableCollection([]);
        this.myFTPService = new FTPDetailPMService();
        //this._TypeCodeItems.push({ 'Id': '', 'Name': '' });
        //this._TypeCodeItems.push({ 'Id': 'IN', 'Name': 'In' });
        //this._TypeCodeItems.push({ 'Id': 'OUT', 'Name': 'Out' });

        //this._PartnerCodeItems =//.push({ 'Id': 'Malam', 'Name': 'Malam' });
        //    [
        //        { 'Id': '', 'Name': '' },
        //        { 'Id': 'MAMAN', 'Name': 'Maman' },

        //    ];
        //this._InterfaceNameItems =
        //    [
        //    { 'Id': "", 'Name': '' },
        //    { 'Id': "SUBMANIFEST", 'Name': 'SubManifest' },
        //];


        this.CurrentSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            this._CustomsPartnerFtpExtendedPMService.GetScreenOption(SessionLocator.Tenant).subscribe((res:any) => {
                let screenOption = res.Result;
                this._PartnerCodeItems = screenOption.PartnerCodeItems;
                //this._InterfaceNameItems = screenOption.InterfaceNameItems;
                this._InterfaceNameItems = [];
                this._InterfaceNameItems.push(new KeyValuePair('',''));
                this._InterfaceDetailsItems = [];
                let listInterfaceDetailsItems: any[] = screenOption.InterfaceDetailsItems;
                listInterfaceDetailsItems.forEach(r => {


                    let val = r.Value;
                    let myInterfaceDetails: InterfaceDetails = JSON.parse(val);
                    this._InterfaceNameItems.push(new KeyValuePair(myInterfaceDetails.Code,myInterfaceDetails.Name));
                    this._InterfaceDetailsItems.push(myInterfaceDetails);
                });
                this._TypeCodeItems = screenOption.TypeCodeItems;
                this.ReLoadList();
            })
            
        });
    }

    IsRequierd() {
        if (this._CustomsPartnerFtpPM == null) { return;}
        this.UIProperties.SetRequired("InterfaceName1", this.ObjectTableName, AppTool.IsNullOrEmpty(this.InterfaceName));
        this.UIProperties.SetRequired("PartnerCode1", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PartnerCode));
        this.UIProperties.SetRequired("TypeCode1", this.ObjectTableName, AppTool.IsNullOrEmpty(this.TypeCode));
    }
    ReLoadList() {
        this.CurrentSession.StartBusyIndicator("");
        this._InEditMode = false;
        this._IsNew = false;
        this._CustomsPartnerFtpPM = null;
        this._CustomsPartnerFtpListService.getAll().subscribe((myResult:any) => {
            this.CurrentSession.StopBusyIndicator();

            console.log("Get All CustomsPartnerFtp Definition: ", myResult);
            if (myResult != null && myResult.Result != null) {
                let _mappedListsArray: Array<CustomsPartnerFtpList> = myResult.Result;
                _mappedListsArray.forEach(row => {
                    let detail = this._InterfaceDetailsItems.filter(r => r.Code == row.InterfaceName)[0];
                    row.InterfaceCodeName = detail?.Name;
                });
                
                this._FetchCustomsPartnerFtpResultList.InsertCollection(_mappedListsArray);

            }
        });
        this.IsLoaded = true;
    }

    CancelButtonClicked() {
        if (this._InEditMode) {
            this._InEditMode = false;
            this._CustomsPartnerFtpPM = null;
            return;
        }
        this.CurrentSession.CloseCurrentWindow();
    }

    

    _IsNew: boolean = false;
    OkButtonClicked() {
        if (!AppTool.IsNullOrEmpty(this.User)) {
            this.User = this.User.trim();
        }
        if (!AppTool.IsNullOrEmpty(this.Password)) {
            this.Password = this.Password.trim();
        }
        if(!AppTool.IsNullOrEmpty(this.CustomerUniqueCode)){
            this.CustomerUniqueCode = this.CustomerUniqueCode.trim();
        }
        this.ValidateCustomsPartnerFtp();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        this.ValidateWebApi();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));


        if (this._IsNew == true) {
            this._CustomsPartnerFtpPMService.insert(this._CustomsPartnerFtpPM)
                .subscribe((response:any) => {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    var res: ServiceResponse = response;
                    if (res.HasError) {
                        this.ValidationErrorsList = res.ErrorsArray;
                        
                        return;
                    } else {
                        this.ReLoadList();
                    }
            });
        }
        else {
            this._CustomsPartnerFtpPMService.update(this._CustomsPartnerFtpPM).subscribe((response:any) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var res: ServiceResponse = response;
                if (res.HasError) {
                    this.ValidationErrorsList = res.ErrorsArray;
                    
                    return;
                } else {
                    this.ReLoadList();
                }
            });
        }
        //this.CurrentSession.CurrentWindow.StopBusyIndicator();

        //console.log("..Saved Successfully ");


        ///this.CurrentSession.CloseCurrentWindow();
    }

   
    AddCustomsPartnerFtpCommand() {
        this._IsNew = true;
        this._CustomsPartnerFtpPM = new CustomsPartnerFtpPM();
        this._CustomsPartnerFtpPM.Tenant = SessionLocator.Tenant;
        this.ClearScreen()//this._SettingsHost = null;
        
        this._InEditMode = true;
      //  this._CustomsPartnerFtpResultList.Insert(new CustomsPartnerFtpVM(new CustomsPartnerFtpPM(), true));
    }
    DeleteButtonClicked(item: CustomsPartnerFtpList) {
        this._IsNew = false;
        //this._CustomsPartnerFtpResultList.Remove(item);
        //.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {

      

        this._CustomsPartnerFtpExtendedPMService.delete(item.Id).subscribe((response:any) => {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var res: ServiceResponse = response;
            if (res.HasError) {
                this.ValidationErrorsList = res.ErrorsArray;

                return;
            } else {
                this.ReLoadList();
            }
        });
        
    }
     EditButtonClicked(item: CustomsPartnerFtpList) {
        this._IsNew = false;
        
        this.ClearScreen();
        
        
        this.CurrentSession.StartBusyIndicatorLoading();
        this._CustomsPartnerFtpPMService.get(item.Id)
            .subscribe(myResponse => {
                this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    return;
                }
                    
                this._CustomsPartnerFtpPM = myResponse.Result;
                this._InEditMode = true;
                
                if (!AppTool.IsNullOrEmpty(this._CustomsPartnerFtpPM.InterfaceName)) {
                    this._InterfaceDetail = this._InterfaceDetailsItems.filter(r => r.Code == this._CustomsPartnerFtpPM.InterfaceName)[0];
                }
                 
                switch (this._InterfaceDetail.ViaMethod) {
                    case "WEBAPI":
                        {
                            this._WebApiDefinition = new WebApiDefinition();
                            if (!AppTool.IsNullOrEmpty(this._CustomsPartnerFtpPM.CommunicationDetails)) {
                                this._WebApiDefinition = JSON.parse(this._CustomsPartnerFtpPM.CommunicationDetails)
                            }
                            
                        } break;
                    case "FTP":
                        {
                            if (!AppTool.IsNullOrEmpty(this._CustomsPartnerFtpPM.FtpDetailsId)) {
                                this.LoadFTP(this._CustomsPartnerFtpPM.FtpDetailsId, this._CustomsPartnerFtpPM.TypeCode);
                            }

                        } break;
                    default:
                        break;
                }

                
                
                
            });
    }




    //#Region "EditMode"


    _CustomsPartnerFtpPM: CustomsPartnerFtpPM;
    public get Id() { return this._CustomsPartnerFtpPM.Id; }
    public set Id(newValue: string) { if (this._CustomsPartnerFtpPM.Id != newValue) { this._CustomsPartnerFtpPM.Id = newValue; } }



    public get Tenant() { return this._CustomsPartnerFtpPM.Tenant; }
    public set Tenant(newValue: number) { if (this._CustomsPartnerFtpPM.Tenant != newValue) { this._CustomsPartnerFtpPM.Tenant = newValue } }

    //_SelectedItemTypeCode: KeyValuePair;
    public get SelectedItemTypeCode(): KeyValuePair { return this._TypeCodeItems.filter(r => r.Key == this._CustomsPartnerFtpPM.TypeCode)[0]; }
    public set SelectedItemTypeCode(newValue: KeyValuePair) {
        //this._SelectedItemTypeCode = newValue;
        this.TypeCode = /*this._SelectedItemTypeCode*/newValue.Key;
    }
    public get TypeCode() { return this._CustomsPartnerFtpPM.TypeCode; }
    public set TypeCode(newValue: string) {
        if (this._CustomsPartnerFtpPM.TypeCode != newValue) {
            this._CustomsPartnerFtpPM.TypeCode = newValue; this.IsRequierd();
        }
    }


    public get SelectedItemPartnerCode(): KeyValuePair { return this._PartnerCodeItems.filter(r => r.Key == this._CustomsPartnerFtpPM.PartnerCode)[0]; }
    public set SelectedItemPartnerCode(newValue: KeyValuePair) {
        this.PartnerCode = newValue.Key;
    }

    public get PartnerCode() { return this._CustomsPartnerFtpPM.PartnerCode; }
    public set PartnerCode(newValue: string) { if (this._CustomsPartnerFtpPM.PartnerCode != newValue) { this._CustomsPartnerFtpPM.PartnerCode = newValue; this.IsRequierd();} }


    
    public get SelectedItemInterfaceName(): KeyValuePair { return this._InterfaceNameItems.filter(r => r.Key == this._CustomsPartnerFtpPM.InterfaceName)[0]; }  
    public set SelectedItemInterfaceName(newValue: KeyValuePair) {
        this.InterfaceName = newValue.Key;
    }
    public get InterfaceName() { return this._CustomsPartnerFtpPM.InterfaceName; }
    public set InterfaceName(newValue: string) {
        if (this._CustomsPartnerFtpPM.InterfaceName != newValue) {
            this._CustomsPartnerFtpPM.InterfaceName = newValue;
            this.IsRequierd();
        }
    }



    public get FtpDetailsId() { return this._CustomsPartnerFtpPM.FtpDetailsId; }
    public set FtpDetailsId(newValue: string) { if (this._CustomsPartnerFtpPM.FtpDetailsId != newValue) { this._CustomsPartnerFtpPM.FtpDetailsId = newValue; } }



    public get FileName() { return this._CustomsPartnerFtpPM.FileName; }
    public set FileName(newValue: string) { if (this._CustomsPartnerFtpPM.FileName != newValue) { this._CustomsPartnerFtpPM.FileName = newValue; } }


    
    public get FileExt() { return this._CustomsPartnerFtpPM.FileExt; }
    public set FileExt(newValue: string) { if (this._CustomsPartnerFtpPM.FileExt != newValue) { this._CustomsPartnerFtpPM.FileExt = newValue; } }


    _WebApiDefinition: WebApiDefinition;
    public get WEBAPIURL() { return this._WebApiDefinition.WEBAPIURL; }
    public set WEBAPIURL(newValue: string) { if (this._WebApiDefinition.WEBAPIURL != newValue) { this._WebApiDefinition.WEBAPIURL = newValue; } }

    
    public get WEBAPIAuthenticationURL() { return this._WebApiDefinition.WEBAPIAuthenticationURL; }
    public set WEBAPIAuthenticationURL(newValue: string) { if (this._WebApiDefinition.WEBAPIAuthenticationURL != newValue) { this._WebApiDefinition.WEBAPIAuthenticationURL= newValue; } }

    
    public get serviceURL() { return this._WebApiDefinition.serviceURL; }
    public set serviceURL(newValue: string) { if (this._WebApiDefinition.serviceURL != newValue) { this._WebApiDefinition.serviceURL= newValue; } }

    
    public get User() { return this._WebApiDefinition.User; }
    public set User(newValue: string) { if (this._WebApiDefinition.User != newValue) { this._WebApiDefinition.User = newValue; } }

    
    public get Password() { return this._WebApiDefinition.Password; }
    public set Password(newValue: string) { if (this._WebApiDefinition.Password != newValue) { this._WebApiDefinition.Password = newValue; } }

    public get CustomerUniqueCode() { return this._WebApiDefinition.CustomerUniqueCode; }
    public set CustomerUniqueCode(newValue: string) { if (this._WebApiDefinition.CustomerUniqueCode != newValue) { this._WebApiDefinition.CustomerUniqueCode = newValue; } }

    ClearScreen() {
        this.ValidationErrorsList = [];
        this._SettingsHost = null;
        this._WebApiDefinition = new WebApiDefinition();
        this.IsRequierd();
        //this.WEBAPIAuthenticationURL = this.WEBAPIURL = null;
        //this.Password = this.User = null;

    }
    TypeCodeChanged(/*selectControl: any*/TypeCode) {
        this._CustomsPartnerFtpPM.TypeCode = /*selectControl.value*/TypeCode;
        this.IsRequierd()
    }
    PartnerCodeChanged(/*selectControl: any*/PartnerCode) {
        this._CustomsPartnerFtpPM.PartnerCode = /*selectControl.value*/PartnerCode;
        this.IsRequierd()
    }
    _InterfaceDetail: InterfaceDetails;
    InterfaceNameChanged(/*selectControl: any*/ InterfaceKey) {
        this._CustomsPartnerFtpPM.InterfaceName = InterfaceKey/*selectControl.value*/;
        this.IsRequierd()
        this.SetFromServer();
    }
    SetFromServer() {
        if (!AppTool.IsNullOrEmpty(this._CustomsPartnerFtpPM.InterfaceName)) {
            this._InterfaceDetail = this._InterfaceDetailsItems.filter(r => r.Code == this._CustomsPartnerFtpPM.InterfaceName)[0];
            this.PartnerCode = this._InterfaceDetail.Partner;
            this.TypeCode = this._InterfaceDetail.TypeCode;
            if (this._InterfaceDetail.ViaMethod == "WEBAPI") {
                this._WebApiDefinition = new WebApiDefinition();
            }
        }
    }
    _SettingsHost: string;
    
    private LoadFTP(id: string, code: string) {
        this.myFTPService.get(id).subscribe((myResult:any) => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                var myEntity: FTPDetailPM = myResponse.Result;

                if (myEntity != null) {
                        this._SettingsHost = myEntity.Host;
                }
            }
        });
    }

    ValidateCustomsPartnerFtp() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.InterfaceName)) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("שם מסר הינו חובה");
            return;
        }
        if (AppTool.IsNullOrEmpty(this.TypeCode)) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("קוד סוג הינו חובה");
            return;
        }
        

        if (AppTool.IsNullOrEmpty(this.PartnerCode)) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("קוד שותף הינו חובה");
            return;
        }

        
    }
    ValidateWebApi() {
        if (this._InterfaceDetail != null && this._InterfaceDetail.ViaMethod == 'WEBAPI') {
            
            if (AppTool.IsNullOrEmpty(this.WEBAPIURL)) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push("כתובת השירות הינו חובה");
                return;
            }
            var pattern = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
            if (!pattern.test(this.WEBAPIURL)) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push("כתובת השירות אינו חוקי");
                return;

            }
            
            if (!AppTool.IsNullOrEmpty(this.WEBAPIAuthenticationURL)) {
                if (!pattern.test(this.WEBAPIAuthenticationURL)) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push("כתובת אימות השירות אינו חוקי");
                    return;
                }
            }
            if (!AppTool.IsNullOrEmpty(this.WEBAPIAuthenticationURL) ||
                !AppTool.IsNullOrEmpty(this.User) ||
                !AppTool.IsNullOrEmpty(this.Password) 
            ) {

                /// MAYBE IF ONE IS NOT NULL ALL OTHER SHOULDNT BR NULL ALSO ??!!

            }
            this._CustomsPartnerFtpPM.CommunicationDetails = JSON.stringify(this._WebApiDefinition);
        }
        
        

    }
    AddFTP() {
        this.ValidateCustomsPartnerFtp();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add FTP Detail";
        logWindow.WindowArgs = { Code: this.TypeCode, IsNew: true };
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    
                        this.FtpDetailsId = comp.EntityPM.Id;
                        this._SettingsHost = comp.EntityPM.Host;

                    
                }
            });
        });
    }
    EditFTP() {
        this.ValidateCustomsPartnerFtp();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        var settingId = null;
        settingId = this.FtpDetailsId;

        if (!AppTool.IsNullOrEmpty(settingId)) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { Code: this.TypeCode, IsNew: false, EntityId: settingId };
            logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {

                        this.FtpDetailsId = comp.EntityPM.Id;
                        this._SettingsHost = comp.EntityPM.Host;

                    }

                        
                    
                });
            });
        }
    }
    //#endEditMode





  
}

class InterfaceDetails {
    Code: string
    Name: string

    TypeCode: string
    Partner: string
    ViaMethod: string
}
class WebApiDefinition {
    WEBAPIURL: string
    WEBAPIAuthenticationURL: string
    serviceURL: string
    User: string
    Password: string
    CustomerUniqueCode :string 
}
