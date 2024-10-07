using Xunit;
using FluentAssertions;
using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Tests.ClassesTests
{
    public class ElectionTests
    {
        [Fact]
        public void Election_Status_ReturnsCompleted()
        {
            // Arrange
            var tmpElection = new Election(pName: "name"
                                         , pDescription: "description"
                                         , pDate: DateTime.MinValue
                                          );

            // Act
            var tmpResult = tmpElection.Status;

            // Assert
            tmpResult.Should().Be(ElectionStatus.Completed);
        }

        [Fact]
        public void Election_Status_ReturnsInProgress()
        {
            // Arrange
            var tmpElection = new Election(pName: "name"
                                         , pDescription: "description"
                                         , pDate: DateTime.Today
                                          );

            // Act
            var tmpResult = tmpElection.Status;

            // Assert
            tmpResult.Should().Be(ElectionStatus.InProgress);
        }

        [Fact]
        public void Election_Status_ReturnsScheduled()
        {
            // Arrange
            var tmpElection = new Election(pName: "name"
                                         , pDescription: "description"
                                         , pDate: DateTime.MaxValue
                                          );

            // Act
            var tmpResult = tmpElection.Status;

            // Assert
            tmpResult.Should().Be(ElectionStatus.Scheduled);
        }
    }
}