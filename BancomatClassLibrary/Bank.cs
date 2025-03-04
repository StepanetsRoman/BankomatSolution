using System;
using System.Collections.Generic;
using System.Linq;

namespace BancomatClassLibrary
{
    public class Bank
    {
        private static readonly Random Random = new Random();
        public string BankName { get; }
        public List<AutomatedTellerMachine> AtmList { get; }
        public List<Account> Accounts { get; }

        public event EventHandler<MessageEventArgs> Message;

        public Bank(string name)
        {
            BankName = name ?? throw new ArgumentNullException(nameof(name), "Назва банку не може бути пустою");
            AtmList = new List<AutomatedTellerMachine>();
            Accounts = new List<Account>();
        }

        public void AddAtm(int bankId, string address, double balance)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                Message?.Invoke(this, new MessageEventArgs("Адреса банкомата не може бути пустою"));
                return;
            }

            var atm = new AutomatedTellerMachine(bankId, address, balance);
            atm.Message += Message;
            AtmList.Add(atm);
        }

        public bool CreateAccount(string name, string pinCode)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Notify("Ім'я не може бути пустим");
                return false;
            }
            if (pinCode.Length != 4 || !pinCode.All(char.IsDigit))
            {
                Notify("Пін-код повинен містити 4 цифри");
                return false;
            }

            var cardNumber = GenerateUniqueCardNumber();
            var account = new Account(name, cardNumber, pinCode);
            account.Message += Message;
            Accounts.Add(account);

            Notify($"Аккаунт створено для {name}. Номер рахунку: {cardNumber}");
            return true;
        }

        public bool Authenticate(string cardNumber, string pinCode)
        {
            var account = Accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
            if (account == null)
            {
                Notify("Картка з таким номером відсутня");
                return false;
            }
            if (pinCode != account.PinCode)
            {
                Notify("Невірний пін-код");
                return false;
            }
            Notify("Аутентифікація успішна");
            return true;
        }

        public void TransferFunds(string fromCardNumber, string toCardNumber, double amount)
        {
            var fromAccount = Accounts.FirstOrDefault(a => a.CardNumber == fromCardNumber);
            var toAccount = Accounts.FirstOrDefault(a => a.CardNumber == toCardNumber);

            if (fromAccount == null || toAccount == null)
            {
                Notify("Один з рахунків не знайдено");
                return;
            }
            if (fromAccount == toAccount)
            {
                Notify("Неможливо здійснити переказ на власний рахунок");
                return;
            }

            if (fromAccount.Withdraw(amount))
            {
                toAccount.AddToBalance(amount);
                Notify($"Переказ {amount} грн від {fromAccount.Name} до {toAccount.Name} здійснено успішно");
            }
            else
            {
                Notify("Недостатньо коштів для переказу");
            }
        }

        public Account FindAccount(string cardNumber)
        {
            var account = Accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
            if (account == null)
            {
                Notify("Картку з таким номером не знайдено");
            }
            return account;
        }

        private string GenerateUniqueCardNumber()
        {
            string cardNumber;
            do
            {
                cardNumber = string.Concat(Enumerable.Range(0, 6).Select(_ => Random.Next(0, 10).ToString()));
            } while (Accounts.Any(a => a.CardNumber == cardNumber));

            return cardNumber;
        }

        public string GetAtmAddress(int index)
        {
            if (index < 0 || index >= AtmList.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Неправильний індекс банкомата");
            }

            return "вул." + AtmList[index].GetBankomatAddress();
        }

        private void Notify(string message)
        {
            Message?.Invoke(this, new MessageEventArgs(message));
        }
    }
}
