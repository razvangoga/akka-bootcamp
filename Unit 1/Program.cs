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

            // time to make your first actors!
            IActorRef writerActor = WinTailActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "writerActor");
            IActorRef readerActor = WinTailActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(writerActor)), "readerActor");

            // tell console reader to begin
            readerActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            WinTailActorSystem.AwaitTermination();
        }
    }
    #endregion
}
