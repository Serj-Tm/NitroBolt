using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitroBolt.Functional
{
  public static class ExceptionHelper
  {
    public static IEnumerable<string> GetInnerMessages(this Exception exc)
    {
      while (exc != null)
      {
        yield return exc.Message;
        exc = exc.InnerException;
      }
    }
    public static string ToDisplayMessage(this Exception exc)
    {
      return exc.GetInnerMessages().JoinToString(" -> ");
    }
  }
}
