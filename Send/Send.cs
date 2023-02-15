using Newtonsoft.Json;
using RabbitMQ.Client;
using Send.Domain.DTOs;
using System.Text;


List<VeiculosDTO> veiculosEnviar = new List<VeiculosDTO>();

//Lendo um arquivo com alguns dados de exemplo para criar um cenario de uso basico de rabbitMq
string workingDirectory = Environment.CurrentDirectory;
string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
using (StreamReader reader = new StreamReader(projectDirectory + @".\Arquivo\EnvioBase.csv"))
{
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();
        string[] values = line.Split(';');

        VeiculosDTO message = new VeiculosDTO
        {
            Carro = values[0],
            Cor = values[1],
            AnoVeiculo = values[2],
            NomeProprietario = values[3],
            CpfProprietario = values[4]
        };

        veiculosEnviar.Add(message);
    }
}

//Criando uma conexão com o rabbitMq e declarando as Exchanges 

ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("carrosNovos", ExchangeType.Direct);
channel.ExchangeDeclare("carrosVelhos", ExchangeType.Direct);

foreach (var item in veiculosEnviar)
{
    if (Convert.ToInt32(item.AnoVeiculo) > 2015)
    {
        string carrosNovos = JsonConvert.SerializeObject(item);
        byte[] body = Encoding.UTF8.GetBytes(carrosNovos);
        channel.BasicPublish(exchange: "carrosNovos", routingKey: "", basicProperties: null, body: body);
        Console.WriteLine($"O Carro {item.Carro} foi enviado para a exchange carrosNovos por ter sido fabricado apos 2015");
    }
    else
    {
        string carrosVelhos = JsonConvert.SerializeObject(item);
        byte[] body = Encoding.UTF8.GetBytes(carrosVelhos);
        channel.BasicPublish(exchange: "carrosVelhos", routingKey: "", basicProperties: null, body: body);
        Console.WriteLine($"O Carro {item.Carro} foi enviado para a exchange carrosVelhos por ter sido fabricado até 2015");
    }

    Console.Write("\r\n");

}

Console.ReadLine();