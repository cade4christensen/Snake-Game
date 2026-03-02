//</author> Tom Rasmussen and Cade Christensen
//</date> 11/21/2025
using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// Represents a snake entity in the game world, including its id,
/// position, state, direction, score, and lifecycle information.
/// </summary>
public class Snakes
{
    /// <summary>
    /// Gets or sets the unique identifier for this snake.
    /// </summary>
    [JsonPropertyName("snake")]
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the display name of the snake/player.
    /// </summary>
    [JsonPropertyName("name")]
    public string name { get; set; }
    
    /// <summary>
    /// Gets or sets the ordered list of points representing the snake's body segments.
    /// The last point in the list corresponds to the snake's head.
    /// </summary>
    [JsonPropertyName("body")]
    public List<Point2D> body { get; set; }
    
    /// <summary>
    /// Gets or sets the direction vector the snake is currently moving in.
    /// </summary>
    [JsonPropertyName("dir")]
    public Point2D dir { get; set; }
    
    /// <summary>
    /// Gets or sets the current score of the snake.
    /// </summary>
    [JsonPropertyName("score")]
    public int score { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the snake has just died in the current game frame.
    /// This is used to detect new death events.
    /// </summary>
    [JsonPropertyName("died")]
    public bool died { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the snake is currently alive.
    /// </summary>
    [JsonPropertyName("alive")]
    public bool alive { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the snake has disconnected from the game.
    /// </summary>
    [JsonPropertyName("dc")]
    public bool dc { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this snake has just joined the game.
    /// </summary>
    [JsonPropertyName("join")]
    public bool join { get; set; }
    
    /// <summary>
    /// Gets or sets a boolean indicating if the leave time has been recorded.
    /// </summary>
    [JsonIgnore]
    public bool LeaveTimeRecorded { get; set; } = false;
    
    /// <summary>
    /// A boolean representing whether the player has already been inserted in the SQL database.
    /// </summary>
    [JsonIgnore]
    public bool PlayerInserted { get; set; } = false;
    
    /// <summary>
    /// An integer representing the max score this snake has reached.
    /// </summary>
    [JsonIgnore]
    public int MaxScore { get; set; } = 0;

    /// <summary>
    /// Initializes a new instance of the Snakes class
    /// with an empty body list.
    /// </summary>
    public Snakes()
    {
        body = new List<Point2D>();
    }
}