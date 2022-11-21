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