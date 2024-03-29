﻿using Astral.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral
{
    public interface IDetectorService
    {
        /// <summary>
        /// Get's called whenever a prediction occurs.
        /// </summary>
        public event EventHandler<IEnumerable<PredictionResult>>? PredictionReceived;
    }
}
