import { Component } from '@angular/core';
import { DateTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DateTimeToDatePipe } from '../../../Controls/Pipes/DateTimeToDatePipe';

@Component({
    templateUrl: './NewChargifyAWBStockComponent.html',
})

export class NewChargifyAWBStockComponent extends BaseComponent {

    private CurrentSession = SessionLocator.SelectedSession;
    private commonDomainService: CommonDomainService;
    public IsINTTRAPackage = false;
    public IsChargifyAccount = false;
    public ValidationWarningsMessage = null;
    public IsValidationWarningsVisible = false;
    public DataContext = this;
    public IsVisible = false;
    IsConnectaPanageaPartner = false;

    constructor() {
        super();
        this.InitalizeServices();
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.IsINTTRAPackage = args.IsINTTRAPackage;
            this.IsChargifyAccount = args.IsChargifyAccount;
            this.CheckConnectaPanageaPartner();
        }
    }

    CheckConnectaPanageaPartner() {
        this.commonDomainService.GetCheckConnectaPanageaPartner().subscribe((myResult: any) => {
            var connectaPanageaPartner = myResult.Result;
            if (connectaPanageaPartner != null) {
                this.IsConnectaPanageaPartner = true;
            }
            this.SetAWBSINTTRALables();
            this.IsVisible = true;
        });
    }

    InitalizeServices() {
        this.commonDomainService = new CommonDomainService();
    }

    private isAWBStockChecked = true;
    get IsAWBStockChecked() { return this.isAWBStockChecked; }
    set IsAWBStockChecked(newValue: boolean) {
        if (this.isAWBStockChecked != newValue) {
            this.isAWBStockChecked = newValue;
            this.ResetCheckBoxes();
        }
    }

    ResetCheckBoxes() {
        if (!this.IsAWBStockChecked) {
            this.Is100AWBSPackage = false;
            this.Is200AWBSPackage = false;
            this.Is500AWBSPackage = false;
            this.Is1000AWBSPackage = false;

        }
        else {
            this.Is100INTTRAPackage = false;
            this.Is200INTTRAPackage = false;
            this.Is500INTTRAPackage = false;
            this.Is1000INTTRAPackage = false;
        }
    }
    SetAWBStock(isChecked: boolean) {
        this.IsAWBStockChecked = isChecked;
    }

    private is100AWBSPackage = false;
    get Is100AWBSPackage() { return this.is100AWBSPackage; }
    set Is100AWBSPackage(newValue: boolean) {
        if (this.is100AWBSPackage != newValue) {
            this.is100AWBSPackage = newValue;
            this.CheckValidationWarningsVisible();
        }
    }

    private is200AWBSPackage = false;
    get Is200AWBSPackage() { return this.is200AWBSPackage; }
    set Is200AWBSPackage(newValue: boolean) {
        if (this.is200AWBSPackage != newValue) {
            this.is200AWBSPackage = newValue;
            this.CheckValidationWarningsVisible();
        }
    }

    private is500AWBSPackage = false;
    get Is500AWBSPackage() { return this.is500AWBSPackage; }
    set Is500AWBSPackage(newValue: boolean) {
        if (this.is500AWBSPackage != newValue) {
            this.is500AWBSPackage = newValue;
            this.CheckValidationWarningsVisible();
        }
    }

    private is1000AWBSPackage = false;
    get Is1000AWBSPackage() { return this.is1000AWBSPackage; }
    set Is1000AWBSPackage(newValue: boolean) {
        if (this.is1000AWBSPackage != newValue) {
            this.is1000AWBSPackage = newValue;
            this.CheckValidationWarningsVisible();
        }
    }

    public AWBSPackage100Label = "100 AWBs package ($100 for use within 12 months)";
    public AWBSPackage200Label = "200 AWBs package ($180 for use within 12 months)";
    public AWBSPackage500Label = "500 AWBs package ($390 for use within 12 months)";
    public AWBSPackage1000Label = "1000 AWBs package ($650 for use within 12 months)";

    public INTTRAPackage100Label = "100 INTTRA package ($100 for use within 12 months)";
    public INTTRAPackage200Label = "200 INTTRA package ($180 for use within 12 months)";
    public INTTRAPackage500Label = "500 INTTRA package ($390 for use within 12 months)";
    public INTTRAPackage1000Label = "1000 INTTRA package ($650 for use within 12 months)";

    SetAWBSINTTRALables() {
        this.AWBSPackage100Label = "100 AWBs package ($100 for use within 12 months)";
        this.AWBSPackage200Label = "200 AWBs package ($180 for use within 12 months)";
        this.AWBSPackage500Label = "500 AWBs package ($390 for use within 12 months)";
        this.AWBSPackage1000Label = "1000 AWBs package ($650 for use within 12 months)";
        this.INTTRAPackage100Label = "100 INTTRA package ($100 for use within 12 months)";
        this.INTTRAPackage200Label = "200 INTTRA package ($180 for use within 12 months)";
        this.INTTRAPackage500Label = "500 INTTRA package ($390 for use within 12 months)";
        this.INTTRAPackage1000Label = "1000 INTTRA package ($650 for use within 12 months)";
        if (this.IsConnectaPanageaPartner) {
            this.AWBSPackage100Label = "100 AWBs package ($55 for use within 12 months)";
            this.AWBSPackage200Label = "200 AWBs package ($110 for use within 12 months)";
            this.AWBSPackage500Label = "500 AWBs package ($275 for use within 12 months)";
            this.AWBSPackage1000Label = "1000 AWBs package ($550 for use within 12 months)";
            this.INTTRAPackage100Label = "100 INTTRA package ($55 for use within 12 months)";
            this.INTTRAPackage200Label = "200 INTTRA package ($110 for use within 12 months)";
            this.INTTRAPackage500Label = "500 INTTRA package ($275 for use within 12 months)";
            this.INTTRAPackage1000Label = "1000 INTTRA package ($550 for use within 12 months)";
        }
    }

    private is100INTTRAPackage = false;
    get Is100INTTRAPackage() { return this.is100INTTRAPackage; }
    set Is100INTTRAPackage(newValue: boolean) {
        if (this.is100INTTRAPackage != newValue) {
            this.is100INTTRAPackage = newValue;
            this.CheckValidationWarningsVisible();
        }
    }

    private is200INTTRAPackage = false;
    get Is200INTTRAPackage() { return this.is200INTTRAPackage; }
    set Is200INTTRAPackage(newValue: boolean) {
        if (this.is200INTTRAPackage != newValue) {
            this.is200INTTRAPackage = newValue;
            this.CheckValidationWarningsVisible();
        }
    }

    private is500INTTRAPackage = false;
    get Is500INTTRAPackage() { return this.is500INTTRAPackage; }
    set Is500INTTRAPackage(newValue: boolean) {
        if (this.is500INTTRAPackage != newValue) {
            this.is500INTTRAPackage = newValue;
            this.CheckValidationWarningsVisible();
        }
    }

    private is1000INTTRAPackage = false;
    get Is1000INTTRAPackage() { return this.is1000INTTRAPackage; }
    set Is1000INTTRAPackage(newValue: boolean) {
        if (this.is1000INTTRAPackage != newValue) {
            this.is1000INTTRAPackage = newValue;
            this.CheckValidationWarningsVisible();
        }
    }

    CheckValidationWarningsVisible() {
        this.IsValidationWarningsVisible = this.SetIsValidationWarningsVisible();
        if (this.IsValidationWarningsVisible) {
            this.SetIsValidationWarningsMesage();
        }
    }

    SetIsValidationWarningsVisible() {
        if (this.Is100AWBSPackage)
            return true;
        if (this.Is200AWBSPackage)
            return true;
        if (this.Is500AWBSPackage)
            return true;
        if (this.Is1000AWBSPackage)
            return true;
        if (this.Is100INTTRAPackage)
            return true;
        if (this.Is200INTTRAPackage)
            return true;
        if (this.Is500INTTRAPackage)
            return true;
        if (this.Is1000INTTRAPackage)
            return true;
        return false;
    }
    private totalPrice: number;
    private totalStocks: number;
    SetIsValidationWarningsMesage() {
        this.ValidationWarningsMessage = null;
        this.totalPrice = this.GetTotalStockPrice();
        this.totalStocks = this.GetTotalStockPackages();
        var todayDate = DateTool.GetCurrentDateAsUtc();
        todayDate.setMonth(todayDate.getMonth() + 12);
        var expirationDate = DateTimeToDatePipe.Pipe(todayDate);
        var title = "";
        if (this.IsChargifyAccount && this.IsINTTRAPackage) {
            title = this.IsAWBStockChecked ? " AWBs" : " INTTRA packages";
        }
        else {
            title = this.IsChargifyAccount ? " AWBs" : " INTTRA packages";
        }

        var warninig = "You have selected " + this.totalStocks +  title + " for a total price of $" + this.totalPrice + ". The expiration date of your stock is " + expirationDate + ". Please confirm your selection by clicking on Confirm button.";
        this.ValidationWarningsMessage = warninig;
    }

    GetTotalStockPrice() {
        var total = 0;
        if (this.Is100AWBSPackage)
            total = this.IsConnectaPanageaPartner ? total + 55 : total+100;
        if (this.Is200AWBSPackage)
            total = this.IsConnectaPanageaPartner ? total + 110 :total + 180;
        if (this.Is500AWBSPackage)
            total = this.IsConnectaPanageaPartner ? total + 275 : total + 390;
        if (this.Is1000AWBSPackage)
            total = this.IsConnectaPanageaPartner ? total + 550 : total + 650;
        if (this.Is100INTTRAPackage)
            total = this.IsConnectaPanageaPartner ? total + 55 : total + 100;
        if (this.Is200INTTRAPackage)
            total = this.IsConnectaPanageaPartner ? total + 110 :total + 180;
        if (this.Is500INTTRAPackage)
            total = this.IsConnectaPanageaPartner ? total + 275 :total + 390;
        if (this.Is1000INTTRAPackage)
            total = this.IsConnectaPanageaPartner ? total + 550 : total + 650;
        return total;
    }

    GetTotalStockPackages() {
        var total = 0;
        if (this.Is100AWBSPackage)
            total = total + 100;
        if (this.Is200AWBSPackage)
            total = total + 200;
        if (this.Is500AWBSPackage)
            total = total + 500;
        if (this.Is1000AWBSPackage)
            total = total + 1000;
        if (this.Is100INTTRAPackage)
            total = total + 100;
        if (this.Is200INTTRAPackage)
            total = total + 200;
        if (this.Is500INTTRAPackage)
            total = total + 500;
        if (this.Is1000INTTRAPackage)
            total = total + 1000;
        return total;
    }

    SendButtonClicked() {
        var confirmWindow = new ConfirmWindow();
        var title = "";
        if (this.IsChargifyAccount && this.IsINTTRAPackage) {
            title = this.IsAWBStockChecked ? "Are you sure you want to purchase " + this.totalStocks + " AWBs stock?" : "Are you sure you want to buy " + this.totalStocks +" INTTRA messages stock?";
        }
        else {
            title = this.IsChargifyAccount ? "Are you sure you want to purchase " + this.totalStocks + " AWBs stock?" : "Are you sure you want to buy " + this.totalStocks +" INTTRA messages stock?";
        }
        confirmWindow.Show(title);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CompleteSendStockPackagesProcess();
            }
            else if (confirmWindow.No) {
               
            }
        });
    }

    CompleteSendStockPackagesProcess() {
        this.CurrentSession.StartBusyIndicatorLoading();

        var isAWBStockChecked = false;
        if (this.IsChargifyAccount == true && this.IsINTTRAPackage == true) {
            isAWBStockChecked = this.IsAWBStockChecked;
        }
        else {
            if (this.IsChargifyAccount == true) {
                isAWBStockChecked = true;
            }
            else if (this.IsINTTRAPackage == true) {
                isAWBStockChecked = false;
            }
        }
        this.commonDomainService.GetChargifyAWBStock(isAWBStockChecked, this.totalStocks, this.totalPrice).subscribe((myResult: any) => {
            this.CurrentSession.StopBusyIndicator();
            if (myResult && !myResult.HasError) {
                this.ShowSendingStockCompleteMessage();
            }
            this.CurrentSession.CloseCurrentWindow();

        });
    }

    ShowSendingStockCompleteMessage() {
        var msg = new MessageWindow();
        msg.Width = 600;
        msg.Height = 150;
        msg.Show("Thank you for trusting Logitude!\r\nYour order has been received and the new messaging stock has been added to your account.\r\nYour invoice will be prepared and sent within a few days.");
    }
     
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
