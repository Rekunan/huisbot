﻿using huisbot.Models.Osu;
using static System.Formats.Asn1.AsnWriter;

namespace huisbot.Utils.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="OsuBeatmap"/> class.
/// </summary>
internal static class BeatmapExtensions
{
  /// <summary>
  /// Returns the length of the beatmap, including clock rate changes through specified mods.
  /// </summary>
  /// <param name="beatmap">The beatmap.</param>
  /// <param name="modsStr">The mods.</param>
  /// <returns>The length of the beatmap including the specified mods.</returns>
  public static TimeSpan GetLength(this OsuBeatmap beatmap, string modsStr)
  {
    string[] mods = modsStr.Chunk(2).Select(x => new string(x)).ToArray();
    TimeSpan span = TimeSpan.FromSeconds(beatmap.Length);

    // If DoubleTime/Nightcore, the length is divided by 1.5.
    if (mods.Contains("DT") || mods.Contains("NC"))
      return span / 1.5;
    // If HalfTime, the length is multiplied by 1.5.
    else if (mods.Contains("HT"))
      return span * 1.5;

    return span;
  }

  /// <summary>
  /// Returns the BPM of the beatmap, including clock rate changes through mods.
  /// </summary>
  /// <param name="beatmap">The beatmap.</param>
  /// <param name="modsStr">The mods.</param>
  /// <returns>The BPM of the beatmap including the specified mods.</returns>
  public static double GetBPM(this OsuBeatmap beatmap, string modsStr)
  {
    string[] mods = modsStr.Chunk(2).Select(x => new string(x)).ToArray();

    // If DoubleTime/Nightcore, the BPM is multiplied by 1.5.
    if (mods.Contains("DT") || mods.Contains("NC"))
      return Math.Round(beatmap.BPM * 1.5, 2);
    // If HalfTime, the BPM is didided by 1.5.
    else if (mods.Contains("HT"))
      return Math.Round(beatmap.BPM / 1.5, 2);

    return beatmap.BPM;
  }

  /// <summary>
  /// Returns the mod-adjusted circle size of the beatmap.
  /// </summary>
  /// <param name="beatmap">The beatmap.</param>
  /// <param name="modsStr">The mod string.</param>
  /// <returns>The mod-adjusted circle size.</returns>
  public static double AdjustedCS(this OsuBeatmap beatmap, string modsStr)
  {
    string[] mods = modsStr.Chunk(2).Select(x => new string(x)).ToArray();

    double cs = beatmap.CircleSize;
    // If HardRock, the CS is multiplied by 1.3.
    if (mods.Contains("HR"))
      cs *= 1.3;
    // If Easy, the CS is multiplied by 0.5.
    else if (mods.Contains("EZ"))
      cs *= 0.5;

    return Math.Min(cs, 10);
  }

  /// <summary>
  /// Returns the mod-adjusted approach rate of the beatmap.
  /// </summary>
  /// <param name="beatmap">The beatmap.</param>
  /// <param name="modsStr">The mod string.</param>
  /// <returns>The mod-adjusted approach rate.</returns>
  public static double AdjustedAR(this OsuBeatmap beatmap, string modsStr)
  {
    string[] mods = modsStr.Chunk(2).Select(x => new string(x)).ToArray();

    double ar = beatmap.ApproachRate;
    // If HardRock, the AR is multiplied by 1.4, up to 10.
    if (mods.Contains("HR"))
      ar = Math.Min(ar * 1.4, 10);
    // If Easy, the AR is multiplied by 0.5.
    else if (mods.Contains("EZ"))
      ar *= 0.5;
    // If DoubleTime/Nightcore or HalfTime, the pre-empt is multiplied by 2/3 and 4/3 respectively.
    if (mods.Contains("DT") || mods.Contains("NC") || mods.Contains("HT"))
    {
      int ms = (int)(ar >= 5 ? ar == 5 ? 1200 : 1200 - 750 * (ar - 5) / 5d : 1200 + 600 * (5 - ar) / 5d);
      ms = (int)(ms * (mods.Contains("HT") ? 4d / 3 : 2d / 3));
      ar = Math.Min(11.11, (ms == 1200) ? 5 : (ms > 1200) ? 5 - 5 * (1200 - ms) / 600d : 5 + 5 * (1200 - ms) / 750d);
    }

    return ar;
  }

  /// <summary>
  /// Returns the mod-adjusted overall difficulty of the beatmap.
  /// </summary>
  /// <param name="beatmap">The beatmap.</param>
  /// <param name="modsStr">The mod string.</param>
  /// <returns>The mod-adjusted overall difficulty.</returns>
  public static double AdjustedOD(this OsuBeatmap beatmap, string modsStr)
  {
    string[] mods = modsStr.Chunk(2).Select(x => new string(x)).ToArray();

    double od = beatmap.OverallDifficulty;
    // If HardRock, the OD is multiplied by 1.4.
    if (mods.Contains("HR"))
      od = Math.Min(od * 1.4, 10);
    // If Easy, the OD is multiplied by 0.5.
    else if (mods.Contains("EZ"))
      od *= 0.5;
    // If DoubleTime/Nightcore, the OD is multiplied by 1.5.
    if (mods.Contains("DT") || mods.Contains("NC"))
      od *= 1.5;
    // If HalfTime, the OD is multiplied by 0.75.
    else if (mods.Contains("HT"))
      od *= 0.66;

    return Math.Min(od, 11.1);
  }

  /// <summary>
  /// Returns the mod-adjusted drain rate of the beatmap.
  /// </summary>
  /// <param name="beatmap">The beatmap.</param>
  /// <param name="modsStr">The mod string.</param>
  /// <returns>The mod-adjusted drain rate.</returns>
  public static double AdjustedHP(this OsuBeatmap beatmap, string modsStr)
  {
    string[] mods = modsStr.Chunk(2).Select(x => new string(x)).ToArray();

    double hp = beatmap.DrainRate;
    // If HardRock, the HP is multiplied by 1.4.
    if (mods.Contains("HR"))
      hp *= 1.4;
    // If Easy, the HP is multiplied by 0.5.
    else if (mods.Contains("EZ"))
      hp *= 0.5;

    return Math.Min(hp, 10);
  }
}
