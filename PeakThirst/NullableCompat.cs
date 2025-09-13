// NullableCompat.cs — put this anywhere in your plugin project.
// Minimal compatibility stubs so Unity's runtime can resolve nullable attributes
using System;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class NullableAttribute : Attribute
    {
        public NullableAttribute(byte b) { }
        public NullableAttribute(byte[] b) { }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class NullableContextAttribute : Attribute
    {
        public NullableContextAttribute(byte b) { }
    }
}
