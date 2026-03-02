//</author> Tom Rasmussen and Cade Christensen
//</date> 12/4/2025
using System.Net;
using System.Net.Sockets;
using System.Text;
using GUI.Client.Controllers;
using MySql.Data.MySqlClient;
using Snake.Controllers;

namespace WebServer;

/// <summary>
/// Class representing the Webserver that displays the connected SQL Database.
/// </summary>
public static class WebServer
{
    /// <summary>
    /// String representing the link to the connected database.
    /// </summary>
    private const string _connectionString = "server=atr.eng.utah.edu;database=u1408800;uid=u1408800;password=gitonmylevel;Port=3306;";
    
    /// <summary>
    /// Initiates the webserver and begins the loop to connect it to the client.
    /// </summary>
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 80);
        listener.Start();

        //from ps8 server
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient(); 
            NetworkConnection connection = new NetworkConnection(client);
            new Thread(()=>HandleHttpConnection(connection)).Start();
        }

    }

    /// <summary>
    /// Builds and sends the HTTP to the browser over connection
    /// </summary>
    /// <param name="client"></param>
    /// <param name="html"></param>
    private static void SendHttp(NetworkConnection client, string html)
    {
        var encoding = new UTF8Encoding(false);
        int length = encoding.GetByteCount(html);
        //from webserver demo lecture
        string goodHeader = "HTTP/1.1 200 OK\r\n" +
                            "Connection: close\r\n" +
                            "Content-Type: text/html; charset=UTF-8\r\n" +
                            "Content-Length: " + length + "\r\n" + "\r\n";
        client.Send(goodHeader + html);
    }

    /// <summary>
    /// Helper method to handle the http connection and parse request to the correct
    /// web page in order to send html response.
    /// </summary>
    /// <param name="client"></param>
    private static void HandleHttpConnection(NetworkConnection client)
    {
        string message = client.ReadLine();

        if (string.IsNullOrWhiteSpace(message))
        {
            client.Disconnect();
            return;
        }

        try
        {
            if (message.Contains("GET /games?gid="))
            {
                string[] content = message.Split(' ');
                string url = content[1];
                string[] urlSplit = url.Split("gid=");
                string gameIDString = urlSplit[1];
                int gameID = int.Parse(gameIDString);
                SendHttp(client, HandleSpecificGamePage(gameID));
            }

            else if (message.Contains("GET /games "))
            {
                SendHttp(client, HandleAllGamesPage());
            }

            else if (message.Contains("GET / "))
            {
                SendHttp(client, HandleHomePage());
            }
            else
            {
                //load in basic error textpage for error handling
                SendHttp(client, "<html><h3>Error loading page</h3></html>");
            }
        }
        catch (Exception ex)
        {
            
        }

        client.Disconnect();
    }
    
    /// <summary>
    /// Creates the home page with HTML
    /// </summary>
    /// <returns></returns>
    private static string HandleHomePage()
    {
        return "<html>" +
            "<h3>Welcome to the Snake Games Database!</h3>" +
            "<a href=\"/games\">View Games</a>" +
            "</html>";
    }

    /// <summary>
    /// Creates the game page with all the games using HTML
    /// </summary>
    /// <returns></returns>
    private static string HandleAllGamesPage()
    {
        //Creates top of the page
        string html =
            "<html>" + 
            "<table border=\"1\">" +
            "<thead>" +
            "<tr>" +
            "<td>ID</td><td>Start</td><td>End</td>" +
            "</tr>" +
            "</thead>" +
            "<tbody>";
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var command = new MySqlCommand("SELECT id, start_time,end_time FROM Games", connection);
            
            using var reader = command.ExecuteReader();

            //Creates the table
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                DateTime start = reader.GetDateTime(1);
                string end = reader.IsDBNull(2) ? "" : reader.GetDateTime(2).ToString();
                
                html +="<tr>";
                html += $"<td><a href=\"/games?gid={id}\">{id}</a></td>";
                html += $"<td>{start}</td>";
                html += $"<td>{end}</td>";
                html +="</tr>";
            }
            html +="</tbody></table></html>";
            return html;
    }

    /// <summary>
    /// Creates the game page for a specific game
    /// </summary>
    /// <param name="gid"></param>
    /// <returns></returns>
    private static string HandleSpecificGamePage(int gid)
    {
        //Creates top of page
        string html =
            "<html>" +
            $"<h3>Stats for Game {gid}</h3>" +
            "<table border=\"1\">" +
            "<thead>" +
            "<tr>" +
            "<td>Player ID</td><td>Player Name</td><td>Max Score</td><td>Enter Time</td><td>Leave Time</td>" +
            "</tr>" +
            "</thead>" +
            "<tbody>";

        //Uses database
        var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        var command = new MySqlCommand("SELECT id, name, max_score, enter_time, leave_time FROM Players WHERE game_id = @gid", connection);
        
        command.Parameters.AddWithValue("@gid", gid); 
        
        using var reader = command.ExecuteReader();

        //Creates the table
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int score  = reader.GetInt32(2);
            DateTime enter =  reader.GetDateTime(3);
            string leave = reader.IsDBNull(4) ? "" : reader.GetDateTime(4).ToString();
            
            html +="<tr>";
            html += $"<td>{id}</td>";
            html += $"<td>{name}</td>";
            html += $"<td>{score}</td>";
            html += $"<td>{enter}</td>";
            html += $"<td>{leave}</td>";
            html +="</tr>";
        }

        html +="</tbody></table></html>";
        return html;
    }
}