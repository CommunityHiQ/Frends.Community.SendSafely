﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class UploadKeycodeRequest
    {
        [JsonProperty(PropertyName = "keycode")]
        public String Keycode { get; set; }
    }
}
