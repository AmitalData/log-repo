declare var window: any;
import { Component, ChangeDetectorRef } from '@angular/core';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { OnInit, Output, EventEmitter, ComponentRef, QueryList } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';

import { ListComponentArgs } from '../../../Infrastructure/Args';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

import { ClosedTableStatusListService } from '../../../Customs/Services/StandardLists/ClosedTableStatusListService';
import { CustomsClosedTableList } from '../../../Customs/EntityLists/CustomsClosedTableList';
import { ClosedTableStatusList } from '../../../Customs/EntityLists/ClosedTableStatusList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { SystemTableRequestParams } from '../../../Customs/DataContract/RequestParams/SystemTableRequestParams';
import { SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';

@Component({
    moduleId: module.id,
    templateUrl: 'CustomsClosedTablesListTemplate.html',
})

export class CustomsClosedTablesListTemplate {

    _CustomsClosedTable: CustomsClosedTableList;
    public fieldName: any;
    TableUpdateButtonIsEnabled: boolean = false;
    UpdateButtonVisibility: boolean = false;
    TableUpdateButtonOpacity: string = "1";
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        //        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;

        if (AppTool.IsNullOrEmpty(CustomsClosedTablesListTemplate.translate_CommunicationLogBView)) {
            this._entityResourceService.getEntityResourceByTableName("CommunicationLog")
                .subscribe(response => {
                    CustomsClosedTablesListTemplate.translate_CommunicationLogBView = TextCodeTranslator.Translate("CommunicationLog.B.View");// itzik : Translate +_entityResourceService - its bad :due that i done this- 
                });
        }
    }

    static translate_CommunicationLogBView: string = "";
    // itzik : Translate +_entityResourceService - its bad :due that i done this- 
    get CommunicationLogBView() {
        return CustomsClosedTablesListTemplate.translate_CommunicationLogBView;
    }

    setVariables(CustomsClosedTable: CustomsClosedTableList, fieldName: string) {
        ///console.log(rowData);
        this._CustomsClosedTable = CustomsClosedTable;

        this.fieldName = fieldName;
        this.RefreshFields();
    }
    RefreshFields() {
        if (this._CustomsClosedTable.Existed) {
            this.UpdateButtonVisibility = true;
        }
        if (this._CustomsClosedTable.StatusCode != "2") {
            this.TableUpdateButtonIsEnabled = true;// 
            this.TableUpdateButtonOpacity = "1";
        }
        else {
            this.TableUpdateButtonIsEnabled = false;
            this.TableUpdateButtonOpacity = "0.7";
        }
        this.CD.detectChanges();
    }


    TableUpdateButtonCommandAction() {
        if (this.TableUpdateButtonIsEnabled) {
            this.UpdateClosedTable();
            return;
        }
        let confirm = new ConfirmWindow();

        var text = TextCodeTranslator.Translate("Customs.M.AlreadySendReSend");
        if (AppTool.IsNullOrEmpty(text)) {
            text = "יש בקשה זהה בתהליך, האם להמשיך ?";
        }
        confirm.Show(text);

        confirm.WindowClosed.subscribe((event: any) => {
            if (confirm.Yes) {
                this.UpdateClosedTable();
            }
        });
    }
    ShowDetailsNotExistTable() {


        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 850;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = this._CustomsClosedTable.CustomsLocalName;
        logitudeWindow.WindowArgs = this._CustomsClosedTable;
        //logitudeWindow.Show('./Customs/Components/Maintenance/ClosedTableNotExistedComponent');
        logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/ClosedTableNotExistedComponent');
        return;



        
    }
    ShowDetails() {
         if (!this._CustomsClosedTable.Existed) {
            this.ShowDetailsNotExistTable();
            return;
        }
        var objectTablePM = //window.ObjectTables.filter(d => d.Id == ObjectTableId)[0];
            window.ObjectTables.filter(t => t.Name == this._CustomsClosedTable.ObjectTableName)[0];
        //var objectTablePM =   window.ObjectTables.filter(d => d.Id == ObjectTableId)[0];
        if (objectTablePM) {
            var allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === objectTablePM.Id).sort((a, b) => {
                return a.IndexOrder - b.IndexOrder
            });
            if (allQueries.length == 0) {
                //var myConfirmWindow = new ConfirmWindow();
                //myConfirmWindow.Show("No Queries found for " + item.TranslatedName);
            }

            else {
                var listArgs = new ListComponentArgs();



                var SelectedQuery = null;



                SelectedQuery = allQueries[0];
           
                listArgs.QueryCode = SelectedQuery.Code;
                listArgs.ObjectTableName = objectTablePM.Name;

                
                switch (listArgs.ObjectTableName) {
                    case 'Customs.GovernmentProcedureType':
                    case "Customs.NotificationDefinition":
                    case "Customs.CustomsHouseType":
                    case "Customs.CustomDocumentType":
                    case "Customs.UIMessage":
                    case "Customs.CustomsCountry":
                    //case "Customs.InternationalSite":
                        listArgs.SuppressOnRowSelected = false;
                        break;
                    default:
                        listArgs.SuppressOnRowSelected = true;
                        break;
                }
            

                listArgs.BackButtonTitle = "Maintenance";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    listArgs.DisplayTitle = TextCodeTranslator.Translate(SelectedQuery.NameTextCodeCode);
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            //this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
            }
        }
    }
    UpdateClosedTable() {
        //this.TableUpdateButtonIsEnabled = false;
        

        var myClosedTableStatusListService = new ClosedTableStatusListService();
        //this.CurrentSession.StartBusyIndicator("");
        myClosedTableStatusListService.getSingleFromCache("2").subscribe(result => {
            let status: ClosedTableStatusList = result.Result as ClosedTableStatusList;
            this._CustomsClosedTable.StatusName = status.LocalName
            this._CustomsClosedTable.LastUpdateDate = DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), 0);
            this._CustomsClosedTable.StatusCode = "2";
            //this._CustomsClosedTable.StatusName = "מעדכן";///<span _ngcontent-hdd-22="">מעדכן</span>
            this.RefreshFields();

        });
        
        let systemTableRequestParams = new SystemTableRequestParams();
        systemTableRequestParams.TableId = this._CustomsClosedTable.Id;
        systemTableRequestParams.Tenant = SessionLocator.Tenant;
        systemTableRequestParams.RequestVIA == SendRequestVIA.WebServiceBatch;//all the time 

        let myIIGGeneralMessagesService = new IIGGeneralMessagesService();
        myIIGGeneralMessagesService.PostUpdateClosedTables(systemTableRequestParams).subscribe(
            res => { }
        )
    }

}
