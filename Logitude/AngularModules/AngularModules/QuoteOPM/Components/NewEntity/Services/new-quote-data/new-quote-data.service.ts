import { Injectable } from '@angular/core';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { ContactList } from 'Common/EntityLists/ContactList';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryList } from 'Common/EntityLists/CountryList';
import { AddressService } from 'Common/Services/ExtendedLists/AddressService';
import { NewQuoteOPWebService } from 'Customs/Services/WebServices/NewQuoteOPWebService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { DirectionList } from 'Infrastructure/EntityLists/DirectionList';
import { MoveTypeList } from 'Infrastructure/EntityLists/MoveTypeList';
import { TransportModeList } from 'Infrastructure/EntityLists/TransportModeList';
import { ObjectTablePM } from 'Infrastructure/EntityPMs/ObjectTablePM';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DirectionListService } from 'Infrastructure/Services/StandardLists/DirectionListService';
import { TransportModeListService } from 'Infrastructure/Services/StandardLists/TransportModeListService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { QuoteOPPMService } from 'QuoteOPM/Services/StandardPMs/QuoteOPPMService';
import { Observable } from 'rxjs';
import { filter, take } from 'rxjs/operators';
import { ShipmentTypeList } from 'Shipment/EntityLists/ShipmentTypeList';
import { ShipmentTypeListService } from 'Shipment/Services/StandardLists/ShipmentTypeListService';

declare const window: any;

@Injectable()
export class NewQuoteDataService {
  _entityResourceService: EntityResourceService = new EntityResourceService();
  addressService: AddressService = new AddressService();

  constructor(
    private entityListService: EntityListService,
    private newQuoteOPWebService: NewQuoteOPWebService,
  ) { }

  async getDirectionList(): Promise<DirectionList[]> {
    return this.getDataFromService(new DirectionListService().getAllFromCache());
  }

  async getTransportModeList(): Promise<TransportModeList[]> {
    return this.getDataFromService(new TransportModeListService().getAllFromCache())
  }

  async getShipmentTypeList(): Promise<ShipmentTypeList[]> {
    return this.getDataFromService(new ShipmentTypeListService().getAll());
  }

  async getCardsTable(): Promise<CardList[]> {
    return await this.getTable('Card') as CardList[];
    //   const cards: ServiceResponse = await new CardListService().getAll().toPromise() as ServiceResponse;
    //   return cards.Result as CardList[];
  }

  async getContactsTable(cardId: string): Promise<ContactList[]> {
    const filters = new ApiQueryFilters();
    filters.addAdditionalFilter('CardId', cardId, null, null, "Contains", true, false, false, "Text", false, false);
    filters.addAdditionalFilter('InActive', false, null, null, "Equals", false, false, false, null, false, false);
    filters.SortDirection = "Ascending";
    filters.PageIndex = 0;
    filters.PageSize = 50;

    return new Promise<ContactList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('Contact', filters).then();

      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });

    //     var v = {
    // usePrimNG: false,
    // ForceCacheRefresh: false,
    // DontApplyVirtualization: false,
    // GetAll: false,
    // PageIndex: 0,
    // PageSize: 50,
    // SortBy: EnglishName,
    // SortDirection: Descending,
    // AdditionalFilters: [{"FieldName":"InActive","FieldValue":false,"FieldValue2":null,"FieldValue3":null,"Operator":"Equals","IsCustom":false,"DisplayInList":true,"IsCustomField":false,"FieldDataType":"Boolean","IgnoreFilter":false,"IsCacheOnClient":false,"IsLookUpfilter":false},
    // {"FieldName":"CountryId","FieldValue":"1-257","FieldValue2":null,"FieldValue3":null,"Operator":"Equals","IsCustom":false,"DisplayInList":true,"IsCustomField":false,"FieldDataType":"Boolean","IgnoreFilter":false,"IsCacheOnClient":false,"IsLookUpfilter":false}]
    //     }

  }

  async getMoveTypeTable(transportModeId: string): Promise<MoveTypeList[]> {
    const filters = new ApiQueryFilters();
    filters.addAdditionalFilter('TransportModeId', transportModeId, null, null, "Equals", false, true, false, "LookUp", false, true, false);
    filters.addAdditionalFilter('InActive', false, null, null, "Equals", false, false, false, null, false, true, false);
    filters.SortDirection = "Ascending";
    filters.PageIndex = 0;
    filters.PageSize = 1000;

    return new Promise<MoveTypeList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('MoveType', filters).then();

      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });
  }

  async getPort(directionId: string, transportModed: string): Promise<Incoterm[]> {
    const res: ServiceResponse = await this.newQuoteOPWebService.GetPortsItemsList(directionId, transportModed, '', 1000, false).toPromise();
    return res.Result as Incoterm[];
  }

  async getSpecialService(directionId: string, transportModed: string): Promise<Incoterm[]> {
    const res: ServiceResponse = await this.newQuoteOPWebService.GetCarriersItemsList(directionId, transportModed, '', 1000, false).toPromise();
    return res.Result as Incoterm[];
  }

  async getSpecialServiceItems(directionId: string, transportModed: string): Promise<Incoterm[]> {
    const res: ServiceResponse = await this.newQuoteOPWebService.GetSpecialServiceItemsList(directionId, transportModed, '', 1000, false).toPromise();
    return res.Result as Incoterm[];
  }

  async getIncoterms(): Promise<Incoterm[]> {
    const res: ServiceResponse = await this.newQuoteOPWebService.GetETBPAYTRitemList('', '', 1000, false).toPromise();
    return res.Result as Incoterm[];
  }

  async getCityTable(countryId: string = null): Promise<CountryCityList[]> {
    const filters = new ApiQueryFilters();
    filters.addAdditionalFilter('InActive', false, null, null, "Equals", false, true, false, "Boolean", false, false, false);
    filters.SortDirection = "Ascending";
    filters.PageIndex = 0;
    filters.PageSize = 1000;

    if (countryId)
      filters.addAdditionalFilter('CountryId', countryId, null, null, "Equals", false, true, false, "Boolean", false, false, false);

    return new Promise<CountryCityList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('CountryCity', filters).then();

      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });
  }

  async getCounriesTable(): Promise<CountryList[]> {
    const filters = new ApiQueryFilters();
    filters.addAdditionalFilter('InActive', false, null, null, "Equals", false, false, false, null, false, false, false);
    filters.SortDirection = "Ascending";
    filters.PageIndex = 0;
    filters.PageSize = 1000;

    return new Promise<CountryList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('Country', filters).then();

      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });
  }

  async getAddress(cardId: string, tenant: number): Promise<AddressList> {
    return new Promise<AddressList>((resolve, reject) =>
      this.addressService.GetMainAddressByCardId(cardId, tenant)
        .pipe(filterIsNotNull(), take(1))
        .subscribe((myResult: ServiceResponse) => resolve(myResult.Result)));
  }

  creatingNewQuote(entityPM: QuoteOPPM): Promise<any> {
    return new Promise<any>((resolve, reject) => {
      new QuoteOPPMService().insert(entityPM).subscribe((myResponse: ServiceResponse) => {
        if (myResponse.HasError)
          reject(myResponse.ErrorsArray);
        else
          resolve(null);
      });
    })
  }



  private getTable(tableName: string): Promise<any> {
    return new Promise<ServiceResponse>((resolve, reject) => {
      this._entityResourceService.getEntityResourceByTableName(tableName, 0)
        .pipe(filterIsNotNull(), take(1))
        .subscribe(async () => {
          const LookUpTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name === tableName)[0];
          const loadPr: any = (LookUpTable?.CacheOnClient) ?
            await this.entityListService.getAllFromCache(tableName, new ApiQueryFilters()) :
            await this.entityListService.getAll(tableName);

          const response: ServiceResponse = await (<Observable<Promise<ServiceResponse>>>loadPr).toPromise();
          resolve(response.Result);
        });
    })
  }

  private getDataFromService(ob: Observable<any>): Promise<any[]> {
    return new Promise<TransportModeList[]>((resolve, reject) =>
      ob.pipe(filterIsNotNull(), take(1))
        .subscribe((res: ServiceResponse) =>
          resolve(res.Result)
        ));
  }
}

export function filterIsNotNull() {
  return filter((x: any) => x);
}

export interface Itm {
  $id: string;
  Name: string;
  PTERMID: string;
}

export interface Incoterm {
  $id: string;
  itm: Itm;
}
