using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {

    public enum CheckerColor {
        White,
        Black
    }

    public abstract class Checker {
        //Содержит поле, на котором находится шашка.
        public Game Game { get; }

        //Содержит картинку шашки.
        public Image Image { get; set; }

        //Содержит цвет шашки
        public CheckerColor Color { get; set; }

        //Содержит колонку шашки
        public int Column { get; set; }

        //Содержит ряд шашки
        public int Row { get; set; }

        public Checker(int row, int column, Game game, Image img, CheckerColor color) {
            Game = game;
            Color = color;
            Row = row;
            Column = column;
            Image = img;
        }

        public Direction[] AttackDirections { get; } = {
            new Direction.UpperLeft(),
            new Direction.UpperRight(),
            new Direction.DownRight(),
            new Direction.DownLeft()
        };

        public Direction CountMoveDirection(int moveRow, int moveColumn) {

            if (Row - moveRow > 0 && Column - moveColumn > 0) {
                return new Direction.UpperLeft();
            }
            else if (Row - moveRow > 0 && Column - moveColumn < 0) {
                return new Direction.UpperRight();
            }
            else if (Row - moveRow < 0 && Column - moveColumn > 0) {
                return new Direction.DownLeft();
            }
            else if (Row - moveRow < 0 && Column - moveColumn < 0) {
                return new Direction.DownRight();
            }
            return null;
        }

        public abstract MovesType CountMoveType(int row, int column);

        public abstract MovesType TryToMove(int row, int column);

        public abstract (bool res, List<Move> moves) CanEat();
        public abstract List<Move> CountCheckerAbleMoves();

        public override bool Equals(Object obj) {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
                return false;
            }
            else {
                Checker c = (Checker)obj;
                return (c.Color == Color && c.Row == Row && c.Column == Column);
            }
        }

        public override int GetHashCode() {
            return (Row << 2) ^ Column;
        }

    }
}
