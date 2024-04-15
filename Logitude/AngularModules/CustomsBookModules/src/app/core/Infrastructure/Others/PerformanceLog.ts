export class PerformanceLog {
    /*[Key]*/
    public Id: string;
    public LogDateTimeGMT: Date;
    public LogDateTimeLocal: Date;
    public Email: string;
    public ModelName: string;
    public MethodName: string;
    public MonitoringService: boolean;
    public ExecutionTime: number;
    public UserIP: string;
    public MethodParameters: string;
    public Tenant: number;
    public ServerTime: number;
}