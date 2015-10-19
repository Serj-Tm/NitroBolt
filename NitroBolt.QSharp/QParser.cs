using System;
using System.Collections.Generic;
using System.Linq;


namespace NitroBolt.QSharp
{
  public class QParser
  {
    const char Quote = '\'';
    const char DoubleQuote = '"';
    public static QNode[] Parse(string text, int index = 0, int state = 0)
    {
      var stack = new Stack<ParseInfo>();
      var info = new ParseInfo();

      Action PopWhileSingle = () =>
      {
        for (; stack.Count > 0 && info.isSingle == true; )
        {
          var parent = stack.Pop();
          parent.PushChild(info);
          info = parent;
        }
      };
      var lexemBuilder = new System.Text.StringBuilder();
      for (; ; )
      {
        char ch = index < text.Length ? text[index] : (char)0;
        var isEnd = index >= text.Length;
        if (state == 0)
        {
          if (index >= text.Length)
          {
            PopWhileSingle();
            if (stack.Count > 0)
              throw new Exception($"Не хватает закрывающихся скобок - '{stack.Count}' шт.");

            return info.allElements;
          }
          else if (char.IsWhiteSpace(ch))
          {
            index++;
            continue;
          }
          else if (ch == Quote)
          {
            state = 2;
            lexemBuilder = new System.Text.StringBuilder();
            index++;
            continue;
          }
          else if (ch == DoubleQuote)
          {
            state = 3;
            lexemBuilder = new System.Text.StringBuilder();
            index++;
            continue;
          }
          else if (char.IsLetter(ch) || char.IsDigit(ch) || ch == '-' || ch == '_')
          {
            state = 1;
            lexemBuilder = new System.Text.StringBuilder();
            lexemBuilder.Append(ch);
            index++;
            continue;
          }
          else if (ch == ':' && (index + 1) < text.Length && text[index + 1] == ':')
          {
            state = 1;
            lexemBuilder = new System.Text.StringBuilder();
            lexemBuilder.Append(text.Substring(index, 2));
            index += 2;
          }
          else if (ch == ':')
          {
            stack.Push(info);
            info = new ParseInfo { isSingle = true };
            index++;
            continue;
          }
          else if (ch == '{' || ch == '(')
          {
            stack.Push(info);
            info = new ParseInfo();
            index++;
            continue;
          }
          else if (ch == '}' || ch == ')')
          {
            PopWhileSingle();
            var parentInfo = stack.Pop();
            parentInfo.PushChild(info);
            info = parentInfo;
            index++;
            continue;
          }
          else if (ch == ',' || ch == ';')
          {
            PopWhileSingle();
            info.PushCurrent(null);
            index++;
            continue;
          }
          else if (ch == '/' && (index + 1) < text.Length && text[index + 1] == '/')
          {
            state = 4;
            lexemBuilder = new System.Text.StringBuilder();
            index += 2;
            continue;
          }
          else
          {
            throw new Exception(string.Format("invalid char in index:{0}", index));
            //index++;
            //continue;
          }
        }
        else if (state == 1)
        {
          if (char.IsLetter(ch) || char.IsDigit(ch) || ch == '-' || ch == '_')
          {
            lexemBuilder.Append(ch);
            index++;
            continue;
          }
          else
          {
            var q = new QNode(lexemBuilder.ToString());
            info.PushCurrent(q);
            state = 0;
            continue;
          }
        }
        else if (state == 2 || state == 3)
        {
          var quote = state == 2 ? Quote : DoubleQuote;
          if (isEnd)
            throw new Exception(string.Format("не законченная строка '{0}'", lexemBuilder));
          else if (ch == quote)
          {
            if ((index + 1) < text.Length && text[index + 1] == quote)
            {
              lexemBuilder.Append(quote);
              index += 2;
              continue;
            }
            else
            {
              var q = new QNode(lexemBuilder.ToString());
              info.PushCurrent(q);
              state = 0;
              index++;
              continue;
            }
          }
          else
          {
            lexemBuilder.Append(ch);
            index++;
            continue;
          }
        }
        else if (state == 4)
        {
          if (ch == '\r' || ch == '\n')
          {
            //var q = new QElement(lexemBuilder.ToString());
            //info.PushCurrent(q);
            state = 0;
            index++;
            continue;
          }
          else
          {
            lexemBuilder.Append(ch);
            index++;
            continue;
          }
        }

      }
    }
    class ParseInfo
    {
      public List<QNode> prevElements = new List<QNode>();
      public QNode current;
      public QNode[] allElements
      {
        get
        {
          if (current == null)
            return prevElements.ToArray();
          return prevElements.Concat(Enumerable.Repeat(current, 1)).ToArray();
        }
      }
      public bool isSingle = false;

      public void PushCurrent(QNode newCurrent)
      {
        if (current != null)
          prevElements.Add(current);
        current = newCurrent;
      }
      public void PushChild(ParseInfo info)
      {
        var parentInfo = this;
        if (parentInfo.current != null)
        {
          parentInfo.current = new QNode(parentInfo.current.Value, parentInfo.current.Nodes.Concat(info.allElements).ToArray());
        }
        else
        {
          parentInfo.prevElements = parentInfo.allElements.Concat(info.allElements).ToList();
        }
      }
    }
  }
}