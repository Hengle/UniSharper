﻿/*
 *	The MIT License (MIT)
 *
 *	Copyright (c) 2018 Jerry Lee
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */

using JsonFx.Json;
using System.Collections.Generic;
using UnityEngine;

namespace UniSharper.Rendering.DataParsers
{
    internal class UnityJsonDataParser : TilingSheetDataParser
    {
        #region Methods

        public override Dictionary<string, Rect> ParseData(string name, string data)
        {
            if (!DataDict.ContainsKey(name))
            {
                DataDict.Add(name, new Dictionary<string, Rect>());

                JsonReader reader = new JsonReader();
                UnityJsonData jsonData = reader.Read<UnityJsonData>(data);

                DualSize texSize = jsonData.meta.size;

                foreach (KeyValuePair<string, Frame> item in jsonData.frames)
                {
                    string frameName = item.Key;
                    Frame frameData = item.Value;

                    // Calculate
                    Vector2 scale = new Vector2(frameData.frame.w / texSize.w, frameData.frame.h / texSize.h);
                    Vector2 offset = new Vector2(frameData.frame.x / texSize.w, (texSize.h - frameData.frame.y - frameData.sourceSize.h) / texSize.h);
                    Rect rect = new Rect(offset.x, offset.y, scale.x, scale.y);
                    DataDict[name].Add(frameName, rect);
                }
            }

            return DataDict[name];
        }

        #endregion Methods

        #region Classes

        private class DualSize
        {
            #region Fields

            public float h;
            public float w;

            #endregion Fields

            #region Constructors

            public DualSize(float w, float h)
            {
                this.w = w;
                this.h = h;
            }

            #endregion Constructors

            #region Methods

            public override string ToString()
            {
                return string.Format("w={0}, h={1}", w, h);
            }

            #endregion Methods
        }

        private class Frame
        {
            #region Fields

            public QuatSize frame;
            public string name;
            public bool rotated;
            public DualSize sourceSize;
            public QuatSize spriteSourceSize;
            public bool trimmed;

            #endregion Fields

            #region Constructors

            public Frame(string name, QuatSize frame, bool rotated, bool trimmed, QuatSize spriteSourceSize, DualSize sourceSize)
            {
                this.name = name;
                this.frame = frame;
                this.rotated = rotated;
                this.trimmed = trimmed;
                this.spriteSourceSize = spriteSourceSize;
                this.sourceSize = sourceSize;
            }

            #endregion Constructors

            #region Methods

            public override string ToString()
            {
                return string.Format("name={0}, frame={1}, rotated={2}, trimmed={3}, spriteSourceSize={4}, sourceSize={5}", name, frame, rotated, trimmed, spriteSourceSize, sourceSize);
            }

            #endregion Methods
        }

        private class Meta
        {
            #region Fields

            public string app;
            public string format;
            public string image;
            public string scale;
            public DualSize size;
            public string smartupdate;
            public string version;

            #endregion Fields

            #region Constructors

            public Meta(string app, string version, string image, string format, DualSize size, string scale, string smartupdate)
            {
                this.app = app;
                this.version = version;
                this.image = image;
                this.format = format;
                this.size = size;
                this.scale = scale;
                this.smartupdate = smartupdate;
            }

            #endregion Constructors

            #region Methods

            public override string ToString()
            {
                return string.Format("image={0}, size={1},", image, size);
            }

            #endregion Methods
        }

        private class QuatSize
        {
            #region Fields

            public float h;
            public float w;
            public float x;
            public float y;

            #endregion Fields

            #region Constructors

            public QuatSize(float x, float y, float w, float h)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
            }

            #endregion Constructors

            #region Methods

            public override string ToString()
            {
                return string.Format("x={0}, y={1}, w={2}, h={3}", x, y, w, h);
            }

            #endregion Methods
        }

        private class UnityJsonData
        {
            #region Fields

            public Dictionary<string, Frame> frames;
            public Meta meta;

            #endregion Fields

            #region Constructors

            public UnityJsonData(Dictionary<string, Frame> frames, Meta meta)
            {
                this.frames = frames;
                this.meta = meta;
            }

            #endregion Constructors
        }

        #endregion Classes
    }
}