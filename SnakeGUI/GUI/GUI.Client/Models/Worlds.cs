//</author> Tom Rasmussen and Cade Christensen
//</date> 11/21/2025
using System.Text.Json.Serialization;

namespace GUI.Client.Models;
/// <summary>
/// Represents the complete game world state, including snakes,
/// walls, powerups, and metadata such as world size and dimensions.
/// </summary>
public class Worlds
{
    /// <summary>
    /// Gets or sets the size of the world.
    /// This corresponds to the "WorldSize" field from the server's JSON.
    /// </summary>
    [JsonPropertyName("WorldSize")]
    public int Size { get; set; }
    
    /// <summary>
    /// Gets or sets the unique ID of the player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the world width in pixels.
    /// </summary>
    public int Width { get; set; } = 0;
    
    /// <summary>
    /// Gets or sets the world height in pixels.
    /// </summary>
    public int Height { get; set; } = 0;

    /// <summary>
    /// A collection of all snakes currently active in the world,
    /// indexed by their unique snake ID.
    /// </summary>
    public Dictionary<int, Snakes> Snakes { get; private set; } 

    /// <summary>
    /// A collection of all walls in the world,
    /// indexed by their wall ID.
    /// </summary>
    public Dictionary<int, Wall> Walls { get; private set; } 

    /// <summary>
    /// A collection of active powerups in the world,
    /// indexed by their powerup ID.
    /// </summary>
    public Dictionary<int, Powerups> Powerups { get; private set; } 
    
    /// <summary>
    /// Initializes a new world with the given size and empty collections
    /// for snakes, walls, and powerups.
    /// </summary>
    /// <param name="size">The logical size of the world sent by the server.</param>
    public Worlds(int size)
    {
        Size = size;
        Snakes = new Dictionary<int, Snakes>();
        Walls = new Dictionary<int, Wall>();
        Powerups = new Dictionary<int, Powerups>();
    }

    /// <summary>
    /// Creates a copy of an existing World instance.
    /// Used to safely duplicate the world for rendering without modifying live data.
    /// </summary>
    /// <param name="w">The world to copy.</param>
    public Worlds(Worlds w)
    {
        Size = w.Size;
        PlayerId = w.PlayerId;
        Width = w.Width;
        Height = w.Height;
        Snakes = new Dictionary<int, Snakes>(w.Snakes);
        Walls = new Dictionary<int, Wall>(w.Walls);
        Powerups = new Dictionary<int, Powerups>(w.Powerups);
    }
    
    /// <summary>
    /// Adds or replaces a snake in the world state.
    /// </summary>
    /// <param name="snake">The snake to add or update.</param>
    public void AddSnake(Snakes snake)
    {
        {
            Snakes[snake.Id] = snake;
        }
    }

    /// <summary>
    /// Adds a wall to the world if it does not already exist.
    /// </summary>
    /// <param name="wall">The wall object to add.</param>
    public void AddWall(Wall wall)
    {
        if (!Walls.ContainsKey(wall.Id))
        {
            Walls[wall.Id] = wall;
        }
    }
   
    /// <summary>
    /// Adds a powerup to the world if it does not already exist.
    /// </summary>
    /// <param name="powerup">The powerup object to add.</param>
    public void AddPowerup(Powerups powerup)
    {
        if (!Powerups.ContainsKey(powerup.Id))
        {
            Powerups[powerup.Id] = powerup;
        }
    }
    
    /// <summary>
    /// Removes a powerup from the world by ID.
    /// </summary>
    /// <param name="powerupId">The ID of the powerup to remove.</param>
    public void RemovePowerup(int powerupId)
    {
        if (Powerups.ContainsKey(powerupId))
        {
            Powerups.Remove(powerupId);
        }
    }
}