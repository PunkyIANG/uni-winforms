using System.Text.Json;
using System.Windows.Forms;
using GPUProject.lab1v2;
using GPUProject.Resources;

namespace lab2;

public partial class GpuForm : Form
{
    public readonly GraphicsCard? CurrentGpu;

    public MemoryManufacturer MemoryManufacturer
    {
        get => CurrentGpu.Memory.manufacturer;
        set
        {
            if (value != CurrentGpu.Memory.manufacturer)
                CurrentGpu.Memory = CurrentGpu.Memory with { manufacturer = value };
        }
    }

    public bool IsSkHynixManufacturer
    {
        get => CurrentGpu.Memory.manufacturer == MemoryManufacturer.SKHynix;
        set
        {
            if (value)
            {
                CurrentGpu.Memory = CurrentGpu.Memory with { manufacturer = MemoryManufacturer.SKHynix };
                // Console.WriteLine($"SKHynix {value}");
            }
        }
    }

    public bool IsMicronManufacturer
    {
        get => CurrentGpu.Memory.manufacturer == MemoryManufacturer.Micron;
        set
        {
            if (value)
            {
                CurrentGpu.Memory = CurrentGpu.Memory with { manufacturer = MemoryManufacturer.Micron };
                // Console.WriteLine($"Micron {value}");
            }
        }
    }

    public bool IsSamsungManufacturer
    {
        get => CurrentGpu.Memory.manufacturer == MemoryManufacturer.Samsung;
        set
        {
            if (value)
            {
                CurrentGpu.Memory = CurrentGpu.Memory with { manufacturer = MemoryManufacturer.Samsung };
                // Console.WriteLine($"Samsung {value}");
            }
        }
    }

    public MemoryType MemoryType
    {
        get => CurrentGpu.Memory.type;
        set
        {
            if (value != CurrentGpu.Memory.type)
                CurrentGpu.Memory = CurrentGpu.Memory with { type = value };
        }
    }

    public byte MemorySize
    {
        get => CurrentGpu.Memory.size;
        set
        {
            if (value != CurrentGpu.Memory.size)
            {
                CurrentGpu.Memory = CurrentGpu.Memory with { size = value };
                memorySizeLabel.Text = $"Size: {value} GB";
            }
        }
    }

    private Label memorySizeLabel;

    public GpuForm() : this(new GraphicsCard())
    {
    }

    public GpuForm(GraphicsCard? inputGpu)
    {
        inputGpu ??= new GraphicsCard();

        CurrentGpu = inputGpu;
        InitializeComponent();

        AutoSize = true;
        MinimumSize = new Size(543, 320);
        Size = new Size(543, 320);
        MaximumSize = new Size(543, 320);

        this.SetBind(nameof(Text), CurrentGpu, nameof(CurrentGpu.Model));

        #region col1

        var col1 = new Panel
        {
            Dock = DockStyle.Left,
            AutoSize = true,
            // Margin = new Padding(5)
        };

        var manufacturerDropDown = new ComboBox
        {
            Location = new Point(0, 0),
            Width = 160,
            Height = 40,
            DataSource = Enum.GetValues<Manufacturer>(),
        };
        manufacturerDropDown.SetBind(nameof(ComboBox.SelectedItem),
            CurrentGpu, nameof(CurrentGpu.Manufacturer));
        col1.Controls.Add(manufacturerDropDown);

        var modelTextBox = new TextBox
        {
            Location = new Point(0, 40),
            Width = 160,
            Height = 40,
            PlaceholderText = "Model",
        };
        modelTextBox.SetBind(nameof(modelTextBox.Text), CurrentGpu, nameof(CurrentGpu.Model));

        col1.Controls.Add(modelTextBox);

        Control[] outputButtons =
        {
            new Button
            {
                Text = "VGA",
                Location = new Point(0, 80),
                Width = 80,
                Height = 40,
            },
            new Button
            {
                Text = "DVI",
                Location = new Point(80, 80),
                Width = 80,
                Height = 40,
            },
            new Button
            {
                Text = "HDMI",
                Location = new Point(0, 120),
                Width = 80,
                Height = 40,
            },
            new Button
            {
                Text = "DP",
                Location = new Point(80, 120),
                Width = 80,
                Height = 40,
            },
        };

        outputButtons[0].Click += (_, _) => CurrentGpu.OutputTypes.Add(OutputType.VGA);
        outputButtons[1].Click += (_, _) => CurrentGpu.OutputTypes.Add(OutputType.DVI);
        outputButtons[2].Click += (_, _) => CurrentGpu.OutputTypes.Add(OutputType.HDMI);
        outputButtons[3].Click += (_, _) => CurrentGpu.OutputTypes.Add(OutputType.DisplayPort);

        col1.Controls.AddRange(outputButtons);


        var outputList = new ListBox
        {
            Location = new Point(0, 160),
            Height = 110,
            Width = 160,
            DataSource = CurrentGpu.OutputTypes,
        };

        outputList.MouseDoubleClick += (_, _) =>
        {
            if (outputList.SelectedIndex != ListBox.NoMatches)
                CurrentGpu.OutputTypes.RemoveAt(outputList.SelectedIndex);
        };

        col1.Controls.Add(outputList);

        #endregion

        #region col2

        var col2 = new Panel
        {
            Dock = DockStyle.Left,
            AutoSize = true,
            // Margin = new Padding(5)
        };

        var resolutionCheckBoxes = new[]
        {
            new CheckBox
            {
                Location = new Point(5, 0),
                Text = "Full HD",
                Height = 35,
                Width = 155,
            },
            new CheckBox
            {
                Location = new Point(5, 35),
                Text = "1440p",
                Height = 35,
                Width = 155,
            },
            new CheckBox
            {
                Location = new Point(5, 70),
                Text = "4K",
                Height = 35,
                Width = 155,
            },
        };

        resolutionCheckBoxes[0].SetBind(nameof(CheckBox.Checked),
            CurrentGpu.RecommendedResolutions, nameof(CurrentGpu.RecommendedResolutions.FullHD));
        resolutionCheckBoxes[1].SetBind(nameof(CheckBox.Checked),
            CurrentGpu.RecommendedResolutions, nameof(CurrentGpu.RecommendedResolutions.TwoK));
        resolutionCheckBoxes[2].SetBind(nameof(CheckBox.Checked),
            CurrentGpu.RecommendedResolutions, nameof(CurrentGpu.RecommendedResolutions.FourK));

        col2.Controls.AddRange(resolutionCheckBoxes);

        var priceLabel = new Label
        {
            Location = new Point(0, 106),
            Text = "Price:",
            Width = 70,
            Height = 35,
        };
        col2.Controls.Add(priceLabel);

        var priceNumericUpDown = new NumericUpDown
        {
            Location = new Point(70, 105),
            Maximum = 2000,
            Width = 91,
            Height = 35,
        };
        col2.Controls.Add(priceNumericUpDown);

        priceNumericUpDown.SetBind(nameof(NumericUpDown.Value), CurrentGpu, nameof(CurrentGpu.Price));

        var baseClockLabel = new Label
        {
            Location = new Point(0, 141),
            Text = "Clock:",
            Width = 70,
            Height = 35,
        };
        col2.Controls.Add(baseClockLabel);

        var baseClockNumericUpDown = new NumericUpDown
        {
            Location = new Point(70, 140),
            Maximum = 3000,
            Width = 91,
            Height = 35,
        };
        col2.Controls.Add(baseClockNumericUpDown);

        baseClockNumericUpDown.SetBind(nameof(NumericUpDown.Value), CurrentGpu, nameof(CurrentGpu.BaseClock));

        var productionCheckbox = new CheckBox
        {
            Location = new Point(5, 175),
            Text = "Is in active\r\nproduction",
            Height = 65,
            Width = 155,
        };
        col2.Controls.Add(productionCheckbox);

        productionCheckbox.SetBind(nameof(CheckBox.Checked), CurrentGpu, nameof(CurrentGpu.IsInActiveProduction));

        #endregion

        #region col3

        var col3 = new Panel
        {
            Dock = DockStyle.Left,
            AutoSize = true,
            // Margin = new Padding(5)
        };

        var memoryTypeDropDown = new ComboBox
        {
            DataSource = Enum.GetValues<MemoryType>(),
            // DisplayMember = "MemoryType",
            // ValueMember = "MemoryType",
            Width = 160,
        };
        memoryTypeDropDown.SetBind(nameof(ComboBox.SelectedItem),
            this, nameof(MemoryType));
        col3.Controls.Add(memoryTypeDropDown);

        var radioButtons = new[]
        {
            new RadioButton
            {
                Location = new Point(0, 40),
                Height = 30,
                Width = 160,
                Text = "SKHynix",
            },
            new RadioButton
            {
                Location = new Point(0, 70),
                Height = 30,
                Width = 160,
                Text = "Micron",
            },
            new RadioButton
            {
                Location = new Point(0, 100),
                Height = 30,
                Width = 160,
                Text = "Samsung",
            }
        };

        radioButtons[0].SetBind(nameof(RadioButton.Checked), this, nameof(IsSkHynixManufacturer));
        radioButtons[1].SetBind(nameof(RadioButton.Checked), this, nameof(IsMicronManufacturer));
        radioButtons[2].SetBind(nameof(RadioButton.Checked), this, nameof(IsSamsungManufacturer));

        foreach (var radioButton in radioButtons)
            radioButton.CheckedChanged += (_, _) =>
            {
                MemoryManufacturer[] values =
                {
                    MemoryManufacturer.SKHynix,
                    MemoryManufacturer.Micron,
                    MemoryManufacturer.Samsung
                };
        
                for (var i = 0; i < radioButtons.Length; i++)
                    if (radioButtons[i].Checked)
                    {
                        MemoryManufacturer = values[i];
                        return;
                    }
            };

        col3.Controls.AddRange(radioButtons);

        //TODO: bind this to mem size
        //so that it shows Size: 5 GB
        memorySizeLabel = new Label
        {
            Location = new Point(0, 130),
            Text = $"Size: {CurrentGpu.Memory.size} GB",
            Width = 120,
            Height = 30
        };
        col3.Controls.Add(memorySizeLabel);

        var memorySizeTrackBar = new TrackBar
        {
            Location = new Point(10, 170),
            Size = new Size(130, 45),
            Maximum = 16,
            Minimum = 1,
            TickFrequency = 1,
            LargeChange = 1,
            SmallChange = 1,
        };
        memorySizeTrackBar.SetBind(nameof(memorySizeTrackBar.Value),
            this, nameof(MemorySize));
        col3.Controls.Add(memorySizeTrackBar);

        var showButton = new Button
        {
            Location = new Point(0, 200),
            Height = 35,
            Text = "show",
        };
        showButton.Click += (_, _) =>
            MessageBox.Show(JsonSerializer.Serialize(CurrentGpu));
        col3.Controls.Add(showButton);

        #endregion

        Controls.Add(col3);
        Controls.Add(col2);
        Controls.Add(col1);
    }
}