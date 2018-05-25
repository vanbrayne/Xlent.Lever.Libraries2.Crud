using Xlent.Lever.Libraries2.Core.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <summary>
    /// Indicates that the implementor has one or more crud methods./>.
    /// </summary>
    /// <typeparam name="TId">The type for the <see cref="IUniquelyIdentifiable{TId}.Id"/> property.</typeparam>
    public interface ICrudable<in TId>
    {
    }

    /// <summary>
    /// Indicates that the implementor has one or more crud methods./>.
    /// </summary>
    /// <typeparam name="TModel">The type for the objects in persistant storage.</typeparam>
    /// <typeparam name="TId">The type for the <see cref="IUniquelyIdentifiable{TId}.Id"/> property.</typeparam>
    public interface ICrudable<in TModel, in TId> : ICrudable<TId>
    {
    }

    /// <summary>
    /// Indicates that the implementor has one or more crud methods./>.
    /// </summary>
    /// <typeparam name="TModelCreate">The type for creating objects in persistant storage.</typeparam>
    /// <typeparam name="TModelReturned">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId">The type for the <see cref="IUniquelyIdentifiable{TId}.Id"/> property.</typeparam>
    public interface ICrudable<in TModelCreate, in TModelReturned, in TId> : ICrudable<TModelReturned, TId>
    {
    }
}
