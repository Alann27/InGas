using InGas.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace InGas.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        public MasterViewModel(INavigationService navigationService, DialogService dialogService, IDatabaseService databaseService) : base(navigationService, dialogService, databaseService)
        {
            SelectedPageCommand = new DelegateCommand<Page>(OnSelectedPage);
        }

        public List<Page> Pages { get; } = NavigationConstants.Pages;
        private Page _selectedPage = NavigationConstants.Pages.FirstOrDefault();

        public DelegateCommand<Page> SelectedPageCommand { get; set; }

        public async void OnSelectedPage(Page page)
        {
            if (_selectedPage.NavigationUri != page.NavigationUri)
            {
                _selectedPage = page;
                await NavigationService.NavigateAsync($"{NavigationConstants.NavigationPage}/{page.NavigationUri}");
            }
            else
            {
                await DialogService.DisplayAlert(StringConstants.Error, $"{page.Name} is already on screen", StringConstants.Ok);
            }
        }
    }
}
