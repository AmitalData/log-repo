import { Injectable } from "@angular/core";
import { CustomsItemList } from "Customs/EntityLists/CustomsItemList";
import { CustomsItemListService } from "Customs/Services/StandardLists/CustomsItemListService";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { LogtuideTableDataService } from "QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service";

@Injectable()
export class customsItemsService {
    public async checkClassificationCodeIsImport(classificationCode: string ): Promise<boolean> {
        const classificationCodeWithoutLastNumber: string = classificationCode.substring(0, classificationCode.length - 1);
        
        const apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.PageIndex = 0;
        apiQueryFilters.PageSize = 50;
        apiQueryFilters.addAdditionalFilter("FullClassification", classificationCodeWithoutLastNumber , null, null, "Contains", false, false, false, "string");

        const customsItemList: CustomsItemList[] = await LogtuideTableDataService.createInstance().getDataFromService(
            new CustomsItemListService().getByFilters(apiQueryFilters)
        ) as CustomsItemList[];

        return customsItemList.some(x=> x.CustomsBookTypeID == 2 || x.CustomsBookTypeID == 3);
    }

}