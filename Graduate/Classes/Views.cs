using Graduate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduate.Classes
{
    public class Views
    {
        public class ReportView
        {
            public Journal Report { get; set; }
            public string Lab { get; set; }
            public string Student { get; set; }
            public string Group { get; set; }
            public List<string> Groups { get; set; }
            public List<string> Labs { get; set; }
        }
        public class StudentView
        {
            public Students Student { get; set; }
            public string Gr { get; set; }
            public List<string> Groups { get; set; } = App._context.Groups.Select(x => x.Title).ToList();
        }
        public class MaterialView
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public List<string> Themes { get; set; } = App._context.Themes.Select(x => x.Title).ToList();
        }
    }
}
