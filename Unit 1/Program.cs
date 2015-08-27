using System;
using Akka.Actor;
using WinTail.Actors;

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

            Props tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = WinTailActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            Props consoleWriterProps = Props.Create(typeof(ConsoleWriterActor));
            IActorRef writerActor = WinTailActorSystem.ActorOf(consoleWriterProps, "writerActor");

            //Props validationActorProps = Props.Create(() => new ValidationActor(writerActor));
            //IActorRef validationActor = WinTailActorSystem.ActorOf(validationActorProps, "validationActor");
            Props fileValidatorActorProps = Props.Create(() => new FileValidatorActor(writerActor, tailCoordinatorActor));
            IActorRef fileValidationActor = WinTailActorSystem.ActorOf(fileValidatorActorProps, "fileValidationActor");

            Props consoleReaderProps = Props.Create<ConsoleReaderActor>(fileValidationActor);
            IActorRef readerActor = WinTailActorSystem.ActorOf(consoleReaderProps, "readerActor");

            // tell console reader to begin
            readerActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            WinTailActorSystem.AwaitTermination();
        }
    }
    #endregion
}
