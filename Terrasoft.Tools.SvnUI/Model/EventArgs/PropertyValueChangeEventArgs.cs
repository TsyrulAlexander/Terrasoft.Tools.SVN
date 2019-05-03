namespace Terrasoft.Tools.SvnUI.Model.EventArgs
{
    public class PropertyValueChangeEventArgs<T> : System.EventArgs
    {
        public PropertyValueChangeEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue { get; }
        public T NewValue { get; }
    }
}