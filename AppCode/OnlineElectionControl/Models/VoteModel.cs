using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Models
{
    public class VoteModel
    {
        public Election Election;

        public IEnumerable<IGrouping<string, ElectableMember>> Members;

        public VoteModel(Election pElection)
        {
            Election = pElection;

            var tmpElectableMembers = ElectableMember.GetList(pIncludingParty: true, pIncludingUser: true, pElectionIds: new List<int> { (int) Election.ElectionId! });
            Members = tmpElectableMembers.OrderBy(m => m.Ordering).GroupBy(m => m.Party!.Name).OrderBy(p => p.Key);
        }
    }
}
