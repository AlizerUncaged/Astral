﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Control
{
    public interface IPage
    {
        event EventHandler<IPage> Replaced;
    }
}
