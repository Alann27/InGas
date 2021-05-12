using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Prism.Services;

namespace InGas.Services
{
    public class DialogService : IDialogService
    {
        public DialogService(IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
        }
        private IPageDialogService _pageDialogService;
        public async Task DisplayAlert(string title, string message, string okButton)
        {
            await _pageDialogService.DisplayAlertAsync(title, message, okButton);
        }

        public async Task DisplayAlert(string title, string message, string okButton, string cancelButton)
        {
            await _pageDialogService.DisplayAlertAsync(title, message, okButton, cancelButton);
        }

        public async Task<string> Prompt(string title, string message, string okButton, string cancelButton, string placeholder)
        {
            var result = await UserDialogs.Instance.PromptAsync(message, title, okButton, cancelButton, placeholder, InputType.Name);

            return result.Value;
        }
    }
}
