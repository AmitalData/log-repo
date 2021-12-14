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
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { DirectionListService } from 'Infrastructure/Services/StandardLists/DirectionListService';
import { TransportModeListService } from 'Infrastructure/Services/StandardLists/TransportModeListService';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { QuoteOPPMService } from 'QuoteOPM/Services/StandardPMs/QuoteOPPMService';
import { filter, take } from 'rxjs/operators';
import { ShipmentTypeList } from 'Shipment/EntityLists/ShipmentTypeList';
import { ShipmentTypeListService } from 'Shipment/Services/StandardLists/ShipmentTypeListService';
import { PackageTypeList } from 'Common/EntityLists/PackageTypeList';
import { LogtuideTableDataService } from '../../components/autocomplate-table/logtuide-table-data.service';
import { Subject } from 'rxjs';
import { TenantPMService } from 'Common/Services/StandardPMs/TenantPMService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TenantList } from 'Common/EntityLists/TenantList';
import { TenantListService } from 'Common/Services/StandardLists/TenantListService';
import { CardPMService } from 'Common/Services/StandardPMs/CardPMService';
import { CardPM } from 'Common/EntityPMs/CardPM';
import { ContactPMService } from 'Common/Services/StandardPMs/ContactPMService';
import { ContactPM } from 'Common/EntityPMs/ContactPM';

@Injectable()
export class NewQuoteDataService {
  $resetForm = new Subject();
  addressService: AddressService = new AddressService();

  constructor(
    private entityListService: EntityListService,
    private cardPMService: CardPMService,
    private contactPMService: ContactPMService,
    private newQuoteOPWebService: NewQuoteOPWebService,
    private logtuideTableDataService: LogtuideTableDataService,
    private tanentsService: TenantListService,
  ) { }

  async getTenantsData(): Promise<TenantList> {
    return this.logtuideTableDataService.getDataFromService(this.tanentsService.getSingle(SessionLocator.TenantPM.Id)) as any;
  }

  async getDirectionList(): Promise<DirectionList[]> {
    return this.logtuideTableDataService.getDataFromService(new DirectionListService().getAllFromCache());
  }

  async getTransportModeList(): Promise<TransportModeList[]> {
    return this.logtuideTableDataService.getDataFromService(new TransportModeListService().getAllFromCache())
  }

  async getShipmentTypeList(): Promise<ShipmentTypeList[]> {
    return this.logtuideTableDataService.getDataFromService(new ShipmentTypeListService().getAll());
  }

  async getContact(id: string): Promise<ContactPM> {
    return await this.logtuideTableDataService.getDataFromService(this.contactPMService.get(id))
  }

  async getCard(id: string): Promise<CardPM> {
    return await this.logtuideTableDataService.getDataFromService(this.cardPMService.get(id))
  }

  async getCardsTable(): Promise<CardList[]> {
    return await this.logtuideTableDataService.getTable('Card') as CardList[];
  }

  async getContactsTable(cardId: string): Promise<ContactList[]> {
    const filters = new ApiQueryFilters();
    filters.addAdditionalFilter('CardId', cardId, null, null, "Contains", true, false, false, "Text", false, false);
    filters.addAdditionalFilter('InActive', false, null, null, "Equals", false, false, false, null, false, false);
    filters.SortDirection = "Ascending";
    // filters.PageIndex = 0;
    // filters.PageSize = 50;
    filters.GetAll = true;

    return new Promise<ContactList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('Contact', filters).then();

      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });
  }

  async getMoveTypeTable(transportModeId: string): Promise<MoveTypeList[]> {
    const filters = new ApiQueryFilters();
    filters.addAdditionalFilter('TransportModeId', transportModeId, null, null, "Equals", false, true, false, "LookUp", false, true, false);
    filters.addAdditionalFilter('InActive', false, null, null, "Equals", false, false, false, null, false, true, false);
    filters.SortDirection = "Ascending";
    // filters.PageIndex = 0;
    // filters.PageSize = 1000;
    filters.GetAll = true;

    return new Promise<MoveTypeList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('MoveType', filters).then();

      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });
  }

  async getPackageTypeTable(): Promise<PackageTypeList[]> {
    const filters = new ApiQueryFilters();
    //filters.addAdditionalFilter('InActive', false, null, null, "Equals", false, false, false, null, false, false);
    filters.SortDirection = "Ascending";
    // filters.PageIndex = 0;
    // filters.PageSize = 50;
    filters.GetAll = true;
    return new Promise<PackageTypeList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('PackageType', filters).then();

      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });
  }

  async getPorts(directionId: string, transportModed: string, filter: ApiQueryFilters): Promise<Port[]> {
    const res: ServiceResponse = await this.newQuoteOPWebService.GetPorts(directionId, transportModed, filter).toPromise();
    return res.Result.body as Port[];
  }

  async getCarrierses(directionId: string, transportModed: string, filter: ApiQueryFilters): Promise<Carrier[]> {
    const res: ServiceResponse = await this.newQuoteOPWebService.GetCarriers(directionId, transportModed, filter).toPromise();
    return res.Result.body as Carrier[];
  }

  async getSpecialServices(directionId: string, transportModed: string, filter: ApiQueryFilters): Promise<SpecialService[]> {
    const res: ServiceResponse = await this.newQuoteOPWebService.GetSpecialService(directionId, transportModed, filter).toPromise();
    return res.Result.body as SpecialService[];
  }

  async getIncoterms(filter: ApiQueryFilters): Promise<Incoterm[]> {
    const res: ServiceResponse = await this.newQuoteOPWebService.GetIncoterm(filter).toPromise();
    return res.Result.body as Incoterm[];
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
    // filters.PageIndex = 0;
    // filters.PageSize = 1000;
    filters.GetAll = true;

    return new Promise<CountryList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('Country', filters).then();

      resService.pipe(filterIsNotNull(), take(1))
        .subscribe((resp: any) => resolve(resp.Result));
    });
  }

  async getAddresses(cardId: string, tenant: number): Promise<AddressList[]> {
    const filters = new ApiQueryFilters();
    filters.GetAll = true
    filters.SortDirection = "Ascending";
    filters.addAdditionalFilter('CardId', cardId, null, null, "Equals", true, false, false, "Text", false, false);
    filters.addAdditionalFilter('Tanent', cardId, null, null, "Equals", true, false, false, "Text", false, false);

    return new Promise<AddressList[]>(async (resolve, reject) => {
      const resService: any = await this.entityListService.getByFilters('Address', filters).then();

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
      new QuoteOPPMService().insert(entityPM)
        .pipe(filterIsNotNull(), take(1))
        .subscribe((myResponse: ServiceResponse) => {
          if (myResponse.HasError)
            reject(myResponse.ErrorsArray);
          else
            resolve(null);
        });
    })
  }
}

export function filterIsNotNull() {
  return filter((x: any) => x);
}

export type Port = {
  Code: string
  CountryId: string
  CountryName: string
  Name: string
}

export type Carrier = {
  Prefix: string
  Name: string
  AIRLINE_ID: string
}

export type Incoterm = {
  Name: string
  PTERMID: string
}

export type SpecialService = {
  SERVLEVEL_ID: string
  Name: string
}

