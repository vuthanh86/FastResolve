using System;
using System.Collections;

namespace FastDiContainer
{
    public class RegistrationItem : IEquatable<RegistrationItem>
    {
        public RegistrationItem(Type concreteType, Type derivedType, LifeTime lifeTime = LifeTime.Instance)
        {
            ConcreteType = concreteType ?? throw new ArgumentNullException($"{nameof(concreteType)} can not be null.");
            DerivedType = derivedType ?? throw new ArgumentNullException($"{nameof(derivedType)} can not be null.");
            LifeTime = lifeTime;
        }

        public Type ConcreteType { get; }

        public Type DerivedType { get; }

        public LifeTime LifeTime { get; }

        public object DerivedObject { get; set; }

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
