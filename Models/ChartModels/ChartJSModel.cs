using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ChartModels
{
    public class ChartJSModel
    {
        //I need 2 collections of strings and 1 collections of integers
        public List<string> Labels { get; set; }

        public List<int> Data { get; set; }

        public List<string> BackgroundColors { get; set; }
    }
}
