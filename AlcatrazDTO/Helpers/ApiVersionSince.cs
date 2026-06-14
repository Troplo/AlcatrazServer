using System;

namespace Alcatraz.DTO.Versioning
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ApiVersionSinceAttribute : Attribute
    {
        public int Version { get; }
     
        public ApiVersionSinceAttribute(int version)
        {
            Version = version;
        }
    }
}