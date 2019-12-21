using System;
using Digitus.Trial.Backend.Api.Security.Attributes;

namespace Digitus.Trial.Backend.Api.Security.Models
{
    [Model(CollectionName = "Sequence")]
    public class Sequence : ModelBase
    {

        public string SequenceName { get; set; }

        public int SequenceValue { get; set; }
    }
}
