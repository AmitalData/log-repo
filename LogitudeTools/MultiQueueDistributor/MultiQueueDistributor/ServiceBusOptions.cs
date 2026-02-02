namespace MultiQueueDistributor
{
    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SourceQueue { get; set; } = string.Empty;
        public string QueueNamePrefix { get; set; } = string.Empty;
        public string RoutingProperty { get; set; } = string.Empty;

        public bool Enabled { get; set; } = true;
        public bool DryRun { get; set; } = false;
        public int MaxMessages { get; set; } = 0;
        public bool DeadLetterOnMissingRoutingKey { get; set; } = false;
        public bool AutoCreateQueues { get; set; } = true;
    }
}
