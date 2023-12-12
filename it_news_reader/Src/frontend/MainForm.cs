using System.Collections;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace it_news_reader;

public partial class MainForm : Form
{
    private ListBox entityList;
    private Panel leftPanel;
    private StatusStrip statusBar;
    private ToolStripStatusLabel timeStatusLabel;
    private System.Windows.Forms.Timer timer;
    private Dictionary<int, string> articlesOrderId;
    private FlowLayoutPanel flowLayoutPanel;
    private Rectangle Bound; 
    ToolStripMenuItem habrMenuItem;
    ToolStripMenuItem tprogerMenuItem;
    ToolStripMenuItem allMenuItem ;

    public MainForm()
    {
        //InitializeComponent();
        BackColor = Color.LightBlue;
        WindowState = FormWindowState.Maximized;
        InitializeUI();
    }

    private void InitializeUI()
    {
        Bound = Screen.FromControl(this).Bounds;
        InitializeStatusBar();
        InitializeTimer();
        InitializeLeftPanel();
        InitializeMenu();
        InitializeFlowLayotDesign();
    }
    private void InitializeFlowLayotDesign()
    {
        flowLayoutPanel = new FlowLayoutPanel
        {
            AutoScroll = true,
            WrapContents = true,
            Location = new Point(Bound.Width / 100 * 25, Bound.Height / 100 * 5),
            Size = new Size(Bound.Width / 100 * 75, Bound.Height),
        };
        Controls.Add(flowLayoutPanel);
        var query = new QueryBuilder().Select("title, link, image, text")
                                          .From("articles")
                                          .Get();
        var result = new SqliteManager().Select(query);
        foreach (DataRow row in result.Rows)
        {
            try { 
                var entity = new EntityControl
                (
                    row["title"].ToString(),
                    row["link"].ToString(),
                    row.IsNull("image") ? null : row["image"].ToString(),
                    row["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Height / 100 * 80
                );

                flowLayoutPanel.Controls.Add(entity);
            } catch { 
                continue;
            }
        }
    }
    private void InitializeMenu()
    {
        MenuStrip menuStrip = new MenuStrip();
        menuStrip.BackColor = Color.Blue;
        ToolStripMenuItem refreshMenuItem = new ToolStripMenuItem("Refresh");
        ToolStripMenuItem siteMenuItem = new ToolStripMenuItem("Site");
        ToolStripMenuItem newestMenuItem = new ToolStripMenuItem("Newest");
        ToolStripMenuItem oldestMenuItem = new ToolStripMenuItem("Oldest");
        refreshMenuItem.Click += RefreshMenuItem_Click;
        newestMenuItem.Click += NewestMenuItem_Click;
        oldestMenuItem.Click += OldestMenuItem_Click;

        habrMenuItem = new ToolStripMenuItem("Habr");
        tprogerMenuItem = new ToolStripMenuItem("Tproger");
        allMenuItem = new ToolStripMenuItem("All");

        siteMenuItem.DropDownItems.Add(habrMenuItem);
        siteMenuItem.DropDownItems.Add(tprogerMenuItem);
        siteMenuItem.DropDownItems.Add(allMenuItem);

        tprogerMenuItem.Click += Tproger_Click;
        habrMenuItem.Click += Habr_Click;
        allMenuItem.Click += RefreshMenuItem_Click;
        allMenuItem.Checked = true;

        menuStrip.Items.AddRange(new ToolStripItem[] {refreshMenuItem, siteMenuItem, newestMenuItem, oldestMenuItem });
        MainMenuStrip = menuStrip;
        Controls.Add(menuStrip);

    }
    private void Tproger_Click(object sender, EventArgs e) 
    {
        allMenuItem.Checked = false;
        habrMenuItem.Checked = false;
        tprogerMenuItem.Checked = true;
        flowLayoutPanel.Controls.Clear();
        entityList.Items.Clear();
        articlesOrderId.Clear();
        var query = new QueryBuilder().Select("title, link, image, text")
                                          .From("articles")
                                          .AndWhere(new Where("link", @"'https://tproger.ru%'", "LIKE"))
                                          .Get();
        var result = new SqliteManager().Select(query);
        List<EntityControl> flowLayoutPanelBuffer = new List<EntityControl>();
        for (int i = 0; i < result.Rows.Count; ++i)
        {
            try { 
                var entity = new EntityControl
                (
                    result.Rows[i]["title"].ToString(),
                    result.Rows[i]["link"].ToString(),
                    result.Rows[i].IsNull("image") ? null : result.Rows[i]["image"].ToString(),
                    result.Rows[i]["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Height / 100 * 80
                );
                flowLayoutPanelBuffer.Add(entity);
                entityList.Items.Add(result.Rows[i]["title"].ToString());
                articlesOrderId.Add(i, result.Rows[i]["link"].ToString());
            } catch { 
                continue;
            }
        }
        flowLayoutPanel.Controls.AddRange(flowLayoutPanelBuffer.ToArray());
    }
    private void Habr_Click(object sender, EventArgs e) 
    {
        allMenuItem.Checked = false;
        habrMenuItem.Checked = true;
        tprogerMenuItem.Checked = false;
        flowLayoutPanel.Controls.Clear();
        entityList.Items.Clear();
        articlesOrderId.Clear();
        var query = new QueryBuilder().Select("title, link, image, text")
                                          .From("articles")
                                          .AndWhere(new Where("link", @"'https://habr.com%'", "LIKE"))
                                          .Get();
        var result = new SqliteManager().Select(query);
        List<EntityControl> flowLayoutPanelBuffer = new List<EntityControl>();
        for (int i = 0; i < result.Rows.Count; ++i)
        {
            try { 
                var entity = new EntityControl
                (
                    result.Rows[i]["title"].ToString(),
                    result.Rows[i]["link"].ToString(),
                    result.Rows[i].IsNull("image") ? null : result.Rows[i]["image"].ToString(),
                    result.Rows[i]["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Height / 100 * 80
                );
                flowLayoutPanelBuffer.Add(entity);
                entityList.Items.Add(result.Rows[i]["title"].ToString());
                articlesOrderId.Add(i, result.Rows[i]["link"].ToString());
            } catch { 
                continue;
            }
        }
        flowLayoutPanel.Controls.AddRange(flowLayoutPanelBuffer.ToArray());
    }
    private void RefreshMenuItem_Click(object sender, EventArgs e) 
    {
        allMenuItem.Checked = true;
        habrMenuItem.Checked = false;
        tprogerMenuItem.Checked = false;
        flowLayoutPanel.Controls.Clear();
        articlesOrderId.Clear();
        entityList.Items.Clear();
        var query = new QueryBuilder().Select("title, link, image, text")
                                          .From("articles")
                                          .Get();
        var result = new SqliteManager().Select(query);
        List<EntityControl> flowLayoutPanelBuffer = new List<EntityControl>();
        for (int i = 0; i < result.Rows.Count; ++i)
        {
            try { 
                var entity = new EntityControl
                (
                    result.Rows[i]["title"].ToString(),
                    result.Rows[i]["link"].ToString(),
                    result.Rows[i].IsNull("image") ? null : result.Rows[i]["image"].ToString(),
                    result.Rows[i]["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Height / 100 * 80
                );
                flowLayoutPanelBuffer.Add(entity);
                entityList.Items.Add(result.Rows[i]["title"].ToString());
                articlesOrderId.Add(i, result.Rows[i]["link"].ToString());
            } catch { 
                continue;
            }
        }
        flowLayoutPanel.Controls.AddRange(flowLayoutPanelBuffer.ToArray());
    }
    private void OldestMenuItem_Click(object sender, EventArgs e) 
    {
        flowLayoutPanel.Controls.Clear();
        var query = new QueryBuilder().Select("title, link, image, text")
                                          .From("articles")
                                          .Order("date", "ASC")
                                          .Order("link", "ASC")
                                          .Get();
        var result = new SqliteManager().Select(query);
        entityList.Items.Clear();
        articlesOrderId.Clear();
        List<EntityControl> flowLayoutPanelBuffer = new List<EntityControl>();
        for (int i = 0; i < result.Rows.Count; ++i)
        {
            try { 
                var entity = new EntityControl
                (
                    result.Rows[i]["title"].ToString(),
                    result.Rows[i]["link"].ToString(),
                    result.Rows[i].IsNull("image") ? null : result.Rows[i]["image"].ToString(),
                    result.Rows[i]["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Height / 100 * 80
                );
                flowLayoutPanelBuffer.Add(entity);
                entityList.Items.Add(result.Rows[i]["title"].ToString());
                articlesOrderId.Add(i, result.Rows[i]["link"].ToString());
            } catch { 
                continue;
            }
        }
        flowLayoutPanel.Controls.AddRange(flowLayoutPanelBuffer.ToArray());
    }
    private void NewestMenuItem_Click(object sender, EventArgs e) 
    {
        flowLayoutPanel.Controls.Clear();
        var query = new QueryBuilder().Select("title, link, image, text")
                                          .From("articles")
                                          .Order("date", "DESC")
                                          .Order("link", "DESC")
                                          .Get();
        var result = new SqliteManager().Select(query);
        entityList.Items.Clear();
        articlesOrderId.Clear();
        List<EntityControl> flowLayoutPanelBuffer = new List<EntityControl>();
        for (int i = 0; i < result.Rows.Count; ++i)
        {
            try { 
                var entity = new EntityControl
                (
                    result.Rows[i]["title"].ToString(),
                    result.Rows[i]["link"].ToString(),
                    result.Rows[i].IsNull("image") ? null : result.Rows[i]["image"].ToString(),
                    result.Rows[i]["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Height / 100 * 80
                );
                flowLayoutPanelBuffer.Add(entity);
                entityList.Items.Add(result.Rows[i]["title"].ToString());
                articlesOrderId.Add(i, result.Rows[i]["link"].ToString());
            } catch { 
                continue;
            }
        }
        flowLayoutPanel.Controls.AddRange(flowLayoutPanelBuffer.ToArray());
    }
    private void InitializeLeftPanel()
    {
        var bounds = Screen.FromControl(this).Bounds;
        leftPanel = new Panel
        {
            BackColor = Color.LightBlue,
            Location = new Point(0, 30),
            Size = new Size(bounds.Width / 100 * 20, bounds.Height),
            AutoScroll = true,
            BorderStyle = BorderStyle.None,
        };
        Controls.Add(leftPanel);

        entityList = new ListBox
        {
            BackColor = Color.FromArgb(255, 133, 176, 240),
            Size = new Size(leftPanel.Width, leftPanel.Height),
        };
        var query = new QueryBuilder().Select("link, title")
                .From("articles")
                .Get();
        var result = new SqliteManager().Select(query);
        articlesOrderId = new Dictionary<int, string>();
        for(int i = 0; i < result.Rows.Count; ++i)
        {
            entityList.Items.Add(result.Rows[i]["title"]);
            articlesOrderId.Add(i, result.Rows[i]["link"].ToString());
        }
        entityList.SelectedIndexChanged += EntityList_SelectedIndexChanged;
        leftPanel.Controls.Add(entityList);
    }
    private void InitializeTimer()
    {
        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000;
        timer.Tick += Timer_Tick;
        timer.Start();
        UpdateTime();
    }
    private void InitializeStatusBar()
    {
        // StatusStrip
        statusBar = new StatusStrip
        {
            Dock = DockStyle.Bottom,
        };
        Controls.Add(statusBar);

        // ToolStripStatusLabel for time
        timeStatusLabel = new ToolStripStatusLabel
        {
            Dock = DockStyle.Right,
            Text = "TM: ",
        };
        statusBar.Items.Add(timeStatusLabel);
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        UpdateTime();
    }
    private void UpdateTime()
    {
        timeStatusLabel.Text = "TM: " + DateTime.Now.ToString("HH:mm:ss");
    }
    private void EntityList_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Handle the selection change
        int selectedIndex = entityList.SelectedIndex;

        if (selectedIndex >= 0)
        {
            var query = new QueryBuilder().Select("title, link, text, image")
                                          .From("articles")
                                          .AndWhere(new Where("link", $"'{articlesOrderId[selectedIndex]}'"))
                                          .Get();
            var result = new SqliteManager().Select(query);
            if (result.Rows.Count > 0) { 
                flowLayoutPanel.Controls.Clear();
                var entity = new EntityControl
                (
                    result.Rows[0]["title"].ToString(),
                    result.Rows[0]["link"].ToString(),
                    result.Rows[0].IsNull("image") ? null : result.Rows[0]["image"].ToString(),
                    result.Rows[0]["text"].ToString(),
                    Bound.Width / 100 * 25,
                    Bound.Width / 100 * 95,
                    Bound.Height / 100 * 10,
                    Bound.Height / 100 * 80
                );

                flowLayoutPanel.Controls.Add(entity);
              
            }
        }
    }
 }