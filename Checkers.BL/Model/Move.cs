using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public sealed class Move {
        public Checker AttackedChecker { get; set; }

        public Tile MoveTile { get; set; }

        public Move(Tile moveTile, Checker attackedChecker = null) {
            AttackedChecker = attackedChecker;
            MoveTile = moveTile;
        }
    }
}
