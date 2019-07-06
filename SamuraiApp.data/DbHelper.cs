using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;


namespace SamuraiApp.data
{
    public class DbHelper
    {
        private SamuraiContext _context = new SamuraiContext();

        //https://app.pluralsight.com/player?course=entity-framework-core-2-getting-started


        public void ModefyingRelatedDataWhyNotTracking()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            var quote = samurai.Quotes[0];
            quote.Text += " Did you hear that?";

            using(var newContext = new SamuraiContext())
            {
                // Med update vil quote blive opdateret 2 gange
                //newContext.Quotes.Update(quote);

                //Brug istedet Entry og EntityState til at gøre opmærksom på at quote er ændret så vil den kun blive opdateret en gang.
                newContext.Entry(quote).State = EntityState.Modified;
                newContext.SaveChanges();
            }

        }



        public void ModefyingRelatedDataWithTracking()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            samurai.Quotes[0].Text += " Did you hear that?";

            //Sletning af relateret data.
            //samurai.Quotes.Remove(samurai.Quotes.Where(q => q.Text.Contains("horse")).FirstOrDefault());
           
            _context.SaveChanges();
        }




        public void FilteringOmRelatedData()
        {
            //Filtrere ud fra relateret data uden at få det relaterede data med i resultatet
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("Kill")))
                .ToList();
        }

        public List<dynamic> ProjectDynamicWithQuots()
        {
            //Laver man to forespørgsler på den samme context samler EF selv resultatet som her under hvor Quotes automatisk vil blive en del af samurai.
            var samurais = _context.Samurais.ToList();
            var killQuots = _context.Quotes.Where(q => q.Text.Contains("Kill")).ToList();
            
            return samurais.ToList<dynamic>();
        }

        //Returnere en anonym type
        public List<dynamic> ProjectDynamic()
        {
             var someProps = _context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes }).ToList();
            // var someProps = _context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes.Count }).ToList();
            //var someProps = _context.Samurais.Select(s => new { s.Id, s.Name, newProp = s.Quotes.Where(q => q.Text.Contains("kill")).ToList() }).ToList();
                        
            return someProps.ToList<dynamic>();
        }
               
        public List<Samurai> EagerLoadingSamuraisWithQouts()
        {
            var samuraiWithQuots = _context.Samurais.Include(s => s.Quotes).ToList();
           // var samuraiWithQuots = _context.Samurais.FirstOrDefault(s => s.Name.Contains("Henrik"));
           
            return samuraiWithQuots;
        }

        public Samurai EagerLoadingSamuraisWithQoutsFiltered()
        {
                var samuraiWithQuots = _context.Samurais.Where(s => s.Name.Contains("Henrik"))
                .Include(s => s.Quotes)
                .Include(s=>s.SecretIdentity)
                .FirstOrDefault();

            return samuraiWithQuots;
        }

        public Samurai EagerLoadingOneSamuraisWithQoutsFiltered(int id)
        {
            var samuraiWithQuots = _context.Samurais.Where(s => s.Id == id)
            .Include(s => s.Quotes)
            .Include(s => s.SecretIdentity)
            .FirstOrDefault();

            return samuraiWithQuots;
        }
        public void AddChildrenToExistingObjectWhileNotTracking(int samuraiId)
        {

            var quote = new Quote
            {
                Text = "I bee back!",
                SamuraiId = samuraiId        
            };

            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }             
        }

        public void InsertNewPkFkGraphMultioleChildren()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "I kill you!!"},
                    new Quote {Text = "I told you to watsh out!"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        public void InsertNewPkFkGraph()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "Ive welcome you!!"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        public void DeleteUsingID(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();

        }

        public void DeleteWhileNotTracking()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Henrik");
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Remove(samurai);
                newContext.SaveChanges();
            }

        }

        public void DeleteMany()
        {
            var samurais = _context.Samurais.Where(s => s.Name.Contains("o"));
            _context.Samurais.RemoveRange(samurais);
            //_context.Samurais.RemoveRange(_context.Samurais.Where(s => s.Name.Contains("o")));
            //_context.RemoveRange(samurais);
            _context.SaveChanges();
        }

        public void DeleteWhileTracking()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "HenrikTextSan");
            _context.Samurais.Remove(samurai);
            // Alternativer
            //_context.Remove(samurai);
            //_context.Samurais.Remove(_context.Samurai.Find(1));
            _context.SaveChanges();
        }

        public void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);

            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }

        }

        public void InsertBattle()
        {
            _context.Battles.Add(new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) });
            _context.SaveChanges();

        }

        public void RetrivingAndUpdatingMultipleSamurai()
        {
            var samurais = _context.Samurais.ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }

        public void RetrivingAndUpdatingSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            //samurai.Name += "Test";
            samurai.Name = "Test";
            _context.SaveChanges();
        }

        private void MoreQueryes()
        {
            //var name = "Chubby";
            // var samurais = _context.Samurais.Where(s => s.Name == name);
            //var samurais = _context.Samurais.FirstOrDefault(s => s.Name == name);
            var samurais = _context.Samurais.Find(2);
        }

        public void SimpleSamuraiQuery()
        {
            using (var context = new SamuraiContext())
            {
                var samurais = context.Samurais.ToList();
            }
        }

        public void InsertMultipleSamurais()
        {
            var samurai = new Samurai { Name = "Henrik" };
            var samuraiChups = new Samurai { Name = "Chubby" };

            using (var context = new SamuraiContext())
            {
                context.Samurais.AddRange(samurai, samuraiChups);
                context.SaveChanges();
            }

        }

        public void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Henrik" };

            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }
    }
}
