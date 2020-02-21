﻿using System.Drawing;


namespace Checkers.BL.Model {

    public enum CheckerType {
        White,
        Black
    }

    public sealed class Checker {

        #region private variables
        private CheckerType c_type;
        private int row;
        private int column;
        private bool isSelected;
        private readonly Image image;
        #endregion

        #region public properties
        public Image Image { 
            get {
                return image;
            }
        }
        public CheckerType Type { 
            get {
                return c_type;
            }
        }
        public int Row { 
            get {
                return row;
            } 
            set {
                row = value;
            }
        }
        public int Column {
            get {
                return column;
            }
            set {
                column = value;
            }
        }
        #endregion

        #region Class Designer
        public Checker(CheckerType type, int row, int column) {

            c_type = type;
            Row = row;
            Column = column;

            if(type == CheckerType.White) {
                image = Images.whitechecker;
            }

            if(type == CheckerType.Black) {
                image = Images.whitechecker; //Change to black color //TODO
            }
        }
        #endregion

        public override bool Equals(object obj) {
            return ((Checker)obj).column == this.column && ((Checker)obj).row == this.row;
        }

    }
}
