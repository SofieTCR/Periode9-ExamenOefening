using OnlineElectionControl.Classes;
namespace OnlineElectionControl.Models
{
    public class PartyMemberModel
    {
        public List<User> Members;
        public PartyMemberModel(int pId)
        {
            Members = User.GetList(pPartyId: pId, pIncludingNonMembers: true);
        }
    }
}
