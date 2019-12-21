using System;
namespace Digitus.Trial.Backend.Api.Security.Attributes
{
    public enum IdentityFieldTypes
    {
        Integer,
        UID
    }
    public class IdentityFieldAttribute : Attribute
    {
        public IdentityFieldAttribute()
        {
        }
        public IdentityFieldTypes IdentityFieldType { get; set; }
    }
}
