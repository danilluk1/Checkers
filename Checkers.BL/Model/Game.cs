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


        #endregion private variables

        #region public properties

        public Board Board { get; set; }
        public CheckerColor CurrentStepColor { get; set; } = CheckerColor.White;

        public Checker SelectedChecker { get; set; }
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
            Board = new Board(this);          
        }

        #endregion Class designer


        /*
         * Алгоритм хода фигуры.
         * Принимаем шашку, которой необходимо сходить, и координаты клетки хода.
         * row < 8 и column < 8 (размеры поля)
         * Наше поле знает, как обновлять View при изменении модели поля(Active View)
         */

        public void Move(Checker checker, int row, int column) {
            if (checker == null) return;
            if (checker.Color == CurrentStepColor) {
                var result = SelectedChecker.TryToMove(row, column);

                if (result == MovesType.Attack) {
                    if (!checker.CanEat().res) {
                        CurrentStepColor = checker.Color == CheckerColor.White ? CheckerColor.Black : CheckerColor.White;
                    }
                    else {
                        CurrentStepColor = checker.Color;
                    }
                }
                else if (result == MovesType.Default) {
                    CurrentStepColor = checker.Color == CheckerColor.White ? CheckerColor.Black : CheckerColor.White;
                }
                if (result != MovesType.None) {
                    SelectedChecker = Board.Tiles[row, column].Checker;
                    FieldUpdated?.Invoke();
                }
            }
        }

        public void AddChecker(Checker checker) {
            switch (checker.Color) {
                case CheckerColor.Black:
                    BlackCheckers.Add(checker);
                break;
                case CheckerColor.White:
                    WhiteCheckers.Add(checker);
                break;
            }
        }

        
    }
}