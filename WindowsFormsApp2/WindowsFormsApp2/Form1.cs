using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
//hello world 123
namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        // Danh sách cạnh (u, v, w)
        List<(int u, int v, int w)> edges = new List<(int, int, int)>();
        int[,] pos; // vị trí các đỉnh
        int n = 5;  // số đỉnh

        public Form1()
        {
            InitializeComponent();
            pos = new int[n, 2];

            // Tạo vị trí ngẫu nhiên cho 5 đỉnh
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                pos[i, 0] = rnd.Next(50, panelDraw.Width - 50);
                pos[i, 1] = rnd.Next(50, panelDraw.Height - 50);
            }

            // Khởi tạo các cạnh (đồ thị mẫu)
            edges.Add((0, 1, 2));
            edges.Add((0, 2, 3));
            edges.Add((1, 2, 1));
            edges.Add((1, 3, 4));
            edges.Add((2, 4, 5));
            edges.Add((3, 4, 2));
        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            Graphics g = panelDraw.CreateGraphics();
            g.Clear(Color.White);

            // Vẽ tất cả cạnh (màu xám)
            Pen penGray = new Pen(Color.Gray, 1);
            foreach (var edge in edges)
            {
                g.DrawLine(penGray,
                    pos[edge.u, 0], pos[edge.u, 1],
                    pos[edge.v, 0], pos[edge.v, 1]);
            }

            // Kruskal
            edges.Sort((a, b) => a.w.CompareTo(b.w));
            int[] parent = Enumerable.Range(0, n).ToArray();

            int Find(int x)
            {
                if (parent[x] == x) return x;
                return parent[x] = Find(parent[x]);
            }

            void Union(int x, int y)
            {
                parent[Find(x)] = Find(y);
            }

            Pen penRed = new Pen(Color.Red, 3);
            int total = 0;

            foreach (var edge in edges)
            {
                if (Find(edge.u) != Find(edge.v))
                {
                    Union(edge.u, edge.v);
                    total += edge.w;

                    // Vẽ cạnh chọn (đỏ)
                    g.DrawLine(penRed,
                        pos[edge.u, 0], pos[edge.u, 1],
                        pos[edge.v, 0], pos[edge.v, 1]);
                    System.Threading.Thread.Sleep(500);
                }
            }

            // Vẽ lại các đỉnh
            foreach (int i in Enumerable.Range(0, n))
            {
                g.FillEllipse(Brushes.LightBlue, pos[i, 0] - 10, pos[i, 1] - 10, 20, 20);
                g.DrawString(i.ToString(), new Font("Arial", 10), Brushes.Black, pos[i, 0] - 10, pos[i, 1] - 25);
            }

            MessageBox.Show("Tổng chi phí = " + total);
        }
    }
}
