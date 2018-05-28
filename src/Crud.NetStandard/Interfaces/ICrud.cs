﻿namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ICrud{TModelCreate, TModel,TId}" />
    public interface ICrud<TModel, TId> : 
        ICrud<TModel, TModel, TId>,
        ICreate<TModel, TId>,
        ICreateWithSpecifiedId<TModel, TId>
    {
    }

    /// <summary>
    /// Interface for CRUD operations."/>.
    /// </summary>
    public interface ICrud<in TModelCreate, TModel, TId> :
        ICreate<TModelCreate, TModel, TId>,
        ICreateWithSpecifiedId<TModelCreate, TModel, TId>,
        IRead<TModel, TId>,
        IReadAll<TModel, TId>,
        IUpdate<TModel, TId>,
        IDelete<TId>,
        IDeleteAll,
        ILockable<TId> 
        where TModel : TModelCreate
    {
    }
}
