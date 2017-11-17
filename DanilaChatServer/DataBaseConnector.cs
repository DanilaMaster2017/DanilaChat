using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanilaChat;

namespace DanilaChatServer
{
    public class DataBaseConnector
    {
        const string conectionString = 
            "Provider=SQLOLEDB.1;Password=Danila0912;"
            + "Persist Security Info=True;User ID=danila;"
            + "Initial Catalog=chatDataBase;"
            + "Data Source=chatdb.ctl9bxrxhvlu.us-east-2.rds.amazonaws.com";

        static string loginString = "SELECT * FROM ListChatUser WHERE UserLogin = ?";
        static string selectUserString = "SELECT * FROM ListChatUser WHERE UserId = ?";
        static string readListString = "SELECT * FROM ListOfConversation_{0}";

        static string readConversationString = 
            "WITH ResultSet ( [RowNumber], [UserId], "
            + "[Message], [TimeDispatch]) "
            + "AS( SELECT ROW_NUMBER() OVER(ORDER BY TimeDispatch DESC), "
            + "* FROM Conversation_{0}_{1} ) "
            + "SELECT [UserId], [Message], [TimeDispatch] "
            + "FROM ResultSet WHERE RowNumber BETWEEN {2} AND {3}";

        static string messageString = 
            "INSERT INTO Conversation_{0}_{1} (UserId, Message) Values (?, ?)";

        static string regestrationString = "INSERT INTO ListChatUser "
            + "(UserLogin, Password, Name, Surname, BrithDay, Gender) " 
            + "Values (?, ?, ?, ?, ?, ?)";

        static string createConversationsString =
            "CREATE TABLE ListOfConversation_{0}( "
            + "UserId int NOT NULL "
            + "CONSTRAINT FK_ListOfConversation_{0}_UserId FOREIGN KEY(UserId) "
            + "REFERENCES ListChatUser(UserId) "
            + "ON UPDATE CASCADE "
            + "ON DELETE CASCADE, "
            + "NumberOfUnread int NOT NULL "
            + "CONSTRAINT DF_ListOfConversation_{0}_NumberOfUnread "
            + "DEFAULT (0) ); ";

        static string selectString = 
            "WITH ResultSet ( [RowNumber], [UserId], [UserLogin], "
            + "[Password], [Name], [Surname], [BrithDay], [Gender], [Avatar]) "
            + "AS( SELECT ROW_NUMBER() OVER(ORDER BY Surname), "
            + "* FROM ListChatUser ) "
            + "SELECT[UserId], [UserLogin], [Password], [Name], "
            + "[Surname], [BrithDay], [Gender], [Avatar] "
            + "FROM ResultSet WHERE RowNumber BETWEEN {0} AND {1}";

        static string addFriendString = 
            "INSERT INTO ListOfConversation_{0} (UserId) Values (?)";

        static string createConversationString =
          "CREATE TABLE[dbo].[Conversation_{0}_{1}] "
          + "( [UserId] [int] NOT NULL "
          + "CONSTRAINT FK_Conversation_{0}_{1}_UserId "
          + "FOREIGN KEY(UserId) "
          + "REFERENCES ListChatUser(UserId) "
          + "ON UPDATE CASCADE "
          + "ON DELETE CASCADE, "
          + "[Message] [nvarchar] (4000) NOT NULL, "
          + "[TimeDispatch] [datetime] NOT NULL "
          + "CONSTRAINT DF_Conversation_{0}_{1}_UserId "
          + "DEFAULT (getdate())) ";

        static string checkFriendString = 
            "Select UserId FROM ListOfConversation_{0} WHERE UserId = ?";

        static OleDbCommand loginCommand = new OleDbCommand();
        static OleDbCommand readListCommand = new OleDbCommand();
        static OleDbCommand selectUserCommand = new OleDbCommand();
        static OleDbCommand readConversationCommand = new OleDbCommand();
        static OleDbCommand messageCommand = new OleDbCommand();
        static OleDbCommand regestrationCommand = new OleDbCommand();
        static OleDbCommand createConversationsCommand = new OleDbCommand();
        static OleDbCommand selectCommand = new OleDbCommand();
        static OleDbCommand addFriendCommand = new OleDbCommand();
        static OleDbCommand createConversationCommand = new OleDbCommand();
        static OleDbCommand checkFriendCommand = new OleDbCommand();

        public DataBaseConnector()
        {
            OleDbConnection conectionDBChat = new OleDbConnection();
            conectionDBChat.ConnectionString = conectionString;
            conectionDBChat.Open();

            loginCommand.Connection = conectionDBChat;
            loginCommand.CommandText = loginString;
            loginCommand.Parameters.Add("@Login", OleDbType.VarChar, 50);

            readListCommand.Connection = conectionDBChat;

            readConversationCommand.Connection = conectionDBChat;

            selectUserCommand.Connection = conectionDBChat;
            selectUserCommand.CommandText = selectUserString;
            selectUserCommand.Parameters.Add("@Id", OleDbType.Integer);

            messageCommand.Connection = conectionDBChat;
            messageCommand.Parameters.Add("@Id", OleDbType.Integer);
            messageCommand.Parameters.Add("@Message", OleDbType.LongVarWChar);

            regestrationCommand.Connection = conectionDBChat;
            regestrationCommand.CommandText = regestrationString;
            regestrationCommand.Parameters.Add("@Login", OleDbType.VarChar, 50);
            regestrationCommand.Parameters.Add("@Password", OleDbType.VarChar, 50);
            regestrationCommand.Parameters.Add("@Name", OleDbType.VarWChar, 50);
            regestrationCommand.Parameters.Add("@Surname", OleDbType.VarWChar, 50);
            regestrationCommand.Parameters.Add("@BrithDay", OleDbType.Date);
            regestrationCommand.Parameters.Add("@Gender", OleDbType.VarChar, 8);

            createConversationsCommand.Connection = conectionDBChat;

            selectCommand.Connection = conectionDBChat;

            addFriendCommand.Connection = conectionDBChat;
            addFriendCommand.Parameters.Add("@Id", OleDbType.Integer);

            createConversationCommand.Connection = conectionDBChat;

            checkFriendCommand.Connection = conectionDBChat;
            checkFriendCommand.Parameters.Add("@Id", OleDbType.Integer);
            //Console.WriteLine("Yes");
        }

        public static List<string> LoginChat(string login, string password,  Human user)
        {
            List<string> answer = new List<string>();

            loginCommand.Parameters[0].Value = login;
            OleDbDataReader reader = loginCommand.ExecuteReader();


            if (!reader.Read())
            {
                answer.Add("Invalid");
                return answer;
            }

            
            string validPassword = reader.GetString(2);    
            
            if (password == validPassword)
            {
                answer.Add("Ok");
               
                user = readUserFromReader(reader, user);
                answer.Add(user.ToString());
                
                readListOfConversation(user.UserId, answer);
                reader.Close();
                answer[0] += " " + (answer.Count - 2);
                return answer;
            }
            else
            {
                reader.Close();
                answer.Add("Invalid");
                return answer;
            }
        }

        static Human readUserFromReader(OleDbDataReader reader, Human user)
        {
            if (user == null) user = new Human();

            user.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
            user.Name = reader.GetString(reader.GetOrdinal("Name"));
            user.Surname = reader.GetString(reader.GetOrdinal("Surname"));

            string date = reader.GetString(reader.GetOrdinal("BrithDay"));
            string[] massDate = date.Split('-');
            int year = int.Parse(massDate[0]);
            int month = int.Parse(massDate[1]);
            int day = int.Parse(massDate[2]);
            user.Brithday = new DateTime(year, month, day);
            
            user.Gender = reader.GetString(reader.GetOrdinal("Gender"));
            //user.Avatar = reader.GetString(reader.GetOrdinal("Avatar"));

            return user;
        }

        static void readListOfConversation(int id, List<string> answer)
        {
            readListCommand.CommandText = String.Format(readListString, id);

            OleDbDataReader reader = readListCommand.ExecuteReader();

            while (reader.Read())
            {
                selectUserCommand.Parameters[0].Value = 
                    reader.GetInt32(reader.GetOrdinal("UserId"));
                OleDbDataReader readerUser = selectUserCommand.ExecuteReader();

                readerUser.Read();
                Human friend = readUserFromReader(readerUser, null);
                answer.Add(friend.ToString() + " " 
                    + reader.GetInt32(reader.GetOrdinal("NumberOfUnread")));
                readerUser.Close();
            }
            reader.Close();
        }

        static public List<string> ReadConversation(int id1, int id2, int numberQuery)
        {
            List<string> answer = new List<string>();
            int requestCount = 25;
            int beginIndex = 1 + numberQuery * requestCount;

            readConversationCommand.CommandText = 
                String.Format(readConversationString, id1, id2, beginIndex,
                beginIndex + requestCount - 1);

            OleDbDataReader readConversation = 
                readConversationCommand.ExecuteReader();

            while (readConversation.Read())
            {
                string result = "";

                int number = readConversation.GetOrdinal("UserId");
                result += readConversation.GetInt32(number) + " ";

                number = readConversation.GetOrdinal("Message");
                result += readConversation.GetString(number) + "#EndMessage#" + " ";

                number = readConversation.GetOrdinal("TimeDispatch");
                result += readConversation.GetDateTime(number);

                answer.Add(result);
            }
            if (answer.Count < requestCount) answer.Add("e");

            readConversation.Close();
            return answer;
        }

        static public void SendingMessage(int senderId, int reciverId, string message)
        {
            messageCommand.Parameters[0].Value = senderId;
            messageCommand.Parameters[1].Value = message;

            int id1, id2;

            if (senderId < reciverId)
            {
                id1 = senderId;
                id2 = reciverId;
            }
            else
            {
                id2 = senderId;
                id1 = reciverId;
            }

            messageCommand.CommandText = string.Format(messageString, id1, id2);
            messageCommand.ExecuteNonQuery();
        }

        static public List<string> RegestrationUser(string[] query)
        {
            List<string> answer = new List<string>();

            string login = query[1];
            string password = query[2];
            string name = query[4];
            string surname = query[5];
            DateTime brithDay = DateTime.Parse(query[6] + " " + query[7]);
            string gender = query[8];

            regestrationCommand.Parameters[0].Value = login;
            regestrationCommand.Parameters[1].Value = password;
            regestrationCommand.Parameters[2].Value = name;
            regestrationCommand.Parameters[3].Value = surname;
            regestrationCommand.Parameters[4].Value = brithDay.Date;
            regestrationCommand.Parameters[5].Value = gender;

            int count = regestrationCommand.ExecuteNonQuery();

            if (count == 0)
            {
                answer.Add("Invalid");
                return answer;
            }

            loginCommand.Parameters[0].Value = login;
            int id = (int)loginCommand.ExecuteScalar();

            createConversationsCommand.CommandText =
                string.Format(createConversationsString, id);
            createConversationsCommand.ExecuteNonQuery();
            answer.Add("Ok " + id);

            return answer;
        }

        public static List<string> SelectUsers(int numberQuery)
        {
            List<string> answer = new List<string>();
            int requestCount = 20;
            int beginIndex = 1 + requestCount * numberQuery;

            selectCommand.CommandText = 
                string.Format(selectString, beginIndex, 
                beginIndex + requestCount - 1);

            OleDbDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                Human man = readUserFromReader(reader, null);
                answer.Add(man.ToString());
            }
            reader.Close();

            if (answer.Count < requestCount) answer.Add("e");

            return answer;
        }

        public static bool CreateFriendship(int id1, int id2)
        {
            checkFriendCommand.CommandText =
                string.Format(checkFriendString, id1);
            checkFriendCommand.Parameters[0].Value = id2;

            object answer = checkFriendCommand.ExecuteScalar();

            if (answer != null) return false;

            createConversationCommand.CommandText =
                string.Format(createConversationString, id1, id2);

            createConversationCommand.ExecuteNonQuery();


            addFriendCommand.CommandText = 
                string.Format(addFriendString, id1);
            addFriendCommand.Parameters[0].Value = id2;

            addFriendCommand.ExecuteNonQuery();


            addFriendCommand.CommandText =
                string.Format(addFriendString, id2);
            addFriendCommand.Parameters[0].Value = id1;

            addFriendCommand.ExecuteNonQuery();

            return true;
        }

    }
}
