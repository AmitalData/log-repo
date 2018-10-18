import {RequestParamsBase} from './RequestParamsBase';

export class BankAccountToRefundRequestParams extends RequestParamsBase {

    public FileType: string;
    public FileNumber: string;
    public Numeral: number;
    public IdentifierType: string;
    public IdentifierCode: string;
    public CountryCode: string;
    public InternalBank: string;
    public BankCode: string;
    public AccountBranch: string;
    public AccountNumber: string;
    public AccountCurrency: string;
}

