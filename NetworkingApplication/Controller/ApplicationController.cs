using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

using NetworkingApplication.Models;

namespace NetworkingApplication.ApplicationController
{
    public class Controller
    {
        public static string HelpMessage
        {
            get
            {
                return "Please submit your command: type" + Environment.NewLine +
                        "[user name] -> [message]  - to post a message" + Environment.NewLine +
                        "[user name] - to read a person's posts" + Environment.NewLine +
                        "[user name] follows [user to follow] - to follow another person" + Environment.NewLine +
                        "[user name] wall - to view all your posts and those of the people you follow" + Environment.NewLine + Environment.NewLine +
                        "Press Ctrl+C to exit the application" + Environment.NewLine;
            }
        }

        public string ProcessCommand(string input)
        {
            string returnValue = null;
            string delimiters = @"(->)|(follows)|(wall)";
            string[] fullCommand = Regex.Split(input, delimiters);

            if (fullCommand.GetUpperBound(0) == 0)
            {
                returnValue = Message.ReadMessages(input);
            }
            else
            {
                switch (fullCommand[1])
                {
                    case "->": //posting
                        Message.PostMessage(input);
                        break;
                    case "follows": //follows
                        returnValue = User.FollowUser(input);
                        break;
                    case "wall": //wall
                        returnValue = Message.GetUserWall(fullCommand[0].Trim());
                        break;
                }
            }
            return returnValue;
        }
    }
}
