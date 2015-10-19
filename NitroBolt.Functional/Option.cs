using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitroBolt.Functional
{
  public class Option<T>
  {
    public Option(T value)
    {
      this.Value = value;
    }
    public readonly T Value;

    public static implicit operator Option<T>(T value)
    {
      return new Option<T>(value);
    }

  }
  public static class OptionHlp
  {
    public static T Else<T>(this Option<T> option, T value)
    {
      if (option != null)
        return option.Value;
      return value;
    }
  }
}
