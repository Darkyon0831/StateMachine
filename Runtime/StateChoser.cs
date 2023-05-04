using System.Collections.Generic;

namespace StateMachine
{
    public abstract class StateChoser : ParameterContainerHolder
    {
        public abstract int Chose(in List<State> states);
    }
}
