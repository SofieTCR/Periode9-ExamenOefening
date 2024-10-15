using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Models
{
    public class PartyModel
    {
        public Party party;

        public List<User> partyMembers;
        public PartyModel(int pId)
        {
            party = new Party(pId: pId);
            partyMembers = User.GetList(pPartyIds: new List<int> { pId }
                                      , pIncludingNonMembers: false);
        }
    }
}
