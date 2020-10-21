using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    class BlackChecker : Checker {

        public Direction[] MoveDirections { get; } = {
            new Direction.DownLeft(),
            new Direction.DownRight()
        };

        #region Class Designer
        public BlackChecker(int row, int column, Game game)
            : base(row, column, game, Images.blackchecker, CheckerColor.Black) {

        }
        #endregion      

        public override MovesType CountMoveType(int row, int column) {
            //Считаем направления хода относительно текущего положения.
            var direction = CountMoveDirection(row, column);
            //Если шашка ходит на 1 клетку, то проверям, чтобы направление совпадало с возможным.
            if (Math.Abs(Column - column) == Game.DEFAULT_STEP &&
                Math.Abs(Row - row) == Game.DEFAULT_STEP) {

                //Проверяем чтобы шашки могли двигаться только вперед при обычном ходе             
                if (!direction.IsWhiteCheckerDirection)
                    return MovesType.Default;
            }
            //Если шашка ходит на 2 клетки, то проверяем, чтобы был возможен сруб.
            if (Math.Abs(Column - column) == Game.ATTACK_DEFAULT_STEP &&
                     Math.Abs(Row - row) == Game.ATTACK_DEFAULT_STEP) {
                int col;
                int ro;

                col = Column + direction.ColumnIndex;
                ro = Row + direction.RowIndex;

                if (Game.Board[ro, col].IsContainsChecker &&
                        Game.Board[ro, col].Checker.Color != Color) {
                    return MovesType.Attack;
                }
            }
            return MovesType.None;
        }

        public override MovesType TryToMove(int row, int column) {
            //Проверям условие, так как иначе нет смысла выполнять эту функцию.
            if (row < Game.FIELD_HEIGHT && column < Game.FIELD_WIDTH) {

                //Считаем направления хода относительно текущего положения.
                Direction direction = CountMoveDirection(row, column);

                List<Tile> tiles = CountCheckerAbleTiles();
                if (!tiles.Contains(Game.Board[row, column])) {
                    return MovesType.None;
                }

                //Получаем тип хода
                var moveType = CountMoveType(row, column);

                if (moveType == MovesType.Attack) {
                    int c;
                    int r;

                    r = Row + direction.RowIndex;
                    c = Column + direction.ColumnIndex;

                    Game.WhiteCheckers.Remove(Game.Board[r, c].Checker);
                    Game.Board[r, c].Checker = null;
                }
                else {

                }

                //Так как ход уже совешен, мы можем установить текущей клетке поле ContainsChecker = false
                Game.Board[Row, Column].IsContainsChecker = false;
                Game.Board[Row, Column].Checker = null;

                //Устанавливаем шашке, новое положение на поле.
                Column = column;
                Row = row;

                //Устанавливаем в клетку, в которую совершается ход нашу шашку.
                Game.Board[row, column].Checker = this;

                return moveType;
            }
            return MovesType.None;
        }
        /*
         * Проходим от положения шашки циклом в 4 стороны и смотрим, существует ли взоможность съесть
         * */
        public override (bool res, List<Tile> attackTiles) CanEat() {
            List<Tile> attackTiles = new List<Tile>();
            for (int i = 0; i < AttackDirections.Length; i++) {
                List<Tile> tiles = AttackDirections[i].GetDiagonal(this, Game.Board);
                if (tiles.Count > 2) {
                    for (int j = 0; j < tiles.Count; j++) {
                        if (tiles[1].IsContainsChecker && tiles[1].Checker.Color != Color) {
                            if (!tiles[2].IsContainsChecker) {
                                attackTiles.Add(tiles[2]);
                            }
                        }
                    }
                }
            }
            if (attackTiles.Any()) {
                return (true, attackTiles);
            }
            else {
                return (false, null);
            }

        }

        public override List<Tile> CountCheckerAbleTiles() {
            List<Tile> result = new List<Tile>();
            var (canEat, attackTiles) = CanEat();
            if (!canEat && !IsMustAttack()) {
                for (int i = 0; i < MoveDirections.Length; i++) {
                    List<Tile> tiles = MoveDirections[i].GetDiagonal(this, Game.Board);
                    if (tiles.Count > 1) {
                        for (int j = 0; j < tiles.Count; j++) {
                            if (!tiles[1].IsContainsChecker) {

                                result.Add(tiles[1]);
                            }
                        }
                    }
                }
            }
            else {
                if (attackTiles != null) {
                    result.AddRange(attackTiles);
                }
            }


            return result;
        }

        public bool IsMustAttack() {
            foreach (Checker checker in Game.BlackCheckers) {
                if (checker.CanEat().res) {
                    return true;
                }
            }
            return false;
        }
    }
}
