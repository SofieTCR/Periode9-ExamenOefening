using OnlineElectionControl.Classes;
using System.Runtime.ExceptionServices;

namespace OnlineElectionControl.Models
{
    public class ResultModel
    {
        public Dictionary<string, Dictionary<User, List<Vote>>> VotesPerPartyPerMember { get; set; }
        public Dictionary<string, Dictionary<Party, List<Vote>>> VotesPerCityPerParty { get; set; }

        public Election Election { get; set; }

        public ResultModel(int pElectionId)
        {
            var tmpVotes = Vote.GetList(pElectionIds: new List<int> { pElectionId });
            VotesPerPartyPerMember = tmpVotes
                .GroupBy(vote => vote.ElectableMember.Party!.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(vote => vote.ElectableMember.User)
                          .ToDictionary(
                              ug => ug.Key,
                              ug => ug.ToList()
                          )
                );

            //var tmp = new Dictionary<string, Dictionary<string, List<Vote>>>();
            //foreach (var vote in tmpVotes)
            //{
            //    if (!tmp.ContainsKey(vote.Voter.City)) tmp[vote.Voter.City] = new Dictionary<string, List<Vote>>();

            //    if (!tmp[vote.Voter.City].ContainsKey(vote.ElectableMember.Party!.Name))
            //        tmp[vote.Voter.City][vote.ElectableMember.Party!.Name] = new List<Vote>();

            //    tmp[vote.Voter.City][vote.ElectableMember.Party!.Name].Add(vote);
            //}
            VotesPerCityPerParty = tmpVotes
                .GroupBy(vote => vote.Voter.City)
                .ToDictionary(
                    cityGroup => cityGroup.Key,
                    cityGroup => cityGroup
                        .GroupBy(vote => vote.ElectableMember.Party)
                        .ToDictionary(
                            partyGroup => partyGroup.Key,
                            partyGroup => partyGroup.ToList()
                        )
                );

            Election = tmpVotes.FirstOrDefault()!.Election;

        }
    }
}