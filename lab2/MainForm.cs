using GPUProject.Resources;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Timer = System.Windows.Forms.Timer;

namespace GPUProject.lab1v2;

public partial class MainForm : Form
{
    ListView listView;
    ListBox gpuList;
    TextBox saveLoadTextBox;

    ComboBox manufacturerDropDown;
    TextBox modelTextBox;
    Button[] outputButtons;
    ListBox outputList;
    CheckBox[] resolutionCheckBoxes;

    NumericUpDown priceNumericUpDown;
    NumericUpDown baseClockNumericUpDown;

    ComboBox memoryTypeDropDown;

    RadioButton[] radioButtons;

    Label memorySizeLabel;
    TrackBar memorySizeTrackBar;
    CheckBox productionCheckbox;

    ToolStripStatusLabel toolStripStatusLabel;

    BindingList<GraphicsCard> modelData;
    GraphicsCard selectedGpu;
    int timerTicks;

    readonly MemoryManufacturer[] memValues =
    {
        MemoryManufacturer.SKHynix,
        MemoryManufacturer.Micron,
        MemoryManufacturer.Samsung
    };


    #region memory middleman

    public MemoryManufacturer memoryManufacturer
    {
        get { return selectedGpu.Memory.manufacturer; }
        set
        {
            if (value != selectedGpu.Memory.manufacturer)
                selectedGpu.Memory = new Memory
                {
                    manufacturer = value,
                    type = selectedGpu.Memory.type,
                    size = selectedGpu.Memory.size,
                };
        }
    }

    public MemoryType memoryType
    {
        get { return selectedGpu.Memory.type; }
        set
        {
            if (value != selectedGpu.Memory.type)
                selectedGpu.Memory = new Memory
                {
                    manufacturer = selectedGpu.Memory.manufacturer,
                    type = value,
                    size = selectedGpu.Memory.size,
                };
        }
    }

    public byte memorySize
    {
        get { return selectedGpu.Memory.size; }
        set
        {
            if (value != selectedGpu.Memory.size)
                selectedGpu.Memory = new Memory
                {
                    manufacturer = selectedGpu.Memory.manufacturer,
                    type = selectedGpu.Memory.type,
                    size = value,
                };
        }
    }

    #endregion


    public MainForm()
    {
        InitializeComponent();
        Height = 600;

        Text = "GPU App v2";

        var mainContainer = new Panel
        {
            Location = new Point(0, 30),
            Dock = DockStyle.Fill,
        };

        #region col1

        modelData = new BindingList<GraphicsCard>(GraphicsCard.GenerateGraphicsCards(10).ToList());
        selectedGpu = modelData.First();

        gpuList = new ListBox
        {
            Height = 330,
            DataSource = modelData,
            DisplayMember = "Model",
        };

        gpuList.MouseDoubleClick += GPUList_MouseDoubleClick;
        gpuList.MouseUp += GPUList_MouseClick;

        gpuList.SelectedValueChanged += GPUList_SelectedValueChanged;

        // Controls.Add(gpuList);

        var listViewLabel = new Label
        {
            Location = new Point(10, 0),
            Text = "List of GPUs",
        };
        mainContainer.Controls.Add(listViewLabel);

        listView = new ListView
        {
            Location = new Point(10, 25),
            Height = 305,
            Width = 110,
            View = View.SmallIcon,
            // Width = 120,
            // Alignment = ListViewAlignment.Top,
            MultiSelect = false,
        };

        foreach (var item in modelData)
        {
            listView.Items.Add(new ListViewItem(item.Model));
        }

        listView.ItemSelectionChanged += ListView_SelectedValueChanged;
        listView.MouseDoubleClick += ListView_MouseDoubleClick;
        listView.MouseClick += ListView_MouseClick;


        mainContainer.Controls.Add(listView);

        //

        saveLoadTextBox = new TextBox
        {
            Location = new Point(10, 330),
            Width = 110,
            PlaceholderText = "File to save/load",
        };

        mainContainer.Controls.Add(saveLoadTextBox);

        var saveButton = new Button
        {
            Location = new Point(10, 360),
            Width = 55,
            Height = 30,
            Text = "Save",
        };

        saveButton.Click += (sender, e) => SaveGpu();

        mainContainer.Controls.Add(saveButton);

        var loadButton = new Button
        {
            Location = new Point(65, 360),
            Width = 55,
            Height = 30,
            Text = "Load",
        };

        loadButton.Click += (sender, e) => LoadGpu();

        mainContainer.Controls.Add(loadButton);

        Controls.Add(mainContainer);

        #endregion

        #region col2

        var col2Panel = new Panel
        {
            Location = new Point(130, 0),
            Width = 130,
            Height = 400,
            BorderStyle = BorderStyle.Fixed3D
        };

        mainContainer.Controls.Add(col2Panel);

        manufacturerDropDown = new ComboBox
        {
            Location = new Point(0, 0),
            DataSource = Enum.GetValues<Manufacturer>(),
        };

        col2Panel.Controls.Add(manufacturerDropDown);


        modelTextBox = new TextBox
        {
            Location = new Point(0, 35),
            Width = 120,
            PlaceholderText = "Model",
        };

        modelTextBox.TextChanged += ModelTextBox_TextChanged;

        col2Panel.Controls.Add(modelTextBox);


        outputButtons = new Button[]
        {
            new Button
            {
                Text = "VGA",
                Location = new Point(0, 70),
                Width = 60,
                Height = 30,
            },
            new Button
            {
                Text = "DVI",
                Location = new Point(60, 70),
                Width = 60,
                Height = 30,
            },
            new Button
            {
                Text = "HDMI",
                Location = new Point(0, 105),
                Width = 60,
                Height = 30,
            },
            new Button
            {
                Text = "DP",
                Location = new Point(60, 105),
                Width = 60,
                Height = 30,
            },
        };

        outputButtons[0].Click += (sender, e) => AddOutputType(OutputType.VGA);
        outputButtons[1].Click += (sender, e) => AddOutputType(OutputType.DVI);
        outputButtons[2].Click += (sender, e) => AddOutputType(OutputType.HDMI);
        outputButtons[3].Click += (sender, e) => AddOutputType(OutputType.DisplayPort);

        col2Panel.Controls.AddRange(outputButtons);

        outputList = new ListBox
        {
            Location = new Point(0, 140),
            Height = 110,
            DataSource = selectedGpu.OutputTypes,
        };

        col2Panel.Controls.Add(outputList);

        resolutionCheckBoxes = new CheckBox[]
        {
            new CheckBox
            {
                Location = new Point(0, 250),
                Text = "Full HD",
            },
            new CheckBox
            {
                Location = new Point(0, 275),
                Text = "1440p",
            },
            new CheckBox
            {
                Location = new Point(0, 300),
                Text = "4K",
            },
        };

        col2Panel.Controls.AddRange(resolutionCheckBoxes);

        var priceLabel = new Label
        {
            Location = new Point(0, 326),
            Text = "Price:",
            Width = 45,
        };
        col2Panel.Controls.Add(priceLabel);

        priceNumericUpDown = new NumericUpDown
        {
            Location = new Point(50, 325),
            Maximum = 2000,
            Width = 71,
        };
        col2Panel.Controls.Add(priceNumericUpDown);


        var baseClockLabel = new Label
        {
            Location = new Point(0, 361),
            Text = "Clock:",
            Width = 50,
        };
        col2Panel.Controls.Add(baseClockLabel);

        baseClockNumericUpDown = new NumericUpDown
        {
            Location = new Point(50, 360),
            Maximum = 3000,
            Width = 71,
        };
        col2Panel.Controls.Add(baseClockNumericUpDown);

        #endregion

        #region col3

        var memoryGroupBox = new GroupBox
        {
            Location = new Point(260, 0),
            Width = 140,
            Height = 220,
            Text = "Memory"
        };

        memoryTypeDropDown = new ComboBox
        {
            Location = new Point(10, 20),
            DataSource = Enum.GetValues<MemoryType>(),
            DisplayMember = "MemoryType",
            ValueMember = "MemoryType",
        };
        memoryGroupBox.Controls.Add(memoryTypeDropDown);

        // memoryManufacturerDropDown = new ComboBox
        // {
        //     Location = new Point(10, 55),
        //     DataSource = Enum.GetValues<MemoryManufacturer>(),
        // };
        // memoryGroupBox.Controls.Add(memoryManufacturerDropDown);


        radioButtons = new RadioButton[]
        {
            new RadioButton
            {
                Location = new Point(10, 55),
                Text = "SKHynix",
            },
            new RadioButton
            {
                Location = new Point(10, 80),
                Text = "Micron",
            },
            new RadioButton
            {
                Location = new Point(10, 105),
                Text = "Samsung",
            }
        };

        foreach (var radioButton in radioButtons)
            radioButton.CheckedChanged += RadioButton_CheckedChanged;

        memoryGroupBox.Controls.AddRange(radioButtons);


        memorySizeLabel = new Label
        {
            Location = new Point(10, 140),
            // Text = "Size:",
            Width = 120,
        };
        memoryGroupBox.Controls.Add(memorySizeLabel);

        // memorySizeNumericUpDown = new NumericUpDown
        // {
        //     Location = new Point(60, 90),
        //     Maximum = 255,
        //     Width = 71,
        // };
        // memoryGroupBox.Controls.Add(memorySizeNumericUpDown);

        memorySizeTrackBar = new TrackBar
        {
            Location = new Point(10, 170),
            Size = new Size(130, 45),
            Maximum = 16,
            Minimum = 1,
            TickFrequency = 1,
            LargeChange = 1,
            SmallChange = 1,
        };
        memorySizeTrackBar.ValueChanged += (sender, e) => TrackBarValueChanged();

        memoryGroupBox.Controls.Add(memorySizeTrackBar);


        productionCheckbox = new CheckBox
        {
            Location = new Point(260, 225),
            Text = "Is in active\r\nproduction",
            Height = 50,
        };
        mainContainer.Controls.Add(productionCheckbox);


        var showButtonToolTip = new ToolTip();
        var showButton = new Button
        {
            Location = new Point(260, 275),
            Height = 35,
            Text = "show",
        };
        showButton.Click += ShowCurrentValue;
        showButtonToolTip.SetToolTip(showButton, "Show an alert with the json data of this gpu");
        mainContainer.Controls.Add(showButton);

        mainContainer.Controls.Add(memoryGroupBox);


        var timerLabel = new Label
        {
            Location = new Point(260, 350),
            Width = 130,
            Height = 30,
            Text = "timer text"
        };
        mainContainer.Controls.Add(timerLabel);

        var timer = new System.Windows.Forms.Timer
        {
            Interval = 300,
        };

        var progressBar = new ProgressBar
        {
            Location = new Point(260, 380),
            Width = 120,
            Height = 30,
            Maximum = 100,
        };
        mainContainer.Controls.Add(progressBar);


        timer.Tick += (sender, e) =>
        {
            timerTicks++;
            timerTicks %= progressBar.Maximum + 1;
            timerLabel.Text = $"{timerTicks} ticks";
            progressBar.Value = timerTicks;
        };

        var timerButton = new Button
        {
            Location = new Point(260, 320),
            Width = 130,
            Height = 30,
            Text = "Start/stop timer",
        };
        timerButton.Click += (sender, e) => { timer.Enabled = !timer.Enabled; };
        mainContainer.Controls.Add(timerButton);

        #endregion


        #region ImageList

        var imageList = new ImageList
        {
            ImageSize = new Size(16, 16),
            ColorDepth = ColorDepth.Depth32Bit,
        };

        foreach (var path in Directory.GetFiles("../../../img/"))
            imageList.Images.Add(Image.FromFile(path));

        #endregion

        #region ToolStrip

        var toolStripContainer = new ToolStripContainer { Dock = DockStyle.Fill };

        var toolStrip = new ToolStrip();
        
        var newToolStripButton = new ToolStripButton
        {
            Text = "New",
            TextDirection = ToolStripTextDirection.Vertical90,
        };
        newToolStripButton.Click += (_, _) => { AddGpu(); };
        
        var openToolStripButton = new ToolStripButton
        {
            Text = "Open",
            TextDirection = ToolStripTextDirection.Vertical90,
        };
        openToolStripButton.Click += (_, _) => { OpenWithModal(); };
        
        var saveToolStripButton = new ToolStripButton
        {
            Text = "Save",
            TextDirection = ToolStripTextDirection.Vertical90,
        };
        saveToolStripButton.Click += (_, _) => { SaveWithModal(); };
        
        
        toolStrip.Items.Add(newToolStripButton);
        toolStrip.Items.Add(openToolStripButton);
        toolStrip.Items.Add(saveToolStripButton);
        

        toolStripContainer.RightToolStripPanel.Controls.Add(toolStrip);
        mainContainer.Controls.Add(toolStripContainer);

        #endregion

        #region StatusStrip

        var statusStrip = new StatusStrip
        {
            Text = "statusStrip1",
            Dock = DockStyle.Bottom,
        };

        toolStripStatusLabel = new ToolStripStatusLabel("Ready");
        var emptyLabel = new ToolStripStatusLabel { Spring = true };
        var timeLabel = new ToolStripStatusLabel(DateTime.Now.ToString("HH:mm:ss"));

        statusStrip.Items.Add(toolStripStatusLabel);
        statusStrip.Items.Add(emptyLabel);
        statusStrip.Items.Add(timeLabel);

        var timeTimer = new Timer
        {
            Interval = 1000,
            Enabled = true,
        };
        timeTimer.Tick += (_, _) => { timeLabel.Text = DateTime.Now.ToString("HH:mm:ss"); };

        mainContainer.Controls.Add(statusStrip);

        #endregion

        #region MenuStrip

        var menuStrip = new MenuStrip
        {
            Text = "menuStrip1",
            Dock = DockStyle.Top,
        };

        var fileToolStripMenuItem = new ToolStripMenuItem("&File");
        // var editToolStripMenuItem = new ToolStripMenuItem("&Edit");

        #region FileToolStripMenuItem

        var newToolStripMenuItem = new ToolStripMenuItem
        {
            Text = "New",
            ShortcutKeys = Keys.Control | Keys.N,
            ShowShortcutKeys = true,
            Image = imageList.Images[1],
        };
        newToolStripMenuItem.Click += (_, _) => { AddGpu(); };

        var openToolStripMenuItem = new ToolStripMenuItem
        {
            Text = "Open",
            ShortcutKeys = Keys.Control | Keys.O,
            ShowShortcutKeys = true,
            // Image = imageList.Images[0],
        };
        openToolStripMenuItem.Click += (_, _) => { OpenWithModal(); };

        var saveToolStripMenuItem = new ToolStripMenuItem
        {
            Text = "Save",
            ShortcutKeys = Keys.Control | Keys.S,
            ShowShortcutKeys = true,
            Image = imageList.Images[0],
        };
        saveToolStripMenuItem.Click += (_, _) => { SaveWithModal(); };

        fileToolStripMenuItem.DropDownItems.Add(newToolStripMenuItem);
        // fileToolStripMenuItem.DropDownItems.Add("-");
        fileToolStripMenuItem.DropDownItems.Add(openToolStripMenuItem);
        fileToolStripMenuItem.DropDownItems.Add(saveToolStripMenuItem);

        #endregion

        // #region EditToolStripMenuItem
        //
        // var undoToolStripMenuItem = new ToolStripMenuItem
        // {
        //     Text = "Undo",
        //     ShortcutKeys = Keys.Control | Keys.Z,
        //     ShowShortcutKeys = true,
        // };
        //
        // var redoToolStripMenuItem = new ToolStripMenuItem
        // {
        //     Text = "Redo",
        //     ShortcutKeys = Keys.Control | Keys.Y,
        //     ShowShortcutKeys = true,
        // };
        //
        // editToolStripMenuItem.DropDownItems.Add(undoToolStripMenuItem);
        // editToolStripMenuItem.DropDownItems.Add(redoToolStripMenuItem);
        // // editToolStripMenuItem.DropDownItems.Add("-");
        //
        // #endregion

        menuStrip.Items.Add(fileToolStripMenuItem);
        // menuStrip.Items.Add(editToolStripMenuItem);

        MainMenuStrip = menuStrip;
        Controls.Add(menuStrip);

        #endregion


        ResetDataBindings();
    }

    void GPUList_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (gpuList.SelectedIndex != ListBox.NoMatches)
            modelData.RemoveAt(gpuList.SelectedIndex);

        if (modelData.Count == 0)
            modelData.Add(new GraphicsCard());

        selectedGpu = modelData[0];
        ResetDataBindings();
    }

    void GPUList_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) AddGpu();
    }

    void GPUList_SelectedValueChanged(object? sender, EventArgs e)
    {
        if (gpuList.SelectedIndex != ListBox.NoMatches)
            selectedGpu = (GraphicsCard)gpuList.SelectedValue;

        ResetDataBindings();
    }

    void ListView_SelectedValueChanged(object? sender, EventArgs e)
    {
        if (listView.SelectedIndices.Count != 0)
            selectedGpu = modelData[listView.SelectedIndices[0]];

        ResetDataBindings();
    }

    void ListView_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (listView.SelectedIndices.Count != 0)
        {
            modelData.RemoveAt(listView.SelectedIndices[0]);
            listView.Items.RemoveAt(listView.SelectedIndices[0]);
        }

        if (modelData.Count == 0)
        {
            modelData.Add(new GraphicsCard());
            listView.Items.Add(modelData[0].Model);
            selectedGpu = modelData[0];
        }


        ResetDataBindings();
    }

    void ListView_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) AddGpu();
    }

    void AddGpu()
    {
        var item = modelData.AddNew();
        listView.Items.Add(item.Model);
    }

    void ShowCurrentValue(object? sender, EventArgs e)
    {
        MessageBox.Show(JsonSerializer.Serialize<GraphicsCard>(selectedGpu));
    }

    void AddOutputType(OutputType outputType)
    {
        selectedGpu.OutputTypes.Add(outputType);
    }

    void TrackBarValueChanged()
    {
        memorySizeLabel.Text = $"Size: {memorySizeTrackBar.Value} GB";
    }

    void ModelTextBox_TextChanged(object? sender, EventArgs e)
    {
        if (listView.SelectedItems.Count != 0)
            listView.SelectedItems[0].Text = modelTextBox.Text;
        else
            Console.WriteLine("WARNING: editing an item without a set index!");
    }

    void RadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        MemoryManufacturer[] values =
        {
            MemoryManufacturer.SKHynix,
            MemoryManufacturer.Micron,
            MemoryManufacturer.Samsung
        };

        for (int i = 0; i < radioButtons.Length; i++)
            if (radioButtons[i].Checked)
            {
                memoryManufacturer = values[i];
                return;
            }
    }

    void SaveWithModal()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Save GPU File",
            FileName = selectedGpu.Model,
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
            SaveGpu(saveFileDialog.FileName);
    }

    void OpenWithModal()
    {
        var openFileDialog = new OpenFileDialog()
        {
            Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Open GPU file",
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != "")
            LoadGpu(openFileDialog.FileName);
    }

    void SaveGpu(string filename = "")
    {
        if (filename == string.Empty)
            filename = saveLoadTextBox.Text != string.Empty
                ? saveLoadTextBox.Text
                : $"{selectedGpu.Model}.json";

        GraphicsCard.WriteGPU(filename, selectedGpu);
        toolStripStatusLabel.Text = $"Saved to {filename}";
    }

    void LoadGpu(string filename = "")
    {
        if (filename == string.Empty)
            filename = saveLoadTextBox.Text;
        
        if (filename == string.Empty)
            toolStripStatusLabel.Text = "Specify a filename first!";

        if (GraphicsCard.TryReadGPU(filename, out var newGpu))
        {
            modelData.Add(newGpu);
            listView.Items.Add(newGpu.Model);

            gpuList.SelectedIndex = gpuList.Items.Count - 1;

            selectedGpu = modelData.Last();
            ResetDataBindings();
            toolStripStatusLabel.Text = $"Successfully loaded {filename}";
        }
        else
        {
            toolStripStatusLabel.Text = "Load failed, probably missing file";
        }
    }

    void ResetDataBindings()
    {
        manufacturerDropDown.ResetBind(
            nameof(ComboBox.SelectedItem),
            selectedGpu,
            nameof(selectedGpu.Manufacturer)
        );

        modelTextBox.ResetBind(
            nameof(TextBox.Text),
            selectedGpu,
            nameof(selectedGpu.Model)
        );

        outputList.DataSource = selectedGpu.OutputTypes;

        resolutionCheckBoxes[0].ResetBind(
            nameof(CheckBox.Checked),
            selectedGpu.RecommendedResolutions,
            nameof(selectedGpu.RecommendedResolutions.FullHD)
        );

        resolutionCheckBoxes[1].ResetBind(
            nameof(CheckBox.Checked),
            selectedGpu.RecommendedResolutions,
            nameof(selectedGpu.RecommendedResolutions.TwoK)
        );

        resolutionCheckBoxes[2].ResetBind(
            nameof(CheckBox.Checked),
            selectedGpu.RecommendedResolutions,
            nameof(selectedGpu.RecommendedResolutions.FourK)
        );

        priceNumericUpDown.ResetBind(
            nameof(NumericUpDown.Value),
            selectedGpu,
            nameof(selectedGpu.Price)
        );

        baseClockNumericUpDown.ResetBind(
            nameof(NumericUpDown.Value),
            selectedGpu,
            nameof(selectedGpu.BaseClock)
        );


        memoryTypeDropDown.ResetBind(
            nameof(ComboBox.SelectedItem),
            this,
            nameof(this.memoryType)
        );

        for (int i = 0; i < memValues.Length; i++)
            if (selectedGpu.Memory.manufacturer == memValues[i])
            {
                radioButtons[i].Checked = true;
                break;
            }


        // memoryManufacturerDropDown.ResetBind(
        //     nameof(ComboBox.SelectedItem),
        //     this,
        //     nameof(this.memoryManufacturer)
        // );

        // memorySizeNumericUpDown.ResetBind(
        //     nameof(NumericUpDown.Value),
        //     this,
        //     nameof(this.memorySize)
        // );

        memorySizeTrackBar.ResetBind(
            nameof(TrackBar.Value),
            this,
            nameof(this.memorySize)
        );

        TrackBarValueChanged();

        productionCheckbox.ResetBind(
            nameof(CheckBox.Checked),
            selectedGpu,
            nameof(selectedGpu.IsInActiveProduction)
        );
    }
}