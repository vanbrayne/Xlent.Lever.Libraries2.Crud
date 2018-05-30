﻿namespace Xlent.Lever.Libraries2.Crud.Mappers
{
    /// <inheritdoc cref="IMapper{TClientModel,TServerModel}" />
    /// <typeparam name="TClientModel">The model the client uses when updating items.</typeparam>
    /// <typeparam name="TServerModel">The model that the server uses. </typeparam>
    public interface IMapper<TClientModel, TServerModel> : IMapper<TClientModel, TClientModel, TServerModel>
    {
    }

    /// <summary>
    /// Methods for mapping data between the client and server models.
    /// </summary>
    /// <typeparam name="TClientModelCreate">The model that the client uses when creating items.</typeparam>
    /// <typeparam name="TClientModel">The model the client uses when updating items.</typeparam>
    /// <typeparam name="TServerModel">The model that the server uses. </typeparam>
    public interface IMapper<in TClientModelCreate, TClientModel, TServerModel> : 
        ICreateMapper<TClientModelCreate, TServerModel>, 
        IUpdateMapper<TClientModel, TServerModel>, 
        IReadMapper<TClientModel, TServerModel>
    {
    }
}