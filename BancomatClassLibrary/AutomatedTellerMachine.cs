using System;

namespace BancomatClassLibrary
{
    public class AutomatedTellerMachine
    {
        public int BankId { get; private set; }
        public string BankomatAddress { get; private set; }
        public double BankomatBalance { get; private set; }
        public bool IsOperational { get; private set; } = true;
        public event EventHandler<MessageEventArgs> Message;

        public AutomatedTellerMachine(int bankId, string bankomatAddress, double bankomatBalance)
        {
            if (bankomatBalance < 0)
            {
                throw new ArgumentException("Баланс банкомата не може бути від’ємним");
            }

            BankId = bankId;
            BankomatAddress = bankomatAddress ?? throw new ArgumentNullException(nameof(bankomatAddress), "Адреса банкомата не може бути пустою");
            BankomatBalance = bankomatBalance;
        }

        public bool WithDrawMoney(Account account, double moneyToGet)
        {
            if (!IsOperational)
            {
                Notify("Банкомат тимчасово не працює. Зверніться до іншого пристрою.");
                return false;
            }

            if (moneyToGet <= 0)
            {
                Notify("Сума для зняття повинна бути більше нуля.");
                return false;
            }

            if (BankomatBalance >= moneyToGet)
            {
                if (account.Withdraw(moneyToGet))
                {
                    BankomatBalance -= moneyToGet;
                    Notify($"Операція успішна. Ви зняли {moneyToGet} грн.");
                    return true;
                }
                Notify("Недостатньо коштів на рахунку.");
                return false;
            }

            Notify($"Недостатньо коштів у банкоматі. Максимальна доступна сума: {BankomatBalance} грн.");
            return false;
        }

        public bool PutMoney(Account account, double moneyToPut)
        {
            if (!IsOperational)
            {
                Notify("Банкомат тимчасово не працює. Зверніться до іншого пристрою.");
                return false;
            }

            if (moneyToPut <= 0)
            {
                Notify("Сума для поповнення повинна бути більше нуля.");
                return false;
            }

            if (account.AddToBalance(moneyToPut))
            {
                BankomatBalance += moneyToPut;
                Notify($"Операція успішна. Ви поповнили рахунок на {moneyToPut} грн.");
                return true;
            }

            Notify("Не вдалося поповнити рахунок.");
            return false;
        }

        public string GetBankomatAddress()
        {
            return BankomatAddress;
        }

        private void Notify(string message)
        {
            Message?.Invoke(this, new MessageEventArgs(message));
        }
    }
}
