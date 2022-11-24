export type NodeType =
    "startNode" |
    "conditionNode" |
    "loopNode" |
    "setValueNode" |
    "declareVariableNode" |
    "createRecordNode" |
    "updateRecordNode" |
    "getRecordNode" |
    "sendEmailNode" |
    "endNode" |
    "connectorNode" |
    "repeaterNode" |
    "dummyNode" |
    "labelNode" |
    null;

export type ConditionDisabled = null | "d,f,o";

export type ShowVariables = {
    ShowRecordsVariables: boolean,
    ShowDeclaredVariables: boolean,
    ShowRecordsCollectionVariables: boolean,
    ShowDeclaredCollectionVariables: boolean
};