using CurrencyTranslater.Server.Algorithm;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace CurrencyTranslate.Server.UnitTests
{
    [TestFixture]
    public class CurrencyRepresenterTests
    {
        private Mock<ILanguageProvider> _languageProviderMock;
        private CurrencyRepresenter _currencyRepresenter;

        [SetUp]
        public void Setup()
        {
            _languageProviderMock = new Mock<ILanguageProvider>();
            _currencyRepresenter = new CurrencyRepresenter(_languageProviderMock.Object);
        }

        [Test] public void GetSupportedCulturesReturnsSameAsGetLanguagesReturns()
        {
            var test_cultures = new string[] { "test_language_1", "test_language_1" };
            _languageProviderMock.Setup(provider => provider.GetLanguages()).Returns(test_cultures);

            var result =  _currencyRepresenter.GetSupportedCultures();
            Assert.That(result, Is.EqualTo(test_cultures));
            _languageProviderMock.Verify(provider => provider.GetLanguages(), Times.Once);
        }

        [Test]
        public void WhenIsSupportedIsTrueThenUpdateLanguageSucceeds()
        {
            _languageProviderMock.Setup(provider => provider.IsSupported(It.IsAny<string>())).Returns(true);
            
            var result = _currencyRepresenter.UpdateLanguage("us-EN");
            
            Assert.That(result, Is.True);
            _languageProviderMock.Verify(provider => provider.IsSupported("us-EN"), Times.Once);
            _languageProviderMock.Verify(provider => provider.UpdateActiveLanguage("us-EN"), Times.Once);
        }

        [Test]
        public void WhenIsSupportedIsFalseThenUpdateLanguageFails()
        {
            _languageProviderMock.Setup(provider => provider.IsSupported(It.IsAny<string>())).Returns(false);
            
            var result = _currencyRepresenter.UpdateLanguage("us-EN");
            
            Assert.That(result, Is.False);
            _languageProviderMock.Verify(provider => provider.IsSupported("us-EN"), Times.Once);
            _languageProviderMock.Verify(provider => provider.UpdateActiveLanguage("us-EN"), Times.Never);
        }

        [Test]
        public void WhenGetActiveLanguageSucceedsThenGetWordsSucceeds()
        {
            _languageProviderMock.Setup(provider => provider.GetActiveLanguage()).Returns("text-TEST");
            _languageProviderMock.Setup(provider => provider.GetActiveCultureInfo()).Returns(new CultureInfo("en-US"));

            var output = _currencyRepresenter.GetWord("10");

            Assert.AreEqual(output, "ten US Dollars");
            _languageProviderMock.Verify(provider => provider.GetActiveLanguage(), Times.Once);
            _languageProviderMock.Verify(provider => provider.GetActiveCultureInfo(), Times.Once);
        }

        [Test]
        public void WhenGetActiveLanguageFailsThenGetWordsThrowsInvalidOperationException()
        {
            _languageProviderMock.Setup(provider => provider.GetActiveLanguage()).Returns((string)null);

            Assert.Throws<InvalidOperationException>(() => _currencyRepresenter.GetWord("10"));
        }

        [Test]
        public void WhenLanguageIsUsEnglishThenTranslateToWordSucceeds()
        {
            _languageProviderMock.Setup(provider => provider.GetActiveLanguage()).Returns("text-TEST");
            _languageProviderMock.Setup(provider => provider.GetActiveCultureInfo()).Returns(new CultureInfo("en-US"));

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

            _languageProviderMock.Verify(provider => provider.GetActiveLanguage(), Times.Exactly(6));
            _languageProviderMock.Verify(provider => provider.GetActiveCultureInfo(), Times.Exactly(6));
        }
    }
}