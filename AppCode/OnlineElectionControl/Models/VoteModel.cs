using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Models
{
    public class VoteModel
    {
        public Election Election;

        public VoteModel(Election pElection)
        {
            Election = pElection;
        }
    }
}
