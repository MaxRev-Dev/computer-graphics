using GraphicExtensions;
using MaxRev.Extensions.Binary;
using MaxRev.Extensions.Matrix;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private Graphics _graphics;
        private Bitmap _bitmap;
        private float[,] mainFigure;
        private float[,] axis;
        private float[,] worldToCamera;
        private float[,] projMatrix;
        private Pen foregroundPen;
        private bool _wordTransformOn;
        private bool _autoRotate;
        private bool _moveModel;
        private float depth = 5;
        private readonly Queue<float[]> _lastpoints = new Queue<float[]>();
        private float _angle;
        private readonly StringBuilder _testOutputBuilder;
        private readonly StringWriter _testOutputWriter;
        private Pen invisiblePen;
        private Color _background;
        private Color _foreground;
        private int[][] faces;
        private float angleOfView;
        private float near;
        private float far;
        private float minRotate = 0.01745329F; // Math.PI / 180

        #endregion

        #region Properties

        private float drawableWidth => _graphics.VisibleClipBounds.Width;
        private float drawableHeight => _graphics.VisibleClipBounds.Height;

        #endregion

        public Main()
        {
            InitializeComponent();
            CreatePlayground();
            InitModel();
            playground.MouseWheel += Playground_MouseWheel;
            playground.Paint += (s, e) => e.Graphics.DrawImage(_bitmap, 0, 0);
            _testOutputBuilder = new StringBuilder();
            _testOutputWriter = new StringWriter(_testOutputBuilder);
        }

        #region Reset

        private void InitModel()
        {
            _foreground = Color.Blue;
            _background = Color.DarkRed;
            foregroundPen = new Pen(_foreground, 1);
            invisiblePen = new Pen(_background, 1);

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
            ResetWorld();
        }

        private void ResetModel()
        {
            mainFigure = new[,]
            {
                {0f, -1, 0,1}, //0 A 
                {-1f, 0, 0,1}, //1 B
                {0f, 1, 0,1}, //2 C
                {1f, 0, 0,1}, //3 D
                {0f, 0, 1,1}, //4 E
            };
            axis = new[,]
            {
                {0f, 0, 0 ,1},
                {1f, 0, 0 ,1},
                {0, 1, 0 ,1},
                {0, 0, 1, 1 }
            };
            faces = new[]
            {
                new[] {0, 1, 2, 3}, // ABCD
                new[] {1, 4, 0}, // BEA
                new[] {0, 4, 3}, // AED
                new[] {2, 4, 1}, // CEB
                new[] {3, 4, 2} // DEC
            };
            depth = 100;
            Transform(CG.TranslateZ(10));
        }

        private void ResetWorld()
        {
            // mainFigure = CG.ApplyTransform(mainFigure, CG.TranslateZ(-10));
            worldToCamera = MatrixExtensions.IdentityF(4);
            angleOfView = 90;
            near = 0.1f;
            far = 100;
            projMatrix = MatrixExtensions.IdentityF(4);
            worldToCamera[3, 1] = -10;
            worldToCamera[3, 2] = -20;
            // CG.setProjectionMatrix(projMatrix, angleOfView, near, far);
        }

        #endregion

        private void Transform(float[,] trs)
        {
            mainFigure = CG.ApplyTransform(mainFigure, trs);
            axis = CG.ApplyTransform(axis, trs);
        }

        private void CreatePlayground()
        {
            _bitmap?.Dispose();
            _graphics?.Dispose();
            if (playground.Bounds.IsEmpty) return;
            _bitmap = new Bitmap(playground.Bounds.Width, playground.Bounds.Height);
            _graphics = Graphics.FromImage(_bitmap);
        }

        private void OnFrame(object sender, EventArgs e)
        {
            CG.setProjectionMatrix(projMatrix, angleOfView, near, far);
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

            projMatrix.PrintThrough(_testOutputWriter);
            mainFigure.PrintThrough(_testOutputWriter);
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
            float scale = (float)(Math.Tan(angOfView * 0.5 * Math.PI / 180) * n);
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
            float[,] trs = MatrixExtensions.IdentityF(4);
            bool modCtrl = Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Control);
            bool modAlt = Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Alt);
            var tens = Eps(1) * 10;
            if (IsPressed(Key.R))
            {
                ResetModel();
                ResetWorld();
            }

            if (IsPressed(Key.OemPlus))
            {
                if (IsPressed(Key.LeftCtrl))
                {
                    far += 10;
                }
                else
                    near += .1f;
            }
            else if (IsPressed(Key.OemMinus))
            {
                if (IsPressed(Key.LeftCtrl))
                {
                    far -= 10;
                }
                else
                    near -= .1f;
            }

            depthLabel.Text = $@"F:{far:f2} N:{near:f2}";

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

        private void Playground_MouseWheel(object sender, MouseEventArgs e)
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

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            if (playground.Bounds.IsEmpty) return;
            _graphics.Clear(playground.BackColor);
            DrawFigure(foregroundPen);
            playground.Invalidate();
        }

        private void DrawFigure(Pen pen)
        {
            HistoryPath(mainFigure);

            for (int faceIndex = 0; faceIndex < faces.Length; faceIndex++)
            {
                var path = new List<(float x, float y)>();
                var face = GetFace(faceIndex).ToArray();
                foreach (var edge in face)
                {
                    var vertCamera = edge.ToArray().Multiply(worldToCamera);
                    var projectedVert = vertCamera.Multiply(projMatrix).ToPoint();
                    if (TryDropVertex(projectedVert))
                        continue;
                    path.Add(PointToScreen(projectedVert.x, projectedVert.y));
                }

                var isVisible = IsVisibleFace(face);

                for (int i = 0; i < path.Count; i++)
                    for (int j = i + 1; j < path.Count; j++)
                        _graphics.DrawLine(isVisible ? pen : invisiblePen, path[i].x, path[i].y,
                            path[j].x, path[j].y);
                /*var points = path.Select(x => new PointF(x.x, x.y)).ToArray();
                _graphics.FillPolygon(isVisible ? Brushes.PaleGreen : Brushes.Aquamarine, points);*/
            }

            DrawAxis();
        }

        private void DrawAxis()
        {
            var colors = new[]
            {
                Pens.Red,
                Pens.Green,
                Pens.Yellow
            };
            for (int i = 1; i < axis.GetLength(0); i++)
            {
                var projectedVert = ProjectToScreen(axis.point(i));
                var projectedVert2 = ProjectToScreen(axis.point(0));
                if (TryDropVertex(projectedVert) ||
                    TryDropVertex(projectedVert2))
                    continue;
                var (x1, y1) =
                    PointToScreen(projectedVert.x, projectedVert.y);
                var (x2, y2) =
                    PointToScreen(projectedVert2.x, projectedVert2.y);
                _graphics.DrawLine(colors[i - 1], new PointF(x1, y1), new PointF(x2, y2));
            }
        }

        private (float x, float y, float z) ProjectToScreen((float x, float y, float z) point1)
        {
            var vertCamera = point1.ToArray().Multiply(worldToCamera);
            var projectedVert = vertCamera.Multiply(projMatrix).ToPoint();
            return projectedVert;
        }

        private IEnumerable<(float x, float y, float z)> GetFace(int faceIndex)
        {
            for (int i = 0; i < faces[faceIndex].Length; i++)
            {
                yield return mainFigure.point(faces[faceIndex][i]);
            }
        }

        private bool TryDropVertex((float x, float y, float z) projectedVert)
        {
            // out of visible screen bounds
            return projectedVert.x < -1 ||
                   projectedVert.x > 1 ||
                   projectedVert.y < -1 ||
                   projectedVert.y > 1;
        }

        private (float x, float y) PointToScreen(float argX, float argY)
        {
            return ((argX + 1) * 0.5f * drawableWidth,
                (argY + 1) * 0.5f * drawableHeight);
        }

        private void HistoryPath(float[,] vertices)
        {
            Func<float[], (float, float)> v = f => PointToScreen(f[0], f[1]);

            var figureCenter = CG.FigureCenter(vertices);
            _lastpoints.Enqueue(figureCenter);
            if (_lastpoints.Count > 500)
                _lastpoints.Dequeue();
            foreach (var vx in _lastpoints)
            {
                var vertCamera = vx.Multiply(worldToCamera);
                var projectedVert = vertCamera.Multiply(projMatrix).ToPoint();
                if (TryDropVertex(projectedVert)) continue;
                var (xr, yr) = v(projectedVert.ToArray());
                _graphics.DrawEllipse(Pens.Blue, xr, yr, 2, 2);
            }
        }

        private float[] viewVector => new[] { 0f, -1, 0 };

        private bool IsVisibleFace((float x, float y, float z)[] edge)
        {
            /*
             * [x1   x2 x3]
             * [y1   y2 y3]
             * [z1   z2 z3] 
             */
            // transposed matrix
            var v1 = edge[0].ToArray(); // edge1
            var v2 = edge[1].ToArray(); // edge2
            var v3 = edge[2].ToArray(); // edge3
            var a = v1.Subtract(v2);
            var b = v1.Subtract(v3);
            var n = a.Cross(b).Normalize();
            return viewVector.Dot(n) > 0;
        }

     /*   private float[,] TransformBy(float[,] trs, float[,] rot)
        {
            return mainFigure
                  .Multiply(trs)
                  .Multiply(rot)
                  .Multiply(trs.Cast<float, double>().Inverse().Cast<double, float>());
        }
*/
        private float eps(float x) => (float)(x > 0 ? Math.PI / 300 : -Math.PI / 300);

        private float Eps(float x) => (float)(x * Math.PI / 300);
    }
}