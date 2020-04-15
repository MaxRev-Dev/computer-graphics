using GraphicExtensions;
using MaxRev.Extensions.Matrix;
using Playground.Helpers;
using Playground.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using Playground.Projections;
using Playground.Projections.Abstractions;
using Color = System.Drawing.Color;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Pen = System.Drawing.Pen;

namespace Playground
{
    public sealed partial class Main : Form
    {
        #region Fields

        // graphics
        private Graphics _graphics;
        private Bitmap _bitmap;

        // graphic models container
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

        private Pen foregroundPen;
        private Color _background;
        private Color _foreground;

        // debug test
        private readonly StringBuilder _testOutputBuilder;
        private readonly StringWriter _testOutputWriter;

        private readonly IProjectorEngine _projector;
        #endregion

        #region Properties

        #endregion

        public Main()
        {
            InitializeComponent();
            CreatePlayground();

            _projector = new PlanarProjectorEngine(_graphics);
            //_projector = new DimetricProjectorEngine(_graphics);

            Load += (s, e) =>
            {
                _extensions.AddRange(new IGraphicExtension[] {
                    //new KochSnowflake(foregroundPen),
                    //new NewtonBasins(),
                    //new Axis(),
                    //new Tetrahedron(),
                    //new Ellipsoid(),
                    //new Cube(),
                    //new FernBranch()
                    //new Hyperbola(),
                    new TCB_Spline()
                });
                InitModelAndFrameTick();
            };


            playground.MouseWheel += CG_MouseWheel;
            playground.KeyPress += Main_KeyPress;
            playground.Paint += (s, e) => e.Graphics.DrawImage(_bitmap, 0, 0);
            _testOutputBuilder = new StringBuilder();
            _testOutputWriter = new StringWriter(_testOutputBuilder);

        }

        #region Reset

        private void InitModelAndFrameTick()
        {
            _foreground = Color.Blue;
            _background = Color.DarkRed;
            foregroundPen = new Pen(_foreground, 1);
            ResetAll();
            var timer = new Timer
            {
                Interval = 33 // 33 ms/frame ≈ (30.3fps)
            };
            timer.Start();
            timer.Tick += OnFrame;
        }

        private void ResetAll()
        {
            ResetModel();
            _projector.ResetWorld();
        }


        private void ResetModel()
        {
            _extensions.InitializeAll(_projector);

            // Set fern's values if ext available
            if (_extensions.Get<FernBranch>() is FernBranch fern)
            {
                fern_A.Minimum = 0;
                fern_A.Maximum = 90;
                fern_K0.Minimum = 0;
                fern_K0.Maximum = 100;
                fern_K1.Minimum = 0;
                fern_K1.Maximum = 180;
                fern_B.Minimum = 0;
                fern_B.Maximum = 180;
                fern_Lmin.Minimum = 10;
                fern_Lmin.Maximum = 1000;
                fern_Lmin.Value = (int)fern.Lmin;
                fern_K0.Value = (int)fern.K0 * 100;
                fern_K1.Value = (int)fern.K1 * 100;
                fern_B.Value = (int)(fern.Beta / Math.PI * 180);
                fern_A.Value = (int)(fern.Alfa / Math.PI * 180);
            }

            //Transform(CG.TranslateZ(-10));
        }
        #endregion

        private void Transform(float[,] trs)
        {
            _extensions.ApplyTransformation(trs);
        }

        private void CreatePlayground()
        {
            _bitmap?.Dispose();
            _graphics?.Dispose();
            if (playground.Bounds.IsEmpty) return;
            _bitmap = new Bitmap(playground.Bounds.Width, playground.Bounds.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _projector?.Use(_graphics);
        }

        private void OnFrame(object sender, EventArgs e)
        {
            _projector.OnFrame();
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

        #region GLU

        void gluPerspective(
            float angOfView,
            float imageAspectRatio,
            float n,
            out float b, out float t, out float l, out float r)
        {
            var scale = (float)(Math.Tan(angOfView * 0.5 * Math.PI / 180) * n);
            r = imageAspectRatio * scale;
            l = -r;
            t = scale;
            b = -t;
        }

        // set the OpenGL perspective projection matrix
        void glFrustum(
            float b, float t, float l, float r, float n, float f, float[,] M)
        {
            // OpenGL perspective projection matrix
            M[0, 0] = 2 * n / (r - l);
            M[0, 1] = 0;
            M[0, 2] = 0;
            M[0, 3] = 0;

            M[1, 0] = 0;
            M[1, 1] = 2 * n / (t - b);
            M[1, 2] = 0;
            M[1, 3] = 0;

            M[2, 0] = (r + l) / (r - l);
            M[2, 1] = (t + b) / (t - b);
            M[2, 2] = -(f + n) / (f - n);
            M[2, 3] = -1;

            M[3, 0] = 0;
            M[3, 1] = 0;
            M[3, 2] = -2 * f * n / (f - n);
            M[3, 3] = 0;
        }

        #endregion

        #region Keys & wheel

        private bool CheckKeys()
        {
            var trs = MatrixExtensions.IdentityF(4);
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
                _projector.ResetWorld();
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
            return ((left & KeyStates.Down) != 0);
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
                case 'c':
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


        private void Draw()
        {
            if (playground.Bounds.IsEmpty) return;
            _graphics.Clear(playground.BackColor);
            _extensions.DrawAll(_projector);
            playground.Invalidate();
        }

        private bool TryDropVertex((float x, float y, float z) projectedVert)
        {
            // out of visible screen bounds
            return projectedVert.x < -1 ||
                   projectedVert.x > 1 ||
                   projectedVert.y < -1 ||
                   projectedVert.y > 1;
        }
        /* private void HistoryPath(float[,] vertices)
         {
             Func<float[], (float, float)> v = f => PointToScreen(f[0], f[1]);

             var figureCenter = CG.FigureCenter(vertices);
             _lastpoints.Enqueue(figureCenter);
             if (_lastpoints.Count > 500)
                 _lastpoints.Dequeue();
             foreach (var vx in _lastpoints)
             {
                 var vertCamera = vx.Multiply(worldToCamera);
                 var projectedVert = vertCamera.Multiply(projMatrix).ToPoint3D();
                 if (TryDropVertex(projectedVert)) continue;
                 var (xr, yr) = v(projectedVert.ToArray());
                 _graphics.DrawEllipse(Pens.Blue, xr, yr, 2, 2);
             }
         }*/


        private float eps(float x) => (float)(x > 0 ? Math.PI / 300 : -Math.PI / 300);

        private float Eps(float x) => (float)(x * Math.PI / 300);

        #region Fern controls

        private void fern_A_Scroll(object sender, EventArgs e)
        {
            var br = _extensions.Get<FernBranch>();
            if (br != default)
            {
                br.Alfa = (float)(fern_A.Value * 1f * Math.PI / 180);
                fernALabel.Text = $@"Fern alfa: {fern_A.Value}";
            }
        }

        private void fern_K0_Scroll(object sender, EventArgs e)
        {
            var br = _extensions.Get<FernBranch>();
            if (br != default)
            {
                br.K0 = fern_K0.Value * .01f;
                fernK0Label.Text = $@"Fern K0: {fern_K0.Value * .01f}";
            }
        }

        private void fern_K1_Scroll(object sender, EventArgs e)
        {
            var br = _extensions.Get<FernBranch>();
            if (br != default)
            {
                br.K1 = fern_K1.Value * .01f;
                fernK1Label.Text = $@"Fern K1: {fern_K1.Value * .01f}";
            }
        }

        private void fern_B_Scroll(object sender, EventArgs e)
        {
            var br = _extensions.Get<FernBranch>();
            if (br != default)
            {
                br.Beta = (float)(fern_B.Value * 1f * Math.PI / 180);
                fernBLabel.Text = $@"Fern beta: {fern_B.Value}";
            }
        }

        private void fern_Lmin_Scroll(object sender, EventArgs e)
        {
            var br = _extensions.Get<FernBranch>();
            if (br != default)
            {
                br.Lmin = fern_Lmin.Value;
                fernLminLabel.Text = $@"Fern Lmin: {fern_Lmin.Value}";
            }
        }

        #endregion
    }
}