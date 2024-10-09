using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Models
{
    public class HomeModel
    {
        /// <summary>
        /// A method to return a string array of the provided string, split on the letter t
        /// </summary>
        /// <param name="splitString">The string to be split</param>
        /// <returns></returns>
        public string[] SplitOnT(string splitString)
        {
            return splitString.Split('t');
        }

        public List<Election> GetPastElections => Election.GetList(
            pStatus: new List<ElectionStatus> { ElectionStatus.Completed}
          , pSortOrder: SortOrder.DESC
          , pMaxNumber: 5
        );

        public List<Election> GetFutureElections => Election.GetList(
            pStatus: new List<ElectionStatus> { ElectionStatus.Scheduled, ElectionStatus.InProgress }
          , pSortOrder: SortOrder.ASC
          , pMaxNumber: 5
        );
    }
}
