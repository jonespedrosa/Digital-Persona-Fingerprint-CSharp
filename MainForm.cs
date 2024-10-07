﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digital_Persona_Fingerprint
{
    public partial class MainForm : Form, DPFP.Capture.EventHandler
    {
        private DPFP.Capture.Capture Capturer;

        // Test OnGitHub

        public MainForm()
        {
            InitializeComponent();
        }

        protected void MakeReport(string message)
        {
            this.Invoke(new Action(delegate ()
            {
                StatusText.Text = message;
            }));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                Capturer = new DPFP.Capture.Capture();
                if (Capturer != null)
                {
                    Capturer.EventHandler = this;
                    MakeReport("Press start capture to start scanning");
                } 
                else
                {
                    MakeReport("Cannot initiate capture operation");
                }
            }
            catch
            {
                MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was connected");
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was disconnected");
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint was removed from the fingerprint reader");
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was touched");
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            MakeReport("The fingerprint reader was captured");
            Process(Sample);
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CapturedFeedback)
        {
            if (CapturedFeedback == DPFP.Capture.CaptureFeedback.Good)
                MakeReport("The quality of the fingerprint sample is good.");
            else
                MakeReport("The quality of the fingerprint sample is poor.");
        }

        protected virtual void Process(DPFP.Sample Sample)
        {
            DrawPicture(ConvertSampleToBitmap(Sample));
        }

        protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();
            Bitmap bitmap = null;
            Convertor.ConvertToPicture(Sample, ref bitmap);

            return bitmap;
        }

        private void DrawPicture(Bitmap bitmap)
        {
            fingerImage.Image = new Bitmap(bitmap, fingerImage.Size);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(Capturer != null)
            {
                try
                {
                    Capturer.StartCapture();
                           MakeReport("Using the fingerprint reader, scan your fingerprint.");
                }
                catch
                {
                    MakeReport("Can't initiate captured.");
                }
            }
        }
    }
}
