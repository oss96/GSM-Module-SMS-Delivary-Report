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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_Modem {
    public partial class Form1 : Form {
        private SerialPort port;
        private string portName;

        public Form1() {
            InitializeComponent();

            portName = string.Empty;
            port = null;
        }

        private bool ConnectSerialPort(string portName) {
            try {
                if (portName != string.Empty) {
                    port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
                    port.DataReceived += Port_DataReceived;
                    port.ErrorReceived += Port_ErrorReceived;
                    port.PinChanged += Port_PinChanged;
                    port.NewLine = Environment.NewLine;
                    port.Encoding = Encoding.ASCII;

                    try {
                        port.Open();

                    } catch (UnauthorizedAccessException) {
                        // ignore exception
                        System.Media.SystemSounds.Asterisk.Play();
                    }

                } else {
                    MessageBox.Show("Port Name not set", "ERROR!");
                    return false;
                }

            } catch (IOException ioe) {
                CloseSerialPort();
                MessageBox.Show(ioe.Message, "ERROR!");
                return false;
            } catch (Exception e) {
                CloseSerialPort();
                MessageBox.Show(e.Message, "ERROR!");
                return false;
            }

            if (btnConnect.Text == "Connect") {
                btnConnect.Text = "Disconnect";
            }

            return true;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            var ports = SerialPort.GetPortNames();
            comboBoxComPort.DataSource = ports;
        }

        protected override void OnClosing(CancelEventArgs e) {
            CloseSerialPort();

            base.OnClosing(e);
        }

        private void comboBoxPorts_SelectedValueChanged(object sender, EventArgs e) {
            var cb = sender as ComboBox;
            portName = cb.SelectedValue.ToString();
        }

        private void CloseSerialPort() {
            if (port != null) {
                if (port.IsOpen) {
                    try {
                        port.Close();

                    } catch (IOException e) {
                        // ignore, and just close
                    }
                }
                port = null;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e) {
            if (btnConnect.Text == "Connect") {
                if (ConnectSerialPort(portName)) {
                    btnConnect.Text = "Disconnect";
                    comboBoxComPort.Enabled = false;
                    btnRefreshPorts.Enabled = false;
                } else {
                    System.Media.SystemSounds.Asterisk.Play();
                }

            } else {
                CloseSerialPort();
                btnConnect.Text = "Connect";
                comboBoxComPort.Enabled = true;
                btnRefreshPorts.Enabled = true;
            }
        }

        private void SendCommand(string command) {
            if (port == null) {
                if (!ConnectSerialPort(portName)) {
                    return;
                }
            }

            if (!port.IsOpen) {
                port.Open();
            }

            port.WriteLine(command);
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) {
            Debug.WriteLine(e.EventType.ToString() + ": " + e.ToString());
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            Debug.WriteLine(e.EventType.ToString() + ": " + e.ToString());
            string res = (sender as SerialPort).ReadExisting();
            ParseSmsResponse(res);
        }

        private void Port_PinChanged(object sender, SerialPinChangedEventArgs e) {
            Debug.WriteLine(e.EventType.ToString() + ": " + e.ToString());
        }

        private void btnRefreshPorts_Click(object sender, EventArgs e) {
            var ports = SerialPort.GetPortNames();
            comboBoxComPort.DataSource = ports;
        }


        private void ParseSmsResponse(string response) {
            if (response.ToLower().Contains("cmgl")) {
                string[] allResponses = response.Split(new string[] { "+CMGL:" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < allResponses.Length; i++) {
                    string[] parts = allResponses[i].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length > 1) {
                        SmsStatusReportPdu smsInfo = (SmsStatusReportPdu)IncomingSmsPdu.Decode(parts[1], true);
                        string[] listItem = new string[] {
                            smsInfo.RecipientAddress,
                            smsInfo.Status.Category.ToString(),
                            smsInfo.SCTimestamp.ToDateTime().ToString(),
                            smsInfo.DischargeTime.ToDateTime().ToString(),
                            parts[1]
                        };

                        listViewSMS.Invoke((MethodInvoker)delegate {
                            listViewSMS.Items.Add(new ListViewItem(listItem));
                        });
                    }
                }

            }
        }

        private void btnExit_Click(object sender, EventArgs e) {
            SendCommand("at+cmgl=4");
        }
    }
}
