using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurrencyTranslate.Client.Service;
using System;
using System.Globalization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CurrencyTranslate.Client.ViewModels
{
    /// <summary>
    /// This class represents the view model of a TranslatorView.
    /// </summary>
    public sealed class TranslatorViewModel : ObservableObject
    {
        #region Fields

        private string _numberInWord;
        private string _errorMessage;
        private readonly TranslateServiceClient _client;
        private CultureInfo _selectedCultureCache;
        private string _givenNumberCache;

        #endregion

        #region Commands

        /// <summary>
        /// The command to clear the window
        /// </summary>
        public RelayCommand ResetCommand { get; }

        /// <summary>
        /// The command to convert number to word. 
        /// </summary>
        public AsyncRelayCommand<string> ConvertNumberCommand { get; }

        /// <summary>
        /// The command to update translate language. 
        /// </summary>
        public AsyncRelayCommand<CultureInfo> UpdateLanguageCommand { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value to translate into word.
        /// </summary>
        public double? GivenNumber { get; set; }

        /// <summary>
        /// Gets the number as verbal represented words
        /// </summary>
        public string NumberInWord
        {
            get => _numberInWord;
            private set => SetProperty(ref _numberInWord, value);
        }

        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TranslatorViewModel class.
        /// </summary>  
        public TranslatorViewModel(TranslateServiceClient client)
        {
            _client = client;
            // init command
            ResetCommand = new RelayCommand(OnResetCommand, CanExecuteReset);
            ConvertNumberCommand = new AsyncRelayCommand<string>(OnConvertNumberCommandAsync);
            UpdateLanguageCommand = new AsyncRelayCommand<CultureInfo>(OnUpdateLanguageCommandAsync);
        }

        #endregion

        #region Helpers

        /// <summary>
        ///  Invoke when UpdateLanguageCommand executes.
        /// </summary>
        private async Task OnUpdateLanguageCommandAsync(CultureInfo cultureInfo)
        {
            try
            {
                await _client.UpdateLanguageAsync(cultureInfo.Name);

                _selectedCultureCache = cultureInfo;

                await ConvertNumberAsync(_givenNumberCache);
            }
            catch (FaultException e)
            {
                ErrorMessage = $"Error occured in Server: {e.Message}";
            }
            catch (Exception e)
            {
                ErrorMessage = $"Unknown Error: {e.Message}";
            }
        }

        /// <summary>
        /// Invoke when ConvertNumberCommand executes.
        /// </summary>
        private async Task OnConvertNumberCommandAsync(string number)
        {
            if (_givenNumberCache == number)
                return;

            await ConvertNumberAsync(number);
        }

        /// <summary>
        /// Invoke when ResetCommand executes.
        /// </summary>
        private void OnResetCommand()
        {
            // clear input field and error message
            ErrorMessage = null;
            NumberInWord = null;
        }

        private bool CanExecuteReset()
        {
            var ok = !(string.IsNullOrEmpty(NumberInWord) && string.IsNullOrEmpty(ErrorMessage));
            return ok;
        }

        /// <summary>
        /// Convert a number to words.
        /// </summary>
        private async Task ConvertNumberAsync(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                OnResetCommand();
                ResetCommand.NotifyCanExecuteChanged();
                return;
            }

            if (!double.TryParse(number, NumberStyles.Float, _selectedCultureCache, out var givenNumber))
            {
                ErrorMessage = @"Please input number only !";
                return;
            }

            if (givenNumber < 0 || givenNumber > 999999999.99) // input limit
            {
                ErrorMessage = @"Number can not be negative or greater than 999 999 999,99";
                return;
            }

            try
            {
                NumberInWord = await _client.GetConvertedWordAsync(givenNumber);
                ErrorMessage = null;
            }
            catch (FaultException e)
            {
                ErrorMessage = $"Error occured in Server: {e.Message}";
            }
            catch (Exception e)
            {
                ErrorMessage = $"Unknown Error: {e.Message}";
            }
            finally
            {
                _givenNumberCache = number;
                ResetCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion
    }
}
