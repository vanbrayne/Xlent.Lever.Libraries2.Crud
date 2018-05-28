using System;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Model
{
    /// <summary>
    /// Information about a claimed lock
    /// </summary>
    public class Lock<TId> : IValidatable, IUniquelyIdentifiable<TId>
    {
        /// <inheritdoc />
        public TId Id { get; set; }

        /// <summary>
        /// The id of the object that the lock is for.
        /// </summary>
        public TId ItemId { get; set; }

        /// <summary>
        /// The id of the object that the lock is for.
        /// </summary>
        public DateTimeOffset ValidUntil { get; set; }

        /// <inheritdoc />
        public void Validate(string errorLocation, string propertyPath = "")
        {
            FulcrumValidate.IsNotDefaultValue(Id, nameof(Id), errorLocation);
            FulcrumValidate.IsNotDefaultValue(ItemId, nameof(ItemId), errorLocation);
            FulcrumValidate.IsNotDefaultValue(ValidUntil, nameof(ValidUntil), errorLocation);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{ItemId} ({ValidUntil})";
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Lock<TId> @lock)) return false;
            return Equals(Id, @lock.Id) && Equals(ItemId, @lock.ItemId);
        }
    }
}