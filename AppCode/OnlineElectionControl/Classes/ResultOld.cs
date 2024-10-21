using System.Runtime.CompilerServices;

namespace OnlineElectionControl.Classes
{
    public class ResultOld_kech
    {
        // Result properties

        //// Election id
        //public int ElectionId
        //{
        //    get;
        //    private set;
        //}

        //// Election name
        //public string ElectionName;

        //// Election Date
        //public DateTime ElectionDate;

        //// User id
        //public int ElectableMemberId
        //{
        //    get;
        //    private set;
        //}

        //// Electable Member First Name
        //public string? ElectableMemberFirstName;

        // Electable Member Last Name
        //        public string? ElectableMemberLastName;

        //        // Voter's City
        //        public string VoterCity;

        //        // List with voter Results
        //        //public List<ResultOld>? Results;

        //        // Party Name
        //        public string PartyName;

        //        // Derived Properties
        //        // Electable Member Full Name
        //        public string ElectableMemberFullName;

        //        //public static List<ResultOld> GetList(int pElectionId)
        //        {
        //            var tmpQuery = @"SELECT election.Id AS ElectionId, election.Name AS ElectionName, election.Date AS ElectionDate, ElectedMember_UserId AS ElectableMemberId, user.Firstname AS ElectableMemberFirstName, user.Lastname AS ElectableMemberLastName, user.City AS VoterCity, party.Name AS PartyName 
        //                            FROM vote 
        //                            INNER JOIN election ON election.Id = Voted_ElectionId 
        //                            INNER JOIN electablemember ON electablemember.User_UserId = ElectedMember_UserId 
        //                            INNER JOIN user ON user.Id = ElectedMember_UserId 
        //                            INNER JOIN party ON party.Id = user.Party_PartyId 
        //                            WHERE election.id = @pElectionId";

        //            var tmpParams = new Dictionary<string, object>() { { "@pElectionId", pElectionId } };
        //            var tmpResults = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParams);

        //            List<ResultOld> ResultsList = new List<ResultOld>();
        //            foreach (var tmpResult in tmpResults)
        //            {
        //                ResultsList.Add(new ResultOld(
        //                    pElectionId: (int)tmpResult[nameof(ElectionId)],
        //                    pElectionName: (string)tmpResult[nameof(ElectionName)],
        //                    pElectionDate: (DateTime)tmpResult[nameof(ElectionDate)],
        //                    pElectableMemberId: (int)tmpResult[nameof(ElectableMemberId)],
        //                    pVoterCity: (string)tmpResult[nameof(VoterCity)],
        //                    pElectableMemberFullName: $"{tmpResult[nameof(ElectableMemberFirstName)]} {tmpResult[nameof(ElectableMemberLastName)]}",
        //                    pPartyName: (string)tmpResult[nameof(PartyName)]
        //                ));
        //            }

        //            //VotesPerParty(ResultsList);
        //            return ResultsList;
        //        }

        //        //public static List<string> VotesPerParty(List<Result> ResultsList)
        //        //{


        //        //    return VoterPerPartyList;
        //        //}

        //        // Constructor
        //        public ResultOld(int pElectionId, string pElectionName, DateTime pElectionDate, int pElectableMemberId, string pVoterCity, string pElectableMemberFullName, string pPartyName)
        //        {
        //            ElectionId = pElectionId;
        //            ElectionName = pElectionName;
        //            ElectionDate = pElectionDate;
        //            ElectableMemberId = pElectableMemberId;
        //            VoterCity = pVoterCity;
        //            ElectableMemberFullName = pElectableMemberFullName;
        //            PartyName = pPartyName;
        //        }
    }
}
