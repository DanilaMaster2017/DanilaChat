using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using DanilaChat;

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

                DataBaseConnector dbConnection = new DataBaseConnector();
                //GeneratedNewUsers(40);

                SocketHandler.ServerSocket.BeginAccept(SocketHandler.HandlerAccept, null);

                

                Console.ReadLine();
            }
/*
        static void GeneratedNewUsers(int count)
        {
            for (int i = 0; i < 40;i++)
            {
                Human newUsers = new Human();

                newUsers.Name = "Петрович" + i;
                newUsers.Surname = "Петров";
                newUsers.Brithday = DateTime.Now;
                newUsers.Gender = "male";

                string login = "user" + i;
                string password = "user" + i;

                string regestrationString = "n " + login + " " + password + " " + newUsers.ToString();  

                DataBaseConnector.RegestrationUser(regestrationString.Split());
            }
            Console.WriteLine("Completed");
        }
        */
            
        }
    }
    

