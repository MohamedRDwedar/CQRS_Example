using System;

namespace TestPlanning.Common.Decorators
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ConsumeMessageAttribute : Attribute
    {

    }
}
