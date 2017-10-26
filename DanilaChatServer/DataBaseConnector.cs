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
        static string readConversationString = "SELECT * FROM Conversation_{0}_{1}";

        static OleDbCommand loginCommand = new OleDbCommand();
        static OleDbCommand readListCommand = new OleDbCommand();
        static OleDbCommand selectUserCommand = new OleDbCommand();
        static OleDbCommand readConversationCommand = new OleDbCommand();

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

            //Console.WriteLine("Yes");
        }

        public static List<string> LoginChat(string login, string password, Human user)
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

        static public List<string> ReadConversation(int id1, int id2)
        {
            List<string> answer = new List<string>();

            readConversationCommand.CommandText = 
                String.Format(readConversationString, id1, id2);

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

            readConversation.Close();
            return answer;
        }

        static int SearchUserByLogin()
        {
            return 1;
        }

        static bool checkPassword(string writeUser, string validPassword)
        {
            return writeUser == validPassword;
        }


    }
}
