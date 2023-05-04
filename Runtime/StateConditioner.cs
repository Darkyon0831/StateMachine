namespace StateMachine
{
    public abstract class StateConditioner : ParameterContainerHolder
{
        public abstract bool Check();
    }
}
