using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class Parameters
    {
        private static string _randomId = new Random(DateTime.UtcNow.Second).ToString();
        public static string randomID { get { return "1"; } }

        public static bool IsEditMode { get; set; }
    }
}
