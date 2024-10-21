using OnlineElectionControl.Classes;
using System.Runtime.ExceptionServices;

namespace OnlineElectionControl.Models
{
    public class ResultModel
    {
        public List<ResultOld> Results { get; set; }

        public ResultModel(int pElectionId)
        {
            Results = ResultOld.GetList(pElectionId);
        }
    }
}