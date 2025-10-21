using Shared.ErrorCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Wrappers
{
    public class Result<T>
    {
        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public string ErrorCode { get; set; } = null;
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public Result() { }

        public Result(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Result(string errorCode, string message)
        {
            Succeeded = false;
            ErrorCode = string.IsNullOrEmpty(errorCode) ? ErrorCodeEnum.COM_ERR_000.ToString() : errorCode;
            Message = message;
        }
        public Result(ErrorCodeEnum errorCode, params object[] args)
        {
            Succeeded = false;
            ErrorCode = errorCode.ToString();
            Message = GetDescription(errorCode, args);
        }
        private static string GetDescription(ErrorCodeEnum errorCode, params object[] args)
        {
            var type = errorCode.GetType();
            var nemInfo = type.GetMember(errorCode.ToString());
            if (nemInfo.Length > 0)
            {
                var attrs = nemInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    var des = ((DescriptionAttribute)attrs[0]).Description;
                    return string.Format(des, args);
                }
            }
            return errorCode.ToString();
        }
    }
}
