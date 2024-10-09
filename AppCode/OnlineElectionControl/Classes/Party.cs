namespace OnlineElectionControl.Classes
{
      public class Party
      {
            // Election Properties

            /// <summary>
            /// Id with which this party is stored in the database.
            /// </summary>
            public int? PartyId
            {
                  get;
                  private set;
            }

            /// <summary>
            /// The name of this party, frequently uses shorthand like VVD.
            /// </summary>
            public string Name;

            /// <summary>
            /// The description containing information about the party, can be left empty.
            /// </summary>
            public string? Description;

            /// <summary>
            /// Contains the positions of the party on political issues, can be left empty.
            /// </summary>
            public string? Positions;

            /// <summary>
            /// A link to an image containing the logo of the party, can be left empty.
            /// </summary>
            public string? LogoLink;

            /// <summary>
            /// The id with which the leader of this party is stored in the database.
            /// </summary>
            public int Leader_UserId;

            /// <summary>
            /// Validation messages, This list contains the issues encountered during validation.
            /// </summary>
            public List<string> Vml = new List<string>();

            // Derived election properties

            private User? _leader_User;
            public User Leader_User
            {
                  get
                  {
                        if (_leader_User == null || _leader_User.UserId != Leader_UserId) _leader_User = new User(pId: Leader_UserId);
                        return _leader_User;
                  }
            }

            // Constructors

            /// <summary>
            /// Constructor to fill a new object.
            /// </summary>
            public Party(string pName
                       , string? pDescription
                       , string? pPositions
                       , string? pLogoLink
                       , int pLeader_UserId)

                 : this(pId: null
                      , pName: pName
                      , pDescription: pDescription
                      , pPositions: pPositions
                      , pLogoLink: pLogoLink
                      , pLeader_UserId: pLeader_UserId)
            { }

            /// <summary>
            /// Constructor to get a party from the database.
            /// </summary>
            public Party(int pId)
            {
                  var tmpQuery = @"SELECT Id AS PartyId,
                                          Name,
                                          Description,
                                          Positions,
                                          LogoLink,
                                          Leader_UserId
                                     FROM `party`
                                    WHERE Id = @pId";
                  var tmpParams = new Dictionary<string, object>() { { "@pId", pId } };

                  var tmpResults = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParams);

                  if (tmpResults.Count != 1) throw new Exception($"Did not find a party with PartyId: {pId}");

                  PartyId = pId;
                  Name = (string) tmpResults[0][nameof(Name)];
                  Description = tmpResults[0][nameof(Description)] as string;
                  Positions = tmpResults[0][nameof(Positions)] as string;
                  LogoLink = tmpResults[0][nameof(LogoLink)] as string;
                  Leader_UserId = (int) tmpResults[0][nameof(Leader_UserId)];
            }

            /// <summary>
            /// Private constructor that allows filling the ElectionId.
            /// </summary>
            private Party(int? pId
                        , string pName
                        , string? pDescription
                        , string? pPositions
                        , string? pLogoLink
                        , int pLeader_UserId)
            {
                  PartyId = pId;
                  Name = pName;
                  Description = pDescription;
                  Positions = pPositions;
                  LogoLink = pLogoLink;
                  Leader_UserId = pLeader_UserId;
            }

            // Public Methods
            public bool ValidateObject()
            {
                  Vml.Clear();

                  // Name validation.
                  if (Name.Length < 3) Vml.Add("Name is too short!");
                  if (Name.Length > 255) Vml.Add("Name is too long!");
                  var tmpQuery = "SELECT Id AS PartyId FROM `party` WHERE Name = @Name";
                  var tmpParams = new Dictionary<string, object>() { { "@Name", Name } };
                  if (PartyId != null)
                  {
                        tmpQuery += " AND Id != @PartyId";
                        tmpParams["@ElectionId"] = PartyId;
                  }
                  var tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
                  if (tmpResult.Count != 0) Vml.Add("Name already in use!");

                  // Description Validation
                  if (Description != null && Description.Length > 65e3) Vml.Add("Description is too long!");

                  // Positions Validation
                  if (Positions != null && Positions.Length > 65e3) Vml.Add("Positions is too long!");

                  // LogoLink Validation
                  if (LogoLink != null && LogoLink.Length > 255) Vml.Add("LogoLink is too long!");

                  // Leader_UserId Validation
                  tmpQuery = "SELECT Id AS PartyId FROM `party` WHERE Leader_UserId = @Leader_UserId";
                  tmpParams = new Dictionary<string, object>() { { "@Leader_UserId", Leader_UserId } };
                  if (PartyId != null)
                  {
                        tmpQuery += " AND Id != @PartyId";
                        tmpParams["@ElectionId"] = PartyId;
                  }
                  tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
                  if (tmpResult.Count != 0) Vml.Add("Leader_User is already leading a party!");

                  tmpQuery = "SELECT Id AS UserId FROM `user` WHERE Id = @Leader_UserId";
                  tmpResult = Database.ExecuteQuery(tmpQuery, tmpParams);
                  if (tmpResult.Count != 1) Vml.Add("Leader_User does not exist!");

                  return Vml.Count == 0;
            }

            public bool Save()
            {
                  if (!ValidateObject()) return false;

                  string tmpQuery = string.Empty;
                  var tmpParameters = new Dictionary<string, object>
                  {
                      { "@PartyId", PartyId }
                    , { "@Name", Name }
                    , { "@Description", Description }
                    , { "@Positions", Positions }
                    , { "@LogoLink", LogoLink }
                    , { "@Leader_UserId", Leader_UserId }
                  };
                  if (PartyId == null)
                  {
                        // Create New
                        tmpQuery = @"INSERT INTO `party` (Name
                                                        , Description
                                                        , Positions
                                                        , LogoLink
                                                        , Leader_UserId)
                                                  VALUES (@Name
                                                        , @Description
                                                        , @Positions
                                                        , @LogoLink
                                                        , @Leader_UserId);
                                     SELECT LAST_INSERT_ID();";

                        var tmpResultingId = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

                        if (tmpResultingId.Count != 1) throw new Exception("Did not receive the insertion id back from the database!");

                        PartyId = Convert.ToInt32(tmpResultingId[0]["LAST_INSERT_ID()"]);
                  }
                  else
                  {
                        // Update existing
                        tmpQuery = @"UPDATE `party` SET Name = @Name
                                                      , Description = @Description
                                                      , Positions = @Positions
                                                      , LogoLink = @LogoLink
                                                      , Leader_UserId = @Leader_UserId
                                             WHERE Id = @PartyId;";

                        if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
                  }

                  return true;
            }

            public bool Delete()
            {
                  if (PartyId == null) return false;

                  var tmpQuery = @"DELETE FROM `party` WHERE Id = @PartyId";
                  var tmpParameters = new Dictionary<string, object> { { "@PartyId", PartyId } };

                  if (Database.ExecuteNonQuery(pQuery: tmpQuery, pParameters: tmpParameters) != 1) throw new Exception("Something went wrong during the execution of the non-query!");
                  return true;
            }

            public static List<Party> GetList(bool pIncludingDescription = false
                                            , bool pIncludingPositions = false
                                            , bool pIncludingLogoLink = false)
            {
                  List<Party> tmpParties = new List<Party>();
                  var tmpReferenceDate = DateTime.Today;
                  var tmpQuery = @"SELECT Id AS PartyId,
                                                Name,";
                  if (pIncludingDescription) tmpQuery += "Description,";
                  if (pIncludingPositions) tmpQuery += "Positions,";
                  if (pIncludingLogoLink) tmpQuery += "LogoLink,";
                  tmpQuery += @"Leader_UserId
                               FROM `party`
                              WHERE 1 = 1 ";

                  var tmpParameters = new Dictionary<string, object> { };

                  tmpQuery += ";";

                  var tmpResultList = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

                  foreach (var tmpParty in tmpResultList)
                  {
                        tmpParties.Add(new Party(pId: (int) tmpParty[nameof(PartyId)]
                                              , pName: (string) tmpParty[nameof(Name)]
                                              , pDescription: pIncludingDescription ? tmpParty[nameof(Description)] as string : null
                                              , pPositions: pIncludingPositions ? tmpParty[nameof(Positions)] as string : null
                                              , pLogoLink: pIncludingLogoLink ? tmpParty[nameof(LogoLink)] as string : null
                                              , pLeader_UserId: (int) tmpParty[nameof(Leader_UserId)]
                        ));
                  }

                  return tmpParties;
            }

      }
}
