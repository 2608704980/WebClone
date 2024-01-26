using System.Data;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Threading;
namespace 网站克隆1._0
{
    public partial class 网站克隆 : Form
    {
        public 网站克隆()
        {
            InitializeComponent();
            //为了防止重复。设置本地数据库.
            //首先查询数据库是否存在.应该放在指定路径下面。
            SQLiteHelper.connectionString = "Data Source=BaseData.db;";
            if (!File.Exists("BaseData.db"))
            {
                SQLiteHelper.CreatDB("BaseData.db");
                //开始新增语句.

            }
        }
        public delegate void ThreadStart();
        private  void 开始采集_Click(object sender, EventArgs e)
        {
            HtmlWeb web = new HtmlWeb();
            //探索层数.numericUpDown1
           int i= 1;
            递归调用(this.textBox1.Text, web,i,this.numericUpDown1.Value);
        }

        private void 递归调用(string aUrl, HtmlWeb web, int i, decimal value)
        {
            if (aUrl.EndsWith("#"))
            {
                return;
            }
            if (aUrl.Contains("?"))
            {
                return;
            }
            if (aUrl.EndsWith("//"))
            {
                return;
            }
            if(aUrl=="/")
            {
                return;
            }
            HtmlDocument hd = web.Load(aUrl);
            string a = "//a"; //检索a标签.
                              //if (i < value)
                              //{

            //}
            //else
            //{
            //    return;
            //}


            string imgpaths = this.textBox2.Text.Trim('\\') + "\\";
            foreach (HtmlNode node in hd.DocumentNode.SelectNodes(a))
            {
                //如果不存在href。则不执行.
                if (node.Attributes["href"] != null)
                {
                    //然后获取a标签的内容.
                     aUrl = node.Attributes["href"].Value.ToString();
                    string nUrl = aUrl;
                    //首先判断是否是以http开头。
                    if (!aUrl.StartsWith("http"))
                    {
                        nUrl = this.textBox1.Text.Trim('/')  + aUrl;
                    }
                    //查询是否存在.
                    DataRow r = SQLiteHelper.ExecuteDataRow("select * from mysite where Url='" + nUrl + "'");
                    if (r == null)
                    {
                        SQLiteHelper.ExecuteSql("INSERT INTO mysite (Url,downtrue) VALUES('" + nUrl + "', 0)");
                        r = SQLiteHelper.ExecuteDataRow("select * from mysite where Url='" + nUrl + "'");
                    }
                    if (r["downtrue"].ToString() == "0")
                    {
                        GetImgList(imgpaths + DateTime.Now.ToString("yyyyMMdd") + "\\", nUrl, r, this.textBox1.Text.Trim('/'));
                    }
                    i = i + 1;
                    var mr = SQLiteHelper.ExecuteDataRow("select * from test where Url='" + nUrl + "'");
                    if (mr == null)
                    {
                        SQLiteHelper.ExecuteSql("INSERT INTO test (url,id) VALUES('" + nUrl + "', " + i + ")");
                        递归调用(nUrl, web, i, value);

                    }
                  
                    SQLiteHelper.ExecuteSql("INSERT INTO test (url,id) VALUES('" + nUrl + "', "+ i+")");

                  
                }
            }

        }

        
        private void testc()
        {
            for (int i = 0; i < 10; i++)
            {
                //this.richTextBox2.Text = this.richTextBox2.Text + imgUrl + "\n";
            }
        }
        private static void GetImgList(string imgpaths, string imgurl, string index)
        {
            string imgPath = "//img";//选择img
            int imgNum = 0;//图片编号
                           //获取img标签中的图片
                           //定义线程.
            HtmlWeb web = new HtmlWeb();
            WebClient wc = new WebClient();
            HtmlDocument hd = web.Load(imgurl);

            //string imgpaths = this.textBox2.Text + "/images/" + this.textBox3.Text + "/";
            foreach (HtmlNode node in hd.DocumentNode.SelectNodes(imgPath))
            {
                if (node.Attributes["src"] != null)
                {
                    string imgUrl = node.Attributes["src"].Value.ToString();
                    if (imgUrl != "" && imgUrl != " ")
                    {
                        if (!imgUrl.StartsWith("http"))
                        {
                            imgUrl = index + imgUrl;
                        }
                        imgNum++;
                        //生成文件名，自动获取后缀
                        string fileName = imgNum + imgUrl.Substring(imgUrl.LastIndexOf("."));

                        ImgDownloader.DownloadImg(wc, imgUrl, imgpaths, fileName);
                    }
                }
                else
                {//如果src为空。则选择ess-data
                    if (node.Attributes["ess-data"] != null)
                    {
                        string imgUrl = node.Attributes["ess-data"].Value.ToString();
                        if (imgUrl != "" && imgUrl != " ")
                        {
                            imgNum++;
                            //生成文件名，自动获取后缀
                            string fileName = imgNum + imgUrl.Substring(imgUrl.LastIndexOf("."));

                            ImgDownloader.DownloadImg(wc, imgUrl, imgpaths, fileName);
                        }
                    }

                }
            }
        }
        private static void GetImgList(string imgpaths,string imgurl, DataRow r,string index)
        {
            string imgPath = "//img";//选择img
            int imgNum = 0;//图片编号
                           //获取img标签中的图片
                           //定义线程.
            HtmlWeb web = new HtmlWeb();
            WebClient wc = new WebClient();
            HtmlDocument hd = web.Load(imgurl);
            
            //string imgpaths = this.textBox2.Text + "/images/" + this.textBox3.Text + "/";
            foreach (HtmlNode node in hd.DocumentNode.SelectNodes(imgPath))
            {
                if (node.Attributes["src"] != null)
                {
                    string imgUrl = node.Attributes["src"].Value.ToString();
                    if (imgUrl != "" && imgUrl != " ")
                    {
                        if (!imgUrl.StartsWith("http"))
                        {
                            imgUrl= index + imgUrl;
                        }
                        imgNum++;
                        //生成文件名，自动获取后缀
                        string fileName = imgNum + imgUrl.Substring(imgUrl.LastIndexOf("."));

                        ImgDownloader.DownloadImg(wc, imgUrl, imgpaths, fileName);
                    }
                }
                else
                {//如果src为空。则选择ess-data
                    if (node.Attributes["ess-data"] != null)
                    {
                        string imgUrl = node.Attributes["ess-data"].Value.ToString();
                        if (imgUrl != "" && imgUrl != " ")
                        {
                            imgNum++;
                            //生成文件名，自动获取后缀
                            string fileName = imgNum + imgUrl.Substring(imgUrl.LastIndexOf("."));

                            ImgDownloader.DownloadImg(wc, imgUrl, imgpaths, fileName);
                        }
                    }

                }
            }
            //更新记录
            SQLiteHelper.ExecuteSql("update mysite set downtrue=1 where Url='" + imgurl + "'");
        }
        /// <summary>
        /// 图片下载器
        /// </summary>
        public class ImgDownloader
        {
            /// <summary>
            /// 下载图片
            /// </summary>
            /// <param name="webClient"></param>
            /// <param name="url">图片url</param>
            /// <param name="folderPath">文件夹路径</param>
            /// <param name="fileName">图片名</param>
            public static void DownloadImg(WebClient webClient, string url, string folderPath, string fileName)
            {
                //如果文件夹不存在，则创建一个
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                //判断路径是否完整，补全不完整的路径
                if (url.IndexOf("https:") == -1 && url.IndexOf("http:") == -1)
                {
                    url = "https:" + url;
                }
                //下载图片
                try
                {
                    webClient.DownloadFile(url, folderPath + fileName);
                    FileInfo fileInfo = new FileInfo(folderPath + fileName);
                    //if (fileInfo.Length < 1024 * 20)
                    //{
                    //    File.Delete(folderPath + fileName);
                    //}
                    //Console.WriteLine(fileName + "下载成功");
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    Console.WriteLine(url);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            //if(this.textBox1.Text.Length > 0)
            //{
            //    string[] x = this.textBox1.Text.Split("/");
            //    this.textBox3.Text = x[6].Replace(".html","");
            //}
        }

        private void 单页采集_Click(object sender, EventArgs e)
        {
            //HtmlWeb web = new HtmlWeb();
            string imgpaths = this.textBox2.Text.Trim('\\') + "\\";
            //HtmlDocument hd = web.Load(richTextBox2.Text);
            var hd = new HtmlDocument();
            hd.LoadHtml(richTextBox2.Text);
            string imgPath = "//img";//选择img
            foreach (HtmlNode node in hd.DocumentNode.SelectNodes(imgPath))
            {
                string imgUrl = node.Attributes["src"].Value.ToString();
                richTextBox1.Text = richTextBox1.Text + "\n\r" + imgUrl;
                string fileName =   imgUrl.Substring(imgUrl.LastIndexOf("/"));
                WebClient wc = new WebClient();
                ImgDownloader.DownloadImg(wc, imgUrl, imgpaths, fileName);
            }
                 
              //  string imgpaths = this.textBox2.Text.Trim('\\') + "\\";
           
           // GetImgList(imgpaths + DateTime.Now.ToString("yyyyMMdd") + "\\", nUrl, this.textBox1.Text.Trim('/'));
        }
    }
}