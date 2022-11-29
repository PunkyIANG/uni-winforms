using GPUProject.Resources;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Accessibility;
using lab2;
using Timer = System.Windows.Forms.Timer;

namespace GPUProject.lab1v2;

public partial class MainForm : Form
{
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
            newToolStripMenuItem.Click += (_, _) =>
            {
                var newMdiChild = new GpuForm();
                newMdiChild.MdiParent = this;
                newMdiChild.Show();
            };
            var openToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Open",
                ShortcutKeys = Keys.Control | Keys.O,
            };
            // openToolStripMenuItem.Click += (_, _) => OpenWithModal();

            var saveToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Save",
                ShortcutKeys = Keys.Control | Keys.S,
                Image = imageList.Images[1],
            };
            // saveToolStripMenuItem.Click += (_, _) => SaveWithModal(); 

            var closeToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Exit"
            };
            closeToolStripMenuItem.Click += (_, _) => Application.Exit();

            fileToolStripMenuItem.DropDownItems.Add(newToolStripMenuItem);
            // fileToolStripMenuItem.DropDownItems.Add("-");
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
            // statusBarToolStripMenuItem.Click += (_, _) =>
            // {
            //     statusStrip.Visible = !statusStrip.Visible;
            //     statusBarToolStripMenuItem.Image =
            //         statusBarToolStripMenuItem.Image == null
            //             ? imageList.Images[0]
            //             : null;
            // };
            
            var toolBarToolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Tool Bar",
                Image = imageList.Images[0],
            };
            // toolBarToolStripMenuItem.Click += (_, _) =>
            // {
            //     toolStrip.Visible = !toolStrip.Visible;
            //     toolBarToolStripMenuItem.Image =
            //         toolBarToolStripMenuItem.Image == null
            //             ? imageList.Images[0]
            //             : null;
            // };

            viewToolStripMenuItem.DropDownItems.Add(statusBarToolStripMenuItem);
            viewToolStripMenuItem.DropDownItems.Add(toolBarToolStripMenuItem);
            
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

    }
}