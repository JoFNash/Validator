using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Documents;
using ClientGUI.MVVM.Core.Command;
using ClientGUI.MVVM.Core.ViewModel;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using ClientGRPC;
using Server;
using StructsWPF;

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
        private List<string> _telephoneNumbers;
        private List<string> _emails;
        private List<string> _addresses;
        private ClientGRPC.Client _client;
        #endregion

        /* свойтсва */
        #region Properties

        public bool ValidatorStartedProperty { get; set; }

        public string NameProperty { get; set; }
        public string SurnameProperty { get; set; }
        public string PatronymicProperty { get; set; }
        public string TelephoneNumberProperty { get; set; }
        public string EmailProperty { get; set; }
        public string AdressProperty { get; set; }
        public string PassportNumberProperty { get; set; }
        public string DateBirthdayProperty { get; set; }
        
        #endregion

        /* команда для кнопки "запустить валидатор!!!!!!111!111111111111!!11ё!!" */
        public RelayCommand StartValidationCommand { get; set; }
        
        public MainWindowViewModel()
        {
            _client = new Client("https://localhost:7298");
            
            ValidatorStartedProperty = true;

            StartValidationCommand = new RelayCommand(StartValidatorButtonMethod, 
                o =>  
                     !string.IsNullOrEmpty(NameProperty) && 
                     !string.IsNullOrEmpty(SurnameProperty) && 
                     !string.IsNullOrEmpty(PatronymicProperty) &&
                     !string.IsNullOrEmpty(TelephoneNumberProperty) && 
                     !string.IsNullOrEmpty(EmailProperty) &&
                     !string.IsNullOrEmpty(AdressProperty) &&
                     !string.IsNullOrEmpty(PassportNumberProperty) &&
                     !string.IsNullOrEmpty(DateBirthdayProperty));
        }

        private void SplitProperties()
        {
            _telephoneNumbers = TelephoneNumberProperty.Split(", ").ToList();
            _emails = EmailProperty.Split(", ").ToList();
            _addresses = AdressProperty.Split(", ").ToList();
            
        }

        private void ShowMessageBox(string message, string header)
        {
            MessageBox.Show(message, header, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void WriteAboutMistakes(DataReplyWPF reply)
        {
            bool myBool = true;
            /* ФИО */
            if (!reply.Fullname.Surname.IsValid)
            {
                ShowMessageBox(reply.Fullname.Surname.Comment, "Ошибка в фамилии");
                myBool = false;
            }

            if (!reply.Fullname.Name.IsValid)
            {
                ShowMessageBox(reply.Fullname.Name.Comment, "Ошибка в имени");
                myBool = false;
            }
            
            if (!reply.Fullname.Patronymic.IsValid)
            {
                ShowMessageBox(reply.Fullname.Patronymic.Comment, "Ошибка в отчестве");
                myBool = false;
            }
            
            for (int i = 0; i < reply.PhoneNumbers.Count; i++)
            {
                if (!reply.PhoneNumbers[i].IsValid)
                {
                    ShowMessageBox($"Телефон {reply.PhoneNumbers[i].Value} введен некорректно :( \n" + 
                                   reply.PhoneNumbers[i].Comment, 
                        "Ошибка в графе \"Номер(-а) телефона(-ов)\" ");
                    myBool = false;
                }
            }
            
            for (int i = 0; i < reply.Emails.Count; i++)
            {
                if (!reply.Emails[i].IsValid)
                {
                    ShowMessageBox($"Электронная почта {reply.Emails[i].Value} введена некорректно :( \n" + 
                                   reply.Emails[i].Comment, 
                        "Ошибка в графе \"Электронная(-ые) почта(-ы)\" ");
                    myBool = false;
                }
            }
            
            for (int i = 0; i < reply.Addresses.Count; i++)
            {
                if (!reply.Addresses[i].IsValid)
                {
                    ShowMessageBox($"Адрес {reply.Addresses[i].Value} введен некорректно :(", "Ошибка в графе \"Адрес(-а)\" ");
                    myBool = false;
                }
            }

            if (!reply.Passport.IsValid)
            {
                ShowMessageBox(reply.Passport.Comment, "Ошибка в графе \"Номер паспорта\"");
                myBool = false;
            }
            
            if (!reply.BirthDate.IsValid)
            {
                ShowMessageBox(reply.BirthDate.Comment, "Ошибка в графе \"Дата рождения\"");
                myBool = false;
            }

            if (myBool)
            {
                MessageBox.Show(  "Мы Вам обязательно перезвоним :)", "Все данные введены корректно! ", MessageBoxButton.YesNo);
                Application.Current.Shutdown();
            }
        }

        private async void StartValidatorButtonMethod(object o)
        {
            ValidatorStartedProperty = false;
            _listObjects = new List<string> {NameProperty, SurnameProperty,
                PatronymicProperty, TelephoneNumberProperty, EmailProperty, 
                AdressProperty, PassportNumberProperty, DateBirthdayProperty };
            var request = new DataRequestWPF();
            request.Fullname.Name = NameProperty;
            request.Fullname.Surname = SurnameProperty;
            request.Fullname.Patronymic = PatronymicProperty;
            request.Passport = PassportNumberProperty;
            DateTime time;
            try
            {
                time = DateTime.Parse(DateBirthdayProperty);
            }
            catch
            {
                MessageBox.Show("Вы ввели время хреново!", "Ошибка в графе \" Дата рождения\"", MessageBoxButton.OK, MessageBoxImage.Error);
                time = DateTime.UnixEpoch;
            }
            request.BirthDate = DateTime.SpecifyKind(time, DateTimeKind.Utc)
                .ToTimestamp();
            SplitProperties();
            request.PhoneNumbers = _telephoneNumbers;
            request.Emails = _emails;
            request.Addresses = _addresses;
            
            var reply = _client.Validate(request);

            WriteAboutMistakes(reply);

        }
    }
}