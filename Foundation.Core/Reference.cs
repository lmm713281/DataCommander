﻿using System;

namespace Foundation
{
    public sealed class Reference : IDisposable
    {
        private ReferenceCounter _referenceCounter;

        private Reference(ReferenceCounter referenceCounter)
        {
            if (referenceCounter == null)
                throw new ArgumentNullException();

            _referenceCounter = referenceCounter;
        }

        public static Reference Add(ReferenceCounter referenceCounter)
        {
            var reference = new Reference(referenceCounter);
            referenceCounter.Add();
            return reference;
        }

        public void Remove()
        {
            if (_referenceCounter != null)
            {
                _referenceCounter.Remove();
                _referenceCounter = null;
            }
        }

        ~Reference() => Dispose();
        public void Dispose() => Remove();
    }
}