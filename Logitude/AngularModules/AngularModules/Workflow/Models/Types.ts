export type NodeType =
    "startNode" |
    "conditionNode" |
    "loopNode" |
    "setValueNode" |
    "declareVariableNode" |
    "collectionFilterNode" |
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

export type ExpressionVariable = {
    code: string,
    type: string
};

export type ExpressionValue = {
    expression: string,
    variables: ExpressionVariable[]
};

export type Entity = {
    Code: string,
    Name: string
};

export type ChildEntity = {
    Code: string,
    Name: string,
    ParentEntityCode: string,
    ChildField: string
};