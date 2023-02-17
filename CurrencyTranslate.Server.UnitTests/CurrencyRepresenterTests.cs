using CurrencyTranslater.Server.Algorithm;
using NUnit.Framework;

namespace CurrencyTranslate.Server.UnitTests
{
    public class CurrencyRepresenterTests
    {
        private CurrencyRepresenter _currencyRepresenter;

        [SetUp]
        public void Setup()
        {
            _currencyRepresenter = new CurrencyRepresenter();
            _currencyRepresenter.UpdateLanguage("en-US");
        }

        [Test]
        public void WhenCultureIsUsEnglishThenTranslateToWordSucceeds()
        {
            var output = _currencyRepresenter.GetWord("0");
            Assert.AreEqual(output, "zero US Dollars");

            output = _currencyRepresenter.GetWord("1");
            Assert.AreEqual(output, "one US Dollar");

            output = _currencyRepresenter.GetWord("25.1");
            Assert.AreEqual(output, "twenty-five US Dollars and ten Cents");

            output = _currencyRepresenter.GetWord("0.01");
            Assert.AreEqual(output, "zero US Dollars and one Cent");

            output = _currencyRepresenter.GetWord("45100");
            Assert.AreEqual(output, "forty-five thousand one hundred US Dollars");

            output = _currencyRepresenter.GetWord("999999999.99");
            Assert.AreEqual(output, "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine US Dollars and ninety-nine Cents");
        }
    }
}