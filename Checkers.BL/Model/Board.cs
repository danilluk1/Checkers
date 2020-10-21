using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public sealed class Board {
        private Game game;
        public const int FIELD_WIDTH = 8;
        public const int FIELD_HEIGHT = 8;


        public Tile[,] Tiles { get; set; }

        public Board(Game game) {
            this.game = game;
            //Инициализация поля.
            Tiles = new Tile[FIELD_HEIGHT, FIELD_WIDTH];
            Init();
            
        }

        public Tile this[int row, int col] {
            get {
                return Tiles[row, col];
            }

            set {
                Tiles[row, col] = value;
            }
        }

        private void Init() {
            //Заполняем поле.
            for (int y = 0; y < FIELD_HEIGHT; y++) {
                for (int x = 0; x < FIELD_WIDTH; x++) {
                    Tiles[x, y] = new Tile(x, y);
                }
            }


            //Инициализирем черных
            Tiles[0, 1].Checker = new BlackChecker(0, 1, game);
            Tiles[0, 3].Checker = new BlackChecker(0, 3, game);
            Tiles[0, 5].Checker = new BlackChecker(0, 5, game);
            Tiles[0, 7].Checker = new BlackChecker(0, 7, game);
            Tiles[1, 0].Checker = new BlackChecker(1, 0, game);
            Tiles[1, 2].Checker = new BlackChecker(1, 2, game);
            Tiles[1, 4].Checker = new BlackChecker(1, 4, game);
            Tiles[1, 6].Checker = new BlackChecker(1, 6, game);
            Tiles[2, 1].Checker = new BlackChecker(2, 1, game);
            Tiles[2, 3].Checker = new BlackChecker(2, 3, game);
            Tiles[2, 5].Checker = new BlackChecker(2, 5, game);
            Tiles[2, 7].Checker = new BlackChecker(2, 7, game);

            //Инициализирем белых
            Tiles[5, 0].Checker = new WhiteChecker(5, 0, game);
            Tiles[5, 2].Checker = new WhiteChecker(5, 2, game);
            Tiles[5, 4].Checker = new WhiteChecker(5, 4, game);
            Tiles[5, 6].Checker = new WhiteChecker(5, 6, game);
            Tiles[6, 1].Checker = new WhiteChecker(6, 1, game);
            Tiles[6, 3].Checker = new WhiteChecker(6, 3, game);
            Tiles[6, 5].Checker = new WhiteChecker(6, 5, game);
            Tiles[6, 7].Checker = new WhiteChecker(6, 7, game);
            Tiles[7, 0].Checker = new WhiteChecker(7, 0, game);
            Tiles[7, 2].Checker = new WhiteChecker(7, 2, game);
            Tiles[7, 4].Checker = new WhiteChecker(7, 4, game);
            Tiles[7, 6].Checker = new WhiteChecker(7, 6, game);

            for (int y = 0; y < FIELD_HEIGHT; y++) {
                for (int x = 0; x < FIELD_WIDTH; x++) {
                    if (Tiles[x, y].IsContainsChecker) {
                        game.AddChecker(Tiles[x, y].Checker);
                    }
                }
            }
        }
    }
}
