import {GenericRequestParams} from './GenericRequestParams';

export class AddAddressContactForClientRequestParams extends GenericRequestParams  {

  

    public ClientId: string;
    public ExternalId:string;
    public PassportNumber:string;
    public PassportTypeCode: string;
    public PassportCountryCode: string;
    public OperationType: OperationTypes;
    public AddressCode: ClientAddress;

  
}

        export enum OperationTypes {
            Add = 1,
            Update = 2,
            Delete = 3,
        }

export class ClientAddress{
    //Address Details
    public AddressId: string;
    public AddressContactState: string;
    public AddressTypeCode: string;
    public AddressPurposeCode: string;
    public IsPalestinianCity: boolean;
    public IsHebrewAddress: boolean;
    public BranchName: string;
    public ContactIdentifier: string;
    public ContactFirstName: string;
    public ContactLastName: string;
    public ContactRoleTypeCode: string;
    public AuthorizedSignerPermit1: string;
    public AuthorizedSignerPermit2: string;
    public AuthorizedSignerPermit3: string;
    public LocalCityCode: string;
    public LocalSecondLine: string;
    public LocalStreetName: string;
    public LocalHouseLetter: string;
    public LocalEntrance: string;
    public EnglishCountryCode: string;
    public EnglishSubCountryCode: string;
    public EnglishCityName: string;
    public EnglishMainAddressLine: string;
    public EnglishPostalCode: string;
    public LocalApartment: string;
    public LocalPOBox: string;
    public LocalPostalCode: string;
    public LocalHouseNumber: string;
    public CustomAddressCode: string;

    //Communication Details
    public ClientsAddressCommunication: ClientsAddressCommunicationResult[];



}

export class ClientsAddressCommunicationResult  {
    public CommunicationAddress: string;
    public CommunicationType: string;
    public CommunicationTypeName: string;

}
        