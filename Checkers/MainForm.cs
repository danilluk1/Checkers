using Checkers.BL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers {
    public partial class MainForm : Form {
        private Game game;
        private PictureBox[,] field = new PictureBox[Game.FIELD_HEIGHT, Game.FIELD_WIDTH];
        private bool isLeftMousePressed;
        public MainForm() { 
            InitializeComponent();

            #region GameFieldDefaultSetup
            for (int y = 0; y < Game.FIELD_HEIGHT; y++) {
                for(int x = 0; x < Game.FIELD_WIDTH; x++) {
                    field[y, x] = new PictureBox {
                        Width = 100,
                        Height = 100,
                        Left = 0 + x * 100,
                        Top = 2 + y * 100,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = y + "" + x
                    };
                    field[y, x].MouseDown += Tile_Click;
                    field[y, x].MouseDoubleClick += Tile_MouseDoubleClick;
                }
            }
            for(int y = 0; y < Game.FIELD_HEIGHT; y++) {
                for(int x = 0; x < Game.FIELD_WIDTH; x++) {
                    if (y % 2 == 0) {
                        field[y, 0].BackColor = Color.LightPink;
                        field[y, 1].BackColor = Color.Black;
                        field[y, 2].BackColor = Color.LightPink;
                        field[y, 3].BackColor = Color.Black;
                        field[y, 4].BackColor = Color.LightPink;
                        field[y, 5].BackColor = Color.Black;
                        field[y, 6].BackColor = Color.LightPink;
                        field[y, 7].BackColor = Color.Black;
                    }
                    else {
                        field[y, 0].BackColor = Color.Black;
                        field[y, 1].BackColor = Color.LightPink;
                        field[y, 2].BackColor = Color.Black;
                        field[y, 3].BackColor = Color.LightPink;
                        field[y, 4].BackColor = Color.Black;
                        field[y, 5].BackColor = Color.LightPink;
                        field[y, 6].BackColor = Color.Black;
                        field[y, 7].BackColor = Color.LightPink;
                    }
                    Controls.Add(field[y, x]);
                }
            }
            #endregion

            game = new Game(new Team("Белые"), new Team("Черные"));

            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    if (game.Tiles[x, y].IsContainsChecker) {
                        field[x, y].BackgroundImage = game.Tiles[x, y].Checker.Image;
                    }
                }
            }
            game.FieldUpdated += UpdateView;
        }


        private void Tile_MouseDoubleClick(object sender, MouseEventArgs e) {
            string cords = (sender as PictureBox).Tag.ToString();

            int row = Convert.ToInt32(cords[0].ToString());
            int column = Convert.ToInt32(cords[1].ToString());
            game.Move(game.SelectedChecker, row, column);
        }


        private void Tile_Click(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {

                string cords = (sender as PictureBox).Tag.ToString();
                int row = Convert.ToInt32(cords[0].ToString());
                int column = Convert.ToInt32(cords[1].ToString());

                game.SelectedChecker = game.Tiles[row, column].Checker;

                UpdateView();
            }

        }
        private void UpdateView() {
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    if (game.Tiles[x, y].IsContainsChecker) {
                        field[x, y].BackgroundImage = game.Tiles[x, y].Checker.Image;
                        if (y % 2 == 0) {
                            field[y, 0].BackColor = Color.LightPink;
                            field[y, 1].BackColor = Color.Black;
                            field[y, 2].BackColor = Color.LightPink;
                            field[y, 3].BackColor = Color.Black;
                            field[y, 4].BackColor = Color.LightPink;
                            field[y, 5].BackColor = Color.Black;
                            field[y, 6].BackColor = Color.LightPink;
                            field[y, 7].BackColor = Color.Black;
                        }
                        else {
                            field[y, 0].BackColor = Color.Black;
                            field[y, 1].BackColor = Color.LightPink;
                            field[y, 2].BackColor = Color.Black;
                            field[y, 3].BackColor = Color.LightPink;
                            field[y, 4].BackColor = Color.Black;
                            field[y, 5].BackColor = Color.LightPink;
                            field[y, 6].BackColor = Color.Black;
                            field[y, 7].BackColor = Color.LightPink;
                        }
                    }
                    else {
                        field[x, y].BackgroundImage = null;
                    }
                }
            }
            if (game.SelectedChecker != null) {
                if (game.Tiles[game.SelectedChecker.Row, game.SelectedChecker.Column].Checker.Equals(game.SelectedChecker)) {
                    field[game.SelectedChecker.Row, game.SelectedChecker.Column].BackColor = Color.Yellow;
                }
            }
        }
    }
}
