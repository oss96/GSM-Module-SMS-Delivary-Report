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
        //////////////////////////////////////////

        private SerialPort port;
        private string portName;
        private Thread statusThread;
        private AutoResetEvent receiveNow;

        //////////////////////////////////////////

        public MainForm()
        {
            InitializeComponent();

            portName = string.Empty;
            port = null;
            this.MinimumSize = this.Size;
            this.dataGridViewSMS.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            statusStrip1.Items.Insert(1, new ToolStripSeparator());
        }

        //////////////////////////////////////////

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var ports = SerialPort.GetPortNames();
            comboBoxComPort.DataSource = ports;
            this.portName = this.comboBoxComPort.Text;
            programStatus.Text = "Status: not connected to a COM Port";
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
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(dataGridViewSMS, new object[] {
                        false,
                        item.GetID().ToString(),
                        item.GetTelNr(),
                        item.GetStatus(),
                        item.GetTimeStamp().ToString(),
                        item.GetDischarge().ToString(),
                        item.GetMessage()
                    });
                this.dataGridViewSMS.Rows.Add(dataGridViewRow);
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
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.dataGridViewSMS.Rows.Clear();
            this.copyStatus.Text = "";
        }
        private void dataGridViewSMS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == senderGrid.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None).Index)
            {
                try
                {
                    Clipboard.SetText(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    for (int i = 0; i < senderGrid.RowCount; i++)
                        senderGrid.Rows[i].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                    SetStatusMessage("Copied Message " + senderGrid.Rows[e.RowIndex].Cells[1].Value, false);
                }
                catch (Exception)
                {
                    ShowStatusMessage("Value could not be copied. Please Restart the PC");
                }
            }
            else if (e.ColumnIndex == senderGrid.Columns.GetFirstColumn(DataGridViewElementStates.None, DataGridViewElementStates.None).Index)
            {
                if (e.RowIndex != -1 && senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewCheckBoxCell)
                {
                    if (Convert.ToBoolean(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == true)
                    {
                        senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell).FalseValue;
                    }
                    else
                    {
                        senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                    }
                }
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (port.IsOpen)
                    port.Close();
            }
            catch (Exception ex)
            {
            }
        }
        private void dataGridViewSMS_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection cells = (sender as DataGridView).SelectedCells;
            for (int i = 0; i < cells.Count; i++)
                if (cells[i].ColumnIndex == 0 || cells[i].ColumnIndex == 6)
                    cells[i].Selected = false;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            foreach (var item in dataGridViewSMS.Rows)
                if (Convert.ToBoolean((item as DataGridViewRow).Cells[0].Value))
                    ids.Add(Convert.ToInt32((item as DataGridViewRow).Cells[1].Value.ToString()));
            foreach (var item in ids)
            {
                string result = ExecCommand("at+cmgd=" + item);
            }
            this.btnDelete.Enabled = false;
            btnListSMS_Click(null, e);
        }
        private void dataGridViewSMS_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                bool shouldBeState = false;
                foreach (var item in (sender as DataGridView).Rows)
                {
                    if (!Convert.ToBoolean((item as DataGridViewRow).Cells[0].Value))
                    {
                        shouldBeState = true;
                    }
                }

                foreach (var item in (sender as DataGridView).Rows)
                {
                    (item as DataGridViewRow).Cells[0].Value = shouldBeState;
                }
            }
        }
        private void dataGridViewSMS_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            bool deleteButtonState = false;
            foreach (var item in senderGrid.Rows)
            {
                if (Convert.ToBoolean((item as DataGridViewRow).Cells[e.ColumnIndex].Value))
                {
                    deleteButtonState = true;
                }
            }
            btnDelete.Enabled = deleteButtonState;
            if (e.RowIndex != -1)
            {
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewCheckBoxCell)
                {
                    if (!Convert.ToBoolean(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                    {
                        foreach (var item in senderGrid.Rows[e.RowIndex].Cells)
                            if ((item as DataGridViewCell).ColumnIndex != 6)
                                (item as DataGridViewCell).Style.BackColor = Color.White;
                    }
                    else
                    {
                        btnDelete.Enabled = true;
                        foreach (var item in senderGrid.Rows[e.RowIndex].Cells)
                            if ((item as DataGridViewCell).ColumnIndex != 6)
                                (item as DataGridViewCell).Style.BackColor = Color.LightBlue;
                    }
                }
            }

        }
        #endregion


        #region Connection
        /// <summary>
        /// Opens the Serialport Connection
        /// </summary>
        /// <param name="portName"></param>
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
                SetStatusMessage("Error: COMPort " + comboBoxComPort.Text + " already in use by another Program", true);
            }
        }

        /// <summary>
        /// Closes the Serialport Connection
        /// </summary>
        /// <param name="portName"></param>
        private void ClosePortConnection()
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    SetStatusMessage("Status: COM Port " + comboBoxComPort.Text + " disconnected", true);
                    this.checkBoxAutoRefresh.Enabled = false;
                    this.btnListSMS.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                SetStatusMessage("Error: Could not disconnect Comport " + comboBoxComPort.Text, true);
            }
        }
        #endregion


        #region Functions
        /// <summary>
        /// Refreshes the read list automatically
        /// </summary>
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

        /// <summary>
        /// Excecutes AT commands
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
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
                    SetStatusMessage("No success message was received.", true);
                return input;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reads the received SMS responses
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
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
                            SetStatusMessage("Response received is incomplete.", true);
                        else
                            SetStatusMessage("No data received from modem.", true);
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

        /// <summary>
        /// Parses the SMS responses and displays the response on the DataGridView
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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
                    string pattern = "^[0-9]+";
                    Regex rgx = new Regex(pattern);
                    if (item2.Contains(","))
                    {
                        string s = rgx.Match(item2).ToString();
                        newConfirmationmessage.SetID(Int32.Parse(s));
                    }
                    else if (item2.Contains("F"))
                    {
                        newConfirmationmessage.SetMessage(item2);
                        IncomingSmsPdu sms = IncomingSmsPdu.Decode(item2, true);
                        newConfirmationmessage.SetTelNr((sms as SmsStatusReportPdu).RecipientAddress);
                        newConfirmationmessage.SetTimeStamp((sms as SmsStatusReportPdu).SCTimestamp.ToDateTime());
                        newConfirmationmessage.SetDischarge((sms as SmsStatusReportPdu).DischargeTime.ToDateTime());
                        if ((sms as SmsStatusReportPdu).Status.Category.ToString() == "Success")
                        {
                            newConfirmationmessage.SetStatus("Received");
                        }
                        else
                        {
                            newConfirmationmessage.SetStatus("not Received");

                        }
                        confirmationMessages.Add(newConfirmationmessage);
                    }
                }
            }
            return confirmationMessages;
        }
        #endregion


        #region Status
        /// <summary>
        /// Determine what kind of message you want to display
        /// </summary>
        /// <param name="statusMessage">the string to display</param>
        /// <param name="tmp">wether the message is a program status or copyStatus</param>
        private void SetStatusMessage(string statusMessage, bool tmp)
        {
            if (tmp)
            {
                statusThread = new Thread(
       () => ShowStatusMessage(statusMessage));
                statusThread.Start();
            }
            else
            {
                statusThread = new Thread(
       () => ShowCopiedValue(statusMessage));
                statusThread.Start();
            }
        }

        /// <summary>
        /// Display a string on the statuslabel "programStatus"
        /// </summary>
        /// <param name="message"></param>
        private void ShowStatusMessage(string message)
        {
            string previousMessage = programStatus.Text;
            this.programStatus.Text = message;
            Thread.Sleep(2500);
            this.programStatus.Text = previousMessage;
        }

        /// <summary>
        /// Display a string on the statuslabel "copyStatus"
        /// </summary>
        /// <param name="message"></param>
        private void ShowCopiedValue(string message)
        {
            this.copyStatus.Text = message;
        }
        #endregion

        //////////////////////////////////////////

        /* Next Steps:
         * save responses locally
         * delete saved responses
         * all saved responses should start with the id 11 - ... because new responses have ids from 1 - 10
         * Autorefresh without refreshing everything, only the new responses
        */
    }
}
