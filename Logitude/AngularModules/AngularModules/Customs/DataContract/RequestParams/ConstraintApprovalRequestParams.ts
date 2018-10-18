import {RequestParamsBase} from './RequestParamsBase';

export class ConstraintApprovalRequestParams extends RequestParamsBase{
    ConstraintNumber: string;
    ConstraintTypeName: string;
    ConstraintStatusName: string;
    AgentExplanation: string;
    DeclarationId: string;
    ApprovalDecision: string;
}