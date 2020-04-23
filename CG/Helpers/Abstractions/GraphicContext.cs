using System.Drawing;

namespace Playground.Helpers.Abstractions
{
    internal class GraphicContext
    {
        private Graphics _graphics;

        public void Reset(Bitmap bitmap)
        {
            if (bitmap == default) return;
            Bitmap = bitmap;
            _graphics?.Dispose();
            _graphics = Graphics.FromImage(bitmap);
        }

        public Graphics Graphics => _graphics ??= Graphics.FromImage(Bitmap);

        public Bitmap Bitmap { get; private set; }
    }
}