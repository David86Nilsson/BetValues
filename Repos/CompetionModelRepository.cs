//using System.Collections.Generic;
//using System.Linq;
//using WPFAllsvenskan.Data;
//using WPFAllsvenskan.Models;

//namespace WPFAllsvenskan.Services
//{
//    public class CompetionModelRepository
//    {
//        private readonly BetValueDbContext context;

//        public CompetionModelRepository(BetValueDbContext context)
//        {
//            this.context = context;
//        }
//        public List<CompetionModel> GetCompetions()
//        {
//            return context.Competions.ToList();
//        }
//        public CompetionModel? GetCompetion(string name)
//        {
//            return context.Competions.FirstOrDefault(c => c.Name == name);
//        }
//        public void AddCompetion(CompetionModel competion)
//        {
//            context.Competions.Add(competion);
//            context.SaveChanges();
//        }
//        public void UpdateCompetion(CompetionModel competion)
//        {
//            context.Competions.Update(competion);
//        }
//        public void RemoveCompetion(CompetionModel competion)
//        {
//            context.Competions.Remove(competion);
//        }
//    }
//}
