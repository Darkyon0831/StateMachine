namespace StateMachine
{
    public abstract class StateConditioner : ParameterContainerHolder
    {
        public abstract bool Begin();
        
        public abstract bool End();
    }
}
