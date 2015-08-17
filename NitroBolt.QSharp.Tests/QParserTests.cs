using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NitroBolt.QSharp.Tests
{
  [TestClass]
  public class QParserTests
  {
    [TestMethod]
    public void QParser_SimpleParse()
    {
      var qs = QParser.Parse("div{x, 1}");
      Assert.IsNotNull(qs);
      Assert.AreEqual(1, qs.Length);
      var q = qs.FirstOrDefault();
      Assert.AreEqual("div", q.Value?.ToString());
      Assert.AreEqual(2, q.Nodes.Count());
      Assert.AreEqual("x", q.Nodes.ElementAtOrDefault(0).AsString());
      Assert.AreEqual("1", q.Nodes.ElementAtOrDefault(1).AsString());
    }
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    //[Ignore]
    public void QParser_MissedBracket()
    {
      var qs = QParser.Parse("div{x, 1");
    }
  }
}
