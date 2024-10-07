namespace OnlineElectionControl.Classes
{
    public class Election
    {
        // Election Properties

        /// <summary>
        /// Id with which this election is stored in the database.
        /// </summary>
        public int? ElectionId
        {
            get;
            private set;
        }

        /// <summary>
        /// The name of this election, frequently includes the year.
        /// </summary>
        public string Name;

        /// <summary>
        /// The description of this election, can be left empty.
        /// </summary>
        public string? Description;

        /// <summary>
        /// The date on which this election will take place.
        /// </summary>
        public DateTime Date;

        /// <summary>
        /// Validation messages, This list contains the issues encountered during validation.
        /// </summary>
        public List<string> Vml = new List<string>();

        // Derived election properties

        /// <summary>
        /// The current state of the election.
        /// </summary>
        public ElectionStatus Status
        {
            get
            {
                if (Date < DateTime.Today) return ElectionStatus.Completed;
                else if (Date == DateTime.Today) return ElectionStatus.InProgress;
                else if (Date > DateTime.Today) return ElectionStatus.Scheduled;
                else throw new Exception(message: "Date is not in the past, today or in the future!");
            }
        }

        // Constructors

        /// <summary>
        /// Constructor to fill a new object.
        /// </summary>
        public Election(string pName
                      , string? pDescription
                      , DateTime pDate)

             : this(pId: null
                  , pName: pName
                  , pDescription: pDescription
                  , pDate: pDate)
        { }

        /// <summary>
        /// Constructor to get election from the database.
        /// </summary>
        public Election(int pId)
        {
            var tmpQuery = @"SELECT Id AS ElectionId,
                                    Name,
                                    Description,
                                    Date
                               FROM `election`
                              WHERE Id = @pId";
            var tmpParams = new Dictionary<string, object>() { { "@pId", pId } };

            var tmpResults = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParams);

            if (tmpResults.Count != 1) throw new Exception($"Did not find an election with ElectionId: {pId}");

            ElectionId = pId;
            Name = (string) tmpResults[0][nameof(Name)];
            Description = tmpResults[0][nameof(Description)] as string;
            Date = (DateTime) tmpResults[0][nameof(Date)];
        }

        /// <summary>
        /// Private constructor that allows filling the ElectionId.
        /// </summary>
        private Election(int? pId
                       , string pName
                       , string? pDescription
                       , DateTime pDate)
        {
            ElectionId = pId;
            Name = pName;
            Description = pDescription;
            Date = pDate;
        }

        // Public Methods
        public bool ValidateObject()
        {
            Vml.Clear();

            // Name validation.
            if (Name.Length < 3) Vml.Add("Name is too short!");
            if (Name.Length > 255) Vml.Add("Name is too long!");
            var tmpQuery = "SELECT Id AS UserId FROM `election` WHERE Name = @Name";
            var tmpParams = new Dictionary<string, object>() { { "@Name", Name } };
            if (ElectionId != null)
            {
                tmpQuery += " AND Id != @ElectionId";
                tmpParams["@ElectionId"] = ElectionId;
            }
            var tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
            if (tmpResult.Count != 0) Vml.Add("Name already in use!");

            // Description Validation
            if (Description != null && Description.Length > 65e3) Vml.Add("Description is too long!");

            // Date validation.

            return Vml.Count == 0;
        }

        public bool Save()
        {
            if (!ValidateObject()) return false;

            string tmpQuery = string.Empty;
            var tmpParameters = new Dictionary<string, object>
            {
                { "@ElectionId", ElectionId! }
              , { "@Name", Name }
              , { "@Description", Description }
              , { "@Date", Date }
            };
            if (ElectionId == null)
            {
                // Create New
                tmpQuery = @"INSERT INTO `election` (Name
                                                   , Description
                                                   , Date)
                                             VALUES (@Name
                                                   , @Description
                                                   , @Date);
                             SELECT LAST_INSERT_ID();";

                var tmpResultingId = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

                if (tmpResultingId.Count != 1) throw new Exception("Did not receive the insertion id back from the database!");

                ElectionId = Convert.ToInt32(tmpResultingId[0]["LAST_INSERT_ID()"]);
            }
            else
            {
                // Update existing
                tmpQuery = @"UPDATE `election` SET Name = @Name
                                                 , Description = @Description
                                                 , Date = @Date
                                             WHERE Id = @ElectionId;";

                if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
            }

            return true;
        }

        public bool Delete()
        {
            if (ElectionId == null) return false;

            var tmpQuery = @"DELETE FROM `election` WHERE Id = @ElectionId";
            var tmpParameters = new Dictionary<string, object> { { "@ElectionId", ElectionId } };

            if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
            return true;
        }

        public static List<Election> GetList(List<ElectionStatus> pStatus = null
                                           , int? pMaxNumber = null
                                           , SortOrder pSortOrder = SortOrder.NONE)
        {
            List<Election> tmpElections = new List<Election>();
            var tmpReferenceDate = DateTime.Today;
            var tmpQuery = @"SELECT Id AS ElectionId,
                                    Name,
                                    Description,
                                    Date
                               FROM `election`
                              WHERE 1 = 1 ";

            var tmpParameters = new Dictionary<string, object>
            {
                { "@ReferenceDate", tmpReferenceDate }
            };

            if (pStatus != null && pStatus.Any())
            {
                var tmpStatusConditions = new List<string>();

                if (pStatus.Contains(ElectionStatus.Completed))
                    tmpStatusConditions.Add("Date < @ReferenceDate");

                if (pStatus.Contains(ElectionStatus.InProgress))
                    tmpStatusConditions.Add("Date = @ReferenceDate");

                if (pStatus.Contains(ElectionStatus.Scheduled))
                    tmpStatusConditions.Add("Date > @ReferenceDate");

                // Combine status conditions with OR logic
                tmpQuery += " AND (" + string.Join(" OR ", tmpStatusConditions) + ")";
            }

            if (pSortOrder == SortOrder.ASC)
            {
                tmpQuery += " ORDER BY Date ASC";
            }
            else if (pSortOrder == SortOrder.DESC)
            {
                tmpQuery += " ORDER BY Date DESC";
            }

            if (pMaxNumber.HasValue)
            {
                tmpQuery += " LIMIT @MaxNumber";
                tmpParameters.Add("@MaxNumber", pMaxNumber.Value);
            }

            var tmpResultList = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

            foreach (var tmpElection in tmpResultList)
            {
                tmpElections.Add(new Election(
                    pId: (int) tmpElection[nameof(ElectionId)],
                    pName: (string) tmpElection[nameof(Name)],
                    pDescription: tmpElection[nameof(Description)] as string,
                    pDate: (DateTime) tmpElection[nameof(Date)]
                ));
            }

            return tmpElections;
        }

    }
}
