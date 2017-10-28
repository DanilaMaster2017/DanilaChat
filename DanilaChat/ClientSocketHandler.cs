using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DanilaChat
{
    public static class ClientSocketHandler
    {
        static Socket client;

        static ClientSocketHandler()
        {
            if (client != null)
            {
                client.Close();
                client.Dispose();
            }

            client = new Socket(AddressFamily.InterNetwork,
               SocketType.Stream, ProtocolType.Tcp);
            IPAddress[] adress; //= Dns.GetHostAddresses("ec2-13-59-225-182.us-east-2.compute.amazonaws.com");

            adress = Dns.GetHostAddresses("127.0.0.1");

            IPEndPoint point = new IPEndPoint(adress[0], 8345);

            client.Connect(point);
        }

        public static void SendToServer(string query)
        {
            byte[] buffer = new byte[query.Length * 2];
            Encoding.Unicode.GetBytes(query, 0, query.Length, buffer, 0);
            client.Send(buffer);
        }

        public static string WaitReciveFromServer()
        {
            byte[] answer = new byte[8192];

            int countByte = client.Receive(answer);
            string resultat = Encoding.Unicode.GetString(answer, 0, countByte);

            return resultat;
        }

    
            
    }
}
