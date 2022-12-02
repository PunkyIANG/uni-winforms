using GPUProject.Resources;
using lab2;
using Timer = System.Windows.Forms.Timer;

namespace GPUProject.lab1v2;

public partial class MainForm : Form
{
    private readonly ToolStripStatusLabel _toolStripStatusLabel;
    private StatusStrip statusStrip;
    public MainForm()
    {
        InitializeComponent();

        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;
        
        #region ImageList

        var imageList = new ImageList
        {
            ImageSize = new Size(16, 16),
            ColorDepth = ColorDepth.Depth32Bit,
        };

        foreach (var path in Directory.GetFiles("img/"))
            imageList.Images.Add(Image.FromFile(path));

        #endregion
        
        #region ToolStrip

        // var toolStripContainer = new ToolStripContainer() /* { Dock = DockStyle.Fill }*/;

        var toolStripPanelLeft = new ToolStripPanel { Dock = DockStyle.Left };
        var toolStripPanelRight = new ToolStripPanel { Dock = DockStyle.Right };
        var toolStripPanelTop = new ToolStripPanel { Dock = DockStyle.Top };
        var toolStripPanelBottom = new ToolStripPanel { Dock = DockStyle.Bottom };

        var toolStrip = new ToolStrip();    
        
        var newToolStripButton = new ToolStripButton
        {
            Text = "New",
            TextDirection = ToolStripTextDirection.Vertical90,
        };
        newToolStripButton.Click += (_, _) => LoadMdiChild();
        
        var openToolStripButton = new ToolStripButton
        {
            Text = "Open",
            TextDirection = ToolStripTextDirection.Vertical90,
        };
        openToolStripButton.Click += (_, _) => OpenWithModal(); 
        
        var saveToolStripButton = new ToolStripButton
        {
            Text = "Save",
            TextDirection = ToolStripTextDirection.Vertical90,
        };
        saveToolStripButton.Click += (_, _) => SaveWithModal();
        
        
        toolStrip.Items.Add(newToolStripButton);
        toolStrip.Items.Add(openToolStripButton);
        toolStrip.Items.Add(saveToolStripButton);
        
        
        // toolStripContainer.RightToolStripPanel.Controls.Add(toolStrip);
        toolStripPanelRight.Join(toolStrip);
        // Controls.Add(toolStripContainer);
        
        Controls.Add(toolStripPanelLeft);
        Controls.Add(toolStripPanelRight);
        Controls.Add(toolStripPanelTop);
        Controls.Add(toolStripPanelBottom);

        #endregion
        
        #region MenuStrip

        var menuStrip = new MenuStrip
        {
            Text = "menuStrip1",
            Dock = DockStyle.Top,
        };

        var fileToolStripMenuItem = new ToolStripMenuItem("&File");
        var viewToolStripMenuItem = new ToolStripMenuItem("&View");
        var windowToolStripMenuItem = new ToolStripMenuItem("&Window");
        var helpToolStripMenuItem = new ToolStripMenuItem("&Help");

        windowToolStripMenuItem.DropDownOpening += (_, _) =>
        {
            // takes care of updating children names in the window
            if (ActiveMdiChild == null) return;
            
            var activeChild = ActiveMdiChild;

            ActivateMdiChild(null);
            ActivateMdiChild(activeChild);
        };

        menuStrip.MdiWindowListItem = windowToolStripMenuItem;
        
        {
            #region fileToolStrip

            var newToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "New",
                ShortcutKeys = Keys.Control | Keys.N,
                Image = imageList.Images[2],
            };
            newToolStripMenuItem.Click += (_, _) => LoadMdiChild();

            var openToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Open",
                ShortcutKeys = Keys.Control | Keys.O,
            };
            openToolStripMenuItem.Click += (_, _) => OpenWithModal();

            var saveToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Save",
                ShortcutKeys = Keys.Control | Keys.S,
                Image = imageList.Images[1],
            };
            saveToolStripMenuItem.Click += (_, _) => SaveWithModal(); 

            var closeToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Exit"
            };
            closeToolStripMenuItem.Click += (_, _) => Application.Exit();

            fileToolStripMenuItem.DropDownItems.Add(newToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(openToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(saveToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(closeToolStripMenuItem);
            
            #endregion

            #region viewToolStrip

            var statusBarToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Status Bar",
                Image = imageList.Images[0],
            };
            statusBarToolStripMenuItem.Click += (_, _) =>
            {
                statusStrip.Visible = !statusStrip.Visible;
                statusBarToolStripMenuItem.Image =
                    statusBarToolStripMenuItem.Image == null
                        ? imageList.Images[0]
                        : null;
            };
            
            var toolBarToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Tool Bar",
                Image = imageList.Images[0],
            };
            toolBarToolStripMenuItem.Click += (_, _) =>
            {
                toolStrip.Visible = !toolStrip.Visible;
                toolBarToolStripMenuItem.Image =
                    toolBarToolStripMenuItem.Image == null
                        ? imageList.Images[0]
                        : null;
            };

            var cascadeToolStripMenuItem = new ToolStripMenuItem("Cascade");
            cascadeToolStripMenuItem.Click += (_,_) => LayoutMdi(MdiLayout.Cascade);
            
            var verticalTileToolStripMenuItem = new ToolStripMenuItem("Vertical Tile");
            verticalTileToolStripMenuItem.Click += (_,_) => LayoutMdi(MdiLayout.TileVertical);
            
            var horizontalTileToolStripMenuItem = new ToolStripMenuItem("Horizontal Tile");
            horizontalTileToolStripMenuItem.Click += (_,_) => LayoutMdi(MdiLayout.TileHorizontal);

                
            viewToolStripMenuItem.DropDownItems.Add(statusBarToolStripMenuItem);
            viewToolStripMenuItem.DropDownItems.Add(toolBarToolStripMenuItem);
            viewToolStripMenuItem.DropDownItems.Add(cascadeToolStripMenuItem);
            viewToolStripMenuItem.DropDownItems.Add(verticalTileToolStripMenuItem);
            viewToolStripMenuItem.DropDownItems.Add(horizontalTileToolStripMenuItem);

            #endregion

            #region helpToolStrip

            var aboutToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "About",
                ShortcutKeys = Keys.Control | Keys.A
            };
            aboutToolStripMenuItem.Click += (_, _) =>
            {
                MessageBox.Show(@"GPU editor program
Created by Turcanu Cristian
Version v2.1",
                    "About GPU App");
            };
            helpToolStripMenuItem.DropDownItems.Add(aboutToolStripMenuItem);

            #endregion
        }

        menuStrip.Items.Add(fileToolStripMenuItem);
        menuStrip.Items.Add(viewToolStripMenuItem);
        menuStrip.Items.Add(windowToolStripMenuItem);
        menuStrip.Items.Add(helpToolStripMenuItem);

        MainMenuStrip = menuStrip;
        Controls.Add(menuStrip);

        #endregion

        #region StatusStrip

        statusStrip = new StatusStrip
        {
            Text = "statusStrip1",
            Dock = DockStyle.Bottom,
        };

        _toolStripStatusLabel = new ToolStripStatusLabel("Ready");
        var emptyLabel = new ToolStripStatusLabel { Spring = true };
        var timeLabel = new ToolStripStatusLabel(DateTime.Now.ToString("HH:mm:ss"));

        statusStrip.Items.Add(_toolStripStatusLabel);
        statusStrip.Items.Add(emptyLabel);
        statusStrip.Items.Add(timeLabel);

        var timeTimer = new Timer
        {
            Interval = 1000,
            Enabled = true,
        };
        timeTimer.Tick += (_, _) => timeLabel.Text = DateTime.Now.ToString("HH:mm:ss");

        Controls.Add(statusStrip);

        #endregion

        #region ContextMenuStrip

        var contextMenuStrip = new ContextMenuStrip();

        {
            var newToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "New",
                ShortcutKeys = Keys.Control | Keys.N,
                ShowShortcutKeys = true,
                Image = imageList.Images[2],
            };
            newToolStripMenuItem.Click += (_, _) => LoadMdiChild();

            var openToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Open",
                ShortcutKeys = Keys.Control | Keys.O,
                ShowShortcutKeys = true,
                // Image = imageList.Images[0],
            };
            openToolStripMenuItem.Click += (_, _) => OpenWithModal();

            var saveToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Save",
                ShortcutKeys = Keys.Control | Keys.S,
                ShowShortcutKeys = true,
                Image = imageList.Images[1],
            };
            saveToolStripMenuItem.Click += (_, _) => SaveWithModal();


            contextMenuStrip.Items.Add(newToolStripMenuItem);
            contextMenuStrip.Items.Add(openToolStripMenuItem);
            contextMenuStrip.Items.Add(saveToolStripMenuItem);
        }        
        ContextMenuStrip = contextMenuStrip;

        #endregion

    }

    void LoadMdiChild(GraphicsCard? gpu = null)
    {
        var newMdiChild = new GpuForm(gpu);
        newMdiChild.MdiParent = this;
        newMdiChild.Show();
    }
    
    void OpenWithModal()
    {
        var openFileDialog = new OpenFileDialog()
        {
            Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Open GPU file",
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != "")
        {
            if (GraphicsCard.TryReadGPU(openFileDialog.FileName, out var newGpu))
            {
                LoadMdiChild(newGpu);
                _toolStripStatusLabel.Text = $"Successfully loaded {openFileDialog.FileName}";
            }
            else
            {
                _toolStripStatusLabel.Text = "Load failed, probably missing file";
            }
        }
    }
    
    void SaveWithModal()
    {
        if (ActiveMdiChild is not GpuForm child)
            return;
        
        var selectedGpu = child.CurrentGpu;
        
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Save GPU File",
            FileName = selectedGpu.Model,
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
        {
            GraphicsCard.WriteGPU(saveFileDialog.FileName, selectedGpu);
            _toolStripStatusLabel.Text = $"Saved to {saveFileDialog.FileName}";

        }
    }
}