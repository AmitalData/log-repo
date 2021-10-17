import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class DeclarationStatusResponseData extends INF_MSG_GenericResponseData {

    public ResponseStatusXML: string;
    public DeclarationID: string;
    public DeclarationStatusColor: string;
    public DeclarationVersion: string;
    public DeclarationStatusCode: string;
    public DeclarationStatusText: string;
    public LogisticStatusCode: string;
    public LogisticStatusText: string;
    public TaxationDateTime: string;
    public ReleaseDateTime: string;
    public DeclarationOfficeID: string;
    public DeclarationOfficeText: string;
    public FinancialStatusCode: string;
    public FinancialStatusText: string;
    public SubmitDateTime: string;
    public WarningMessage: string;
    public HandeledWroker: string;

    public AvailabiltyQuantitiesList: Array<AvailabiltyLogDeclarationCargoQuantities>;
    public MultiDeclaration: Array<MultiDeclaration>;

}


export class AvailabiltyLogDeclarationCargoQuantities {

    public CargoIdentifierTypeCode: string;
    public CargoIdentifierTypeText: string;
    public CargoIdentifierKey1: string;
    public CargoIdentifierKey2: string;
    public CargoIdentifierKey3: string;
    public IsSecondRound: boolean;
    public CargoPackageTypeCode: string;
    public CargoPackageTypeText: string;
    public CargoPackageQuantity: string;
    public CargoPackageWeight: string;
    public CargoWeightMeasurementUnitCode: string;
    public CargoWeightMeasurementUnitText: string;
    public DeclarationPackageTypeCode: string;
    public DeclarationPackageTypeText: string;
    public DeclarationPackgeQuantity: string;
    public DeclarationPackageWeight: string;
    public DeclarationWeightMeasurementUnitCode: string;
    public DeclarationWeightMeasurementUnitText: string;
    public ComparisonResult: string;
}


export class MultiDeclaration extends INF_MSG_GenericResponseData {

    public ResponseStatusXML: string;
    public DeclarationID: string;
    public DeclarationStatusColor: string;
    public DeclarationVersion: string;
    public DeclarationStatusCode: string;
    public DeclarationStatusText: string;
    public LogisticStatusCode: string;
    public LogisticStatusText: string;
    public TaxationDateTime: Date;
    public ReleaseDateTime: Date;
    public DeclarationOfficeID: string;
    public DeclarationOfficeText: string;
    public FinancialStatusCode: string;
    public FinancialStatusText: string;
    public SubmitDateTime:  Date;
    public WarningMessage: string;
    public HandeledWroker: string;
}
