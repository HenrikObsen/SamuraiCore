using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SamuraiApp.Data;
using SamuraiApp.Domain;


namespace SamuraiApp.data
{
    public class DbHelper
    {
        private SamuraiContext _context = new SamuraiContext();
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
