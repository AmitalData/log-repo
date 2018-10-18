import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class MasterBOLFeedBackResponseData extends INF_MSG_GenericResponseData {

    public InternalCargosList: Array<InternalCargoResult>;
}

export class InternalCargoResult {

    public CargoIdentiferTypeId: string;
    public cargoIdentifierKey1: string;
    public cargoIdentifierKey2: string;
    public cargoIdentifierKey3: string;
    public TotalWheight: string;
    public Submitter: string;

}