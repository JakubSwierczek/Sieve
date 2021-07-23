using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sieve.Models
{
    public class SortTerm : ISortTerm, IEquatable<SortTerm>
    {
        private const string EscapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";
        private static readonly string[] Operators = new string[] {
            "==",
        };

        public SortTerm() { }

        public string Sort
        {
            set
            {
                var filterSplits = value
                   .Split(Operators, StringSplitOptions.RemoveEmptyEntries)
                   .Select(t => t.Trim()).ToArray();

                var name = filterSplits[0];


                Name = name.StartsWith("-") ? name[1..] : name;
                Descending = name.StartsWith("-");
                Values = filterSplits.Length > 1 ? Regex.Split(filterSplits[1], EscapedPipePattern).Select(t => t.Trim()).ToArray() : null;
            }
        }

        public string Name { get; private set; }

        public bool Descending { get; private set; }

        public string[] Values { get; private set; }

        public bool Equals(SortTerm other)
        {
            return other != null
                   && Name == other.Name
                   && Values.SequenceEqual(other.Values)
                   && Descending == other.Descending;
        }
    }
}
