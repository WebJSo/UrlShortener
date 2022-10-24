using Shared.Extensions;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Tests ShortUrlExtensionTests public methods to ensure url format and validity standard is maintained
    /// </summary>
    public class ShortUrlExtensionTests
    {
        [Theory]
        [InlineData("ht", "https://ht")]
        [InlineData("http://www.google.com", "http://www.google.com")]
        [InlineData("https://www.google.com?test=true", "https://www.google.com?test=true")]
        [InlineData("http://www.goo/questions/5627", "http://www.goo/questions/5627")]
        [InlineData("https://www./", "https://www./")]
        [InlineData("www.base64encode.org/", "https://www.base64encode.org/")]
        [InlineData("www.google.com", "https://www.google.com")]
        [InlineData("://www.base64encode.org/", "https://www.base64encode.org/")]
        [InlineData("http//www.google.com", "http//www.google.com")]
        [InlineData("h:www.base64encode.org/", "https://h:www.base64encode.org/")]
        public void TestFormatUrlMatchesCorrectUrlFormat(string input, string output)
        {
            // output url matches formatted url
            Assert.Equal(output, ShortUrlExtensions.FormatUrl(input));
        }

        [Theory]
        [InlineData("http://www.google.com", true)]
        [InlineData("https://www.google.com?test=true&tester=false", true)]
        [InlineData("http://www.goo/questions/5627", true)]
        [InlineData("https://www.base64encode.org/", true)]
        [InlineData("https://www./", false)]
        [InlineData("h:www.base64encode.org/", false)]
        [InlineData("://www.base64encode.org/", false)]
        [InlineData("ht", false)]
        [InlineData("www.google.com", false)]
        [InlineData("http//www.google.com", false)]
        public void TestValidUrlMatchesCorrectValidStatus(string input, bool output)
        {
            // output bool status matches validUrl status
            Assert.Equal(output, ShortUrlExtensions.ValidUrl(input));
        }

        [Theory]
        [InlineData("www.google.com", true)]
        [InlineData("://www.base64encode.org/", true)]
        [InlineData("h:www.base64encode.org/", false)]
        public void TestFormatThenValidUrlMatchesCorrectValidStatus(string input, bool output)
        {
            // output bool status matches validUrl status when formatted first
            Assert.Equal(output, ShortUrlExtensions.FormatUrl(input).ValidUrl());
        }

        [Theory]
        [InlineData(1, "MQ")]
        [InlineData(2, "Mg")]
        [InlineData(30, "MzA")]
        [InlineData(42, "NDI")]
        [InlineData(501, "NTAx")]
        [InlineData(650, "NjUw")]
        [InlineData(12000, "MTIwMDA")]
        [InlineData(14052, "MTQwNTI")]
        [InlineData(160520, "MTYwNTIw")]
        [InlineData(1807250, "MTgwNzI1MA")]
        public void TestShortUrlMatchesTrimmedBase64String(int input, string output)
        {
            // output short url matches ShortUrl result from input
            Assert.Equal(output, ShortUrlExtensions.ShortUrl(input));
        }
    }
}
