﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Foundation.Diagnostics.Contracts;

namespace Foundation
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct SmallDateInterval
    {
        public readonly SmallDate Start;
        public readonly SmallDate End;

        public SmallDateInterval(SmallDate start, SmallDate end)
        {
            FoundationContract.Requires<ArgumentException>(start <= end);
            Start = start;
            End = end;
        }

        [Pure]
        public bool Contains(SmallDate date)
        {
            var contains = Start.Value <= date.Value && date.Value <= End.Value;
            return contains;
        }

        [Pure]
        public bool Contains(SmallDateInterval other)
        {
            var contains = Start.Value <= other.Start.Value && other.End.Value <= End.Value;
            return contains;
        }

        [Pure]
        public SmallDateInterval? Intersect(SmallDateInterval other)
        {
            var start = ElementPair.Max(Start, other.Start);
            var end = ElementPair.Min(End, other.End);
            var intersects = start <= end;
            return intersects
                ? new SmallDateInterval(start, end)
                : (SmallDateInterval?) null;
        }

        [Pure]
        public bool Intersects(SmallDateInterval other)
        {
            var start = ElementPair.Max(Start, other.Start);
            var end = ElementPair.Min(End, other.End);
            var intersects = start <= end;
            return intersects;
        }

        [Pure]
        public int GetLength()
        {
            var length = End - Start + 1;
            return length;
        }

        [Pure]
        public IEnumerable<SmallDate> GetDates()
        {
            for (var date = Start; date <= End; date = date.AddDays(1))
                yield return date;
        }

        [Pure]
        public FoundationDateTimeInterval ToFoundationDateTimeInterval()
        {
            var start = Start.ToDateTime();
            var end = End.ToDateTime().AddDays(1);
            return new FoundationDateTimeInterval(start, end);
        }

        private string DebuggerDisplay => $"{Start.DebuggerDisplay}-{End.DebuggerDisplay}";
    }
}