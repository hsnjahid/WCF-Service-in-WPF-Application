using CurrencyTranslater.Server.Algorithm;
using NUnit.Framework;

namespace CurrencyTranslate.Server.UnitTests
{
    public class CurrencyRepresenterTests
    {
        [Test]
        public void TestRepresentsToDollar()
        {
            var output = CurrencyRepresenter.RepresentsToDollar(0);
            Assert.AreEqual(output, "zero dollars");

            output = CurrencyRepresenter.RepresentsToDollar(1);
            Assert.AreEqual(output, "one dollar");

            output = CurrencyRepresenter.RepresentsToDollar(25.1);
            Assert.AreEqual(output, "twenty-five dollars and ten cents");

            output = CurrencyRepresenter.RepresentsToDollar(0.01);
            Assert.AreEqual(output, "zero dollars and one cent");

            output = CurrencyRepresenter.RepresentsToDollar(45100);
            Assert.AreEqual(output, "forty-five thousand one hundred dollars");

            output = CurrencyRepresenter.RepresentsToDollar(999999999.99);
            Assert.AreEqual(output, "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents");
        }
    }
}