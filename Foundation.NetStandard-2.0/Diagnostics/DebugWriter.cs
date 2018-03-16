﻿using System.Diagnostics;
using System.IO;
using System.Text;

namespace Foundation.Diagnostics
{
    /// <summary>
    /// Summary description for DebugWriter.
    /// </summary>
    public class DebugWriter : TextWriter
    {
        private static DebugWriter instance;

        /// <summary>
        /// 
        /// </summary>
        public static DebugWriter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DebugWriter();
                }

                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Encoding Encoding => null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public override void Write(char[] buffer, int index, int count)
        {
            var message = new string(buffer, index, count);
            Debug.Write(message);
        }
    }
}