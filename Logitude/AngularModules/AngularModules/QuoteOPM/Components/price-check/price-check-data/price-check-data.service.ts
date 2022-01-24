import { Injectable } from '@angular/core';
import { ChargesTypeList } from 'Common/EntityLists/ChargesTypeList';
import { ProductTypeList } from 'Common/EntityLists/ProductTypeList';
import { ChargesTypeListService } from 'Common/Services/StandardLists/ChargesTypeListService';
import { ProductTypeListService } from 'Common/Services/StandardLists/ProductTypeListService';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';

@Injectable()
export class PriceCheckDataService {

  constructor(
    private logtuideTable: LogtuideTableDataService,
    private chargesTypeListS: ChargesTypeListService,
    private productTypeListS: ProductTypeListService,
  ) { }

  async getCahargesType(): Promise<ChargesTypeList[]> {
    return await this.logtuideTable.getDataFromService(this.chargesTypeListS.getAllFromCache())
  }

  async getProducteType(): Promise<ProductTypeList[]> {
    return await this.logtuideTable.getDataFromService(this.productTypeListS.getAllFromCache())
  }
}
