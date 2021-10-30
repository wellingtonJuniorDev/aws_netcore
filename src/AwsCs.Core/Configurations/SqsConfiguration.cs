namespace AwsCs.Core.Configurations
{
    public class SqsConfiguration
    {
        public string Region { get; set; }
        public string FirstQueueName { get; set; }
        public string FirstTopicName { get; set; }
        public string SecondQueueName { get; set; }
        public string SecondTopicName { get; set; }
    }
}
