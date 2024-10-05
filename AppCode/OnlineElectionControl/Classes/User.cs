using Mysqlx.Resultset;
using System.Text.RegularExpressions;

namespace OnlineElectionControl.Classes
{
    public class User
    {
        // User Properties

        /// <summary>
        /// Id with which this user is stored in the database.
        /// </summary>
        public int? UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// Username with which this user can log into the application.
        /// </summary>
        public string Username;

        /// <summary>
        /// Password with which this user can log into the application.
        /// </summary>
        private string Password;

        /// <summary>
        /// The user's first name.
        /// </summary>
        public string FirstName;

        /// <summary>
        /// The user's last name.
        /// </summary>
        public string LastName;

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email;

        /// <summary>
        /// The user's birthdate.
        /// </summary>
        public DateTime Birthdate;

        /// <summary>
        /// The city where the user currently resides.
        /// </summary>
        public string City;

        /// <summary>
        /// The date on which data should be gathered about this user.
        /// </summary>
        public DateTime ReferenceDate = DateTime.Today;

        /// <summary>
        /// Validation messages, This list contains the issues encountered during validation.
        /// </summary>
        public List<string> Vml = new List<string>();

        // Derived User properties

        /// <summary>
        /// The user's initials without the last name.
        /// </summary>
        public string UserInitials => string.Join(". ", FirstName
                                            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(namePart => namePart[0])) + ".";


        /// <summary>
        /// The user's initials with the last name.
        /// </summary>
        public string UserSortName => UserInitials + " " + LastName;

        /// <summary>
        /// The user's age on the ReferenceDate.
        /// </summary>
        public int UserAge => ReferenceDate.Year 
                            - Birthdate.Year
                            - (ReferenceDate < Birthdate.AddYears(ReferenceDate.Year - Birthdate.Year) ? 1 : 0);

        /// <summary>
        /// If the user is eligible to vote on the ReferenceDate.
        /// </summary>
        public bool UserIsEligible => UserAge >= 18;

        /// <summary>
        /// Constructor to fill a new object.
        /// </summary>
        public User(string pUsername
                  , string pFirstName
                  , string pLastName
                  , string pEmail
                  , DateTime pBirthdate
                  , string pCity)

             : this(pId: null
                  , pUsername: pUsername
                  , pPassword: string.Empty
                  , pFirstName: pFirstName
                  , pLastName: pLastName
                  , pEmail: pEmail
                  , pBirthdate: pBirthdate
                  , pCity: pCity) { }

        /// <summary>
        /// Constructor to get user from the database.
        /// </summary>
        public User(int pId)
        {
            var tmpQuery = @"SELECT Id AS UserId,
                                    Username,
                                    Password,
                                    FirstName,
                                    LastName,
                                    Email,
                                    Birthdate,
                                    City
                               FROM `user`
                              WHERE Id = @pId";
            var tmpParams = new Dictionary<string, object>() { { "@pId",  pId } };

            var tmpResults = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParams);

            if (tmpResults.Count != 1) throw new Exception($"Did not find a user with UserId: {pId}");

            UserId = pId;
            Username = (string) tmpResults[0][nameof(Username)];
            Password = (string) tmpResults[0][nameof(Password)];
            FirstName = (string) tmpResults[0][nameof(FirstName)];
            LastName = (string) tmpResults[0][nameof(LastName)];
            Email = (string) tmpResults[0][nameof(Email)];
            Birthdate = (DateTime) tmpResults[0][nameof(Birthdate)];
            City = (string) tmpResults[0][nameof(City)];
        }

        /// <summary>
        /// Private constructor that allows filling the UserId.
        /// </summary>
        private User(int? pId
                   , string pUsername
                   , string pPassword
                   , string pFirstName
                   , string pLastName
                   , string pEmail
                   , DateTime pBirthdate
                   , string pCity)
        {
            UserId = pId;
            Username = pUsername;
            Password = pPassword;
            FirstName = pFirstName;
            LastName = pLastName;
            Email = pEmail;
            Birthdate = pBirthdate;
            City = pCity;
        }

        // Public Methods
        public bool ValidateObject()
        {
            Vml.Clear();

            // Username validation.
            if (Username.Length < 3) Vml.Add("Username is too short!");
            if (Username.Length > 50) Vml.Add("Username is too long!");
            var tmpQuery = "SELECT Id AS UserId FROM `user` WHERE Username = @Username";
            var tmpParams = new Dictionary<string, object>() { {"@Username", Username} };
            if (UserId != null)
            {
                tmpQuery += " AND Id != @UserId";
                tmpParams["@UserId"] = UserId;
            }
            var tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
            if (tmpResult.Count != 0) Vml.Add("Username already in use!");

            // Password validation.
            if (Password.Length == 0) Vml.Add("Password is not set!");

            // Firstname validation.
            if (FirstName.Length < 3) Vml.Add("First name is too short!");
            if (FirstName.Length > 255) Vml.Add("First name is too long!");

            // Lastname validation.
            if (LastName.Length < 3) Vml.Add("Last name is too short!");
            if (LastName.Length > 255) Vml.Add("Last name is too long!");

            // Email validation.
            if (Email.Length > 255) Vml.Add("Email is too long");
            if (!Email.Contains('@') || !Email.Contains('.')) Vml.Add("Email is invalid!");

            // Birthdate validation.

            // City validation
            if (City.Length < 3) Vml.Add("City is too short!");
            if (City.Length > 64) Vml.Add("City is too long!");

            return Vml.Count == 0;
        }

        public bool SetPassword(string pPassword)
        {
            Vml.Clear();
            if (pPassword.Length < 8) Vml.Add("Password is too short!");
            if (!Regex.IsMatch(pPassword, @"[0-9]")) Vml.Add("Password must contain a number!");
            if (!Regex.IsMatch(pPassword, @"[a-z]")) Vml.Add("Password must contain a lowercase character!");
            if (!Regex.IsMatch(pPassword, @"[A-Z]")) Vml.Add("Password must contain an uppercase character!");

            if (Vml.Count > 0) return false;
            Password = BCrypt.Net.BCrypt.HashPassword(pPassword);
            return true;
        }

        public bool VerifyPassword(string pPassword)
        {
            return BCrypt.Net.BCrypt.Verify(pPassword, Password);
        }

        public bool Save()
        {
            if (!ValidateObject()) return false;

            string tmpQuery = string.Empty;
            var tmpParameters = new Dictionary<string, object>
            {
                { "@UserId", UserId }
              , { "@Username", Username }
              , { "@Password", Password }
              , { "@Firstname", FirstName }
              , { "@Lastname", LastName }
              , { "@Email", Email }
              , { "@Birthdate", Birthdate }
              , { "@City", City }
            };
            if (UserId == null)
            {
                // Create New
                tmpQuery = @"INSERT INTO `user` (Username
                                               , Password
                                               , Firstname
                                               , Lastname
                                               , Email
                                               , Birthdate
                                               , City)
                                         VALUES (@Username
                                               , @Password
                                               , @Firstname
                                               , @Lastname
                                               , @Email
                                               , @Birthdate
                                               , @City);
                             SELECT LAST_INSERT_ID();";

                var tmpResultingId = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

                if (tmpResultingId.Count != 1) throw new Exception("Did not receive the insertion id back from the database!");

                UserId = Convert.ToInt32(tmpResultingId[0]["LAST_INSERT_ID()"]);
            }
            else
            {
                // Update existing
                tmpQuery = @"UPDATE `user` SET Username = @Username
                                             , Password = @Password
                                             , Firstname = @Firstname
                                             , Lastname = @Lastname
                                             , Email = @Email
                                             , Birthdate = @Birthdate
                                             , City = @City 
                                         WHERE Id = @UserId;";

                if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
            }

            return true;
        }

        public bool Delete()
        {
            if (UserId == null) return false;

            var tmpQuery = @"DELETE FROM `user` WHERE Id = @UserId";
            var tmpParameters = new Dictionary<string, object> { { "@UserId", UserId } };

            if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
            return true;
        }

        public static List<User> GetList(DateTime? pReferenceDate = null
                                       , bool pIsEligible = false)
        {
            List<User> tmpUsers = new List<User>();
            var tmpReferenceDate = pReferenceDate ?? DateTime.Today;
            var tmpQuery = @"SELECT Id AS UserId,
                                    Username,
                                    Password,
                                    FirstName,
                                    LastName,
                                    Email,
                                    Birthdate,
                                    City
                               FROM `user`
                              WHERE ((@pIsEligible = 0) OR TIMESTAMPDIFF(YEAR, Birthdate, @ReferenceDate) >= 18)";
            var tmpParameters = new Dictionary<string, object> 
            { 
                { "@pIsEligible", pIsEligible }
              , { "@ReferenceDate", tmpReferenceDate }
            };

            // Future additional checks go here

            tmpQuery += ";";
            var tmpList = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

            foreach (var user in tmpList)
            {
                var tmpUser = new User(pId: (int) user[nameof(UserId)]
                                     , pUsername: (string) user[nameof(Username)]
                                     , pPassword: (string) user[nameof(Password)]
                                     , pFirstName: (string) user[nameof(FirstName)]
                                     , pLastName: (string) user[nameof(LastName)]
                                     , pEmail: (string) user[nameof(Email)]
                                     , pBirthdate: (DateTime) user[nameof(Birthdate)]
                                     , pCity: (string) user[nameof(City)]);

                tmpUser.ReferenceDate = tmpReferenceDate;

                tmpUsers.Add(tmpUser);
            }

            return tmpUsers;
        }
    }
}
