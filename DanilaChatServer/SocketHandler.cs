using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using DanilaChat;

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

        static Dictionary<int, DataSocket> UsersOnline = 
            new Dictionary<int, DataSocket>();

        static Dictionary<int, int> createFriendship =
            new Dictionary<int, int>();
        

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

            try
            {
                data.ClientConnection.EndReceive(result);
            }
            catch
            {
                UsersOnline.Remove(data.User.UserId);
                data.ClientConnection.Close();
                return;
            }
            

            Console.WriteLine("Recive");

            string query = Encoding.Unicode.GetString(data.Buffer);
            int index = query.IndexOf('\0');
            query =  query.Remove(index);
            string[] parametrs = query.Split();

            switch (parametrs[0])
            {
                case "Login":
                    answerDB = DataBaseConnector.LoginChat
                        (parametrs[1], parametrs[2], data.User);

                    if (answerDB[0].Contains("Ok"))
                        UsersOnline[data.User.UserId] = data;
                   
                    break;

                case "Read":
                    int id1 = int.Parse(parametrs[1]);
                    int id2 = int.Parse(parametrs[2]);
                    int numberQuery = int.Parse(parametrs[3]);

                    answerDB = DataBaseConnector.ReadConversation(id1, id2, numberQuery);

                    int friendId = (id1 == data.User.UserId) ? id2 : id1;                   
                    answerDB[0] = "Conversation " + friendId + " " + answerDB[0];
                    break;

                case "Message":
                    index = parametrs[0].Length + parametrs[1].Length + 2;
                    string message = query.Substring(index);

                    int receiver = int.Parse(parametrs[1]);
                    DataBaseConnector.SendingMessage
                        (data.User.UserId, receiver, message);

                    if (UsersOnline.ContainsKey(receiver))
                        SendingOnlineMessage
                            (UsersOnline[receiver], data.User.UserId, message);
                    break;

                case "Regestration":
                    answerDB = DataBaseConnector.RegestrationUser(parametrs);
                    break;

                case "Select":
                    answerDB = DataBaseConnector.SelectUsers(int.Parse(parametrs[1]));
                    answerDB[0] = "Select " + answerDB[0];
                    break;

                case "AddFriend":
                    int senderId = data.User.UserId;
                    int receiverId = int.Parse(parametrs[1]);

                    if ((createFriendship.ContainsKey(receiverId))
                        && (createFriendship[receiverId] == senderId)) break;

                    createFriendship[senderId] = receiverId;
                    int Id1, Id2;

                    if (senderId > receiverId)
                    {
                        Id1 = receiverId;
                        Id2 = senderId;
                    }
                    else
                    {
                        Id2 = receiverId;
                        Id1 = senderId;
                    }

                    if (!DataBaseConnector.CreateFriendship(Id1, Id2))
                    {
                        createFriendship.Remove(senderId);
                        break;
                    }

                    if (UsersOnline.ContainsKey(receiverId))
                    {
                        SendingNewFriend(UsersOnline[receiverId], data.User); 
                    }

                    AddedSuccessfull(data);
                    createFriendship.Remove(senderId);
                    break;



                default :
                    break;
            }

            if (answerDB != null)
            {
                string answer = string.Join(" ", answerDB);

                byte[] answerSocket = Encoding.Unicode.GetBytes(answer);
                data.ClientConnection.Send(answerSocket);
            }

            data.Buffer = new byte[1024];
            data.ClientConnection.BeginReceive
                (data.Buffer, 0, 1024, SocketFlags.None, ReciveFromClient, data);
        }

        static void SendingOnlineMessage
            (DataSocket receiverSocket, int idSender, string message)
        {
            string answer = "Message " + idSender + " " + message;

            byte[] answerSocket = Encoding.Unicode.GetBytes(answer);
            receiverSocket.ClientConnection.Send(answerSocket);
        
        }

        static void SendingNewFriend(DataSocket receiverSocket, Human newFriend)
        {
            string answer = "AddFriend " + newFriend;

            byte[] answerSocket = Encoding.Unicode.GetBytes(answer);
            receiverSocket.ClientConnection.Send(answerSocket);
        }

        static void AddedSuccessfull(DataSocket client)
        {
            string answer = "AddedSuccessfull";

            byte[] answerSocket = Encoding.Unicode.GetBytes(answer);
            client.ClientConnection.Send(answerSocket);
        }
    }
}
