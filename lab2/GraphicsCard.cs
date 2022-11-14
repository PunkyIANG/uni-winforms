using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GPUProject.Resources;

static class InitValues
{
    public static string[] nvidiaModels = {
            "GTX 1660",
            "GTX 1060",
            "GT 1030",
            "GTX 1050Ti",
        };
    public static string[] amdModels = {
            "RX580",
            "Vega 56",
            "Radeon 7",
        };
    public static string[] intelModels = {
            "Iris Xe",
            "UH630"
        };
}
class GraphicsCard
{
    #region getsetboilerplate

    public Manufacturer Manufacturer { get; set; }
    public string Model { get; set; }
    public BindingList<OutputType> OutputTypes { get; set; }
    public ResolutionsRepresentation RecommendedResolutions { get; set; }
    public decimal Price { get; set; }
    public uint BaseClock { get; set; }
    public Memory Memory { get; set; }
    public bool IsInActiveProduction { get; set; }
    #endregion

    public GraphicsCard()
    {
        RecommendedResolutions = new();
        Model = "New GPU";
        OutputTypes = new BindingList<OutputType>();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    #region done
    public static GraphicsCard[] GenerateGraphicsCards(int count)
    {
        var rng = new Random(80085);
        var results = new GraphicsCard[count];

        foreach (ref var graphicsCard in results.AsSpan())
        {
            var manufacturer = (Manufacturer)(rng.Next(3));
            string[] selectedArr;

            switch (manufacturer)
            {
                case Manufacturer.Nvidia:
                    selectedArr = InitValues.nvidiaModels;
                    break;
                case Manufacturer.AMD:
                    selectedArr = InitValues.amdModels;
                    break;
                case Manufacturer.Intel:
                    selectedArr = InitValues.intelModels;
                    break;
                default:
                    selectedArr = InitValues.nvidiaModels;
                    break;
            }

            string model = selectedArr[rng.Next() % selectedArr.Length];

            var outputTypes = new BindingList<OutputType>();
            // foreach (ref var outputType in outputTypes)
            outputTypes.Add((OutputType)(rng.Next(4)));

            var recommendedResolutions = new ResolutionsRepresentation(rng.Next(8));
            var price = (decimal)rng.NextSingle() * 500;
            var baseClock = (uint)rng.Next(1000) + 1000;
            var memory = new Memory
            {
                type = (MemoryType)(rng.Next(4)),
                manufacturer = (MemoryManufacturer)(rng.Next(3)),
                size = (byte)rng.Next(12),
            };

            var isInActiveProduction = rng.Next(2) == 0;


            graphicsCard = new GraphicsCard
            {
                Manufacturer = manufacturer,
                Model = model,
                OutputTypes = outputTypes,
                RecommendedResolutions = recommendedResolutions,
                Price = price,
                BaseClock = baseClock,
                Memory = memory,
                IsInActiveProduction = isInActiveProduction,
            };
        }



        return results;
    }

    public static bool TryReadGPU(string path, out GraphicsCard graphicsCard)
    {
        // TODO: actually handle exceptions
        try
        {
            graphicsCard = JsonSerializer.Deserialize<GraphicsCard>(File.ReadAllText(path));
            return true;
        }
        catch (Exception e)
        {
            graphicsCard = new GraphicsCard();
            Console.WriteLine(e);
            return false;
        }
    }

    public static void WriteGPU(string path, GraphicsCard gpu)
    {
        File.WriteAllText(path, JsonSerializer.Serialize<GraphicsCard>(gpu));
    }
    #endregion
}

#region declarations

public struct Memory
{
    public MemoryType type { get; set; }
    public MemoryManufacturer manufacturer { get; set; }
    public byte size { get; set; }
}

[Flags]
public enum RecommendedResolutions
{
    None = 0,
    FullHD = 1 << 0,
    TwoK = 1 << 1,
    FourK = 1 << 2,
}

public enum Manufacturer
{
    Nvidia,
    AMD,
    Intel,
}

public enum MemoryType
{
    DDR4,
    GDDR5,
    GDDR6,
    GDDR6X,
}

public enum MemoryManufacturer
{
    SKHynix,
    Micron,
    Samsung,
}

public enum OutputType
{
    VGA,
    DVI,
    HDMI,
    DisplayPort,
}

#endregion


public class ResolutionsRepresentation
{
    public ResolutionsRepresentation()
    {

    }
    public ResolutionsRepresentation(int value)
    {
        FullHD = (value & 1) == 1 ? true : false;
        TwoK = (value & 2) == 2 ? true : false;
        FourK = (value & 4) == 2 ? true : false;
    }
    public bool FullHD { get; set; }
    public bool TwoK { get; set; }
    public bool FourK { get; set; }
}