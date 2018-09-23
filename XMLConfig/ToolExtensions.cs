using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLConfing
{
  public static class ToolExtensions
    {
        /// <summary>
        /// 判断一个对象是否为System.Boolean 或者 Boolean? 类型
        /// </summary>
        /// <param name="obj">待判断对象</param>
        /// <returns>bool</returns>
        public static bool IsBooleanType(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            Type type = obj.GetType();
            if (type == typeof(Nullable<Boolean>) || type == typeof(Boolean))
            {
                return true;
            }
            return false;
        }
    }
}
