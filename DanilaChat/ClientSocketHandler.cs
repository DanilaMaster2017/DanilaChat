using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace DanilaChat
{
    public static class ClientSocketHandler
    {
        static Socket client;
        static byte[] buffer;

        delegate void meDelegate(string message, bool iSender);

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

        static void ReciveFromServer(IAsyncResult result)
        {
            List<string> answerDB = null;

            client.EndReceive(result);

            string query = Encoding.Unicode.GetString(buffer);
            int index = query.IndexOf('\0');
            query = query.Remove(index);
            string[] parametrs = query.Split();

            switch (parametrs[0])
            {               
                case "Message":
                    int sender = int.Parse(parametrs[1]);
                    if (Form1.OpenDialog.ContainsKey(sender))
                    {
                        index = parametrs[0].Length + parametrs[1].Length + 2;
                        string message = query.Substring(index);

                        meDelegate del = Form1.OpenDialog[sender].ShowMessage;

                        Form1.OpenDialog[sender].Invoke(del, message, false);
                    }
                    break;

                case "Conversation":
                    Human friend = Form1.GetFriendAtId(int.Parse(parametrs[1]));

                    index = parametrs[0].Length + parametrs[1].Length + 2;
                    string messages = query.Substring(index);
                    // createChatScreen( new string[] { "3","yu suka#EndMessage#", "дата", "время"}, friend);

                    

                   // ChatScreen chatScreen = new ChatScreen(messages.Split(), friend);
                    //chatScreen.ShowDialog();
                    
                    Thread thread = new Thread(NewForm);
                    thread.Start(
                        new object[] {
                            messages.Split(),
                            friend 
                        });
                    
                    break;


                default:
                    break;
                    
            }

            BeginRecive();
        }

        static void NewForm(object parametr)
        {
            object[] parametrs = (object[])parametr;

            string[] message = (string[])parametrs[0];
            Human friend = (Human)parametrs[1];

            ChatScreen chat = new ChatScreen(message, friend);
            Form1.OpenDialog[friend.UserId] = chat;
            chat.ShowDialog();
            
        }

        public static void createChatScreen(string[] messages, Human friend)
        {
            

            
        }
        public static void BeginRecive()
        {
            buffer = new byte[8192];
            client.BeginReceive(buffer, 0, 8192,
                SocketFlags.None, ReciveFromServer, null);
        }

    }
}
