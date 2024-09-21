using Xunit;
using FluentAssertions;
using OnlineElectionControl.Models;

namespace OnlineElectionControl.Tests.ModelTests
{
    public class HomeModelTests
    {
        [Theory]
        [InlineData("Some string with four t's in it, for real", 5, new[] { "Some s", "ring wi", "h four ", "'s in i", ", for real" })]
        [InlineData("Test the string, testing", 6, new[] { "Tes", " ", "he s", "ring, ", "es", "ing" })]
        [InlineData("A totally random test string", 6, new[] { "A ", "o", "ally random ", "es", " s", "ring" })]
        [InlineData("This is just a test", 4, new[] { "This is jus", " a ", "es", "" })]
        [InlineData("No t in this sentence!", 4, new[] { "No ", " in ", "his sen", "ence!" })]
        [InlineData("Tiny test with multiple t's", 6, new[] { "Tiny ", "es", " wi", "h mul", "iple ", "'s" })]
        [InlineData("The quick brown fox", 1, new[] { "The quick brown fox" })]


        public void HomeModel_SplitOnT_ReturnStringArray(string inputString, int outputCount, string[] resultingArray)
        {
            // Arrange
            var homeModel = new HomeModel();

            // Act
            var result = homeModel.SplitOnT(inputString);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(outputCount);
            result.Should().BeEquivalentTo(resultingArray);
        }
    }
}
