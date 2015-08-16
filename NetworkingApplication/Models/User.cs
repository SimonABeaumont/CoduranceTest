using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetworkingApplication.Models
{
    public class User
    {
        private List<int> followingUsers;

        public int Id { get; set; }
        public string Name { get; set; }

        public List<int> FollowingUsers
        {
            get
            {
                if (followingUsers == null)
                {
                    followingUsers = new List<int>();
                }
                return followingUsers;
            }
        }

        public static string FollowUser(string command)
        {            
            string delimiters = @"(follows)";
            string[] fullMessage = Regex.Split(command, delimiters);

            if (Users.Exists(fullMessage[0].Trim()))
            {
                User mainUser = Users.GetUser(fullMessage[0].Trim());
                string toFollow = fullMessage[2].Trim();

                if (Users.Exists(toFollow))
                {
                    mainUser.FollowingUsers.Add(Users.GetUser(toFollow).Id);
                    return null;
                }
                else
                {
                    return String.Format("User {0} does not exist.", toFollow);
                }
            }
            else
            {
                return "User " + fullMessage[0].Trim() + " does not exist.";
            }
        }
    }

    public static class Users
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

        public static User Create(string userName)
        {            
            if (!Exists(userName))
            {
                User newUser = new User
                {
                    Id = Users.NewId,
                    Name = userName
                };
                ApplicationData.Users.Add(newUser);
                return newUser;
            }
            else
            {
                return Users.GetUser(userName);
            }
        }

        public static bool Exists(string userName)
        {
            return ApplicationData.Users.Exists(u => u.Name == userName);
        }

        public static bool Exists(int userId)
        {
            return ApplicationData.Users.Exists(u => u.Id == userId);
        }

        public static User GetUser(string userName)
        {
            return ApplicationData.Users.Find(u => u.Name == userName);
        }

        public static User GetUser(int userId)
        {
            return ApplicationData.Users.Find(u => u.Id == userId);
        }
    }
}
