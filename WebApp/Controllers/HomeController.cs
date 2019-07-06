using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SamuraiApp.data;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            DbHelper dbHelper = new DbHelper();
            dbHelper.ModefyingRelatedDataWithTracking();

            //var dynamicList = dbHelper.ProjectDynamic();
            //InsertSamurai();
            //InsertMultipleSamurais();
            //SimpleSamuraiQuery();
            //MoreQueryes();
            //RetrivingAndUpdatingSamurai();
            //RetrivingAndUpdatingMultipleSamurai();
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            //DeleteWhileTracking();
            //DeleteMany()
            //DeleteWhileNotTracking();
            //DeleteUsingID(2);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Henrik Obsen" };

            using (SamuraiContext context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }
    }
}
