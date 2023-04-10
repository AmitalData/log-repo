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

export type ConditionDisabled = null | "d,f,o" | "d,o,v";

export type SetValueDisabled = null | "d,f";

export type FlowVariablesTreeListProperties = {
    ShowRecordsVariables?: boolean,
    ShowDeclaredVariables?: boolean,
    ShowRecordsCollectionVariables?: boolean,
    ShowDeclaredCollectionVariables?: boolean,
    ShowGlobalVariables?: boolean,
    OnlyCurrentLoopItemVariables?: boolean,
    IsObjectVariableSelectable?: boolean,
    IsNoChildrenObjectVariables?: boolean
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
    Name: string,
    IsCustom: boolean,
    IsChild: boolean,
    ParentEntity: string | null
};

export type EntitiesType = "all" | "parent" | "child";

export type GlobalVariable = {
    Code: string,
    Name?: string | null,
    Type: string,
    LookupType?: string | null,
    PicklistType?: string | null
};