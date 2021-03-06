﻿using System;

namespace Foundation.Linq
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public static class IdentityFunction<TElement>
    {
        /// <summary>
        /// 
        /// </summary>
        public static Func<TElement, TElement> Instance
        {
            get
            {
                return x => x;
            }
        }
    }
}