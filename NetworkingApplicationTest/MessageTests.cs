using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingApplication.Models;

namespace NetworkingApplicationTest
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void CreateMessage()
        {
            ApplicationData.Messages.Clear();
            ApplicationData.Users.Clear();
            Users.Create("User 1");
            Assert.AreEqual(0, ApplicationData.Messages.Count);

            Messages.Create("User 1", "new message text");
            Assert.AreEqual(1, ApplicationData.Messages.Count);

            Messages.Create("User 1", "message 2");
            Assert.AreEqual(2, ApplicationData.Messages.Count);
        }

        [TestMethod]
        public void GetMessagesForAUserDoNotShowUserName()
        {
            ApplicationData.Messages.Clear();
            ApplicationData.Users.Clear();
            int userId = Users.Create("User 1").Id;
            Assert.AreEqual(0, ApplicationData.Messages.Count);

            Messages.Create("User 1", "new message text");
            string messageText = Messages.GetMessages(userId);
            Assert.AreEqual("new message text \r\n", messageText);
            Messages.Create("User 1", "message 2");
            messageText = Messages.GetMessages(userId);
            Assert.AreEqual("message 2 \r\nnew message text \r\n", messageText);
        }

        [TestMethod]
        public void GetMessagesForAUserShowUserName()
        {
            ApplicationData.Messages.Clear();
            ApplicationData.Users.Clear();
            int userId = Users.Create("User 1").Id;
            Assert.AreEqual(0, ApplicationData.Messages.Count);

            Messages.Create("User 1", "new message text");
            string messageText = Messages.GetMessages(userId, true);
            Assert.AreEqual("User 1 - new message text \r\n", messageText);
        }

        [TestMethod]
        public void TimeAgo1SecondAgo()
        {
            DateTime now = DateTime.Now;
            DateTime oneSecondAgo = now.AddSeconds(-1);

            String timeAgoText = Messages.TimeAgo(now, oneSecondAgo);
            Assert.AreEqual("(1 second ago)", timeAgoText);
        }

        [TestMethod]
        public void PostMessage()
        {
            ApplicationData.Messages.Clear();
            ApplicationData.Users.Clear();
            Assert.AreEqual(0, ApplicationData.Messages.Count);
            Message.PostMessage("user 1 -> hello world");
            int userId = Users.GetUser("user 1").Id;
            Assert.AreEqual(1, ApplicationData.Messages.Count);
            string messageText = Messages.GetMessages(userId, false);
            Assert.AreEqual("hello world \r\n", messageText);
        }

        [TestMethod]
        public void ReadMessageForAUserWhoDoesNotExist()
        {
            string messageText = Message.ReadMessages("Fred");        
            Assert.AreEqual("User Fred does not exist.", messageText);
        }

        [TestMethod]
        public void ReadMessageForAUserWhoDoesExist()
        {
            ApplicationData.Messages.Clear();
            ApplicationData.Users.Clear();
            Message message = Messages.Create("Fred", "Hello World") ;
            message.MessageTime = message.MessageTime.AddMinutes(-5);
            string messageText = Message.ReadMessages("Fred");
            Assert.AreEqual("Hello World (5 minutes ago)\r\n", messageText);

            message = Messages.Create("Fred", "Hello World 2");
            message.MessageTime = message.MessageTime.AddMinutes(-1);
            messageText = Message.ReadMessages("Fred");
            Assert.AreEqual("Hello World 2 (1 minute ago)\r\nHello World (5 minutes ago)\r\n", messageText);
        }

        [TestMethod]
        public void GetUserWall()
        {
            ApplicationData.Messages.Clear();
            ApplicationData.Users.Clear();
            Message message = Messages.Create("Fred", "Hello World");
            message.MessageTime = message.MessageTime.AddMinutes(-10);
            string messageText = Message.GetUserWall("Fred");
            Assert.AreEqual("Fred - Hello World (10 minutes ago)\r\n", messageText);

            message = Messages.Create("Fred", "Hello World 2");
            message.MessageTime = message.MessageTime.AddMinutes(-8);
            messageText = Message.GetUserWall("Fred");
            Assert.AreEqual("Fred - Hello World 2 (8 minutes ago)\r\nFred - Hello World (10 minutes ago)\r\n", messageText);

            message = Messages.Create("Alice", "Hi all");
            User.FollowUser("Fred follows Alice");
            message.MessageTime = message.MessageTime.AddMinutes(-7);
            messageText = Message.GetUserWall("Fred");
            Assert.AreEqual("Fred - Hello World 2 (8 minutes ago)\r\nFred - Hello World (10 minutes ago)\r\nAlice - Hi all (7 minutes ago)\r\n", messageText);

            messageText = Message.GetUserWall("Alice");
            Assert.AreEqual("Alice - Hi all (7 minutes ago)\r\n", messageText);
        }

        [TestMethod]
        public void GetMessageText()
        {
            ApplicationData.Messages.Clear();
            ApplicationData.Users.Clear();
            Message message = Messages.Create("Fred", "Hello World");
            Message message2 = Messages.Create("Fred", "Hello World2");
            message.MessageTime = message.MessageTime.AddMinutes(-8);
            message2.MessageTime = message2.MessageTime.AddHours(-5);
            string messageText = Messages.GetMessageText(message, true);
            Assert.AreEqual("Fred - Hello World (8 minutes ago)", messageText);

            messageText = Messages.GetMessageText(message2, true);
            Assert.AreEqual("Fred - Hello World2 (5 hours ago)", messageText);
        }

        [TestMethod]
        public void GetUserWallForANonExistantUser()
        {
            ApplicationData.Messages.Clear();
            ApplicationData.Users.Clear();
            Message message = Messages.Create("Fred", "Hello World");
            message.MessageTime = message.MessageTime.AddMinutes(-10);
            string messageText = Message.GetUserWall("Dennis");
            Assert.AreEqual("User Dennis does not exist", messageText);
        }

        //[TestMethod]
        //public void UserExistsByName()
        //{
        //    ApplicationData.Users.Clear();
        //    Assert.IsFalse(Users.Exists("User 1"));
        //    Users.Create("User 1");
        //    Assert.IsTrue(Users.Exists("User 1"));
        //}

        //[TestMethod]
        //public void UserExistsById()
        //{
        //    ApplicationData.Users.Clear();
        //    Assert.IsFalse(Users.Exists("User 1"));
        //    int id = Users.Create("User 1").Id;
        //    Assert.IsTrue(Users.Exists(id));
        //}

        //[TestMethod]
        //public void GetUserByName()
        //{
        //    ApplicationData.Users.Clear();
        //    Users.Create("User 1");
        //    User user = Users.GetUser("User 1");
        //    Assert.IsFalse((user.Name == "User 2"));
        //    Assert.IsTrue((user.Name == "User 1"));
        //}

        //[TestMethod]
        //public void GetUserById()
        //{
        //    ApplicationData.Users.Clear();
        //    int id = Users.Create("User 1").Id;
        //    User user = Users.GetUser(id);
        //    Assert.IsTrue((user.Id == id));
        //    Assert.IsTrue((user.Name == "User 1"));
        //}

        //[TestMethod]
        //public void FollowUser()
        //{
        //    ApplicationData.Users.Clear();
        //    int id = Users.Create("User 1").Id;
        //    User user = Users.GetUser(id);
        //    Assert.IsTrue((user.FollowingUsers.Count == 0));
        //    Users.Create("User 2");
        //    User.FollowUser("User 1 follows User 2");
        //    Assert.IsTrue((user.FollowingUsers.Count == 1));

        //    User user2 = Users.GetUser("User 2");
        //    Assert.IsTrue((user2.FollowingUsers.Count == 0));
        //    User.FollowUser("User 2 follows User 2");
        //    Assert.IsTrue((user2.FollowingUsers.Count == 1));

        //    Assert.IsTrue((User.FollowUser("User 1 follows User 3") == "User User 3 does not exist."));
        //}

        //[TestMethod]
        //public void FollowNonExistantUser()
        //{
        //    ApplicationData.Users.Clear();
        //    Users.Create("User 1");

        //    Assert.IsTrue((User.FollowUser("User 1 follows User 3") == "User User 3 does not exist."));
        //}

        //[TestMethod]
        //public void NonExistantUserTriesToFollow()
        //{
        //    ApplicationData.Users.Clear();
        //    Assert.IsTrue((User.FollowUser("User 1 follows User 3") == "User User 1 does not exist."));
        //}
    }
}
