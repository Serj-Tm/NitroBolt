using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroBolt.QSharp.Tests
{
    [TestClass]

    public class QNodeToTextTests
    {
        [TestMethod]
        public void Number()
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
                Assert.AreEqual("1,3", 1.3.ToString());

                var text = new QNode(1.3).ToText();
                Assert.AreEqual("'1.3'", text);
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            }

        }
    }
}
