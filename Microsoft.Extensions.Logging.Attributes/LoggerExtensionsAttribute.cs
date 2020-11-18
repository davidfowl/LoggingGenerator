// © Microsoft Corporation. All rights reserved.

namespace Microsoft.Extensions.Logging
{
    using System;
    
    /// <summary>
    /// Indicates an interface should trigger the production of class with strongly-typed logging methods.
    /// </summary>
    /// <remarks>
    /// This attribute is a trigger to cause code generation of strongly-typed methods
    /// that provide a convenient and highly efficient way to log through an existing
    /// ILogger interface.
    /// 
    /// The declaration of the interface to which this attribute is applied cannot be
    /// nested in another type and cannot be generic.
    /// </remarks>
    [System.AttributeUsage(System.AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class LoggerExtensionsAttribute : Attribute
    {
        /// <summary>
        /// Creates an attribute to annotate an interface as triggering the generation of strongly-typed logging methods.
        /// </summary>
        /// <param name="className">Forces the name of the generated class. If this value is null, the class name is derived by
        /// removing the leading I from the interface name.
        /// </param>
        public LoggerExtensionsAttribute(string? className = null) => (ClassName) = (className);

        /// <summary>
        /// Gets or sets the name of the generated class, or null if the name should be auto-generated. 
        /// </summary>
        public string? ClassName { get; set; }
    }
}
