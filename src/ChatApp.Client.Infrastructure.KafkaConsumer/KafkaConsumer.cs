namespace ChatApp.Client.Infrastructure.KafkaConsumer;

using Confluent.Kafka;

public abstract class KafkaConsumer<TContract> : IKafkaConsumer<string, TContract> where TContract : class
{
    private readonly IConsumer<string, TContract> _consumer;
    private readonly CancellationTokenSource _cts;

    public event Action<Message<string, TContract>> OnMessageReceived;
    public event Action<string> OnError;
    public event Action<IConsumer<string, TContract>, Error> OnKafkaError;

    protected KafkaConsumer(
        KafkaConsumerConfig consumerConfig,
        IDeserializer<string> keyDeserializer,
        IDeserializer<TContract> valueDeserializer)
    {
        _cts = new CancellationTokenSource();

        _consumer = new ConsumerBuilder<string, TContract>(consumerConfig.ConsumerConfig)
            .SetKeyDeserializer(keyDeserializer)
            .SetValueDeserializer(valueDeserializer)
            .SetErrorHandler(this.OnKafkaError)
            .Build();

        _consumer.Subscribe(consumerConfig.MessageTopic);
    }

    public void Start()
    {
        Task.Factory.StartNew(() =>
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    var result = _consumer.Consume(500);
                    if (result != null)
                    {
                        this.HandleMessage(result);
                    }
                }
            }
            catch (Exception ex)
            {
                OnKafkaError.Invoke(_consumer, new Error(ErrorCode.BrokerNotAvailable, ex.Message));
            }
        }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    private void HandleMessage(ConsumeResult<string, TContract> consumeResult)
    {
        try
        {
            ProcessMessage(consumeResult);
        }
        catch (Exception ex)
        {
            OnError.Invoke(ex.Message);
        }
    }

    private void ProcessMessage(ConsumeResult<string, TContract> consumeResult)
    {
        OnMessageReceived.Invoke(consumeResult.Message);
    }

    public void Dispose()
        => _consumer.Dispose();
}