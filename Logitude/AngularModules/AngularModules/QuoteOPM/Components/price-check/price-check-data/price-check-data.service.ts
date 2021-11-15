import { Injectable } from '@angular/core';
import { ChargesTypeList } from 'Common/EntityLists/ChargesTypeList';
import { ChargesTypeListService } from 'Common/Services/StandardLists/ChargesTypeListService';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';

@Injectable()
export class PriceCheckDataService {

  constructor(
    private logtuideTable: LogtuideTableDataService,
    private chargesTypeListS: ChargesTypeListService,
  ) { }

  async getCahargesType(): Promise<ChargesTypeList[]> {
    return await this.logtuideTable.getDataFromService(this.chargesTypeListS.getAllFromCache())
  }
}
