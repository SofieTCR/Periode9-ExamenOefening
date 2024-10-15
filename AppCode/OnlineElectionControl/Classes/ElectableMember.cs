namespace OnlineElectionControl.Classes
{
    public class ElectableMember
    {
        // ElectableMember Properties

        /// <summary>
        /// UserId with which this ElectableMember is stored in the database.
        /// </summary>
        public int User_UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// ElectionId with which this ElectableMember is stored in the database.
        /// </summary>
        public int Election_ElectionId
        {
            get;
            private set;
        }

        /// <summary>
        /// Ordering of the ElectableMember within the party.
        /// </summary>
        public int Ordering;

        /// <summary>
        /// Validation messages, This list contains the issues encountered during validation.
        /// </summary>
        public List<string> Vml = new List<string>();

        // Derived election properties

        private User? _user;
        /// <summary>
        /// The object of the relevant User
        /// </summary>
        public User User
        {
            get
            {
                if (_user == null || _user.UserId != User_UserId) _user = new User(pId: User_UserId);
                return _user;
            }
        }

        
        public Party? _party;
        /// <summary>
        /// The object of the relevant Election
        /// </summary>
        public Party? Party
        {
            get
            {
                if (User.Party_PartyId == null) return null;
                if (_party == null || _party.PartyId != User.Party_PartyId) _party = new Party(pId: (int) User.Party_PartyId);
                return _party;
            }
        }


        // Constructors

        /// <summary>
        /// Constructor to fill a new object.
        /// </summary>
        public ElectableMember(int pUserId
                             , int pElectionId
                             , int pOrdering)

             : this(pUserId: pUserId
                  , pElectionId: pElectionId
                  , pOrdering: pOrdering
                  , pUser: null
                  , pParty: null)
        { }

        /// <summary>
        /// Constructor to get an ElectableMember from the database.
        /// </summary>
        public ElectableMember(int pUserId
                             , int pElectionId)
        {
            var tmpQuery = @"SELECT User_UserId,
                                    Election_ElectionId,
                                    Ordering
                               FROM `electablemember`
                              WHERE User_UserId = @pUserId
                                AND Election_ElectionId = @pElectionId";
            var tmpParams = new Dictionary<string, object>() {
                { "@pUserId", pUserId }
              , { "@pElectionId", pElectionId }
            };

            var tmpResults = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParams);

            if (tmpResults.Count != 1) throw new Exception($"Did not find a party with ElectableMember with UserId: {pUserId} and ElectionId {pElectionId}");

            User_UserId = pUserId;
            Election_ElectionId = pElectionId;
            Ordering = (int) tmpResults[0][nameof(Ordering)];
        }

        /// <summary>
        /// Private constructor that allows filling the ElectionId.
        /// </summary>
        private ElectableMember(int pUserId
                              , int pElectionId
                              , int pOrdering
                              , User? pUser
                              , Party? pParty)
        {
            User_UserId = pUserId;
            Election_ElectionId = pElectionId;
            Ordering = pOrdering;
            _user = pUser;
            _party = pParty;
        }

        // Public Methods
        public bool ValidateObject()
        {
            Vml.Clear();

            // User_UserId validation
            var tmpQuery = "SELECT Id AS UserId FROM `user` WHERE Id = @User_UserId";
            var tmpParams = new Dictionary<string, object> { { "@User_UserId", User_UserId } };
            var tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
            if (tmpResult.Count != 1) Vml.Add("User does not exist!");

            // User_UserId validation
            tmpQuery = "SELECT Id AS ElectionId FROM `election` WHERE Id = @Election_ElectionId";
            tmpParams = new Dictionary<string, object> { { "@Election_ElectionId", Election_ElectionId } };
            tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
            if (tmpResult.Count != 1) Vml.Add("Election does not exist!");

            // Ordering validation

            return Vml.Count == 0;
        }

        public bool Save()
        {
            if (!ValidateObject()) return false;

            string tmpQuery = "SELECT User_UserId, Election_ElectionId FROM `electablemember` WHERE User_UserId = @User_UserId AND Election_ElectionId = @Election_ElectionId;";
            var tmpParameters = new Dictionary<string, object>
                  {
                      { "@User_UserId", User_UserId }
                    , { "@Election_ElectionId", Election_ElectionId }
                    , { "@Ordering", Ordering }
                  };
            if (Database.ExecuteQuery(tmpQuery, tmpParameters).Count == 0)
            {
                // Create New
                tmpQuery = @"INSERT INTO `electablemember` (User_UserId
                                                          , Election_ElectionId
                                                          , Ordering)
                                                    VALUES (@User_UserId
                                                          , @Election_ElectionId
                                                          , @Ordering);";

                if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
            }
            else
            {
                // Update existing
                tmpQuery = @"UPDATE `electablemember` SET Ordering = @Ordering WHERE User_UserId = @User_UserId AND Election_ElectionId = @Election_ElectionId;";

                if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
            }

            return true;
        }

        public bool Delete()
        {
            string tmpQuery = "SELECT User_UserId, Election_ElectionId FROM `electablemember` WHERE User_UserId = @User_UserId AND Election_ElectionId = @Election_ElectionId;";
            var tmpParameters = new Dictionary<string, object>
                  {
                      { "@User_UserId", User_UserId }
                    , { "@Election_ElectionId", Election_ElectionId }
                    , { "@Ordering", Ordering }
                  };
            if (Database.ExecuteQuery(tmpQuery, tmpParameters).Count == 1)
            {
                tmpQuery = @"DELETE FROM `electablemember` WHERE User_UserId = @User_UserId AND Election_ElectionId = @Election_ElectionId;";
                if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
                return true;
            }

            return false;            
        }

        public static List<ElectableMember> GetList(bool pIncludingParty = false
                                                  , bool pIncludingUser = false
                                                  , List<int>? pPartyIds = null
                                                  , List<int>? pElectionIds = null)
        {
            List<ElectableMember> tmpElectableMembers = new List<ElectableMember>();
            List<Party> tmpParties = new List<Party>();
            List<User> tmpUsers = new List<User>();

            if (pIncludingParty) tmpParties = Party.GetList(pPartyIds: pPartyIds);
            if (pIncludingUser) tmpUsers = User.GetList(pIsEligible: true
                                                      , pPartyIds: pPartyIds
                                                      , pIncludingNonMembers: false);

            var tmpQuery = @"SELECT `electablemember`.User_UserId,
                                    `electablemember`.Election_ElectionId,
                                    `electablemember`.Ordering,
                                    `user`.Party_PartyId
                               FROM `electablemember`
                          LEFT JOIN `user` ON `electablemember`.User_UserId = `user`.Id
                              WHERE 1";

            var tmpParameters = new Dictionary<string, object> { };

            if (pElectionIds != null && pElectionIds.Count != 0)
            {
                var electionParams = pElectionIds.Select((id, index) => $"@pElectionId_{index}_").ToList();
                tmpQuery += " AND `electablemember`.Election_ElectionId IN (" + string.Join(", ", electionParams) + ")";

                for (int i = 0; i < pElectionIds.Count; i++)
                {
                    tmpParameters.Add($"@pElectionId_{i}_", pElectionIds[i]);
                }
            }

            if (pPartyIds != null && pPartyIds.Count != 0)
            {
                var partyParms = pPartyIds.Select((id, index) => $"@pPartyId_{index}_").ToList();
                tmpQuery += " AND `user`.Party_PartyId IN (" + string.Join(", ", partyParms) + ")";

                for (int i = 0; i < pPartyIds.Count; i++)
                {
                    tmpParameters.Add($"@pPartyId_{i}_", pPartyIds[i]);
                }
            }

            tmpQuery += ";";

            var tmpResultList = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

            foreach (var tmpElectableMember in tmpResultList)
            {
                tmpElectableMembers.Add(
                    new ElectableMember(pUserId: (int) tmpElectableMember[nameof(User_UserId)]
                                      , pElectionId: (int) tmpElectableMember[nameof(Election_ElectionId)]
                                      , pOrdering: (int) tmpElectableMember[nameof(Ordering)]
                                      , pUser: pIncludingUser ? tmpUsers.FirstOrDefault(u => u.UserId == (int) tmpElectableMember[nameof(User_UserId)]) : null
                                      , pParty: pIncludingParty ? tmpParties.FirstOrDefault(p => p.PartyId == (int) tmpElectableMember["Party_PartyId"]) : null
                ));
            }

            return tmpElectableMembers;
        }

    }
}
