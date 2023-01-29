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

export type FlowVariablesTreeListProperties = {
    ShowRecordsVariables: boolean,
    ShowDeclaredVariables: boolean,
    ShowRecordsCollectionVariables: boolean,
    ShowDeclaredCollectionVariables: boolean,
    OnlyCurrentLoopItemVariables: boolean,
    IsObjectVariableSelectable: boolean
};

export type ExpressionVariable = {
    code: string,
    type: string,
    variableUsedFrom: string | null
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
    ParentEntityCode: string
};

export type EntitiesType = "all" | "parent" | "child";