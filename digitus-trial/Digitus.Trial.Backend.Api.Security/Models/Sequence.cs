using System;
using Digitus.Trial.Backend.Api.Attributes;

namespace Digitus.Trial.Backend.Api.Models
{
    [Model(CollectionName = "Sequence")]
    public class Sequence : ModelBase
    {

        public string SequenceName { get; set; }

        public int SequenceValue { get; set; }
    }
}
