using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace SerialPortApp.App.Client
{
    public class CustomClient
    {
        private int _port = 9001;
        private int _timeout = 1000;
        private string _ipHost = "127.0.0.1";
        private TcpClient _tcpClient;
        
        public CustomClient()
        {
            var host = IPAddress.Parse(_ipHost);
            var endPoint = new IPEndPoint(host, _port);
            _tcpClient = new TcpClient();
            _tcpClient.Connect(endPoint);
        }


        public void ReloadApp()
        {
            var host = IPAddress.Parse(_ipHost);
            

            var endPoint = new IPEndPoint(host, _port);
            var client = new TcpClient();
            client.Connect(endPoint);
            
            /*
            var response = WriteTelnet(_tcpClient,"test", _timeout);
            Console.WriteLine(response);

            Console.ReadLine();*/
        }

        public string ReadTelnet(long timeout = 1000)
        {
            for (long i = 0; i < timeout; i++)
            {
                var stream = _tcpClient.GetStream();

                // if (client.ReceiveBufferSize <= 0 || !stream.CanRead || !stream.DataAvailable)
                if (_tcpClient.ReceiveBufferSize <= 0)
                    continue;

                do
                {
                    System.Threading.Thread.Sleep(10000);

                    var buffer = new byte[_tcpClient.ReceiveBufferSize];
                    var p = stream.Read(buffer, 0, _tcpClient.ReceiveBufferSize);

                    var response = Encoding.GetEncoding(1251).GetString(buffer).Trim('\0');
                    Console.WriteLine($@"{i}. {response}");
                    if (response == "")
                    {
                        continue;
                    }

                    return response;
                } while (stream.DataAvailable);
            }

            return null;
        }

        public void WriteTelnet(string command, long timeout = 1000)
        {
            for (long i = 0; i < timeout; i++)
            {
                var stream = _tcpClient.GetStream();
 
                // отправляем сообщение
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(command);
                writer.Flush();
            }
        }
    }
}