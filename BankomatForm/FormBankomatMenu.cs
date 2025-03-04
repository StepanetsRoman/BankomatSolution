using System;
using BancomatClassLibrary;
using System.Windows.Forms;

namespace BankomatForm
{
    public partial class FormBankomatMenu : Form
    {
        private Bank currentBank;
        private Account currentAccount;
        private AutomatedTellerMachine activeBankomat;
        private bool amountEnter = false;
        public enum OperationType
        {
            None,
            Deposit,
            Withdraw,
            Transfer
        }
        private OperationType currentOperation = OperationType.None;

        public FormBankomatMenu(Bank bank, Account account, AutomatedTellerMachine bankomat)
        {
            InitializeComponent();
            HidePanel();
            panelStart.Visible = true;
            activeBankomat = bankomat;
            currentBank = bank;
            currentAccount = account;
        }

        void HidePanel()
        {
            panelStart.Visible = false;
            panelEnterAmount.Visible = false;
            panelEnterCardNumber.Visible = false;
            panelShowBalance.Visible = false;
            panelPutMoney.Visible = false;
            panelWithDraw.Visible = false;
            labelAmountEnter.Text = "";
            labelCardEnter.Text = "";
        }
        private void SwitchPanel(Panel panelToShow)
        {
            HidePanel();
            panelToShow.Visible = true;
        }
        private void btnNum1_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 1;
            }
            else
            {
                labelCardEnter.Text += 1;
            }
        }
        private void btnNum2_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 2;
            }
            else
            {
                labelCardEnter.Text += 2;
            }
        }
        private void btnNum3_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 3;
            }
            else
            {
                labelCardEnter.Text += 3;
            }
        }
        private void btnNum4_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 4;
            }
            else
            {
                labelCardEnter.Text += 4;
            }
        }
        private void btnNum5_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 5;
            }
            else
            {
                labelCardEnter.Text += 5;
            }
        }
        private void btnNum6_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 6;
            }
            else
            {
                labelCardEnter.Text += 6;
            }
        }
        private void btnNum7_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 7;
            }
            else
            {
                labelCardEnter.Text += 7;
            }
        }
        private void btnNum8_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 8;
            }
            else
            {
                labelCardEnter.Text += 8;
            }
        }
        private void btnNum9_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 9;
            }
            else
            {
                labelCardEnter.Text += 9;
            }
        }
        private void btnNum0_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text += 0;
            }
            else
            {
                labelCardEnter.Text += 0;
            }
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            if (amountEnter)
            {
                labelAmountEnter.Text = "";
            }
            else
            {
                labelCardEnter.Text = "";
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            HidePanel();
            panelShowBalance.Visible = true;
            labelShowBalance.Text = currentAccount.CardBalance.ToString("F2") + " грн";
        }

        private void btnPutMoney_Click(object sender, EventArgs e)
        {
            SwitchPanel(panelEnterAmount);
            currentOperation = OperationType.Deposit;
            amountEnter = true;
        }

        private void btnWithDraw_Click(object sender, EventArgs e)
        {
            SwitchPanel(panelEnterAmount);
            currentOperation = OperationType.Withdraw;
            amountEnter = true;
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            SwitchPanel(panelEnterCardNumber);
            currentOperation = OperationType.Transfer;
            amountEnter = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            double amount;
            if (double.TryParse(labelAmountEnter.Text, out amount) && amount > 0)
            {
                switch (currentOperation)
                {
                    case OperationType.Deposit:
                        HandleDeposit(amount);
                        break;
                    case OperationType.Withdraw:
                        HandleWithdraw(amount);
                        break;
                    case OperationType.Transfer:
                        HandleTransfer(amount);
                        break;
                    default:
                        ShowErrorMessage("Некоректна операція.");
                        break;
                }
            }
            else
            {
                ShowErrorMessage("Некоректна сума.");
            }
        }

        private void HandleDeposit(double amount)
        {
            activeBankomat.PutMoney(currentAccount, amount);
            SwitchPanel(panelPutMoney);
            labelPutMoney.Text = labelAmountEnter.Text + " грн";
        }

        private void HandleWithdraw(double amount)
        {
            if (activeBankomat.WithDrawMoney(currentAccount, amount))
            {
                SwitchPanel(panelWithDraw);
            }
        }

        private void HandleTransfer(double amount)
        {
            panelEnterCardNumber.Visible = false;
            panelEnterAmount.Visible = true;
            amountEnter = true;

            currentBank.TransferFunds(currentAccount.CardNumber, labelCardEnter.Text, amount);
            SwitchPanel(panelStart);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
