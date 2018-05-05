using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdealAutomate.Core {
  public class OpenedFile {
    public string _Tab { get; set; }
    public string _FileName { get; set; }
        public string _Process { get; set; }

        public OpenedFile(string _tab, string _fileName, string _process) {
      _Tab = _tab;
      _FileName = _fileName;
      _Process = _process;
    }
  }
}
