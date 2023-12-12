using System.Diagnostics;

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
                        Title = srcTitle;
                        Link = srcLink;
                        ImageUrl = srcImage;
                        Text = srcText;
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
                        this.Size = new Size(Width, Height);
                        this.Margin = new Padding(20);

                        // Create and configure controls for the entity
                        var titleLabel = new Label
                        {
                                Text = Title,
                                AutoSize = false,
                                AutoEllipsis = true,
                                Font = new Font("Times New Roman", 17, FontStyle.Bold),
                                Width = this.Width,
                                Height = ((Title.Length * 17) / this.Width + 1) * 40,
                        };

                        var linkLabel = new LinkLabel
                        {
                                Text = Link,
                                AutoSize = true,
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
                                } catch (Exception ex)
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
                                Size = new Size(this.Width, (int)(this.Height / (1 + (int)(3.0/5.0)))),
                        };

                        // Create a TableLayoutPanel to arrange controls
                        var tableLayoutPanel = new TableLayoutPanel
                        {
                                AutoSize = true,
                                ColumnCount = 1,
                        };

                        tableLayoutPanel.Controls.Add(titleLabel);
                        tableLayoutPanel.Controls.Add(linkLabel);

                        if (!string.IsNullOrWhiteSpace(Link))
                        {
                                var pictureBox = new PictureBox
                                {
                                        Dock = DockStyle.Left,
                                        ImageLocation = ImageUrl,
                                        Size = new Size((int)(Width / 2.5), Height / 3),
                                        SizeMode = PictureBoxSizeMode.StretchImage,
                                };
                                tableLayoutPanel.Controls.Add(pictureBox);
                        }
                        tableLayoutPanel.Controls.Add(textLabel);

                        // Add the TableLayoutPanel to the UserControl
                        this.Controls.Add(tableLayoutPanel);
                }
        }

