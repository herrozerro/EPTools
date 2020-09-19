using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPTools.Blazor.MyExtensions
{
    public static class MyExtensions
    {
		public static string Replace(this string s, int index, int length, string replacement)
		{
			var builder = new StringBuilder();
			builder.Append(s.Substring(0, index));
			builder.Append(replacement);
			builder.Append(s.Substring(index + length));
			return builder.ToString();
		}
	}
}
