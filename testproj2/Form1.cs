// ReSharper disable IdentifierTypo

using System.Drawing.Text;
using Timer = System.Windows.Forms.Timer;
namespace testproj2;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

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
        toolStrip.Items.Add(new ToolStripButton
        {
            Text = "One",
            TextDirection = ToolStripTextDirection.Vertical270,
        });
        toolStrip.Items.Add(new ToolStripButton
        {
            Text = "Two",
            TextDirection = ToolStripTextDirection.Vertical270,
        });
        
        

        toolStripContainer.LeftToolStripPanel.Controls.Add(toolStrip);
        Controls.Add(toolStripContainer);
        #endregion

        #region StatusStrip
        
        var statusStrip = new StatusStrip
        {
            Text = "statusStrip1",
            Dock = DockStyle.Bottom,
        };

        var toolStripStatusLabel = new ToolStripStatusLabel("Ready");
        var emptyLabel = new ToolStripStatusLabel { Spring = true };
        var timeLabel = new ToolStripStatusLabel(DateTime.Now.ToString("HH:mm:ss"));

        statusStrip.Items.Add(toolStripStatusLabel);
        statusStrip.Items.Add(emptyLabel);
        statusStrip.Items.Add(timeLabel);

        var timer = new Timer
        {
            Interval = 1000,
            Enabled = true,
        };
        timer.Tick += (_, _) => { timeLabel.Text = DateTime.Now.ToString("HH:mm:ss"); };
        
        Controls.Add(statusStrip);
        #endregion
        
        #region MenuStrip

        var menuStrip = new MenuStrip
        {
            Text = "menuStrip1",
            Dock = DockStyle.Top,
        };

        var fileToolStripMenuItem = new ToolStripMenuItem("&File");
        var editToolStripMenuItem = new ToolStripMenuItem("&Edit");

        #region FileToolStripMenuItem

        var newToolStripMenuItem = new ToolStripMenuItem
        {
            Text = "New",
            ShortcutKeys = Keys.Control | Keys.N,
            ShowShortcutKeys = true,
            Image = imageList.Images[1],
        };

        var openToolStripMenuItem = new ToolStripMenuItem
        {
            Text = "Open",
            ShortcutKeys = Keys.Control | Keys.O,
            ShowShortcutKeys = true,
            // Image = imageList.Images[0],
        };
        
        var saveToolStripMenuItem = new ToolStripMenuItem
        {
            Text = "Save",
            ShortcutKeys = Keys.Control | Keys.S,
            ShowShortcutKeys = true,
            Image = imageList.Images[0],
        };

        fileToolStripMenuItem.DropDownItems.Add(newToolStripMenuItem);
        // fileToolStripMenuItem.DropDownItems.Add("-");
        fileToolStripMenuItem.DropDownItems.Add(openToolStripMenuItem);
        fileToolStripMenuItem.DropDownItems.Add(saveToolStripMenuItem);

        #endregion

        #region EditToolStripMenuItem

        var undoToolStripMenuItem = new ToolStripMenuItem
        {
            Text = "Undo",
            ShortcutKeys = Keys.Control | Keys.Z,
            ShowShortcutKeys = true,
        };

        var redoToolStripMenuItem = new ToolStripMenuItem
        {
            Text = "Redo",
            ShortcutKeys = Keys.Control | Keys.Y,
            ShowShortcutKeys = true,
        };

        editToolStripMenuItem.DropDownItems.Add(undoToolStripMenuItem);
        editToolStripMenuItem.DropDownItems.Add(redoToolStripMenuItem);
        // editToolStripMenuItem.DropDownItems.Add("-");

        #endregion

        menuStrip.Items.Add(fileToolStripMenuItem);
        menuStrip.Items.Add(editToolStripMenuItem);

        MainMenuStrip = menuStrip;
        Controls.Add(menuStrip);

        #endregion

        #region ContextMenuStrip

        var contextMenuStrip = new ContextMenuStrip();
        
        contextMenuStrip.Items.Add(newToolStripMenuItem);
        contextMenuStrip.Items.Add(openToolStripMenuItem);
        contextMenuStrip.Items.Add(saveToolStripMenuItem);
        
        ContextMenuStrip = contextMenuStrip;

        #endregion
    }
}