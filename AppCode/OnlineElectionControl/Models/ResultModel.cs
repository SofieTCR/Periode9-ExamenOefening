using OnlineElectionControl.Classes;
using System.Runtime.ExceptionServices;

namespace OnlineElectionControl.Models
{
    public class ResultModel
    {
        public List<Result> Results { get; set; }

        public ResultModel(int pElectionId)
        {
            Results = Result.GetList(pElectionId);
        }
    }
}