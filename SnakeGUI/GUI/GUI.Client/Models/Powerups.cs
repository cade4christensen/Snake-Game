//</author> Tom Rasmussen and Cade Christensen
//</date> 11/21/2025
using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// Represents a powerup item in the game world, including its location,
/// identity, and whether it has been consumed or removed.
/// </summary>
public class Powerups
{
    /// <summary>
    /// Gets or sets the unique identifier for this powerup.
    /// </summary>
    [JsonPropertyName("power")]
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the location of the powerup in the game world.
    /// </summary>
    [JsonPropertyName("loc")]
    public Point2D loc { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the powerup has been consumed
    /// or removed from the game.
    /// </summary>
    [JsonPropertyName("died")]
    public bool died  { get; set; }

    /// <summary>
    /// Initializes a new instance of the Powerups class
    /// with a default location.
    /// </summary>
    public Powerups()
    {
        loc = new Point2D();
    }
}