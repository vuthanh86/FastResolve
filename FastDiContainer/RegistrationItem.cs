using System;
using System.Collections;

namespace FastDiContainer
{
    public class RegistrationItem : IEquatable<RegistrationItem>
    {
        public RegistrationItem(Type concreteType, Type derivedType, LifeTime lifeTime = LifeTime.Instance)
        {
            _concreteType = concreteType ?? throw new ArgumentNullException($"{nameof(concreteType)} can not be null.");
            _derivedType = derivedType ?? throw new ArgumentNullException($"{nameof(derivedType)} can not be null.");
            _lifeTime = lifeTime;

        }

        private readonly Type _concreteType;

        public Type ConcreteType => _concreteType;

        private readonly Type _derivedType;

        public Type DerivedType => _derivedType;

        private readonly LifeTime _lifeTime;

        public LifeTime LifeTime => _lifeTime;

        private object _objectInstance;

        public object ObjectInstance
        {
            get => _objectInstance;
            set => _objectInstance = value;
        }

        #region Equality members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RegistrationItem) obj);
        }

        public bool Equals(RegistrationItem other)
        {
            if (ReferenceEquals(null, other)) return false;

            if (!ReferenceEquals(ConcreteType, other.ConcreteType)) return false;

            if (!ReferenceEquals(DerivedType, other.DerivedType)) return false;

            return ConcreteType == other.ConcreteType && DerivedType == other.DerivedType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConcreteType, DerivedType);
        }

        public static bool operator ==(RegistrationItem left, RegistrationItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RegistrationItem left, RegistrationItem right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
