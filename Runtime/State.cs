using System;
using UnityEngine;

namespace StateMachine
{
    public class State : ScriptableObject
    {
        [SerializeField] new string name;
        [SerializeField] StateConditioner conditioner;
        [SerializeField] StateExecuter executer;
        [SerializeField] bool isDefault = false;
        [SerializeField] bool isActive = false;

        public StateConditioner Conditioner { set { conditioner = value; } }
        public StateExecuter Executer { set { executer = value; } }

        public bool IsDefault { get { return isDefault; } set { isDefault = value; } }

        public bool IsActive { get { return isActive;} set { isActive = value; } }

        public void Run()
        {
            if (executer != null)
            {
                executer.PreRun();
                executer.Run();
                executer.PostRun();
            }
        }

        public bool Begin()
        {
            if (conditioner != null)
                return conditioner.Begin();

            return false;
        }

        public bool End()
        {
            if (conditioner != null)
                return conditioner.End();

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
