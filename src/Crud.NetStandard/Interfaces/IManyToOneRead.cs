namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IManyToOne{TManyModel,TId}" />
    public interface IManyToOneRead<TManyModel, in TId> :
        IManyToOne<TManyModel, TId>,
        IRead<TManyModel, TId>
    {
    }
}
