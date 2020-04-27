using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Playground.Helpers.Abstractions;
using Playground.Projections.Abstractions;

namespace Playground.Helpers.Reflection
{
    internal class AttributeBinder
    {
        public void BindTo(Control control, IProjectorEngine sender, PropertyInfo prop)
        {
            var ma = prop.GetCustomAttribute<ModifiableAttribute>();
            if (ma is null) return;

            BindComboBox(sender, control, prop, x => EngineChanged(x, ma.RequiresReset));
            BindCheckBox(sender, control, prop, x => EngineChanged(x, ma.RequiresReset));
            BindTrackBar(sender, control, prop, ma, x => EngineChanged(sender, ma.RequiresReset));
        }

        public void BindTo(Control control, IGraphicExtension sender, PropertyInfo prop)
        {
            var ma = prop.GetCustomAttribute<ModifiableAttribute>();
            if (ma is null) return;
            BindCheckBox(sender, control, prop, x => Redraw(sender, ma.RequiresReset));
            BindTrackBar(sender, control, prop, ma, x => Redraw(sender, ma.RequiresReset));
        }

        private void BindComboBox<T>(T sender, Control control, PropertyInfo prop, Action<T> OnChangeCallback)
        {
            if (prop.PropertyType.BaseType == typeof(Enum))
            {
                var c = new ComboBox
                {
                    Text = prop.Name,
                    TabStop = false, FormattingEnabled = false
                };
                c.Items.AddRange(prop.PropertyType.GetEnumNames().Cast<object>().ToArray());

                c.SelectedIndexChanged += (s, e) =>
                {
                    prop.SetValue(sender, Enum.Parse(prop.PropertyType, (string)c.SelectedItem));
                    OnChangeCallback(sender);
                };
                c.SelectedIndex = 0;
                control.Controls.Add(c);
            }

        }

        private void BindTrackBar<T>(T sender, Control control, PropertyInfo prop, ModifiableAttribute ma, Action<T> OnChangeCallback)
        {
            if (prop.PropertyType == typeof(float) ||
                prop.PropertyType == typeof(double) ||
                prop.PropertyType == typeof(int))
            {
                var flow = new FlowLayoutPanel();
                flow.VerticalScroll.Enabled = true; 
                var c = new TrackBar
                {
                    Maximum = (int)(ma.Max),
                    Minimum = (int)(ma.Min),
                    Value = ma.GetIntegerForControl((float)Convert.ChangeType(prop.GetValue(sender), typeof(float))),
                    TabStop = false,
                    CausesValidation = false
                };
                var desr = new Label { Text = $@"{prop.Name}:{ma.GetSimpleValue(c.Value)}" };
                c.Scroll += (s, e) =>
                {
                    var t = ma.GetSimpleValue(c.Value);
                    prop.SetValue(sender, Convert.ChangeType(t, prop.PropertyType));
                    desr.Text = $@"{prop.Name}:{t}";
                    OnChangeCallback(sender);
                };
                flow.FlowDirection = FlowDirection.BottomUp;
                flow.Controls.Add(c);
                flow.Controls.Add(desr);
                control.Controls.Add(flow);
            }
        }
        private void BindCheckBox<T>(T sender, Control control, PropertyInfo prop, Action<T> OnChangeCallback)
        {
            if (prop.PropertyType == typeof(bool))
            {
                var c = new CheckBox
                {
                    Text = prop.Name,
                    TabStop = false
                };
                c.CheckedChanged += (s, e) =>
                {
                    prop.SetValue(sender, Convert.ChangeType(c.Checked, prop.PropertyType));
                    OnChangeCallback(sender);
                };
                c.Checked = (bool)prop.GetValue(sender);
                control.Controls.Add(c);
            }
        }

        private void Redraw(IGraphicExtension sender, bool resetModel)
        {
            OnRedraw?.Invoke(sender, resetModel);
        }

        private void EngineChanged(IProjectorEngine sender, bool resetModel)
        {
            OnEngineChanged?.Invoke(sender, resetModel);
        }

        public event Action<IGraphicExtension, bool> OnRedraw;
        public event Action<IProjectorEngine, bool> OnEngineChanged;
    }
}