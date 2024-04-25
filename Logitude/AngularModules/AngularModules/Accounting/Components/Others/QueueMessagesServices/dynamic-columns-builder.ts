export function buildColumns<T>(classType: new () => T): any[] {
    // Get the properties of the class using keyof T
    const properties = Object.keys({ ...classType.prototype }) as (keyof T)[];

    const columns = properties.map(prop => ({
        FieldName: prop,
        DataTypeCode: getTypeCode(null),
        Display: String(prop),
        Styles: { width: '100px' },
        IsCustomTemplate: true
    }));

    return columns;
}

function getTypeCode(value: any): string {
    if (typeof value === 'string') {
        return 'string';
    } else if (typeof value === 'number') {
        return 'Number';
    } else if (value instanceof Date) {
        return 'DateTime';
    } else {
        return 'unknown';
    }
}
