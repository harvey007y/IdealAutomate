using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreatePopupGuide {
  public class ComboBoxPair {
    public string _Key { get; set; }
    public string _Value { get; set; }

    public ComboBoxPair(string _key, string _value) {
      _Key = _key;
      _Value = _value;
    }
  }
}
