using System;
using System.Windows.Forms;
using Basler.Pylon;

namespace GUISampleMultiCam
{
    public partial class ParameterSliderUserControl : UserControl
    {
        private IParameter parameter = null;
        private bool sliderMoving = false;
        private bool logarithmic = false;
        private Label valueLabel = null;
        double min = 0;
        double max = 0;
        double val = 0;
        double percent = 0;
        private readonly int SLIDER_CONSTANT_FACTOR = 10000;

        public ParameterSliderUserControl()
        {
            InitializeComponent();
        }

        // Sets the parameter displayed by the user control.
        public IParameter Parameter
        {
            set
            {
                // Unsubscribe from the previous parameter.
                if (parameter != null)
                {
                    parameter.ParameterChanged -= ParameterChanged;
                }

                // Set the new parameter and subscribe to it.
                parameter = value;
                if (parameter != null)
                {
                    parameter.ParameterChanged += ParameterChanged;
                    UpdateValues();
                }
                else
                {
                    Reset();
                }
            }
        }

        // Change the behavior of the slider to logarithmic.
        public bool Logarithmic
        {
            get { return logarithmic; }
            set { this.logarithmic = value; }
        }

        // Converts percent value to slider value.
        private int PercentToSliderValue(double percent)
        {
            int result;
            if (logarithmic)
            {
                result =  (int)(Math.Log(percent + 1, 2) * SLIDER_CONSTANT_FACTOR);
            }
            else
            {
                result = (int)(((max - min) / 100.0) * percent);
            }
            return result;
        }

        // Converts slider value to percent value.
        private double SliderToPercentValue(int sliderValue)
        {
            double result;
            if (logarithmic)
            {
                // Due to rounding in PercentToSliderValue, we can't reach the maximum.
                // If the slider maximum has been reached, return 100 %.
                if (slider.Maximum == sliderValue)
                {
                    result = 100.0;
                }
                else
                {
                    result = Math.Pow(2, (double)sliderValue / SLIDER_CONSTANT_FACTOR) - 1;
                }
            }
            else
            {
                result = (((double)sliderValue) / (max - min)) * 100.0;
            }
            return result;
        }

        public void SetLabel(Label label)
        {
            valueLabel = label;
            valueLabel.Text = "0";
        }

        private void Reset()
        {
            slider.Enabled = false;
            valueLabel.Text = "0";
        }

        private void ParameterChanged(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(ParameterChanged), sender, e);
                return;
            }
            try
            {
                // Update accessibility and parameter values.
                UpdateValues();
            }
            catch
            {
                // If errors occurred, disable the control.
                Reset();
            }
        }

        // Gets the current values from the parameter and displays them.
        private void UpdateValues()
        {
            try
            {
                if (parameter != null  && parameter.IsReadable)
                {
                    if (parameter.IsWritable && !sliderMoving)
                    {
                        if (this.parameter is IFloatParameter)
                        {
                            IFloatParameter floatParameter = this.parameter as IFloatParameter;
                            // Break any recursion if the value does not exactly match the slider value.
                            sliderMoving = true;
                            // Get the values.
                            min = floatParameter.GetMinimum();
                            max = floatParameter.GetMaximum();
                            val = floatParameter.GetValue();
                            percent = floatParameter.GetValuePercentOfRange();
                            slider.SmallChange = 1;
                        }
                        else
                        {
                            IIntegerParameter intParameter = this.parameter as IIntegerParameter;
                            // Break any recursion if the value does not exactly match the slider value.
                            sliderMoving = true;
                            // Get the values.
                            min = intParameter.GetMinimum();
                            max = intParameter.GetMaximum();
                            val = intParameter.GetValue();
                            percent = intParameter.GetValuePercentOfRange();
                            // Configure the SmallChange property of the parameter increment value to prevent invalid values.
                            slider.SmallChange = (int)intParameter.GetIncrement();
                        }
                        // Update the slider. Scale values by scaling factor.
                        slider.Minimum = PercentToSliderValue(0);
                        slider.Maximum = PercentToSliderValue(100);
                        slider.Value = PercentToSliderValue(percent);
                        slider.TickFrequency = (slider.Maximum - slider.Minimum) / 10;
                        valueLabel.Text = string.Format("{0:0}", val);

                        // Update the access status.
                        slider.Enabled = parameter.IsWritable;

                        return;
                    }
                }
                else
                {
                    slider.Enabled = false;
                    valueLabel.Text = string.Format("0");
                }
            }
            catch
            {
                // If errors occurred, disable the control.
                Reset();
            }
            finally
            {
                sliderMoving = false;
            }
        }

        // Occurs when the slider position changes.
        private void SliderScroll(object sender, EventArgs e)
        {
            if (this.parameter != null)
            {
                try
                {
                    if (this.parameter is IFloatParameter)
                    {
                        IFloatParameter parameter = this.parameter as IFloatParameter;
                        if (parameter.IsWritable && !sliderMoving)
                        {
                            // Break any recursion if the value does not exactly match the slider value.
                            sliderMoving = true;

                            // Set the value. Scale by scaling factor.
                            parameter.SetValuePercentOfRange(SliderToPercentValue(slider.Value));
                            double val = parameter.GetValue();
                            valueLabel.Text = string.Format("{0:0}", val);
                        }
                    }
                    else
                    {
                        IIntegerParameter parameter = this.parameter as IIntegerParameter;
                        if (parameter.IsWritable && !sliderMoving)
                        {
                            // Break any recursion if the value does not exactly match the slider value.
                            sliderMoving = true;

                            // Set the value. Scale by scaling factor.
                            parameter.SetValuePercentOfRange(SliderToPercentValue(slider.Value));
                            long val = parameter.GetValue();
                            valueLabel.Text = val.ToString();
                        }
                    }
                }
                catch(Exception)
                {
                    // Ignore any errors here.
                }
                finally
                {
                    sliderMoving = false;
                }
            }
        }
    }
}
