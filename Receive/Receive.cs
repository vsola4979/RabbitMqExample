using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Receive.Domain.DTOs;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//declarando exchange
channel.ExchangeDeclare("carrosNovos", ExchangeType.Direct);
channel.ExchangeDeclare("carrosVelhos", ExchangeType.Direct);

var carrosNovosFila = channel.QueueDeclare().QueueName;
var carrosVelhosFila = channel.QueueDeclare().QueueName;

channel.QueueBind(carrosNovosFila, "carrosNovos", "");
channel.QueueBind(carrosVelhosFila, "carrosVelhos", "");

Console.WriteLine("Aguardando novas mensagens das Exchanges");



var consumerFirst = new EventingBasicConsumer(channel);
consumerFirst.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine("Mensagem recebida na carrosNovos: " + message);
};
channel.BasicConsume(carrosNovosFila, true, consumerFirst);


var consumerSecond = new EventingBasicConsumer(channel);
consumerSecond.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine("Mensagem recebida na carrosVelhos: " + message);
};
channel.BasicConsume(carrosVelhosFila, true, consumerSecond);

Console.ReadLine();