using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace 网站克隆1._0
{
    public abstract class Hs
    {
        #region 获取默认数据库链接字符串
        public static string GetDatastr(string data = "data")
        {
            //设置类型.
            JsonList mt = new JsonList();
            if(!File.Exists(Directory.GetCurrentDirectory() + "/Config.json")){
                mt.Data = "1";
                mt.Type = "没发现数据配置文件." + Directory.GetCurrentDirectory() + "/Config.json";
               return JsonConvert.SerializeObject(mt);
            }
            else
            {
                //读取文件
                string Fi = File.ReadAllText(Directory.GetCurrentDirectory() + "/Config.json");
                JObject dt = (JObject)JsonConvert.DeserializeObject(Fi);
                mt.Type = "2";
                mt.Type= dt[data].ToString();
                return dt[data].ToString();
            }
            
        }
        #endregion     
        #region 日志记录
        public static void LogWrite(string 内容, string 用户, string 类型)
        {
            SQLiteHelper.ExecuteSql(SQLiteHelper.LogConStr, "INSERT INTO LogData(内容, 操作时间, 用户, 类型)VALUES('" + 内容 + "',DATETIME(), '" + 用户 + "', '" + 类型 + "');");
        }
        #endregion
        #region 信息类型
        public class MsgType
        {
            public int Type { get; set; }
            public string Content { get; set; }
            public string 用户id { get; set; }
            public string 来源id { get; set; }
            public string 时间 { get; set; }

        }
        #endregion
        #region 数据表
        public static DataTable _UserTable;
        #endregion
    }
    public class JsonList
    {
        public string Data { get; set; } //返回的类型
        public string Type { get; set; } //返回的类型

    }
}
