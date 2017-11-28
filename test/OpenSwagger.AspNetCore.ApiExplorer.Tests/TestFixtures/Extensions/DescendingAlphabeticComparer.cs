using System.Collections.Generic;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.TestFixtures.Extensions
{
    public class DescendingAlphabeticComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return y.CompareTo(x);
        }
    }
}