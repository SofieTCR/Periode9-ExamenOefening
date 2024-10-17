using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Models
{
    public class SendEmailModel
    {
        public IEnumerable<IGrouping<string, Election>> Elections;

        public SendEmailModel()
        {
            var tmpElections = Election.GetList(pStatus: new List<ElectionStatus> { ElectionStatus.InProgress, ElectionStatus.Scheduled });
            Elections = tmpElections.OrderBy(e => e.Date).GroupBy(e => e.Status.ToString()).OrderBy(eg => eg.Key);
        }
    }
}
