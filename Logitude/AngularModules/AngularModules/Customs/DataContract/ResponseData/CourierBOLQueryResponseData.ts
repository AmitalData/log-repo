import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class CourierBOLQueryResponseData extends INF_MSG_GenericResponseData {

    public ResponseStatusXML: string;
    public CourierBOLDetailsList: Array<CourierBOLDetailsResult>;
}

export class CourierBOLDetailsResult {

    public cargoIdentifierKey1: string;
    public cargoIdentifierKey2: string;
    public cargoIdentifierKey3: string;
    public cargoIdentifierType: string;

}

