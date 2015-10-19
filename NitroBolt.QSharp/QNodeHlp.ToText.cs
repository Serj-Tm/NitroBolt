using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.QSharp
{
    partial class QNodeHlp
    {

        public static string ToText(this QNode root)
        {
            return ToText(Enumerable.Repeat(root, root != null ? 1 : 0));
        }

        public static string ToText(this IEnumerable<QNode> roots)
        {
            if (roots == null)
                return null;
            if (!roots.Any())
                return "";

            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                var stack = new Stack<QNode[]>();
                var i_stack = new Stack<int>();

                var qs = roots.ToArray();
                var i = 0;

                var builder = new StringBuilder();
                for (;;)
                {
                    for (; i < qs.Length;)
                    {
                        var q = qs[i];
                        var pre = GetPrefix(stack.Count);
                        if (qs.Length > 1)
                            builder.Append(pre);
                        if (q.Value != null)
                        {
                            var text = ValueToText(q.Value);
                            if (text.Length == 0)
                                builder.Append("''");
                            else if (text.All(ch => char.IsLetterOrDigit(ch)))
                                builder.Append(text);
                            else if (!text.Contains("'"))
                            {
                                builder.Append('\'');
                                builder.Append(text);
                                builder.Append('\'');
                            }
                            else if (!text.Contains("\""))
                            {
                                builder.Append('"');
                                builder.Append(text);
                                builder.Append('"');
                            }
                            else
                            {
                                builder.Append('"');
                                builder.Append(text.Replace("\"", "\\\""));
                                builder.Append('"');
                            }
                        }
                        if (q.Nodes.Any())
                        {
                            stack.Push(qs);
                            i_stack.Push(i + 1);

                            qs = q.Nodes.ToArray(); //TODO убрать ToArray
                            i = 0;

                            if (qs.Length > 1)
                            {
                                builder.AppendLine();
                                builder.Append(pre);
                                builder.AppendLine("{");
                            }
                            else
                            {
                                builder.Append(": ");
                            }
                            continue;
                        }
                        else
                        {
                            if (i + 1 < qs.Length)
                            {
                                builder.Append(',');
                            }
                            if (qs.Length > 1)
                                builder.AppendLine();
                        }
                        ++i;
                    }
                    if (stack.Count == 0)
                        break;
                    if (qs.Length > 1)
                    {
                        builder.Append(GetPrefix(stack.Count - 1));
                        builder.Append('}');
                    }
                    qs = stack.Pop();
                    i = i_stack.Pop();
                    if (i < qs.Length)
                        builder.Append(',');
                    if (qs.Length > 1)
                        builder.AppendLine();
                }
                return builder?.ToString();
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            }
        }

        static string[] Prefixes = Enumerable.Range(0, 100).Select(i => new string(' ', i * 2)).ToArray();
        static string GetPrefix(int level)
        {
            var prefixes = Prefixes;//защищаемся от многопоточности

            if (level < prefixes.Length)
                return prefixes[level];
            var len = Math.Max(prefixes.Length * 2, level + 1);
            prefixes = prefixes.Concat(Enumerable.Range(prefixes.Length, len - prefixes.Length).Select(i => new string(' ', i * 2))).ToArray();
            Prefixes = prefixes;
            return prefixes[level];
        }

        static readonly System.Globalization.CultureInfo RuCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-Ru");
        static string ValueToText(object value)
        {
            if (value is DateTime)
                return ((DateTime)value).ToString(RuCulture);
            return value.ToString();
        }
    }
}
