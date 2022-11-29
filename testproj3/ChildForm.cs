using System.Windows.Forms;

namespace testproj3;

public partial class ChildForm : Form
{
    public ChildForm()
    {
        InitializeComponent();

        var textBox = new RichTextBox
        {
            Anchor = AnchorStyles.Top,
            Dock = DockStyle.Fill,
        };
        
        Controls.Add(textBox);
    }
}