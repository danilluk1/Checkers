using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public abstract class Direction {
        public int RowIndex { get; set; }

        public int ColumnIndex { get; set; }

        public bool IsWhiteCheckerDirection { get; set; }

        public List<Tile> GetDiagonal(Checker checker, Board board) {
            List<Tile> tiles = new List<Tile>();
            int j = checker.Column;
            for (int i = checker.Row; i < Board.FIELD_HEIGHT && i >= 0 && j < Board.FIELD_WIDTH && j >= 0; i += RowIndex) {
                tiles.Add(board[i, j]);
                j += ColumnIndex;
            }
            return tiles;
        }

        public class UpperLeft : Direction {
            public UpperLeft() {
                RowIndex = -1;
                ColumnIndex = -1;
                IsWhiteCheckerDirection = true;
            }

        }
        public class UpperRight : Direction {
            public UpperRight() {
                RowIndex = -1;
                ColumnIndex = 1;
                IsWhiteCheckerDirection = true;
            }
        }
        public class DownLeft : Direction {
            public DownLeft() {
                RowIndex = 1;
                ColumnIndex = -1;
                IsWhiteCheckerDirection = false;
            }
        }
        public class DownRight : Direction {
            public DownRight() {
                RowIndex = 1;
                ColumnIndex = 1;
                IsWhiteCheckerDirection = false;
            }
        }
    }
}
