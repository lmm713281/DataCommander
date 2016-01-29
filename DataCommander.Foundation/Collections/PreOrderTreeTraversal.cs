﻿namespace DataCommander.Foundation.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// 
    /// </summary>
    public static class PreOrderTreeTraversal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootNode"></param>
        /// <param name="getChildNodes"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(T rootNode, Func<T, IEnumerable<T>> getChildNodes, Action<T> action)
        {
            Contract.Requires<ArgumentNullException>(rootNode != null);
            Contract.Requires<ArgumentNullException>(getChildNodes != null);
            Contract.Requires<ArgumentNullException>(action != null);

            action(rootNode);

            foreach (var childNode in getChildNodes(rootNode))
            {
                ForEach(childNode, getChildNodes, action);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootNode"></param>
        /// <param name="getChildNodes"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(T rootNode, Func<T, IEnumerable<T>> getChildNodes, Func<T, bool> predicate) where T : class
        {
            Contract.Requires<ArgumentNullException>(rootNode != null);
            Contract.Requires<ArgumentNullException>(getChildNodes != null);
            Contract.Requires<ArgumentNullException>(predicate != null);

            T firstOrDefault = null;

            if (predicate(rootNode))
            {
                firstOrDefault = rootNode;
            }
            else
            {
                foreach (var childNode in getChildNodes(rootNode))
                {
                    firstOrDefault = FirstOrDefault<T>(childNode, getChildNodes, predicate);
                    if (firstOrDefault != null)
                    {
                        break;
                    }
                }
            }

            return firstOrDefault;
        }
    }
}