using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.FirebaseService.Remote
{
    [Serializable]
    public abstract class RemoteVariableCollection
    {
        private Dictionary<string, RemoteVariable> _variables = new Dictionary<string, RemoteVariable>();
        public Dictionary<string, RemoteVariable> Variables
        {
            get { return _variables; }
        }
        public abstract void AddToFetchQueue();
        protected void AddVariable(RemoteVariable variable)
        {
            if (_variables.ContainsKey(variable.GetName()))
            {
                Debug.LogError($"Duplicate entry: Variable '{variable.GetName()}' already exists.");
                return;
            }

            _variables.Add(variable.GetName(), variable);
        }

        protected RemoteDouble CreateRemoteInt(string name, double value)
        {
            RemoteDouble newVariable = new(name, value);
            AddVariable(newVariable);
            return newVariable;
        }
        protected RemoteBool CreateRemoteBool(string name, bool value)
        {
            RemoteBool newVariable = new(name, value);
            AddVariable(newVariable);
            return newVariable;
        }
    }
}