using OnlineElectionControl.Classes;
namespace OnlineElectionControl.Models
{
    public class ElectableMemberModel
    {
        public List<(User user, ElectableMember? electableMember)> SortedMembers;

        public int ElectionId;

        public ElectableMemberModel(int pPartyId, Election pElection)
        {
            ElectionId = (int) pElection.ElectionId!;
            var tmpMembers = User.GetList(pIsEligible: true
                                        , pReferenceDate: pElection.Date
                                        , pPartyIds: new List<int> { pPartyId }
                                        , pIncludingNonMembers: false);
            var tmpElectableMembers = ElectableMember.GetList(pPartyIds: new List<int> { pPartyId }
                                                            , pElectionIds: new List<int> { (int) pElection.ElectionId! });

            SortedMembers = tmpMembers
                .Select(user => (user,
                                 electableMember: tmpElectableMembers.FirstOrDefault(em => em.User_UserId == user.UserId)))
                .OrderByDescending(tuple => tuple.electableMember != null)
                .ThenBy(tuple => tuple.electableMember?.Ordering ?? int.MaxValue)
                .ThenBy(tuple => tuple.user.UserSortName)
                .ToList();
        }

    }
}
