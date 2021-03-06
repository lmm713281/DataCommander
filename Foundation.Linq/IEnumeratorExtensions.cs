﻿using System;
using System.Collections;
using System.Collections.Generic;
using Foundation.Assertions;
using Foundation.Diagnostics.Contracts;

namespace Foundation.Linq
{
    public static class IEnumeratorExtensions
    {
        #region Public Methods

        public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> enumerator)
        {
            Assert.IsNotNull(enumerator);
            return new Enumerable<T>(enumerator);
        }

        public static List<T> Take<T>(this IEnumerator<T> enumerator, int count)
        {
            Assert.IsNotNull(enumerator);
            FoundationContract.Requires<ArgumentOutOfRangeException>(count >= 0);

            var list = new List<T>(count);

            for (var i = 0; i < count; i++)
            {
                if (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    list.Add(item);
                }
                else
                {
                    break;
                }
            }

            return list;
        }

        #endregion

        #region Private Classes

        private sealed class Enumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> _enumerator;

            public Enumerable(IEnumerator<T> enumerator)
            {
                Assert.IsNotNull(enumerator);
                _enumerator = enumerator;
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => _enumerator;
            IEnumerator IEnumerable.GetEnumerator() => _enumerator;
        }

        #endregion
    }
}