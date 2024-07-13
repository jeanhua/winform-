using System.Drawing.Drawing2D;

namespace stewie
{
    public partial class Form1 : Form
    {
        private Point m_mousePos;
        private bool m_isMouseDown;
        private ContextMenuStrip menuStrip;
        public Form1()
        {
            InitializeComponent();
            //隐藏标题栏
            this.FormBorderStyle = FormBorderStyle.None;
            //窗口移动控制
            pictureBg.MouseDown += OnMouseDown;
            pictureBg.MouseUp += OnMouseUp;
            pictureBg.MouseMove += OnMouseMove;

            //背景图大小设置
            pictureBg.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBg.Parent = this;
            pictureBg.Location = new Point(0, 0);
            //背景图透明设置
            Bitmap img = (Bitmap)pictureBg.Image;
            var grapth = GetNoneTransparentRegion(img, 100);
            this.Region = new Region(grapth);
            this.BackgroundImage = pictureBg.Image;
            this.BackgroundImageLayout = ImageLayout.Zoom;
            //窗口最前
            this.TopMost = true;
            //右键菜单
            menuStrip = new ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("exit");
            menuStrip.Items.Add(toolStripMenuItem);
            toolStripMenuItem.Click += ExitApp;
            this.ContextMenuStrip = menuStrip;

        }

        protected void ExitApp(object sender,EventArgs e)
        {
            Application.Exit();
        }

        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;
        }

        protected void OnMouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
            this.Focus();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }

        public GraphicsPath GetNoneTransparentRegion(Bitmap img, byte alpha)
        {
            int height = img.Height;
            int width = img.Width;

            int xStart, xEnd;
            GraphicsPath grpPath = new GraphicsPath();
            for (int y = 0; y < height; y++)
            {
                //逐行扫描；
                for (int x = 0; x < width; x++)
                {
                    //略过连续透明的部分；
                    while (x < width && img.GetPixel(x, y).A <= alpha)
                    {
                        x++;
                    }
                    //不透明部分；
                    xStart = x;
                    while (x < width && img.GetPixel(x, y).A > alpha)
                    {
                        x++;
                    }
                    xEnd = x;
                    if (img.GetPixel(x - 1, y).A > alpha)
                    {
                        grpPath.AddRectangle(new Rectangle(xStart, y, xEnd - xStart, 1));
                    }
                }
            }
            return grpPath;
        }
    }
}
