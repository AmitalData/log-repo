import { Injectable } from "@angular/core";
import { CustomsItemList } from "Customs/EntityLists/CustomsItemList";
import { CustomsItemListService } from "Customs/Services/StandardLists/CustomsItemListService";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { LogtuideTableDataService } from "Infrastructure/Services/logtuide-table-data.service";
 
@Injectable()
export class customsItemsService {
    public async checkClassificationCode(classificationCode: string, isExport: boolean): Promise<boolean> {
        const classificationCodeWithoutLastNumber: string = classificationCode.substring(0, classificationCode.length - 1);
        
        const apiQueryFilters: ApiQueryFilters = customsItemsService.initTaxExemptCodeTypesFilter(isExport);
        apiQueryFilters.addAdditionalFilter("FullClassification", classificationCodeWithoutLastNumber , null, null, "Contains", false, false, false, "string");

        const customsItemList: CustomsItemList[] = await LogtuideTableDataService.createInstance().getDataFromService(
            new CustomsItemListService().getByFilters(apiQueryFilters)
        ) as CustomsItemList[];

        return !!customsItemList.length;
    }

    static initTaxExemptCodeTypesFilter(isExport: boolean): ApiQueryFilters {
        const taxExemptCodeTypesFilter = new ApiQueryFilters();
        taxExemptCodeTypesFilter.GetAll = true; 
        taxExemptCodeTypesFilter.addAdditionalFilter("CustomsBookTypeID", '1', null, null, isExport ? "NotEqual" : "Equals", false, false, false, "string")        
        taxExemptCodeTypesFilter.addAdditionalFilter("CustomsItemCategoryID", '2', '3', null, 'Equals', false, false, false, "string")
        taxExemptCodeTypesFilter.addAdditionalFilter("dateExpire", 1, null, null, 'Equals', true, false, false, "string")

        return taxExemptCodeTypesFilter;
    }
}