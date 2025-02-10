using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;
//using ControllLayer;
//using Seagull.BarTender.Print;
using System.Net;
using System.IO;

namespace ByYou
{
    /* Usage
        private ApiMes mApiMes = new ApiMes(Properties.Settings.Default.Urlstring);
        
       Url format: http://127.0.0.1:8000/
        
     */
    // Alan Add Class to control MES API 20230310
    public class ApiMes
    {
        public String root_url = "";
        public static String URL_CHECKSN_API = "/shopfloor/productsn/CheckSN/";
        public static String URL_REQUEST_NEXT_SN_API = "/shopfloor/api/requestNextSn/";

        public static String URL_SENDRECVDATASFC = "/shopfloor/api/sendrecvdatasfc/";

        public static String URL_EMPLOYEEAUTH = "/shopfloor/employees/EmployeeAuth/";
        public static String URL_QCAUTH = "/shopfloor/employees/EmployeeQCAuth/";
        public static String URL_IsNeedFirstSN = "/shopfloor/workoders/IsNeedFirstSN/";


        public ApiMes(string root_url="")
        {
            this.root_url = root_url;
        }

        public async Task<string> checkSnApi(string sn, string line, string station, string index, string user = "")
        {
            String msg = "PASS";
            int count = 0;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_CHECKSN_API);
            CheckSnSend dataSend = new CheckSnSend();
            dataSend.SerialNumber = sn;
            dataSend.Station = station;
            dataSend.Line = line;
            dataSend.Index = index;
            dataSend.User = user;
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    if (response.IsSuccessStatusCode)
                    {
                        msg = "PASS";
                    }
                    else
                    {
                        msg = "[checkSnApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[checkSnApi][ERROR]: " + ex.Message;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }
            return msg;
        }

        public async Task<string> requestNextSnApi(RequestNexSnSend dataSend)
        {
            String msg = "PASS";
            int count = 0;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_REQUEST_NEXT_SN_API);
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    if (response.IsSuccessStatusCode)
                    {
                        msg = "PASS";
                    }
                    else
                    {
                        msg = "[requestNextSnApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    count++;
                    msg = "[checkSnApi][ERROR]: " + ex.Message;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }


            return msg;
        }

        public async Task<string> inputFirstCreate(string sn, string station_id)
        {
            String msg = "PASS";
            int count = 0;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_SENDRECVDATASFC);
            InputFirstCreateSend dataSend = new InputFirstCreateSend();
            dataSend.data_type = "INPUT_FIRST_CREATE";
            dataSend.dut_id = sn;
            dataSend.station_id = station_id;
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (!d.Contains("FAIL"))
                            msg = "PASS";
                        else
                            msg = d;
                    }
                    else
                    {
                        msg = "[checkSnApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[checkSnApi][ERROR]: " + ex.Message;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }

            return msg;
        }

        public async Task<string> employeeAuthApi(string username, string password)
        {
            String msg = "PASS";

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_EMPLOYEEAUTH);
            EmployeeAuthSend dataSend = new EmployeeAuthSend();
            dataSend.username = username;
            dataSend.password = password;
            try
            {
                var JsonString = JsonSerializer.Serialize(dataSend);
                StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                if (response.IsSuccessStatusCode)
                {
                    msg = "PASS";
                }
                else
                {
                    msg = "[employeeAuthApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {

                msg = "[employeeAuthApi][ERROR]: " + ex.Message;
            }

            return msg;
        }

        public async Task<string> QCAuthApi(string username, string password)
        {
            String msg = "PASS";

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_QCAUTH);
            EmployeeAuthSend dataSend = new EmployeeAuthSend();
            dataSend.username = username;
            dataSend.password = password;
            try
            {
                var JsonString = JsonSerializer.Serialize(dataSend);
                StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                if (response.IsSuccessStatusCode)
                {
                    msg = "PASS";
                }
                else
                {
                    msg = "[QCAuthApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {

                msg = "[QCAuthApi][ERROR]: " + ex.Message;
            }

            return msg;
        }

        public async Task<(bool, string)> getSNByGiftApi(string sn, string station_id)
        {
            bool result = true;
            String msg = "PASS";
            int count = 0;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_SENDRECVDATASFC);
            GetSNByGiftSend dataSend = new GetSNByGiftSend();
            dataSend.data_type = "GET_SN_BY_GIFT";
            dataSend.dut_id = sn;
            dataSend.station_id = station_id;
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        if (!d.Contains("FAIL"))
                            result = true;
                        else
                            result = false;
                    }
                    else
                    {
                        result = false;
                        msg = "[getSNByGiftApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[getSNByGiftApi][ERROR]: " + ex.Message;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }

            return (result, msg);
        }

        public async Task<(bool, string)> rePrintConfirmApi(string sn, string station_id, string wo, string category, bool dev_mode=true)
        {
            bool result = true;
            String msg = "PASS";
            int count = 0;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_SENDRECVDATASFC);
            ReprintConfirmSend dataSend = new ReprintConfirmSend();
            dataSend.data_type = "REPRINT_CONFIRM";
            dataSend.dut_id = sn;
            dataSend.station_id = station_id;
            dataSend.data.WorkOder = wo;
            dataSend.data.category = category;
            dataSend.data.dev_mode = dev_mode;

            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        if (!d.Contains("FAIL"))
                            result = true;
                        else
                            result = false;
                    }
                    else
                    {
                        result = false;
                        msg = "[rePrintConfirmApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[rePrintConfirmApi][ERROR]: " + ex.Message;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }

            return (result, msg);
        }


        public async Task<(bool, string)> getProductInfoApi(string station_id)
        {
            bool result = true;
            String msg = "PASS";
            int count = 0;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_SENDRECVDATASFC);
            GetProductInfoSend dataSend = new GetProductInfoSend();
            dataSend.data_type = "GET_PRODUCT_INFO";
            dataSend.dut_id = "NONE";
            dataSend.station_id = station_id;
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        if (!d.Contains("FAIL"))
                            result = true;
                        else
                            result = false;
                    }
                    else
                    {
                        result = false;
                        msg = "[getProductInfoApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[getProductInfoApi][ERROR]: " + ex.Message;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }

            return (result, msg);
        }

        public async Task<(bool, string)> getProductInfoBySNApi(string sn, string station_id)
        {
            bool result = true;
            String msg = "PASS";
            int count = 0;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_SENDRECVDATASFC);
            GetProductInfoSend dataSend = new GetProductInfoSend();
            dataSend.data_type = "GET_PRODUCT_INFO_BY_SN";
            dataSend.dut_id = sn;
            dataSend.station_id = station_id;
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        if (!d.Contains("FAIL"))
                            result = true;
                        else
                            result = false;
                    }
                    else
                    {
                        result = false;
                        msg = "[getProductInfoBySNApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[getProductInfoBySNApi][ERROR]: " + ex.Message;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }

            return (result, msg);
        }


        public async Task<(bool, string)> getCompleteQtyByStationApi(string station_id)
        {
            bool result = true;
            String msg = "PASS";
            int count = 0;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_SENDRECVDATASFC);
            GetCompleteQtyByStationSend dataSend = new GetCompleteQtyByStationSend();
            dataSend.data_type = "GET_COMPLETE_QTY_BY_STATION";
            dataSend.dut_id = "NONE";
            dataSend.station_id = station_id;
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        if (!d.Contains("FAIL"))
                            result = true;
                        else
                            result = false;
                    }
                    else
                    {
                        result = false;
                        msg = "[getProductInfoApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[getProductInfoApi][ERROR]: " + ex.Message;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }

            return (result, msg);
        }

        public async Task<(bool, string)> IsNeedInputSNApi(string wo, string station_id)
        {
            String msg = "PASS";
            bool result = true;
            int count = 0;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_IsNeedFirstSN);
            IsNeedInputSNSend dataSend = new IsNeedInputSNSend();
            dataSend.wo = wo;
            dataSend.StationId = station_id;
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        result = true;
                    }
                    else
                    {
                        msg = "[IsNeedInputSNApi][ERROR]: " + d;
                        result = false;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[IsNeedInputSNApi][ERROR]: " + ex.Message;
                    result = false;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }
            return (result, msg);
        }

        public async Task<(bool, string)> bindApi(string sn, string station_id, List<string> SubList)
        {
            bool result = true;
            String msg = "PASS";
            int count = 0;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_SENDRECVDATASFC);

            BINDData dataBind = new BINDData();
            dataBind.sn = SubList;
            BINDSend dataSend = new BINDSend();
            dataSend.data_type = "BIND";
            dataSend.dut_id = sn;
            dataSend.station_id = station_id;
            dataSend.data = dataBind;

            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent data = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, data);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        if (!d.Contains("FAIL"))
                            result = true;
                        else
                            result = false;
                    }
                    else
                    {
                        result = false;
                        msg = "[bindApi][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[bindApi][ERROR]: " + ex.Message;
                    result = false;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }

            return (result, msg);
        }

        public async Task<(bool, string, ProductLabelResponse)> getProductLabel(string skuCode, string stationType)
        {
            string msg = "PASS";
            ProductLabelResponse data = new ProductLabelResponse();
            bool result = true;
            int count = 0;



            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + $"/shopfloor/productLabel/?SkuCode={skuCode}&StationCode={stationType}&page_size=10");
            while (count < 3)
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(httpClient.BaseAddress);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        data = JsonSerializer.Deserialize<ProductLabelResponse>(d);
                        result = true;
                    }
                    else
                    {
                        msg = "[getProductLabel][ERROR]: " + d;
                        result = false;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[getProductLabel][ERROR]: " + ex.Message;
                    result = false;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }
            return (result, msg, data);
        }

        public async Task<(bool, string, ProductSNResponse)> getProductSN(string sn)
        {
            string msg = "PASS";
            ProductSNResponse data = new ProductSNResponse();
            bool result = true;
            int count = 0;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + $"/shopfloor/productsn/?serialnumber={sn}&page_size=10");
            while (count < 3)
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(httpClient.BaseAddress);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        data = JsonSerializer.Deserialize<ProductSNResponse>(d);
                        result = true;
                    }
                    else
                    {
                        msg = "[getProductSN][ERROR]: " + d;
                        result = false;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[getProductSN][ERROR]: " + ex.Message;
                    result = false;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }
            return (result, msg, data);
        }

        public string DownloadFileByUrl(string urlLabel)
        {
            if (urlLabel == "")
                return "";
            WebClient webClient = new WebClient();
            Uri uri = new Uri(this.root_url + urlLabel);
            string fileName = Path.GetFileName(uri.AbsolutePath);
            if (!File.Exists(fileName))
            {
                webClient.DownloadFile(uri, fileName);
            }
            else
            {
                File.Delete(fileName);
                webClient.DownloadFile(uri, fileName);
            }
            return Path.GetFullPath(fileName);
        }

        public async Task<(bool, string, GetQtyCompleteResponse)> getQtyCompleteAtStation(string station_id)
        {
            bool result = true;
            String msg = "PASS";
            GetQtyCompleteResponse data = new GetQtyCompleteResponse();
            int count = 0;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + URL_SENDRECVDATASFC);

            InputFirstCreateSend dataSend = new InputFirstCreateSend();
            dataSend.data_type = "GET_COMPLETE_QTY_BY_STATION";
            dataSend.dut_id = "NONE";
            dataSend.station_id = station_id;

            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent sdata = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, sdata);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        data = JsonSerializer.Deserialize<GetQtyCompleteResponse>(d);
                        if (!d.Contains("FAIL"))
                            result = true;
                        else
                            result = false;
                    }
                    else
                    {
                        result = false;
                        msg = "[getQtyCompleteAtStation][ERROR]: " + response.Content.ReadAsStringAsync().Result;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[getQtyCompleteAtStation][ERROR]: " + ex.Message;
                    result = false;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }

            return (result, msg, data);
        }

        public async Task<(bool, string, StationLinkPartResponse)> getStationLinkPart(string skuCode, string stationType)
        {
            string msg = "PASS";
            StationLinkPartResponse data = new StationLinkPartResponse();
            bool result = true;
            int count = 0;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + $"/shopfloor/StationLinkPart/?SkuCode={skuCode}&StationType={stationType}&page_size=10");
            while (count < 3)
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(httpClient.BaseAddress);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        data = JsonSerializer.Deserialize<StationLinkPartResponse>(d);
                        result = true;
                    }
                    else
                    {
                        msg = "[getStationLinkPart][ERROR]: " + d;
                        result = false;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[getStationLinkPart][ERROR]: " + ex.Message;
                    result = false;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }
            return (result, msg, data);
        }


        /* Usage:
            (bool result, string msg, requestAllocateMACResponse data) res = await mApiMes.requestAllocateMACApi(txtSN.Text);
                if (!res.result)
                {
                    MessageBox.Show(res.msg);
                    focusAndSelectSN(false);
                    return;
                }
            MessageBox.Show(res.data.mac);
         */
        public async Task<(bool, string, requestAllocateMACResponse)> requestAllocateMACApi(string sn)
        {
            string msg = "PASS";
            requestAllocateMACSend dataSend = new requestAllocateMACSend();
            dataSend.dut_id = sn;
            requestAllocateMACResponse data = new requestAllocateMACResponse();
            bool result = true;
            int count = 0;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + "/shopfloor/productsn/requestAllocateMAC/");
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent ds = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, ds);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        data = JsonSerializer.Deserialize<requestAllocateMACResponse>(d);
                        result = true;
                    }
                    else
                    {
                        msg = "[requestAllocateMACApi][ERROR]: " + d;
                        result = false;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[requestAllocateMACApi][ERROR]: " + ex.Message;
                    result = false;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }
            return (result, msg, data);
        }

        /*  Usage
            (bool result, string msg, string data) res11 = await mApiMes.updateProvisioningDataApi(txtSN.Text, mac: "991BB565017F", mac2:"123124124",
                HSK: "444234234sdsdsadasd", resetCode: "12344");
            if (!res11.result)
            {
                MessageBox.Show(res11.msg);
                focusAndSelectSN(false);
                return;
            }         
         */
        public async Task<(bool, string, string)> updateProvisioningDataApi(string sn, string mac="", string mac2="", string wifiMac1="", string wifiMac2="",
            string matterCode="", string augustCode="", string HSK="", string resetCode="", string lockId="", string fwVersion="" )
        {
            string msg = "PASS";
            string data = "";
            bool result = true;
            int count = 0;

            updateProvisioningDataSend dataSend = new updateProvisioningDataSend();
            dataSend.SerialNumber = sn;
            dataSend.Mac = mac;
            dataSend.Mac2 = mac2;
            dataSend.WifiMac1 = wifiMac1;
            dataSend.WifiMac2 = wifiMac2;
            dataSend.MatterCode = matterCode;
            dataSend.AugustCode = augustCode;
            dataSend.HSK = HSK;
            dataSend.ResetCode = resetCode;
            dataSend.LockId = lockId;
            dataSend.FW_Version = fwVersion;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.root_url + "/shopfloor/productsn/updateProvisioningData/");
            while (count < 3)
            {
                try
                {
                    var JsonString = JsonSerializer.Serialize(dataSend);
                    StringContent ds = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, ds);
                    string d = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        msg = d;
                        data = d;
                        result = true;
                    }
                    else
                    {
                        msg = "[updateProvisioningDataApi][ERROR]: " + d;
                        result = false;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    count++;
                    msg = "[updateProvisioningDataApi][ERROR]: " + ex.Message;
                    result = false;
                    if (msg.Contains("An error occurred while sending the request"))
                    {
                        await Task.Delay(2000);
                        continue;
                    }
                    break;
                }
            }
            return (result, msg, data);
        }


        public class CheckSnSend
        {
            public string SerialNumber { get; set; }
            public string Station { get; set; }
            public string Line { get; set; }
            public string Index { get; set; }
            public string User { get; set; }

        }

        public class RequestNexSnSend
        {
            public RequestNexSnSend(
                string sn = "", string st = "", string line = "", string idx = "",
                string user = "", bool result = false, string des = "OK",
                float totaltime = 0, List<TestData> data=null )
            {
                this.SerialNumber = sn;
                this.Station = st;
                this.Line = line;
                this.Index = idx;
                this.User = user;
                this.Result = result;
                this.Description = des;
                this.TotalTime = totaltime;
                this.data = new List<TestData>();
            }

            public class TestData
            {
                public string id { get; set; }
                public string name { get; set; }
                public string step_name { get; set; }
                public string result { get; set; }
                public string value { get; set; }
                public string lower_limit { get; set; }
                public string upper_limit { get; set; }
                public string unit { get; set; }
                public string time { get; set; }
                public bool enable { get; set; }
                public override string ToString()
                {
                    return $"ID:{id}-Name:{name}-Step name:{step_name}-Value:{value}-Min:{lower_limit}-Max:{upper_limit}-Result:{result}";
                }
            }

            public string SerialNumber { get; set; }
            public string Station { get; set; }
            public string Line { get; set; }
            public string Index { get; set; }
            public string User { get; set; }
            public bool Result { get; set; }
            public string Description { get; set; } = "OK";
            public float TotalTime { get; set; }
            public List<TestData> data { get; set; } = new List<TestData>();


        }

        public class InputFirstCreateSend
        {
            public string data_type { get; set; }
            public string dut_id { get; set; }
            public string station_id { get; set; }

        }

        public class GetSNByGiftSend
        {
            public string data_type { get; set; }
            public string dut_id { get; set; }
            public string station_id { get; set; }

        }

        public class GetProductInfoSend
        {
            public string data_type { get; set; }
            public string dut_id { get; set; }
            public string station_id { get; set; }

        }

        public class GetCompleteQtyByStationSend
        {
            public string data_type { get; set; }
            public string dut_id { get; set; }
            public string station_id { get; set; }

        }

        public class EmployeeAuthSend
        {
            public string username { get; set; }
            public string password { get; set; }

        }

        public class IsNeedInputSNSend
        {
            public string wo { get; set; }
            public string StationId { get; set; }
        }

        public class IsNeedInputSNResponse
        {
            public bool Result { get; set; }
        }

        public class BINDData
        {
            public List<string> sn { get; set; }
            public BINDData()
            {
                this.sn = new List<string>();
            }
        }

        public class BINDSend
        {
            public string data_type { get; set; }
            public string dut_id { get; set; }
            public string station_id { get; set; }
            public BINDData data { get; set; }
        }

        public class ProductLabelResponse
        {
            public int count { get; set; }
            public string next { get; set; }
            public object previous { get; set; }
            public List<ProductLabelInfo> results { get; set; }

            public class ProductLabelInfo
            {
                public int id { get; set; }
                public object ZplCode { get; set; }
                public object version { get; set; }
                public object Dpi { get; set; }
                public string TemplateFile { get; set; }
                public string StationType { get; set; }
                public string Product { get; set; }
                public string Printer { get; set; }
                public int CopyQty { get; set; }
                public DateTime created_date { get; set; }
                public DateTime updated_date { get; set; }
            }
        }

        public class ProductSNResponse
        {
            public int count { get; set; }
            public string next { get; set; }
            public object previous { get; set; }
            public List<ProductSNInfo> results { get; set; }

            public class ProductSNInfo
            {
                public int id { get; set; }
                public string LastStation { get; set; }
                public string PO { get; set; }
                public string WorkOder { get; set; }
                public string NextStation { get; set; }
                public string Line { get; set; }
                public string SkuCode { get; set; }
                public string CodeAffiliate { get; set; }
                public string CartonQty { get; set; }
                public string PalletQty { get; set; }
                public string GiftQty { get; set; }
                public string Truck { get; set; }
                public string SerialNumber { get; set; }
                public string Mac { get; set; }
                public string Mac2 { get; set; }
                public string WifiMac1 { get; set; }
                public string WifiMac2 { get; set; }
                public string Status { get; set; }
                public int FailCount { get; set; }
                public bool IsUpload { get; set; }
                public string MatterCode { get; set; }
                public string AugustCode { get; set; }
                public string HSK { get; set; }
                public string LockId { get; set; }
                public string ResetCode { get; set; }
                public string FW_Version { get; set; }
                public string GiftId { get; set; }
                public string CartonId { get; set; }
                public string PalletId { get; set; }
                public string MoveStation { get; set; }

                public DateTime created_date { get; set; }
                public DateTime updated_date { get; set; }
            }
        }

        public class GetQtyCompleteResponse
        {
            public string result { get; set; }
            public string message { get; set; }
            public DataQty data { get; set; }

            public class DataQty
            {
                public List<string> sn { get; set; }
            }
        }

        public class StationLinkPartResponse
        {
            public int count { get; set; }
            public object next { get; set; }
            public object previous { get; set; }
            public List<StationLinkPartResult> results { get; set; }

            public class StationLinkPartResult
            {
                public int id { get; set; }
                public string StationType { get; set; }
                public string Product { get; set; }
                public string PartCode { get; set; }
                public int Quantity { get; set; }
                public string PartCodeNameRule { get; set; }
                public bool AllowDuplicate { get; set; }
                public bool Active { get; set; }
                public DateTime created_date { get; set; }
                public DateTime updated_date { get; set; }
            }
        }

        public class ReprintConfirmSend
        {
            public string data_type { get; set; }
            public string dut_id { get; set; }
            public string station_id { get; set; }
            public ReprintConfirmData data { get; set; }

            public ReprintConfirmSend()
            {
                this.data = new ReprintConfirmData();
            }


            public class ReprintConfirmData
            {
                public string category { get; set; }
                public string WorkOder { get; set; }
                public bool dev_mode { get; set; }
            }
        }

        public class requestAllocateMACSend
        {
            public string dut_id { get; set; }
        }

        public class requestAllocateMACResponse
        {
            public string mac { get; set; }
            public string issue { get; set; }
        }

        public class updateProvisioningDataSend
        {
            public string SerialNumber { get; set; }
            public string Mac { get; set; }
            public string Mac2 { get; set; }
            public string WifiMac1 { get; set; }
            public string WifiMac2 { get; set; }
            public string MatterCode { get; set; }
            public string AugustCode { get; set; }
            public string HSK { get; set; }
            public string ResetCode { get; set; }
            public string LockId { get; set; }
            public string FW_Version { get; set; }

            public updateProvisioningDataSend()
            {
                this.SerialNumber = "";
                this.Mac = "";
                this.Mac2 = "";
                this.WifiMac1 = "";
                this.WifiMac2 = "";
                this.MatterCode = "";
                this.AugustCode = "";
                this.HSK = "";
                this.ResetCode = "";
                this.LockId = "";
                this.FW_Version = "";
            }
        }

    }

}
