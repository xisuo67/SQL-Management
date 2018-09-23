using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
// 此处代码来源于博客【在.net中读写config文件的各种方法】的示例代码
// http://www.cnblogs.com/fish-li/archive/2011/12/18/2292037.html
namespace XMLConfing
{
    /// <summary>
    /// 表示*.config文件中的一个XmlCommand配置项。
    /// </summary>
    [XmlType("XmlCommand")]
    [Serializable]
    public class XmlCommandItem
    {
        /// <summary>
        /// 命令的名字，这个名字将在XmlCommand.From时被使用。
        /// </summary>
        [XmlAttribute("Name")]
        public string CommandName;

        /// <summary>
        /// 命令所引用的所有参数集合
        /// </summary>
        [XmlArrayItem("Parameter")]
        public List<XmlCmdParameter> Parameters = new List<XmlCmdParameter>();

        /// <summary>
        /// 命令的文本。是一段可运行的SQL脚本或存储过程名称。
        /// </summary>
        [XmlElement]
        public MyCDATA CommandText;
        //public string CommandText;

        /// <summary>
        /// SQL命令类型
        /// </summary>
        [DefaultValueAttribute(CommandType.Text)]
        [XmlAttribute]
        public CommandType CommandType = CommandType.Text;

        /// <summary>
        /// 获取或设置在终止执行命令的尝试并生成错误之前的等待时间。 
        /// </summary>
        [DefaultValueAttribute(30)]
        [XmlAttribute]
        public int Timeout = 30;
    }

    /// <summary>
    /// XmlCommand的命令参数。
    /// </summary>
    [Serializable]
    public class XmlCmdParameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        [XmlAttribute]
        public string Name;

        /// <summary>
        /// 参数的数据类型
        /// </summary>
        [XmlAttribute]
        public OracleDbType Type;

        /// <summary>
        /// 参数值的长度。
        /// </summary>
        [DefaultValueAttribute(0)]
        [XmlAttribute]
        public int Size;


        /// <summary>
        /// 参数的输入输出方向
        /// </summary>
        [DefaultValueAttribute(ParameterDirection.Input)]
        [XmlAttribute]
        public ParameterDirection Direction = ParameterDirection.Input;
    }

    /// <summary>
    /// XmlCommand的命令参数拓展。
    /// </summary>
    public static class XmlCmdParameterExtensions
    {
        /// <summary>
        /// 将XmlCommand参数转换成OracleParameter[]
        /// </summary>
        /// <param name="item">XmlCommand的命令参数</param>
        /// <param name="ht">参数键值Hashtable</param>
        /// <returns>OracleParameter[]</returns>
        public static OracleParameter[] ToArrayOracleParameter(this XmlCommandItem item, Hashtable ht)
        {
            if (item.Parameters == null || item.Parameters.Count == 0 || ht == null || ht.Count == 0)
            {
                return null;
            }
            int length = item.Parameters.Count;
            OracleParameter[] arrParameter = new OracleParameter[item.Parameters.Count];
            XmlCmdParameter cmdParameter = null;
            for (int i = 0; i < length; i++)
            {
                cmdParameter = item.Parameters[i];
                object value = ht[cmdParameter.Name.Replace(":", "")];
                if (value == null || value == DBNull.Value)
                {
                    arrParameter[i] = new OracleParameter(cmdParameter.Name, DBNull.Value);
                }
                else
                {
                    if (!value.IsBooleanType())
                    {
                        arrParameter[i] = new OracleParameter(cmdParameter.Name, cmdParameter.Type, value, cmdParameter.Direction);
                    }
                    else
                    {
                        arrParameter[i] = new OracleParameter(cmdParameter.Name, cmdParameter.Type, Convert.ToInt32(value), cmdParameter.Direction);
                    }
                }
            }
            return arrParameter;
        }

        /// <summary>
        /// 将XmlCommand参数转换成OracleParameter[]
        /// </summary>
        /// <param name="item">XmlCommand的命令参数</param>
        /// <param name="ht">new{A="123"}形式参数</param>
        /// <returns>OracleParameter[]</returns>
        public static OracleParameter[] ToArrayOracleParameter(this XmlCommandItem item, object argsObject)
        {
            if (item.Parameters == null || item.Parameters.Count == 0 || argsObject == null)
            {
                return null;
            }
            int length = item.Parameters.Count;
            OracleParameter[] arrParameter = new OracleParameter[item.Parameters.Count];
            XmlCmdParameter cmdParameter = null;
            PropertyInfo[] properties = argsObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            PropertyInfo pInfo = null;
            for (int i = 0; i < length; i++)
            {
                cmdParameter = item.Parameters[i];
                pInfo = Array.Find<PropertyInfo>(properties, (e) => { return (":" + e.Name).Equals(cmdParameter.Name, StringComparison.OrdinalIgnoreCase); });
                if (pInfo == null)
                {
                    arrParameter[i] = new OracleParameter(cmdParameter.Name, DBNull.Value);
                    continue;
                }
                object value = pInfo.FastGetValue(argsObject);
                if (value == null || value == DBNull.Value)
                {
                    arrParameter[i] = new OracleParameter(cmdParameter.Name, DBNull.Value);
                }
                else
                {
                    if (!value.IsBooleanType())
                    {
                        arrParameter[i] = new OracleParameter(cmdParameter.Name, cmdParameter.Type, value, cmdParameter.Direction);
                    }
                    else
                    {
                        arrParameter[i] = new OracleParameter(cmdParameter.Name, cmdParameter.Type, Convert.ToInt32(value), cmdParameter.Direction);
                    }
                }
            }
            return arrParameter;
        }
    }
}
