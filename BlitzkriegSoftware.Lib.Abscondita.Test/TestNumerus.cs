using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlitzkriegSoftware.SecureRandomLibrary;

namespace BlitzkriegSoftware.Lib.Abscondita.Test
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TestNumerus
    {

        #region "Boilerplate"
        private const int NumberOfTests = 500;
        private static TestContext testContext;
        private static readonly SecureRandom dice = new SecureRandom();

        [ClassInitialize]
        public static void InitClass(TestContext testContext)
        {
            TestNumerus.testContext = testContext;
        }

        #endregion

        public static void DoTest(int index, long expected)
        {
            long actual;
            string text;
            string el1;
            string el2;

            using (var t1 = new TxTimer2())
            {
                text = Numerus.Hide(expected);
                el1 = TxTimer2.DisplayElaspsedTime(t1.ElapsedMilliseconds);
            }

            using (var t1 = new TxTimer2())
            {
                actual = Numerus.Unhide(text);
                el2 = TxTimer2.DisplayElaspsedTime(t1.ElapsedMilliseconds);
            }

            testContext.WriteLine($"[{index}] {expected} => {text} ({el1}) => {actual} ({el2})");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Quick()
        {
            DoTest(0, 0);
        }

        [TestMethod]
        public void Seq()
        {
            for (int i = 1; i < (NumberOfTests + 1); i++)
            {
                DoTest(i, i);
            }
        }

        [TestMethod]
        public void Sampling1()
        {
            for(int i=0; i< NumberOfTests; i++)
            {
                var expected = dice.Next();
                DoTest(i, expected);
            }
        }

        [TestMethod]
        public void MinMax()
        {
            DoTest(1, int.MinValue);
            DoTest(2, int.MaxValue);
            DoTest(3, long.MinValue);
            DoTest(4, long.MaxValue);
        }

    }
}
