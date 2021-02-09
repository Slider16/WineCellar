using System;
using System.Collections.Generic;
using System.Text;

namespace WineCellar.Blazor.UI.Components.Modal
{
    public class ModalOptions
    {
        public string Position { get; set; }
        public string Style { get; set; }
        public bool? DisableBackgroundCancel { get; set; }
        public bool? HideHeader { get; set; }
        public bool? HideCloseButton { get; set; }

    }
}
