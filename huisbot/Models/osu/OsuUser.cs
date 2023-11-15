﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huisbot.Models.osu;

/// <summary>
/// Represents a user from the osu! API v1.
/// </summary>
internal class OsuUser
{
  /// <summary>
  /// The ID of the user.
  /// </summary>
  [JsonProperty("user_id")]
  public int Id { get; private set; }
}
