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
        /// Constructor to fill a new object.
        /// </summary>
        public User(string pUsername
                  , string pPassword
                  , string pFirstName
                  , string pLastName
                  , string pEmail
                  , DateTime pBirthdate
                  , string pCity)

             : this(pId: null
                  , pUsername: pUsername
                  , pPassword: pPassword
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
    }
}
