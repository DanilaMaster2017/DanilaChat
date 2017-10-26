using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DanilaChatServer
{
    public class SocketHandler
    {
        static Socket serverSocket;
        public static Socket ServerSocket
        {
            get { return serverSocket; }
            set { serverSocket = value; }
        }
        

        public static void HandlerAccept(IAsyncResult result)
        {
            serverSocket.BeginAccept(HandlerAccept, null);

            Console.WriteLine("Accept");

            Socket client = serverSocket.EndAccept(result);

            DataSocket data = new DataSocket();
            data.ClientConnection = client;

            client.BeginReceive(data.Buffer, 0, 1024, SocketFlags.None, ReciveFromClient, data);
        }

        static void ReciveFromClient(IAsyncResult result)
        {
            List<string> answerDB = null;

            DataSocket data = (DataSocket)result.AsyncState;
            data.ClientConnection.EndReceive(result);

            Console.WriteLine("Recive");

            string query = Encoding.Unicode.GetString(data.Buffer);
            int index = query.IndexOf('\0');
            query =  query.Remove(index);
            string[] parametrs = query.Split();

            switch (parametrs[0])
            {
                case "Login":
                    answerDB = DataBaseConnector.LoginChat(parametrs[1], parametrs[2], data.User);
                    break;

                case "Read":
                    answerDB = DataBaseConnector.ReadConversation
                        (int.Parse(parametrs[1]), int.Parse(parametrs[2]));

                    break;

                default :
                    break;
            }


            if (query.Contains("q"))
            {
                data.ClientConnection.Dispose();
                data = null;
                return;
            }

            string answer = string.Join(" ", answerDB);
            
            byte[] answerSocket = Encoding.Unicode.GetBytes(answer);
            data.ClientConnection.Send(answerSocket);
            
            data.Buffer = new byte[1024];
            data.ClientConnection.BeginReceive(data.Buffer, 0, 1024, SocketFlags.None, ReciveFromClient, data);
        }
    }
}
