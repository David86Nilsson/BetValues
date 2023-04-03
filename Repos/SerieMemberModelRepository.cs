

using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Repos
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
            return context.SerieMembers.Include(s => s.Team).Include(s => s.Serie).FirstOrDefault(g => g.Id == id);
        }
        public SerieMemberModel? GetSerieMember(string teamName, int serieId)
        {
            return context.SerieMembers.Include(s => s.Team).Include(s => s.Serie).
                FirstOrDefault(sm => sm.Team.Name == teamName && sm.Serie.Id == serieId);
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

        public async Task<SerieMemberModel?> GetSerieMemberAsync(TeamModel homeTeam, int id)
        {
            return await context.SerieMembers.Include(s => s.Team).Include(s => s.Serie).FirstOrDefaultAsync(sm => sm.TeamId == homeTeam.Id && sm.SerieId == id);
        }

        public async Task AddSerieMemberAsync(SerieMemberModel serieMember)
        {
            await context.SerieMembers.AddAsync(serieMember);
        }

        public async Task<IEnumerable<SerieMemberModel>?> GetSerieMembersAsync(int serieId)
        {
            return await context.SerieMembers.Include(s => s.Team).Include(s => s.Serie).Where(sm => sm.SerieId == serieId).ToListAsync();
        }
    }
}
