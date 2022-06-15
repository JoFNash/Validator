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

        private void WriteAboutMistakes(DataReplyWPF reply)
        {
            bool myBool = true;
            /* ФИО */
            if (!reply.Fullname.Surname.IsValid)
            {
                MessageBox.Show(reply.Fullname.Surname.Comment);
                myBool = false;
            }

            if (!reply.Fullname.Name.IsValid)
            {
                MessageBox.Show(reply.Fullname.Name.Comment);
                myBool = false;
            }
            
            if (!reply.Fullname.Patronymic.IsValid)
            {
                MessageBox.Show(reply.Fullname.Patronymic.Comment);
                myBool = false;
            }
            
            for (int i = 0; i < reply.PhoneNumbers.Count; i++)
            {
                if (!reply.PhoneNumbers[i].IsValid)
                {
                    MessageBox.Show($"Телефон {reply.PhoneNumbers[i].Value} введен некорректно :(");
                    myBool = false;
                }
            }
            
            for (int i = 0; i < reply.Emails.Count; i++)
            {
                if (!reply.Emails[i].IsValid)
                {
                    MessageBox.Show($"Электронная почта {reply.Emails[i].Value} введена некорректно :(");
                    myBool = false;
                }
            }
            
            for (int i = 0; i < reply.Addresses.Count; i++)
            {
                if (!reply.Addresses[i].IsValid)
                {
                    MessageBox.Show($"Адрес {reply.Addresses[i].Value} введен некорректно :(");
                    myBool = false;
                }
            }

            if (!reply.Passport.IsValid)
            {
                MessageBox.Show(reply.Passport.Comment);
                myBool = false;
            }
            
            if (!reply.BirthDate.IsValid)
            {
                MessageBox.Show(reply.BirthDate.Comment);
                myBool = false;
            }

            if (myBool)
            {
                MessageBox.Show("Все данные введены корректно! " + "Мы Вам обязательно перезвоним :)");
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
                MessageBox.Show("Вы ввели время хреново!");
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