// ReSharper disable IdentifierTypo
namespace testproj2;

public partial class Form1 : Form
{
    public Form1()
    {
        // Make the Form an MDI parent.
        // IsMdiContainer = true;

        // Create ToolStripPanel controls.
        var tspTop = new ToolStripPanel();
        var tspBottom = new ToolStripPanel();
        var tspLeft = new ToolStripPanel();
        var tspRight = new ToolStripPanel();

        // Dock the ToolStripPanel controls to the edges of the form.
        tspTop.Dock = DockStyle.Top;
        tspBottom.Dock = DockStyle.Bottom;
        tspLeft.Dock = DockStyle.Left;
        tspRight.Dock = DockStyle.Right;

        // Create ToolStrip controls to move among the 
        // ToolStripPanel controls.

        // Create the "Top" ToolStrip control and add
        // to the corresponding ToolStripPanel.
        var tsTop = new ToolStrip();
        tsTop.Items.Add("Top");
        tspTop.Join(tsTop);

        // Create the "Bottom" ToolStrip control and add
        // to the corresponding ToolStripPanel.
        var tsBottom = new ToolStrip();
        tsBottom.Items.Add("Bottom");
        tspBottom.Join(tsBottom);

        // Create the "Right" ToolStrip control and add
        // to the corresponding ToolStripPanel.
        var tsRight = new ToolStrip();
        tsRight.Items.Add("Right");
        tspRight.Join(tsRight);

        // Create the "Left" ToolStrip control and add
        // to the corresponding ToolStripPanel.
        var tsLeft = new ToolStrip();
        tsLeft.Items.Add("Left");
        tspLeft.Join(tsLeft);

        // Create a MenuStrip control with a new window.
        var ms = new MenuStrip();
        var windowMenu = new ToolStripMenuItem("Window");
        var windowNewMenu = new ToolStripMenuItem("New", null, windowNewMenu_Click);
        windowMenu.DropDownItems.Add(windowNewMenu);
        ((ToolStripDropDownMenu)(windowMenu.DropDown)).ShowImageMargin = false;
        ((ToolStripDropDownMenu)(windowMenu.DropDown)).ShowCheckMargin = true;

        // Assign the ToolStripMenuItem that displays 
        // the list of child forms.
        ms.MdiWindowListItem = windowMenu;

        // Add the window ToolStripMenuItem to the MenuStrip.
        ms.Items.Add(windowMenu);

        // Dock the MenuStrip to the top of the form.
        ms.Dock = DockStyle.Top;

        // The Form.MainMenuStrip property determines the merge target.
        MainMenuStrip = ms;

        // Add the ToolStripPanels to the form in reverse order.
        Controls.Add(tspRight);
        Controls.Add(tspLeft);
        Controls.Add(tspBottom);
        Controls.Add(tspTop);

        // Add the MenuStrip last.
        // This is important for correct placement in the z-order.
        Controls.Add(ms);
    }

    // This event handler is invoked when 
    // the "New" ToolStripMenuItem is clicked.
    // It creates a new Form and sets its MdiParent 
    // property to the main form.
    private void windowNewMenu_Click(object? sender, EventArgs e)
    {
        // var f = new Form();
        // // f.MdiParent = this;
        // f.Text = "Form - " + MdiChildren.Length;
        // Controls.Add(f);
        // f.Show();
        var panel = new Panel();
        panel.Controls.Add(new Label{Text = "Label " + MdiChildren.Length});
        Controls.Add(panel);
    }

}
