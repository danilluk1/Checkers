using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Checkers.BL.Model {

    public enum CheckerColor {
        White,
        Black
    }

    public sealed class Checker : IChecker, ICloneable {
        public Image Image { get; set; }

        public CheckerColor Color { get; set; }

        public int Column { get; set; }

        public int Row { get; set; }

        public Tile[,] Field { get; set; }

        public IEnumerable<Checker> Checkers { get; }

        #region Class Designer
        public Checker(CheckerColor color, int row, int column, Tile[,] field, IEnumerable<Checker> checkers) {
            Field = field;
            Checkers = checkers;
            Color = color;
            Row = row;
            Column = column;
            Image = color == CheckerColor.White ? Images.whitechecker : Images.blackchecker;

        }
        #endregion

        

        public bool IsCheckerAbleToMove(int row, int column, MovesDirection direction) {
            /*
             * Проверяем, чтобы координаты нашего кода не выходили за пределы поля.
             * Если она выходит за пределы, то нет смысла выполнять метод.
            */
            if (row > Game.FIELD_WIDTH |
                row < Game.MIN_CORD_VALUE |
                column < Game.MIN_CORD_VALUE |
                column > Game.FIELD_HEIGHT)
                return false;

            /*
             * Если клетка, в которую совершается ход уже занята возвращаем false.
             * Можно не производить дальнейшие вычисления.
             */
            if (Field[row, column].IsContainsChecker)
                return false;

            if (IsMustAttack() && !this.CanEat()) 
                return false;

            //Проверяем, чтобы при обычном ходе была возможность сходить только на 1 по диагонали.
            if (Math.Abs(Column - column) == Game.DEFAULT_STEP &&
                Math.Abs(Row - row) == Game.DEFAULT_STEP){
                //Проверяем чтобы шашки могли двигаться только вперед при обычном ходе
                if (Color == CheckerColor.Black) {
                    if (direction == MovesDirection.DownLeft || direction == MovesDirection.DownRight)
                        return true;
                }
                else {
                    if (direction == MovesDirection.TopLeft || direction == MovesDirection.TopRight)
                        return true;
                }
            }
            if (Math.Abs(Column - column) == Game.ATTACK_DEFAULT_STEP &&
                     Math.Abs(Row - row) == Game.ATTACK_DEFAULT_STEP) {
                int c;
                int r;
                switch (direction) {
                    case MovesDirection.DownLeft:
                    r = Row + 1;
                    c = Column - 1;
                    if (Field[r, c].IsContainsChecker && Field[r, c].Checker.Color != Color) {
                        return true;
                    }
                    break;

                    case MovesDirection.DownRight:
                    r = Row + 1;
                    c = Column + 1;
                    if (Field[r, c].IsContainsChecker && Field[r, c].Checker.Color != Color) {
                        return true;
                    }
                    break;

                    case MovesDirection.TopLeft:
                    r = Row - 1;
                    c = Column - 1;
                    if (Field[r, c].IsContainsChecker && Field[r, c].Checker.Color != Color) {
                        return true;
                    }
                    break;

                    case MovesDirection.TopRight:
                    r = Row - 1;
                    c = Column + 1;
                    if (Field[r, c].IsContainsChecker && Field[r, c].Checker.Color != Color) {
                        return true;
                    }
                    break;
                }
            }

            return false;
        }

        public MovesDirection CountMoveDirection(int moveRow, int moveColumn) {

            if (Row - moveRow > 0 && Column - moveColumn > 0) {
                return MovesDirection.TopLeft;
            }
            else if (Row - moveRow > 0 && Column - moveColumn < 0) {
                return MovesDirection.TopRight;
            }
            else if (Row - moveRow < 0 && Column - moveColumn > 0) {
                return MovesDirection.DownLeft;
            }
            else if (Row - moveRow < 0 && Column - moveColumn < 0) {
                return MovesDirection.DownRight;
            }
            return MovesDirection.None;
        }

        public MovesType CountMoveType(int row, int column) {

            var direction = CountMoveDirection(row, column);
            if (Math.Abs(Column - column) == Game.DEFAULT_STEP &&
                Math.Abs(Row - row) == Game.DEFAULT_STEP){

                //Проверяем чтобы шашки могли двигаться только вперед при обычном ходе
                if (Color == CheckerColor.Black) {
                    if (direction == MovesDirection.DownLeft || direction == MovesDirection.DownRight)
                        return MovesType.Default;
                }
                else {
                    if (direction == MovesDirection.TopLeft || direction == MovesDirection.TopRight)
                        return MovesType.Default;
                }
            }
            //Проверяем чтобы при срубе обычной шашкой все происходило как надо.
            if (Math.Abs(Column - column) == Game.ATTACK_DEFAULT_STEP &&
                     Math.Abs(Row - row) == Game.ATTACK_DEFAULT_STEP) { 
                int col;
                int ro;
                switch (direction) {
                    case MovesDirection.DownLeft:

                    ro = Row + 1;
                    col = Column - 1;

                    if (Field[ro, col].IsContainsChecker && 
                        Field[ro, col].Checker.Color != Color) {
                        return MovesType.Attack;
                    }
                    break;

                    case MovesDirection.DownRight:

                    ro = Row + 1;
                    col = Column + 1;
                    if (Field[ro, col].IsContainsChecker && Field[ro, col].Checker.Color != Color) {
                        return MovesType.Attack;
                    }
                    break;

                    case MovesDirection.TopLeft:
                    ro = Row - 1;
                    col = Column - 1;
                    if (Field[ro, col].IsContainsChecker && Field[ro, col].Checker.Color != Color) {
                        return MovesType.Attack;
                    }
                    break;

                    case MovesDirection.TopRight:
                    ro = Row - 1;
                    col = Column + 1;
                    if (Field[ro, col].IsContainsChecker && Field[ro, col].Checker.Color != Color) {
                        return MovesType.Attack;
                    }
                    break;

                    default:
                        return MovesType.None;
                }
            }
            return MovesType.None;
        }

        public MovesType TryToMove(int row, int column) {
            //Проверям условие, так как иначе нет смысла выполнять эту функцию.
            if (row < Game.FIELD_HEIGHT && column < Game.FIELD_WIDTH) {           

                //Устанавливаем направление хода.
                MovesDirection direction = CountMoveDirection(row, column);

                //Проверяем возможность хода
                //Если ход невозможен возращаемся.
                var result = IsCheckerAbleToMove(row, column, direction);
                var moveType = CountMoveType(row, column);
                if (!result) return MovesType.None;

                if (moveType == MovesType.Attack) {
                    int c;
                    int r;

                    switch (direction) {
                        case MovesDirection.DownLeft:
                        r = Row + 1;
                        c = Column - 1;
                        break;

                        case MovesDirection.DownRight:
                        r = Row + 1;
                        c = Column + 1;
                        break;

                        case MovesDirection.TopLeft:
                        r = Row - 1;
                        c = Column - 1;
                        break;

                        case MovesDirection.TopRight:
                        r = Row - 1;
                        c = Column + 1;
                        break;
                        default:
                        return MovesType.None;
                    }
                    Field[r, c].Checker = null;
                    Checkers.ToList().Remove(Field[r, c].Checker);
                }
                else {

                }

                //Так как ход уже совешен, мы можем установить текущей клетке поле ContainsChecker = false
                Field[Row, Column].IsContainsChecker = false;
                Field[Row, Column].Checker = null;

                //Устанавливаем шашке, новое положение на поле.
                Column = column;
                Row = row;

                //Устанавливаем в клетку, в которую совершается ход нашу шашку.
                Field[row, column].Checker = (Checker)this.Clone();

                return moveType;
            }
            return MovesType.None;
        }
        /*
         * Проходим от положения шашки циклом в 4 стороны и смотрим, существует ли взоможность съесть
         * */
        public bool CanEat() {

            try {
                if (Color == CheckerColor.White) {
                    if (Row + 1 < 8 && Row + 2 < 8 && Row - 1 >= 0 && Row - 2 >= 0) {
                        if (Column + 1 < 8 && Column + 2 < 8 && Column - 1 >= 0 && Column - 2 >= 0) {
                            if (Field[Row + 1, Column + 1].IsContainsChecker && Field[Row + 1, Column + 1].Checker.Color != Color && !Field[Row + 2, Column + 2].IsContainsChecker) {
                                return true;
                            }
                            else if (Field[Row - 1, Column + 1].IsContainsChecker && Field[Row - 1, Column + 1].Checker.Color != Color && !Field[Row - 2, Column + 2].IsContainsChecker) {
                                return true;
                            }
                            else if (Field[Row + 1, Column - 1].IsContainsChecker && Field[Row + 1, Column - 1].Checker.Color != Color && !Field[Row + 2, Column - 2].IsContainsChecker) {
                                return true;
                            }
                            else if (Field[Row - 1, Column - 1].IsContainsChecker && Field[Row - 1, Column - 1].Checker.Color != Color && !Field[Row - 2, Column - 2].IsContainsChecker) {
                                return true;
                            }
                        }
                    }
                    return false;
                }
                else if (Color == CheckerColor.Black) {
                    if (Row + 1 < 8 && Row + 2 < 8 && Row - 1 >= 0 && Row - 2 >= 0) {
                        if (Column + 1 < 8 && Column + 2 < 8 && Column - 1 >= 0 && Column - 2 >= 0) {
                            if (Field[Row + 1, Column + 1].IsContainsChecker && Field[Row + 1, Column + 1].Checker.Color != Color && !Field[Row + 2, Column + 2].IsContainsChecker) {
                                return true;
                            }
                            else if (Field[Row - 1, Column + 1].IsContainsChecker && Field[Row - 1, Column + 1].Checker.Color != Color && !Field[Row - 2, Column + 2].IsContainsChecker) {
                                return true;
                            }
                            else if (Field[Row + 1, Column - 1].IsContainsChecker && Field[Row + 1, Column - 1].Checker.Color != Color && !Field[Row + 2, Column - 2].IsContainsChecker) {
                                return true;
                            }
                            else if (Field[Row - 1, Column - 1].IsContainsChecker && Field[Row - 1, Column - 1].Checker.Color != Color && !Field[Row - 2, Column - 2].IsContainsChecker) {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            catch (IndexOutOfRangeException) {
                return false;
            }
            return false;
        }

        public bool IsMustAttack() {
            foreach(IChecker checker in Checkers) {
                if (checker.CanEat()) {
                    return true;
                }
            }
            return false;
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
