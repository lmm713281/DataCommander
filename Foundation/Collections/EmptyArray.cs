﻿#if FOUNDATION_4_6 || FOUNDATION_4_7

namespace Foundation.Collections
{
    internal static class EmptyArray<T>
    {
        public static readonly T[] Value = new T[0];
    }
}

#endif