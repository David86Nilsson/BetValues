using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Services
{
    public class SerieMemberModelRepository
    {
        private readonly BetValueDbContext context;

        public SerieMemberModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public List<SerieMemberModel> GetSerieMembers()
        {
            return context.SerieMembers.ToList();
        }
        public SerieMemberModel? GetSerieMember(int id)
        {
            return context.SerieMembers.FirstOrDefault(g => g.Id == id);
        }
        public void AddSerieMember(SerieMemberModel serieMember)
        {
            context.SerieMembers.Add(serieMember);
        }
        public void UpdateSerieMember(SerieMemberModel serieMember)
        {
            context.SerieMembers.Update(serieMember);
        }
        public void RemoveSerieMember(SerieMemberModel serieMember)
        {
            context.SerieMembers.Remove(serieMember);
        }

        public SerieMemberModel? GetSerieMember(string name, string year)
        {
            return context.SerieMembers.Include(s => s.Team).Include(s => s.Serie).
                FirstOrDefault(sm => sm.Team.Name == name && sm.Serie.Year == year);
        }
    }
}
