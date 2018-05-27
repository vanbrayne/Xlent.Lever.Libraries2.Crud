﻿namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <summary>
    /// Interface for -RUD operations."/>.
    /// </summary>
    /// <typeparam name="TModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId">The type for the id.</typeparam>
    public interface IRud<TModel, in TId> : IRead<TModel, TId>, IReadAll<TModel, TId>, IUpdate<TModel, TId>, IDelete<TId>, IDeleteAll, ILockable<TId>
    {
    }
}
