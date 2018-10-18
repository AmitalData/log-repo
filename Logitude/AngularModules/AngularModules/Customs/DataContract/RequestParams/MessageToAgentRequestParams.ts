import {GenericRequestParams} from './GenericRequestParams';

export class MessageToAgentRequestParams extends GenericRequestParams {
    public NotificationId: string;
    public DeclarationId: string;
}