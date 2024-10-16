namespace OnlineElectionControl.Classes
{
    public class Current
    {
        public static int? LoggedInUserId;

        public static bool UserIsLoggedIn => LoggedInUserId != null;

        private static User? _loggedInUser;

        public static User? LoggedInUser
        {
            get
            {
                if (LoggedInUserId == null) return null;

                if (_loggedInUser == null || _loggedInUser.UserId != LoggedInUserId) _loggedInUser = new User(pId: (int) LoggedInUserId);

                return _loggedInUser;
            }
        }

        public static void DeleteCache()
        {
            _loggedInUser = null;
        }

        public static bool UserCanVote(int pElectionId)
        {
            if (!UserIsLoggedIn || !LoggedInUser!.UserIsEligible) return false;
            var tmpQuery = "SELECT Voter_UserId FROM `vote` WHERE `vote`.Voter_UserId = @pUserId AND `vote`.Voted_ElectionId = @pElectionId;";
            var tmpParams = new Dictionary<string, object> { { "@pUserId", LoggedInUserId! }, { "@pElectionId", pElectionId } };
            return Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParams).Count == 0;
        }
    }
}
