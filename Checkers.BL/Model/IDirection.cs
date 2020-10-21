using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.BL.Model {
    public interface IDirection {
        int RowIndex {get; set;}
        int ColumnIndex { get; set; }
    }
}
