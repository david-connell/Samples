using System;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private static double EctronParseStringToDouble_Pim(char unit, string val)
        {
            int offset = val.IndexOf(unit);
            val = val.Substring(0, offset);

            //double.TryParse(val, out result);
            // Replace the ectron decimal point to whatever the system uses as decimal separator and convert to double
            val = val.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            return System.Convert.ToDouble(val);
        }

        private static double EctronParseStringToDouble(char unit, string val)
        {
            int offset = val.IndexOf(unit);
            val = val.Substring(0, offset);
            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = "."; //The Ectron always does this!
            return Double.Parse(val, nfi);

        }

        [TestMethod]
        public void TextDoubleWithDecimalPoint()
        {
            CultureInfo oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-nl");

            Assert.AreEqual(EctronParseStringToDouble('C', "+20.00C"), 20.0);
        }

        [TestMethod]
        public void TextDoubleWithDecimalComma()
        {
            CultureInfo oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-nl");

            Assert.AreEqual(EctronParseStringToDouble('C', "+20,00C"), 20.0);
        }



    }
}
