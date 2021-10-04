export class RegexSelectors {

    public static LogLovId(entityType: string): string {

        return "#LogLov_" + RegexSelectors.LogLovLocation(entityType) + "_" + entityType + "Id";
    }

    public static Edit(entityType: string): string {

        return "[data-cy='Edit" + entityType + "']"
    }

    private static LogLovLocation(entityType) {
        if (entityType == "Partner") {
            return "ARInvoice"
        }
        return "ShipmentReceivable"
    }

    public static NewWizardButton(name: string): string {
        return "#NewButton_" + name;
    }

}