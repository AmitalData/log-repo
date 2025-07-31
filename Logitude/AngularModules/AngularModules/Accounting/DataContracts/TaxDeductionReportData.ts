import { EventEmitter, Output } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { PropertyChangedArgs } from "Infrastructure/EventEmitterArgs/PropertyChangedArgs";

export class TaxDeductionReportData {

      readonly PropertyChanged = new EventEmitter<PropertyChangedArgs>();
      public readonly UIProperties = new UIProperties(this);
      constructor() {
                    this.UIProperties = new UIProperties(this); 
      }

    id: string;
    settingDeductionFileNumber: string;
    tenantVatNumber: string;
    phone: string;
    taxYear: string;
    ByMonthList: ByMonthList[];
    byVendorList: ByVendorList[];
    TotalForCompany: TotalForCompany[];
    dbVendorsList: DBVendorsList[];
    totalAmountInLocalCurrency?: number;
    totalDeductionInLocalCurrency?: number;
    totalAmountInLocalCurrency08?: number;
    totalTaxDeductionInLocalCurrency08?: number;
    totalEndBalance?: number;
    vendorsCount?: number;
    deductionLines: TaxDeductionReportLine[];
    fromDate?: Date;
    toDate?: Date;
    tenantAddress1?: string;
    tenantAddress2?: string;
}

export interface TaxDeductionReportLine {
    vendorId?: string;
    MonthOfRegisterDate?: number;
    amountInLocalCurrency?: number;
    taxDeductionLocalAmount?: number;
    taxDeductionPercentage?: string;
    deductionType?: string;
}

export interface TotalForCompany {
    companyName?: string;
    deductionFileNumber?: string;
    TotalPayments?: number;
    TotalDeductions?: number;
}

export interface DBVendorsList {
    rigesterDate?: Date;
    vendorId?: string;
    glAccountId?: string;
    amountInLocalCurrency?: number;
    taxDeductionLocalAmount?: number;
    deductionFileTypeCode: string;
    endYearBalance?: number;
}

export interface ByMonthList {
    Month: number;
    ReportMonth: string;
    TotalAmountInLocalCurrency?: number;
    TotalTaxDeductionLocalAmount?: number;
    TotalVendors: number;
    TotalPaymentsWithoutDivided?: number;
    TotalDeductionsWithoutDivided?: number;
    TotalDivided?: number;
    TotalDeductionsFromDivided?: number;
}

export interface ByVendorList {
    vendorName?: string;
    month?: number;
    deductionFileNumber?: string;
}