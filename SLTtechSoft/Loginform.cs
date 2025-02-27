using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using AASTV_Auto_Test;
using ByYou;
using static ByYou.ApiMes;

namespace SLTtechSoft
{
    public partial class Loginform : Form
    {

        ApiMes apiMes;
        string URL = "";
        private (bool result, string msg, WO_Response data) _Get_WO;
        GetProductInfoResponse _productInfo = new GetProductInfoResponse();
        string LINE_INFOR = "";
        public Loginform()
        {
            InitializeComponent();
            InitialMES();
        }
        public void InitialMES ()
        {
            URL = txtURL.Text;
            apiMes = new ApiMes(URL);
        }
        public async void getempoy ()
        {
            string result = await apiMes.employeeAuthApi(txtUser.Text,txtPassWord.Text);
            txtResult.Text = result;
        }
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            getempoy();
            
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            LINE_INFOR = "LTEST"+"_"+"FFT1"+"_"+"1";
            if (!await GetPOInformation()) return;
        }
        public async Task<bool> GetPOInformation()
        {

            try
            {
                (bool result, string data) response = await apiMes.getProductInfoApi(LINE_INFOR);
                if (response.result == false)
                {
                    throw new Exception(response.data);
                }
                GetProductInfoResponse productInfoResponse = new GetProductInfoResponse();
                productInfoResponse = JsonSerializer.Deserialize<GetProductInfoResponse>(response.data);
                _productInfo = productInfoResponse;
                Global.WO = _productInfo.data.WONumber;
                bool woResult = await Get_WO(Global.WO);
                //Global.rule_sn = _productInfo.data.Product.SnRule;
                return true;
            }
            catch (Exception ex)
            {

                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                //MessageBox.Show($"[GetPOInformation][ERROR][Line {line}] {ex.Message}", "GetPOInformation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show($"Không tìm thấy WO được Active tại Line {Global.line}] {ex.Message}", "GetPOInformation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string workOder = Microsoft.VisualBasic.Interaction.InputBox("Vui lòng nhập WorkOder:", "Nhập WorkOder", "");

                if (!string.IsNullOrEmpty(workOder))
                {
                    bool woResult = await Get_WO(workOder);
                    if (woResult)
                    {
                        MessageBox.Show("WorkOder xử lý thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Không thể xử lý WorkOder, vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("WorkOder không hợp lệ, vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }

        }
        public async Task<bool> Get_WO(string WorkOder)
        {
            try
            {
                Global.WO = WorkOder;
                _Get_WO = await apiMes.Get_WO(Global.WO);
                if (_Get_WO.data.results == null || !_Get_WO.data.results.Any())
                {
                    throw new Exception($"Không tìm thấy WO: {Global.WO}");
                }
                if (_Get_WO.result == false)
                {
                    throw new Exception(_Get_WO.msg);
                }

                foreach (var tmp_part in _Get_WO.data.results)
                {
                    Global.SKU = tmp_part.Product;
                    Global.Rule_SN = tmp_part.NewCodeAASPV;
                    Global.Line = tmp_part.Line;
                    Global.Mes_Model = tmp_part.Model;
                    Global.Mes_Model_Name = tmp_part.ModelName;
                    Global.Q_ty = tmp_part.TotalQuantity;
                    Global.PO = tmp_part.PO;
                }
                return true;
                //if (success)
                //{
                //    SKUcode = WO_Response.results[0].Product;
                //    Global.rule_sn = WO_Response.results[1].NewCodeAASPV;
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                MessageBox.Show($"[GetPOInformation][ERROR][Line {line}] {ex.Message}", "GetPOInformation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
