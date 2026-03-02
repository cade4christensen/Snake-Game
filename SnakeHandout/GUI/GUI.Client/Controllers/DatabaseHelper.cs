//</author> Tom Rasmussen and Cade Christensen
//</date> 12/4/2025
using MySql.Data.MySqlClient;

namespace GUI.Client.Controllers;

/// <summary>
/// A class containing the helper methods used to interact with the connected SQL databse.
/// </summary>
public class DatabaseHelper
{
    /// <summary>
    /// String representing the link to the SQL Database.
    /// </summary>
    private const string _connectionString = "server=atr.eng.utah.edu;database=u1408800;uid=u1408800;password=gitonmylevel;Port=3306;";
    
    /// <summary>
    /// Asynchronous operation to add a given player to the SQL databse.
    /// </summary>
    /// <param name="playerId">integer representing a given player id</param>
    /// <param name="playerName">string representing a given player name</param>
    /// <param name="gameId">integer representing the given game id</param>
    public async Task AddPlayerAsync(int playerId, string playerName, int gameId)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var command = new MySqlCommand(
            @"INSERT INTO Players 
          (id, name, max_score, enter_time, game_id) 
          VALUES (@id, @name, 0, @enter, @gid)", 
            connection);
        
        command.Parameters.AddWithValue("@id", playerId);
        command.Parameters.AddWithValue("@name", playerName);
        command.Parameters.AddWithValue("@enter", 
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@gid", gameId);
        
        await command.ExecuteNonQueryAsync();
    }
    
    /// <summary>
    /// Asynchronous operation to update the given player score in a given game to a given score in the SQL database.
    /// </summary>
    /// <param name="playerId">integer representing a given player id</param>
    /// <param name="gameId">integer representing the given game id</param>
    /// <param name="newScore">holds the given score of the player to update</param>
    public async Task UpdatePlayerScoreAsync(int playerId, int gameId, int newScore)
    {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var command = new MySqlCommand(
                @"UPDATE Players 
                SET max_score = GREATEST(max_score, @score)
                WHERE id = @pid AND game_id = @gid",
                connection);
            command.Parameters.AddWithValue("@score", newScore);
            command.Parameters.AddWithValue("@pid", playerId);
            command.Parameters.AddWithValue("@gid", gameId);
            
            await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Asynchronous operation to add a new game to the SQL database.
    /// </summary>
    /// <returns></returns>
    public async Task<int> AddGameAsync()
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand(
            "INSERT INTO Games (start_time) VALUES (@start); SELECT LAST_INSERT_ID();", connection);
        
        command.Parameters.AddWithValue("@start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    /// <summary>
    /// Asynchronous operation to update the time the given player left the game
    /// </summary>
    /// <param name="playerId">integer representing a given player id</param>
    /// <param name="gameId">integer representing the given game id</param>
    /// <param name="leave">a DateTime representing when the player left</param>
    public async Task UpdateLeaveTimeAsync(int playerId, int gameId, DateTime leave)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new MySqlCommand(
      @"UPDATE Players 
                SET leave_time = @leave 
                WHERE id = @pid AND game_id = @gid", 
            connection);
        
        command.Parameters.AddWithValue("@leave", leave.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@pid", playerId);
        command.Parameters.AddWithValue("@gid", gameId);
        
        await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Asynchronous operation to End a game that is currently in the SQL
    /// </summary>
    /// <param name="gameId">integer representing the given game id</param>
    public async Task EndGameAsync(int gameId)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var command = new MySqlCommand("UPDATE Games SET end_time = @end WHERE id = @gid; " +
                                       "UPDATE Players SET leave_time = @end WHERE game_id = @gid AND leave_time IS NULL;", connection);
        
        
        command.Parameters.AddWithValue("@end", 
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@gid", gameId);

        await command.ExecuteNonQueryAsync();
    }
}