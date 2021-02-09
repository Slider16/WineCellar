using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using WineCellar.Blazor.UI.Components.Modal;

namespace WineCellar.Blazor.UI.Services
{
    public interface IModalService
    {
        event Action<ModalResult> OnClose;
        void Show<T>(string title, ModalParameters parameters) where T : ComponentBase;
        void Show<T>(string title, ModalParameters parameters = null, ModalOptions options = null) where T : ComponentBase;
        void Show(Type contentComponent, string title, ModalParameters parameters, ModalOptions options);
        void Close(ModalResult modalResult);
        void Cancel();
    }
}
