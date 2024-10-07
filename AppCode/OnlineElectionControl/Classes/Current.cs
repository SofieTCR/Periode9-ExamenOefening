namespace OnlineElectionControl.Classes
{
    public class Current
    {
        public static int? LoggedInUserId;

        public static bool UserIsLoggedIn => LoggedInUserId != null;

        private static User _loggedInUser;

        public static User? LoggedInUser
        {
            get
            {
                if (LoggedInUserId == null) return null;

                if (_loggedInUser == null || _loggedInUser.UserId != LoggedInUserId) _loggedInUser = new User(pId: (int) LoggedInUserId);

                return _loggedInUser;
            }
        }
    }
}
