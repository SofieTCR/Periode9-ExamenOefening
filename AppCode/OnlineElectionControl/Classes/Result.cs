namespace OnlineElectionControl.Classes
{
    public class Result
    {
        // Result properties

        // Election id
        public int ElectionId
        {
            get;
            private set;
        }

        // Election name
        public string ElectionName;

        // Election Date
        public DateTime ElectionDate;

        // User id
        public int ElectableMemberId
        {
            get;
            private set;
        }

        // Electable Member First Name
        public string? ElectableMemberFirstName;

        // Electable Member Last Name
        public string? ElectableMemberLastName;

        // Electable Member City
        public string ElectableMemberCity;

        // List with voter Results
        public List<Result>? Results;

        // Derived Properties
        // Electable Member Full Name
        public string ElectableMemberFullName;

        public static List<Result> GetList(int pElectionId)
        {
            var tmpQuery = @"SELECT election.Id AS ElectionId, election.Name AS ElectionName, election.Date AS ElectionDate, ElectedMember_UserId AS ElectableMemberId, user.Firstname AS ElectableMemberFirstName, user.Lastname AS ElectableMemberLastName, user.City AS ElectableMemberCity
                             FROM vote
                             INNER JOIN election ON election.Id = Voted_ElectionId 
                             INNER JOIN electablemember ON electablemember.User_UserId = ElectedMember_UserId 
                             INNER JOIN user ON user.Id = ElectedMember_UserId
                             WHERE election.id = @pElectionId";

            var tmpParams = new Dictionary<string, object>() { { "@pElectionId", pElectionId } };
            var tmpResults = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParams);

            List<Result> ResultsList = new List<Result>();
            foreach (var tmpResult in tmpResults)
            {
                ResultsList.Add(new Result(
                    pElectionId: (int)tmpResult[nameof(ElectionId)],
                    pElectionName: (string)tmpResult[nameof(ElectionName)],
                    pElectionDate: (DateTime)tmpResult[nameof(ElectionDate)],
                    pElectableMemberId: (int)tmpResult[nameof(ElectableMemberId)],
                    pElectableMemberCity: (string)tmpResult[nameof(ElectableMemberCity)],
                    pElectableMemberFullName: $"{tmpResult[nameof(ElectableMemberFirstName)]} {tmpResult[nameof(ElectableMemberLastName)]}"
                ));
            }

            return ResultsList;
        }

        // Constructor
        public Result(int pElectionId, string pElectionName, DateTime pElectionDate, int pElectableMemberId, string pElectableMemberCity, string pElectableMemberFullName)
        {
            ElectionId = pElectionId;
            ElectionName = pElectionName;
            ElectionDate = pElectionDate;
            ElectableMemberId = pElectableMemberId;
            ElectableMemberCity = pElectableMemberCity;
            ElectableMemberFullName = pElectableMemberFullName;
        }
    }
}
