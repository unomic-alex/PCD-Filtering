using System;
using System.Windows.Forms;
using AML = ArenaManager;
using Point = ArenaManager.Point;
using ChartDirector;


namespace BATMAN_WIN
{
	public partial class Form1 : Form
	{
		// Arena Manager 클래스 생성
		AML.MANAGER manager = new AML.MANAGER();


        // 3D view angles
        private double m_elevationAngle;
        private double m_rotationAngle;

        // Keep track of mouse drag
        private int m_lastMouseX;
        private int m_lastMouseY;
        private bool m_isDragging;

        /// <summary>
        /// 프로그램의 진입점 입니다.
        /// </summary>
        public Form1()
		{
			InitializeComponent();

			manager.ConnectDevice();

			manager.StartStream();
			manager.GetIImage();
			
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			m_elevationAngle = 30;
			m_rotationAngle = 45;

            m_isDragging = false;
            m_lastMouseX = -1;
            m_lastMouseY = -1;

            winChartViewer1.updateViewPort(true, false);
		}

		private void winChartViewer1_ViewPortChanged(object sender, WinViewPortEventArgs e)
		{
			// Update the chart if necessary
			if (e.NeedUpdateChart)
				drawChart((WinChartViewer)sender);
		}

		public void drawChart(WinChartViewer viewer)
		{
            Point[] data = manager.MakeTree();
            //Point[] data = manager.byteToFloatArray();
            //data = manager.YZFiltering(data);

            
			double[] x = new double[307200];
			double[] y = new double[307200];
			double[] z = new double[307200];

			for (int i = 0; i < data.Length; i++)
			{
				int index = i / 3;
				x[index] = data[i].x;
				y[index] = data[i].y;
				z[index] = data[i].z;
			}

            ThreeDScatterChart c = new ThreeDScatterChart(640, 480);
			c.addTitle("3D Scatter Chart");
			c.setSize(700, 700);

			c.setColorAxis(645, 270, Chart.Left, 200, Chart.Right);

			c.setPlotRegion(350, 350, 360, 360, 270);

			c.addScatterGroup(x, y, z, "", Chart.CircleShape, 11, Chart.SameAsMainColor);

			c.setViewAngle(m_elevationAngle, m_rotationAngle);

			c.xAxis().setTitle("X-Axis");
			c.yAxis().setTitle("Y-Axis");
			c.zAxis().setTitle("Z-Axis");

			//winChartViewer1.Chart = c;
			viewer.Chart = c;
		}

        private void winChartViewer1_MouseMoveChart(object sender, MouseEventArgs e)
        {
            int mouseX = winChartViewer1.ChartMouseX;
            int mouseY = winChartViewer1.ChartMouseY;

            // Drag occurs if mouse button is down and the mouse is captured by the m_ChartViewer
            if (((MouseButtons & MouseButtons.Left) != 0) && winChartViewer1.Capture)
            {
                if (m_isDragging)
                {
                    // The chart is configured to rotate by 90 degrees when the mouse moves from 
                    // left to right, which is the plot region width (360 pixels). Similarly, the
                    // elevation changes by 90 degrees when the mouse moves from top to button,
                    // which is the plot region height (270 pixels).
                    m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
                    m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
                    winChartViewer1.updateViewPort(true, false);
                }

                // Keep track of the last mouse position
                m_lastMouseX = mouseX;
                m_lastMouseY = mouseY;
                m_isDragging = true;
            }
        }
        private void winChartViewer1_MouseUpChart(object sender, MouseEventArgs e)
        {
            m_isDragging = false;
            winChartViewer1.updateViewPort(true, false);
        }

        private void winChartViewer1_Click(object sender, EventArgs e)
		{

		}

        /// <summary>
        /// 화면에서 "exit" 을 Click 했을 때 event 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, EventArgs e)
        {
            manager.StopStream();
            manager.DisconnectDevice();
            manager.CloseSystem();
        }
    }
}