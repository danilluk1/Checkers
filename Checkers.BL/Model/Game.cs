using System;

namespace Checkers.BL.Model {

    public sealed class Game {

        #region constansts
        public const int FIELD_WIDTH = 8;
        public const int FIELD_HEIGHT = 8;
        public const int MIN_CORD_VALUE = 0;
        #endregion

        #region private variables
        private Team firstTeam;
        private Team secondTeam;
        private Tile[,] tiles;
        private Checker selectedChecker;
        #endregion

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
        #endregion

        #region public events
        public event Action FieldUpdated;
        #endregion

        #region Class designer
        public Game(Team ft, Team st) {

            //Инициализация команд.
            firstTeam = ft ?? throw new ArgumentNullException(nameof(ft));
            secondTeam = st ?? throw new ArgumentNullException(nameof(st));

            //Инициализация поля.
            tiles = new Tile[FIELD_HEIGHT, FIELD_WIDTH];

            //Заполняем поле.
            for(int y = 0; y < FIELD_HEIGHT; y++) {
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
        }

        #endregion

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

                //Проверяем возможность хода
                if (!IsCheckerAbleToMove(checker, row, column)) return;

                //Так как ход уже совешен, мы можем установить текущей клетке поле ContainsChecker = false
                //TODO возможно понадобится tiles.[row, column].Checker = null;
                tiles[checker.Row, checker.Column].IsContainsChecker = false;
                tiles[checker.Row, checker.Column].Checker = null;


                //Устанавливаем шашке, новое положение на поле.
                checker.Column = column;
                checker.Row = row;

                //Устанавливаем в клетку, в которую совершается ход нашу шашку.
                tiles[row, column].Checker = (Checker)checker.Clone();

                //После хода автоматически выбираем нашу шашку
                selectedChecker = tiles[row, column].Checker;

                //Пробрасываем событие изменения поля
                FieldUpdated?.Invoke();
            }
        }
        #endregion

        #region Movement ables algoritm

        /*TODO
         * Алгоритм проверки возможность хода
         * Принимаем шашку, возвращаем возможность хода
         * Необходимо проверять ход на срубы, + возможности дамки.
         */
        private bool IsCheckerAbleToMove(Checker checker, int row, int column) {

            /*
             * Проверяем, чтобы координаты нашего кода не выходили за пределы поля.
             * Если она выходит за пределы, то нет смысла выполнять метод.
            */
            if (row > FIELD_WIDTH |
                row < MIN_CORD_VALUE |
                column < MIN_CORD_VALUE |
                column > FIELD_HEIGHT)
                return false;

            /* 
             * Если клетка, в которую совершается ход уже занята возвращаем false. 
             * Можно не производить дальнейшие вычисления.
             */
            if (tiles[row, column].IsContainsChecker)
                return false;

            //Проверяем, чтобы при обычном ходе была возможность сходить только на 1 по диагонали.
            if (Math.Abs(checker.Column - column) == 1 &&
                Math.Abs(checker.Row - row) == 1)  
                return true;

            //Цикл пригодится при реализации атак.
            for (int y = 0; y < FIELD_HEIGHT; y++) {
                for(int x = 0; x < FIELD_WIDTH; x++) {

                }
            }
            return false;
        }
        #endregion

    }
}
