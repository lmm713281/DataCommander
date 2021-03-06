﻿using System;
using System.Collections;

namespace Foundation.Collections
{
    internal sealed class EmptyNonGenericEnumerator : IEnumerator
    {
        public static readonly EmptyNonGenericEnumerator Value = new EmptyNonGenericEnumerator();

        private EmptyNonGenericEnumerator()
        {
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }

        public object Current => throw new InvalidOperationException();
    }
}