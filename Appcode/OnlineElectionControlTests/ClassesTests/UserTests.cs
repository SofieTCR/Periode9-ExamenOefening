using Xunit;
using FluentAssertions;
using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Tests.ClassesTests
{
    public class UserTests
    {
        [Theory]
        [InlineData("Jesus Herbert", "J. H.")]
        [InlineData("Sofie", "S.")]
        [InlineData("Rebecca Delilah", "R. D.")]
        [InlineData("Jack", "J.")]
        [InlineData("Adolph Blaine Charles David Earl Fredrick Gerald Hubert Irvin John", "A. B. C. D. E. F. G. H. I. J.")]
        public void User_UserInitials_ReturnsInitials(string pFirstName, string pInitials)
        {
            // Arrange
            var tmpUser = new User(pUsername: "username"
                                 , pPassword: "password"
                                 , pFirstName: pFirstName
                                 , pLastName: "lastname"
                                 , pEmail: "email"
                                 , pBirthdate: DateTime.Now
                                 , pCity: "city"
                                  );
            // Act
            var result = tmpUser.UserInitials;

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain(".");
            result.Should().Be(pInitials);
        }

        [Theory]
        [InlineData(new[] { "Jesus Herbert", "Christ" }, "J. H. Christ")]
        [InlineData(new[] { "Sofie", "Brink" }, "S. Brink")]
        [InlineData(new[] { "Rebecca Delilah", "de Jong" }, "R. D. de Jong")]
        [InlineData(new[] { "Jack", "van Bommel" }, "J. van Bommel")]
        [InlineData(new[] { "Adolph Blaine Charles David Earl Fredrick Gerald Hubert Irvin John", "Wolfgang" }, "A. B. C. D. E. F. G. H. I. J. Wolfgang")]
        public void User_UserSortName_ReturnsSortname(string[] pName, string pResult)
        {
            // Arrange
            var tmpUser = new User(pUsername: "username"
                                 , pPassword: "password"
                                 , pFirstName: pName[0]
                                 , pLastName: pName[1]
                                 , pEmail: "email"
                                 , pBirthdate: DateTime.Now
                                 , pCity: "city"
                                  );
            // Act
            var result = tmpUser.UserSortName;

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain(".");
            result.Should().Be(pResult);
        }

        [Theory]
        [InlineData(new[] { 1945, 05, 05 }, new[] { 1969, 07, 20 }, 24)]
        [InlineData(new[] { 1990, 03, 12 }, new[] { 2024, 10, 04 }, 34)]
        [InlineData(new[] { 1985, 07, 22 }, new[] { 2023, 01, 01 }, 37)]
        [InlineData(new[] { 2002, 11, 08 }, new[] { 2021, 07, 15 }, 18)]
        [InlineData(new[] { 1978, 04, 05 }, new[] { 2020, 12, 25 }, 42)]
        public void User_UserAge_ReturnsAge(int[] pBirthdate, int[] pReferenceDate, int pResult)
        {
            // Arrange
            var tmpBirthdate = new DateTime(year: pBirthdate[0], month: pBirthdate[1], day: pBirthdate[2]);
            var tmpReferenceDate = new DateTime(year: pReferenceDate[0], month: pReferenceDate[1], day: pReferenceDate[2]);
            var tmpUser = new User(pUsername: "username"
                                 , pPassword: "password"
                                 , pFirstName: "firstname"
                                 , pLastName: "lastname"
                                 , pEmail: "email"
                                 , pBirthdate: tmpBirthdate
                                 , pCity: "city"
                                  );

            // Act
            tmpUser.ReferenceDate = tmpReferenceDate;
            var result = tmpUser.UserAge;

            // Assert
            result.Should().Be(pResult);
        }
    }
}
