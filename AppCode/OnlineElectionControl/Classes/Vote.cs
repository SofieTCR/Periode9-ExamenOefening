using System.Security.Cryptography;

namespace OnlineElectionControl.Classes
{
    public class Vote
    {
        // ElectableMember Properties

        /// <summary>
        /// UserId with which this Vote is stored in the database.
        /// </summary>
        public int Voter_UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// ElectionId with which this Vote is stored in the database.
        /// </summary>
        public int Voted_ElectionId
        {
            get;
            private set;
        }

        /// <summary>
        /// The ElectableMember this vote is for.
        /// </summary>
        public int ElectedMember_UserId;

        /// <summary>
        /// Validation messages, This list contains the issues encountered during validation.
        /// </summary>
        public List<string> Vml = new List<string>();

        // Derived election properties

        /// <summary>
        /// The object of the relevant Voter.
        /// </summary>
        public User Voter
        {
            get
            {
                if (_voter == null || _voter.UserId != Voter_UserId) _voter = new User(pId: Voter_UserId);
                return _voter;
            }
        }
        private User? _voter;

        /// <summary>
        /// The Object of the relevant Electable Member.
        /// </summary>
        public ElectableMember ElectableMember
        {
            get
            {
                if (_electableMember == null || _electableMember.User_UserId != ElectedMember_UserId) _electableMember = new ElectableMember(pUserId: ElectedMember_UserId, pElectionId: Voted_ElectionId);
                return _electableMember;
            }
        }
        private ElectableMember? _electableMember;

        public Election Election
        {
            get
            {
                if (_election == null || _election.ElectionId != Voted_ElectionId) _election = new Election(pId: Voted_ElectionId);
                return _election;
            }
        }
        private Election? _election;

        // Constructors

        /// <summary>
        /// Constructor to fill a new object.
        /// </summary>
        public Vote(int pVoter_UserId
                  , int pVoted_ElectionId
                  , int pElectedMember_UserId)

             : this(pVoter_UserId: pVoter_UserId
                  , pVoted_ElectionId: pVoted_ElectionId
                  , pElectedMember_UserId: pElectedMember_UserId
                  , pVoter: null
                  , pElectableMember: null
                  , pElection: null)
        { }

        /// <summary>
        /// Constructor to get a vote from the database.
        /// </summary>
        public Vote(int pVoter_UserId
                  , int pVoted_ElectionId)
        {
            var tmpQuery = @"SELECT Voter_UserId,
                                    Voted_ElectionId,
                                    ElectedMember_UserId
                               FROM `vote`
                              WHERE Voter_UserId = @pVoter_UserId
                                AND Voted_ElectionId = @pVoted_ElectionId";

            var tmpParams = new Dictionary<string, object>() {
                { "@pVoter_UserId", pVoter_UserId }
              , { "@pVoted_ElectionId", pVoted_ElectionId }
            };

            var tmpResults = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParams);

            if (tmpResults.Count != 1) throw new Exception($"Did not find a vote with Voter_UserId: {pVoter_UserId} and Voted_ElectionId {pVoted_ElectionId}");

            Voter_UserId = pVoter_UserId;
            Voted_ElectionId = pVoted_ElectionId;
            ElectedMember_UserId = (int) tmpResults[0][nameof(ElectedMember_UserId)];
        }

        /// <summary>
        /// Private constructor that allows filling the ElectionId.
        /// </summary>
        private Vote(int pVoter_UserId
                   , int pVoted_ElectionId
                   , int pElectedMember_UserId
                   , User? pVoter
                   , ElectableMember? pElectableMember
                   , Election? pElection)
        {
            Voter_UserId = pVoter_UserId;
            Voted_ElectionId = pVoted_ElectionId;
            ElectedMember_UserId = pElectedMember_UserId;
            _voter = pVoter;
            _electableMember = pElectableMember;
        }

        // Public Methods
        public bool ValidateObject()
        {
            Vml.Clear();

            // Voter_UserId validation
            var tmpQuery = "SELECT Id AS UserId FROM `user` WHERE Id = @Voter_UserId";
            var tmpParams = new Dictionary<string, object> { { "@Voter_UserId", Voter_UserId } };
            var tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
            if (tmpResult.Count != 1) Vml.Add("Voter_User does not exist!");

            // Voted_ElectionId validation
            tmpQuery = "SELECT Id AS ElectionId FROM `election` WHERE Id = @Voted_ElectionId";
            tmpParams = new Dictionary<string, object> { { "@Voted_ElectionId", Voted_ElectionId } };
            tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
            if (tmpResult.Count != 1) Vml.Add("Election does not exist!");

            // ElectedMember_UserId validation
            tmpQuery = "SELECT Id AS UserId FROM `user` WHERE Id = @ElectedMember_UserId";
            tmpParams = new Dictionary<string, object> { { "@ElectedMember_UserId", ElectedMember_UserId } };
            tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
            if (tmpResult.Count != 1) Vml.Add("ElectedMember_User does not exist!");

            return Vml.Count == 0;
        }

        public bool Save()
        {
            if (!ValidateObject()) return false;

            string tmpQuery = "SELECT Voter_UserId, Voted_ElectionId FROM `vote` WHERE Voter_UserId = @Voter_UserId AND Voted_ElectionId = @Voted_ElectionId;";
            var tmpParameters = new Dictionary<string, object>
                  {
                      { "@Voter_UserId", Voter_UserId }
                    , { "@Voted_ElectionId", Voted_ElectionId }
                    , { "@ElectedMember_UserId", ElectedMember_UserId }
                  };
            if (Database.ExecuteQuery(tmpQuery, tmpParameters).Count == 0)
            {
                // Create New
                tmpQuery = @"INSERT INTO `vote` (Voter_UserId
                                               , Voted_ElectionId
                                               , ElectedMember_UserId)
                                         VALUES (@Voter_UserId
                                               , @Voted_ElectionId
                                               , @ElectedMember_UserId);";

                if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
            }
            else
            {
                // Update existing
                tmpQuery = @"UPDATE `vote` SET ElectedMember_UserId = @ElectedMember_UserId WHERE Voter_UserId = @Voter_UserId AND Voted_ElectionId = @Voted_ElectionId;";

                if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
            }

            return true;
        }

        public bool Delete()
        {
            string tmpQuery = "SELECT Voter_UserId, Voted_ElectionId FROM `vote` WHERE Voter_UserId = @Voter_UserId AND Voted_ElectionId = @Voted_ElectionId;";
            var tmpParameters = new Dictionary<string, object>
                  {
                      { "@Voter_UserId", Voter_UserId }
                    , { "@Voted_ElectionId", Voted_ElectionId }
                  };
            if (Database.ExecuteQuery(tmpQuery, tmpParameters).Count == 1)
            {
                tmpQuery = @"DELETE FROM `vote` WHERE Voter_UserId = @Voter_UserId AND Voted_ElectionId = @Voted_ElectionId;";
                if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
                return true;
            }

            return false;
        }

        public static List<Vote> GetList(bool pIncludingVoters = true
                                       , bool pIncludingElectableMembers = true
                                       , bool pIncludingElections = true
                                       , List<string>? pCities = null
                                       , List<int>? pPartyIds = null
                                       , List<int>? pElectableMemberIds = null
                                       , List<int>? pElectionIds = null)
        {
            List<Vote> tmpVotes = new List<Vote>();
            List<User> tmpVoters = new List<User>();
            List<ElectableMember> tmpElectableMembers = new List<ElectableMember>();
            List<Election> tmpElections = new List<Election>();

            if (pIncludingVoters) tmpVoters = User.GetList();
            if (pIncludingElectableMembers) tmpElectableMembers = ElectableMember.GetList(pIncludingParty: true
                                                                                       , pIncludingUser: true
                                                                                       , pPartyIds: pPartyIds
                                                                                       , pElectionIds: pElectionIds);
            if (pIncludingElections) tmpElections = Election.GetList(pElectionIds: pElectionIds);

            var tmpQuery = @"SELECT v.Voter_UserId,
                                    v.Voted_ElectionId,
                                    v.ElectedMember_UserId,
                                voter.City,
                              elected.Party_PartyId
                               FROM `vote` v
                               JOIN user voter ON voter.Id = v.Voter_UserId
                               JOIN user elected ON elected.Id = v.ElectedMember_UserId
                              WHERE 1";

            var tmpParameters = new Dictionary<string, object> { };

            if (pCities != null && pCities.Count != 0)
            {
                var CityParams = pCities.Select((id, index) => $"@pCity_{index}_").ToList();
                tmpQuery += " AND voter.City IN (" + string.Join(", ", CityParams) + ")";

                for (int i = 0; i < pCities.Count; i++)
                {
                    tmpParameters.Add($"@pCity_{i}_", pCities[i]);
                }
            }

            if (pPartyIds != null && pPartyIds.Count != 0)
            {
                var PartyParams = pPartyIds.Select((id, index) => $"@pPartyId_{index}_").ToList();
                tmpQuery += " AND elected.Party_PartyId IN (" + string.Join(", ", PartyParams) + ")";

                for (int i = 0; i < pPartyIds.Count; i++)
                {
                    tmpParameters.Add($"@pPartyId_{i}_", pPartyIds[i]);
                }
            }

            if (pElectableMemberIds != null && pElectableMemberIds.Count != 0)
            {
                var ElectableMemberParams = pElectableMemberIds.Select((id, index) => $"@pElectedMember_{index}_").ToList();
                tmpQuery += " AND v.ElectedMember_UserId IN (" + string.Join(", ", ElectableMemberParams) + ")";

                for (int i = 0; i < pElectableMemberIds.Count; i++)
                {
                    tmpParameters.Add($"@pElectedMember_{i}_", pElectableMemberIds[i]);
                }
            }

            if (pElectionIds != null && pElectionIds.Count != 0)
            {
                var electionParams = pElectionIds.Select((id, index) => $"@pElectionId_{index}_").ToList();
                tmpQuery += " AND v.Voted_ElectionId IN (" + string.Join(", ", electionParams) + ")";

                for (int i = 0; i < pElectionIds.Count; i++)
                {
                    tmpParameters.Add($"@pElectionId_{i}_", pElectionIds[i]);
                }
            }

            tmpQuery += ";";

            var tmpResultList = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

            foreach (var tmpElectableMember in tmpResultList)
            {
                tmpVotes.Add(
                    new Vote(pVoter_UserId: (int)tmpElectableMember[nameof(Voter_UserId)]
                           , pVoted_ElectionId: (int)tmpElectableMember[nameof(Voted_ElectionId)]
                           , pElectedMember_UserId: (int)tmpElectableMember[nameof(ElectedMember_UserId)]
                           , pVoter: pIncludingVoters ? tmpVoters.FirstOrDefault(user => user.UserId == (int)tmpElectableMember[nameof(Voter_UserId)]) : null
                           , pElectableMember: pIncludingElectableMembers ? tmpElectableMembers.FirstOrDefault(e => e.User_UserId == (int)tmpElectableMember[nameof(ElectedMember_UserId)] 
                                                                                                           && e.Election_ElectionId == (int)tmpElectableMember[nameof(Voted_ElectionId)]) : null
                           , pElection: pIncludingElections ? tmpElections.FirstOrDefault(e => e.ElectionId == (int)tmpElectableMember[nameof(Voted_ElectionId)]) : null
                ));
            }

            return tmpVotes;
        }

    }
}
