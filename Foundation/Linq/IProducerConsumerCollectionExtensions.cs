﻿using System.Collections.Concurrent;

namespace Foundation.Linq
{
#if FOUNDATION_3_5
    using DataCommander.Foundation.Collections;
#else
#endif

    /// <summary>
    /// 
    /// </summary>
    public static class IProducerConsumerCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int Take<T>(this IProducerConsumerCollection<T> collection, T[] target)
        {
#if CONTRACTS_FULL
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(target != null);
#endif

            var i = 0;
            while (i < target.Length)
            {
                T item;
                var succeeded = collection.TryTake(out item);
                if (succeeded)
                {
                    target[i] = item;
                    i++;
                }
                else
                {
                    break;
                }
            }
            return i;
        }
    }
}