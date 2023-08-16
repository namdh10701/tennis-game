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
        public double Value { get { return _value; } set { _value = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string _name;
        public double _value;
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
            Debug.Log("set value float");
            Value = (double)value;
        }
        public override string GetName()
        {
            return Name;
        }
    }

    [System.Serializable]
    public class RemoteBool : RemoteVariable
    {
        public bool Value { get { return _value; } set { _value = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string _name;
        public bool _value;
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

            Value = (bool)value;
        }
        public override string GetName()
        {
            return Name;
        }
    }
    [System.Serializable]
    public class RemoteJson : RemoteVariable
    {
        public string Value { get { return _value; } set { _value = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string _name;
        public string _value;
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
            Debug.Log("set value json");
            Value = (string)value;
        }
        public override string GetName()
        {
            return Name;
        }
    }
}