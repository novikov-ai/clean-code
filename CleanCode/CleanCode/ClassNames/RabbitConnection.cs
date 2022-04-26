using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CleanCode.ClassNames
{
    // 3.1 (1)
    // RabbitManager - RabbitConnection
    public class RabbitConnection : IDisposable
    {
        private const string MessageBrokerHost = "localhost";
        
        private const string MessagesQueueName = "dev.queue";

        private const string ErrorMessage =
            "RabbitManager is not connected to host.";

        private bool _isConnectedToHost = false;
        
        private IModel _messageBrokerChannel;
        private IConnection _messageBrokerConnection;

        public event EventHandler<string> MessageReceived;

        public void Connect()
        {
            var connectionFactory = new ConnectionFactory() {HostName = MessageBrokerHost};

            _messageBrokerConnection = connectionFactory.CreateConnection();
            
            _messageBrokerChannel = _messageBrokerConnection.CreateModel();

            _messageBrokerChannel.QueueDeclare(MessagesQueueName, false, false, false, null);

            var messageBrokerConsumer = new EventingBasicConsumer(_messageBrokerChannel);
            messageBrokerConsumer.Received += (model, ea) =>
            {
                byte[] bodyMessageBytes = ea.Body.ToArray();
                
                string bodyMessageText = Encoding.UTF8.GetString(bodyMessageBytes);
                
                MessageReceived?.Invoke(this, bodyMessageText);
            };
            
            _messageBrokerChannel.BasicConsume(MessagesQueueName, true, messageBrokerConsumer);

            _isConnectedToHost = true;
        }

        public void SendMessage(string message)
        {
            if (!_isConnectedToHost)
            {
                throw new Exception(ErrorMessage);
            }

            var bodyMessageBytes = Encoding.UTF8.GetBytes(message);
             _messageBrokerChannel.BasicPublish("", MessagesQueueName, null, bodyMessageBytes);
        }

        public void Dispose()
        {
            _messageBrokerChannel?.Dispose();
            _messageBrokerConnection?.Dispose();
        }
    }
}