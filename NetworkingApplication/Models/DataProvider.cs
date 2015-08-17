using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingApplication.Models
{
    public static class ApplicationData
    {
        private static List<Message> messageData;
        private static List<User> userData;

        public static List<Message> Messages
        {
            get
            {
                if (messageData == null)
                {
                    messageData = new List<Message>();
                }
                return messageData;
            }
        }

        public static List<User> Users
        {
            get
            {
                if (userData == null)
                {
                    userData = new List<User>();
                }
                return userData;
            }
        }
    }
}