using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DanilaChatServer
{
    class Program
    {

            static void Main(string[] args)
            {
                SocketHandler.ServerSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                EndPoint point = new IPEndPoint(IPAddress.Any, 8345);
                try
                {
                    SocketHandler.ServerSocket.Bind(point);
                    SocketHandler.ServerSocket.Listen(100);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Невозможно запустить сервер " + exc.Message);
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("Server run");

                SocketHandler.ServerSocket.BeginAccept(SocketHandler.HandlerAccept, null);

                DataBaseConnector dbConnection = new DataBaseConnector();
            
               // List<string> an = DataBaseConnector.LoginChat("DanilaMaster", "DanilaIsCool", null);

                Console.ReadLine();
            }
            
        }
    }
    

