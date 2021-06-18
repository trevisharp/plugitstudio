using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Model
{
    public unsafe class Picture : IDisposable
    {
        public bool Disposed { get; set; }
        public Bitmap Bitmap => bmp;
        public Graphics Graphics => g;
        public int Width => width;
        public int Height => height;
        public int Stride => stride;
        public byte* Pointer => (byte*)handler.AddrOfPinnedObject().ToPointer();

        private byte[] img;
        private Bitmap bmp;
        private GCHandle handler;
        private int width;
        private int height;
        private int stride;
        private Graphics g;

        public Picture(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.stride = 4 * width;
            this.img = new byte[stride * height];
            this.handler = GCHandle.Alloc(this.img, GCHandleType.Pinned);
            this.bmp = new Bitmap(width, height, stride, 
                PixelFormat.Format32bppArgb, handler.AddrOfPinnedObject());
            this.g = Graphics.FromImage(bmp);
        }
        
        public void Dispose()
        {
            if (Disposed)
                return;
            bmp.Dispose();
            handler.Free();
            Disposed = true;
        }

        public static implicit operator Bitmap(Picture painter)
            => painter.Bitmap;
    }
}