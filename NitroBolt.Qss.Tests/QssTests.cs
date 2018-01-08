using Microsoft.VisualStudio.TestTools.UnitTesting;
using NitroBolt.Functional;
using NitroBolt.QSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NitroBolt.Qss.QssTransform;

namespace NitroBolt.Qss.Tests
{
    [TestClass]
    public class QssTests
    {
        [TestMethod]
        public void Qss_OperatorTest()
        {
            var text = "1 '->' 2";
            var nodes = QParser.Parse(text);
            Assert.AreEqual("->", nodes[1]?.Value);

            var collapsed = TreeProcess(nodes, ArrowCollapse).ToArray();
            Assert.AreEqual("->", collapsed[0]?.Value);
            Assert.AreEqual("1", collapsed[0]?.Nodes?.ElementAtOrDefault(0)?.Value);
            Assert.AreEqual("2", collapsed[0]?.Nodes?.ElementAtOrDefault(1)?.Value);
        }

    }
}
