﻿namespace Foundation
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CommandLineArgument
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public CommandLineArgument(
            int index,
            string name,
            string value)
        {
            Index = index;
            Name = name;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }
    }
}