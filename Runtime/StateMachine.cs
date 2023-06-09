using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.U2D;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] ParameterContainer parameterContainer;
        [SerializeField] List<State> states;
        [SerializeField] StateChoser stateChoser;

        int lastActiveState = -1;

        public ParameterContainer ParameterContainer { get { return parameterContainer; } set { parameterContainer = value; } }
        public StateChoser StateChoser { get { return stateChoser; } set { stateChoser = value; } }

        public void SetDefaultState(int index)
        {
            if (index >= 0 && index < states.Count)
            {
                states[index].IsDefault = true;

                for (int i = 0; i < states.Count; i++)
                {
                    if (i != index)
                        states[i].IsDefault = false;
                }
            }
        }

        public void AddState(StateConditioner conditioner = null, StateExecuter executer = null)
        {
            State state = ScriptableObject.CreateInstance<State>();
            state.Conditioner = conditioner;
            state.Executer = executer;

            states.Add(state);
        }

        void Start()
        {
            if (stateChoser == null)
                stateChoser = gameObject.AddComponent<BeginAndEndChoser>();

            if (parameterContainer == null)
                parameterContainer = gameObject.AddComponent<DefaultStateParameters>();
        }

        void Update()
        {
            if (states.Count > 0)
            {
                stateChoser.parameter = parameterContainer;
                int choseIndex = stateChoser.Chose(states);

                if (choseIndex >= 0 && choseIndex < states.Count)
                {
                    State state = states[choseIndex];

                    if (lastActiveState != -1)
                        states[lastActiveState].IsActive = false;

                    state.UpdateState(parameterContainer);
                    state.Run();

                    state.IsActive = true;
                    lastActiveState = choseIndex;
                }
            }
        }
    }
}
