using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurrencyTranslate.Client.Service;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

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

        #endregion

        #region Commands

        /// <summary>
        /// The command to clear the window
        /// </summary>
        public ICommand ResetCommand { get; }

        /// <summary>
        /// The command to convert number to word. 
        /// </summary>
        public ICommand ConvertNumberCommand { get; }

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
        /// Initializes a new instance of the <see cref="TranslatorViewModel"/> class.
        /// </summary>  
        public TranslatorViewModel(TranslateServiceClient client)
        {
            _client = client;
            // init command
            ResetCommand = new RelayCommand(ResetResults);
            ConvertNumberCommand = new AsyncRelayCommand(ConvertNumberAsync);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Convert a number to words.
        /// </summary>
        private async Task ConvertNumberAsync()
        {
            if (GivenNumber == null)
                return;

            if (GivenNumber < 0 || GivenNumber > 999999999.99) // input limit
            {
                ErrorMessage = @"Number can not be negative or greater than 999 999 999,99";
                return;
            }

            try
            {
                NumberInWord = await _client.GetConvertedWordAsync(GivenNumber.Value);
            }
            catch (Exception e)
            {
                ErrorMessage = $"Converting number failed : {e.Message}";
            }
        }

        /// <summary>
        /// On command clear
        /// </summary>
        private void ResetResults()
        {
            // clear input field and error message
            GivenNumber = null;
            OnPropertyChanged(nameof(GivenNumber));
            ErrorMessage = null;
            NumberInWord = null;
        }

        #endregion
    }
}
