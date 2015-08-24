using System;
using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem WinTailActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            WinTailActorSystem = ActorSystem.Create("WinTailActorSystem");

            PrintInstructions();

            // time to make your first actors!
            IActorRef writerActor = WinTailActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "writerActor");
            IActorRef readerActor = WinTailActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(writerActor)), "readerActor");

            // tell console reader to begin
            readerActor.Tell("start");

            // blocks the main thread from exiting until the actor system is shut down
            WinTailActorSystem.AwaitTermination();
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.Write("Some lines will appear as");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" red ");
            Console.ResetColor();
            Console.Write(" and others will appear as");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" green! ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }
    }
    #endregion
}
