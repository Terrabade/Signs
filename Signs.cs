using System.Globalization;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Signs;
[ApiVersion(2, 1)]
public class Signs : TerrariaPlugin
{
    public override string Name => "Signs";
    public override string Author => "Terrabade";
    public override string Description => "Converts sign coordinates + text contents into a timestamped text transcript.";
    public override Version Version => new Version(1, 0, 0);
    public Signs(Main game) : base(game) { }

    public override void Initialize()
    {
        Directory.CreateDirectory(Path.Combine(TShock.SavePath, this.Name));
        Commands.ChatCommands.Add(new Command("signs.convert", (args) => {
            using (var writer = File.CreateText(Path.Combine(TShock.SavePath, this.Name, $"signs-transcript-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}.txt")))
            {
                int signLength = Main.sign.Length;
                int nullCount = 0;
                int textEmptyCount = 0;
                for (int i = 0; i < signLength; i++)
                {
                    var sign = Main.sign[i];
                    if (sign != null && sign.text != null && sign.text != "")
                        writer.WriteLine($"[X: {sign.x} Y: {sign.y}] {sign.text}");
                    else if (sign == null)
                        nullCount++;
                    else if (sign.text == "")
                        textEmptyCount++;
                    
                }
                args.Player.SendSuccessMessage($"{signLength} chests were indexed.\n{nullCount} sign spaces are available on the map.\n{textEmptyCount} signs have no text.");
            }
            
        }, "convertsigns"));
    }

}