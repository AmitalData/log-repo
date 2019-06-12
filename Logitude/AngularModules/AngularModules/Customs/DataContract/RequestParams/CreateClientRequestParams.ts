import {GenericRequestParams} from './GenericRequestParams';


export class CreateClientRequestParams extends GenericRequestParams  {
    public IsExternalId: boolean;

    public FullName: string;

    public ClientTypeSpecificCode: string;

    public IsActive: boolean;

    public LocalFirstName: string;

    public LocalLastName: string;
    public  LocalCorporationName : string;
    
    public EnglishFirstName: string;
    
    public EnglishLastName: string;
    
    public EnglishCorporationName: string;
    
    public BirthDate: Date;
    
    public GenderCode: string;

    public DunsNumber: string;


    public PassportNumber: string;
    
    public PassportCountryCode: string;
    
    public PassportTypeCode: string;

    public PassportFirstName: string;

    public PassportLastName: string;

    public EnglishBirthPlace: string;

    public EnglishFatherName: string;


    public PassportExpirationDate: Date;

    public PassportIssueDate: Date;

    public ClientAddresses: ClientAdressParams[];

    public  ClientTypeSpecificName: string;

    public  PassportCountryName: string;

    public  GenderName: string;


    public  PassportTypeName: string;
    public  IsImporter: boolean;


    public  IsExporter: boolean;

    public  ConcurrencyGUID: string;


    public  NewConcurrencyGUID: string;

    public  FacilitationTypeCode: string;

    public ClientDrivingLicenses: ClientDrivingLicenseParams[];

    public NationalIdentificationNumber: string;
}

export class ClientAdressParams {

    public ContactStateCode:string;

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

    private localSecondLine: string;

    public LocalSecondLine: string;


    public LocalStreetName: string;

    public LocalHouseLetter: string;


    public LocalEntrance: string;

    public EnglishCountryCode: string;

    public EnglishSubCountryCode: string;


    public EnglishCityName: string;


    public EnglishMainAddressLine: string;


    public EnglishPostalCode: string;


    public LocalApartment: number;

    public LocalPOBox: string;

    public LocalPostalCode: string;


    public LocalHouseNumber: string;

    public ClientAddressCommunicationType: ClientAddressCommunicationType[];

       public  ContactStateName: string;

       public  AddressTypeName: string;

       public  AddressPurposeName: string;


       public  ContactRoleTypeName: string;

       private  authorizedSignerPermit1Name: string;


       public  AuthorizedSignerPermit1Name: string;


       public  AuthorizedSignerPermit2Name: string;

       public  AuthorizedSignerPermit3Name: string;


       public  LocalCityName: string;

       public  EnglishCountryName: string;

       public  EnglishSubCountryName: string;

       public  CustomAddressCode: string;


       public  AddressId: string;
}

export class ClientAddressCommunicationType {

    public CommunicationTypeCode: string;

    public CommunicationAddress:string;

    public CommunicationTypeName: string;
}

export class ClientDrivingLicenseParams {

    public DrivingLicenseNumber: string;
    public DriverLicenseValidityDate: Date;
    public DrivingLicenseCountryID: string;
    public ClientDrivingLicenseTypes: ClientDrivingLicenseTypeParams[];
}

export class ClientDrivingLicenseTypeParams {

    public DriversLicenseTypeCode: string;
}
