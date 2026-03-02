// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>
//</author> Tom Rasmussen and Cade Christensen
//</date> 11/5/2025

using System.Net;
using System.Net.Sockets;
using System.Text;
using Networking;

// ReSharper disable once CheckNamespace
namespace Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public abstract class ChatServer
{
    private static readonly  List<NetworkConnection> connections = new ();

    /// <summary>
    ///   The main program.
    /// </summary>
    private static void Main(string[] _)
    {
        Server.StartServer(HandleConnect, 11_000);
        Console.Read(); // don't stop the program.
    }

    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect( NetworkConnection connection )
    {
        lock(connections)
            connections.Add(connection);
        
        string username = "";
        // handle all messages until disconnect.
        try
        {
            username = connection.ReadLine();
            while (connection.IsConnected)
            { 
                var message = connection.ReadLine();
                lock (connections)
                {
                    foreach (NetworkConnection connects in connections)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(message)) //send message
                                connects.Send(username + ": " + message);
                        }
                        catch
                        {
                            connections.Remove(connects);
                            try
                            {
                                connects.Disconnect();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

        }
        catch (Exception e)
        {
        }
        finally //makes sure it disconnects
        {
            lock (connections)
            {
                connections.Remove(connection);
                connection.Disconnect();
            }
            // do anything necessary to handle a disconnected client in here
        }
    }
}