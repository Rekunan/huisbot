﻿using huisbot.Models.Utility;
using huisbot.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huisbot.Services;

/// <summary>
/// The persistence service is responsible for managing the access to the persistence database.
/// </summary>
public class PersistenceService
{
  private readonly Database _database;

  public PersistenceService(Database database)
  {
    _database = database;
  }

  /// <summary>
  /// Returns the link between the specified discord user and the linked osu! user or null if no link exists.
  /// </summary>
  /// <param name="discordId">The discord ID of the user.</param>
  /// <returns>The link between the specified discord user and the linked osu! user or null if no link exists.</returns>
  public async Task<OsuDiscordLink?> GetOsuDiscordLinkAsync(ulong discordId)
  {
    return await _database.OsuDiscordLinks.FirstOrDefaultAsync(x => x.DiscordId == discordId);
  }

  /// <summary>
  /// Adds or updates a link between the specified discord user and the specified osu! user to the database.
  /// </summary>
  /// <param name="discordId">The discord ID of the user.</param>
  /// <param name="osuId">The osu! user ID of the user.</param>
  public async Task SetOsuDiscordLinkAsync(ulong discordId, int osuId)
  {
    // Check whether the link already exists. If it does, update it.
    if (await GetOsuDiscordLinkAsync(discordId) is OsuDiscordLink link)
    {
      link.OsuId = osuId;
      await _database.SaveChangesAsync();
      return;
    }

    // Otherwise add the link to the database.
    _database.OsuDiscordLinks.Add(new OsuDiscordLink(discordId, osuId));
    await _database.SaveChangesAsync();
  }

  /// <summary>
  /// Returns all beatmap aliases.
  /// </summary>
  /// <returns>The beatmap aliases.</returns>
  public async Task<BeatmapAlias[]> GetBeatmapAliasesAsync()
  {
    return await _database.BeatmapAliases.ToArrayAsync();
  }
}