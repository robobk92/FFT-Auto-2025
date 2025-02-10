using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SLTtechSoft.Welcome;
namespace SLTtechSoft
{
    public partial class Welcome : Form
    {
        int count = 0;
        public class TestMode
        {
            public bool NoCam = false;
            public bool NoPLC = false;
            public bool NoScanner = false;
        }
        private TestMode _TestMode = new TestMode();
        public Welcome()
        {
            CheckLastProcessRunning(false);
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }
       
        public void StartNewForm()
        {
            _TestMode.NoCam = NoCam.Checked ;
            _TestMode.NoPLC = NoPLC.Checked;
            Form1 form1 = new Form1();
           form1.Show();
            this.Hide();
        }
        private void CheckLastProcessRunning(bool bShowMessage)
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                if (bShowMessage)
                {
                    MessageBox.Show("Another process is running, Please End last process", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                this.Close();
                Environment.Exit(0);
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
           
            for (int i = 1; i <= 100; i ++)
            {
                if (worker.CancellationPending == true)
                {
                    System.Threading.Thread.Sleep(100);
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(10);
                    worker.ReportProgress(i);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
          
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartNewForm();
            
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
