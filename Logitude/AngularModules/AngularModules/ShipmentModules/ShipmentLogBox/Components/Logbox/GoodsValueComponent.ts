import {Component, OnInit, AfterViewInit} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CustomNumbersPipe} from '../../../../Infrastructure/Pipes/CustomNumbersPipe';

@Component({
  templateUrl: './GoodsValueComponent.html'
})

export class GoodsValueComponent implements OnInit, AfterViewInit {

    DataContext: GoodsValueComponent = this;
   
    AdditionalData: any;
    Language: string = 'HB';
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.Language = SessionLocator.TenantPM.Language;

    }
    ngOnInit() {
        
    }
    ngAfterViewInit() {

    }
    SetWindowArgs(args: any) {
        this.AdditionalData = args.AdditionalData;
    } 
   
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    
    public get SupAccount() { return new CustomNumbersPipe().transform(this.AdditionalData.SupAccount,0) }
    public set SupAccount(newValue: string) { this.AdditionalData.SupAccount = newValue; }

    public get IncotermId() { return this.AdditionalData.IncotermId }
    public set IncotermId(newValue: string) { this.AdditionalData.IncotermId = newValue; }

    public get Value() { return new CustomNumbersPipe().transform(this.AdditionalData.Value,0) }
    public set Value(newValue: string) { this.AdditionalData.Value = newValue; }

    public get CurrencyName() { return this.AdditionalData.CurrencyName }
    public set CurrencyName(newValue: string) { this.AdditionalData.CurrencyName = newValue; }

    public get CountryName() { return this.AdditionalData.CountryName }
    public set CountryName(newValue: string) { this.AdditionalData.CountryName = newValue; }

    public get SupplierName() { return this.AdditionalData.SupplierName }
    public set SupplierName(newValue: string) { this.AdditionalData.SupplierName = newValue; }

    public get SupplierFreight() { return new CustomNumbersPipe().transform(this.AdditionalData.SupplierFreight,0) }
    public set SupplierFreight(newValue: string) { this.AdditionalData.SupplierFreight = newValue; }
    
}
