using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NetworkingApplication.ApplicationController;


namespace NetworkingApplication
{
    public static class Program
    {
        private static Controller control;

        private static Controller controller
        {
            get
            {
                if (control == null)
                {
                    control = new Controller();
                }
                return control;
            }
        }

        public static void Main(string[] args)
        {
            Console.Title = "Codurance Messaging Demo - Simon Beaumont";
            RunApplication();
        }

        private static void RunApplication()
        {
            DisplayMessage(Controller.HelpMessage);
            try
            {
                while (true)
                {
                    Console.Write(">");
                    String consoleInput = Console.ReadLine();
                    if (consoleInput != null)
                    {
                        string consoleText = controller.ProcessCommand(consoleInput);
                        if (consoleText != null)
                        {
                            DisplayMessage(consoleText);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                DisplayMessage(string.Format("The follow error occured {0}; ", ex.Message));
                RunApplication();
            }
        }

        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
