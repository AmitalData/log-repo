
export interface Ns1ResponseHeader {
  xmlnsns1: string;
  ns1CorrelationId: string;
  ns1Status: string;
  ns1ErrorDescription: string;
  ns1ErrorCode: string;
  ns1ExternalId: string;
}

export interface ResponseContentHeader {
  TransmitionDateTime: string;
  ApplicationID: string;
  Remark: string;
}

export interface CargoIdentifier {
  cargoIdentifierType: string;
  cargoIdentifierKey1: string;
  cargoIdentifierKey2: string;
  cargoIdentifierKey3: string;
}

export interface Exception {
  ExceptionLevel: string;
  ExeptionType: string;
  ExceptionParms: string;
  ExeptionDescription: string;
  EnglishDescription: string;
  BindingXpathField: string;
}

export interface MNMSG2791ExportDeliveryAnswerMessage {
  ResponseContentHeader: ResponseContentHeader;
  CargoIdentifier: CargoIdentifier;
  Exception: Exception[] | Exception;
}

export interface Body {
  MN_MSG2791_ExportDeliveryAnswerMessage: MNMSG2791ExportDeliveryAnswerMessage;
}

export interface Ns0ESBResponse {
  xmlnsns0: string;
  ns1ResponseHeader: Ns1ResponseHeader;
  Body: Body;
}

export interface ExportStorageMsgXML {
  ns0ESBResponse: Ns0ESBResponse;
}


export interface UIMessage {
  $id: string;
  Code: string;
  EnglishName: string;
  Inactive: boolean;
  LocalName: string;
  SearchFields: string;
  Sort?: any;
}



