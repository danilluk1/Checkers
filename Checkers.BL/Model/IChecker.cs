using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    interface IChecker {
        //Содержит поле, на котором находится шашка.
        Tile[,] Field { get; }

        //Содержит картинку шашки.
        Image Image { get; set; }

        //Содержит цвет шашки
        CheckerColor Color { get; set; }

        //Содержит колонку шашки
        int Column { get; set; }

        //Содержит ряд шашки
        int Row { get; set; }

        //Содержит список со всеми шашками своей команды
        IEnumerable<Checker> Checkers { get; }

        bool IsCheckerAbleToMove(int row, int column, MovesDirection direction);
        MovesDirection CountMoveDirection(int row, int column);

        MovesType CountMoveType(int row, int column);

        MovesType TryToMove(int row, int column);

        bool CanEat();


    }
}
