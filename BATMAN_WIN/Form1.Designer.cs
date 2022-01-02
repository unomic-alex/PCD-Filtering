
namespace BATMAN_WIN
{
	partial class Form1
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		


		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            this.exit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // winChartViewer1
            // 
            this.winChartViewer1.ChartSizeMode = ChartDirector.WinChartSizeMode.StretchImage;
            this.winChartViewer1.Location = new System.Drawing.Point(82, 26);
            this.winChartViewer1.Name = "winChartViewer1";
            this.winChartViewer1.Size = new System.Drawing.Size(922, 542);
            this.winChartViewer1.TabIndex = 2;
            this.winChartViewer1.TabStop = false;
            this.winChartViewer1.ViewPortChanged += new ChartDirector.WinViewPortEventHandler(this.winChartViewer1_ViewPortChanged);
            this.winChartViewer1.Click += new System.EventHandler(this.winChartViewer1_Click);
            
            this.winChartViewer1.MouseMoveChart += new System.Windows.Forms.MouseEventHandler(this.winChartViewer1_MouseMoveChart);
            this.winChartViewer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.winChartViewer1_MouseUpChart);
            //this.winChartViewer1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.winChartViewer1_MouseDown);
            //this.winChartViewer1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.winChartViewer1_MouseMove);
            //this.winChartViewer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.winChartViewer1_MouseUp);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(1010, 545);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(75, 23);
            this.exit.TabIndex = 3;
            this.exit.Tag = "";
            this.exit.Text = "exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 598);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.winChartViewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private ChartDirector.WinChartViewer winChartViewer1;
        private System.Windows.Forms.Button exit;
    }
}

