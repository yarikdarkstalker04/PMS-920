using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Net.Sockets;
using System.Net;
using Modbus.Data;


namespace PMS_92
{
    class Program
    {
        static void Menu()
        {
            Console.Clear();
            Console.WriteLine("[1] - Создание файлов");
            Console.WriteLine("[2] - Загрузка файлов");
            Console.WriteLine("[3] - Выполнение функций");
            Console.WriteLine("[0] - Выход");
        }
        static PMS_92_Connection Create_json_connection(IPAddress ip, int tcp)
        {
            CONNECTION_JS connect_js = new CONNECTION_JS(ip.ToString(), tcp.ToString())
            {
                IP_JS = ip.ToString(),
                TCP_JS = tcp.ToString()
            };
            PMS_92_Connection connect = new PMS_92_Connection(ip, tcp);
            string objectSerialized = JsonSerializer.Serialize(connect_js);
            File.WriteAllText("PMS_92_Connection.json", objectSerialized);
            return connect;
        }
        static PMS_92_Unit Create_json_unit(List<ushort[]> rqst_cllct, byte adrs,string rqst_js)
        {
            UNIT_JS unit_js = new UNIT_JS(rqst_js.ToString(), adrs.ToString())
            {
                request_collection_js = rqst_js.ToString(),
                adres_js = adrs.ToString(),  
        };
            PMS_92_Unit unit = new PMS_92_Unit(rqst_cllct, adrs);
            string objectSerialized = JsonSerializer.Serialize(unit_js);
            File.WriteAllText("PMS_92_Unit.json", objectSerialized);
            return unit;
        }
        static PMS_92_Unit Load_unit_from_json()
        {
            if (!(File.Exists("PMS_92_Unit.json")))
            {
                Console.WriteLine("Файл 'PMS_92_Unit.json' отсутствует");
                return null;
            }
            else
            {
                string objectJsonFile = File.ReadAllText("PMS_92_Unit.json");
                UNIT_JS unitLoaded = JsonSerializer.Deserialize<UNIT_JS>(objectJsonFile);
                ushort rq1=0;
                ushort rq2=0;
                string number = "";
                string numbers = unitLoaded.request_collection_js;
                List<ushort[]> rqstcllc = new List<ushort[]>();
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (char.IsDigit(numbers[i]))
                    {
                        number += numbers[i];
                    }
                    else if (numbers[i] == ',')
                    {
                        rq1 = ushort.Parse(number);
                        number = "";
                    }
                    else
                    {
                        rq2 = ushort.Parse(number);
                        number = "";
                        rqstcllc.Add(new ushort[] {rq1,rq2});
                    }
                    

                }
                PMS_92_Unit unit = new PMS_92_Unit(rqstcllc,byte.Parse(unitLoaded.adres_js));
                return unit;
            }

        }
        static PMS_92_Connection Load_connection_from_json()
        {
            if (!(File.Exists("PMS_92_Connection.json")))
            {
                Console.WriteLine("Файл 'PMS_92_Connection.json' отсутствует");
                return null;
            }
            else
            {
                string objectJsonFile = File.ReadAllText("PMS_92_Connection.json");
                CONNECTION_JS connectionLoaded = JsonSerializer.Deserialize<CONNECTION_JS>(objectJsonFile);
                PMS_92_Connection connect = new PMS_92_Connection(IPAddress.Parse(connectionLoaded.IP_JS),int.Parse(connectionLoaded.TCP_JS));
                return connect;
            }
        }
        static void Main(string[] args)
        {
            bool not_exit = true;
            bool not_exit1;
            int punkt;
            string checkYN;
            int count;
            PMS_92_Connection connect = null;
            CONNECTION_JS connect_js = null;
            PMS_92_Unit unit = null;
            Menu();

            while (not_exit)
            {
                try
                {
                    punkt = Convert.ToInt16(Console.ReadLine());
                }
                catch
                {
                    continue;
                }

                switch (punkt)
                {
                    case 0:
                        not_exit = false;
                        break;
                    case 1:
                        not_exit1 = true;
                        while (not_exit1)
                        {
                            Console.Clear();
                            Console.WriteLine("[1] - Создание файла Connection");
                            Console.WriteLine("[2] - Создание файла Unit");
                            Console.WriteLine("[0] - Назад");
                            Int16 punkt1;
                            try
                            {
                                punkt1 = Convert.ToInt16(Console.ReadLine());
                            }
                            catch
                            {
                                continue;
                            }

                            switch (punkt1)
                            {
                                case 1:
                                    Console.Write("Ввести Соединение вручную?(Y/N) ");
                                    checkYN = Console.ReadLine();
                                    if (checkYN.ToLower() == "y")
                                    {
                                        Console.Write("Введите TCP: ");
                                        int TCP;
                                        try
                                        {
                                            TCP = int.Parse(Console.ReadLine());
                                            if (TCP < 100 && TCP > 11000)
                                            {
                                                continue;
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        Console.Write("Введите IP: ");
                                        IPAddress IP;
                                        try
                                        {
                                            IP = IPAddress.Parse(Console.ReadLine());
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        connect = Create_json_connection(IP, TCP);
                                    }
                                    else if (checkYN.ToLower() == "n")
                                    {
                                        IPAddress iip = IPAddress.Parse("127.0.0.1");
                                        connect_js = new CONNECTION_JS(iip.ToString(), "502")
                                        {
                                            IP_JS = iip.ToString(),
                                            TCP_JS = "502"
                                        };
                                        connect = new PMS_92_Connection(iip, 502);
                                        var objectSerialized = JsonSerializer.Serialize(connect_js);
                                        File.WriteAllText("PMS_92_Connection.json", objectSerialized);
                                    }
                                    else
                                    {
                                        continue;
                                    }


                                    break;
                                case 2:
                                    if (connect != null)
                                    {
                                        Console.Write("Введите количество запросов: ");

                                        List<ushort[]> rqst = new List<ushort[]>();
                                        string rqst_js = "";
                                        try
                                        {
                                            count = int.Parse(Console.ReadLine());
                                            if (count < 1)
                                            {
                                                continue;
                                            }
                                            ushort rq1;
                                            ushort rq2;
                                            for (int i = 0; i < count; i++)
                                            {
                                                Console.WriteLine("Запрос №{0}", i + 1);
                                                Console.Write("Введите стартовый регистр: ");
                                                rq1 = ushort.Parse(Console.ReadLine());
                                                Console.Write("Введите количество регистров: ");
                                                rq2 = ushort.Parse(Console.ReadLine());

                                                rqst.Add(new ushort[] { rq1, rq2 });
                                                rqst_js += rq1.ToString() + "," + rq2.ToString() + ":";

                                            }
                                            byte adrs;
                                            Console.Write("Введите сетевой адрес: ");
                                            adrs = byte.Parse(Console.ReadLine());
                                            unit = Create_json_unit(rqst, adrs, rqst_js);
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Сначала заполните Соединение");
                                        Console.ReadKey();
                                        Menu();
                                        not_exit1 = false;
                                    }
                                    break;
                                case 0:
                                    Menu();
                                    not_exit1 = false;
                                    break;
                                default:
                                    break;

                            }
                        }
                        break;
                    case 2:
                        not_exit1 = true;
                        while (not_exit1)
                        {
                            Console.Clear();
                            Console.WriteLine("[1] - Загрузка файлов Connection и Unit");
                            Console.WriteLine("[2] - Загрузка файла Connection");
                            Console.WriteLine("[3] - Загрузка файла Unit");
                            Console.WriteLine("[0] - Назад");
                            Int16 punkt2;
                            try
                            {
                                punkt2 = Convert.ToInt16(Console.ReadLine());
                            }
                            catch
                            {
                                continue;
                            }
                            switch (punkt2)
                            {
                                case 1:
                                    connect = Load_connection_from_json();
                                    unit = Load_unit_from_json();
                                    break;
                                case 2:
                                    connect = Load_connection_from_json();
                                    break;
                                case 3:
                                    unit = Load_unit_from_json();
                                    break;
                                case 0:
                                    Menu();
                                    not_exit1 = false;
                                    break;
                            }
                        }
                        break;
                    case 3:
                        if (connect == null)
                        {
                            Console.WriteLine("Заполните Connection");
                            Menu();
                            break;
                        }
                        if (unit == null)
                        {
                            Console.WriteLine("Заполните Unit");
                            Menu();
                            break;
                        }
                            TcpClient server = new TcpClient();
                            server.BeginConnect(connect.IP, connect.TCP, null, null);
                            ModbusIpMaster master = ModbusIpMaster.CreateIp(server);
                            for (ushort i = 0; i < unit.request_collection.LongCount(); i++)
                            {
                            ushort[] holding = master.ReadHoldingRegisters(unit.adres, unit.request_collection[i][0], unit.request_collection[i][1]);
                                Console.WriteLine("Запрос №{0}:", i + 1);
                                for (ushort j = 0; j < holding.Length; j++)
                                {
                                    Console.WriteLine("Значение регистра №{0}: {1}", j+1, holding[j].ToString("X"));
                                }
                                Console.WriteLine();
                            }
                            Console.ReadKey();
                        Menu();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
