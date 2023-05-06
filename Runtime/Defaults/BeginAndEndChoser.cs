using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using static UnityEditorInternal.VersionControl.ListControl;

public class BeginAndEndChoser : StateMachine.StateChoser
{
    int currentIndex = -1;

    public override int Chose(in List<State> states)
    {
        int index = 0;

        if (currentIndex >= 0)
        {
            State state = states[currentIndex];

            if (state.End() == false)
                index = currentIndex;
            else
                currentIndex = -1;
        }

        if (currentIndex == -1 || states[currentIndex].IsDefault)
        {
            int chosenIndex = ChoseState(states);
            currentIndex = chosenIndex;
            index = chosenIndex;
        }

        return index;
    }

    int ChoseState(in List<State> states)
    {
        int chosenIndex = -1;
        int defaultStateIndex = -1;

        for (int i = 0; i < states.Count; i++)
        {
            State state = states[i];

            if (state == null) continue;

            state.UpdateState(parameter);
            if (state.Begin())
            {
                chosenIndex = i;

                break;
            }
            else if (state.IsDefault)
                defaultStateIndex = i;
        }

        if (chosenIndex == -1 && defaultStateIndex != -1)
            chosenIndex = defaultStateIndex;

        return chosenIndex;
    }
}
