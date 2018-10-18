import {GenericRequestParams} from './GenericRequestParams';



export class CollateralRequestParams extends GenericRequestParams {

    public CustomCollateralId: string;
    public CustomsCollateralsAnswers: CustomsCollateralsAnswerParams[];
    }

export class CustomsCollateralsAnswerParams {
    public CustomsCollateralId: string;
    public LineNumber: number;
    public Tenant: number;
    public AnswerEntityTypeCode: string;
    public AllocatedAmount: number;
    public Remarks: string;
    public  CustomsTapgFile: string;
    public  CustomsNumeral: string;
    public  AnswerForCollateralStatusCode: string;
    public Errors: string;
    public  AnswerEntityType: string;
    public AnswerForCollateralStatus: string;

    }
