# About

Renamed bad variable names at 2 classes according:

- 6.1. abstraction level, where class or method works => 5 examples

- 6.2. computer science terms => 4 examples

- 6.3. it's context (class, method/function) => 3 examples

- 6.4. it's length (the best length is 8-20 chars) => 5 examples

~~~
// example format

// 6.X (Y)
// <old name> - <new name>
code usage
~~~

### [RabbitManager class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames2/RabbitManager.cs)
~~~
public class RabbitManager : IDisposable
{
    // 6.2 (1)
    // Host - MessageBrokerHost
    private const string MessageBrokerHost = "localhost";
        
    // 6.2 (2)
    // Queue - MessagesQueueName
    private const string MessagesQueueName = "dev.queue";
    
    // 6.4 (1)
    // error - ErrorMessage
    private const string ErrorMessage = "RabbitManager is not connected to host.";
    
    ...
    
    public void Connect()
    {
        ... 
        
        // 6.1 (1)
        // factory - connectionFactory
        var connectionFactory = new ConnectionFactory() {HostName = MessageBrokerHost};
        
        ...
    
        // 6.1 (2)
        // _connection - _messageBrokerConnection
        _messageBrokerConnection = connectionFactory.CreateConnection();
        
        ...
        
        // 6.1 (3)
        // _channel - _messageBrokerChannel
        _messageBrokerChannel = _messageBrokerConnection.CreateModel();
        
        ...
        
        // 6.1 (4)
        // consumer - messageBrokerConsumer
        var messageBrokerConsumer = new EventingBasicConsumer(_messageBrokerChannel);
        
        messageBrokerConsumer.Received += (model, ea) =>
        {
           // 6.2 (3)
           // body - bodyMessageBytes
           byte[] bodyMessageBytes = ea.Body.ToArray();
                
           // 6.2 (4)
           // message - bodyMessageText
           string bodyMessageText = Encoding.UTF8.GetString(bodyMessageBytes);
           
           ...
           
        };
        
        ...
        
        // 6.1 (5)
        // old name: _connected - _isConnectedToHost
        _isConnectedToHost = true;
    }
    
    ...
    
}
~~~

### [RequestHandler class](https://github.com/novikov-ai/clean-code/blob/master/CleanCode/CleanCode/VariableNames2/RequestHandler.cs)
~~~
public class RequestHandler : IDisposable
{
    // 6.3 (1)
    // _client - _httpClient
    private readonly HttpClient _httpClient;
    
    // 6.4 (2)
    // AppId - ApplicationId
    private const string ApplicationId = "611cd9aa3d908b7172086a05";
    
    ...
    
    public async Task<string> GetRequest(string apiUrl, string endpoint)
    {
        ...
            
        try
        {
            // 6.3 (2) 
            // response - responseMessage
            HttpResponseMessage responseMessage = await _client.GetAsync($"{apiUrl}{endpoint}");
                    
            ...
        }
            
        ...
    }
    
    public async Task<T> PostRequest<T>(string apiUrl, ICreate newObject,
            string endpoint)
    {
        ...

        try
        {
             // 6.3 (3)
             // serialized - serializedObject
             var serializedObject = JsonConvert.SerializeObject(newObject);
             
             // 6.4 (3)
             // content - postingContent
             var postingContent = new StringContent(serializedObject, Encoding.UTF8,
                    "application/json");

             // 6.4 (4)
             // response - responseMessage
             var responseMessage = await _httpClient.PostAsync($"{apiUrl}{endpoint}", postingContent);
             
             ...

             // 6.4 (5) choosing name according to length
             // result - responseResult
             var responseResult = await responseMessage.Content.ReadAsStringAsync();
             
             ...
        }
        
        ...
    }
        
    ...
}
~~~