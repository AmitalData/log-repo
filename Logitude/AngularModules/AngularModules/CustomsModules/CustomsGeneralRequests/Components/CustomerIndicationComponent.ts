import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimImporterDeclarsP3LoiPM } from '../../../Customs/EntityPMs/ClaimImporterDeclarsP3LoiPM';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: './CustomerIndicationComponent.html',
})

export class CustomerIndicationComponent extends BaseComponent {
    public DataContext: CustomerIndicationComponent = this;

    public CustomerIndicationList: ObservableCollection;
    private isControlEnabled: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    isLoad:boolean=false;
    UpdateDate:Date;
    IsClientIndication:boolean=false;
    EntityResourceService:EntityResourceService=new EntityResourceService();
    constructor(public entityArgs: EntityArgs) {
        super();

        this.CustomerIndicationList = new ObservableCollection([]);
    }

    SetWindowArgs(args: any) {
       
        this.EntityResourceService.getEntityResourceByTableName("Customs.ClientIndication").subscribe((response: any) => {
            this.IsClientIndication = args.IsClientIndication
            if(this.IsClientIndication){
                          
                this.CustomerIndicationList.InsertCollection(args.CustomerIndicationList);

                if (args.CustomerIndicationList &&  args.CustomerIndicationList.length>0) {
                    this.UpdateDate = args.CustomerIndicationList[0].CreateDate;
                }
            }
            else{
                this.CustomerIndicationList = args.CustomerIndicationList;
            }
           
            this.isLoad = true;
           
      });     
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    //#endregion
}
