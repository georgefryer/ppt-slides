namespace NoughtsAndCrosses
{
    interface IIOHandlerFactory
    {
        IInputHandler CreateInputHandler();
        IOutputHandler CreateOutputHandler();
    }

    class ConsoleIOHandlerFactory : IIOHandlerFactory
    {
        public IInputHandler CreateInputHandler()
        {
            return new ConsoleInputHandler();
        }

        public IOutputHandler CreateOutputHandler()
        {
            return new ConsoleOutputHandler();
        }
    }
}
