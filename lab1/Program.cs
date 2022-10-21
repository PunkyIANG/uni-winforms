using System.Text.Json;
using GPUProject.Resources;

namespace GPUProject.Lab1;

internal class Program
{
    static JsonSerializerOptions options = new JsonSerializerOptions();
    static void Main(string[] args)
    {
        options.WriteIndented = true;
        // Console.WriteLine(JsonSerializer.Serialize<GraphicsCard>(GraphicsCard.GenerateGraphicsCards(10)[0], options));

        var gpu = GraphicsCard.GenerateGraphicsCards(1)[0];
        string path = "gpu.json";

        WriteGPU(path, gpu);
        TryReadGPU(path, out var newGpu);

        Console.WriteLine(Environment.CurrentDirectory);
        Console.WriteLine(JsonSerializer.Serialize<GraphicsCard>(newGpu, options));
    }

    static bool TryReadGPU(string path, out GraphicsCard graphicsCard)
    {
        // TODO: actually handle exceptions
        graphicsCard = JsonSerializer.Deserialize<GraphicsCard>(File.ReadAllText(path));
        return true;
    }

    static void WriteGPU(string path, GraphicsCard gpu)
    {
        File.WriteAllText(path, JsonSerializer.Serialize<GraphicsCard>(gpu, options));
    }
}