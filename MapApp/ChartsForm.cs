using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace MapTestApp
{
    public partial class ChartsForm : Form
    {
        private DataRow drPata;  //传入的县区数据

        public ChartsForm(DataRow DrPata)
        {
            InitializeComponent();
            this.drPata = DrPata;
        }

        private void ChartsForm_Load(object sender, EventArgs e)
        {
            LoadChart();
        }

        private void LoadChart()
        {
            if (drPata == null || drPata.ItemArray.Length <= 0)
                return;

            LoadChart0(); //加载柱图
            LoadTotalPersonData(); //合计人口
            LoadTotalDomicileData(); //合计户籍数
            LoadTotalHomePersonData(); //家庭户
            LoadTotalCollectivePersonData(); //集体户

        }

        /// <summary>
        /// 加载柱图
        /// </summary>
        private void LoadChart0()
        {
            #region 无用
            //try
            //{
            //    PieItem CHI1 = zedGraphControl0.GraphPane.CurveList[0] as PieItem;
            //    IPointListEdit CH1List = CHI1.Points as IPointListEdit;
            //    CH1List.Clear();
            //}
            //catch
            //{ }
            //GraphPane myPane;
            //myPane = zedGraphControl0.GraphPane;
            //myPane.Title.Text = "人口柱图";
            //myPane.Title.FontSpec.Size = 20;
            //myPane.YAxis.Title.Text = string.Empty;
            //myPane.XAxis.Title.Text = string.Empty;
            ////myPane.YAxis.Title.Text = "数量";
            ////myPane.YAxis.Title.FontSpec.Size = 18;
            ////myPane.YAxis.Scale.FontSpec.Size = 15;

            //myPane.Legend.Border.Color = Color.White; //去掉图例边框
            //myPane.Legend.FontSpec.Size = 18; //图例字体大小

            //double[] Values = new double[2];
            //string[] Labels = new string[2];
            ////男女人口
            ////Labels[0] = "男性人数" + Convert.ToInt32(drPata["男人口"].ToString().Trim());
            ////Values[0] = Convert.ToInt32(drPata["男人口"].ToString().Trim());
            ////CH1List.Add(new PointPair{ }
            ////Labels[1] = "女性人数" + Convert.ToInt32(drPata["女人口"].ToString().Trim());
            ////Values[1] = Convert.ToInt32(drPata["女人口"].ToString().Trim());

            //BarItem myCurve;
            //myCurve = myPane.AddBar("男性人数", new double[2] { 1, 2 }, new double[2] { Convert.ToDouble(drPata["男人口"].ToString().Trim()), Convert.ToDouble(drPata["女人口"].ToString().Trim()) }, Color.Red);  
            //myCurve.Bar.Fill = new Fill(Color.Red, Color.White, Color.Red);

            //myCurve = myPane.AddBar("女性人数", new double[1] { 2 }, new double[1] { Convert.ToDouble(drPata["女人口"].ToString().Trim()) }, Color.Red);
            //myCurve.Bar.Fill = new Fill(Color.Red, Color.White, Color.Red);  
  
            ////myCurve = myPane.AddBar("占有率", null, dOcc, Color.Blue);  
            ////myCurve.Bar.Fill = new Fill(Color.Blue, Color.White, Color.Blue);  
  
            ////myCurve = myPane.AddBar("车流量", null, dSpeed, Color.Green);  
            ////myCurve.Bar.Fill = new Fill(Color.Green, Color.White, Color.Green);  


            ////myPane.AddBar("sss", CH1List, Color.Red);
            #endregion

            #region 柱图
            string[] tzTypeArr = new string[8]{"家庭户数","集体户数","男性总人口","女性总人口","集体户男","集体户女","家庭户男","家庭户女"};
            Dictionary<int, double[]> dict = new Dictionary<int, double[]>();
            Color[] colorArr ={ Color.Orange,Color.PaleGreen,Color.SlateBlue,
                              Color.Pink,Color.Green,Color.LightSkyBlue,
                              Color.Gray,Color.GreenYellow,Color.RosyBrown};

            this.zedGraphControl0.SuspendLayout();

            this.zedGraphControl0.Controls.Clear();
            ZedGraph.GraphPane myPane = this.zedGraphControl0.GraphPane;

            myPane.Title.Text = "本区人口数据柱图";
            myPane.XAxis.Title.Text = "";
            myPane.YAxis.Title.Text = "";

            int start_x = 5;
            double tzZhz = 0;
            for (int i = 0; i < tzTypeArr.Length; i++)
            {
                //tzZhz = tzZhfArr[i] < 0 ? 0 : tzZhfArr[i];
                tzZhz = 10;
                double y = 0;
                switch (i)
                {
                    case 0:
                        y = Convert.ToDouble(drPata["家庭户户数"]);
                        break;
                    case 1:
                        y = Convert.ToDouble(drPata["集体户户数"]);
                        break;
                    case 2:
                        y = Convert.ToDouble(drPata["男人口"]);
                        break;
                    case 3:
                        y = Convert.ToDouble(drPata["女人口"]);
                        break;
                    case 4:
                        y = Convert.ToDouble(drPata["集体户男"]);
                        break;
                    case 5:
                        y = Convert.ToDouble(drPata["集体户女"]);
                        break;
                    case 6:
                        y = Convert.ToDouble(drPata["男家庭户人口"]);
                        break;
                    case 7:
                        y = Convert.ToDouble(drPata["女家庭户人口"]);
                        break;
                }
                ZedGraph.BarItem bar = myPane.AddBar(tzTypeArr[i], new double[] { start_x }, new double[] { y }, colorArr[i]);
                bar.Bar.Fill = new ZedGraph.Fill(colorArr[i], Color.White, colorArr[i]);
                bar.Label = new ZedGraph.Label(tzTypeArr[i], new FontSpec() { Size = 20 });

                //ZedGraph.TextObj myText = new ZedGraph.TextObj(string.Format("{0},{1}", tzTypeArr[i], tzZhz), start_x, 3);
                //myText.Location.CoordinateFrame = ZedGraph.CoordType.AxisXYScale;
                //myText.Location.AlignH = ZedGraph.AlignH.Center;
                //myText.Location.AlignV = ZedGraph.AlignV.Center;
                //myText.FontSpec.Family = "宋体";
                //myText.FontSpec.Size = 16f;
                //myText.FontSpec.Fill.IsVisible = false;
                //myText.FontSpec.Border.IsVisible = false;
                ////myText.FontSpec.Angle = 35;//控制 文字 倾斜度
                //myPane.GraphObjList.Add(myText);
                start_x += 15;
            }

            myPane.Fill = new ZedGraph.Fill(Color.WhiteSmoke, Color.Lavender, 0F);
            myPane.Chart.Fill = new ZedGraph.Fill(Color.FromArgb(255, 255, 245),
                        Color.FromArgb(255, 255, 190), 90F);
            //设置 柱体的宽度
            myPane.BarSettings.ClusterScaleWidth = 30;
            // Bars are stacked
            myPane.BarSettings.Type = ZedGraph.BarType.Cluster;

            myPane.XAxis.IsVisible = false;

            // Enable the X and Y axis grids
            myPane.XAxis.MajorGrid.IsVisible = false;
            myPane.YAxis.MajorGrid.IsVisible = true;


            myPane.XAxis.Scale.Min = -10;
            this.zedGraphControl0.ResumeLayout();
            this.zedGraphControl0.AxisChange();

            #endregion
        }

        /// <summary>
        /// 合计人口（男xx 女xx 合计xx）
        /// </summary>
        private void LoadTotalPersonData()
        {
            try
            {
                PieItem CHI1 = zedGraphControl2.GraphPane.CurveList[0] as PieItem;
                IPointListEdit CH1List = CHI1.Points as IPointListEdit;
                CH1List.Clear();
            }
            catch
            { }
            GraphPane myPane;
            myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "合计人口";
            myPane.Title.FontSpec.Size = 20;
            myPane.YAxis.Title.Text = string.Empty;
            myPane.XAxis.Title.Text = string.Empty;
            //myPane.YAxis.Title.Text = "数量";
            //myPane.YAxis.Title.FontSpec.Size = 18;
            //myPane.YAxis.Scale.FontSpec.Size = 15;

            myPane.Legend.Border.Color = Color.White; //去掉图例边框
            myPane.Legend.FontSpec.Size = 18; //图例字体大小

            double[] Values = new double[2];
            string[] Labels = new string[2];
            //男女人口
            Labels[0] = "男性人数" + Convert.ToInt32(drPata["男人口"].ToString().Trim());
            Values[0] = Convert.ToInt32(drPata["男人口"].ToString().Trim());

            Labels[1] = "女性人数" + Convert.ToInt32(drPata["女人口"].ToString().Trim());
            Values[1] = Convert.ToInt32(drPata["女人口"].ToString().Trim());

            label1.Text = string.Format("合计人口（男{0} 女{1} 合计{2}）"
                , Convert.ToInt32(drPata["男人口"].ToString().Trim())
                , Convert.ToInt32(drPata["女人口"].ToString().Trim())
                , Convert.ToInt32(drPata["合计人口"].ToString().Trim()));
            myPane.AddPieSlices(Values, Labels);
        }

        /// <summary>
        /// 合计户籍数（集体户xx 家庭户xx 合计xx）
        /// </summary>
        private void LoadTotalDomicileData()
        {
            try
            {
                PieItem CHI1 = zedGraphControl2.GraphPane.CurveList[0] as PieItem;
                IPointListEdit CH1List = CHI1.Points as IPointListEdit;
                CH1List.Clear();
            }
            catch
            { }
            GraphPane myPane;
            myPane = zedGraphControl2.GraphPane;
            myPane.Title.Text = "合计户籍数";
            myPane.Title.FontSpec.Size = 20;
            myPane.YAxis.Title.Text = string.Empty;
            myPane.XAxis.Title.Text = string.Empty;
            //myPane.YAxis.Title.Text = "数量";
            //myPane.YAxis.Title.FontSpec.Size = 18;
            //myPane.YAxis.Scale.FontSpec.Size = 15;

            myPane.Legend.Border.Color = Color.White; //去掉图例边框
            myPane.Legend.FontSpec.Size = 18; //图例字体大小

            double[] Values = new double[2];
            string[] Labels = new string[2];
            //男女人口
            Labels[0] = "家庭户户数" + Convert.ToInt32(drPata["家庭户户数"].ToString().Trim());
            Values[0] = Convert.ToInt32(drPata["家庭户户数"].ToString().Trim());

            Labels[1] = "集体户户数" + Convert.ToInt32(drPata["集体户户数"].ToString().Trim());
            Values[1] = Convert.ToInt32(drPata["集体户户数"].ToString().Trim());

            label2.Text = string.Format("合计户籍数（集体户{0} 家庭户{1} 合计{2}）"
                , Convert.ToInt32(drPata["家庭户户数"].ToString().Trim())
                , Convert.ToInt32(drPata["集体户户数"].ToString().Trim())
                , Convert.ToInt32(drPata["合计户数"].ToString().Trim()));
            myPane.AddPieSlices(Values, Labels);
        }

        /// <summary>
        /// 家庭户人口（男xx 女xx 合计xx）
        /// </summary>
        private void LoadTotalHomePersonData()
        {
            try
            {
                PieItem CHI1 = zedGraphControl2.GraphPane.CurveList[0] as PieItem;
                IPointListEdit CH1List = CHI1.Points as IPointListEdit;
                CH1List.Clear();
            }
            catch
            { }
            GraphPane myPane;
            myPane = zedGraphControl3.GraphPane;
            myPane.Title.Text = "家庭户人口";
            myPane.Title.FontSpec.Size = 20;
            myPane.YAxis.Title.Text = string.Empty;
            myPane.XAxis.Title.Text = string.Empty;
            //myPane.YAxis.Title.Text = "数量";
            //myPane.YAxis.Title.FontSpec.Size = 18;
            //myPane.YAxis.Scale.FontSpec.Size = 15;

            myPane.Legend.Border.Color = Color.White; //去掉图例边框
            myPane.Legend.FontSpec.Size = 18; //图例字体大小

            double[] Values = new double[2];
            string[] Labels = new string[2];
            //男女人口
            Labels[0] = "男性人数" + Convert.ToInt32(drPata["男家庭户人口"].ToString().Trim());
            Values[0] = Convert.ToInt32(drPata["男家庭户人口"].ToString().Trim());

            Labels[1] = "女性人数" + Convert.ToInt32(drPata["女家庭户人口"].ToString().Trim());
            Values[1] = Convert.ToInt32(drPata["女家庭户人口"].ToString().Trim());

            label3.Text = string.Format("家庭户人口（男{0} 女{1} 合计{2}）"
                , Convert.ToInt32(drPata["男家庭户人口"].ToString().Trim())
                , Convert.ToInt32(drPata["女家庭户人口"].ToString().Trim())
                , Convert.ToInt32(drPata["家庭户人口小计"].ToString().Trim()));
            myPane.AddPieSlices(Values, Labels);
        }

        /// <summary>
        /// 集体户人口（男xx 女xx 合计xx）
        /// </summary>
        private void LoadTotalCollectivePersonData()
        {
            try
            {
                PieItem CHI1 = zedGraphControl2.GraphPane.CurveList[0] as PieItem;
                IPointListEdit CH1List = CHI1.Points as IPointListEdit;
                CH1List.Clear();
            }
            catch
            { }
            GraphPane myPane;
            myPane = zedGraphControl4.GraphPane;
            myPane.Title.Text = "集体户人口";
            myPane.Title.FontSpec.Size = 20;
            myPane.YAxis.Title.Text = string.Empty;
            myPane.XAxis.Title.Text = string.Empty;
            //myPane.YAxis.Title.Text = "数量";
            //myPane.YAxis.Title.FontSpec.Size = 18;
            //myPane.YAxis.Scale.FontSpec.Size = 15;

            myPane.Legend.Border.Color = Color.White; //去掉图例边框
            myPane.Legend.FontSpec.Size = 18; //图例字体大小

            double[] Values = new double[2];
            string[] Labels = new string[2];
            //男女人口
            Labels[0] = "男性人数" + Convert.ToInt32(drPata["集体户男"].ToString().Trim());
            Values[0] = Convert.ToInt32(drPata["集体户男"].ToString().Trim());

            Labels[1] = "女性人数" + Convert.ToInt32(drPata["集体户女"].ToString().Trim());
            Values[1] = Convert.ToInt32(drPata["集体户女"].ToString().Trim());

            label4.Text = string.Format("集体户人口（男{0} 女{1} 合计{2}）"
                , Convert.ToInt32(drPata["集体户男"].ToString().Trim())
                , Convert.ToInt32(drPata["集体户女"].ToString().Trim())
                , Convert.ToInt32(drPata["集体户小计"].ToString().Trim()));
            myPane.AddPieSlices(Values, Labels);
        }
    }
}