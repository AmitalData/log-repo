import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class ClientSearchResponseData extends INF_MSG_GenericResponseData {
    public Message: string;
    public CanContinue: boolean;
    public ResponseStatusXML: string;
}
