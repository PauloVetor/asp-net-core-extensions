﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NeuroSpeech.Eternity.Tests.Mocks
{
    public class MockBag
    {

        private Dictionary<string, string> bag = new Dictionary<string, string>();

        public string this[string key]
        {
            get => bag[key];
            set => bag[key] = value;
        }

    }
}
