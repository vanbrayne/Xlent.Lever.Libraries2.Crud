using System;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Error.Logic;
using Xlent.Lever.Libraries2.Crud.Interfaces;

namespace Xlent.Lever.Libraries2.Crud.Helpers
{
    /// <summary>
    /// Helper methods for Storage
    /// </summary>
    public static class CrudHelper
    {
        /// <summary>
        /// Create a new Id of type <see cref="string"/> or type <see cref="Guid"/>.
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        public static TId CreateNewId<TId>()
        {
            var id = default(TId);
            if (typeof(TId) == typeof(Guid))
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                id = (dynamic)Guid.NewGuid();
            }
            else if (typeof(TId) == typeof(string))
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                id = (dynamic)Guid.NewGuid().ToString();
            }
            else
            {
                FulcrumAssert.Fail(null,
                    $"{nameof(CreateNewId)} can handle Guid and string as type for Id, but it can't handle {typeof(TId)}.");
            }
            return id;
        }

        /// <summary>
        /// Helper method to convert from one parameter type to another.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ConvertToParameterType<T>(object source)
        {
            object referenceIdAsObject = source;
            try
            {
                var target = (T)referenceIdAsObject;
                return target;
            }
            catch (Exception e)
            {
                InternalContract.Fail(
                    $"The value \"{source}\" of type {source.GetType().Name} can't be converted into type {typeof(T).Name}:\r" +
                    $"{e.Message}");
                // We should not end up at this line, but the compiler think that we can, so we add a throw here.
                throw;
            }
        }

        /// <summary>
        /// If <paramref name="service"/> doesn't implement <typeparamref name="T"/>, an exception is thrown.
        /// </summary>
        /// <param name="service">The service that must implement <typeparamref name="T"/>.</param>
        /// <typeparam name="T">The type that <paramref name="service"/> must implement.</typeparam>
        /// <returns></returns>
        /// <exception cref="FulcrumNotImplementedException">Thrown if <paramref name="service"/> doesn't implement <typeparamref name="T"/>.</exception>
        public static T GetImplementationOrThrow<T>(ICrudable service) where T : ICrudable
        {
            if (service is T implemented) return implemented;
            throw new FulcrumNotImplementedException($"The service {service.GetType()} does not implement {typeof(T).Name}");
        }
    }
}
