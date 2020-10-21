using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public sealed class Tile {

        private Checker checker;
        public bool IsContainsChecker { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Checker Checker { 
            get {
                return checker;
            } 
            set {
                if (checker != null) {
                    checker = value;
                    checker.Row = Row;
                    checker.Column = Column;
                    IsContainsChecker = checker == null ? false : true;
                }
            }
        }

        public Tile(int row, int column) {
            Row = row;
            Column = column;
        }


        public override bool Equals(Object obj) {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
                return false;
            }
            else {
                Tile t = (Tile)obj;
                return Row == t.Row && Column == t.Column;
            }
        }

        public override int GetHashCode() {
            return (Row * 14 ^ 4) ^ Column * 5;
        }

    }
}
