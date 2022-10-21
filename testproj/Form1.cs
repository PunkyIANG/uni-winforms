using System.Drawing;
using System.Windows.Forms;

namespace winforms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        var b = new Button();
        b.Text = "You click me, you become idiot";
        // b.AutoSize = true;
        b.Click += static (sender, _) =>
        {
            var b = (Button) sender!;
            b.Text = "You are an idiot";
        };
        b.Size = new Size(200, 80);
        b.Padding = new Padding(10);
        b.TextAlign = ContentAlignment.TopLeft;
        Controls.Add(b);
    }
}
