using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CleanCode.VariableNames2
{
    public class RabbitManager : IDisposable
    {
        // [6.2] choosing name with computer science term
        // old name: Host
        // new name: MessageBrokerHost
        private const string MessageBrokerHost = "localhost";
        
        // [6.2] choosing name with computer science term
        // old name: Queue
        // new name: MessagesQueueName
        private const string MessagesQueueName = "dev.queue";

        // [6.4] choosing name according to length
        // old name (5 chars): error
        // new name (14 chars): ErrorMessage
        private const string ErrorMessage =
            "RabbitManager is not connected to host.";

        private bool _isConnectedToHost = false;
        
        private IModel _messageBrokerChannel;
        private IConnection _messageBrokerConnection;

        public event EventHandler<string> MessageReceived;

        public void Connect()
        {
            // [6.1] choosing name according appropriate level of abstraction
            // old name: factory
            // new name: connectionFactory
            var connectionFactory = new ConnectionFactory() {HostName = MessageBrokerHost};

            // [6.1] choosing name according appropriate level of abstraction
            // old name: _connection
            // new name: _messageBrokerConnection
            _messageBrokerConnection = connectionFactory.CreateConnection();
            
            // [6.1] choosing name according appropriate level of abstraction
            // old name: _channel
            // new name: _messageBrokerChannel
            _messageBrokerChannel = _messageBrokerConnection.CreateModel();

            _messageBrokerChannel.QueueDeclare(MessagesQueueName, false, false, false, null);

            // [6.1] choosing name according appropriate level of abstraction
            // old name: consumer
            // new name: messageBrokerConsumer
            var messageBrokerConsumer = new EventingBasicConsumer(_messageBrokerChannel);
            messageBrokerConsumer.Received += (model, ea) =>
            {
                // [6.2] choosing name with computer science term
                // old name: body
                // new name: bodyMessageBytes
                byte[] bodyMessageBytes = ea.Body.ToArray();
                
                // [6.2] choosing name with computer science term
                // old name: message
                // new name: bodyMessageText
                string bodyMessageText = Encoding.UTF8.GetString(bodyMessageBytes);
                
                MessageReceived?.Invoke(this, bodyMessageText);
            };
            
            _messageBrokerChannel.BasicConsume(MessagesQueueName, true, messageBrokerConsumer);

            // [6.1] choosing name according appropriate level of abstraction
            // old name: _connected
            // new name: _isConnectedToHost
            _isConnectedToHost = true;
        }

        public void SendMessage(string message)
        {
            if (!_isConnectedToHost)
            {
                throw new Exception(ErrorMessage);
            }

            // [6.2] choosing name with computer science term
            // old name: body
            // new name: bodyMessageBytes
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