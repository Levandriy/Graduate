using Graduate.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Graduate
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LabsAutomationSystemEntities _context = new LabsAutomationSystemEntities();
        public static DateTime? StartPeriod { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public static DateTime? EndPeriod { get; set; } = StartPeriod.Value.AddMonths(1).AddDays(-1);
        public static Guid user { get; set; } = _context.Teachers.Select(x => x.Id).FirstOrDefault();
    }
}
