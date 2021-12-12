import { CashBookExtendedPMService } from './../../../Services/ExtendedPMs/CashBookExtendedPMService';
import { OnInit } from '@angular/core';
import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CashBookPM} from '../../../EntityPMs/CashBookPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { CashBookPMService } from 'Accounting/Services/StandardPMs/CashBookPMService';

@Component({

    templateUrl: './CashbookTotalAdjustWindow.html',
})

export class CashbookTotalAdjustWindow extends BaseComponent implements OnInit {
    public CashbookPM: CashBookPM = null;
    public ObjectTableName = "CashBook";
    tenantCurrency: string = SessionLocator.TenantPM.CurrencyCode;
    public isRTL: boolean = false;
    private cashBookPMService: CashBookPMService = new CashBookPMService();
    _CashBookExtendedPMService: CashBookExtendedPMService = new CashBookExtendedPMService();
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(private entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");


    }

    ngOnInit() {
    }


    SetWindowArgs(args: any) {
        if (args != null) {
            this.CashbookPM = args.CashbookPM;

            this.SetUIProperties();
        }
    }

    SetUIProperties() {
        // this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false); // always dim, WI 41740
    }


    private _Amount : number;
    public get Amount() : number {
        return this._Amount;
    }
    public set Amount(v : number) {
        this._Amount = v;
    }



    OkButtonClicked() {
        this.UpdateCashbook();
        this.SubmitCashbook();
    }

    private SubmitCashbook()
    {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Updating Total...");
        this.cashBookPMService.update(this.CashbookPM)
            .subscribe((myResponse: ServiceResponse) =>
            {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        if (myResponse.Result != undefined && myResponse.Result != null) {
                            this.CurrentSession.CloseCurrentWindow();

                        }
                        else {
                            const messageWindow = new MessageWindow();
                            messageWindow.Show("Error happened while updating totals");
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                    }
                }
            });
    }

    private UpdateCashbook()
    {
        this.CashbookPM.IsTotalUpdatedByCC = true;
        this.CashbookPM.TotalAmount = this.Amount || 0;
    }

    private ResetAmountIfNull()
    {
        if (this.Amount == null)
            this.Amount = 0;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


}
