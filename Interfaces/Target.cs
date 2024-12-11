using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servecoin.interfaces
{
    public interface ITarget
    {
        string name { get; set; }
        string target {  get; set; }
        string accumulated { get; set; }
        string currency { get; set; }
    }

    public class Target : ITarget
    {
        public string name { get; set; }
        public string target { get; set; }
        public string accumulated { get; set; }
        public string currency { get; set; }
    }
}
