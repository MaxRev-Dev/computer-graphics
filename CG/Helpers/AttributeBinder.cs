using System;
using System.Reflection;
using System.Windows.Forms;

namespace Playground.Helpers
{
    internal class AttributeBinder
    {
        public void BindTo(Control control, IGraphicExtension sender, PropertyInfo prop)
        {
            var ma = prop.GetCustomAttribute<ModifiableAttribute>();
            if (ma is null) return;
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
                    Redraw(sender, ma.RequiresReset);
                };
                control.Controls.Add(c);
            }

            if (prop.PropertyType == typeof(float) ||
                prop.PropertyType == typeof(double) ||
                prop.PropertyType == typeof(int))
            {
                var flow = new FlowLayoutPanel();
                var val = Convert.ChangeType(prop.GetValue(sender), typeof(float));
                var c = new TrackBar
                {
                    Maximum = (int)(ma.Max),
                    Minimum = (int)(ma.Min),
                    Value = (int)((float)val * (1f / ma.Scaling)),
                    TabStop = false,
                    CausesValidation = false
                };
                var desr = new Label {Text = $@"{prop.Name}:{c.Value * ma.Scaling}"};
                c.Scroll += (s, e) =>
                {
                    prop.SetValue(sender, Convert.ChangeType(c.Value * ma.Scaling, prop.PropertyType));
                    desr.Text = $@"{prop.Name}:{c.Value * ma.Scaling}";
                    Redraw(sender, ma.RequiresReset);
                };
                flow.FlowDirection = FlowDirection.BottomUp;
                flow.Controls.Add(c);
                flow.Controls.Add(desr);
                control.Controls.Add(flow);
            }
        }

        private void Redraw(IGraphicExtension sender, bool resetModel)
        {
            OnRedraw?.Invoke(sender, resetModel);
        }

        public event Action<IGraphicExtension, bool> OnRedraw;
    }
}