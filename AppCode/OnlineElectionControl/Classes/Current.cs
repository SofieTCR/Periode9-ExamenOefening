namespace OnlineElectionControl.Classes
{
    public class Current
    {
        public static int? LoggedInUserId;

        public static bool UserIsLoggedIn => LoggedInUserId != null;
    }
}
