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
        public const int DEFAULT_STEP = 1;
        public const int ATTACK_DEFAULT_STEP = 2;

        #endregion constansts

        #region private variables

        private Team firstTeam;
        private Team secondTeam;

        private Tile[,] tiles;

        private Checker selectedChecker;

        #endregion private variables

        #region public properties
        public bool IsWhiteMove { get; set; } = true;
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
            tiles[0, 1].Checker = new Checker(CheckerColor.Black, 0, 1, Tiles, BlackCheckers);
            tiles[0, 3].Checker = new Checker(CheckerColor.Black, 0, 3, Tiles, BlackCheckers);
            tiles[0, 5].Checker = new Checker(CheckerColor.Black, 0, 5, Tiles, BlackCheckers);
            tiles[0, 7].Checker = new Checker(CheckerColor.Black, 0, 7, Tiles, BlackCheckers);
            tiles[1, 0].Checker = new Checker(CheckerColor.Black, 1, 0, Tiles, BlackCheckers);
            tiles[1, 2].Checker = new Checker(CheckerColor.Black, 1, 2, Tiles, BlackCheckers);
            tiles[1, 4].Checker = new Checker(CheckerColor.Black, 1, 4, Tiles, BlackCheckers);
            tiles[1, 6].Checker = new Checker(CheckerColor.Black, 1, 6, Tiles, BlackCheckers);
            tiles[2, 1].Checker = new Checker(CheckerColor.Black, 2, 1, Tiles, BlackCheckers);
            tiles[2, 3].Checker = new Checker(CheckerColor.Black, 2, 3, Tiles, BlackCheckers);
            tiles[2, 5].Checker = new Checker(CheckerColor.Black, 2, 5, Tiles, BlackCheckers);
            tiles[2, 7].Checker = new Checker(CheckerColor.Black, 2, 7, Tiles, BlackCheckers);

            //Инициализирем белых
            tiles[5, 0].Checker = new Checker(CheckerColor.White, 5, 0, Tiles, WhiteCheckers);
            tiles[5, 2].Checker = new Checker(CheckerColor.White, 5, 2, Tiles, WhiteCheckers);
            tiles[5, 4].Checker = new Checker(CheckerColor.White, 5, 4, Tiles, WhiteCheckers);
            tiles[5, 6].Checker = new Checker(CheckerColor.White, 5, 6, Tiles, WhiteCheckers);
            tiles[6, 1].Checker = new Checker(CheckerColor.White, 6, 1, Tiles, WhiteCheckers);
            tiles[6, 3].Checker = new Checker(CheckerColor.White, 6, 3, Tiles, WhiteCheckers);
            tiles[6, 5].Checker = new Checker(CheckerColor.White, 6, 5, Tiles, WhiteCheckers);
            tiles[6, 7].Checker = new Checker(CheckerColor.White, 6, 7, Tiles, WhiteCheckers);
            tiles[7, 0].Checker = new Checker(CheckerColor.White, 7, 0, Tiles, WhiteCheckers);
            tiles[7, 2].Checker = new Checker(CheckerColor.White, 7, 2, Tiles, WhiteCheckers);
            tiles[7, 4].Checker = new Checker(CheckerColor.White, 7, 4, Tiles, WhiteCheckers);
            tiles[7, 6].Checker = new Checker(CheckerColor.White, 7, 6, Tiles, WhiteCheckers);

            for (int y = 0; y < FIELD_HEIGHT; y++) {
                for (int x = 0; x < FIELD_WIDTH; x++) {
                    if (tiles[x, y].IsContainsChecker) {
                        if (tiles[x, y].Checker.Color == CheckerColor.White) {
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


        /*
         * Алгоритм хода фигуры.
         * Принимаем шашку, которой необходимо сходить, и координаты клетки хода.
         * row < 8 и column < 8 (размеры поля)
         * Наше поле знает, как обновлять View при изменении модели поля(Active View)
         */

        public void Move(Checker checker, int row, int column) {
            var result = SelectedChecker.TryToMove(row, column);
            if (result) {

                SelectedChecker = (Checker)Tiles[row, column].Checker.Clone();
                FieldUpdated?.Invoke();
            }
        }
    }
}