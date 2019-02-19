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
                    programStatus.Text = "Status: not connected to a COM Port";
                }
            }
            catch (Exception ex)
            {
                TmpMessage("Error: Could not disconnect Comport " + comboBoxComPort.Text);
            }
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

        private void btnListSMS_Click(object sender, EventArgs e)
        {
            string result = ExecCommand("at+cmgl=4");
            string parsedConfirmationMessage = ParseConfirmationMessage(result);
            IncomingSmsPdu sms = IncomingSmsPdu.Decode(parsedConfirmationMessage, true);

            Console.WriteLine(sms.UserDataText);

            this.listViewSMS.Items.Add(result);
        }

        private string ParseConfirmationMessage(string message)
        {
            string[] messageParts = message.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            return messageParts[2];
        }
    }
}
