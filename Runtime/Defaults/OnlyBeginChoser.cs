using StateMachine;
using System.Collections.Generic;

public class OnlyBeginChoser : StateChoser
{
    State lastState = null;
    int lastIndex = 0;

    public override int Chose(in List<State> states)
    {
        int chosenIndex = -1;
        int defaultStateIndex = -1;

        if (lastState != null)
        {
            if (lastState.Begin())
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
                if (state.Begin())
                {
                    chosenIndex = i;

                    lastState = state;
                    lastIndex = i;

                    break;
                }
                else if (state.IsDefault)
                    defaultStateIndex = i;
            }
        }

        if (chosenIndex == -1 && defaultStateIndex != -1)
            chosenIndex = defaultStateIndex;

        return chosenIndex;
    }
}
