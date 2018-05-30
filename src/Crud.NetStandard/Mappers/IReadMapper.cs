namespace Xlent.Lever.Libraries2.Crud.Mappers
{

    /// <summary>
    /// Methods for mapping data between the client and server models.
    /// </summary>
    public interface IReadMapper<out TClientModel, in TServerModel> : IMappable
    {
        /// <summary>
        /// Map fields from the server
        /// </summary>
        /// <param name="source">The record to map from</param>
        /// <returns>A new client object with the mapped values from <paramref name="source"/>.</returns>
        TClientModel MapFromServer(TServerModel source);
    }
}