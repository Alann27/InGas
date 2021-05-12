using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InGas.Services
{
    public interface IDialogService
    {
        Task<string> Prompt(string title, string message, string okButton, string cancelButton, string placeholder);
        Task DisplayAlert(string title, string message, string okButton);
        Task DisplayAlert(string title, string message, string okButton, string cancelButton);
    }
}
