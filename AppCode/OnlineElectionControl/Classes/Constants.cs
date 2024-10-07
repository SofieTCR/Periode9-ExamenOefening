namespace OnlineElectionControl.Classes
{
    public class Constants
    {
        public static readonly int[] GovernmentUserIds = { 1, 3 }; 
    }

    /// <summary>
    /// Enumeration value containing the possible states an election can be in.
    /// </summary>
    public enum ElectionStatus
    {
        Completed   // Indicates that the election is over.
      , InProgress  // Indicates that the election is currently happening.
      , Scheduled   // Indicates that the election is planned for the future.
    }

    /// <summary>
    /// Enumeration value containing the possible sorting orders.
    /// </summary>
    public enum SortOrder
    {
        NONE
      , ASC
      , DESC
    }
}
