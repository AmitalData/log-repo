

import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { CustomsPartnerFtpPM } from '../../../Customs/EntityPMs/CustomsPartnerFtpPM';
import { CustomsPartnerFtpList } from '../../../Customs/EntityLists/CustomsPartnerFtpList';

import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

import { CustomsPartnerFtpPMService } from '../../../Customs/Services/StandardPMs/CustomsPartnerFtpPMService';
import { CustomsPartnerFtpListService } from '../../../Customs/Services/StandardLists/CustomsPartnerFtpListService';

import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsPartnerFtpExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomsPartnerFtpExtendedPMService';
import { RegionList } from '../../../Common/EntityLists/RegionList';
import { FTPDetailPMService } from '../../../Common/Services/StandardPMs/FTPDetailPMService';
import { FTPDetailPM } from '../../../common/EntityPMs/FTPDetailPM';
import { retry } from 'rxjs/operators';

@Component({
    moduleId: module.id,
    templateUrl: './CustomsPartnerFtpListComponent.html',
})
/// itzik:  bad pattren - Due Design paper - How to copy from  CustomsDocumentsDefinitionComponent - DING DING DING SHAME SHAME!!!
export class CustomsPartnerFtpListComponent
    extends BaseComponent
    implements OnInit {
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
    _TypeCodeItems: any[] = [];
    _InterfaceNameItems: any[] = [];
    _PartnerCodeItems: any[] = [];

    _InEditMode: boolean = false;

    private myFTPService: FTPDetailPMService;
    
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


        SessionLocator.CurrentSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this._CustomsPartnerFtpExtendedPMService.GetScreenOption(SessionLocator.Tenant).subscribe(res => {
                let screenOption = res.Result;
                this._PartnerCodeItems = screenOption.PartnerCodeItems;
                this._InterfaceNameItems = screenOption.InterfaceNameItems;
                this._TypeCodeItems = screenOption.TypeCodeItems;
                this.ReLoadList();
            })
            
        });
    }
    ReLoadList() {
        SessionLocator.CurrentSession.StartBusyIndicator("");
        this._InEditMode = false;
        this._IsNew = false;
        this._CustomsPartnerFtpPM = null;
        this._CustomsPartnerFtpListService.getAll().subscribe(myResult => {
            SessionLocator.CurrentSession.StopBusyIndicator();

            console.log("Get All CustomsPartnerFtp Definition: ", myResult);
            if (myResult != null && myResult.Result != null) {
                let _mappedListsArray: Array<CustomsPartnerFtpList> = myResult.Result;

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
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    

    _IsNew: boolean = false;
    OkButtonClicked() {

        this.ValidateCustomsPartnerFtp();
        if (this.ValidationErrorsList != null && this.ValidationErrorsList.length > 0) {
            return;
        }
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));


        if (this._IsNew == true) {
            this._CustomsPartnerFtpPMService.insert(this._CustomsPartnerFtpPM)
                .subscribe(response => {
                    SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
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
            this._CustomsPartnerFtpPMService.update(this._CustomsPartnerFtpPM).subscribe(response => {
                SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                var res: ServiceResponse = response;
                if (res.HasError) {
                    this.ValidationErrorsList = res.ErrorsArray;
                    
                    return;
                } else {
                    this.ReLoadList();
                }
            });
        }
        //SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();

        //console.log("..Saved Successfully ");


        ///SessionLocator.CurrentSession.CloseCurrentWindow();
    }

   
    private AddCustomsPartnerFtpCommand() {
        this._IsNew = true;
        this._CustomsPartnerFtpPM = new CustomsPartnerFtpPM();
        this._CustomsPartnerFtpPM.Tenant = SessionLocator.Tenant;
        this._SettingsHost = null;
        
        this._InEditMode = true;
      //  this._CustomsPartnerFtpResultList.Insert(new CustomsPartnerFtpVM(new CustomsPartnerFtpPM(), true));
    }
    private DeleteButtonClicked(item: CustomsPartnerFtpList) {
        this._IsNew = false;
        //this._CustomsPartnerFtpResultList.Remove(item);
        //.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {

      

        this._CustomsPartnerFtpExtendedPMService.delete(item.Id).subscribe(response => {
            SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            var res: ServiceResponse = response;
            if (res.HasError) {
                this.ValidationErrorsList = res.ErrorsArray;

                return;
            } else {
                this.ReLoadList();
            }
        });
        
    }
    private EditButtonClicked(item: CustomsPartnerFtpList) {
        this._IsNew = false;
        this._SettingsHost = null;
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();
        this._CustomsPartnerFtpPMService.get(item.Id)
            .subscribe(myResponse => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    return;
                }
                    
                this._CustomsPartnerFtpPM = myResponse.Result;
                this._InEditMode = true;
                if (AppTool.IsNullOrEmpty(this._CustomsPartnerFtpPM.FtpDetailsId)) {
                } else {
                    this.LoadFTP(this._CustomsPartnerFtpPM.FtpDetailsId, this._CustomsPartnerFtpPM.TypeCode);
                }
                
            });
    }




    //#Region "EditMode"


    _CustomsPartnerFtpPM: CustomsPartnerFtpPM;
    public get Id() { return this._CustomsPartnerFtpPM.Id; }
    public set Id(newValue: string) { if (this._CustomsPartnerFtpPM.Id != newValue) { this._CustomsPartnerFtpPM.Id = newValue; } }



    public get Tenant() { return this._CustomsPartnerFtpPM.Tenant; }
    public set Tenant(newValue: number) { if (this._CustomsPartnerFtpPM.Tenant != newValue) { this._CustomsPartnerFtpPM.Tenant = newValue } }



    public get TypeCode() { return this._CustomsPartnerFtpPM.TypeCode; }
    public set TypeCode(newValue: string) { if (this._CustomsPartnerFtpPM.TypeCode != newValue) { this._CustomsPartnerFtpPM.TypeCode = newValue; } }



    public get PartnerCode() { return this._CustomsPartnerFtpPM.PartnerCode; }
    public set PartnerCode(newValue: string) { if (this._CustomsPartnerFtpPM.PartnerCode != newValue) { this._CustomsPartnerFtpPM.PartnerCode = newValue; } }



    public get InterfaceName() { return this._CustomsPartnerFtpPM.InterfaceName; }
    public set InterfaceName(newValue: string) { if (this._CustomsPartnerFtpPM.InterfaceName != newValue) { this._CustomsPartnerFtpPM.InterfaceName = newValue; } }



    public get FtpDetailsId() { return this._CustomsPartnerFtpPM.FtpDetailsId; }
    public set FtpDetailsId(newValue: string) { if (this._CustomsPartnerFtpPM.FtpDetailsId != newValue) { this._CustomsPartnerFtpPM.FtpDetailsId = newValue; } }



    public get FileName() { return this._CustomsPartnerFtpPM.FileName; }
    public set FileName(newValue: string) { if (this._CustomsPartnerFtpPM.FileName != newValue) { this._CustomsPartnerFtpPM.FileName = newValue; } }



    public get FileExt() { return this._CustomsPartnerFtpPM.FileExt; }
    public set FileExt(newValue: string) { if (this._CustomsPartnerFtpPM.FileExt != newValue) { this._CustomsPartnerFtpPM.FileExt = newValue; } }

    TypeCodeChanged(selectControl: any) {
        this._CustomsPartnerFtpPM.TypeCode = selectControl.value;
    }
    PartnerCodeChanged(selectControl: any) {
        this._CustomsPartnerFtpPM.PartnerCode = selectControl.value;
    }
    InterfaceNameChanged(selectControl: any) {
        this._CustomsPartnerFtpPM.InterfaceName = selectControl.value;
    }
    _SettingsHost: string;
    
    private LoadFTP(id: string, code: string) {
        this.myFTPService.get(id).subscribe(myResult => {
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
            this.ValidationErrorsList.push("שם מסר היינו חובה");
            return;
        }
        if (AppTool.IsNullOrEmpty(this.TypeCode)) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("קוד סוג הינו חובה");
            return;
        }
        

        if (AppTool.IsNullOrEmpty(this.PartnerCode)) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("קוד שותף היינו חובה");
            return;
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
