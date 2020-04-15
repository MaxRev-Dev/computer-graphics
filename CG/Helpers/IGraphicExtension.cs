﻿using System.Drawing;
using Playground.Projections.Abstractions;

namespace Playground.Helpers
{
    internal interface IGraphicExtension
    {
        Pen PrimaryPen { get; set; }
        Pen SecondaryPen { get; set; }

        bool Enable { get; set; }
        float[,] Model3D { get; } 
        void Draw(IProjectorEngine projector);
        void Transform(float[,] trs);
        void Reset(IProjectorEngine projector);
    }
}