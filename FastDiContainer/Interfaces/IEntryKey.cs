using System;

namespace FastDiContainer.Interfaces
{
    public interface IEntryKey
    {
        Type Type { get; }
        object Key { get; }
    }

    public class EntryKey : IEntryKey
    {
        private object _key;
        private Type _type;

        public EntryKey(object key, Type type)
        {
            _key = key ?? throw new ArgumentNullException($"{nameof(key)} can not be null.");
            _type = type ?? throw new ArgumentNullException($"{nameof(type)} can not be null.");
        }

        #region Implementation of IEntryKey

        public Type Type
        {
            get => _type;
            private set => _type = value;
        }

        public object Key
        {
            get => _key;
            private set => _key = value;
        }

        #endregion

        #region Overrides of Object

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
