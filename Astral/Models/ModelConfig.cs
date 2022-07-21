﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    public class ModelConfig : IConfig
    {
        public string? WeightsFilepath { get; set; } = "./Dependencies/YoloV4/Valorant/yolov4-tiny.weights";

        public string? CfgFilepath { get; set; } = "./Dependencies/YoloV4/Valorant/yolov4-tiny.cfg";

        public string? NamesFilepath { get; set; } = "./Dependencies/YoloV4/Valorant/coco-dataset.labels";
    }
}