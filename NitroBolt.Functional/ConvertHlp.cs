using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitroBolt.Functional
{
  public class ConvertHlp
  {
    [System.Diagnostics.DebuggerStepThrough]
    public static double? ToDouble(object value)
    {
      try
      {
        if (value == null)
          return null;
        if (value is string)
        {
          string s = (string)value;
          return double.Parse(s.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
        }
        return Convert.ToDouble(value);
      }
      catch (Exception)
      {
        //    TraceHlp2.WriteException(exc);
      }
      return null;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static Guid? ToGuid(object value)
    {
      try
      {
        if (value == null)
          return null;
        if (value is Guid)
          return (Guid)value;
        else if (value is byte[])
          return new Guid((byte[])value);
        else if (value is string)
        {
          var s = (string)value;
          if (s == "")
            return null;
          return new Guid(s);
        }
        else
          return new Guid(value.ToString());
      }
      catch (Exception)
      {
      }
      return null;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static int? ToInt(object value)
    {
      try
      {
        if (value == null)
          return null;
        return Convert.ToInt32(value);
      }
      catch (Exception)
      {
      }
      return null;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static long? ToLong(object value)
    {
      try
      {
        if (value == null)
          return null;
        return Convert.ToInt64(value);
      }
      catch (Exception)
      {
      }
      return null;
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static TEnum? ToEnum<TEnum>(object value) where TEnum : struct
    {
      if (value is TEnum)
        return (TEnum)value;
      var text = value?.ToString();
      if (text == null)
        return null;
      TEnum result;
      if (Enum.TryParse<TEnum>(text, out result))
        return result;
      return null;
    }
  }
}
