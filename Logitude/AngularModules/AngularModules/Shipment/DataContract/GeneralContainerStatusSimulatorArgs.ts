export class GeneralContainerStatusSimulatorArgs {
    public Data: string;
    public Success: boolean;
    public Errors: string[] = [];
    public IsFromContainer: boolean;
    public ShipmentId: string;
    public ContainerNumber: string;
    public CarrierId: string;
}