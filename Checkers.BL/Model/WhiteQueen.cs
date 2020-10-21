using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public class WhiteQueen : Checker {

        public Direction[] MoveDirections { get; } = {
            new Direction.DownLeft(),
            new Direction.DownRight(),
            new Direction.UpperRight(),
            new Direction.UpperLeft()
        };

        #region Class Designer
        public WhiteQueen(int row, int column, Game game)
            : base(row, column, game, Images.whitequeen, CheckerColor.White) {

        }
        #endregion  

        public override (bool res, List<Tile> attackTiles) CanEat() {
            throw new NotImplementedException();
        }

        public override List<Tile> CountCheckerAbleTiles() {
            throw new NotImplementedException();
        }

        public override MovesType CountMoveType(int row, int column) {
            throw new NotImplementedException();
        }

        public override MovesType TryToMove(int row, int column) {
            throw new NotImplementedException();
        }
    }
}
