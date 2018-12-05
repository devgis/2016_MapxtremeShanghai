using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MapInfo.Mapping;
using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Tools;
using MapInfo.Mapping.Thematics;
using MapInfo.Styles;

namespace MapTestApp
{
    public partial class MapForm : Form
    {
        public Table XianTable = null;
        List<string> listBMPS = new List<string>();

        public MapForm()
        {
            InitializeComponent();
            mapControl1.Map.ViewChangedEvent += new MapInfo.Mapping.ViewChangedEventHandler(Map_ViewChangedEvent);
            Map_ViewChangedEvent(this, null);

            string CommonFilesPah = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
            string BMPPath = Path.Combine(CommonFilesPah, @"MapInfo\MapXtreme\6.8.0\CustSymb");

            string[] s = Directory.GetFiles(BMPPath, "*.BMP");
            foreach (string stemp in s)
            {
                listBMPS.Add(stemp.Substring(stemp.LastIndexOf("\\") + 1, stemp.Length - 1 - stemp.LastIndexOf("\\")));
            }

            mapControl1.Tools.FeatureSelected += new FeatureSelectedEventHandler(Tools_FeatureSelected);
        }

        void Tools_FeatureSelected(object sender, FeatureSelectedEventArgs e)
        {
            if (Session.Current.Selections.DefaultSelection.Count > 0)
            {
                Feature f = (Session.Current.Selections.DefaultSelection[0] as IResultSetFeatureCollection)[0];
                string Name = f["NAME"].ToString();
                string sql = string.Format("select * from t_ShanghaiData where 区县='{0}'", Name);
                DataTable dt = OleHelper.Instance.GetDataTable(sql);
                Session.Current.Selections.DefaultSelection.Clear();
                if (dt != null && dt.Rows.Count > 0)
                {
                    //MessageBox.Show(dt.Rows[0][0].ToString()+"存在数据!");
                    ChartsForm frmCharts = new ChartsForm(dt.Rows[0]);
                    frmCharts.Text = Name + "六普人口数据";
                    frmCharts.ShowDialog();
                }
                else
                {
                    MessageBox.Show(Name + "数据不存在！");
                }
                
            }
        }

        void Map_ViewChangedEvent(object sender, MapInfo.Mapping.ViewChangedEventArgs e)
        {
            // Display the zoom level
            Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
            if (statusStrip1.Items.Count > 0)
            {
                statusStrip1.Items[0].Text = "缩放: " + dblZoom.ToString() + " " + MapInfo.Geometry.CoordSys.DistanceUnitAbbreviation(mapControl1.Map.Zoom.Unit);
            }
        }

        private void MapForm1_Load(object sender, EventArgs e)
        {
            //加载地图
            string MapPath = Path.Combine(Application.StartupPath, @"map\map.mws");
            MapWorkSpaceLoader mwsLoader = new MapWorkSpaceLoader(MapPath);
            mapControl1.Map.Load(mwsLoader);
            mapControl1.Tools.LeftButtonTool = "Select";

            FeatureLayer fLayer = null;
            foreach (IMapLayer layer in mapControl1.Map.Layers)
            {
                if (layer is FeatureLayer && layer.Alias.Equals("县界"))
                {
                    fLayer = layer as FeatureLayer;
                    XianTable = fLayer.Table;
                    LayerHelper.SetSelectable(layer, true);
                }
                else
                {
                    LayerHelper.SetSelectable(layer, false);
                }
            }
        }

        DataRow drSerachResult = null;

        private void btSearch_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbXianName.Text.Trim()))
            {
                MessageBox.Show("请输入县名！");
                tbXianName.Focus();
            }
            else
            {
                String Where = "NAME like '%" + tbXianName.Text + "%'";
                SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchWhere(Where);
                si.QueryDefinition.Columns = null;
                IResultSetFeatureCollection ifs = MapInfo.Engine.Session.Current.Catalog.Search(XianTable, si);
                if (ifs == null || ifs.Count <= 0)
                {
                    MessageBox.Show("没有符合查询条件的结果！");
                }
                else
                {
                    Feature f = ifs[0];
                    string name = f["NAME"].ToString();
                    mapControl1.Map.Bounds = f.Geometry.Bounds;
                    Session.Current.Selections.DefaultSelection.Clear();
                    Session.Current.Selections.DefaultSelection.Add(ifs);
                }
            }
        }

        private void btReport_Click(object sender, EventArgs e)
        {
            ReportForm frmReport = new ReportForm();
            frmReport.ShowDialog();
        }
    }
}
