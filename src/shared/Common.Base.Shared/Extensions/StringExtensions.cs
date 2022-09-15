using System.Linq;

namespace System
{
  public static class StringExtensions
  {
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
              null => input,
          "" => input,
          _ => input.First().ToString().ToUpper() + input.Substring(1)
        };
  }
}
