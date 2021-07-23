using System;
using System.Linq;
using System.Text.RegularExpressions;
using Sieve.Services;
using SieveUnitTests.Abstractions.Entity;
using SieveUnitTests.Entities;

namespace SieveUnitTests.Services
{
    public class SieveCustomSortMethods : ISieveCustomSortMethods
    {
        public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy, bool desc)
        {
            var result = useThenBy ?
                ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount) :
                source.OrderBy(p => p.LikeCount)
                .ThenBy(p => p.CommentCount)
                .ThenBy(p => p.DateCreated);

            return result;
        }

        public IQueryable<IPost> Popularity(IQueryable<IPost> source, bool useThenBy, bool desc)
        {
            var result = useThenBy ?
                ((IOrderedQueryable<IPost>)source).ThenBy(p => p.LikeCount) :
                source.OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated);

            return result;
        }

        public IQueryable<T> Oldest<T>(IQueryable<T> source, bool useThenBy, bool desc) where T : IBaseEntity
        {
            var result = useThenBy ?
                ((IOrderedQueryable<T>)source).ThenByDescending(p => p.DateCreated) :
                source.OrderByDescending(p => p.DateCreated);

            return result;
        }

        private readonly Regex _distancePattern = new Regex(@"\((?<x>[-\d.]+);(?<y>[-\d.]+)\)");
        public IQueryable<Post> Distance(IQueryable<Post> source, bool useThenBy, bool desc, string[] values)
        {
            if (values != null && values[0] != null && _distancePattern.IsMatch(values[0]))
            {
                var matchResult = _distancePattern.Match(values[0]);
                var x = double.Parse(matchResult.Groups["x"].Value);
                var y = double.Parse(matchResult.Groups["y"].Value);

                return source.OrderBy(post => Math.Sqrt(
                    Math.Pow(post.PointForDistanceSorting.X - x, 2) + Math.Pow(post.PointForDistanceSorting.Y - y, 2)));
            }

            return source;
        }
    }
}
