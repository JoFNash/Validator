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
        private List<string> _adrasses;
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

        private void SplitedProperties()
        {
            _telephoneNumbers = TelephoneNumberProperty.Split(", ").ToList();
            _emails = EmailProperty.Split(", ").ToList();
            _adrasses = AdressProperty.Split(", ").ToList();
            
        }

        // private DataRequest GetDataRequest()
        // {
        //     var emailsList = new List<string>();
        //     foreach (var i in _emails)
        //         emailsList.Add(i);
        //     
        //     var adrassList = new List<string>();
        //     foreach (var i in _adrasses)
        //         adrassList.Add(i);
        //     
        //     var numbersList = new List<string>();
        //     foreach (var i in _telephoneNumbers)
        //         numbersList.Add(i);
        //
        //     var time = DateTime.Parse(DateBirthdayProperty).ToTimestamp();
        //     
        //     var input = new DataRequest { Fullname = new Fullname{Name = NameProperty, 
        //         Surname = SurenameProperty, Patronymic = PatronymicProperty}, Passport = PassportNumberProperty, BirthDate = time};
        //     
        //     foreach (var i in adrassList)
        //         input.Addresses.Add(i);
        //     
        //     foreach (var i in emailsList)
        //         input.Emails.Add(i);
        //     
        //     foreach (var i in numbersList)
        //         input.TelephoneNumbers.Add(i);
        //
        //     return input;
        // }

        // void PrintReply(ValidateString reply, string name)
        // {
        //     Console.WriteLine();
        //     Console.WriteLine($"{name}:");
        //     Console.Write("Value = ");
        //     Console.WriteLine(reply.Value);
        //     Console.Write("Is valid: ");
        //     Console.WriteLine(reply.IsValid);
        //     if (reply.HasComment)
        //     {
        //         Console.Write("Comment: ");
        //         Console.WriteLine(reply.Comment);
        //     }
        // }
        
        private async void StartValidatorButtonMethod(object o)
        {
            ValidatorStartedProperty = false;
            _listObjects = new List<string> {NameProperty, SurenameProperty,
                PatronymicProperty, TelephoneNumberProperty, EmailProperty, 
                AdressProperty, PassportNumberProperty, DateBirthdayProperty };

            //SplitedProperties();
            // var input = GetDataRequest();
            //
            // var httpHandler = new HttpClientHandler();
            // httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //
            // var channel = GrpcChannel.ForAddress("https://localhost:7298", new GrpcChannelOptions { HttpHandler = httpHandler });
            // var client = new Validate.ValidateClient(channel);
            //
            // var reply = await client.ValidateAsync(input); /* await */
            // PrintReply(reply.Fullname.Name, "Name");
            // PrintReply(reply.Fullname.Surname, "Surname");
            // PrintReply(reply.Fullname.Patronymic, "Patronymic");
            //
            // foreach (var email in reply.Addresses)
            // {
            //     PrintReply(email, "Email");
            // }

        }
    }
}