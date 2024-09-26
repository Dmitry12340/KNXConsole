using Knx.Falcon;
using Knx.Falcon.Configuration;
using Knx.Falcon.Sdk;

namespace KNXTest
{
    public class TestTunneling
    {
        private static async Task Main()
        {
            CancellationToken cancellationToken = new CancellationToken();

            //Строка подключения
            string connectionString = "Type=IpTunneling;HostAddress=192.168.8.137;IpPort=3671;ProtocolType=0;UseNat=True";

            //Экземпляр типа подключения к KNX
            var value = IpUnicastConnectorParameters.FromConnectionString(connectionString);

            //Объект взаимодействия с шиной
            KnxBus bus = new KnxBus(value);


            //Открываем соединение
            bus.Connect();
            Console.WriteLine(bus.ConnectionState.ToString());


            //Значение которое будем передовать в групповой адрес
            GroupValue groupValueBool = new GroupValue(false);
            var groupValue = new GroupValue(0);


            //Групповой адрес
            GroupAddress groupAddress = new GroupAddress("0/0/4");

            //Отправляем значение в групповой адрес
            //bus.WriteGroupValue(groupAddress, groupValue);







            //Запрос на получение значения адреса
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(3000);
            MessagePriority massagePriority = MessagePriority.Low;
            var request = await bus.ReadGroupValueAsync(groupAddress, timeSpan, massagePriority, cancellationToken);

            var val = request.Value;

            for (int i = 0; i < val.Length; i++)
            {
                Console.WriteLine(val[i]);
            }



            //Регистрация обработчика событий, который слушает все сообщения на шине
            bus.GroupMessageReceived += (s, e) =>
            {
                if (e.EventType == GroupEventType.ValueWrite || e.EventType == GroupEventType.ValueWrite)
                    Console.WriteLine("{0} {1} = {2}", e.EventType, e.DestinationAddress, e.Value);
            };



            Console.WriteLine("Нажмите любую клавишу что бы завершить программу");
            Console.ReadLine();


            //Закрываем соединение
            bus.Dispose();
            Console.WriteLine(bus.ConnectionState.ToString());




        }
    }
}

