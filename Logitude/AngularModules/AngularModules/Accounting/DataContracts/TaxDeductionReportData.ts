import { EventEmitter, Output } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { PropertyChangedArgs } from "Infrastructure/EventEmitterArgs/PropertyChangedArgs";

export class TaxDeductionReportData {

      @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
      public UIProperties: UIProperties;
      constructor() {
                    this.UIProperties = new UIProperties(this); 
      }

    id: string;
    settingDeductionFileNumber: string;
    tenantVatNumber: string;
    phone: string;
    taxYear: string;
    byMonthList: ByMonthList[];
    byVendorList: ByVendorList[];
    totalForCompany: TotalForCompany[];
    dbVendorsList: DBVendorsList[];
    totalAmountInLocalCurrency?: number;
    totalDeductionInLocalCurrency?: number;
    totalAmountInLocalCurrency08?: number;
    totalTaxDeductionInLocalCurrency08?: number;
    totalEndBalance?: number;
    vendorsCount?: number;
    deductionLines: any;
    fromDate: Date;
    toDate: Date;
    tenantAddress1: string;
    tenantAddress2: string;
}

export interface TotalForCompany {
    companyName: string;
    deductionFileNumber: string;
    totalPayments?: number;
    totalDeductions?: number;
}

export interface DBVendorsList {
    rigesterDate?: Date;
    vendorId: string;
    glAccountId: string;
    amountInLocalCurrency?: number;
    taxDeductionLocalAmount?: number;
    deductionFileTypeCode: string;
    endYearBalance?: number;
}

export interface ByMonthList {
    month: number;
    reportMonth: string;
    totalAmountInLocalCurrency?: number;
    totalTaxDeductionLocalAmount?: number;
    totalVendors: number;
    totalPaymentsWithoutDivided?: number;
    totalDeductionsWithoutDivided?: number;
    totalDivided?: number;
    totalDeductionsFromDivided?: number;
}

export interface ByVendorList {
    vendorName: string;
    month: number;
    deductionFileNumber: string;
}