import {ChangeDetectorRef, Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
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




@Component({
    selector: 'InvoiceApiCommunicationLogDetailsTabComponent',
    
    templateUrl: './InvoiceApiCommunicationLogDetailsTabComponent.html',
})


export class InvoiceApiCommunicationLogDetailsTabComponent extends BaseComponent{
   
    public EntityPM: any;


    public ObjectTableName: string = "InvoiceApiStep";
    public DataContext: any = this;
    public IsDisplayOnly: boolean = false;

    public columns: any[] = null;
    public invoiceApiStepListService: InvoiceApiStepListService;
    public isRTL: boolean = false;

    InvoiceApiStepList: ObservableCollection;


    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this.invoiceApiStepListService = new InvoiceApiStepListService();
        this.EntityPM = this.entityArgs.EntityPM;
        this.InvoiceApiStepList = new ObservableCollection([]);
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

    }
    ngOnInit() {
       
        this.LoadInvocieApiSteps();
        
    }
   
    
    ViewXMLClicked(item) {


        if (item.SecurityId) {
         
            DownloadManager.DownloadPage("", item.SecurityId);
        }
        else {
           
            DownloadManager.DownloadPage(item.DocumentId,null);
         
        }
        
  
    }
    RefreshButtonClicked() {
        //this._CommunicationLogStepDataViewModelList.Clear();
        //this.LoadCommunicationLogSteps();
    }
    LoadInvocieApiSteps() {
        
        this.invoiceApiStepListService.getAll().subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    var invoiceApiCommunicationLogViewModel = [];
                                        
                    result.sort((a, b) => a.Code.localeCompare(b.Code))                  
                           .forEach((item) => {
                            invoiceApiCommunicationLogViewModel.push(new InvoiceApiCommunicationLogViewModel(item,this.EntityPM));
                    });
                    this.InvoiceApiStepList.InsertCollection(invoiceApiCommunicationLogViewModel);
                    
                    this.cd.detectChanges();


                }
            }
           

        });
    }

  

    

     
   
}

 
 
