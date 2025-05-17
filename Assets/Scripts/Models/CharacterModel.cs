using Newtonsoft.Json;
using System;

[Serializable]
public class CharacterModel
{
    [JsonProperty("Id")]
    public string ID {  get; set; }

    [JsonProperty("NewAnimation")]
    public string NewAnimation {  get; set; }

    [JsonProperty("Position")]
    public Position Position { get; set; }

    [JsonProperty("Rotation")]
    public Rotation Rotation { get; set; }

    public CharacterModel()
    {
        ID = string.Empty;
        NewAnimation = string.Empty;

        Position = new Position()
        {
            X = 0,
            Y = 0,
            Z = 0,
        };

        Rotation = new Rotation()
        {
            X = 0,
            Y = 0,
            Z = 0,
        };
    }
}

[Serializable]
public class Position
{
    [JsonProperty("xPos")]
    public float X { get; set; }
    [JsonProperty("yPos")]
    public float Y { get; set; }
    [JsonProperty("zPos")]
    public float Z { get; set; }
}

[Serializable]
public class Rotation
{
    [JsonProperty("xRot")]
    public float X { get; set; }
    [JsonProperty("yRot")]
    public float Y { get; set; }
    [JsonProperty("zRot")]
    public float Z { get; set; }
}
