using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral
{
    public interface IConfiguredService<T> where T : IConfig
    {
        T Configuration { get; }
    }
}
