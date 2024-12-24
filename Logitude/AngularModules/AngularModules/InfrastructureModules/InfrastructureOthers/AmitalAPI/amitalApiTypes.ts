export type FieldData = {
    label: string;
    name: string;
};

export type MoreParam = (FieldData & {
    schemaId: string;
});
