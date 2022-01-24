


import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import { Component, Output, EventEmitter, OnInit, ComponentRef, ViewChild} from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { CourierMasterValidator } from '../../../../Customs/Validators/CourierMasterValidator';
import { ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationCourierStatusListService } from '../../../../Customs/Services/StandardLists/DeclarationCourierStatusListService';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DeclarationEditComponentController } from '../../../../Customs/Controller/DeclarationEditComponentController';
import {DropdownMenuFilterComponent}  from './DropdownMenuFilterComponent'
import { CustomBankListService } from '../../../../Customs/Services/StandardLists/CustomBankListService';
import {CustomBankList} from '../../../../Customs/EntityLists/CustomBankList';


@Component({
    
    selector: 'GetInternalBankComponent',
    templateUrl: './GetInternalBankComponent.html',
})

export class GetInternalBankComponent extends BaseComponent {
    public DataContext: GetInternalBankComponent = this;
    public ObjectTableName: string = "AccountingPeriod";
    
    public ValidationErrorsList: string[];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        
        this.UIProperties.SetEnabled("SelectedBank", this.ObjectTableName, true);
        
        
        
    }

    
    ngOnInit() {
        this.LoadBanks()
        this.CurrentSession.StopBusyIndicator();
    }



    FillErrors() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.SelectedBank)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("Bank Field is Required");
        
        } else {
            this.ValidationErrorsList = [];

        }
    }
    _CustomBankListService: CustomBankListService = new CustomBankListService();
    BanksList: CustomBankList[] =[];
    _SelectedBank: CustomBankList;
    // Properties
    public get SelectedBank() { return this._SelectedBank; }
    public set SelectedBank(newValue: CustomBankList) {
        this._SelectedBank = newValue;
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetRequired("SelectedBank", this.ObjectTableName, true);
        } else {
            this.UIProperties.SetRequired("SelectedBank", this.ObjectTableName, false);
        }
    }
    LoadBanks() {
        this._CustomBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            if (response) {
                if (!response.HasError) {
                    this.BanksList = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                    if (this.BanksList.length == 1) {
                        this.SelectedBank = this.BanksList[0];
                    }
                }
            }
        });
    }
    //public OnSend: (InternalBankId: string) => void;
    
    OkButtonClicked() {

        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.CurrentWindow.Close(this._SelectedBank.Id);
    }
    
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    

}
