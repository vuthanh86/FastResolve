using System;
using System.Collections;

namespace FastDiContainer
{
    public class RegistrationItem : IEquatable<RegistrationItem>
    {
        #region Equality members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RegistrationItem) obj);
        }

        #endregion

        public RegistrationItem(Type concreteType, Type derivedType)
        {
            ConcreteType = concreteType ?? throw new ArgumentNullException($"{nameof(concreteType)} can not be null.");
            DerivedType = derivedType ?? throw new ArgumentNullException($"{nameof(derivedType)} can not be null.");
        }

        public Type ConcreteType { get; }

        public Type DerivedType { get; }
        //
        // public string ItemKey => $"{nameof(_concreteType)}>{nameof(_derivedType)}";

        #region Equality members

        public bool Equals(RegistrationItem other)
        {
            if (ReferenceEquals(null, other)) return false;

            if (ReferenceEquals(this, other)) return true;

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
