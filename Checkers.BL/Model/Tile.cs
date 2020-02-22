using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public sealed class Tile {

        private Checker checker;
        public bool IsContainsChecker { get; set; }

        public Checker Checker { 
            get {
                return checker;
            } 
            set {
                checker = value;
                IsContainsChecker = checker == null ? false : true;               
            }
        }

    }
}
