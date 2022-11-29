using System.Text.Json;
using System.Windows.Forms;
using GPUProject.lab1v2;
using GPUProject.Resources;

namespace lab2;

public partial class GpuForm : Form
{
    private readonly GraphicsCard _currentGpu;

    public MemoryManufacturer MemoryManufacturer
    {
        get => _currentGpu.Memory.manufacturer;
        set
        {
            if (value != _currentGpu.Memory.manufacturer)
                _currentGpu.Memory = _currentGpu.Memory with { manufacturer = value };
        }
    }

    public MemoryType MemoryType
    {
        get => _currentGpu.Memory.type;
        set
        {
            if (value != _currentGpu.Memory.type)
                _currentGpu.Memory = _currentGpu.Memory with { type = value };
        }
    }

    public byte MemorySize
    {
        get => _currentGpu.Memory.size;
        set
        {
            if (value != _currentGpu.Memory.size)
                _currentGpu.Memory = _currentGpu.Memory with { size = value };
        }
    }

    public GpuForm() : this(new GraphicsCard())
    {
    }

    public GpuForm(GraphicsCard inputGpu)
    {
        _currentGpu = inputGpu;
        InitializeComponent();

        AutoSize = true;
        MinimumSize = new Size(543, 320);
        Size = new Size(543, 320);
        MaximumSize = new Size(543, 320);

        this.SetBind(nameof(Text), _currentGpu, nameof(_currentGpu.Model));

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
            _currentGpu, nameof(_currentGpu.Manufacturer));
        col1.Controls.Add(manufacturerDropDown);

        var modelTextBox = new TextBox
        {
            Location = new Point(0, 40),
            Width = 160,
            Height = 40,
            PlaceholderText = "Model",
        };
        modelTextBox.SetBind(nameof(modelTextBox.Text), _currentGpu, nameof(_currentGpu.Model));

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

        outputButtons[0].Click += (_, _) => _currentGpu.OutputTypes.Add(OutputType.VGA);
        outputButtons[1].Click += (_, _) => _currentGpu.OutputTypes.Add(OutputType.DVI);
        outputButtons[2].Click += (_, _) => _currentGpu.OutputTypes.Add(OutputType.HDMI);
        outputButtons[3].Click += (_, _) => _currentGpu.OutputTypes.Add(OutputType.DisplayPort);

        col1.Controls.AddRange(outputButtons);


        var outputList = new ListBox
        {
            Location = new Point(0, 160),
            Height = 110,
            Width = 160,
            DataSource = _currentGpu.OutputTypes,
        };

        outputList.MouseDoubleClick += (_, _) =>
        {
            if (outputList.SelectedIndex != ListBox.NoMatches)
                _currentGpu.OutputTypes.RemoveAt(outputList.SelectedIndex);
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
            _currentGpu.RecommendedResolutions, nameof(_currentGpu.RecommendedResolutions.FullHD));
        resolutionCheckBoxes[1].SetBind(nameof(CheckBox.Checked),
            _currentGpu.RecommendedResolutions, nameof(_currentGpu.RecommendedResolutions.TwoK));
        resolutionCheckBoxes[2].SetBind(nameof(CheckBox.Checked),
            _currentGpu.RecommendedResolutions, nameof(_currentGpu.RecommendedResolutions.FourK));

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

        priceNumericUpDown.SetBind(nameof(NumericUpDown.Value), _currentGpu, nameof(_currentGpu.Price));

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

        baseClockNumericUpDown.SetBind(nameof(NumericUpDown.Value), _currentGpu, nameof(_currentGpu.BaseClock));

        var productionCheckbox = new CheckBox
        {
            Location = new Point(5, 175),
            Text = "Is in active\r\nproduction",
            Height = 65,
            Width = 155,
        };
        col2.Controls.Add(productionCheckbox);

        productionCheckbox.SetBind(nameof(CheckBox.Checked), _currentGpu, nameof(_currentGpu.IsInActiveProduction));

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

        //TODO: bind this thing
        //TODO: check binding on memory
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

        var memorySizeLabel = new Label
        {
            Location = new Point(0, 130),
            Text = "Size:",
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
            MessageBox.Show(JsonSerializer.Serialize(_currentGpu));
        col3.Controls.Add(showButton);

        #endregion

        Controls.Add(col3);
        Controls.Add(col2);
        Controls.Add(col1);
    }
}