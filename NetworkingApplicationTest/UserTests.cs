using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingApplication.Models;

namespace NetworkingApplicationTest
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void CreateUser()
        {
            ApplicationData.Users.Clear();
            Assert.AreEqual(0, ApplicationData.Users.Count);
            Users.Create("User 1");
            Assert.AreEqual(1, ApplicationData.Users.Count);
            Users.Create("User 2");
            Assert.AreEqual(2, ApplicationData.Users.Count);
        }

        [TestMethod]
        public void UserExistsByName()
        {
            ApplicationData.Users.Clear();
            Assert.IsFalse(Users.Exists("User 1"));
            Users.Create("User 1");
            Assert.IsTrue(Users.Exists("User 1"));
        }

        [TestMethod]
        public void UserExistsById()
        {
            ApplicationData.Users.Clear();
            Assert.IsFalse(Users.Exists("User 1"));
            int id = Users.Create("User 1").Id;
            Assert.IsTrue(Users.Exists(id));
        }

        [TestMethod]
        public void GetUserByName()
        {
            ApplicationData.Users.Clear();
            Users.Create("User 1");
            User user = Users.GetUser("User 1");
            Assert.AreNotEqual("User 2", user.Name);
            Assert.AreEqual("User 1", user.Name);
        }

        [TestMethod]
        public void GetUserById()
        {
            ApplicationData.Users.Clear();
            int id = Users.Create("User 1").Id;
            User user = Users.GetUser(id);
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual("User 1", user.Name);
        }

        [TestMethod]
        public void FollowUser()
        {
            ApplicationData.Users.Clear();
            int id = Users.Create("User 1").Id;
            User user = Users.GetUser(id);
            Assert.AreEqual(0, user.FollowingUsers.Count);
            Users.Create("User 2");
            User.FollowUser("User 1 follows User 2");
            Assert.AreEqual(1, user.FollowingUsers.Count);

            User user2 = Users.GetUser("User 2");
            Assert.AreEqual(0, user2.FollowingUsers.Count);
            User.FollowUser("User 2 follows User 2");
            Assert.AreEqual(1, user2.FollowingUsers.Count);
            Assert.AreEqual("User User 3 does not exist.", User.FollowUser("User 1 follows User 3"));
        }

        [TestMethod]
        public void FollowNonExistantUser()
        {
            ApplicationData.Users.Clear();
            Users.Create("User 1");
            Assert.AreEqual("User User 3 does not exist.", User.FollowUser("User 1 follows User 3"));
        }

        [TestMethod]
        public void NonExistantUserTriesToFollow()
        {
            ApplicationData.Users.Clear();
            Assert.AreEqual("User User 1 does not exist.", User.FollowUser("User 1 follows User 3"));
        }
    }
}
