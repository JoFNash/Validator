using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using ClientGUI.MVVM.Core.Command;
using ClientGUI.MVVM.Core.ViewModel;

namespace ClientGUI.MVVM.ViewModel
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        /* Константы программы */
        #region Constants
        
        #endregion
        
        /* Приватные переменные */
        
        #region Variables
        private List<string> _listObjects;
        #endregion

        /* свойтсва */
        #region Properties

        public bool ValidatorStartedProperty { get; set; }

        public string NameProperty { get; set; }
        public string SurenameProperty { get; set; }
        public string PatronymicProperty { get; set; }
        public string TelephoneNumberProperty { get; set; }
        public string EmailProperty { get; set; }
        public string AdressProperty { get; set; }
        public string PassportNumberProperty { get; set; }
        public string DateBirthdayProperty { get; set; }
        
        #endregion

        /* команда для кнопки "запустить валидатор!ё!!" */
        public RelayCommand StartValidationCommand { get; set; }
        
        public MainWindowViewModel()
        {
            ValidatorStartedProperty = true;

            StartValidationCommand = new RelayCommand(StartValidatorButtonMethod, 
                o =>  
                     !string.IsNullOrEmpty(NameProperty) && 
                     !string.IsNullOrEmpty(SurenameProperty) && 
                     !string.IsNullOrEmpty(PatronymicProperty) &&
                     !string.IsNullOrEmpty(TelephoneNumberProperty) && 
                     !string.IsNullOrEmpty(EmailProperty) &&
                     !string.IsNullOrEmpty(AdressProperty) &&
                     !string.IsNullOrEmpty(PassportNumberProperty) &&
                     !string.IsNullOrEmpty(DateBirthdayProperty));
        }

        private void MyMethodForCheck()
        {
            MessageBox.Show(NameProperty.ToString());
        }

        private void StartValidatorButtonMethod(object o)
        {
            ValidatorStartedProperty = false;
            var values = (object[]) o;
            _listObjects = new List<string>(8);
            for (int i = 0; i < 8; i++)
                _listObjects[i] = (string)values[i];
        }
    }
}