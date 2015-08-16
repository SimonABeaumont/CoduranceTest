using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetworkingApplication.Models
{

    public class Message
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageTime { get; set; }
        public int UserId { get; set; }

        public static void PostMessage(string command)
        {
            string[] fullMessage = command.Split('>');

            string messageText = fullMessage[1].Trim();
            string userName = fullMessage[0].Substring(0, (fullMessage[0].Length - 1)).Trim();

            Messages.Create(userName, messageText);
        }

        public static string ReadMessages(string userName)
        {
            if (Users.Exists(userName))
            {
                return Messages.GetMessages(Users.GetUser(userName).Id);
            }
            else
            {
                return String.Format("User {0} does not exist.", userName);
            }
        }

        public static string GetUserWall(string userName)
        {
            User user = Users.GetUser(userName);
            if (user != null)
            {
                string messages = Messages.GetMessages(user.Id, true);
                var sb = new StringBuilder();
                foreach (Message m in ApplicationData.Messages.OrderByDescending(t => t.MessageTime))
                {
                    if (user.FollowingUsers.Contains(m.UserId))
                    {
                        sb.AppendLine(Messages.GetMessageText(m, true));
                    }
                }
                messages += sb.ToString();
                return messages;
            } 
            else
            {
                return string.Format("User {0} does not exist", userName);
            }
        }
    }

    public static class Messages
    {
        private static int newId;

        public static int NewId
        {
            get
            {
                newId += 1;
                return newId;
            }
        }

        public static Message Create(string userName, string message)
        {            
            Message newMessage = new Message
            {
                Id = Messages.NewId,
                MessageText = message,
                MessageTime = DateTime.Now,
                UserId = Users.Create(userName).Id
            };
            ApplicationData.Messages.Add(newMessage);
            return newMessage;
        }

        public static string GetMessages(int userId, bool showUserName = false)
        {
            var sb = new StringBuilder();
            foreach (var message in ApplicationData.Messages.FindAll(m => m.UserId == userId).OrderByDescending(m => m.MessageTime))
            {
                if (showUserName)
                {
                    sb.AppendLine(string.Format("{0} - {1} {2}", Users.GetUser(message.UserId).Name, message.MessageText, TimeAgo(DateTime.Now, message.MessageTime)));
                }
                else
                {
                    sb.AppendLine(string.Format("{0} {1}", message.MessageText, TimeAgo(DateTime.Now, message.MessageTime)));
                }
            }
            return sb.ToString();
        }

        public static string GetMessages(bool showUserName = false)
        {
            var sb = new StringBuilder();
            foreach (var message in ApplicationData.Messages.OrderByDescending(m => m.MessageTime))
            {
                sb.AppendLine(GetMessageText(message));
            }
            return sb.ToString();
        }

        public static string GetMessageText(Message message, bool showUserName = false)
        {
            if (showUserName)
            {
                return string.Format("{0} - {1} {2}", Users.GetUser(message.UserId).Name, message.MessageText, TimeAgo(DateTime.Now, message.MessageTime));
            }
            else
            {
                return string.Format("{0} {1}", message.MessageText, TimeAgo(DateTime.Now, message.MessageTime));
            }
        }

        public static string TimeAgo(DateTime currentTime, DateTime originalTime)
        {
            TimeSpan span = currentTime - originalTime;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("({0} {1} ago)",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("({0} {1} ago)",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("({0} {1} ago)",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("({0} {1} ago)",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 0)
                return String.Format("({0} {1} ago)",
                span.Seconds, span.Seconds == 1 ? "second" : "seconds");
            return string.Empty;
        }
    }
}
 