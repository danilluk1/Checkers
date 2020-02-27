using System;
using System.Collections.Generic;

namespace Checkers.BL.Model {

    public enum MovesDirection {
        TopRight,
        TopLeft,
        DownLeft,
        DownRight,
        None
    }

    public enum MovesType {
        Default,
        Attack,
        None
    }

    public sealed class Game {

        #region constansts

        public const int FIELD_WIDTH = 8;
        public const int FIELD_HEIGHT = 8;
        public const int MIN_CORD_VALUE = 0;

        #endregion constansts

        #region private variables

        private Team firstTeam;
        private Team secondTeam;

        private Tile[,] tiles;

        private Checker selectedChecker;

        private bool isWhiteMove = true;

        #endregion private variables

        #region public properties

        public Tile[,] Tiles {
            get {
                return tiles;
            }
        }

        public Checker SelectedChecker {
            get {
                return selectedChecker;
            }
            set {
                selectedChecker = value;
            }
        }
        public List<Checker> WhiteCheckers { get; set; } = new List<Checker>();
        public List<Checker> BlackCheckers { get; set; } = new List<Checker>();

        #endregion public properties

        #region public events

        public event Action FieldUpdated;

        #endregion public events

        #region Class designer

        public Game(Team ft, Team st) {
            //Инициализация команд.
            firstTeam = ft ?? throw new ArgumentNullException(nameof(ft));
            secondTeam = st ?? throw new ArgumentNullException(nameof(st));

            //Инициализация поля.
            tiles = new Tile[FIELD_HEIGHT, FIELD_WIDTH];

            //Заполняем поле.
            for (int y = 0; y < FIELD_HEIGHT; y++) {
                for (int x = 0; x < FIELD_WIDTH; x++) {
                    tiles[x, y] = new Tile();
                }
            }

            //Инициализирем черных
            tiles[0, 1].Checker = new Checker(CheckerType.Black, 0, 1);
            tiles[0, 3].Checker = new Checker(CheckerType.Black, 0, 3);
            tiles[0, 5].Checker = new Checker(CheckerType.Black, 0, 5);
            tiles[0, 7].Checker = new Checker(CheckerType.Black, 0, 7);
            tiles[1, 0].Checker = new Checker(CheckerType.Black, 1, 0);
            tiles[1, 2].Checker = new Checker(CheckerType.Black, 1, 2);
            tiles[1, 4].Checker = new Checker(CheckerType.Black, 1, 4);
            tiles[1, 6].Checker = new Checker(CheckerType.Black, 1, 6);
            tiles[2, 1].Checker = new Checker(CheckerType.Black, 2, 1);
            tiles[2, 3].Checker = new Checker(CheckerType.Black, 2, 3);
            tiles[2, 5].Checker = new Checker(CheckerType.Black, 2, 5);
            tiles[2, 7].Checker = new Checker(CheckerType.Black, 2, 7);

            //Инициализирем белых
            tiles[5, 0].Checker = new Checker(CheckerType.White, 5, 0);
            tiles[5, 2].Checker = new Checker(CheckerType.White, 5, 2);
            tiles[5, 4].Checker = new Checker(CheckerType.White, 5, 4);
            tiles[5, 6].Checker = new Checker(CheckerType.White, 5, 6);
            tiles[6, 1].Checker = new Checker(CheckerType.White, 6, 1);
            tiles[6, 3].Checker = new Checker(CheckerType.White, 6, 3);
            tiles[6, 5].Checker = new Checker(CheckerType.White, 6, 5);
            tiles[6, 7].Checker = new Checker(CheckerType.White, 6, 7);
            tiles[7, 0].Checker = new Checker(CheckerType.White, 7, 0);
            tiles[7, 2].Checker = new Checker(CheckerType.White, 7, 2);
            tiles[7, 4].Checker = new Checker(CheckerType.White, 7, 4);
            tiles[7, 6].Checker = new Checker(CheckerType.White, 7, 6);

            for (int y = 0; y < FIELD_HEIGHT; y++) {
                for (int x = 0; x < FIELD_WIDTH; x++) {
                    if (tiles[x, y].IsContainsChecker) {
                        if (tiles[x, y].Checker.Type == CheckerType.White) {
                            WhiteCheckers.Add(tiles[x, y].Checker);
                        }
                        else {
                            BlackCheckers.Add(tiles[x, y].Checker);
                        }
                    }
                }
            }
        }

        #endregion Class designer

        #region Movement Alogritm

        /*
         * Алгоритм хода фигуры.
         * Принимаем шашку, которой необходимо сходить, и координаты клетки хода.
         * row < 8 и column < 8 (размеры поля)
         * Наше поле знает, как обновлять View при изменении модели поля(Active View)
         */

        public void Move(Checker checker, int row, int column) {
            //Проверям условие, так как иначе нет смысла выполнять эту функцию.
            if (row < FIELD_HEIGHT && column < FIELD_WIDTH) {

                //Проверяем шашку на null
                if (checker == null) return;

                //Устанавливаем направление хода.
                MovesDirection direction = SetMoveDirection(checker, row, column);

                //Проверяем возможность хода
                //Если ход невозможен возращаемся.
                var result = IsCheckerAbleToMove(checker, row, column, direction);
                if (!result.isAble) return;

                //Если ход - атака, то выполняем часть кода для сруба обычной шашкой.
                //TODO Сруб для дамки.
                if (result.type == MovesType.Attack && !checker.IsKing) {
                    var c = 0;
                    var r = 0;
                    switch (direction) {
                        case MovesDirection.DownLeft:
                        r = checker.Row + 1;
                        c = checker.Column - 1;
                        break;

                        case MovesDirection.DownRight:
                        r = checker.Row + 1;
                        c = checker.Column + 1;
                        break;

                        case MovesDirection.TopLeft:
                        r = checker.Row - 1;
                        c = checker.Column - 1;
                        break;

                        case MovesDirection.TopRight:
                        r = checker.Row - 1;
                        c = checker.Column + 1;
                        break;
                    }
                    if(tiles[r, c].Checker.Type == CheckerType.White) {
                        WhiteCheckers.Remove(tiles[r, c].Checker);
                    }
                    else {
                        BlackCheckers.Remove(tiles[r, c].Checker);
                    }
                    tiles[r, c].Checker = null;
                }


                //Если сходили белые, даем сходить черным, и наоборот.
                isWhiteMove = checker.Type == CheckerType.White ? false : true; 

                //Так как ход уже совешен, мы можем установить текущей клетке поле ContainsChecker = false
                tiles[checker.Row, checker.Column].IsContainsChecker = false;
                tiles[checker.Row, checker.Column].Checker = null;

                //Устанавливаем шашке, новое положение на поле.
                checker.Column = column;
                checker.Row = row;

                //Устанавливаем в клетку, в которую совершается ход нашу шашку.
                tiles[row, column].Checker = (Checker)checker.Clone();              

                //После хода автоматически выбираем нашу шашку
                selectedChecker = (Checker)tiles[row, column].Checker.Clone();
               
                //Пробрасываем событие изменения поля
                FieldUpdated?.Invoke();
            }
        }

        #endregion Movement Alogritm

        #region Movement ables algoritm

        /*TODO
         * Алгоритм проверки возможность хода
         * Принимаем шашку, возвращаем возможность хода
         * Необходимо проверять ход на срубы, + возможности дамки.
         */

        private (bool isAble, MovesType type) IsCheckerAbleToMove(Checker checker, int row, int column, MovesDirection direction) {
            /*
             * Проверяем, чтобы координаты нашего кода не выходили за пределы поля.
             * Если она выходит за пределы, то нет смысла выполнять метод.
            */
            if (row > FIELD_WIDTH |
                row < MIN_CORD_VALUE |
                column < MIN_CORD_VALUE |
                column > FIELD_HEIGHT)
                return (false, MovesType.None);

            /*
             * Если клетка, в которую совершается ход уже занята возвращаем false.
             * Можно не производить дальнейшие вычисления.
             */
            if (tiles[row, column].IsContainsChecker)
                return (false, MovesType.None);

            //Следим за поядком ходов.
            if (checker.Type == CheckerType.White && !isWhiteMove)
                return (false, MovesType.None);
            else if (checker.Type == CheckerType.Black && isWhiteMove)
                return (false, MovesType.None);

            //Проверяем, чтобы при обычном ходе была возможность сходить только на 1 по диагонали.
            if (Math.Abs(checker.Column - column) == 1 &&
                Math.Abs(checker.Row - row) == 1 && !IsCheckerNeedToAttack(CheckerType.White)) {
                //Проверяем чтобы шашки могли двигаться только вперед при обычном ходе
                if (checker.Type == CheckerType.Black) {
                    if (direction == MovesDirection.DownLeft || direction == MovesDirection.DownRight)
                        return (true, MovesType.Default);
                }
                else {
                    if (direction == MovesDirection.TopLeft || direction == MovesDirection.TopRight)
                        return (true, MovesType.Default);
                }
            }
            //Проверяем чтобы при срубе обычной дамкой все происходило как надо.
            else if (Math.Abs(checker.Column - column) == 2 &&
                     Math.Abs(checker.Row - row) == 2) {
                int c;
                int r;
                switch (direction) {
                    case MovesDirection.DownLeft:
                    r = checker.Row + 1;
                    c = checker.Column - 1;
                    if (tiles[r, c].IsContainsChecker && tiles[r, c].Checker.Type != checker.Type)
                        return (true, MovesType.Attack);
                    break;

                    case MovesDirection.DownRight:
                    r = checker.Row + 1;
                    c = checker.Column + 1;
                    if (tiles[r, c].IsContainsChecker && tiles[r, c].Checker.Type != checker.Type)
                        return (true, MovesType.Attack);
                    break;

                    case MovesDirection.TopLeft:
                    r = checker.Row - 1;
                    c = checker.Column - 1;
                    if (tiles[r, c].IsContainsChecker && tiles[r, c].Checker.Type != checker.Type)
                        return (true, MovesType.Attack);
                    break;

                    case MovesDirection.TopRight:
                    r = checker.Row - 1;
                    c = checker.Column + 1;
                    if (tiles[r, c].IsContainsChecker && tiles[r, c].Checker.Type != checker.Type)
                        return (true, MovesType.Attack);
                    break;
                }
            }

            //Цикл пригодится при реализации атак.
            for (int y = 0; y < FIELD_HEIGHT; y++) {
                for (int x = 0; x < FIELD_WIDTH; x++) {
                }
            }
            return (false, MovesType.None);
        }
        #endregion Movement ables algoritm

        //Функция устанавлиевает направление хода для шашки
        private MovesDirection SetMoveDirection(Checker checker, int moveRow, int moveColumn) {
            if (checker.Row - moveRow > 0 && checker.Column - moveColumn > 0) {
                return MovesDirection.TopLeft;
            }
            else if (checker.Row - moveRow > 0 && checker.Column - moveColumn < 0) {
                return MovesDirection.TopRight;
            }
            else if (checker.Row - moveRow < 0 && checker.Column - moveColumn > 0) {
                return MovesDirection.DownLeft;
            }
            else if (checker.Row - moveRow < 0 && checker.Column - moveColumn < 0) {
                return MovesDirection.DownRight;
            }
            return MovesDirection.None;
        }

        private bool IsCheckerNeedToAttack(CheckerType type) {
            if (type == CheckerType.White) {
                foreach (Checker c in WhiteCheckers) {
                    if (c.Row < 6 && c.Column > 2) {
                        if (tiles[c.Row + 1, c.Column - 1].IsContainsChecker && !tiles[c.Row + 2, c.Column - 2].IsContainsChecker)
                            return true;
                    }
                    if (c.Column < 6 && c.Row < 6) {
                        if (tiles[c.Row + 1, c.Column + 1].IsContainsChecker && !tiles[c.Row + 2, c.Column + 2].IsContainsChecker)
                            return true;
                    }
                    if (c.Row > 2 && c.Column > 2) {
                        if (tiles[c.Row - 1, c.Column - 1].IsContainsChecker && !tiles[c.Row - 2, c.Column - 2].IsContainsChecker)
                            return true;
                    }
                    if (c.Row > 2 && c.Column < 6) {
                        if (tiles[c.Row - 1, c.Column + 1].IsContainsChecker && !tiles[c.Row - 2, c.Column + 2].IsContainsChecker)
                            return true;
                    }
                }
            }
            else {
                foreach (Checker c in BlackCheckers) {
                    if (c.Row < 6 && c.Column > 2) {
                        if (tiles[c.Row + 1, c.Column - 1].IsContainsChecker && !tiles[c.Row + 2, c.Column - 2].IsContainsChecker)
                            return true;
                    }
                    if (c.Column < 6 && c.Row < 6) {
                        if (tiles[c.Row + 1, c.Column + 1].IsContainsChecker && !tiles[c.Row + 2, c.Column + 2].IsContainsChecker)
                            return true;
                    }
                    if (c.Row > 2 && c.Column > 2) {
                        if (tiles[c.Row - 1, c.Column - 1].IsContainsChecker && !tiles[c.Row - 2, c.Column - 2].IsContainsChecker)
                            return true;
                    }
                    if (c.Row > 2 && c.Column < 6) {
                        if (tiles[c.Row - 1, c.Column + 1].IsContainsChecker && !tiles[c.Row - 2, c.Column + 2].IsContainsChecker)
                            return true;
                    }
                }
            }

            return false;
        }
    }
}