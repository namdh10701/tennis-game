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
            _variables.Add(variable.GetName(), variable);
        }
    }
}