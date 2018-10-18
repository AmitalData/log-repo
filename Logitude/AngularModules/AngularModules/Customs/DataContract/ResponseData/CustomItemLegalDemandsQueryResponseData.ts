//import { ResponseDataBase } from './ResponseDataBase';
import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';

export class CustomItemLegalDemandsQueryResponseData extends INF_MSG_GenericResponseData {
    public CustomsLegalDemandsList: Array<CustomItemLegalDemandsQueryResult>;
    public CountriesExclusionList: Array<CountriesExclusionResult>;
}

export class CustomItemLegalDemandsQueryResult {
    public AuthoritiyID: string;
    public AuthoritiyName: string;
    public CertificateTypeId: string;
    public CertificateTypeName: string;
    public FullClassification: string;
    public InterConditionsRelationshipName: string;
    public IsAuthorityConformationDetailsExist: boolean;
    public IsCarnetIncluded: string;
    public IsPersonalImportIncluded: string;
    public RegularityPublicationName: string;
    public RegularityRequirementId: string;
    public RequirementGoodsDescription: string;
    public RequirementSourceName: string;
    public TextualCondition: string;
    public TrNumber: string;
    public ConfirmationWebAddressList: Array<ConfirmationWebAddressResult>;
}
export class ConfirmationWebAddressResult {
    public ConfirmationTypeWebAddress: string;
    public FormTypeForConfirmation: string;
    }

export class CountriesExclusionResult {
    public CountryId: string;
    public CountryName: string;
    public RegularityRequirementId: string;
    }
