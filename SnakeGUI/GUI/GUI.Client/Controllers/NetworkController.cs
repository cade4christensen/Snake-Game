//</author> Tom Rasmussen and Cade Christensen
//</date> 11/21/2025    
using GUI.Client.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Snake.Controllers;

namespace GUI.Client.Controllers;

public class NetworkController
{
    /// <summary>
    /// The underlying TCP network connection used for server communication.
    /// </summary>
    private NetworkConnection _connection;

    /// <summary>
    /// Indicates whether the client is currently connected to the server.
    /// </summary>
    public bool IsConnected { get; private set; } = false;
    
    /// <summary>
    /// Disconnects from the server and updates the connection state.
    /// </summary>
    public async Task DisconnectAsync()
    {
        _connection.Disconnect();
        IsConnected = false;
    }

    /// <summary>
    /// Establishes a connection to the game server, sends the player's name,
    /// receives initial world metadata (player ID and world size),
    /// and begins listening for continuous game updates.
    /// </summary>
    /// <param name="server">The server hostname or IP address.</param>
    /// <param name="port">The server port.</param>
    /// <param name="playerName">The local player's name.</param>
    /// <param name="world">The shared world model to update with server data.</param>
    public async Task NetworkConnectAsync(string server, int port, string playerName, Worlds world)
    {
        try
        {
            _connection = new NetworkConnection();
            _connection.Connect(server, port);

            _connection.Send(playerName); //PLAYER NAME FROM VIEW GUI
            world.PlayerId = int.Parse(_connection.ReadLine()); //FIRST INPUT FROM SERV ID
            world.Size = int.Parse(_connection.ReadLine()); //SECOND INPUT FROM SERV SIZE
            world.Height = world.Size;
            world.Width = world.Size;
            IsConnected = true;
            await RecieveGameData(world);
        }
        catch (Exception ex)
        {
            IsConnected = false;
        }
    }

    /// <summary>
    /// Continuously receives JSON game updates from the server and forwards them
    /// for processing.
    /// </summary>
    /// <param name="world">The world model instance to update.</param>
    private async Task RecieveGameData(Worlds world)
    {
        while (IsConnected)
        {
            try
            {
                HandleIncomingMessage(_connection.ReadLine(),world); //JSON INPUT FROM SERV
            }
            catch (Exception e)
            {
                IsConnected = false;
            }
        }
    }

    /// <summary>
    /// Processes a block of JSON sent by the server, identifies its type
    /// (wall, snake, or powerup), deserializes it, and updates the world model.
    /// Handles add, update, and removal logic based on object state.
    /// </summary>
    /// <param name="serverJson">Raw JSON data received from the server.</param>
    /// <param name="world">The world instance to apply updates to.</param>
    private void HandleIncomingMessage(string serverJson, Worlds world)
    {
        // Incoming server message may contain multiple JSON objects separated by newlines.
        // Split them into individual JSON lines.
        string[] jsonSplitLines = serverJson.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (string jsonLine in jsonSplitLines)
        {
            //Walls
            if (jsonLine.Contains("\"wall\""))
            {
                // Deserialize wall JSON into a Wall object
                Wall? w = JsonSerializer.Deserialize<Wall>(jsonLine);
                lock (world)
                {
                    world.AddWall(w);
                }
            }
            
            //Snakes
            else if (jsonLine.Contains("\"snake\""))
            {
                
                Snakes? s = JsonSerializer.Deserialize<Snakes>(jsonLine);
                lock (world)
                {
                        world.AddSnake(s);
                }
            }
            
            //Powerups
            else if (jsonLine.Contains("\"power\""))
            {
                Powerups? p = JsonSerializer.Deserialize<Powerups>(jsonLine);
                lock (world)
                {
                    if (p.died)
                    {
                        world.RemovePowerup(p.Id);
                    }
                    else
                    {
                        world.AddPowerup(p);
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Represents a JSON control command sent to the server to update player movement.
    /// </summary>
    private class ControlCommand
    {
        [JsonPropertyName("moving")] 
        public string moving { get; set; } = "none";
    }

    /// <summary>
    /// Sends a movement command (generated from a keyboard input event)
    /// to the server in JSON form. Direction is sent here from GUI (View).
    /// </summary>
    /// <param name="direction">The movement direction (e.g., "up", "down", "left", "right").</param>
    public void SendKeystrokeFromView(string direction)
    {
        ControlCommand control = new ControlCommand
        {
            moving = direction
        };
        string json = JsonSerializer.Serialize(control);
        _connection.Send(json); //SEND MOVEMENT TO SERV
    }
}