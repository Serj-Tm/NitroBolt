using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitroBolt.Functional
{
  public static partial class CollectionHelper
  {
    /// <summary>
    /// разбить коллекцию на части, где каждая часть (кроме последней) состоит из grouplength элементов
    /// </summary>
    public static IEnumerable<IGrouping<int, TElement>> SplitByCount<TElement>(this IEnumerable<TElement> items, int groupLength)
    {
      return SplitterByCount<TElement>.SplitByCount(items, groupLength);
    }

  }
  ///// <summary>
  ///// Тесты
  ///// </summary>
  //public static partial class Tests
  //{
  //  /// <summary>
  //  /// Тест на функцию IEnumerable&lt;TElement&gt;.SplitByCount
  //  /// </summary>
  //  public static void Test_SplitByCount(AssertManager manager)
  //  {
  //    var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

  //    manager.Assert(items.SplitByCount(2).Count(), 5);
  //    manager.Assert(items.SplitByCount(3).Count(), 4);
  //    manager.Assert(items.SplitByCount(1).Count(), 10);
  //    manager.Assert(Array<int>.Empty.SplitByCount(3).Count(), 0);
  //    manager.AssertTryCatch<ArgumentException>(() => { items.SplitByCount(0).Count(); });
  //    manager.SequenceAssert(items.SplitByCount(2).First().ToArray(), new[] { 1, 2 });
  //    manager.SequenceAssert(items.SplitByCount(2).SelectMany(group => group).ToArray(), items);
  //  }
  //}


  /// <summary>
  /// Вспомогательный класс для функции IEnumerable&lt;TElement&gt;.SplitByCount
  /// </summary>
  /// <typeparam name="TElement"></typeparam>
  public class SplitterByCount<TElement> : IGrouping<int, TElement>
  {
    SplitterByCount(int groupIndex, Iterator iterator)
    {
      this.groupIndex = groupIndex;
      this.iterator = iterator;
    }
    readonly Iterator iterator;
    readonly int groupIndex;

    /// <summary>
    /// Порядковый номер группы. начинается с 0
    /// </summary>
    public int Key
    {
      get { return groupIndex; }
    }

    /// <summary>
    /// Максимальное возможное кол-во элементов в группе. Равное groupLength из SplitByCount
    /// </summary>
    public int MaxSize
    {
      get { return iterator.GroupLength; }
    }

    /// <summary>
    /// Получить элементы в группе
    /// </summary>
    public IEnumerator<TElement> GetEnumerator()
    {
      if (iterator.Position != groupIndex * iterator.GroupLength)
        throw new Exception("поддерживается только последовательный перебор групп");
      for (int i = 0; i < iterator.GroupLength; ++i)
      {
        yield return iterator.Enumerator.Current;
        if (!iterator.MoveNext())
          break;
      }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    
    internal static IEnumerable<IGrouping<int, TElement>> SplitByCount(IEnumerable<TElement> items, int groupLength)
    {
      var iterator = new Iterator(items.GetEnumerator(), groupLength);
      if (!iterator.MoveNext())
        yield break;

      if (groupLength <= 0)
        throw new ArgumentException(string.Format("Длина группы '{0}' должна быть больше 0", groupLength), "groupLength");

      for (int i = 0; ; ++i)
      {
        for (int j = iterator.Position; j < i * groupLength; ++j)
        {
          if (!iterator.MoveNext())
            yield break;
        }
        if (!iterator.IsNext)
          yield break;
        yield return new SplitterByCount<TElement>(i, iterator);
      }
    }


    class Iterator
    {
      public Iterator(IEnumerator<TElement> enumerator, int groupLength)
      {
        this.Enumerator = enumerator;
        this.GroupLength = groupLength;
      }
      public readonly IEnumerator<TElement> Enumerator;
      public int Position = -1;
      public readonly int GroupLength;
      public bool IsNext = false;
      public bool MoveNext()
      {
        Position++;
        this.IsNext = Enumerator.MoveNext();
        return IsNext;
      }
    }

  }

}
