namespace SwiftAPI.API.ServerVariables
{
    public class ServerVariable 
    {
        public string ID;
    }

    public class ServerVariable<T> : ServerVariable
    {
        public delegate void OnServerVariableChanged(T value);

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnChanged?.Invoke(value);
            }
        }

        public OnServerVariableChanged OnChanged;

        protected T value;
    }
}
