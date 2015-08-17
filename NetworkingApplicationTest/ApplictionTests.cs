using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkingApplication;


namespace NetworkingApplicationTest
{
    [TestClass]
    public class ApplicationTests
    {

        [TestMethod]
        public void DisplayMessage()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                using (StringReader sr = new StringReader(""))
                {
                    Console.SetIn(sr);
                    String helpMessage = "send message to console";
                    Program.DisplayMessage(helpMessage);
                    string expected = "send message to console" + Environment.NewLine;
                    Assert.AreEqual(expected, sw.ToString());
                }
            }
        }
    }
}
