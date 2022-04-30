using Godot;
using System;
namespace Maps {
public class World : Node2D
{
    private TileMap map;
    private TileMap components;
    private System.Collections.ArrayList servers = new System.Collections.ArrayList();
    private System.Collections.Generic.Dictionary<int, int> path_tiles = new System.Collections.Generic.Dictionary<int, int>();
    private const int TILE_SERVER_FLAG = 1;
    private const int TILE_NORTH_FLAG = 1 << 1;
    private const int TILE_EAST_FLAG = 1 << 2;
    private const int TILE_WEST_FLAG = 1 << 3;
    private const int TILE_SOUTH_FLAG = 1 << 4;

    public AStar2D AStar { get; set; }

    public override void _Ready()
    {
        AStar = new AStar2D();
        map = GetNode<TileMap>("Floor");
        components = GetNode<TileMap>("Components");

        foreach ( int texture in map.TileSet.GetTilesIds())
        {
            map.TileSet.TileSetTextureOffset(texture, new Vector2(0, -384));
        }

        generate_map(12);
    }

    public override void _PhysicsProcess(float delta)
    {
        // Debug for AStar
        //if (Input.IsActionJustPressed("ui_accept"))
        //{
        //    var mouse_position = map.WorldToMap(GetGlobalMousePosition());
        //    var mouseTile = AStar.GetClosestPoint(new Vector2(mouse_position.x, mouse_position.y));
        //    var mouseTilePosition = AStar.GetPointPosition(mouseTile);
        //    GD.Print("Tile: " + mouseTile + " (" + mouseTilePosition.x + "," + mouseTilePosition.y + ")");
        //    GD.Print("Connected to: " + string.Join(", ", AStar.GetPointConnections(mouseTile)));
        //}
        if (Input.IsActionJustPressed("ui_select"))
        {
            var mouse_position = map.WorldToMap(GetGlobalMousePosition());
            var mouseTile = AStar.GetClosestPoint(new Vector2(mouse_position.x, mouse_position.y));
            add_server(mouseTile);
        }
    }

    private void generate_map(int hypotenuse)
    {
        int height = hypotenuse / 2;
        for(int y = 0; y < height; y++)
        {
            int length = hypotenuse - (2 * y);
            for (int x = 0; x < length; x++)
            {
                var current_id = AStar.GetAvailablePointId();
                path_tiles[current_id] = 0;
                AStar.AddPoint(current_id, new Vector2(x + y, y), 10F);

                if (x != 0)
                {
                    // Connect line parallel to hypotenuse
                    var parallel_point = AStar.GetClosestPoint(new Vector2(x + y - 1, y));
                    AStar.ConnectPoints(current_id, parallel_point);
                }

                if (y != 0)
                {
                    // Connect line towards hypotenuse
                    var point_towards_hypotenuse = AStar.GetClosestPoint(new Vector2(x + y, y - 1));
                    AStar.ConnectPoints(current_id, point_towards_hypotenuse);

                    // Create point on the other siede of the hypotenuse
                    current_id = AStar.GetAvailablePointId();
                    path_tiles[current_id] = 0;
                    AStar.AddPoint(current_id, new Vector2(x + y, -y), 10F);

                    // Connect line towards hypotenuse
                    AStar.ConnectPoints(current_id, AStar.GetClosestPoint(new Vector2(x + y, -y + 1)));
                    if (x != 0)
                    {
                        // Connect line parallel to hypotenuse
                        AStar.ConnectPoints(current_id, AStar.GetClosestPoint(new Vector2(x + y - 1, -y)));
                    }
                }
            }
        }
        render_floor();
    }

    public void connect_server(int serverATile, int serverBTile)
    {
        // Dont connect to current server
        if (serverATile == serverBTile) { return; }

        // Get path between servers
        var full_path = AStar.GetIdPath(serverATile, serverBTile);

        // Calculate whether added server connects to North, East, South, or West
        var first_position = AStar.GetPointPosition(full_path[0]);
        var second_position = AStar.GetPointPosition(full_path[1]);

        if (first_position.x > second_position.x) { path_tiles[full_path[0]] |= TILE_NORTH_FLAG; }
        if (first_position.y > second_position.y) { path_tiles[full_path[0]] |= TILE_EAST_FLAG; }
        if (first_position.x < second_position.x) { path_tiles[full_path[0]] |= TILE_SOUTH_FLAG; }
        if (first_position.y < second_position.y) { path_tiles[full_path[0]] |= TILE_WEST_FLAG; }


        // Calculate whether other server connects to North, East, South, or West
        var last_index = full_path.Length - 1;
        var last_position = AStar.GetPointPosition(full_path[last_index]);
        var second_last_position = AStar.GetPointPosition(full_path[last_index - 1]);

        if (last_position.x > second_last_position.x) { path_tiles[full_path[last_index]] |= TILE_NORTH_FLAG; }
        if (last_position.y > second_last_position.y) { path_tiles[full_path[last_index]] |= TILE_EAST_FLAG; }
        if (last_position.x < second_last_position.x) { path_tiles[full_path[last_index]] |= TILE_SOUTH_FLAG; }
        if (last_position.y < second_last_position.y) { path_tiles[full_path[last_index]] |= TILE_WEST_FLAG; }

        // Calculate the direction for each path tile
        for (int index = 1; index < full_path.Length - 1; index++)
        {
            var current_tile = full_path[index];
            AStar.SetPointWeightScale(current_tile, 1F);

            var current_point = AStar.GetPointPosition(current_tile);
            var previous_point = AStar.GetPointPosition(full_path[index - 1]);
            var next_point = AStar.GetPointPosition(full_path[index + 1]);

            var hasNorth = current_point.x > previous_point.x || current_point.x > next_point.x;
            var hasEast = current_point.y > previous_point.y || current_point.y > next_point.y;
            var hasSouth = current_point.x < previous_point.x || current_point.x < next_point.x;
            var hasWest = current_point.y < previous_point.y || current_point.y < next_point.y;

            if (hasNorth) { path_tiles[current_tile] |= TILE_NORTH_FLAG; }
            if (hasEast) { path_tiles[current_tile] |= TILE_EAST_FLAG; }
            if (hasSouth) { path_tiles[current_tile] |= TILE_SOUTH_FLAG; }
            if (hasWest) { path_tiles[current_tile] |= TILE_WEST_FLAG; }
        }
        render_floor();
    }

    public void add_server(int tile)
    {
        // Skip if tile already is a server
        if (servers.Contains(tile)) { return; }

        // Add server to known servers and mark floor tile as server
        servers.Add(tile);
        var position = AStar.GetPointPosition(tile);
        path_tiles[tile] = path_tiles[tile] | TILE_SERVER_FLAG;

        if (servers.ToArray().Length > 1)
        {
            connect_server(tile, (int)servers.ToArray()[servers.ToArray().Length - 2]);
        } else
        {
            render_floor();
        }
    }

    private void render_floor()
    {
        foreach (int tile in path_tiles.Keys)
        {
            var mask = path_tiles[tile];
            var point = AStar.GetPointPosition(tile);
            // Only works because of strict naming conventions on Floor TileSet
            // It always starts with floor and has options for Server and each direction
            string texture = "Floor";

            if ((mask & TILE_SERVER_FLAG) > 0) { texture += "Server"; components.SetCell((int)point.x, (int)point.y, 0); }
            if ((mask & TILE_NORTH_FLAG) > 0) { texture += "N"; }
            if ((mask & TILE_EAST_FLAG) > 0) { texture += "E"; }
            if ((mask & TILE_SOUTH_FLAG) > 0) { texture += "S"; }
            if ((mask & TILE_WEST_FLAG) > 0) { texture += "W"; }

            map.SetCell((int) point.x, (int) point.y, map.TileSet.FindTileByName(texture));
        }
    }
}
}
