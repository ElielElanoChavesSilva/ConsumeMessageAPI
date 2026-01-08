using RabbitMQ.Client;

namespace ConsumerAPI.Conection
{
    public class RabbitMqConnection
    {
        private readonly IConnection _connection;

        public RabbitMqConnection(string hostName, string userName, string password)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public Task<IChannel> CreateChannelAsync()
        {
            return _connection.CreateChannelAsync();
        }
    }
}
