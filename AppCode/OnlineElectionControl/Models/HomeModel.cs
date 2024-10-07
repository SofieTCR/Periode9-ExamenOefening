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

        public HomeModel()
        {
            //PastElections = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.Completed }, pSortOrder: SortOrder.ASC, pMaxNumber: 5);
            
            //var tmpElection = new Election(pName: $"Landelijke Verkiezingen {DateTime.Now.AddDays(32).ToString("HH:mm:ss [dd-MM-yyyy]")}", pDescription: null, pDate: DateTime.Today.AddDays(32));
            //tmpElection.Save();
            //var tmpElections = Election.GetList();

            //var completedElections = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.Completed });
            //var inProgressElections = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.InProgress });
            //var scheduledElections = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.Scheduled });

            //var scheduledandcurrent = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.InProgress, ElectionStatus.Scheduled });

            //var scheduledandcurrentsortedasc = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.InProgress, ElectionStatus.Scheduled }, pSortOrder: SortOrder.ASC);
            //var scheduledandcurrentsorteddesc = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.InProgress, ElectionStatus.Scheduled }, pSortOrder: SortOrder.DESC);

            //var scheduledandcurrentsortedasc5 = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.InProgress, ElectionStatus.Scheduled }, pSortOrder: SortOrder.ASC, pMaxNumber: 5);
        }
    }
}
