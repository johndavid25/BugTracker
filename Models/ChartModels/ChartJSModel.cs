using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ChartModels
{
    public class ChartJSModel
    {

        public ChartJSModel()
        {
            Labels = new List<string>();
            Data = new List<int>();
            BackgroundColors = new List<string>();
            BorderColors = new List<string>();
        }

        public List<string> Labels { get; set; }

        public List<int> Data { get; set; }

        public List<string> BackgroundColors { get; set; }

        public List<string> BorderColors { get; set; }
    }
}
