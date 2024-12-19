using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servecoin.interfaces
{
    public interface IGoal
    {
        string name { get; set; }
        string goal {  get; set; }
        string accumulated { get; set; }
        string currency { get; set; }
    }

    public class Goal : IGoal
    {
        public string name { get; set; }
        public string goal { get; set; }
        public string accumulated { get; set; }
        public string currency { get; set; }
    }
}
