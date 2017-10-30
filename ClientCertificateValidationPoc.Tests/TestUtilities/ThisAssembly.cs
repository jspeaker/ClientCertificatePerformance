﻿using System;
using System.Reflection;

namespace ClientCertificateValidationPoc.Tests.TestUtilities
{
    public interface IThisAssembly
    {
        string Path();
    }

    public class ThisAssembly : IThisAssembly
    {
        public string Path()
        {
            return System.IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        }
    }
}