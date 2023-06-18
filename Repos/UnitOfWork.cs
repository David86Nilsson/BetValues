using BetValue.Database;

namespace BetValue.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BetValueDbContext context;

        private TeamModelRepository? teamModelRepository;
        private GameModelRepository? gameModelRepository;
        private BetModelRepository? betModelRepository;
        private SerieModelRepository? serieModelRepository;
        private SerieMemberModelRepository? serieMemberModelRepository;
        //private CompetionModelRepository? competitionModelRepository;
        private LeagueModelRepository? leagueModelRepository;
        private CountryModelRepository? countryModelRepository;
        private OddsModelRepository? oddsModelRepository;

        public UnitOfWork(BetValueDbContext context)
        {
            this.context = context;
        }

        //public CompetionModelRepository CompetionModelRepository
        //{
        //    get
        //    {
        //        if (competitionModelRepository == null)
        //        {
        //            competitionModelRepository = new CompetionModelRepository(context);
        //        }
        //        return competitionModelRepository;
        //    }
        //}

        public TeamModelRepository TeamModelRepository
        {
            get
            {
                if (teamModelRepository == null)
                {
                    teamModelRepository = new TeamModelRepository(context);
                }
                return teamModelRepository;
            }
        }

        public GameModelRepository GameModelRepository
        {
            get
            {
                if (gameModelRepository == null)
                {
                    gameModelRepository = new GameModelRepository(context);
                }
                return gameModelRepository;
            }
        }

        public BetModelRepository BetModelRepository
        {
            get
            {
                if (betModelRepository == null)
                {
                    betModelRepository = new BetModelRepository(context);
                }
                return betModelRepository;
            }
        }
        public SerieModelRepository SerieModelRepository
        {
            get
            {
                if (serieModelRepository == null)
                {
                    serieModelRepository = new SerieModelRepository(context);
                }
                return serieModelRepository;
            }
        }
        public SerieMemberModelRepository SerieMemberModelRepository
        {
            get
            {
                if (serieMemberModelRepository == null)
                {
                    serieMemberModelRepository = new SerieMemberModelRepository(context);
                }
                return serieMemberModelRepository;
            }
        }
        public LeagueModelRepository LeagueModelRepository
        {
            get
            {
                if (leagueModelRepository == null)
                {
                    leagueModelRepository = new LeagueModelRepository(context);
                }
                return leagueModelRepository;
            }
        }
        public CountryModelRepository CountryModelRepository
        {
            get
            {
                if (countryModelRepository == null)
                {
                    countryModelRepository = new CountryModelRepository(context);
                }
                return countryModelRepository;
            }
        }
        public OddsModelRepository OddsModelRepository
        {
            get
            {
                if (oddsModelRepository == null)
                {
                    oddsModelRepository = new OddsModelRepository(context);
                }
                return oddsModelRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}

