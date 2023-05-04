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

        public ParameterContainer ParameterContainer { get { return parameterContainer; } set { parameterContainer = value; } }
        public StateChoser StateChoser { get { return stateChoser; } set { stateChoser = value; } }

        public void AddState(StateConditioner conditioner = null, StateExecuter executer = null)
        {
            State state = ScriptableObject.CreateInstance<State>();
            state.Conditioner = conditioner;
            state.Executer = executer;

            states.Add(state);
        }

        private void Start()
        {
            if (stateChoser == null)
                stateChoser = gameObject.AddComponent<DefaultStateChoser>();

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
                    states[choseIndex].UpdateState(parameterContainer);
                    states[choseIndex].Run();
                }
            }
        }
    }
}
