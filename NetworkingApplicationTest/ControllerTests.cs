using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingApplication;
using NetworkingApplication.ApplicationController;
using NetworkingApplication.Models;


namespace NetworkingApplicatinoTest
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void GetHelpMessage()
        {
            string expectedMessage = "Please submit your command: type\r\n[user name] -> [message]  - to post a message\r\n" +
                        "[user name] - to read a person's posts\r\n[user name] follows [user to follow] - to follow another person\r\n" +
                        "[user name] wall - to view all your posts and those of the people you follow\r\n\r\n" +
                        "Press Ctrl+C to exit the application\r\n";
            Assert.AreEqual(expectedMessage, Controller.HelpMessage);
        }

        [TestMethod]
        public void ProcessCommand()
        {
            Controller control = new Controller();
            string message = control.ProcessCommand("gsgdf");
            Assert.AreEqual("User gsgdf does not exist.", message);

            message = control.ProcessCommand("User 1 -> hello");
            Assert.AreEqual(1, ApplicationData.Users.Count);
            Assert.AreEqual(1, ApplicationData.Messages.Count);
            Assert.IsNull(message);
        }
    }
}
