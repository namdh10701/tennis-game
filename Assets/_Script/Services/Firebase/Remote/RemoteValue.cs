namespace Services.FirebaseService.Remote
{
    [System.Serializable]
    public abstract class RemoteVariable
    {
        public object Value { get { return _value; } set { _value = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        protected string _name;
        protected object _value;
    }
    [System.Serializable]
    public class RemoteFloat : RemoteVariable
    {
        public RemoteFloat(string name, float initialValue)
        {
            _name = name;
            _value = initialValue;
        }
    }
    [System.Serializable]
    public class RemoteInt : RemoteVariable
    {
        public RemoteInt(string name, int initialValue)
        {
            _name = name;
            _value = initialValue;
        }
    }
    [System.Serializable]
    public class RemoteBool : RemoteVariable
    {
        public RemoteBool(string name, bool initialValue)
        {
            _name = name;
            _value = initialValue;
        }
    }
    public class RemoteJson : RemoteVariable
    {
        public RemoteJson(string name, string initialValue)
        {
            _name = name;
            _value = initialValue;
        }
    }
}