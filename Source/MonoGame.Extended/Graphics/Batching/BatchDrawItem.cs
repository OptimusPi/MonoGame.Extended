﻿using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Extended.Graphics.Batching
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct BatchDrawItem<TDrawContext>
        where TDrawContext : struct, IDrawContext<TDrawContext>
    {
        internal TDrawContext Context;
        internal readonly int StartIndex;
        internal int PrimitiveCount;
        internal byte PrimitiveType;

        internal BatchDrawItem(PrimitiveType primitiveType, int startIndex, int primitiveCount, TDrawContext context)
        {
            PrimitiveType = (byte)primitiveType;
            StartIndex = startIndex;
            PrimitiveCount = primitiveCount;
            Context = context;
        }

        internal bool CanMergeIntoItem(ref TDrawContext otherData, byte primitiveType)
        {
            return Context.Equals(ref otherData) && PrimitiveType == primitiveType && (PrimitiveType != (byte)Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip || PrimitiveType != (byte)Microsoft.Xna.Framework.Graphics.PrimitiveType.LineStrip);
        }
    }
}
