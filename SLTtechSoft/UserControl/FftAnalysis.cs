﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;

namespace Io_JigTest_Motor
{
    public partial class FftAnalysis : UserControl
    {
        private const int SAMPLE_RATE = 48000;
        private NAudio.Wave.WaveInEvent wvin;
        public FftAnalysis()
        {
            InitializeComponent();
        }

        double _peakFreq = 0;
        public double PeakFreq
        {
            get { return _peakFreq; }
        }

        double _peakPower = 0;
        public double PeakPower
        {
            get { return _peakPower; }
        }

        public void Start()
        {

            formsPlot1.Plot.Margins(0);

            cbDevices.Items.Clear();
            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
                cbDevices.Items.Add(NAudio.Wave.WaveIn.GetCapabilities(i).ProductName);
            if (cbDevices.Items.Count > 0)
                cbDevices.SelectedIndex = 0;
            else txtLog.AppendText("ERROR: no recording devices available");
            timer1.Enabled = true;
        }
        bool isStop = false;
        public void Stop()
        {
            isStop = true;
            timer1.Stop();
            timer1.Enabled = false;
        }
        public void Clear()
        {
            _peakFreq = 00;
            _peakPower = 00;

        }

        private void cbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            wvin?.Dispose();
            wvin = new NAudio.Wave.WaveInEvent();
            wvin.DeviceNumber = cbDevices.SelectedIndex;
            wvin.WaveFormat = new NAudio.Wave.WaveFormat(rate: SAMPLE_RATE, bits: 16, channels: 1);
            wvin.DataAvailable += OnDataAvailable;
            wvin.BufferMilliseconds = 20;
            wvin.StartRecording();
        }

        double[] lastBuffer;
        private void OnDataAvailable(object sender, NAudio.Wave.WaveInEventArgs args)
        {
            int bytesPerSample = wvin.WaveFormat.BitsPerSample / 8;
            int samplesRecorded = args.BytesRecorded / bytesPerSample;
            if (lastBuffer is null || lastBuffer.Length != samplesRecorded)
                lastBuffer = new double[samplesRecorded];
            for (int i = 0; i < samplesRecorded; i++)
                lastBuffer[i] = BitConverter.ToInt16(args.Buffer, i * bytesPerSample);
        }

        ScottPlot.Plottable.SignalPlot signalPlot;
        ScottPlot.Plottable.VLine peakLine;

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (lastBuffer is null)
                return;

            timer1.Enabled = false;

            var window = new FftSharp.Windows.Hanning();
            double[] windowed = window.Apply(lastBuffer);
            double[] zeroPadded = FftSharp.Pad.ZeroPad(windowed);
            double[] fftPower = cbDecibel.Checked ?
                FftSharp.Transform.FFTpower(zeroPadded) :
                FftSharp.Transform.FFTmagnitude(zeroPadded);
            double[] fftFreq = FftSharp.Transform.FFTfreq(SAMPLE_RATE, fftPower.Length);

            // determine peak frequency
            double peakFreq = 0;
            double peakPower = 0;
            for (int i = 0; i < fftPower.Length; i++)
            {
                if (fftFreq[i] < 600)
                    continue;
                if (fftPower[i] > peakPower)
                {
                    peakPower = fftPower[i];
                    peakFreq = fftFreq[i];
                    
                }
            }
            
            _peakFreq = peakFreq;
            _peakPower = peakPower;

            lblPeak.Text = $"Peak Frequency: {peakFreq:N0} Hz";
            lbPower.Text = $"Power:{_peakPower} db";
            formsPlot1.Plot.XLabel("Frequency Hz");

            // make the plot for the first time, otherwise update the existing plot
            if (formsPlot1.Plot.GetPlottables().Count() == 0)
            {
                signalPlot = formsPlot1.Plot.AddSignal(fftPower, 2.0 * fftPower.Length / SAMPLE_RATE);
                peakLine = formsPlot1.Plot.AddVerticalLine(peakFreq, ColorTranslator.FromHtml("#66FF0000"), 2);
            }
            else
            {
                signalPlot.Ys = fftPower;
                peakLine.X = peakFreq;
                peakLine.IsVisible = cbPeak.Checked;
            }

            if (cbAutoAxis.Checked)
            {
                try
                {
                    formsPlot1.Plot.AxisAuto(horizontalMargin: 0);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }

            try
            {
                formsPlot1.Render();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            if (!isStop) timer1.Enabled = true;
        }

     
    }
}
