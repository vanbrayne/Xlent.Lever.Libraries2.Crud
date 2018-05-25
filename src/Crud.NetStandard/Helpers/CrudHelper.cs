using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Error.Logic;
using Xlent.Lever.Libraries2.Crud.Interfaces;

namespace Xlent.Lever.Libraries2.Crud.Helpers
{
    /// <summary>
    /// Help methods for crud operations.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class CrudHelper<TId>
    {
        /// <summary>
        /// If <paramref name="service"/> doesn't implement <typeparamref name="T"/>, an exception is thrown.
        /// </summary>
        /// <param name="service">The service that must implement <typeparamref name="T"/>.</param>
        /// <typeparam name="T">The type that <paramref name="service"/> must implement.</typeparam>
        /// <returns></returns>
        /// <exception cref="FulcrumNotImplementedException">Thrown if <paramref name="service"/> doesn't implement <typeparamref name="T"/>.</exception>
        public T VerifyImplemented<T>(ICrudable<TId> service) where T : ICrudable<TId>
        {
            if (service is T implemented) return implemented;
            throw new FulcrumNotImplementedException($"The service {service.GetType()} does not implement {typeof(T).Name}");
        }
    }

    /// <inheritdoc />
    public class CrudHelper<TModel, TId> : CrudHelper<TId>
    {
        /// <summary>
        /// If <paramref name="service"/> doesn't implement <typeparamref name="T"/>, an exception is thrown.
        /// </summary>
        /// <param name="service">The service that must implement <typeparamref name="T"/>.</param>
        /// <typeparam name="T">The type that <paramref name="service"/> must implement.</typeparam>
        /// <returns></returns>
        /// <exception cref="FulcrumNotImplementedException">Thrown if <paramref name="service"/> doesn't implement <typeparamref name="T"/>.</exception>
        public T VerifyImplemented<T>(ICrudable<TModel, TId> service) where T : ICrudable<TModel, TId>
        {
            if (service is T implemented) return implemented;
            throw new FulcrumNotImplementedException($"The service {service.GetType()} does not implement {typeof(T).Name}");
        }
    }

    /// <inheritdoc />
    public class CrudHelper<TModelCreate, TModelReturned, TId> : CrudHelper<TModelReturned, TId>
    {
        /// <summary>
        /// If <paramref name="service"/> doesn't implement <typeparamref name="T"/>, an exception is thrown.
        /// </summary>
        /// <param name="service">The service that must implement <typeparamref name="T"/>.</param>
        /// <typeparam name="T">The type that <paramref name="service"/> must implement.</typeparam>
        /// <returns></returns>
        /// <exception cref="FulcrumNotImplementedException">Thrown if <paramref name="service"/> doesn't implement <typeparamref name="T"/>.</exception>
        public T VerifyImplemented<T>(ICrudable<TModelCreate, TModelReturned, TId> service) where T : ICrudable<TModelCreate, TModelReturned, TId>
        {
            if (service is T implemented) return implemented;
            throw new FulcrumNotImplementedException($"The service {service.GetType()} does not implement {typeof(T).Name}");
        }
    }
}
