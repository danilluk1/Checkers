using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public class WhiteChecker : Checker, IConvertableChecker {

        public Direction[] MoveDirections { get; } = {
            new Direction.UpperLeft(),
            new Direction.UpperRight()
        };

        #region Class Designer
        public WhiteChecker(int row, int column, Game game)
            : base(row, column, game, Images.whitechecker, CheckerColor.White) {

        }
        #endregion      

        public override MovesType CountMoveType(int row, int column) {
            //Считаем направления хода относительно текущего положения.
            var direction = CountMoveDirection(row, column);

            //Если шашка ходит на 1 клетку, то проверям, чтобы направление совпадало с возможным.
            if (Math.Abs(Column - column) == Game.DEFAULT_STEP &&
                Math.Abs(Row - row) == Game.DEFAULT_STEP) {

                //Проверяем чтобы шашки могли двигаться только вперед при обычном ходе             
                if (direction.IsWhiteCheckerDirection)
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

            if (row < Game.FIELD_HEIGHT && column < Game.FIELD_WIDTH) {

                List<Move> moves = CountCheckerAbleMoves();

                //Устанавливаем направление хода.
                Direction direction = CountMoveDirection(row, column);

                //Получаем тип хода
                var moveType = CountMoveType(row, column);

                var move = moves.FirstOrDefault(t => t.MoveTile.Equals(Game.Board[row, column]));

                if (move == default) return MovesType.None;

                if (move.AttackedChecker != null) {
                    Game.BlackCheckers.Remove(move.AttackedChecker);
                    Game.Board[move.AttackedChecker].Checker = null;
                }
                else {
                    //Дополнительная подготовка не нужна.
                }

                //Так как ход уже совешен, мы можем установить текущей клетке поле ContainsChecker = false
                Game.Board[Row, Column].IsContainsChecker = false;

                //Устанавливаем шашке, новое положение на поле.
                Column = column;
                Row = row;

                if (IsNeedToBeQueen()) {
                    Game.Board[this].Checker = new WhiteQueen(this);
                }
                else {

                    Game.Board[move.MoveTile].Checker = this;
                }

                return moveType;

            }
            return MovesType.None;           
        }
        /*
         * Проходим от положения шашки циклом в 4 стороны и смотрим, существует ли взоможность съесть
         * */
        public override (bool res, List<Move> moves) CanEat() {
            List<Move> _moves = new List<Move>();
            for(int i = 0; i < AttackDirections.Length; i++) {
                List<Tile> tiles = AttackDirections[i].GetDiagonal(this, Game.Board);
                if (tiles.Count > 2) {
                        if (tiles[Game.DEFAULT_STEP].IsContainsChecker && tiles[Game.DEFAULT_STEP].Checker.Color != Color) {
                            if (!tiles[Game.ATTACK_DEFAULT_STEP].IsContainsChecker) {
                                _moves.Add(new Move(tiles[Game.ATTACK_DEFAULT_STEP], tiles[Game.DEFAULT_STEP].Checker));
                            }
                        }
                }
            }
            if (_moves.Any()) {
                return (true, _moves);
            }
            else {
                return (false, null);
            }
                
        }

        public override List<Move> CountCheckerAbleMoves() {
            List<Move> result = new List<Move>();
            var (canEat, attackTiles) = CanEat();
            if (!canEat && !IsWhitesMustAttack()) {
                for (int i = 0; i < MoveDirections.Length; i++) {
                    List<Tile> tiles = MoveDirections[i].GetDiagonal(this, Game.Board);
                    if (tiles.Count > 1) {
                        for (int j = 0; j < tiles.Count; j++) {
                            if (!tiles[Game.DEFAULT_STEP].IsContainsChecker) {

                                result.Add(new Move(tiles[1]));
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

        public bool IsWhitesMustAttack() {
            foreach (Checker checker in Game.WhiteCheckers) {
                if (checker.CanEat().res) {
                    return true;
                }
            }
            return false;
        }

        public bool IsNeedToBeQueen() {
            return Row == 0;
        }


    }
}
