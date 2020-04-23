using GraphicExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Helpers.Abstractions;
using Playground.Helpers.Containers;
using Playground.Helpers.Reflection;
using Playground.Models;
using Playground.Projections.Abstractions;
using Playground.Projections.Engines;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using Color = System.Drawing.Color;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Pen = System.Drawing.Pen;

namespace Playground
{
    public sealed partial class Main : Form
    {
        #region Fields

        // graphics 
        private Bitmap _bitmap;
        private GraphicContext _context;

        // graphic models container
        private readonly ProjectorContainer _projectors = new ProjectorContainer();
        private readonly ExtensionContainer _extensions = new ExtensionContainer();

        // modes
        private bool _wordTransformOn;
        private bool _autoRotate;
        private bool _moveModel;

        // history points 
        private readonly Queue<float[]> _lastpoints = new Queue<float[]>();

        // intermediate rotation angle
        private float _angle;
        private float minRotate = 0.01745329F; // Math.PI / 180
        private readonly float[,] _defaultTrs = MatrixExtensions.IdentityF(4);

        private Pen foregroundPen;
        private Color _foreground;

        // debug test
        private readonly StringBuilder _testOutputBuilder;
        private readonly StringWriter _testOutputWriter;
        #endregion


        public Main()
        {
            InitializeComponent();
            CreatePlayground();

            Load += (s, e) =>
            {
                _context = new GraphicContext();
                _context.Reset(_bitmap);
                _projectors.AddRange(AssemblyExtensions.FindAllImplementationsAndActivate<IProjectorEngine>());

                _projectors.InitializeAll(_context);

                _extensions.AddRange(AssemblyExtensions.FindAllImplementationsAndActivate<IGraphicExtension>());

                _projectors.Use<PerspectiveProjectorEngine>();
                BindExtensionControls();
                InitModelAndFrameTick();
            };

            playground.MouseWheel += CG_MouseWheel;
            playground.KeyPress += Main_KeyPress;
            playground.Paint += (s, e) => e.Graphics.DrawImage(_bitmap, 0, 0);
            _testOutputBuilder = new StringBuilder();
            _testOutputWriter = new StringWriter(_testOutputBuilder);
        }

        #region Init bindings

        private void BindExtensionControls()
        {
            tabs.SelectedIndexChanged += (s, e) => SubscribeTabs(s, _extensions);
            projectorTabs.SelectedIndexChanged += (s, e) => SubscribeTabs(s, _projectors);
            var binder = new AttributeBinder();
            ExtendTo(tabs, _extensions, binder.BindTo);
            ExtendTo(projectorTabs, _projectors, binder.BindTo);

            binder.OnRedraw += (e, reset) =>
            {
                if (reset)
                    e.Reset(_projectors.Current);
                Draw();
            };

            // apply on visible tabs
            projectorTabs.SelectedIndex = tabs.SelectedIndex = 1;
        }

        private void ExtendTo<T>(TabControl control, IActiveContainer<T> extensions, Action<FlowLayoutPanel, T, PropertyInfo> PropertyCallBack)
        {
            foreach (var extension in extensions)
            {
                var name = extension.GetType().Name;
                var page = new TabPage { Text = name };
                control.TabPages.Add(page);
                var floater = new FlowLayoutPanel { Dock = DockStyle.Fill };
                floater.VerticalScroll.Enabled = true;
                floater.AutoScroll = true;
                floater.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                floater.AutoSize = true;
                page.Controls.Add(floater);
                foreach (var prop in extension.GetType().GetProperties())
                {
                    PropertyCallBack(floater, extension, prop);
                }
            }
        }

        private void SubscribeTabs<T>(object sender, IActiveContainer<T> extensions)
        {
            var tab = ((TabControl)sender).SelectedTab;
            var name = tab.Text;
            extensions.ActiveChanged(extensions.FirstOrDefault(x => x.GetType().Name == name));
        }

        #endregion

        #region Reset

        private void InitModelAndFrameTick()
        {
            _foreground = Color.Blue;
            foregroundPen = new Pen(_foreground, 1);
            ResetAll();
            var timer = new Timer
            {
                Interval = 33
            };
            timer.Start();
            timer.Tick += OnFrame;
        }

        private void ResetAll()
        {
            ResetModel();
            _projectors.Current.ResetWorld();
        }

        private void ResetModel()
        {
            _extensions.InitializeAll(_projectors.Current);
        }
        #endregion

        #region Core graphics

        private void Transform(float[,] trs)
        {
            if (!_moveModel)
                _extensions.ApplyTransformation(trs);
            else
            {
                var c = _projectors.Current as PerspectiveProjectorEngine;
                c?.TransformCamera(trs);
            }
        }

        private void CreatePlayground()
        {
            _bitmap?.Dispose();
            _context?.Graphics?.Dispose();
            if (!ValidateGraphics()) return;
            _bitmap = new Bitmap(playground.Bounds.Width, playground.Bounds.Height);
            _context?.Reset(_bitmap);
        }

        private void OnFrame(object sender, EventArgs e)
        {
            _projectors.Current.OnFrame();
            if (_autoRotate)
            {
                if (_angle >= Math.PI * 2)
                    _angle = 0;
                _angle += minRotate;
                Transform(CG.TranslateEllipseXZ(Eps(3f), Eps(6), _angle));
            }

            if (CheckKeys() || _autoRotate)
            {
                Draw();
            }

            //mainFigure.PrintThrough(_testOutputWriter);
            label1.Text = _testOutputBuilder.ToString();
            _testOutputBuilder.Clear();
        }
        private void Draw(IGraphicExtension extension = default)
        {
            if (!ValidateGraphics()) return;
            _context.Graphics.Clear(playground.BackColor);
            if (extension == default)
                _extensions.DrawAll(_projectors.Current);
            else
                extension.Draw(_projectors.Current);

            playground.Invalidate();
        }

        private bool ValidateGraphics()
        {
            return !(playground.Bounds.IsEmpty ||
                     playground.Bounds.Height == 0 ||
                     playground.Bounds.Width == 0);
        }

        #endregion

        #region Keys & wheel

        private bool CheckKeys()
        {
            var trs = _defaultTrs;
            var modCtrl = Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Control);
            var modAlt = Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Alt);
            var tens = Eps(1) * 10;
            if (IsPressed(Key.G))
            {
                _extensions.Get<KochSnowflake>()?.NextGen();
            }

            if (IsPressed(Key.R))
            {
                ResetModel();
                _projectors.Current.ResetWorld();
            }

            if (IsPressed(Key.OemCloseBrackets))
            {
                trs = CG.ScaleP(Eps(100));
            }
            else if (IsPressed(Key.OemOpenBrackets))
            {
                trs = CG.ScaleP(Eps(90));
            }
            else if (IsPressed(Key.Up))
            {
                if (modAlt)
                    trs = CG.RotY(-tens);
                else if (modCtrl)
                    trs = CG.RotY(tens);
                else
                    trs = CG.TranslateY(-tens);
            }
            else if (IsPressed(Key.Down))
            {
                if (modAlt)
                    trs = CG.RotZ(tens);
                else if (modCtrl)
                    trs = CG.RotZ(-tens);
                else
                    trs = CG.TranslateY(tens);
            }
            else if (IsPressed(Key.Left))
            {
                if (modAlt)
                    trs = CG.RotX(tens);
                else if (modCtrl)
                    trs = CG.RotX(-tens);
                else
                    trs = CG.TranslateX(-tens);
            }
            else if (IsPressed(Key.Right))
            {
                if (modAlt)
                    trs = CG.TranslateZ(tens);
                else if (modCtrl)
                    trs = CG.TranslateZ(-tens);
                else
                    trs = CG.TranslateX(tens);
            }

            if (trs != default)
            {
                Transform(trs);
            }

            return !(trs is null);
        }

        private bool IsPressed(Key key)
        {
            var left = Keyboard.GetKeyStates(key);
            return ((left & KeyStates.Down) != 0) && ContainsFocus;
        }

        private void CG_MouseWheel(object sender, MouseEventArgs e)
        {
            var delta = e.Delta;
            Transform(CG.ScaleP(eps(delta)));
            Draw();
        }

        private void Main_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    _wordTransformOn = !_wordTransformOn;
                    break;
                case 'a':
                    _autoRotate = !_autoRotate;
                    break;
                case 'o':
                    _moveModel = !_moveModel;
                    break;
            }
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            CreatePlayground();
            playground.Invalidate();
        }
        #endregion

        #region Helpers
         
        private float eps(float x) => (float)(x > 0 ? Math.PI / 300 : -Math.PI / 300);

        private float Eps(float x) => (float)(x * Math.PI / 300);

        #endregion
    }
}