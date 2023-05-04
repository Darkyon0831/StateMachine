using System;
using UnityEngine;

namespace StateMachine
{
    public class State : ScriptableObject
    {
        [SerializeField] new string name;
        [SerializeField] StateConditioner conditioner;
        [SerializeField] StateExecuter executer;

        public StateConditioner Conditioner { set { conditioner = value; } }
        public StateExecuter Executer { set { executer = value; } }

        public void Run()
        {
            if (executer != null)
                executer.Run();
        }

        public bool Check()
        {
            if (conditioner != null)
                return conditioner.Check();

            return false;
        }

        private void UpdateParameters(in ParameterContainer c)
        {
            if (conditioner != null)
                conditioner.parameter = c;

            if (executer != null)
                executer.parameter = c;
        }

        public void UpdateState(in ParameterContainer c)
        {
            UpdateParameters(c);

            if (executer != null)
                executer.stateName = name;
        }
    }
}
