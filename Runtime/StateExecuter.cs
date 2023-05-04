namespace StateMachine
{
    public abstract class StateExecuter : ParameterContainerHolder
    {
        public string stateName { protected get; set; }

        public abstract void Run();
    }
}
