using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public class BlackQueen : Checker {

        public Direction[] MoveDirections { get; } = {
            new Direction.DownLeft(),
            new Direction.DownRight(),
            new Direction.UpperRight(),
            new Direction.UpperLeft()
        };

        #region Class Designer
        public BlackQueen(int row, int column, Game game)
            : base(row, column, game, Images.whitequeen, CheckerColor.White) {

        }


        public BlackQueen(Checker checker)
            : base(checker.Row, checker.Column, checker.Game, Images.blackqueen, CheckerColor.Black) {

        }
        #endregion  

        public override (bool res, List<Move> moves) CanEat() {
            List<Move> _moves = new List<Move>();
            for (int i = 0; i < AttackDirections.Length; i++) {
                List<Tile> tiles = AttackDirections[i].GetDiagonal(this, Game.Board);
                if (tiles.Count > 2) {
                    for (int j = 1; j < tiles.Count; j++) {
                        if (tiles[j].IsContainsChecker && tiles[j].Checker.Color != Color) {
                            for (int k = j + 1; k < tiles.Count && !tiles[k].IsContainsChecker; k++) {
                                if (!tiles[k].IsContainsChecker) {
                                    _moves.Add(new Move(tiles[k], tiles[j].Checker));
                                }
                            }
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
                    if (tiles.Count > 2) {
                        for (int j = 1; j < tiles.Count && !tiles[j].IsContainsChecker; j++) {
                            if (!tiles[j].IsContainsChecker) {

                                result.Add(new Move(tiles[j], null));
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

        public override MovesType CountMoveType(int row, int column) {
            //Считаем направления хода относительно текущего положения.
            var direction = CountMoveDirection(row, column);

            //Если шашка ходит на 1 клетку, то проверям, чтобы направление совпадало с возможным.
            if (Math.Abs(Column - column) >= Game.DEFAULT_STEP &&
                Math.Abs(Row - row) >= Game.DEFAULT_STEP) {
                return MovesType.Default;
            }

            //Если шашка ходит на 2 клетки, то проверяем, чтобы был возможен сруб.
            if (Math.Abs(Column - column) >= Game.ATTACK_DEFAULT_STEP &&
                     Math.Abs(Row - row) >= Game.ATTACK_DEFAULT_STEP) {
                int col;
                int ro;


                if (CanEat().res) {
                    return MovesType.Attack;
                }
            }
            return MovesType.None;
        }

        public override MovesType TryToMove(int row, int column) {
            //Проверям условие, так как иначе нет смысла выполнять эту функцию.
            if (row < Game.FIELD_HEIGHT && column < Game.FIELD_WIDTH) {

                List<Move> moves = CountCheckerAbleMoves();

                //Устанавливаем направление хода.
                Direction direction = CountMoveDirection(row, column);

                //Получаем тип хода
                var moveType = CountMoveType(row, column);

                var move = moves.FirstOrDefault(t => t.MoveTile.Equals(Game.Board[row, column]));

                if (move == default) return MovesType.None;

                if (move.AttackedChecker != null) {
                    Game.WhiteCheckers.Remove(move.AttackedChecker);
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

                Game.Board[move.MoveTile].Checker = this;

                return moveType;

            }
            return MovesType.None;
        }

    }
}
