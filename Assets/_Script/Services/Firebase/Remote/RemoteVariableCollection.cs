using System.Collections.Generic;
using UnityEngine;

namespace Services.FirebaseService.Remote
{
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
            if (_variables.ContainsKey(variable.Name))
            {
                Debug.LogError($"Duplicate entry: Variable '{variable.Name}' already exists.");
                return;
            }

            _variables.Add(variable.Name, variable);
        }

        protected RemoteInt CreateRemoteInt(string name, int value)
        {
            RemoteInt newVariable = new(name, value);
            AddVariable(newVariable);
            return newVariable;
        }
        protected RemoteFloat CreateRemoteFloat(string name, float value)
        {
            RemoteFloat newVariable = new(name, value);
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