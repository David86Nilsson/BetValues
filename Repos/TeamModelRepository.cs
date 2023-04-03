

using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Repos
{
    public class TeamModelRepository
    {
        private readonly BetValueDbContext context;

        public TeamModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public List<TeamModel> GetTeams()
        {
            return context.Teams.ToList();
        }
        public TeamModel? GetTeam(int id)
        {
            return context.Teams.FirstOrDefault(g => g.Id == id);
        }
        public TeamModel? GetTeam(string name)
        {
            return context.Teams.FirstOrDefault(g => g.Name == name);
        }
        public async Task<TeamModel?> GetTeamAsync(string name)
        {
            return await context.Teams.FirstOrDefaultAsync(t => name.Contains(t.Name));
        }
        public void AddTeam(TeamModel team)
        {
            context.Teams.Add(team);
        }
        public async Task AddTeamAsync(TeamModel team)
        {
            await context.Teams.AddAsync(team);
        }
        public void UpdateTeam(TeamModel team)
        {
            context.Teams.Update(team);
        }
        public void RemoveTeam(TeamModel team)
        {
            context.Teams.Remove(team);
        }
    }
}