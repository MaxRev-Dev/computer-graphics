using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using GraphicExtensions;
using MaxRev.Extensions.Binary;
using MaxRev.Extensions.Matrix;
using Color = System.Drawing.Color;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Pen = System.Drawing.Pen;

namespace Playground
{
    public sealed partial class Main : Form
    {
        private Graphics _graphics;
        private Bitmap _bitmap;
        private float[,] matrix;
        private float[,] mainFigure;
        private Pen foregroundPen;
        private Point lastLocation;
        private float[,] coord;
        private bool _wordTransformOn;
        private bool _autoRotate;
        private float depth = 5;
        private readonly Queue<float[]> _lastpoints = new Queue<float[]>();
        private float _angle;
        private readonly StringBuilder _testOutputBuilder;
        private readonly StringWriter _testOutputWriter;
        private float minRotate => (float)Math.PI / 180;

        public Main()
        {
            InitializeComponent();
            CreatePlayground();
            InitModel();
            playground.MouseWheel += Playground_MouseWheel;
            playground.MouseClick += Playground_MouseClick;
            playground.Paint += Playground_Paint;
            _testOutputBuilder = new StringBuilder();
            _testOutputWriter = new StringWriter(_testOutputBuilder);
        }

        private void InitModel()
        {

            Reset();
            coord = MatrixExtensions.IdentityF(3);

            var _foreground = Color.Blue;
            foregroundPen = new Pen(_foreground, 1);

            var timer = new Timer { Interval = 50 };
            timer.Start();
            timer.Tick += OnFrame;
        }

        private void Playground_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(playground.BackColor);
            e.Graphics.DrawImage(_bitmap, 0, 0);
        }

        private void CreatePlayground()
        {
            _bitmap?.Dispose();
            _graphics?.Dispose();
            if (playground.Bounds.IsEmpty) return;
            _bitmap = new Bitmap(playground.Bounds.Width, playground.Bounds.Height);
            _graphics = Graphics.FromImage(_bitmap);
        }

        private void Playground_MouseClick(object sender, MouseEventArgs e)
        {
            lastLocation = e.Location;
        }
         
        private void OnFrame(object sender, EventArgs e)
        {
            if (_autoRotate)
            {
                /*matrix = matrix
                    */ /*.Multiply(RotZ(eps(1)))
                    .Multiply(RotX(eps(1)))*/ /*
                    .Multiply(RotY(eps(1)));*/
                //if (fov)
                //   
                //else
                //    _angle -= minRotate;
                //if (_angle >= Math.PI)
                //    fov = false;
                //if (_angle <= 0)
                //    fov = true;
                if (_angle >= Math.PI * 2)
                    _angle = 0;
                _angle += minRotate;
                CG.ApplyTransform(ref mainFigure, CG.TranslateEllipseXZ(Eps(3f), Eps(20), _angle));

                //matrix = matrix.Multiply(ScaleP(Eps(100f)));

            }

            if (CheckKeys() || _autoRotate)
            {
                Draw();
            }
            mainFigure.PrintThrough(_testOutputWriter);
            label1.Text = _testOutputBuilder.ToString();
            _testOutputBuilder.Clear();

        }

        private void Reset()
        {
            matrix = MatrixExtensions.IdentityF(4);
            mainFigure = new[,]
            {
                {1f, 1, 1, 1},
                {1f, -1, -1, 1},
                {-1f, 1, -1, 1},
                {-1f, -1, 1, 1},
               
            }.Transpose();
            depth = 100;
            CG.ApplyTransform(ref mainFigure, CG.TranslateZ(-10));
        }
        
        private bool CheckKeys()
        {
            float[,] trs = MatrixExtensions.IdentityF(4);
            bool modCtrl = Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Control);
            bool modAlt = Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Alt);
            var tens = Eps(1);
            if (IsPressed(Key.R))
            {
                Reset();
            }
            if (IsPressed(Key.OemPlus))
            {
                depth += 50f;
            }
            else if (IsPressed(Key.OemMinus))
            {
                depth -= 50f;
            }
            depthLabel.Text = depth.ToString("f2");
            if (IsPressed(Key.OemCloseBrackets))
            {
                trs = ScaleP(Eps(100));
            }
            else if (IsPressed(Key.OemOpenBrackets))
            {
                trs = ScaleP(Eps(90));
            }
            else if (IsPressed(Key.Up))
            {
                if (modAlt)
                    trs = CG.RotY(-tens);
                else if (modCtrl)
                    trs = CG.RotY(tens);
                else
                    trs = CG.TranslateY(tens);
            }
            else if (IsPressed(Key.Down))
            {
                if (modAlt)
                    trs = CG.RotZ(-tens);
                else if (modCtrl)
                    trs = CG.RotZ(tens);
                else
                    trs = CG.TranslateY(-tens);
            }
            else if (IsPressed(Key.Left))
            {
                if (modAlt)
                    trs = CG.RotX(-tens);
                else if (modCtrl)
                    trs = CG.RotX(tens);
                else
                    trs = CG.TranslateX(tens);
            }
            else if (IsPressed(Key.Right))
            {
                if (modAlt)
                    trs = CG.TranslateZ(-tens);
                else if (modCtrl)
                    trs = CG.TranslateZ(tens);
                else
                    trs = CG.TranslateX(-tens);
            }

            if (trs != default)
            {
                CG.ApplyTransform(ref mainFigure, trs);
            }

            return !(trs is null);
        }

        private bool IsPressed(Key key)
        {
            var left = Keyboard.GetKeyStates(key);
            return ((left & KeyStates.Down) != 0);
        }

        private void Playground_MouseWheel(object sender, MouseEventArgs e)
        {
            var delta = e.Delta;
            CG.ApplyTransform(ref mainFigure, ScaleP(eps(delta)));
            Draw();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            if (playground.Bounds.IsEmpty) return;
            _graphics.Clear(playground.BackColor);
            // mainFigure = matrix.Multiply(mainFigure);
            //var final2 = matrix.Multiply(MatrixExtensions.IdentityF(3).Multiply(100)).Multiply(coord);
            foreach (var final in new[] { mainFigure, /*final2*/})
                DrawFigure(final, foregroundPen);
            playground.Invalidate();
        }

        private void DrawFigure(float[,] vertices, Pen pen)
        {
            var w = _graphics.VisibleClipBounds.Width;
            var h = _graphics.VisibleClipBounds.Height; 

            var verts = mainFigure.GetLength(1);
            var figureCenter = CG.FigureCenter(mainFigure); 
            Func<float[], (float, float)> v = f => Translate2D(f[0], f[1], f[2]);
            _lastpoints.Enqueue(figureCenter);
            if (_lastpoints.Count > 500)
                _lastpoints.Dequeue();
            foreach (var vx in _lastpoints)
            {
                var (xr, yr) = v(vx);
                _graphics.DrawEllipse(Pens.Blue, xr, yr, 2, 2);
            }

            for (int index0 = 0; index0 < vertices.GetLength(0); index0++)
            {
                for (int index1 = index0 + 1; index1 < vertices.GetLength(1); index1++)
                {
                    var (x1, y1) =
                        Translate2D(vertices[0, index0], vertices[1, index0], vertices[2, index0]);
                    var (x2, y2) =
                        Translate2D(vertices[0, index1], vertices[1, index1], vertices[2, index1]);

                    try
                    {
                        _graphics.DrawLine(pen, x1, y1, x2, y2);
                    }
                    catch (OverflowException)
                    {
                    }
                }
            }

            /*for (int index0 = 0; index0 < vertices.GetLength(0) - 2; index0++)
            {
                var ver = vertices.GetCol(index0);
                var ver1 = vertices.GetCol(index0 + 1);
                var ver2 = vertices.GetCol(index0 + 2);
                var (x1, y1) = Translate2D(ver[0], ver[1], centerX, centerY);

                var points = new List<PointF>();
                points.Add(new PointF(x1, y1));
                _graphics.DrawPolygon(new Pen(Color.CadetBlue), points.Distinct().ToArray());
            }*/

        }
        private float[,] TransformBy(float[,] trs, float[,] rot)
        {
            return mainFigure
                  .Multiply(trs)
                  .Multiply(rot)
                  .Multiply(trs.Cast<float, double>().Inverse().Cast<double, float>());
        }


        private bool IsVisible(float[,] poly)
        {
            var z = new[] { 0f, 0, 1 };

            var v1 = poly.GetRow(0);
            var v2 = poly.GetRow(1);
            var v3 = poly.GetRow(2);
            var a = v1.Subtract(v2);
            var b = v1.Subtract(v3);
            var n = a.Cross(b);
            return v1.Opposite().Multiply(n) >= 0;
        }
         
        private float[,] ScaleP(float i) => new[,]
        {
            {i, 0, 0, 0},
            {0, i, 0, 0},
            {0, 0, i, 0},
            {0, 0, 0, 1,},
        };

        private float eps(float x) => (float)(x > 0 ? Math.PI / 300 : -Math.PI / 300);

        private float Eps(float x) => (float)(x * Math.PI / 300);

        private (float, float) Translate2D(float x, float y, float z)
        {
            var w = _graphics.VisibleClipBounds.Width;
            var h = _graphics.VisibleClipBounds.Height;

            var centerX = (w / 2f);
            var centerY = (h / 2f);

            return ((x * depth / z + centerX) * 1f, (y * depth / z + centerY) * 1f);
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
            }
        } 

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            CreatePlayground();
            playground.Invalidate();
        }
    }
}