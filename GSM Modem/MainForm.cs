using GsmComm;
using GsmComm.PduConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_Modem
{
    public partial class MainForm : Form
    {
        private bool portConnected = false;
        private SerialPort port;
        private string portName;
        private string response;
        private Thread errorThread;
        private AutoResetEvent receiveNow;

        public MainForm()
        {
            InitializeComponent();

            portName = string.Empty;
            port = null;
        }


        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var ports = SerialPort.GetPortNames();
            comboBoxComPort.DataSource = ports;
            this.portName = this.comboBoxComPort.Text;
            programStatus.Text = "Status: not connected to a COM Port";
            portConnected = false;
            checkBoxAutoRefresh.Enabled = false;
            btnListSMS.Enabled = false;
        }
        private void btnRefreshports_Click(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            comboBoxComPort.DataSource = ports;
            this.portName = this.comboBoxComPort.Text;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")
            {
                OpenPortConnection(this.portName);
            }
            else if (btnConnect.Text == "Disconnect")
            {
                ClosePortConnection();
                btnConnect.Text = "Connect";
            }
        }
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                {
                    receiveNow.Set();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnListSMS_Click(object sender, EventArgs e)
        {
            this.dataGridViewSMS.Rows.Clear();
            this.dataGridViewSMS.Refresh();
            string result = ExecCommand("at+cmgl=4");
            List<ConfirmationMessage> confirmationMessages = ParseConfirmationMessage(result);
            foreach (var item in confirmationMessages)
            {
                string[] row = { item.GetID().ToString(), item.GetTelNr(), item.GetStatus(), item.GetTimeStamp().ToString(), item.GetDischarge().ToString(), item.GetMessage() };
                this.dataGridViewSMS.Rows.Add(row);
            }
        }
        private void checkBoxAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            Thread autoRefresh = new Thread(AutoRefresh);
            if ((sender as CheckBox).Checked)
            {
                autoRefresh.Start();
            }
            else
            {
                autoRefresh = null;
            }
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.dataGridViewSMS.Rows.Clear();
        }
        #endregion

        private void OpenPortConnection(string portName)
        {
            port = new SerialPort();
            receiveNow = new AutoResetEvent(false);
            try
            {
                port.PortName = portName;
                port.BaudRate = 115200;
                port.DataBits = 8;
                port.StopBits = StopBits.One;
                port.Parity = Parity.None;
                port.ReadTimeout = 300;
                port.WriteTimeout = 300;
                port.Encoding = Encoding.GetEncoding("iso-8859-1");
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                port.Open();
                port.DtrEnable = true;
                port.RtsEnable = true;
                programStatus.Text = "Status: Connected to COM Port " + comboBoxComPort.Text;
                btnConnect.Text = "Disconnect";
                this.checkBoxAutoRefresh.Enabled = true;
                this.btnListSMS.Enabled = true;
            }
            catch (Exception ex)
            {
                TmpMessage("Error: COMPort " + comboBoxComPort.Text + " already in use by another Program");
            }
        }
        private void ClosePortConnection()
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    TmpMessage("Status: COM Port " + comboBoxComPort.Text + " disconnected");
                    this.checkBoxAutoRefresh.Enabled = false;
                    this.btnListSMS.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                TmpMessage("Error: Could not disconnect Comport " + comboBoxComPort.Text);
            }
        }

        private void AutoRefresh()
        {
            while (this.checkBoxAutoRefresh.Checked)
            {
                this.InvokeEx(f => f.dataGridViewSMS.Rows.Clear());
                string result = ExecCommand("at+cmgl=4");
                List<ConfirmationMessage> confirmationMessages = ParseConfirmationMessage(result);
                foreach (var item in confirmationMessages)
                {
                    string[] row = { item.GetID().ToString(), item.GetTelNr(), item.GetStatus(), item.GetTimeStamp().ToString(), item.GetDischarge().ToString(), item.GetMessage() };
                    this.InvokeEx(f => f.dataGridViewSMS.Rows.Add(row));
                }
            }
            Thread.Sleep(1000);
        }
        public string ExecCommand(string command)
        {
            try
            {
                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                receiveNow.Reset();
                port.Write(command + "\r");

                string input = ReadResponse(300);
                if ((input.Length == 0) || ((!input.EndsWith("\r\n> ")) && (!input.EndsWith("\r\nOK\r\n"))))
                    TmpMessage("No success message was received.");
                return input;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ReadResponse(int timeout)
        {
            string buffer = string.Empty;
            try
            {
                do
                {
                    if (receiveNow.WaitOne(timeout, false))
                    {
                        string t = port.ReadExisting();
                        buffer += t;
                    }
                    else
                    {
                        if (buffer.Length > 0)
                            TmpMessage("Response received is incomplete.");
                        else
                            TmpMessage("No data received from phone.");
                    }
                }
                while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\n> ") && !buffer.EndsWith("\r\nERROR\r\n"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;
        }
        private void TmpMessage(string errorMessage)
        {
            errorThread = new Thread(
                   () => ShowErrorMessage(errorMessage));
            errorThread.Start();
        }
        private void ShowErrorMessage(string message)
        {
            string previousMessage = programStatus.Text;
            this.programStatus.Text = message;
            Thread.Sleep(2500);
            this.programStatus.Text = previousMessage;
        }
        private List<ConfirmationMessage> ParseConfirmationMessage(string message)
        {
            List<string> messages = message.Split(new string[] { "+CMGL: " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<ConfirmationMessage> confirmationMessages = new List<ConfirmationMessage>();
            messages.RemoveAt(0);
            foreach (var item in messages)
            {
                List<string> messageParts = item.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                ConfirmationMessage newConfirmationmessage = new ConfirmationMessage();
                foreach (var item2 in messageParts)
                {
                    if (item2.Contains(","))
                    {
                        newConfirmationmessage.SetID(Int32.Parse(item2.Remove(1)));
                    }
                    else if (item2.Contains("F"))
                    {
                        newConfirmationmessage.SetMessage(item2);
                        IncomingSmsPdu sms = IncomingSmsPdu.Decode(item2, true);
                        newConfirmationmessage.SetTelNr((sms as SmsStatusReportPdu).RecipientAddress);
                        newConfirmationmessage.SetTimeStamp((sms as SmsStatusReportPdu).SCTimestamp.ToDateTime());
                        newConfirmationmessage.SetDischarge((sms as SmsStatusReportPdu).DischargeTime.ToDateTime());
                        newConfirmationmessage.SetStatus((sms as SmsStatusReportPdu).Status.ToString());
                        confirmationMessages.Add(newConfirmationmessage);
                    }
                }
            }
            return confirmationMessages;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
