using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurrencyTranslate.Client.Service;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace CurrencyTranslate.Client.ViewModels
{
    /// <summary>
    /// This class represents the view model of a MainWindow.
    /// </summary>
    public class MainWindowViewModel : ObservableObject
    {
        #region Fields

        private readonly TranslateServiceClient _translateClient = new TranslateServiceClient();
        private bool _isBusy = true;

        #endregion

        #region Properties

        /// <summary>
        /// The list of supported language of the application.  
        /// </summary>
        public ObservableCollection<CultureInfo> SupportedLanguages { get; } = new ObservableCollection<CultureInfo>();

        /// <summary>
        /// Returns a value which indicates the server is ready or not.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        /// <summary>
        /// The view model for the tranlator.
        /// </summary>
        public TranslatorViewModel TranslatorViewModel { get; }

        /// <summary>
        /// The command for loading main window.
        /// </summary>
        public AsyncRelayCommand LoadingCommand { get; }  

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of MainWindowViewModel.
        /// </summary>
        public MainWindowViewModel()
        {
            TranslatorViewModel = new TranslatorViewModel(_translateClient);
            LoadingCommand = new AsyncRelayCommand(OnLoadingCommandAsync);
        }

        #endregion

        #region Helpers

        private async Task OnLoadingCommandAsync()
        {
            foreach (var language in await _translateClient.GetSupportedLanguagesAsync())
            {
                SupportedLanguages.Add(new CultureInfo(language));
            }

            var state = await _translateClient.GetStateAsync();

            if (state == State.Ready)
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
