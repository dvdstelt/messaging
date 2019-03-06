using System;
using System.Threading.Tasks;
using Messages;

namespace ClientUI
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "ClientUI";

            // Configure your endpoint and start it


            //
            // *** LOOP
            //
            while (true)
            {
                Console.WriteLine("Press 'P' to place an order, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command

                        // Send the command

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        break;
                }

                // Stop endpoint
        }
    }
}