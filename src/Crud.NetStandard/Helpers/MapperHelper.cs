﻿using System;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Crud.Model;
using Xlent.Lever.Libraries2.Core.Error.Logic;
using Xlent.Lever.Libraries2.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Helpers
{
    /// <summary>
    /// Help methods for mapping
    /// </summary>
    public static class MapperHelper
    {
        /// <summary>
        /// Map an id between two types.
        /// </summary>
        /// <param name="source">The id to map.</param>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <exception cref="FulcrumNotImplementedException">Thrown if the type was not recognized. Please add that type to the class <see cref="MapperHelper"/>.</exception>
        public static TTarget MapToType<TTarget, TSource>(TSource source)
        {
            if (source == null) return default(TTarget);
            if (Equals(source, default(TSource))) return default(TTarget);
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);
            if (targetType == typeof(string))
            {
                return (TTarget)(object)source.ToString();
            }
            if (targetType == typeof(Guid))
            {
                var success = Guid.TryParse(source.ToString(), out var valueAsGuid);
                InternalContract.Require(success, $"Could not parse parameter {nameof(source)} ({source}) of type {sourceType.Name} into type Guid.");
                return (TTarget)(object)valueAsGuid;
            }
            if (targetType == typeof(int))
            {
                var success = int.TryParse(source.ToString(), out var valueAsInt);
                InternalContract.Require(success, $"Could not parse parameter {nameof(source)} ({source}) of type {sourceType.Name} into type int.");
                return (TTarget)(object)valueAsInt;
            }
            if (targetType == typeof(int))
            {
                var success = int.TryParse(source.ToString(), out var valueAsInt);
                InternalContract.Require(success, $"Could not parse parameter {nameof(source)} ({source}) of type {sourceType.Name} into type int.");
                return (TTarget)(object)valueAsInt;
            }
            throw new FulcrumNotImplementedException($"There is currently no rule on how to convert an id from type {sourceType.Name} to type {targetType.Name}.");
        }

        /// <summary>
        /// Map an id between two types.
        /// </summary>
        /// <param name="source">The id to map.</param>
        /// <exception cref="FulcrumNotImplementedException">Thrown if the type was not recognized. Please add that type to the class <see cref="MapperHelper"/>.</exception>
        public static SlaveToMasterId<TTarget> MapToType<TTarget, TSource>(SlaveToMasterId<TSource> source)
        {
            if (source == null) return null;
            var targetMasterId = MapToType<TTarget, TSource>(source.MasterId);
            var targetSlaveId = MapToType<TTarget, TSource>(source.SlaveId);
            return new SlaveToMasterId<TTarget>(targetMasterId, targetSlaveId);
        }

        /// <summary>
        /// Map a lock between two types.
        /// </summary>
        /// <param name="source">The lock to map.</param>
        /// <exception cref="FulcrumNotImplementedException">Thrown if the type was not recognized. Please add that type to the class <see cref="MapperHelper"/>.</exception>

        public static Lock<TTarget> MapToType<TTarget, TSource>(Lock<TSource> source)
        {
            return new Lock<TTarget>
            {
                Id = MapToType<TTarget, TSource>(source.Id),
                ItemId = MapToType<TTarget, TSource>(source.ItemId),
                ValidUntil = source.ValidUntil
            };
        }
    }
}
