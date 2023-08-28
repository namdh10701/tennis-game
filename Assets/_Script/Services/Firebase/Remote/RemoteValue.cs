using UnityEngine;

namespace Services.FirebaseService.Remote
{
    [System.Serializable]
    public abstract class RemoteVariable
    {
        public abstract object GetValue();
        public abstract void SetValue(object value);
        public abstract string GetName();
    }
    [System.Serializable]
    public class RemoteDouble : RemoteVariable
    {
        public double Value { get { return _value; } }
        private string _name;
        private double _value;
        public RemoteDouble(string name, double initialValue)
        {
            _name = name;
            _value = initialValue;
        }
        public override object GetValue()
        {
            return _value;
        }
        public override void SetValue(object value)
        {
            _value = (double)value;
        }
        public override string GetName()
        {
            return _name;
        }
    }

    [System.Serializable]
    public class RemoteBool : RemoteVariable
    {
        public bool Value { get { return _value; } }
        private string _name;
        private bool _value;
        public RemoteBool(string name, bool initialValue)
        {
            _name = name;
            _value = initialValue;
        }
        public override object GetValue()
        {
            return _value;
        }
        public override void SetValue(object value)
        {

            _value = (bool)value;
        }
        public override string GetName()
        {
            return _name;
        }
    }
    [System.Serializable]
    public class RemoteJson : RemoteVariable
    {
        public string Value { get { return _value; } }
        private string _name;
        private string _value;
        public RemoteJson(string name, string initialValue)
        {
            _name = name;
            _value = initialValue;
        }
        public override object GetValue()
        {
            return _value;
        }
        public override void SetValue(object value)
        {
            _value = (string)value;
        }
        public override string GetName()
        {
            return _name;
        }
    }
}