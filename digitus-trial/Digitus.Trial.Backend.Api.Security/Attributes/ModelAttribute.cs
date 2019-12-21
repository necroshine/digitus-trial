using System;
using System.ComponentModel;

namespace Digitus.Trial.Backend.Api.Security.Attributes
{
    public class ModelAttribute : Attribute
    {
        public ModelAttribute()
        {
            IgnoreIdentitySeed = false;

        }
        public string CollectionName { get; set; }
        public string DatabaseName { get; set; }

        [DefaultValue(false)]
        public bool IgnoreIdentitySeed { get; set; }
    }
}
