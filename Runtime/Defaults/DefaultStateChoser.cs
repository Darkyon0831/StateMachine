using StateMachine;
using System.Collections.Generic;

public class DefaultStateChoser : StateChoser
{
    State lastState = null;
    int lastIndex = 0;

    public override int Chose(in List<State> states)
    {
        int chosenIndex = -1;

        if (lastState != null)
        {
            if (lastState.Check())
                chosenIndex = lastIndex;
            else
                lastState = null;
        }

        if (chosenIndex == -1)
        {
            for (int i = 0; i < states.Count; i++)
            {
                State state = states[i];

                if (state == null) continue;

                state.UpdateState(parameter);
                if (state.Check())
                {
                    chosenIndex = i;

                    lastState = state;
                    lastIndex = i;

                    break;
                }
            }
        }

        return chosenIndex;
    }
}
