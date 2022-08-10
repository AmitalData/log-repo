export class WhereFilter {
    MainEntityName: string;
    Field: string;
    FilterType: WhereFilterType;
    SecondaryEntityName: string;
    Value: any;
}

export class TreeFilter extends WhereFilter {
    OperatorType: TreeFilterType;
    AdditionalFilters: TreeFilter[];
}

export enum TreeFilterType {
    None,
    And,
    Or
}

export enum WhereFilterType {
    None,
    Equal,
    NotEqual,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
    Contains,
    NotContains,
    StartsWith,
    NotStartsWith,
    EndsWith,
    NotEndsWith,
    Any,
    NotAny,
    IsNull,
    IsNotNull,
    IsEmpty,
    IsNotEmpty,
    IsNullOrEmpty,
    IsNotNullOrEmpty
}
