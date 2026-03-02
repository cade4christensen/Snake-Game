//</author> Tom Rasmussen and Cade Christensen
//</date> 11/21/2025
using System.Text.Json.Serialization;

namespace GUI.Client.Models;

/// <summary>
/// Represents a 2-dimensional point with integer X and Y coordinates.
/// </summary>
public class Point2D
{
    /// <summary>
    /// Gets or sets the X-coordinate of the point.
    /// </summary>
    [JsonPropertyName("X")]
    public int X  { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate of the point.
    /// </summary>
    [JsonPropertyName("Y")]
    public int Y { get; set; }

    /// <summary>
    /// Initializes a new instance of the Point2D class
    /// with default coordinates (0, 0).
    /// </summary>
    public Point2D()
    {
    }
}