import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class CreditQueryResponseData extends INF_MSG_GenericResponseData {

    public ResponseStatusXML: string;
    public BalanceDetailsList: Array<BalanceDetailsResult>;
    public BankAccountsList: Array<BankAccountsResult>;
}

export class BalanceDetailsResult {

    public FreeBalance: string;
    public TemporaryCeiling: string;
    public UsedBalance: string;

}

export class BankAccountsResult {

    public BankAccount: string;
    public UsedBalanceForBankAccount: string;
}

