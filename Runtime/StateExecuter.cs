namespace StateMachine
{
    public abstract class StateExecuter : ParameterContainerHolder
    {
        public string stateName { protected get; set; }

        public virtual void PreRun() { }

        public virtual void Run() { }

        public virtual void PostRun() { }
    }
}
