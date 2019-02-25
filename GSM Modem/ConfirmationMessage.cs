using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_Modem
{
    class ConfirmationMessage
    {
        private int id;
        private string telNr;
        private string message;
        private DateTime timeStamp;
        private DateTime discharge;
        private string status;


        public ConfirmationMessage()
        {
            this.id = 1;
            this.telNr = "";
            this.message = "";
            this.timeStamp = new DateTime(1970, 01, 01);
            this.discharge = new DateTime(1970, 01, 01);
            this.status = "";
        }
        public ConfirmationMessage(int inputID, string inputTelNr, string inputMessage, DateTime inputTimeStamp, DateTime inputDischarge, string inputStatus)
        {
            this.id = inputID;
            this.telNr = inputTelNr;
            this.message = inputMessage;
            this.timeStamp = inputTimeStamp;
            this.discharge = inputDischarge;
            this.status = inputStatus;
        }


        #region Set
        internal void SetID(int inputID)
        {
            this.id = inputID;
        }
        internal void SetTelNr(string inputTelNr)
        {
            this.telNr = inputTelNr;
        }
        internal void SetMessage(string inputMessage)
        {
            this.message = inputMessage;
        }
        internal void SetTimeStamp(DateTime inpuTimeStamp)
        {
            this.timeStamp = inpuTimeStamp;
        }
        internal void SetDischarge(DateTime inputDischarge)
        {
            this.discharge = inputDischarge;
        }
        internal void SetStatus(string inputStatus)
        {
            this.status = inputStatus;
        }
        #endregion

        #region Get
        internal int GetID()
        {
            return this.id;
        }
        internal string GetTelNr()
        {
            return this.telNr;
        }
        internal string GetMessage()
        {
            return this.message;
        }
        internal DateTime GetTimeStamp()
        {
            return this.timeStamp;
        }
        internal DateTime GetDischarge()
        {
            return this.discharge;
        }
        internal string GetStatus()
        {
            return this.status;
        }
        #endregion
    }
}
