using System;
using System.Collections.Generic;
using System.Text;

namespace Kasi_Server.Common.EasyQuartz
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class JobIgnoreAttribute : Attribute
    {
    }
}
