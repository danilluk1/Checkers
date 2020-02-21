using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public sealed class Team {
        private string _name;
        private int totalPoint;


        public Team(string name) {
            _name = name;
            totalPoint = 0;
        }
    }
}
