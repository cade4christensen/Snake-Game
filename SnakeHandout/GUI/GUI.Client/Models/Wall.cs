//</author> Tom Rasmussen and Cade Christensen
//</date> 11/21/2025
using System.Text.Json.Serialization;
namespace GUI.Client.Models;

/// <summary>
/// Represents a wall segment in the game world.
/// A wall is defined by two endpoints p1 and p2
/// and is usually rendered as a series of wall tiles along the line.
/// </summary>
public class Wall
{
    /// <summary>
    /// Gets or sets the unique identifier of the wall.
    /// Mapped from the "wall" field in the server JSON.
    /// </summary>
    [JsonPropertyName("wall")] public int Id { get; set; }

    /// <summary>
    /// Gets or sets the starting endpoint of the wall.
    /// Mapped from the "p1" field in the server JSON.
    /// </summary>
    [JsonPropertyName("p1")] public Point2D p1 { get; set; }

    /// <summary>
    /// Gets or sets the ending endpoint of the wall.
    /// Mapped from the "p2" field in the server JSON.
    /// </summary>
    [JsonPropertyName("p2")] public Point2D p2 { get; set; }

    /// <summary>
    /// Initializes an empty wall instance.
    /// This constructor is required for JSON deserialization.
    /// </summary>
    public Wall()
    {

    }

}

