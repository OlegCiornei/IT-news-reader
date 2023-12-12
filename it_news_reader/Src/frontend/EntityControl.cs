using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace it_news_reader;

public class EntityControl : UserControl
{
        public string Title { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public string Text { get; set; }
        public int Button { get; set; }
        public int Right { get; set; }
        public int Left { get; set; }
        public EntityControl(string srcTitle, string srcLink, string srcImage, string srcText, int srcLeft, int srcRight, int srcTop, int srcButton)
        {
                BackColor = Color.FromArgb(255, 133, 176, 240);
                Title = "   " + srcTitle;
                Link = srcLink;
                ImageUrl = srcImage;
                Text = "   " + srcText;
                Left = srcLeft;
                Right = srcRight;
                Top = srcTop;
                Button = srcButton;
                InitializeUI();
        }

        private void InitializeUI()
        {
                Width = Right - Left;
                Height = Button - Top;
                this.Padding = new Padding(20);

                var titleLabel = new Label
                {
                        Text = Title,
                        AutoSize = false,
                        AutoEllipsis = true,
                        Font = new Font("Times New Roman", 17, FontStyle.Bold),
                        Width = this.Width,
                        Height = ((Title.Length * 17) / this.Width + 1) * 40,
                };

                var linkLabel = new Button
                {
                        Text = "Link",
                        Location = new Point(50, 0),
                        Size = new Size(100, 50),
                        BackColor = Color.Blue,
                        ForeColor = Color.White,
                        TextAlign = ContentAlignment.MiddleCenter,
                };
                linkLabel.Click += (sender, e) =>
                {
                        try
                        {
                                Process.Start(new ProcessStartInfo
                                {
                                        FileName = Link,
                                        UseShellExecute = true
                                });
                        }
                        catch (Exception ex)
                        {

                        }
                };

                var textLabel = new Label
                {
                        Dock = DockStyle.Left,
                        AutoSize = false,
                        AutoEllipsis = true,
                        Font = new Font("Times New Roman", 12),
                        Text = Text,
                        Size = new Size(this.Width, this.Height / 100 * 40),
                };
                textLabel.Height = textLabel.Text.Length * 20 * 14 / textLabel.Width;
                /*
                if (textLabel.Text.Length < (textLabel.Height * textLabel.Width) / (14 * 12))
                {
                        textLabel.Height = (textLabel.Height * textLabel.Width) / (12 * textLabel.Text.Length + 1);
                }*/
                var tableLayoutPanel = new TableLayoutPanel
                {
                        AutoSize = true,
                        ColumnCount = 1,
                };

                tableLayoutPanel.Controls.Add(titleLabel);

                int picture_height = 0;
                if (!string.IsNullOrWhiteSpace(ImageUrl))
                {
                        PictureBox pictureBox = new PictureBox
                        {
                                Dock = DockStyle.Left,
                                ImageLocation = ImageUrl,
                                Size = new Size((int)(Width / 2.5), Height / 100 * 40),
                                SizeMode = PictureBoxSizeMode.StretchImage,
                        };
                        picture_height = Height / 100 * 40;
                        tableLayoutPanel.Controls.Add(pictureBox);
                }
                tableLayoutPanel.Controls.Add(textLabel);
                tableLayoutPanel.Controls.Add(linkLabel);
                Size = new Size(Width, titleLabel.Height + 2*linkLabel.Height + textLabel.Height + picture_height);
                this.Controls.Add(tableLayoutPanel);
        }
}
