using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PipSep.Models
{
    public class PipSepDb : DbContext
    {
        public DbSet<Page> Pages { get; set; }
		public DbSet<ConnectedAccounts> ConnectedAccounts { get; set; }
        public DbSet<ApplicationCV> ApplicationCVs { get; set; }
		public DbSet<CommonThings> CommonThings { get; set; }
    }

    public class PipSepDbInitializer : DropCreateDatabaseIfModelChanges<PipSepDb>
    {
        protected override void Seed(PipSepDb context)
        {
            //Define start settings

            var pages = new List<Page>{
                new Page{
                    Title="About",
                    Content="Suntem cei mai buni",
                    IsMenu=true,
                    DateCreated = DateTime.Now,
					Author = "Default",
                },
                new Page{
                    Title="Program",
                    Content="In fiecare zi dimineata lucram",
                    IsMenu=true,
                    DateCreated = DateTime.Now,
					Author = "Default",
                },
            };

            pages.ForEach(i => context.Pages.Add(i));

            /*var cvs = new List<ApplicationCV>{
                new ApplicationCV{
                    UserName = "aaaaaa",
                    QuestionCV = "a",
                    QuestionWhy = "a",
                    QuestionWhyYou = "a",
                }
            };
            cvs.ForEach(i => context.ApplicationCVs.Add(i));*/
        }
    }

}