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
            Debug.LogWarning(this.GetHashCode() + GetName() + " init here");

            _value = initialValue;
        }
        public override object GetValue()
        {
            return _value;
        }
        public override void SetValue(object value)
        {
            Debug.LogWarning(this.GetHashCode() + GetName());
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
        public string Value { get { return _value; } set { _value = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string _name;
        public string _value;
        public RemoteJson(string name, string initialValue)
        {

            _name = name; Debug.LogWarning(this.GetHashCode() + GetName() + " init here");

            _value = initialValue;
        }
        public override object GetValue()
        {
            return _value;
        }
        public override void SetValue(object value)
        {
            _value = (string)value;
            Debug.Log(_value);
            Debug.Log(Value);
        }
        public override string GetName()
        {
            return _name;
        }
    }
}